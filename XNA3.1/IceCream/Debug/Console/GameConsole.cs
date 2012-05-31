#if !XNATOUCH
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

//
//      coded by un
//            --------------
//                     mindshifter.com
//
//      Modified and extended by Rik Dodsworth (IceCream) 10-05-2009

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using IceCream.Input;

namespace IceCream.Debug.Console
{
    /// <summary>
    /// Delegate called when text is entered into the console
    /// </summary>
    /// <param name="input">The text entered into the console</param>
    /// <param name="time">The game time at which the text was entered</param>
    public delegate void ConsoleInputHandler(string input, GameTime time);

    /// <summary>
    /// Delegate called when a message is logged to the console
    /// </summary>
    /// <param name="message">The message that was logged to the console</param>
    public delegate void ConsoleLogHandler(ConsoleMessage message);

    /// <summary>
    /// Delegate called when a specific command is entered into the console
    /// </summary>
    /// <param name="time">The time at which the command was entered into the console</param>
    /// <param name="args">The arguments passed to the command (text entered after the command, separated by spaces)</param>
    public delegate void ConsoleCommandHandler(GameTime time, string[] args);

    /// <summary>
    /// A set of flags specifying how to display output in the console
    /// </summary>
    [Flags]
    public enum ConsoleDisplayOptions
    {
        None = 0,                                   // Display text of each message only
        TimeStamp = (0x1 << 1),                     // Display the timestamp of each message
        LogLevel = (TimeStamp << 1),                // Display the log level of each message
    }

    /// <summary>
    /// Defines a message logged to the console
    /// </summary>
    public struct ConsoleMessage
    {
        private uint mLevel;      // Log level of the message
        private TimeSpan mGameTime;   // Game time at which the message was logged
        private TimeSpan mRealTime;   // Real time at which the message was logged
        private string mText;       // Text of the message

        /// <summary>
        /// Gets the log level of the message
        /// </summary>
        public uint Level
        {
            get { return mLevel; }
        }

        /// <summary>
        /// Gets the game time at which the message was logged
        /// </summary>
        public TimeSpan GameTime
        {
            get { return mGameTime; }
        }

        /// <summary>
        /// Gets the real time at which the message was logged
        /// </summary>
        public TimeSpan RealTime
        {
            get { return mRealTime; }
        }

        /// <summary>
        /// Gets the text of the message
        /// </summary>
        public string Text
        {
            get { return mText; }
        }

        /// <summary>
        /// Creates a new instance of ConsoleMessage with the specified
        /// log level, timestamp and text
        /// </summary>
        /// <param name="lev">The log level of the message</param>
        /// <param name="gametime">The game time timestamp of the message</param>
        /// <param name="realtime">The real time timestamp of the message</param>
        /// <param name="txt">The text of the message</param>
        public ConsoleMessage(uint lev, TimeSpan gametime, TimeSpan realtime, string txt)
        {
            mLevel = lev;
            mGameTime = gametime;
            mRealTime = realtime;
            mText = txt;
        }
    }

    /// <summary>
    /// GameConsole is a simple console game component that displays a log of text to the 
    /// screen and allows the user to enter input (which is also logged). Handling user
    /// input can be done in two ways, the first by registering a delegate to the
    /// console's 'TextEntered' event, which will trigger any delegates registered to it
    /// when the user enters text into the console. Both the text input and current GameTime
    /// are passed to the delegate for processing, and the function can handle the raw input
    /// any way it wishes to. The second method for handling input is to register a
    /// ConsoleCommandHandler with the console through the 'BindCommandHandler' method.
    /// ConsoleCommandHandler delegates are registered for a specific command string, and
    /// are triggered when that particular string is entered into the console. Any text after
    /// the command is considered to be a set of arguments separated by spaces. These arguments
    /// are passed to the ConsoleCommandHandler delegate when it is triggered.
    /// </summary>
    public class GameConsole : DrawableGameComponent, IGameConsole
    {
        /// <summary>
        /// Initializes the Console, adding an instance of the Console
        /// class to the specified Game's component and service lists. The
        /// IInputManager service may then be retrieved using the Game's
        /// GetService function
        /// </summary>
        /// <param name="game">The XNA Game with which to register the
        /// Console component and IConsole service</param>
        /// <param name="font">The path to the SpriteFont used to draw
        /// text to the console</param>
        /// <param name="textColor">The color of the console text</param>
        /// <param name="backgroundColor">The color of the console background</param>
        /// <param name="backgroundAlpha">The alpha translucency of the console background</param>
        /// <param name="lineCount">The maximum number of text lines visible in
        /// the console at any given time</param>
        public static void Initialize(Game game, string font, Color textColor,
            Color backgroundColor, float backgroundAlpha, int lineCount)
        {
            GameConsole console = new GameConsole(game, font, textColor,
                backgroundColor, backgroundAlpha, lineCount);

            game.Components.Add(console);
            game.Services.AddService(typeof(IGameConsole), console);
        }

