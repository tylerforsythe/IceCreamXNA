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
using XnaGame = XnaTouch.Framework.Game;
#else
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


using XnaGame = Microsoft.Xna.Framework.Game;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using IceCream.Drawing;
using IceCream.Components;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using IceCream.Debug.Console;
using IceCream.Input;
using IceCream.Debug;
using IceCream.Farseer;
using Path = System.IO.Path;


namespace IceCream
{
    /// <summary>
    /// The main Game class
    /// </summary>
    public class Game : XnaGame
    {
        
        #region Fields

        private float _elapsed;
        public static Game Instance;
        protected GraphicsDeviceManager graphics;

        protected Point _nativeResolution;
        protected Point _resolution; 
        protected bool enableVSync = true;
        protected bool DebugConsoleOn = true;

        protected String ContentDirectoryName = "Content";
        protected SpriteFont DefaultFont = null;

        #endregion
        
        #region Properties
        
        /// <summary>
        /// Gets the last Elapsed time since the last update.
        /// </summary>
        /// <remarks>This is updated before any SceneItems or Components are updated</remarks>
        public float Elapsed
        {
            get { return _elapsed; }
        }

        public Point NativeResolution
        {
            get { return _nativeResolution; }
            set { _nativeResolution = value; }
        }

        public Point Resolution
        {
            get { return _resolution; }
            set { _resolution = value; }
        }

        public Color DefaultClearColor
        {
            get;
            set;
        }

        #endregion
        
        #region Constructor

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
			#if !REACH
			graphics.PreferMultiSampling = true;
			#endif
            Game.Instance = this;
			#if !REACH
            _nativeResolution = new Point(1280, 720);
			#else
            _nativeResolution = new Point(320, 480);
			#endif
            _resolution = _nativeResolution;
            this.DefaultClearColor = Color.CornflowerBlue;
        }

        #endregion
        
        #region Methods

        protected override void Initialize()
        {
            //try
            //{
                IceProfiler.StartProfiling(IceProfilerNames.GAME_INITIALIZE);
                graphics.PreferredBackBufferWidth = this.NativeResolution.X;
                graphics.PreferredBackBufferHeight = this.NativeResolution.Y;
				#if !REACH
                graphics.SynchronizeWithVerticalRetrace = true;
				#endif
                graphics.ApplyChanges();
                string _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
                //ComponentTypeContainer.SetAppDomain(AppDomain.CurrentDomain);
                ComponentTypeContainer.LoadAssemblyInformation(_path);
                Drawing.DrawingManager.Intialize();
                IceCore.graphicsDevice = graphics.GraphicsDevice;
                base.Initialize();
                
                IceFarseerManager.Initialize();
                
                IceProfiler.StopProfiling(IceProfilerNames.GAME_INITIALIZE);
            //}
            //catch (Exception err)
            //{
            //    Console.WriteLine(err.ToString());
            //    throw err;
            //}
        }

