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
using System.Text;
#if !REACH
using IceCream.Drawing.IceEffects;
#endif
using IceCream.SceneItems;

namespace IceCream.Drawing
{
    public static class DrawingManager
    {
        private const int NUM_LAYERS = 10;
        private static DrawingLayer[] _layers;
        private static bool[] _ignoreCamera;
        
        private static GraphicsDevice _graphicsDevice;
        private static SpriteBatch _spriteBatch;
        private static DrawingLayerType? _drawingLayerTypeInUse;
		#if !REACH
        private static PrimitiveBatch _primitiveBatch;
        public static PrimitiveBatch PrimitiveBatch
        {
            get { return _primitiveBatch; }
        }
		#endif
        private static bool _enableDebugDrawing = true;
        private static Texture2D _pixelTexture;
		#if !REACH
        public static RenderTargetManager _renderTargetManager;
        public static RenderTargetInstance CurrentRenderTarget;
		private static IceEffect[] _embeddedIceEffects;
        private static NoEffect _noEffect;
		#endif
        private static Point _viewPortSize;        
        private static Effect _refractionLayerEffect;
        private static bool _isRendering;

        private static bool _drawOnRenderTarget = false;

        #region Properties

        public static GraphicsDevice GraphicsDevice
        {
            get { return _graphicsDevice; }
        }

        #if !REACH
        public static IceEffect[] EmbeddedIceEffects
        {
            get { return _embeddedIceEffects; }
        }

        public static RenderTargetManager RenderTargetManager
        {
            get { return _renderTargetManager; }
            set { _renderTargetManager = value; }
        }
        #endif

        public static SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public static Point ViewPortSize
        {
            get { return _viewPortSize; }
            set { _viewPortSize = value; }
        }

        internal static DrawingLayerType? DrawingLayerTypeInUse
        {
            get { return _drawingLayerTypeInUse; }
            set { _drawingLayerTypeInUse = value; }
        }

        internal static Matrix? LastTransformMatrix { get; set; }

        public static Texture2D PixelTexture
        {
            get { return _pixelTexture; }
        }

		#if !REACH
        internal static NoEffect NoEffect
        {
            get { return _noEffect; }
        }
		#endif

        public static Effect RefractionLayerEffect
        {
            get { return _refractionLayerEffect; }
            set { _refractionLayerEffect = value; }
        }

        public static bool IgnoreClearBeforeRendering { get; set; }

        #endregion

        public static void Intialize()
        {
            _layers = new DrawingLayer[NUM_LAYERS];
            _ignoreCamera = new bool[NUM_LAYERS];
            for (int i = 0; i < _layers.Length; i++)
            {
                _layers[i] = new DrawingLayer();                
            }
#if !REACH
            _embeddedIceEffects = new IceEffect[(int)EmbeddedIceEffectType.SizeOfEnum];
            _noEffect = new NoEffect();
#endif
			_isRendering = false;
        }

        public static void LoadContent(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _viewPortSize = new Point(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
            _spriteBatch = new SpriteBatch(graphicsDevice);
			#if !REACH
            _primitiveBatch = new PrimitiveBatch(graphicsDevice);  
            
            _pixelTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] pixelColor = new Color[1];
            pixelColor[0] = Color.White;
            _pixelTexture.SetData<Color>(pixelColor);
			_renderTargetManager = new RenderTargetManager(graphicsDevice);
            CurrentRenderTarget = RenderTargetInstance.BackBuffer;
            _renderTargetManager.LoadContent();
            #else
			_pixelTexture = Texture2D.FromFile(DrawingManager.GraphicsDevice, "Content/pixel.png");
            #endif
            LoadEmbeddedIceEffects();            
        }