        /// <summary>
        /// Describes a ConsoleCommandHandler entry and the argument separators it requires
        /// </summary>
        private class ConsoleCommandHandlerInfo
        {
            public ConsoleCommandHandler Handler;
            public char[] ArgumentSeparators;

            public ConsoleCommandHandlerInfo(ConsoleCommandHandler handler, char[] separators)
            {
                Handler = handler;
                ArgumentSeparators = separators;
            }
        }

        /// <summary>
        /// Event triggered when text is entered into the console
        /// </summary>
        public event ConsoleInputHandler TextEntered;

        /// <summary>
        /// Event triggered when a message is logged to the console
        /// </summary>
        public event ConsoleLogHandler MessageLogged;

        private Dictionary<string,
            ConsoleCommandHandlerInfo> mCommandHandlers;

        // graphics
        private SpriteBatch mSpriteBatch;
        private SpriteFont mFont;
        private BasicEffect mEffect;

        private VertexDeclaration mVertexDeclaration;


        // display
        private ConsoleDisplayOptions mDisplayOptions;
        private string mTimestampFormat;

        private Dictionary<char, Vector2> mCharSizeLut;

        private float mFontHeight;
        private float mHorizontalPadding;
        private int mVisibleLineCount;
        private float mVerticalSpacing;
        private int mCurrentLine;
        private bool mAutoScrollOnOutput;
        private bool mAlertOnUnrecognizedCommand;

        private string mFontName;
        private Color mDefaultTextColor;
        private Dictionary<uint, Color> mTextColors;
        private Color mBackgroundColor;
        private float mBackgroundAlpha;

        // logging
        private bool mEchoEnabled;
        private uint mEchoLogLevel;
        private uint mDefaultLogLevel;
        private int mLogLevelThreshold;
        private int mDisplayLevelThreshold;

        // cursor
        private int mInputPosition;
        private bool mCursorEnabled;
        private float mCursorBlinkSpeed;
        private float mCursorTimer;
        private bool mDrawCursor;
        private float mNotifyBlinkSpeed;
        private float mNotifyTimer;
        private bool mDrawNotify;

        // message log
        private List<ConsoleMessage> mLog;
        private StringBuilder mCurrentText;
        private string mInputPrompt;

        private GameTime mCurrentTime;

        // input
        private KeyboardState mLastKeyboardState;
        private Keys mCloseKey;

        /// <summary>
        /// Gets or sets the console's text color
        /// </summary>
        public Color TextColor
        {
            get { return mDefaultTextColor; }
            set { mDefaultTextColor = value; }
        }