        #region Console Garbage
        private void SetupGameConsole()
        {
            GameConsole.Initialize(this, "DiagnosticFont", Color.White, Color.Black, 0.5f, 15);
            IGameConsole console = (IGameConsole)Services.GetService(typeof(IGameConsole));

            console.LoadStuff();
            console.Log("Type '?' for help");

            console.BindCommandHandler("?", delegate(GameTime time, string[] args)
            {
                console.Log("Commands");
                console.Log("--------");
                //console.Log("PrintArgs [arg1] [arg2] [arg3] ... - Prints the list of arguments passed to it, one per line");
                //console.Log("PrintLine [text] - Prints the specified text to the console");
                //console.Log("SetDefaultLogLevel [level] - Sets the default log level to the specified level");
                //console.Log("SetEchoLogLevel [level] - Sets the log level for echoed output from user input");
                //console.Log("SetLogLevelThreshold [threshold] - Sets the log level threshold to the specified threshold value");
                //console.Log("SetDisplayLevelThreshold [threshold] - Sets the display level threshold to the specified threshold value");
                //console.Log("ToggleEchoEnabled - Toggles the auto input echo feature");
                //console.Log("ToggleTimestamp - Toggles the display of each message's timestamp");
                //console.Log("ToggleLogLevel - Toggles the display of each message's log level");
                //console.Log("SetTextColor [r] [g] [b] - Sets the default text color of console text, where r, g, and b are in the range 0-255");
                //console.Log("SetLogLevelColor [level] [r] [g] [b] - Sets the text color for the specified log level, where r, g and b are in the range 0-255");
                console.Log("SetGameBackgroundColor [r] [g] [b] - Sets the game back color, where r, g and b are in the range 0-255");
                console.Log("SceneStats - Displays the stats of the currently active scene");
            });

            console.BindCommandHandler("PrintArgs", delegate(GameTime time, string[] args)
            {
                foreach (string arg in args)
                {
                    console.Log(arg);
                }
            }, ' ');

            console.BindCommandHandler("SceneStats", delegate(GameTime time, string[] args)
            {
                string _str = "";
                _str = "Scene Items Count : " + SceneManager.ActiveScene.SceneItems.Count.ToString();
                console.Log(_str);

            }, ' ');

     
            console.BindCommandHandler("SetGameBackgroundColor", delegate(GameTime time, string[] args)
            {
                if (args.Length < 3)
                    return;

                try
                {
                    byte r = byte.Parse(args[0]);
                    byte g = byte.Parse(args[1]);
                    byte b = byte.Parse(args[2]);

                    console.TextColor = new Color(r, g, b, 255);
                }
                catch { }
            }, ' ');
            console.BindCommandHandler("PrintLine", delegate(GameTime time, string[] args)
            {
                if (args.Length == 0)
                    return;

                console.Log(args[0]);
            });

            console.BindCommandHandler("SetDefaultLogLevel", delegate(GameTime time, string[] args)
            {
                if (args.Length == 0)
                    return;

                try
                {
                    console.DefaultLogLevel = uint.Parse(args[0]);
                }
                catch { }
            }, ' ');

            console.BindCommandHandler("SetEchoLogLevel", delegate(GameTime time, string[] args)
            {
                if (args.Length == 0)
                    return;

                try
                {
                    console.EchoLogLevel = uint.Parse(args[0]);
                }
                catch { }
            }, ' ');

            console.BindCommandHandler("SetLogLevelThreshold", delegate(GameTime time, string[] args)
            {
                if (args.Length == 0)
                    return;

                try
                {
                    console.LogLevelThreshold = int.Parse(args[0]);
                }
                catch { }
            }, ' ');

            console.BindCommandHandler("SetDisplayLevelThreshold", delegate(GameTime time, string[] args)
            {
                if (args.Length == 0)
                    return;

                try
                {
                    console.DisplayLevelThreshold = int.Parse(args[0]);
                }
                catch { }
            }, ' ');

            console.BindCommandHandler("ToggleEchoEnabled", delegate(GameTime time, string[] args)
            {
                console.EchoEnabled = !console.EchoEnabled;
            });

            console.BindCommandHandler("ToggleTimestamp", delegate(GameTime time, string[] args)
            {
                if ((console.DisplayOptions & ConsoleDisplayOptions.TimeStamp) == ConsoleDisplayOptions.TimeStamp)
                    console.DisplayOptions &= ~ConsoleDisplayOptions.TimeStamp;
                else
                    console.DisplayOptions |= ConsoleDisplayOptions.TimeStamp;
            });

            console.BindCommandHandler("ToggleLogLevel", delegate(GameTime time, string[] args)
            {
                if ((console.DisplayOptions & ConsoleDisplayOptions.LogLevel) == ConsoleDisplayOptions.LogLevel)
                    console.DisplayOptions &= ~ConsoleDisplayOptions.LogLevel;
                else
                    console.DisplayOptions |= ConsoleDisplayOptions.LogLevel;
            });

            console.BindCommandHandler("SetTextColor", delegate(GameTime time, string[] args)
            {
                if (args.Length < 3)
                    return;

                try
                {
                    byte r = byte.Parse(args[0]);
                    byte g = byte.Parse(args[1]);
                    byte b = byte.Parse(args[2]);

                    console.TextColor = new Color(r, g, b, 255);
                }
                catch { }
            }, ' ');

            console.BindCommandHandler("SetLogLevelColor", delegate(GameTime time, string[] args)
            {
                if (args.Length < 4)
                    return;

                try
                {
                    uint level = uint.Parse(args[0]);
                    byte r = byte.Parse(args[1]);
                    byte g = byte.Parse(args[2]);
                    byte b = byte.Parse(args[3]);

                    console.SetLogLevelCustomColor(level, new Color(r, g, b, 255));
                }
                catch { }
            }, ' ');
        }
        #endregion

