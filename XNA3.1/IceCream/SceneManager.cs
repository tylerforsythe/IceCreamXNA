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
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Globalization;
using IceCream.Attributes;
using IceCream.Components;
using IceCream.Drawing;
using IceCream.Serialization;
using IceCream.SceneItems;

namespace IceCream
{
    public static class SceneManager
    {
        #region Static Fields

        public static NetworkSession networkSession = null;
        internal static List<Assembly> _assemblies;
        private static IceScene _activeScene;
        private static List<IceScene> _scenes;
        private static ContentManager _globalcontent;
        private static GlobalDataHolder _globalDataHolder;
        private static List<Material> _embeddedMaterials = new List<Material>();
        private static List<IceFont> _embeddedFonts = new List<IceFont>();

        #endregion

        #region Properties
        /// <summary>
        /// Trial property for networking code | dont remove 
        /// </summary>
        public static bool IsNetworkOwner { get; set; }

        public static GlobalDataHolder GlobalDataHolder
        {
            get { return _globalDataHolder; }
            set { _globalDataHolder = value; }
        }

        public static ContentManager GlobalContent
        {
            get { return _globalcontent; }
        }

        public static List<Material> EmbeddedMaterials
        {
            get { return _embeddedMaterials; }
            set { _embeddedMaterials = value; }
        }

        public static List<IceFont> EmbeddedFonts
        {
            get { return _embeddedFonts; }
            set { _embeddedFonts = value; }
        }

        public static List<IceScene> Scenes
        {
            get { return _scenes; }
        }

        public static IceScene ActiveScene
        {
            get { return _activeScene; }
            set
            {
                if (_activeScene != null)
                {
                    _activeScene.Enabled = false;
                }
                _activeScene = value;
                if (value != null)
                {
                    _activeScene.Enabled = true;
                }
                IceCore.activeSceneChanged = true;
            }
        }

        #endregion

        #region Constructor

        static SceneManager()
        {
            _scenes = new List<IceScene>();
            _assemblies = new List<Assembly>();
            _globalDataHolder = new GlobalDataHolder();

            //LoadAssemblies();
        }

        #endregion

        #region Methods

        private static void LoadAssemblies()
        {
            string _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
            foreach (string _file in Directory.GetFiles(_path))
            {
                try
                {

                    if (_file.EndsWith(".exe") || _file.EndsWith(".dll"))
                    {
                        TraceLogger.TraceInfo("Reading Assembly " + Path.GetFileName(_file));
#if(WINDOWS)
                        Assembly _asm = Assembly.LoadFile(_file);
#else
                        Assembly _asm = Assembly.LoadFrom(_file);
#endif
                        if (HasComponents(_asm))
                            _assemblies.Add(_asm);
                    }
                }
                catch { }
            }
        }

        static string _tempRoot;
        public static void LoadAssemblies(string rootpath)
        {
            string _path = rootpath; // Path.GetDirectoryName(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
            _tempRoot = rootpath;
#if(WINDOWS)
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

#endif
            foreach (string _file in Directory.GetFiles(_path))
            {
                try
                {
                    if (_file.EndsWith(".exe") || _file.EndsWith(".dll"))
                    {
                        //                        #if WINDOWS
                        //Assembly _asm = Assembly.LoadFile(_file);
#if !WINDOWS
                        Assembly _asm = Assembly.LoadFrom(_file);
#else
                        Assembly _asm = null;
                        _asm = Assembly.LoadFile(_file);
#endif

                        if (HasComponents(_asm))
                            _assemblies.Add(_asm);
                        //                      #endif
                    }
                }
                catch (Exception err)
                {
                    TraceLogger.TraceError("Error in SceneManager Load Assemblies\r\n" + err.ToString());
                }
            }
#if(WINDOWS)
            AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
#endif
        }

#if(WINDOWS)
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (var item in Directory.GetFiles(_tempRoot))
            {
                if (item.EndsWith(".exe") || item.EndsWith(".dll"))
                {
                    Assembly tmp = Assembly.LoadFile(item);
                    if (tmp.FullName == args.Name)
                        return tmp;
                }
            }
            return null;
        }

#endif

