#if XNATOUCH
using XnaTouch.Framework;
using XnaTouch.Framework.Audio;
using XnaTouch.Framework.Content;
using XnaTouch.Framework.GamerServices;
using XnaTouch.Framework.Graphics;
using XnaTouch.Framework.Input;
using XnaTouch.Framework.Media;
using XnaTouch.Framework.Net;
using XnaTouch.Framework.Storage;
#else
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


#endif

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using IceCream.Debug;

namespace IceCream.Input
{
    public delegate void InputCommandFiredHandler(InputCommand command);
    

    public static class InputCore
    {
        public static bool DisableInput;
#if !XNATOUCH
        public static GamePadDeadZone GamePadDeadZone { get; set; }
#endif

        #region Events
        public static event InputCommandFiredHandler InputCommandFired;
        #endregion
         
        #region Fields
        private static List<InputCommand> _commands = new List<InputCommand>();

        private static bool[] _padsConnected;

        private static GamePadState[] _currentPadState;
        private static GamePadState[] _lastPadState;

        #if WINDOWS
        private static MouseState _currentMouseState;
        private static MouseState _lastMouseState;
        #endif
        
        private static KeyboardState[] _currentKBState;
        private static KeyboardState[] _lastKBState;
        public static GamePadState[] CurrentPadState
        {
            get { return _currentPadState; }
        }
#if(WINDOWS)
        public static MouseState CurrentMouseState { get { return _currentMouseState; } }
        public static MouseState LastMouseState{ get { return _lastMouseState; } }

#endif
        #endregion

        #region Constructor
        static InputCore()
        {
            _currentKBState = new KeyboardState[4];
            _lastKBState = new KeyboardState[4];
            _currentPadState = new GamePadState[4];
            _lastPadState = new GamePadState[4];
            _padsConnected = new bool[4];
            _padsConnected[0] = false;
            _padsConnected[1] = false;
            _padsConnected[2] = false;
            _padsConnected[3] = false;
#if !XNATOUCH
            InputCore.GamePadDeadZone = GamePadDeadZone.IndependentAxes;
#endif
		}
        #endregion

        #region Methods

        internal static void Update(float elapsed)
        {
            if (DisableInput)
            {
                return;
            }

            IceProfiler.StartProfiling(IceProfilerNames.ICE_CORE_INPUT_UPDATE);

            //Get all 4 pads and keyboard states
            for (int i = 0; i < 4; i++)
            {
				#if !REACH
                _lastPadState[i] = _currentPadState[i];
                _currentPadState[i] = GamePad.GetState((PlayerIndex)i, InputCore.GamePadDeadZone);
                _padsConnected[i] = _currentPadState[i].IsConnected;

                _lastKBState[i] = _currentKBState[i];
                _currentKBState[i] = Keyboard.GetState((PlayerIndex)i);
				#endif
            }

            #if(WINDOWS)
            _lastMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            #endif

            CheckCommands();

            IceProfiler.StopProfiling(IceProfilerNames.ICE_CORE_INPUT_UPDATE);
        }

        /// <summary>
        /// Polls all 4 controllers for a button press of "START" or "A"
        /// </summary>
        /// <returns>The 0-based controller index if one controller pressed START or A, else returns null</returns>
        public static int? PollControllersForActiveOne()
        {
            for (int i = 0; i < 4; i++)
            {
                if (_currentPadState[i].IsConnected == true && (
                    IsNewButtonPress((PlayerIndex)i, Buttons.Start) || IsNewButtonPress((PlayerIndex)i, Buttons.A)))
                {
                    return i;
                }
            }
            return null;
        }


        private static void CheckCommands()
        {            
            for (int i = 0; i < _commands.Count; i++)
            {
                if (i >= _commands.Count)
                    break;

                InputCommand _command = _commands[i];
                if (_command.keys != null)
                {
                    foreach (var item in _command.keys)
                    {
                        if (_command.AnyPlayer)
                        {
                            if (IsNewKeyPress(item))
                            {
                                FireCommand(_command);
                            }
                        }
                        else
                            if (IsNewKeyPress(_command.PlayerIndex,item))
                                FireCommand(_command);
                    }
                }
                if (_command.buttons != null)
                {
                    foreach (var item in _command.buttons)
                    {
                        if (_command.AnyPlayer)
                        {
                            if (IsNewButtonPress(item,ref _command.PlayerIndex))
                            {
                                FireCommand(_command);
                            }
                        }
                        else
                            if (IsNewButtonPress(_command.PlayerIndex, item))
                                FireCommand(_command);
                    }
                }
            #if(WINDOWS)
                if (_command.MouseButton != null)
                {
                    if (_command.MouseButton == System.Windows.Forms.MouseButtons.Left)
                    {
                        if (_currentMouseState.LeftButton == ButtonState.Released &&
                            _lastMouseState.LeftButton == ButtonState.Pressed)
                        {
                            FireCommand(_command);
                        }
                    }
                    if (_command.MouseButton == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (_currentMouseState.RightButton== ButtonState.Released &&
                            _lastMouseState.RightButton == ButtonState.Pressed)
                        {
                            FireCommand(_command);
                        }
                    }
                }
            #endif
            }
        }

        private static void FireCommand(InputCommand _commanad)
        {
            if (InputCommandFired != null)
                InputCommandFired(_commanad);
        }