        /// <summary>
        /// Change the resolution and reload the content, aswell as setup fullscreen and vsync
        /// </summary>
        /// <param name="x">Width</param>
        /// <param name="y">Height</param>
        public virtual void SetVideoResolution(int width, int height, bool isFullScreen, bool vsync)
        {
            Point backBufferSize = new Point(graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                graphics.GraphicsDevice.PresentationParameters.BackBufferHeight);
            if (backBufferSize.X != width || backBufferSize.Y != height || graphics.IsFullScreen != isFullScreen)
            {
                graphics.PreferredBackBufferWidth = width;
                graphics.PreferredBackBufferHeight = height;
				#if !REACH
                graphics.SynchronizeWithVerticalRetrace = vsync;
				#endif
                if (graphics.IsFullScreen != isFullScreen)
                {
                    graphics.ToggleFullScreen();
                }
                graphics.ApplyChanges();
				#if !REACH
                graphics.GraphicsDevice.Reset();
				#endif
            }
        }

        protected override void LoadContent()
        {
            IceProfiler.StartProfiling(IceProfilerNames.GAME_LOADCONTENT);
            IceCore._afterBatch = new SpriteBatch(GraphicsDevice);
            DrawingManager.LoadContent(GraphicsDevice);
            if (!File.Exists(ContentDirectoryName + "/global.ice"))
            {
                throw new Exception("The file global.ice does not exist\nPlease make sure it is included in your Content project.");
            }
            SceneManager.InitializeEmbedded(Services, true);
            SceneManager.LoadGlobalData(ContentDirectoryName + "/");
#if XNATOUCH
			Point backBufferSize = new Point(graphics.GraphicsDevice.Viewport.Width, 
                graphics.GraphicsDevice.Viewport.Height);
#else
            Point backBufferSize = new Point(graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, 
                graphics.GraphicsDevice.PresentationParameters.BackBufferHeight);
#endif
            this.NativeResolution = SceneManager.GlobalDataHolder.NativeResolution;
            if (this.NativeResolution != backBufferSize)
            {
				#if !REACH
                graphics.SynchronizeWithVerticalRetrace = enableVSync;
				#endif
                graphics.PreferredBackBufferWidth = this.NativeResolution.X;
                graphics.PreferredBackBufferHeight = this.NativeResolution.Y;
                graphics.ApplyChanges();
                DrawingManager.LoadContent(GraphicsDevice);
                SceneManager.InitializeEmbedded(Services, true);            
            }
            
            SceneManager.InitializeGlobal();

            IceFarseerManager.Instance.LoadContent(DrawingManager.GraphicsDevice, SceneManager.GlobalContent, DefaultFont);

            //If requested, Setup the game Console window
            if (DebugConsoleOn)
                SetupGameConsole();

            #if !REACH
            if (SceneManager.GlobalDataHolder.AutoSignIntoLive)
                Components.Add(new Microsoft.Xna.Framework.GamerServices.GamerServicesComponent(this));
            #endif

            IceCream.Debug.OnScreenStats.Init();
            
            IceProfiler.StopProfiling(IceProfilerNames.GAME_LOADCONTENT);
        }

        protected override void UnloadContent()
        {
            IceProfiler.StartProfiling(IceProfilerNames.GAME_UNLOADCONTENT);
            #if !REACH
            DrawingManager.RenderTargetManager.UnloadContent();
			#endif

            IceProfiler.StopProfiling(IceProfilerNames.GAME_UNLOADCONTENT);
            #if(PROFILE)
            IceProfiler.SaveProfile();
            #endif
        }

        protected override void Update(GameTime gameTime)
        {
        
            IceCore.GameTime = gameTime;
            _elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;            
            if (SceneManager.ActiveScene != null && !IceCore.activeSceneChanged)
            {
                IceFarseerManager.Instance.Update(_elapsed);
                IceCore.UpdateIceCream(_elapsed);
            }
            else
            {
                IceCore.activeSceneChanged = false;
            }           
            base.Update(gameTime);          
        }