        private static bool HasComponents(Assembly asm)
        {
            foreach (Type t in asm.GetTypes())
            {
                var _attribs = t.GetCustomAttributes(true);
                foreach (var item in _attribs)
                {
                    Attribute b = (Attribute)item;
                    if (b.GetType().Equals(typeof(IceComponentAttribute)))
                    {
                        TraceLogger.TraceInfo("Assembly Has Components To Read");
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Loads the given filename into a scene and returns it. 
        /// </summary>
        /// <param name="filename">The filename of the scene</param>
        /// <returns>The loaded Scene</returns>
        /// <remarks>Automatically loads Materials</remarks>
        public static IceScene LoadScene(string filename)
        {
            return LoadScene(filename, true);
        }

        /// <summary>
        /// Loads the given filename into a scene and returns it. 
        /// </summary>
        /// <param name="filename">The filename of the scene</param>
        /// <param name="loadMaterials">Should load materials</param>
        /// <remarks>If false is passed to load materials they are not loaded from the content manager into the texture object. This is for the editor</remarks>
        public static IceScene LoadScene(string filename, bool loadMaterials)
        {
            if (!filename.EndsWith(".icescene"))
                filename += ".icescene";

            string _filename = Path.GetFullPath(filename);
            if (!File.Exists(_filename))
            {
                throw new FileNotFoundException("File Not Found: " + filename);
            }
            IceScene scene = Serialization.SceneSerializer.DeSerializeScene(filename, "");

            scene.WillRenderNotActive = false;
            Scenes.Add(scene);
            if (_activeScene == null)
            {
                _activeScene = scene;
            }

            if (loadMaterials == true)
            {
                InitializeScene(scene);
            }
            scene.Enabled = true;
            scene.WillRenderNotActive = true;

            return scene;
        }

        public static void UnLoadScene(IceScene oldScene)
        {
            oldScene.Unload();
            oldScene.ContentManager.Unload();
            oldScene.ContentManager.Dispose();

            Scenes.Remove(oldScene);
            if (oldScene == ActiveScene)
                ActiveScene = null;
            if (ActiveScene == null && Scenes.Count > 0) //Pick the top scene
                ActiveScene = Scenes[Scenes.Count - 1];

        }

        private static void InitializeScene(IceScene _scene)
        {
            _scene.InitializeContent(Game.Instance.Services);
            // Create a default full sized camera            
            Camera camera = new Camera();
            camera.Position = Vector2.Zero;
            camera.Update(1/60f);
            _scene.ActiveCameras.Add(camera);

            // Load Materials
            foreach (Material _material in _scene.Materials)
            {
                _material.Texture = _scene._content.Load<Texture2D>(_material.Filename.Substring(0, _material.Filename.Length - 4));
                _material.Scope = AssetScope.Local;
            }
            // Load Fonts
            foreach (IceFont _font in _scene.Fonts)
            {
                _font.Font = _scene._content.Load<SpriteFont>(_font.Filename.Replace(".spritefont", ""));
            }
			#if !XNATOUCH
            foreach (IceEffect _effect in _scene.Effects)
            {
                if (_effect.Effects != null)
                {
                    continue;
                }
                // removing ".fx" from the name
                string name = _effect.Filename.Substring(0, _effect.Filename.Length - 3);
                _effect.Load(GlobalDataHolder.ContentManager, new string[] { name });
            }
#endif
            //Load Scene Components
            foreach (IceSceneComponent _comp in _scene.SceneComponents)
            {
                _comp.SetOwner(_scene);
                _comp.OnRegister();
            }
            //Register Scene Items 
            for (int i = 0; i < _scene.SceneItems.Count; i++)
            {
                SceneItem item = _scene.SceneItems[i];
                item.SceneParent = _scene;
                if (item.IsRegistered == false)
                {
                    item.OnRegister();
                }
            }
        }

        public static IceScene AddBlankScene()
        {
            IceScene _newScene = new IceScene();
            Camera _camera = new Camera();
            _newScene.ActiveCameras.Add(_camera);

            Scenes.Add(_newScene);
            _newScene._content = new ContentManager(Game.Instance.Services);
            if (_activeScene == null)
            {
                _activeScene = _newScene;
            }
            return _newScene;
        }

        public static void InitializeGlobal()
        {
            InitializeGlobal(IceCream.Game.Instance.Services);
        }

        public static void LoadGlobalData(string rootPath)
        {
            String filename = Path.Combine(rootPath, "Content/global.ice");
            SceneSerializer._rootPath = rootPath;
            if (File.Exists(filename))
            {
                Serialization.SceneSerializer.DeSerializeScene(filename, SceneManager.GlobalDataHolder);
            }
        }

        public static void InitializeGlobal(IServiceProvider services)
        {
            InitializeGlobal(services, true);
        }

        public static void InitializeEmbedded(IServiceProvider services, bool loadMaterials)
        {
#if XBOX360
            _globalcontent = new ResourceContentManager(services,XBOXIceResources.ResourceManager); //Add root directory ?
#else
			#if !XNATOUCH
            _globalcontent = new ResourceContentManager(services, WINIceCreamResources.ResourceManager); //Add root directory ?
#endif
#endif

			#if !XNATOUCH
            String[] bloomShaders = { "BloomExtractEffect", "GaussianBlurEffect", "BloomCombineEffect" };
            DrawingManager.EmbeddedIceEffects[(int)EmbeddedIceEffectType.Bloom]
                .Load(SceneManager.GlobalContent, bloomShaders);
            String[] gaussianShaders = { "GaussianBlurEffect" };
            DrawingManager.EmbeddedIceEffects[(int)EmbeddedIceEffectType.GaussianBlur]
                .Load(SceneManager.GlobalContent, gaussianShaders);
            DrawingManager.RefractionLayerEffect = _globalcontent.Load<Effect>("RefractionLayerEffect");
			
            Texture2D _pixelTexture = new Texture2D(DrawingManager.GraphicsDevice, 
                1, 1, 0, TextureUsage.None, SurfaceFormat.Color);
            Color[] pixelColor = new Color[1];
            pixelColor[0] = Color.White;
            _pixelTexture.SetData<Color>(pixelColor);
            EmbeddedMaterials.Add(new Material("Pixel", _pixelTexture, AssetScope.Embedded));
            EmbeddedMaterials.Add(new Material("DefaultParticleMaterial",
                _globalcontent.Load<Texture2D>("DefaultParticleTexture"), AssetScope.Embedded));
            Material defaultTileGridMat = new Material("DefaultTileGridMaterial",
                _globalcontent.Load<Texture2D>("DefaultTileGridTexture"), AssetScope.Embedded);            
            // load default areas of the texture            
            defaultTileGridMat.Areas.Add("1", new Rectangle(0, 0, 32, 32));
            defaultTileGridMat.Areas.Add("2", new Rectangle(34, 0, 32, 32));
            defaultTileGridMat.Areas.Add("3", new Rectangle(0, 34, 32, 32));
            defaultTileGridMat.Areas.Add("4", new Rectangle(34, 34, 32, 32));
            EmbeddedMaterials.Add(defaultTileGridMat);
            EmbeddedMaterials.Add(new Material("BlankMaterial",
                _globalcontent.Load<Texture2D>("blank"), AssetScope.Embedded));

            EmbeddedFonts.Add(new IceFont("DefaultFont",
                _globalcontent.Load<SpriteFont>("Arial"), AssetScope.Embedded));

            EmbeddedFonts.Add(new IceFont("DiagnosticFont",
                _globalcontent.Load<SpriteFont>("diagnosticFont"), AssetScope.Embedded));
			#else
			Texture2D _pixelTexture = Texture2D.FromFile(DrawingManager.GraphicsDevice, "Content/pixel.png");
			EmbeddedMaterials.Add(new Material("Pixel", _pixelTexture, AssetScope.Embedded));
			#endif
        }

        public static void InitializeGlobal(IServiceProvider services, bool loadMaterials)
        {
            GlobalDataHolder._content = new ContentManager(services);
            if (GlobalDataHolder.NativeResolution == Point.Zero)
            {
                GlobalDataHolder.NativeResolution = new Point(800, 600);
            }
            if (loadMaterials == true)
            {
                foreach (Material _mat in GlobalDataHolder.Materials)
                {
                    _mat.Scope = AssetScope.Global;
                    if (_mat.Texture != null)
                    {
                        continue;
                    }
                    String name = _mat.Filename.Substring(0, _mat.Filename.Length - 4);
                    _mat.Texture = GlobalDataHolder.ContentManager.Load<Texture2D>(name);
                }

                foreach (IceFont _mat in GlobalDataHolder.Fonts)
                {
                    _mat.Scope = AssetScope.Global;
                    if (_mat.Font != null)
                    {
                        continue;
                    }
                    String name = _mat.Filename.Substring(0, _mat.Filename.LastIndexOf("."));
                    _mat.Font = GlobalDataHolder.ContentManager.Load<SpriteFont>(name.Replace(".spritefont", "").Replace(".png", ""));
                }
				#if !XNATOUCH
                foreach (IceEffect _effect in GlobalDataHolder.Effects)
                {
                    _effect.Scope = AssetScope.Global;
                    if (_effect.Effects != null)
                    {
                        continue;
                    }
                    String name = _effect.Filename.Substring(0, _effect.Filename.Length - 3);
                    _effect.Load(GlobalDataHolder.ContentManager, new String[] { name });
                }
#endif
            }
        }

        public static Material GetEmbeddedMaterial(string name)
        {
            foreach (Material _material in EmbeddedMaterials)
            {
                if (_material.Name == name)
                {
                    return _material;
                }
            }
            return null;
        }

        public static IceFont GetEmbeddedFont(string name)
        {
            foreach (IceFont _font in EmbeddedFonts)
            {
                if (_font.Name == name)
                {
                    return _font;
                }
            }
            return null;
        }

        public static Material GetEmbeddedParticleMaterial()
        {
            foreach (Material _material in EmbeddedMaterials)
            {
                if (_material.Name == "DefaultParticleMaterial")
                {
                    return _material;
                }
            }
            return null;
        }

        public static Material GetEmbeddedTileGridMaterial()
        {
            foreach (Material _material in EmbeddedMaterials)
            {
                if (_material.Name == "DefaultTileGridMaterial")
                {
                    return _material;
                }
            }
            return null;
        }

        public static Material GetEmbeddedPixelMaterial()
        {
            foreach (Material _material in EmbeddedMaterials)
            {
                if (_material.Name == "Pixel")
                {
                    return _material;
                }
            }
            return null;
        }

        #endregion

        #region Networking
		#if !XNATOUCH
        static void networkSession_GamerJoined(object sender, GamerJoinedEventArgs e)
        {
            GamerJoined(sender, e);
        }
        public static void BeginCreateNetworkSession(NetworkSessionType networkSessionType, int maxLocalPlayers, int maxTotalGamers)
        {
            SceneManager.IsNetworkOwner = true;
            IAsyncResult result = Microsoft.Xna.Framework.Net.NetworkSession.BeginCreate(networkSessionType,
                maxLocalPlayers,
                maxTotalGamers,
                new AsyncCallback(CreateSessionComplete), null);
        }
        public static event EventHandler<GamerJoinedEventArgs> GamerJoined;
        private static void CreateSessionComplete(IAsyncResult result)
        {
            networkSession = NetworkSession.EndCreate(result);
            networkSession.GamerJoined += new EventHandler<GamerJoinedEventArgs>(networkSession_GamerJoined);

        }
        private static void FindSessionsComplete(IAsyncResult result)
        {
            AvailableNetworkSessionCollection sessions = NetworkSession.EndFind(result);
            FindSessionCompleteHandler call = (FindSessionCompleteHandler)result.AsyncState;
            call.Invoke(sessions);
        }
        public static void BeginFindSessions(NetworkSessionType networkSessionType, int p, FindSessionCompleteHandler callback)
        {
            IAsyncResult asyncResult = NetworkSession.BeginFind(networkSessionType,
                                              p,
                                              null, new AsyncCallback(FindSessionsComplete), callback);
        }
#endif
        #endregion
    }

    public delegate void FindSessionCompleteHandler(AvailableNetworkSessionCollection sessions);
}
