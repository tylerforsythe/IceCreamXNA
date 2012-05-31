#if !REACH
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



using System;
using System.Collections.Generic;
using System.Text;

namespace IceCream.Drawing
{
    public enum RenderTargetInstance
    {
        BackBuffer,
        SceneRendering,
        OwnLayerRendering,
        EffectA,
        EffectB,
    }

    public class RenderTargetManager
    {
        #region Fields

        private GraphicsDevice _graphicsDevice;
        private static RenderTarget2D _renderTargetSceneRendering;
        private static RenderTarget2D _renderTargetOwnLayerRendering;
        private static RenderTarget2D _renderTargetB;
        private static RenderTarget2D _renderTargetC;

        #endregion

        #region Properties

        public GraphicsDevice GraphicsDevice
        {
            get { return _graphicsDevice; }
            set { _graphicsDevice = value; }
        }

        #endregion

        #region Constructor

        public RenderTargetManager()
        {

        }

        public RenderTargetManager(GraphicsDevice graphicsDevice)
        {
            this._graphicsDevice = graphicsDevice;
        }

        #endregion

        #region Methods

        public void LoadContent()
        {
            PresentationParameters pp = _graphicsDevice.PresentationParameters;
            //SurfaceFormat surfaceFormat = pp.BackBufferFormat;
            //DepthFormat depthFormat = pp.DepthStencilFormat;        
            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;
            _renderTargetSceneRendering = new RenderTarget2D(_graphicsDevice, width,  height, false, SurfaceFormat.Color, DepthFormat.None);
            _renderTargetOwnLayerRendering = new RenderTarget2D(_graphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);
            _renderTargetB = new RenderTarget2D(_graphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);
            _renderTargetC = new RenderTarget2D(_graphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);
        }

        public void UnloadContent()
        {
            _renderTargetSceneRendering.Dispose();
            _renderTargetB.Dispose();           
        }

        /// <summary>
        /// Recreates the necessary dependencies if the device was resized
        /// </summary>
        public void BackbufferResized(int newWidth, int newHeight)
        {
            if (_renderTargetSceneRendering != null && (_renderTargetSceneRendering.Width != newWidth
                || _renderTargetSceneRendering.Height != newHeight))
            {
                UnloadContent();
                LoadContent();
            }
        }

        public RenderTarget2D GetRenderTarget2D(RenderTargetInstance renderTarget)
        {
            switch (renderTarget)
            {
                case RenderTargetInstance.SceneRendering:
                    return _renderTargetSceneRendering;
                case RenderTargetInstance.OwnLayerRendering:
                    return _renderTargetOwnLayerRendering;
                case RenderTargetInstance.EffectA:
                    return _renderTargetB;
                case RenderTargetInstance.EffectB:
                    return _renderTargetC;
                default:
                    return null;
            }
        }

        public void SwitchTo(RenderTargetInstance renderTarget)
        {
            RenderTarget2D rt = GetRenderTarget2D(renderTarget);
            _graphicsDevice.SetRenderTarget(rt);
        }

        public void SaveTexture(RenderTargetInstance renderTarget, String filePath)
        {
            Texture2D bufferTexture = GetRenderTarget2D(renderTarget);
            using (System.IO.Stream stream = System.IO.File.OpenWrite(filePath))
            {
                bufferTexture.SaveAsPng(stream, bufferTexture.Width, bufferTexture.Height);
            }
        }

        #endregion

    }
}
#endif