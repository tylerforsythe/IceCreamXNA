using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IceCream;
using IceCream.Drawing;

namespace Milkshake.GraphicsDeviceControls
{
    using Color = Microsoft.Xna.Framework.Color;
    using IceCream.SceneItems; 
  
    public partial class SceneItemPreviewControl : GraphicsDeviceControl
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _checkerTexture;
        private Texture2D _crossTexture;
        private Camera _camera;
        internal SceneItem SceneItem
        {
            get;
            set;
        }

        internal Camera Camera
        {
            get { return _camera; }
            set { _camera = value; }
        }


        public SceneItemPreviewControl()
        {
            _camera = new Camera();
            _camera.Position = Vector2.Zero;
            _camera.Pivot = new Vector2(0.5f);
        }

        // Timer controls the update.
        
        
        protected override void Initialize()
        {                       
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _checkerTexture = Texture2D.FromStream(GraphicsDevice, System.IO.File.OpenRead(Application.StartupPath + "\\Resources\\checker.png"));
            _crossTexture = Texture2D.FromStream(GraphicsDevice, System.IO.File.OpenRead(Application.StartupPath + "\\Resources\\cross.png"));
            
            this.VScroll = true;
            this.HScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(0, 0);
        }

        protected override void Draw()
        {            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Use SpriteSortMode.Immediate, so we can apply custom renderstates.
            // Set the texture addressing mode to wrap, so we can repeat
            // many copies of our tiled checkerboard texture.
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
            Rectangle fullRect = new Rectangle(0, 0, this.Width, this.Height);
            // Draw a tiled checkerboard pattern in the background.
            _spriteBatch.Draw(_crossTexture, fullRect, fullRect, Color.White);
            if (SceneItem == null)
            {
                _spriteBatch.End();
            }
            else
            {
                _spriteBatch.End();                
                DrawingManager.ViewPortSize = new Point(this.Width, this.Height);
                _camera.Position = Vector2.Zero;
                _camera.Update(1 / 60f);
                Vector2 oldPivot = SceneItem.Pivot;
                bool oldIsPivotRelative = SceneItem.IsPivotRelative;
                Vector2 oldPosition = SceneItem.Position;
                float oldRotation = SceneItem.Rotation;
                Vector2 oldScale = SceneItem.Scale;
                bool oldVisibility = SceneItem.Visible;
                SceneItem.Visible = true;
                SceneItem.Position = Vector2.Zero;                
                SceneItem.Pivot = new Vector2(0.5f);
                SceneItem.IsPivotRelative = true;
                SceneItem.Scale = Vector2.One;
                SceneItem.Rotation = 0;
                SceneItem.Update(1 / 60f);
                SceneItem.Draw(1 / 60f);
                SceneItem.Position = oldPosition;
                SceneItem.Pivot = oldPivot;
                SceneItem.IsPivotRelative = oldIsPivotRelative;
                SceneItem.Scale = oldScale;
                SceneItem.Rotation = oldRotation;
                SceneItem.Visible = oldVisibility;
                MilkshakeForm.SwapCameraAndRenderScene(_camera);
            }           
        }

    }
}
