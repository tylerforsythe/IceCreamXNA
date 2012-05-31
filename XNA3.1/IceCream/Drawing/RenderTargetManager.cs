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

using System;
using System.Collections.Generic;
using System.Text;

namespace IceCream.Drawing
{
    public enum RenderTargetInstance
    {
        BackBuffer,
        A,
        B,
    }

    public class RenderTargetManager
    {
        #region Fields

        private GraphicsDevice _graphicsDevice;
        private static RenderTarget2D _renderTargetA;
        private static RenderTarget2D _renderTargetB;
        private static ResolveTexture2D _backbufferTexture;
        private static Texture2D _resolveTextureA;
        private static Texture2D _resolveTextureB;

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
            SurfaceFormat surfaceFormat = pp.BackBufferFormat;
            int mipmapLevel = 1;
            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;
            _renderTargetA = new RenderTarget2D(_graphicsDevice, width,
                    height, mipmapLevel, surfaceFormat);
            _renderTargetB = new RenderTarget2D(_graphicsDevice, width,
                height, mipmapLevel, surfaceFormat);
            _backbufferTexture = new ResolveTexture2D(_graphicsDevice, width,
                height, mipmapLevel, surfaceFormat);
        }

        public void UnloadContent()
        {
            _renderTargetA.Dispose();
            _renderTargetB.Dispose();
            if (_resolveTextureA != null)
            {
                _resolveTextureA.Dispose();
            }
            if (_resolveTextureB != null)
            {
                _resolveTextureB.Dispose();
            }
            _backbufferTexture.Dispose();
        }

        /// <summary>
        /// Recreates the necessary dependencies if the device was resized
        /// </summary>
        public void BackbufferResized(int newWidth, int newHeight)
        {
            if (_backbufferTexture != null && (_backbufferTexture.Width != newWidth 
                || _backbufferTexture.Height != newHeight))
            {
                UnloadContent();
                LoadContent();
            }
        }

        public void ResolveToTexture(RenderTargetInstance renderTarget)
        {
            if (_backbufferTexture != null && (_backbufferTexture.Width != 
                GraphicsDevice.PresentationParameters.BackBufferWidth || 
                _backbufferTexture.Height != GraphicsDevice.PresentationParameters.BackBufferHeight))
            {
                UnloadContent();
                LoadContent();
            }
            if (renderTarget == RenderTargetInstance.A)
            {
                _resolveTextureA = _renderTargetA.GetTexture();
            }
            else if (renderTarget == RenderTargetInstance.B)
            {
                _resolveTextureB = _renderTargetB.GetTexture();
            }
            else
            {
                _graphicsDevice.ResolveBackBuffer(_backbufferTexture);
            }
        }

        public Texture2D GetTexture(RenderTargetInstance renderTarget)
        {
            if (renderTarget == RenderTargetInstance.A)
            {
                return _resolveTextureA;
            }
            else if (renderTarget == RenderTargetInstance.B)
            {
                return _resolveTextureB;
            }
            else
            {
                return (Texture2D)_backbufferTexture;
            }
        }

        public void SwitchTo(RenderTargetInstance renderTarget)
        {
            if (renderTarget == RenderTargetInstance.A)
            {
                _graphicsDevice.SetRenderTarget(0, _renderTargetA);
            }
            else if (renderTarget == RenderTargetInstance.B)
            {
                _graphicsDevice.SetRenderTarget(0, _renderTargetB);
            }
            else
            {
                _graphicsDevice.SetRenderTarget(0, null);
            }
        }

        #endregion

    }
}
#endif