        private static void LoadEmbeddedIceEffects()
        {
			#if !REACH
            _embeddedIceEffects[(int)EmbeddedIceEffectType.None]
                = new NoEffect();
            _embeddedIceEffects[(int)EmbeddedIceEffectType.Bloom]
                = new BloomEffect();
            _embeddedIceEffects[(int)EmbeddedIceEffectType.GaussianBlur]
                = new GaussianBlurEffect(); 
			#endif
		}

        public static void RenderScene(Camera camera)
        {

        }

        /// <summary>
        /// Call this method to render all the layers of the scene
        /// </summary>
        public static void RenderScene()
        {
            if (_isRendering == true)
            {
                throw new Exception("RenderScene was previously called but not ended with EndRendering()");
            }            
            _isRendering = true;            

            // Reset the value of the last layer in use
            DrawingLayerTypeInUse = null;
            // Ensure the rendertargets are of correct size
            PresentationParameters pp = _graphicsDevice.PresentationParameters;
			#if !REACH
            _renderTargetManager.BackbufferResized(pp.BackBufferWidth, pp.BackBufferHeight);
            CurrentRenderTarget = RenderTargetInstance.BackBuffer;
            if (_drawOnRenderTarget == true)
            {
                _renderTargetManager.SwitchTo(RenderTargetInstance.SceneRendering);
                CurrentRenderTarget = RenderTargetInstance.SceneRendering;
            }
			#endif
            for (int c = 0; c < SceneManager.ActiveScene.ActiveCameras.Count; c++)
            {
                Camera camera = SceneManager.ActiveScene.ActiveCameras[c];
                Viewport _viewPort = DrawingManager.GraphicsDevice.Viewport;
                _viewPort.X = camera.ViewPortPosition.X;
                _viewPort.Y = camera.ViewPortPosition.Y;
                _viewPort.Width = DrawingManager.ViewPortSize.X;
                _viewPort.Height = DrawingManager.ViewPortSize.Y;
                DrawingManager.GraphicsDevice.Viewport = _viewPort;
                // Clear our viewport
                if (DrawingManager.IgnoreClearBeforeRendering == false)
                {
                    DrawingManager.GraphicsDevice.Clear(SceneManager.ActiveScene.ClearColor);
                }
                // Render the scene one time per active camera (split screen)

                // Render each layer
                for (int i = _layers.Length - 1; i >= 0; i--)
                {
                    if (_layers[i].Visible == true)
                    {
                        Matrix layerMatrix;
                        if (_ignoreCamera[i] == true)
                        {
                            layerMatrix = Matrix.Identity;
                        }
                        else
                        {
                            layerMatrix = camera.GetMatrix(_layers[i].Parallax);
                        }
                        _layers[i].RenderLayer(_graphicsDevice, layerMatrix);
                    }
                }
                // End the sprite batch if needed
                if (DrawingLayerTypeInUse != null)
                {
                    _spriteBatch.End();
                    DrawingManager.DrawingLayerTypeInUse = null;
                    DrawingManager.LastTransformMatrix = null;
                }
				#if !REACH
                // Check for debug shapes to draw
                if (_enableDebugDrawing == true && DebugShapes.LinesList.Count > 0)
                {
                    _primitiveBatch.SetViewPortSize(DrawingManager.ViewPortSize.X, DrawingManager.ViewPortSize.Y);
                    _primitiveBatch.Begin(PrimitiveType.LineList, camera.GetMatrix(DebugShapes.Parallax));
                    foreach (DebugLine line in DebugShapes.LinesList)
                    {
                        _primitiveBatch.AddVertex(line.vertex, line.color);
                    }
                    _primitiveBatch.End();
                }
				#endif
            }
            // End rendering
            _isRendering = false;
            for (int i = _layers.Length - 1; i >= 0; i--)
            {
                _layers[i].EndDrawing();
            }
            DebugShapes.LinesList.Clear();
			#if !REACH
            if (_drawOnRenderTarget == true)
            {
                _drawOnRenderTarget = false;
                DrawRenderTargetContent(RenderTargetInstance.SceneRendering, RenderTargetInstance.BackBuffer, null, 255);
                CurrentRenderTarget = RenderTargetInstance.BackBuffer;
            }
			#endif
        }
		