        public static bool IsNewButtonPress(PlayerIndex playerIndex, Buttons button)
        {
            return _lastPadState[(int)playerIndex].IsButtonUp(button) &&
                _currentPadState[(int)playerIndex].IsButtonDown(button);
        }
  
        public static bool IsNewButtonPress(Buttons button)
        {
            for (int i = 0; i < 4; i++)
            {
                if (IsNewButtonPress((PlayerIndex)i, button))
                    return true;
            }
            return false;
        }

        public static bool IsNewButtonPress(Buttons button,ref PlayerIndex index)
        {
            for (int i = 0; i < 4; i++)
            {
                if (IsNewButtonPress((PlayerIndex)i, button))
                {
                    index = (PlayerIndex)i;
                    return true;
                }
            }
            return false;
        }

        public static bool IsNewKeyPress(Keys key)
        {
            for (int i = 0; i < 4; i++)
            {
                if (IsNewKeyPress((PlayerIndex)i, key))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsNewKeyPress(PlayerIndex playerIndex, Keys key)
        {
            return _lastKBState[(int)playerIndex].IsKeyDown(key) &&
                _currentKBState[(int)playerIndex].IsKeyUp(key);
        }

        public static bool IsKeyDown(PlayerIndex playerIndex, Keys key)
        {
            return _currentKBState[(int)playerIndex].IsKeyDown(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            for (int i = 0; i < 4; i++)
                if (_currentKBState[i].IsKeyDown(key))
                    return true;
            return false;
        }
        
        public static bool IsKeyUp(PlayerIndex playerIndex, Keys key)
        {
            return _currentKBState[(int)playerIndex].IsKeyUp(key);
        }
        
        public static bool IsKeyUp(Keys key)
        {
            for (int i = 0; i < 4; i++)
                if (_currentKBState[i].IsKeyUp(key))
                    return true;
            return false;
        }
        
        public static bool IsButtonDown(PlayerIndex playerIndex, Buttons btn)
        {
            return _currentPadState[(int)playerIndex].IsButtonDown(btn);
        }
        
        public static bool IsButtonDown(Buttons btn)
        {
            for (int i = 0; i < 4; i++)
                if (_currentPadState[i].IsButtonDown(btn))
                    return true;
            return false;
        }

        public static bool IsAnyKeyDown() {
            for (int i = 0; i < 4; i++)
                if (_currentKBState[i].GetPressedKeys().Length > 0)
                    return true;
            return false;
        }

        public static void RegisterCommand(int id, Buttons[] buttons, Keys[] keys)
        {
            
            if (CommandExists(id))
            {
                throw new ArgumentException("Command Already Exists With That ID");
            }
            else
            {
                InputCommand command = new InputCommand();
                command.AnyPlayer = true;
                command.id = id;
                command.buttons = buttons;
                command.keys = keys;
                _commands.Add(command);
            }
        }
        
        public static void UnregisterAllCommands()
        {
            for (int i = 0; i < _commands.Count; i++)
            {
                UnRegisterCommand(_commands[0].id);
            }
        }
        
        private static bool CommandExists(int id)
        {
            foreach (var item in _commands)
            {
                if (item.id == id)
                    return true;
            }
            return false;
        } 

        #endregion

        /// <summary>
        /// Gets the first connected pads Right Thumb Stick as a Vector2
        /// </summary>
        /// <returns></returns>
        public static Vector2 ThumbStickRightVector()
        {
            for (int i = 0; i < 4; i++)
            {
                if(_padsConnected[i])
                    return _currentPadState[i].ThumbSticks.Right;
            }
            return Vector2.Zero;
        }
       
        /// <summary>
        /// Gets the first connected pads Left Thumb Stick as a Vector2
        /// </summary>
        /// <returns></returns>
        public static Vector2 ThumbStickLeftVector()
        {
            for (int i = 0; i < 4; i++)
            {
                if (_padsConnected[i])
                    return _currentPadState[i].ThumbSticks.Left;
            }
            return Vector2.Zero;
        }
        
        public static Vector2 ThumbStickLeftVector(PlayerIndex index)
        {
            return _currentPadState[(int)index].ThumbSticks.Left;
        }

        public static Vector2 ThumbStickRightVector(PlayerIndex index)
        {
            return _currentPadState[(int)index].ThumbSticks.Right;
        }

        public static void UnRegisterCommand(int id)
        {
            for (int i = 0; i < _commands.Count; i++)
            {
                var item = _commands[i];
                if (item.id == id)
                {
                    _commands.RemoveAt(i);
                    i--;
                }
            }
        }
#if(WINDOWS)

        public static void RegisterCommand(int id, System.Windows.Forms.MouseButtons buttonState)
        {
            if (CommandExists(id))
            {
                throw new ArgumentException("Command Already Exists With That ID");
            }
            else
            {
                InputCommand command = new InputCommand();
                command.id = id;
               // command.buttons = buttons;
                command.MouseButton = buttonState;
               // command.keys = keys;

                _commands.Add(command);
            }
        }
#endif
    }

    public struct InputCommand
    {
        public bool AnyPlayer;
        public PlayerIndex PlayerIndex;
        public int id;
        public Buttons[] buttons;
        public Keys[] keys;
#if(WINDOWS)
        public System.Windows.Forms.MouseButtons? MouseButton;
#endif
    }
}