        /// <summary>
        /// Gets or sets the console's background color
        /// </summary>
        public Color BackgroundColor
        {
            get { return mBackgroundColor; }
            set { mBackgroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the console's background alpha translucency
        /// </summary>
        public float BackgroundAlpha
        {
            get { return mBackgroundAlpha; }
            set { mBackgroundAlpha = MathHelper.Clamp(value, 0f, 1f); }
        }

        /// <summary>
        /// Gets or sets the console's display options
        /// </summary>
        public ConsoleDisplayOptions DisplayOptions
        {
            get { return mDisplayOptions; }
            set { mDisplayOptions = value; }
        }

        /// <summary>
        /// Gets or sets the timestamp format for message timestamps.
        /// The following symblols are replaced with their corresponding values
        /// when the timestamp for each message is constructed:
        ///     {Hr} - Hour (in 24 hour format)
        ///     {Min} - Minute
        ///     {Sec} - Second
        ///     {Ms} - Millisecond
        /// 
        /// Sample timestamp strings:
        ///     "{Hr}:{Min}:{Sec}"
        ///     "[{Hr}:{Min}:{Sec}:{Ms}]"
        /// </summary>
        public string TimestampFormat
        {
            get { return mTimestampFormat; }
            set { mTimestampFormat = value; }
        }

        /// <summary>
        /// Gets or sets the prompt used on the input line of the console
        /// </summary>
        public string Prompt
        {
            get { return mInputPrompt; }
            set { mInputPrompt = value; }
        }

        /// <summary>
        /// Gets or sets the number of text lines visible in the
        /// console
        /// </summary>
        public int VisibleLineCount
        {
            get { return mVisibleLineCount; }
            set
            {
                mVisibleLineCount = (value < 1) ? (1) : (value);
            }
        }

        /// <summary>
        /// Gets or sets the vertical spacing between text lines in
        /// the console
        /// </summary>
        public float VerticalLineSpacing
        {
            get { return mVerticalSpacing; }
            set
            {
                mVerticalSpacing = (value < 0) ? (0) : (value);
            }
        }

        /// <summary>
        /// Gets or sets the amount by which text in the console will be
        /// right offset from the leftmost side of the screen
        /// </summary>
        public float HorizontalPadding
        {
            get { return mHorizontalPadding; }
            set
            {
                mHorizontalPadding = (value < 0) ? (0) : (value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the console should
        /// automatically scroll to the last line of the message log whenever a 
        /// message is output to the console
        /// </summary>
        public bool AutoScrollOnOutput
        {
            get { return mAutoScrollOnOutput; }
            set { mAutoScrollOnOutput = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the console will output
        /// the warning "Unrecognized Command" if the user inputs a command not
        /// registered with a command handler
        /// </summary>
        public bool AlertOnUnrecognizedCommand
        {
            get { return mAlertOnUnrecognizedCommand; }
            set { mAlertOnUnrecognizedCommand = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the console
        /// should automatically echo and input it receives from the user
        /// to the console log
        /// </summary>
        public bool EchoEnabled
        {
            get { return mEchoEnabled; }
            set { mEchoEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the log level of messages echoed automatically
        /// if the EchoEnabled property is set to true
        /// </summary>
        public uint EchoLogLevel
        {
            get { return mEchoLogLevel; }
            set { mEchoLogLevel = value; }
        }

        /// <summary>
        /// Gets or sets the console's default log level which will
        /// be assigned to messages logged to the console with no log level
        /// defined
        /// </summary>
        public uint DefaultLogLevel
        {
            get { return mDefaultLogLevel; }
            set { mDefaultLogLevel = value; }
        }

        /// <summary>
        /// Gets or sets the console's log level threshold. If a message's whose
        /// log level is higher than the threshold is logged to the console, it will
        /// be ignored. However, if the value is negative then all messages will be
        /// logged regardless of their log level
        public int LogLevelThreshold
        {
            get { return mLogLevelThreshold; }
            set { mLogLevelThreshold = value; }
        }

        /// <summary>
        /// Gets or sets the console's display level threshold. The console will
        /// only display messages in the log with log levels lower or equal to
        /// the display level threshold. However, if the value is negative,
        /// then all messages will be displayed regardless of their threshold
        /// </summary>
        public int DisplayLevelThreshold
        {
            get { return mDisplayLevelThreshold; }
            set { mDisplayLevelThreshold = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the input cursor
        /// is enabled. If enabled, the user can use the arrow keys to navigate
        /// the edit position within the current input string
        /// </summary>
        public bool CursorEnabled
        {
            get { return mCursorEnabled; }
            set { mCursorEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the blink interval of the input cursor, in seconds. The cursor
        /// will not blink if this value is set to 0.
        /// </summary>
        public float CursorBlinkSpeed
        {
            get { return mCursorBlinkSpeed; }
            set { mCursorBlinkSpeed = (value < 0) ? (0) : (value); }
        }

        /// <summary>
        /// Gets or sets the blink interval of the notifier that is displayed when
        /// the console's current line is not the last line of the log, that is: when
        /// the user has used PageUp/Up to scroll upward in the console. The notifier
        /// will not blink if this value is set to 0.
        /// </summary>
        public float NotifyBlinkSpeed
        {
            get { return mNotifyBlinkSpeed; }
            set { mNotifyBlinkSpeed = (value < 0) ? (0) : (value); }
        }

        /// <summary>
        /// Gets the console log of messages
        /// </summary>
        public ReadOnlyCollection<ConsoleMessage> MessageLog
        {
            get { return mLog.AsReadOnly(); }
        }

        /// <summary>
        /// Gets a value indicating whether or not the console is currently open
        /// </summary>
        public bool IsOpen
        {
            get { return isEnabled; }
        }

        /// <summary>
        /// Creates a new instance of Console
        /// </summary>
        /// <param name="game">The XNA Game with which to register the
        /// Console component and IConsole service</param>
        /// <param name="font">The path to the SpriteFont used to draw
        /// text to the console</param>
        /// <param name="textColor">The color of the console text</param>
        /// <param name="bgColor">The color of the console background</param>
        /// <param name="bgAlpha">The alpha translucency of the console background</param>
        /// <param name="lineCount">The maximum number of text lines visible in
        /// the console at any given time</param>
        internal GameConsole(Game game, string font, Color textColor,
            Color bgColor, float bgAlpha, int lineCount)
            : base(game)
        {
            mFontName = font;
            mDefaultTextColor = textColor;
            mBackgroundColor = bgColor;
            mBackgroundAlpha = MathHelper.Clamp(bgAlpha, 0f, 1f);

            mVisibleLineCount = (lineCount < 1) ? (1) : (lineCount);
            mVerticalSpacing = 5f;
            mHorizontalPadding = 5f;

            mDisplayOptions = ConsoleDisplayOptions.None;
            mTimestampFormat = "{Hr}:{Min}:{Sec}";

            mCharSizeLut = new Dictionary<char, Vector2>();

            mTextColors = new Dictionary<uint, Color>();

            mCommandHandlers
                = new Dictionary<string, ConsoleCommandHandlerInfo>();

            mLog = new List<ConsoleMessage>();
            mCurrentText = new StringBuilder();
            mInputPrompt = ">";

            // echo enabled by default
            mEchoEnabled = true;
            mEchoLogLevel = 0;
            mDefaultLogLevel = 0;

            // log and display all by default
            mLogLevelThreshold = -1;
            mDisplayLevelThreshold = -1;

            // cursor enabled by default
            mCursorEnabled = true;
            mCursorBlinkSpeed = 0.25f;
            mNotifyBlinkSpeed = 0.25f;

            mAutoScrollOnOutput = true;
            mAlertOnUnrecognizedCommand = true;
            mDrawCursor = false;
            mDrawNotify = false;
            mCursorTimer = mCursorBlinkSpeed;
            mNotifyTimer = mNotifyBlinkSpeed;

            mCurrentTime = new GameTime();

            mCloseKey = Keys.OemTilde;

            isEnabled = false;
        }
        public void LoadStuff()
        {
            LoadContent();
        }
        /// <summary>
        /// Loads managed resources used by the console
        /// </summary>
        protected override void LoadContent()
        {
            IGraphicsDeviceService graphics =
                (IGraphicsDeviceService)Game.Services.GetService(typeof(IGraphicsDeviceService));

            mSpriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            mFont = SceneManager.GetEmbeddedFont(mFontName).Font;

            mVertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice,
                    new VertexElement[] {
                        new VertexElement(0, 0, VertexElementFormat.Vector3,
                        VertexElementMethod.Default, VertexElementUsage.Position, 0)
                    });

            mEffect = new BasicEffect(graphics.GraphicsDevice, null);
            mEffect.VertexColorEnabled = false;
            mEffect.LightingEnabled = false;
            mEffect.TextureEnabled = false;
            mEffect.View = Matrix.Identity;
            mEffect.World = Matrix.Identity;

            // measure the font height
            mFontHeight = 0f;

            List<char> chars = ConsoleKeyMap.GetRegisteredCharacters();
            foreach (char c in chars)
            {
                Vector2 size = mFont.MeasureString(c.ToString());

                if (size.Y > mFontHeight)
                    mFontHeight = size.Y;

                mCharSizeLut[c] = size;
            }

            base.LoadContent();
        }
        bool isEnabled = false;
        /// <summary>
        /// Updates any input to the console
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(GameTime gameTime)
        {
            if (!isEnabled)
            {
                
#if !XBOX
                //TODO: Needs refactoring
                IGameConsole console = (IGameConsole)Game.Services.GetService(typeof(IGameConsole));
                if (!console.IsOpen && InputCore.IsNewKeyPress(Keys.OemTilde))
                    console.Open(Keys.OemTilde);
#endif

                return;
            }
            mCurrentTime = gameTime;

            // handle cursor blinking
            mCursorTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (mCursorTimer <= 0)
            {
                mDrawCursor = !mDrawCursor;
                mCursorTimer = mCursorBlinkSpeed + mCursorTimer;
            }

            // handle notify blinking
            mNotifyTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (mNotifyTimer <= 0)
            {
                mDrawNotify = !mDrawNotify;
                mNotifyTimer = mNotifyBlinkSpeed + mNotifyTimer;
            }

            KeyboardState kb = Keyboard.GetState();

            // Close the console if the close key was pressed
            //if ((kb[mCloseKey] == KeyState.Down)
              //  && (mLastKeyboardState[mCloseKey] == KeyState.Up))
            if(InputCore.IsNewKeyPress(mCloseKey))
            {
                isEnabled = false;
                return;
            }

            // handle input cursor movement if cursor is enabled
            if (mCursorEnabled)
            {
                if ((kb[Keys.Left] == KeyState.Down) && (mLastKeyboardState[Keys.Left] == KeyState.Up))
                    if (mInputPosition > 0)
                        mInputPosition--;

                if ((kb[Keys.Right] == KeyState.Down) && (mLastKeyboardState[Keys.Right] == KeyState.Up))
                    if (mInputPosition < mCurrentText.Length)
                        mInputPosition++;
            }

            // handle paging up and down
            if ((kb[Keys.PageUp] == KeyState.Down) && (mLastKeyboardState[Keys.PageUp] == KeyState.Up))
                mCurrentLine = (int)MathHelper.Clamp(mCurrentLine - mVisibleLineCount, 0, mLog.Count - 1);

            if ((kb[Keys.PageDown] == KeyState.Down) && (mLastKeyboardState[Keys.PageDown] == KeyState.Up))
                mCurrentLine = (int)MathHelper.Clamp(mCurrentLine + mVisibleLineCount, 0, mLog.Count - 1);

            if ((kb[Keys.Up] == KeyState.Down) && (mLastKeyboardState[Keys.Up] == KeyState.Up))
                mCurrentLine = (int)MathHelper.Clamp(mCurrentLine - 1, 0, mLog.Count - 1);

            if ((kb[Keys.Down] == KeyState.Down) && (mLastKeyboardState[Keys.Down] == KeyState.Up))
                mCurrentLine = (int)MathHelper.Clamp(mCurrentLine + 1, 0, mLog.Count - 1);

            // Process each pressed key for a key down event
            foreach (Keys key in kb.GetPressedKeys())
            {
                // if its a repeat key, skip it
                if (mLastKeyboardState[key] != KeyState.Up)
                    continue;

                char ch = new char();
                KeyModifier mod = KeyModifier.None;

                // check for shift modifiers
                if ((kb[Keys.LeftShift] == KeyState.Down)
                    || (kb[Keys.RightShift] == KeyState.Down))
                {
                    mod = KeyModifier.Shift;
                }

                if (ConsoleKeyMap.GetCharacter(key, mod, ref ch))
                {
                    
                    mCurrentText.Insert(mInputPosition,new char[]{ch});
                    mInputPosition++;
                }
            }

            // check for backspace
            if (kb[Keys.Back] == KeyState.Down && mLastKeyboardState[Keys.Back] == KeyState.Up)
            {
                if (mInputPosition > 0)
                    mCurrentText.Remove(mInputPosition - 1, 1);

                mInputPosition = (int)MathHelper.Clamp(mInputPosition - 1, 0, mCurrentText.Length);
            }

            // check for entered text
            if ((kb[Keys.Enter] == KeyState.Down) && (mLastKeyboardState[Keys.Enter] == KeyState.Up))
            {
                mInputPosition = 0;

                // if the text is length 0, we won't log it
                if (mCurrentText.Length == 0)
                    return;

                // build the current text input string
                string input = mCurrentText.ToString();

                // break the text into a command and arguments for any command handlers that
                // might be registered for it
#if XBOX
                string[] command = input.Split(new char[] { ' ', '\t' });
#else
                string[] command = input.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
#endif
                // nothing useful...
                if (command.Length == 0)
                    return;

                // log it if echo is enabled
                if (mEchoEnabled)
                    Log(input, mEchoLogLevel);

                // let the raw input handlers do their thing
                if (TextEntered != null)
                    TextEntered(input, gameTime);

                // remove the command part from the input, leaving the arguments
                input = input.Remove(0, command[0].Length + ((command.Length > 1) ? (1) : (0)));

                // clear the current text
                mCurrentText.Remove(0, mCurrentText.Length);

                // call any command handlers registered to the command
                if (mCommandHandlers.ContainsKey(command[0]))
                {
                    string[] args = new string[] { input };

                    if (mCommandHandlers[command[0]].ArgumentSeparators.Length > 0)
                    {
#if XBOX
                        args = input.Split(mCommandHandlers[command[0]].ArgumentSeparators);
#else
                        args = input.Split(mCommandHandlers[command[0]].ArgumentSeparators,
                                                    StringSplitOptions.RemoveEmptyEntries);
#endif
                    }

                    mCommandHandlers[command[0]].Handler(gameTime, args);
                }
                else if (mAlertOnUnrecognizedCommand)
                {
                    Log(string.Format("Unrecognized Command: '{0}'", command[0]), 0);
                }
            }

            // save last keyboard state
            mLastKeyboardState = kb;

            base.Update(gameTime);
        }

        /// <summary>
        /// Breaks the specified text into multiple lines if neccessary, based on the
        /// specified screen width
        /// </summary>
        /// <param name="screenWidth">The width of the current viewport</param>
        /// <param name="text">The text to process</param>
        /// <returns>The set of lines for the specified text that will fit visibly into
        /// the console display area</returns>
        private List<string> GetLines(float screenWidth, string text)
        {
            List<string> lines = new List<string>();

            StringBuilder current = new StringBuilder();
            float width = mHorizontalPadding;

            for (int i = 0; i < text.Length; ++i)
            {
                if (!mCharSizeLut.ContainsKey(text[i]))
                {
                    Vector2 size = mFont.MeasureString(text[i].ToString());
                    mCharSizeLut[text[i]] = size;
                }

                if ((width + mCharSizeLut[text[i]].X) >= (screenWidth - mHorizontalPadding))
                {
                    lines.Add(current.ToString());
                    current.Remove(0, current.Length);

                    current.Append(text[i]);
                    width = mHorizontalPadding;
                }
                else
                {
                    current.Append(text[i]);
                    width += mCharSizeLut[text[i]].X;
                }
            }

            if (current.Length > 0)
                lines.Add(current.ToString());

            return lines;
        }

        /// <summary>
        /// Draws the console to the screen if the console is currently open
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Draw(GameTime gameTime)
        {
            if (!isEnabled)
                return;

            IGraphicsDeviceService graphics =
                (IGraphicsDeviceService)Game.Services.GetService(typeof(IGraphicsDeviceService));

            // measure the height of a line in the current font
            float height = (mFontHeight + mVerticalSpacing) * (mVisibleLineCount + 1);

            // create vertices for the background quad
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(graphics.GraphicsDevice.Viewport.Width, 0, 0),
                new Vector3(graphics.GraphicsDevice.Viewport.Width, height, 0),
                new Vector3(0, height, 0),
            };

            // create indices for the background quad
            short[] indices = new short[] { 0, 1, 2, 2, 3, 0 };

            // set the vertex declaration
            graphics.GraphicsDevice.VertexDeclaration = mVertexDeclaration;

            // create an orthographic projection to draw the quad as a sprite
            mEffect.Projection = Matrix.CreateOrthographicOffCenter(0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height, 0,
                0, 1);

            mEffect.DiffuseColor = mBackgroundColor.ToVector3();
            mEffect.Alpha = mBackgroundAlpha;

            // save current blending mode
            bool blend = graphics.GraphicsDevice.RenderState.AlphaBlendEnable;

            // enable alpha blending
            graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            graphics.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
            graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            graphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = false;

            mEffect.Begin();

            // draw the quad
            foreach (EffectPass pass in mEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                graphics.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                    vertices, 0, 4, indices, 0, 2);

                pass.End();
            }

            mEffect.End();

            // restore previous alpha blend
            graphics.GraphicsDevice.RenderState.AlphaBlendEnable = blend;

            // begin drawing the console text
            mSpriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                SpriteSortMode.Immediate, SaveStateMode.None);

            // measure and draw the prompt
            float promptsize = mFont.MeasureString(mInputPrompt + " ").X;
            Vector2 position = new Vector2(mHorizontalPadding, height - mFontHeight - mVerticalSpacing);
            mSpriteBatch.DrawString(mFont, mInputPrompt + " ", position, mDefaultTextColor);
            position.X += promptsize;

            // measure and draw the current input text
            string current = mCurrentText.ToString();

            float cursorpos = position.X;
            cursorpos += mFont.MeasureString(current.Substring(0, mInputPosition)).X;

            if (cursorpos >= (graphics.GraphicsDevice.Viewport.Width - mHorizontalPadding))
                position.X -= (cursorpos - (graphics.GraphicsDevice.Viewport.Width - mHorizontalPadding));

            mSpriteBatch.DrawString(mFont, current, position, mDefaultTextColor);
            position.Y -= (mFontHeight + mVerticalSpacing);
            position.X = mHorizontalPadding;

            // draw log text
            int messageIndex = (mLog.Count > 0) ? (mCurrentLine) : (-1);
            int lineIndex = mVisibleLineCount;
            while ((messageIndex >= 0) && (lineIndex > 0))
            {
                if ((mDisplayLevelThreshold > 0) && (mLog[messageIndex].Level > (uint)mDisplayLevelThreshold))
                {
                    messageIndex--;
                    continue;
                }

                string text = mLog[messageIndex].Text;
                Color textcolor = (mTextColors.ContainsKey(mLog[messageIndex].Level))
                    ? (mTextColors[mLog[messageIndex].Level])
                    : (mDefaultTextColor);

                if ((mDisplayOptions & ConsoleDisplayOptions.LogLevel) == ConsoleDisplayOptions.LogLevel)
                    text = text.Insert(0, string.Format("[{0}] ", mLog[messageIndex].Level));

                if ((mDisplayOptions & ConsoleDisplayOptions.TimeStamp) == ConsoleDisplayOptions.TimeStamp)
                {
                    TimeSpan time = mLog[messageIndex].RealTime;
                    string timestamp = mTimestampFormat;

                    timestamp = timestamp.Replace("{Hr}", time.Hours.ToString());
                    timestamp = timestamp.Replace("{Min}", time.Minutes.ToString());
                    timestamp = timestamp.Replace("{Sec}", time.Seconds.ToString());
                    timestamp = timestamp.Replace("{Ms}", time.Milliseconds.ToString());

                    text = text.Insert(0, timestamp + " ");
                }

                List<string> lines = GetLines(graphics.GraphicsDevice.Viewport.Width, text);

                for (int i = lines.Count - 1; i >= 0; --i)
                {
                    mSpriteBatch.DrawString(mFont, lines[i], position, textcolor);

                    position.Y -= (mFontHeight + mVerticalSpacing);

                    if (--lineIndex <= 0)
                        break;
                }

                messageIndex--;
            }

            mSpriteBatch.End();

            // draw the cursor
            if (mDrawCursor)
            {
                position.Y = height - mFontHeight - mVerticalSpacing;
                mEffect.DiffuseColor = mDefaultTextColor.ToVector3();
                mEffect.Alpha = 1.0f;

                if (cursorpos >= (graphics.GraphicsDevice.Viewport.Width - mHorizontalPadding))
                    cursorpos -= cursorpos - (graphics.GraphicsDevice.Viewport.Width - mHorizontalPadding);

                Vector3[] cursorverts = new Vector3[]
                {
                    new Vector3(cursorpos, position.Y, 0),
                    new Vector3(cursorpos, position.Y + mFontHeight, 0),
                };

                graphics.GraphicsDevice.VertexDeclaration = mVertexDeclaration;

                mEffect.Begin();
                foreach (EffectPass pass in mEffect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList,
                        cursorverts, 0, 1);
                    pass.End();
                }
                mEffect.End();
            }

            // Handle out-of-page notification
            if (mDrawNotify && (mCurrentLine < mLog.Count - 1))
            {
                float yofs = height - mFontHeight - (mVerticalSpacing * 1.5f);
                mEffect.DiffuseColor = mDefaultTextColor.ToVector3();
                mEffect.Alpha = 1.0f;

                Vector3[] notifyverts = new Vector3[]
                {
                    new Vector3(0, yofs, 0),
                    new Vector3(graphics.GraphicsDevice.Viewport.Width, yofs, 0),
                };

                graphics.GraphicsDevice.VertexDeclaration = mVertexDeclaration;

                mEffect.Begin();
                foreach (EffectPass pass in mEffect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList,
                        notifyverts, 0, 1);
                    pass.End();
                }
                mEffect.End();
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Binds a ConsoleCommandHandler to the specified command
        /// </summary>
        /// <param name="command">The command to which the command handler will be bound</param>
        /// <param name="handler">The command handler to bind to the specified command</param>
        /// <param name="argumentSeparators">The characters with which the command argument string will
        /// be split into separate arguments</param>
        public void BindCommandHandler(string command, ConsoleCommandHandler handler, params char[] argumentSeparators)
        {
            mCommandHandlers[command] = new ConsoleCommandHandlerInfo(handler, argumentSeparators);
        }

        /// <summary>
        /// Unbinds the ConsoleCommandHandler for the specified command
        /// </summary>
        /// <param name="command">The command for which to unbind the current
        /// command handler</param>
        /// <returns>The command handler previously bound to the specified command,
        /// or null if no handler was bound</returns>
        public ConsoleCommandHandler UnbindCommandHandler(string command)
        {
            if (!mCommandHandlers.ContainsKey(command))
                return null;

            ConsoleCommandHandlerInfo info = mCommandHandlers[command];
            mCommandHandlers.Remove(command);

            return info.Handler;
        }

        /// <summary>
        /// Displays the console
        /// </summary>
        /// <param name="closeKey">The key that should deactivate the console
        /// when pressed</param>
        public void Open(Keys closeKey)
        {
            mCloseKey = closeKey;
            mLastKeyboardState = Keyboard.GetState();
            isEnabled = true;
        }

        /// <summary>
        /// Logs a message with the specified log level to the console
        /// </summary>
        /// <param name="message">The message to output to the console</param>
        /// <param name="level">The log level of the message</param>
        public void Log(string message, uint level)
        {
            // ignore it if its above the threshold
            if ((mLogLevelThreshold >= 0) && (level > (uint)mLogLevelThreshold))
                return;

            ConsoleMessage msg = new ConsoleMessage(level, mCurrentTime.TotalGameTime,
                DateTime.Now.TimeOfDay, message);

            mLog.Add(msg);

            if (MessageLogged != null)
                MessageLogged(msg);

            if (mAutoScrollOnOutput)
                mCurrentLine = mLog.Count - 1;
        }

        /// <summary>
        /// Logs a message with log level 0 to the console
        /// </summary>
        /// <param name="message">The message to output to the console</param>
        public void Log(string message)
        {
            // ignore it if its above the threshold
            if ((mLogLevelThreshold >= 0) && (mDefaultLogLevel > (uint)mLogLevelThreshold))
                return;

            ConsoleMessage msg = new ConsoleMessage(mDefaultLogLevel, mCurrentTime.TotalRealTime,
                DateTime.Now.TimeOfDay, message);
            mLog.Add(msg);

            if (MessageLogged != null)
                MessageLogged(msg);

            if (mAutoScrollOnOutput)
                mCurrentLine = mLog.Count - 1;
        }

        /// <summary>
        /// Sets the custom display color of text for the specified log level
        /// </summary>
        /// <param name="level">The log level for which to set the custom text
        /// display color</param>
        /// <param name="color">The custom color with which to display text for
        /// the specified log level</param>
        public void SetLogLevelCustomColor(uint level, Color color)
        {
            mTextColors[level] = color;
        }

        /// <summary>
        /// Gets the custom display color of text for the specified log level,
        /// or returns the default text color if a custom color is not
        /// set for the level
        /// </summary>
        /// <param name="level">The log level for which to get the
        /// custom display color</param>
        /// <returns>The custom display color of text for the specified log level, or
        /// the default text color if no custom color is set for the level</returns>
        public Color GetLogLevelCustomColor(uint level)
        {
            if (mTextColors.ContainsKey(level))
                return mTextColors[level];

            return mDefaultTextColor;
        }

        /// <summary>
        /// Unsets the custom display color of text for the specified log level
        /// </summary>
        /// <param name="level">The log level for which to unset the custom
        /// text color</param>
        public void UnsetLogLevelCustomColor(uint level)
        {
            if (mTextColors.ContainsKey(level))
                mTextColors.Remove(level);
        }
    }


}
#endif