		#if !REACH
        public static void DrawRenderTargetContent(RenderTargetInstance contentSource, RenderTargetInstance target, Texture2D previousContent, byte? opacity)
        {
            // End the sprite batch if needed
            if (DrawingLayerTypeInUse != null)
            {
                _spriteBatch.End();
                DrawingLayerTypeInUse = null;
            }
            Color color = Color.White;
            if (opacity.HasValue)
            {
                color.A = opacity.Value;
            }
            DrawingManager.RenderTargetManager.SwitchTo(target);
            GraphicsDevice.Clear(new Color(0, 0, 0, 0));
            Texture2D renderTexture = _renderTargetManager.GetRenderTarget2D(contentSource);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            if (previousContent != null)
            {
                //DrawingManager.RenderTargetManager.SaveTexture(RenderTargetInstance.OwnLayerRendering, "E:/own.png");
                _spriteBatch.Draw(previousContent, Vector2.Zero, Color.White);
            }
            _spriteBatch.Draw(renderTexture, Vector2.Zero, color);
            _spriteBatch.End();    
        }
		#endif

        public static void DrawFilledRectangle(int layer, Vector2 position, Vector2 size, Color color)
        {
            DrawRequest drawRequest = new DrawRequest(_pixelTexture, position, null, 0,
                size, Vector2.Zero, false, color, false, false, null);
            DrawOnLayer(drawRequest, layer, DrawingBlendingType.Alpha);
        }

        public static void DrawFilledRectangle(int layer, Vector2 position, Vector2 size, Color color, DrawingBlendingType blendingType)
        {
            DrawRequest drawRequest = new DrawRequest(_pixelTexture, position, null, 0,
                size, Vector2.Zero, false, color, false, false, null);
            DrawOnLayer(drawRequest, layer, blendingType);
        }

        public static void DrawPolygon(int layer, Vector2[] vertices, float size, Color color)
        {
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                DrawLine(layer, vertices[i], vertices[i + 1], size, color);           
            }
        }

        public static void DrawLine(int layer, Vector2 start, Vector2 end, float size, Color color)
        {
            Vector2 vector = end - start;
            float lenght = vector.Length();
            Vector2 scale = new Vector2(lenght, size);
            float angle = vector.Angle();
            DrawRequest drawRequest = new DrawRequest(_pixelTexture, start, null, angle,
                scale, new Vector2(0f, 0.5f), false, color, false, false, null);
            DrawOnLayer(drawRequest, layer, DrawingBlendingType.Alpha);
        }

        /// <summary>
        /// Draw on the requested layer
        /// </summary>
        /// <param name="drawRequest">The drawRequest to render</param>
        /// <param name="layer">The desired layer</param>
        public static void DrawOnLayer(DrawRequest drawRequest, int layer, DrawingBlendingType blendingType)
        {
            _layers[layer - 1].Draw(drawRequest, blendingType);
        }

		#if !XNATOUCH
        /// <summary>
        /// Apply the specified Post Process effect on the requested layer after rendering it
        /// </summary>
        public static void ApplyPostProcess(PostProcessRequest postProcessRequest, int layer)
        {
            if (postProcessRequest.Layer.HasValue == false)
            {
                _drawOnRenderTarget = true;
            }
            _layers[layer - 1].ApplyPostProcess(postProcessRequest);
        }
        #endif

        public static void SetLayerParallax(int layer, Vector3 parallax)
        {
            _layers[layer - 1].Parallax = parallax;
        }

        public static void SetLayerIgnoreCamera(int layer, bool ignoreCamera)
        {
            _ignoreCamera[layer - 1] = ignoreCamera;
        }

        public static void SetLayerVisibility(int layer, bool visible)
        {
            _layers[layer - 1].Visible = visible;
        }
    }
}
