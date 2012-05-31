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
using IceCream.Drawing;

namespace IceCream.SceneItems
{
    public class TextItem : SceneItem
    {
        #region Private Fields

        private DrawRequest _drawRequest;     
        private DrawingBlendingType _blendingType;
        private string _text = "Text Item";
        private IceFont _font;
        private bool _autoCenterPivot;
        private byte _opacity;

        #endregion

        #region Constructor

        public TextItem()
        {
            _blendingType = DrawingBlendingType.Alpha;           
            this.Tint = Color.White;
        }

        #endregion

        #region Public Methods

        public virtual Color Tint
        {
            get { return _drawRequest.tint; }
            set
            {
                _drawRequest.tint = value;
                _drawRequest.tint.A = _opacity;
            }
        }

        public override byte Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                _opacity = value;
                _drawRequest.tint.A = _opacity;
            }
        }

        public DrawingBlendingType BlendingType
        {
            get { return _blendingType; }
            set { _blendingType = value; }
        }

        public bool AutoCenterPivot
        {
            get { return _autoCenterPivot; }
            set { _autoCenterPivot = value; }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                if (_autoCenterPivot && Font != null && Font.Font != null)
                    Pivot = _font.Font.MeasureString(_text) / 2;

                UpdateBoundingRect();
            }
        }
        public bool Shadow { get; set; }

        public IceFont Font
        {
            get { return _font; }
            set
            {
                if (_font != value)
                {
                    _font = value;                    
                    _font.FontSet -= new EventHandler(_font_FontSet);
                    _font.FontSet += new EventHandler(_font_FontSet);
                }
            }
        }

        public override void CopyValuesTo(SceneItem target)
        {
            base.CopyValuesTo(target);
            TextItem text = target as TextItem;
            text.Font = this.Font;            
            text.Text = this.Text;
            text.AutoCenterPivot = this.AutoCenterPivot;
            text.Shadow = this.Shadow;
            text.Tint = this.Tint;
        }

        public override void Draw(float elapsed)
        {
            if (!this.Visible)
            {
                return;
            }
            if (this.Shadow == true)
            {
                DrawRequest _drawRequestShadow = new DrawRequest();
                //_drawRequest.font = this.Material.Texture;
                _drawRequestShadow.position = this.Position + new Vector2(3, 2);
                _drawRequestShadow.rotation = this.Rotation;
                _drawRequestShadow.scaleRatio = this.Scale;
                Color shadowColor = Color.Black;
                shadowColor.A = _opacity;
                _drawRequestShadow.tint = shadowColor;
                _drawRequestShadow.font = _font.Font;
                _drawRequestShadow.text = Text;
                _drawRequestShadow.isFont = true;
                _drawRequestShadow.textSize = this.BoundingRectSize;
                _drawRequestShadow.pivot = Pivot;
                _drawRequestShadow.isPivotRelative = this.IsPivotRelative;
                _drawRequestShadow.IgnoreCamera = IgnoreCameraPosition;
                DrawingManager.DrawOnLayer(_drawRequestShadow, this.Layer, _blendingType);
            }
            //_drawRequest.font = this.Material.Texture;
            _drawRequest.position = this.Position;
            _drawRequest.rotation = this.Rotation;
            _drawRequest.scaleRatio = this.Scale;            
            _drawRequest.font = _font.Font;
            _drawRequest.isFont = true;
            _drawRequest.textSize = this.BoundingRectSize;
            _drawRequest.text = this.Text;
            _drawRequest.isPivotRelative = this.IsPivotRelative;
            _drawRequest.pivot = this.Pivot;
            _drawRequest.IgnoreCamera = this.IgnoreCameraPosition;
            DrawingManager.DrawOnLayer(_drawRequest, this.Layer, _blendingType);

            base.Draw(elapsed);
        }

        public override void UpdateBoundingRect()
        {            
            if (_font != null && _font.Font != null)
            {
                this.BoundingRectSize = _font.Font.MeasureString(Text) * Scale;
                base.UpdateBoundingRect();
            }
        }

        #endregion

        #region Private Methods

        void _font_FontSet(object sender, EventArgs e)
        {
            UpdateBoundingRect();
        }

        #endregion
    }
}