        protected override void Draw(GameTime gameTime)
        {
            _elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;            
            if (DrawingManager.IgnoreClearBeforeRendering == false)
            {
                this.GraphicsDevice.Clear(this.DefaultClearColor);
            }
            if (SceneManager.ActiveScene != null)
            {
                if (SceneManager.Scenes.Count > 1)
                {
                    for (int i = 0; i < SceneManager.Scenes.Count; i++)
                    {
                        IceScene item = SceneManager.Scenes[i];

                        if (item != SceneManager.ActiveScene && item.WillRenderNotActive)
                            IceCore.DrawIceCreamScene(graphics.GraphicsDevice, _elapsed, item);
                    }
                }
                if (SceneManager.ActiveScene._hasBeenUpdatedOnce == true)
                {
                    IceCore.DrawIceCreamScene(graphics.GraphicsDevice, _elapsed, SceneManager.ActiveScene);
                }

                IceCore.RenderIceCream();

                IceCream.Debug.OnScreenStats.Draw();

                DrawingManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawingManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                //DrawingManager.SpriteBatch.Begin();
                IceFarseerManager.Instance.DrawDebug(DrawingManager.SpriteBatch, SceneManager.ActiveScene.ActiveCameras[0]);
                //DrawingManager.SpriteBatch.End();

                //PrimitiveBatch _primitiveBatch = new PrimitiveBatch(DrawingManager.GraphicsDevice);
                //_primitiveBatch.SetViewPortSize(DrawingManager.ViewPortSize.X, DrawingManager.ViewPortSize.Y);
                //_primitiveBatch.Begin(PrimitiveType.TriangleList, SceneManager.ActiveScene.ActiveCameras[0].GetMatrix(DebugShapes.Parallax));
                //Color colorFill = Color.Black;

                //List<Vector2> vertices = new List<Vector2>();
                //vertices.Add(new Vector2(0, 0));
                //vertices.Add(new Vector2(300, 0));
                //vertices.Add(new Vector2(100, 200));
                //vertices.Add(new Vector2(10, 20));

                //DebugShapes.DrawPolygon(vertices.ToArray(), colorFill);

                ////_primitiveBatch.AddVertex(vertices[0], colorFill);
                ////_primitiveBatch.AddVertex(vertices[1], colorFill);
                ////_primitiveBatch.AddVertex(vertices[2], colorFill);

                //Vector2[] _tempVertices = new Vector2[Settings.MaxPolygonVertices];
                //SceneItem testItem = SceneManager.ActiveScene.SceneItems[4];
                //foreach (Fixture fixture in testItem.IceFarseerEntity.Body.FixtureList)
                //{
                //    PolygonShape poly = (PolygonShape)fixture.Shape;
                //    int vertexCount = poly.Vertices.Count;
                //    System.Diagnostics.Debug.Assert(vertexCount <= Settings.MaxPolygonVertices);

                //    Transform xf;
                //    fixture.Body.GetTransform(out xf);
                //    for (int i = 0; i < vertexCount; ++i) {
                //        _tempVertices[i] = MathUtils.Multiply(ref xf, poly.Vertices[i]);
                //    }

                //    DebugShapes.DrawSolidPolygon(_tempVertices, vertexCount, colorFill);
                //}
                ////foreach (DebugLine line in DebugShapes.LinesList) {
                ////    _primitiveBatch.AddVertex(line.vertex, line.color);
                ////}
                //_primitiveBatch.End();

            }
            //for (int y = 0; y < SceneManager.ActiveScene._spatialGrid.Rows; y++)
            //{
            //    for (int x = 0; x < SceneManager.ActiveScene._spatialGrid.Cols; x++)
            //    {
            //        Drawing.DebugShapes.DrawLine(
            //            new Vector2(x * SceneManager.ActiveScene._spatialGrid.CellSize,
            //                        0),
            //            new Vector2(x * SceneManager.ActiveScene._spatialGrid.CellSize, 
            //                        SceneManager.ActiveScene._spatialGrid.SceneHeight),
            //            Color.White);

            //    }
            //    Drawing.DebugShapes.DrawLine(
            //            new Vector2(0,
            //                y * SceneManager.ActiveScene._spatialGrid.CellSize),
            //            new Vector2(SceneManager.ActiveScene._spatialGrid.SceneWidth,
            //                y * SceneManager.ActiveScene._spatialGrid.CellSize),
            //            Color.White);
            //}
            
            base.Draw(gameTime);
        }

        #endregion

    }
}
