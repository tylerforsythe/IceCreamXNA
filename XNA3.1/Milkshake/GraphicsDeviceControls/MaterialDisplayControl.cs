#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using IceCream;
using IceCream.Drawing;
#endregion

namespace Milkshake.GraphicsDeviceControls
{
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, which allows it to
    /// render using a GraphicsDevice. This control shows how to use ContentManager
    /// inside a WinForms application. It loads a SpriteFont object through the
    /// ContentManager, then uses a SpriteBatch to draw text. The control is not
    /// animated, so it only redraws itself in response to WinForms paint messages.
    /// </summary>
    class MaterialDisplayControl : GraphicsDeviceControl
    {
        // Texture
        private Texture2D _texture;
        private bool _stretchTexture;
        SpriteBatch _spriteBatch;
        private Texture2D _checkerTexture;
        private Texture2D _crossTexture;
        DrawRequest _drawRequest;

        public Texture2D Texture
        {
            get { return _texture; }
            set 
            { 
                _texture = value;
                _drawRequest.texture = value;
            }
        }

        public bool StretchTexture
        {
            get { return _stretchTexture; }
            set
            {
                _stretchTexture = value;                
            }
        }

        /// <summary>
        /// Initializes the control, creating the ContentManager
        /// and using it to load a SpriteFont.
        /// </summary>
        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);            
            _checkerTexture = Texture2D.FromFile(GraphicsDevice, Application.StartupPath + "\\Resources\\checker.png");
            _crossTexture = Texture2D.FromFile(GraphicsDevice, Application.StartupPath + "\\Resources\\cross.png");
            _stretchTexture = false;
            this.VScroll = true;
            this.HScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(0, 0);            

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        public void LoadTexture(string path)
        {
            _texture = Texture2D.FromFile(GraphicsDevice, path);
            _drawRequest = new DrawRequest(_texture, Vector2.Zero,false, null, 0, Vector2.One, Vector2.Zero, false, Color.White, false, false, null);
        }

        public void UnloadTexture()
        {
            if (_texture != null)
            {
                _texture.Dispose();
            }
        }

        /// <summary>
        /// Get the WndProc messages to prevent flickering when scrolling
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {            
            // WM_HSCROLL or WM_VSCROLL
            if (m.Msg == 0x114 || m.Msg == 0x115)
            {                
                this.Invalidate();
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Draws the control, using SpriteBatch and SpriteFont.
        /// </summary>
        protected override void Draw()
        {            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Use SpriteSortMode.Immediate, so we can apply custom renderstates.
            _spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                              SpriteSortMode.Immediate,
                              SaveStateMode.None);
            // Set the texture addressing mode to wrap, so we can repeat
            // many copies of our tiled checkerboard texture.
            GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
            Rectangle fullRect = new Rectangle(0, 0, this.Width, this.Height);
            // Draw a tiled checkerboard pattern in the background.
            _spriteBatch.Draw(_crossTexture, fullRect, fullRect, Color.White);

            if (_texture == null)
            {
                _spriteBatch.End();   
            }
            else
            {
                Rectangle textureRect = new Rectangle(0, 0, _texture.Width, _texture.Height);
                Rectangle textureSizeRect = new Rectangle(0, 0, _texture.Width, _texture.Height);
                
                if (_stretchTexture)
                {
                    Vector2 targetSize = new Vector2();
                    float textureRatio = (float)_texture.Width / (float)_texture.Height;
                    // if the viewport is wide or square
                    if (GraphicsDevice.Viewport.AspectRatio >= 1)
                    {
                        // if the texture is wide or square
                        if (textureRatio > 1)
                        {
                            targetSize.X = (float)GraphicsDevice.Viewport.Width;
                            targetSize.Y = (float)targetSize.X / textureRatio;
                        }
                        else
                        {
                            targetSize.Y = (float)GraphicsDevice.Viewport.Height;
                            targetSize.X = (float)targetSize.Y * textureRatio;
                        }
                    }
                    else
                    {
                        // if the texture is tall
                        if (textureRatio <= 1)
                        {
                            targetSize.X = (float)GraphicsDevice.Viewport.Width;
                            targetSize.Y = (float)targetSize.X / textureRatio;
                        }
                        else
                        {
                            targetSize.Y = (float)GraphicsDevice.Viewport.Height;
                            targetSize.X = (float)targetSize.Y * textureRatio;
                        }
                    }
                    // disable autoscroll
                    this.AutoScroll = false;
                    this.AutoScrollMinSize = new System.Drawing.Size(0, 0);
                    _drawRequest.scaleRatio = new Vector2((float)targetSize.X / (float)_texture.Width,
                        (float)targetSize.Y / (float)_texture.Height);
                    _drawRequest.position = new Vector2(GraphicsDevice.Viewport.Width / 2.0f, GraphicsDevice.Viewport.Height / 2.0f);
                    _drawRequest.pivot = new Vector2(_texture.Width / 2.0f, _texture.Height / 2.0f);
                    textureSizeRect.Width = (int)(textureSizeRect.Width * _drawRequest.scaleRatio.X);
                    textureSizeRect.Height = (int)(textureSizeRect.Height * _drawRequest.scaleRatio.Y);
                    textureRect = new Rectangle((int)_drawRequest.position.X - (int)(_texture.Width * _drawRequest.scaleRatio.X) / 2,
                        (int)_drawRequest.position.Y - (int)(_texture.Height * _drawRequest.scaleRatio.Y) / 2,
                        (int)(_texture.Width * _drawRequest.scaleRatio.X),
                        (int)(_texture.Height * _drawRequest.scaleRatio.Y));
                }
                else
                {
                    _drawRequest.scaleRatio = Vector2.One;
                    // check the scrolling
                    this.AutoScrollMinSize = new System.Drawing.Size(_texture.Width, _texture.Height);
                    Vector2 deltaPos = new Vector2((GraphicsDevice.Viewport.Width - _texture.Width) / 2.0f, (GraphicsDevice.Viewport.Height - _texture.Height) / 2.0f);
                    if (GraphicsDevice.Viewport.Width > _texture.Width)
                    {
                        deltaPos.X = 0;
                    }
                    if (GraphicsDevice.Viewport.Height > _texture.Height)
                    {
                        deltaPos.Y = 0;
                    }
                    deltaPos.X -= this.AutoScrollPosition.X;
                    deltaPos.Y -= this.AutoScrollPosition.Y;
                    _drawRequest.position = new Vector2(GraphicsDevice.Viewport.Width / 2.0f, GraphicsDevice.Viewport.Height / 2.0f) - deltaPos;
                    _drawRequest.pivot = new Vector2(_texture.Width / 2.0f, _texture.Height / 2.0f);
                    textureRect.X = (int)(_drawRequest.position.X - _drawRequest.pivot.X);
                    textureRect.Y = (int)(_drawRequest.position.Y - _drawRequest.pivot.Y);
                }
                // Draw a tiled checkerboard pattern in the background.
                _spriteBatch.Draw(_checkerTexture, textureRect, textureSizeRect, Color.White);
                _spriteBatch.End();
                _spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                              SpriteSortMode.Immediate,
                              SaveStateMode.None);
                _spriteBatch.Draw(_drawRequest.texture, _drawRequest.position, _drawRequest.sourceRectangle,
                            Color.White, 0, _drawRequest.pivot, _drawRequest.scaleRatio,
                            (_drawRequest.hFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None)
                            | (_drawRequest.vFlip ? SpriteEffects.FlipVertically : SpriteEffects.None), 0);
                _spriteBatch.End();
            }
        }
    }
}