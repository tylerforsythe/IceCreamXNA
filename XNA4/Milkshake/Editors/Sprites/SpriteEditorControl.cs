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
using Milkshake.GraphicsDeviceControls;
using IceCream.SceneItems;

#endregion

namespace Milkshake.Editors.Sprites
{
    public enum SpriteEditorSelectionMode
    {
        Normal,
        SelectingTile,
        Tiled,
    }

    public class SpriteEditorControl : GraphicsDeviceControl
    {
        private SpriteEditor _parent;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private Texture2D _checkerTexture;
        private Texture2D _crossTexture;
        private Sprite _sprite = new Sprite();
        private float _zoom;
        private SpriteEditorSelectionMode _selectionMode;
        private Dictionary<String, Rectangle> _spriteRectangles;
        private String _selectedRectangle;
        private int _alphaCounter;
        private bool _alphaGoingUp;

        #region Properties 

        internal SpriteEditor ParentEditor
        {
            get { return _parent; }
            set { _parent = value; }
        }
        
        internal bool ShowWholeImage { get; set; }
        internal Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }
        
        internal Sprite Sprite
        {
            get { return _sprite; }
            set { _sprite = value; }
        }
        
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }

        public SpriteEditorSelectionMode SelectionMode
        {
            get { return _selectionMode; }
            set { _selectionMode = value; }
        }
   
        public Dictionary<String, Rectangle> SpriteRectangles
        {
            get { return _spriteRectangles; }
            set { _spriteRectangles = value; }
        }
      
        public String SelectedRectangle
        {
            get { return _selectedRectangle; }
            set { _selectedRectangle = value; }
        }

        #endregion

        protected override void Initialize()
        {            
            _spriteBatch = new SpriteBatch(GraphicsDevice);                       
            _checkerTexture = Texture2D.FromStream(GraphicsDevice, System.IO.File.OpenRead(Application.StartupPath + "\\Resources\\checker.png"));
            _crossTexture = Texture2D.FromStream(GraphicsDevice, System.IO.File.OpenRead(Application.StartupPath + "\\Resources\\cross.png"));
            _zoom = 1;
            _alphaCounter = 0;
            _alphaGoingUp = true;
            _selectionMode = SpriteEditorSelectionMode.Normal;
            this.AutoScroll = true;
        }
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }
        /// <summary>
        /// Draws the control, using SpriteBatch and SpriteFont.
        /// </summary>
        protected override void Draw()
        {            
            ParentEditor.ZoomBox.Camera.Update(1/60f);
            GraphicsDevice.Clear(Color.RoyalBlue);
            
            // Use SpriteSortMode.Immediate, so we can apply custom renderstates.
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
            _sprite.Pivot = Vector2.Zero;
            _sprite.Position = Vector2.Zero;
            Rectangle fullRect = new Rectangle(0, 0, this.Width, this.Height);
            Rectangle spriteRect = new Rectangle(0, 0, _sprite.Material.Texture.Width, _sprite.Material.Texture.Height); ;
            Point textureSize = new Point(_sprite.Material.Texture.Width, _sprite.Material.Texture.Height);
            if (!ShowWholeImage)
            {
                if (_sprite.SourceRectangle != null)
                {
                    Rectangle rect = _sprite.SourceRectangle.Value;
                    spriteRect = new Rectangle(0, 0, rect.Width, rect.Height);
                    textureSize = new Point(rect.Width, rect.Height);
                }
            }
            ParentEditor.ZoomBox.Camera.Position = new Vector2(-this.AutoScrollPosition.X, 
                -this.AutoScrollPosition.Y);
            // Draw a tiled checkerboard pattern in the background.
            _spriteBatch.Draw(_crossTexture, fullRect, fullRect, Color.White);
            _spriteBatch.Draw(_checkerTexture,
                new Rectangle(0, 0, (int)(spriteRect.Width * _sprite.Scale.X * ParentEditor.ZoomBox.ZoomFactor),
                    (int)(spriteRect.Height * _sprite.Scale.Y * ParentEditor.ZoomBox.ZoomFactor)),
                new Rectangle(0, 0, (int)(spriteRect.Width * _sprite.Scale.X * ParentEditor.ZoomBox.ZoomFactor),
                (int)(spriteRect.Height * _sprite.Scale.Y * ParentEditor.ZoomBox.ZoomFactor)), Color.White);         

            _spriteBatch.End();

            this.AutoScrollMinSize = new System.Drawing.Size((int)(textureSize.X * _sprite.Scale.X * ParentEditor.ZoomBox.ZoomFactor),
                (int)(textureSize.Y * _sprite.Scale.Y * ParentEditor.ZoomBox.ZoomFactor));

            if (ShowWholeImage)
            {
                Rectangle? _srcRectangle = _sprite.SourceRectangle;
                _sprite.SourceRectangle = null;
                _sprite.Draw(1f);
                _sprite.SourceRectangle = _srcRectangle;

                if (_sprite.SourceRectangle.HasValue)
                {
                    IceCream.Drawing.DebugShapes.DrawRectangle(_sprite.SourceRectangle.Value, Color.Yellow);
                }
            }
            else
            {
                _sprite.Draw(1f);
            }
            // if selection a rectangle, draw a fading selection
            if (_selectionMode == SpriteEditorSelectionMode.SelectingTile)
            {               
                Rectangle hRect = _spriteRectangles[_selectedRectangle];
                Color rectColor = new Color(255, 255, 255, (byte)_alphaCounter);
                Color highlightRectColor = new Color(155, 225, 255, (byte)_alphaCounter);
                foreach (var rect in this.Sprite.Material.Areas)
                {
                    Color color = rectColor;
                    if (rect.Key == _selectedRectangle)
                    {
                        color = highlightRectColor;
                    }
                    DrawingManager.DrawFilledRectangle(1, new Vector2(rect.Value.X, rect.Value.Y),
                        new Vector2(rect.Value.Width, rect.Value.Height), color, DrawingBlendingType.Alpha);
                }
                if (_alphaGoingUp)
                {
                    _alphaCounter += 8;
                    if (_alphaCounter > 190)
                    {
                        _alphaCounter = 190;
                        _alphaGoingUp = false;
                    }
                }
                else 
                {
                    _alphaCounter -= 8;
                    if (_alphaCounter < 40)
                    {
                        _alphaCounter = 40;
                        _alphaGoingUp = true;
                    }
                }
            }

            DrawingManager.ViewPortSize = new Point(this.Width, this.Height);
            MilkshakeForm.SwapCameraAndRenderScene(ParentEditor.ZoomBox.Camera);
            _parent.Update(1 / 60f);
        }

        public SpriteEditorControl()
        {
            _sprite = new Sprite();
        }


        /// <summary>
        /// Called by the parent form when the mouse is moving on the control
        /// </summary>
        /// <param name="mousePos">The mouse position relative to the control</param>
        public void CheckMousePosition(Point mousePos)
        {
            if (_selectionMode == SpriteEditorSelectionMode.SelectingTile)
            {                
                Vector2 translatedPos = ParentEditor.ZoomBox.Camera
                    .ConvertToWorldPos(new Vector2(mousePos.X, mousePos.Y));
                foreach (var rect in this.SpriteRectangles)
                {
                    Rectangle translatedRect = rect.Value;              
                    if (IceMath.IsPointInsideRectangle(new Point((int)translatedPos.X, (int)translatedPos.Y),
                        translatedRect))
                    {
                        _selectedRectangle = rect.Key;                  
                    }                   
                }
            }
        }

        public void MouseClicked(Point mousePos)
        {
            if (_selectionMode == SpriteEditorSelectionMode.SelectingTile)
            {
                Vector2 translatedPos = ParentEditor.ZoomBox.Camera
                    .ConvertToWorldPos(new Vector2(mousePos.X, mousePos.Y));
                if (IceMath.IsPointInsideRectangle(new Point((int)translatedPos.X, (int)translatedPos.Y)
                    , _spriteRectangles[_selectedRectangle]))
                {
                    _selectionMode = SpriteEditorSelectionMode.Tiled;
                    Rectangle spriteRect = _spriteRectangles[_selectedRectangle];
                    Sprite.MaterialArea = _selectedRectangle;
                    this.ParentEditor.RefreshAreaCombo();
                }
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
    }
}
