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
using System.ComponentModel;
using IceCream.Drawing;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml;
using IceCream.Components;
using IceCream.Attributes;

namespace IceCream.SceneItems
{
    public class Sprite : SceneItem, ISubItemCollection
    {
        #region Fields

        private DrawRequest _drawRequest;
        private Material _material;
        private String _materialArea;
        private DrawingBlendingType _blendingType;
        private bool _useTilingSafeBorders;
        private byte _opacity;
        private float _autoScrollOffset;

        #endregion

        #region Properties
      
        [BrowsableAttribute(false)]
        public virtual Material Material
        {
            get { return _material; }
            set
            {

                // update the boundaries 
                UpdateBoundingRect();                
                _material = value;
                if (value != null)
                {
                    _drawRequest.texture = value.Texture;
                }
            }
        }

        public String MaterialArea
        {
            get { return _materialArea; }
            set
            {
                if (_materialArea != value)
                {
                    _materialArea = value;
                    if (String.IsNullOrEmpty(value) == false)
                    {
                        this.SourceRectangle = this.Material.Areas[value];
                    }
                }
            }
        }

        [BrowsableAttribute(false)]
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

        [BrowsableAttribute(false), XmlIgnore]
        public virtual Rectangle? SourceRectangle
        {
            get { return _drawRequest.sourceRectangle; }
            set { _drawRequest.sourceRectangle = value; }
        }

#if !XNATOUCH
        [ContentSerializer]
#endif
        internal Rectangle SrcRect
        {
            set
            {
                if (value != Rectangle.Empty)
                    SourceRectangle = value;
            }
            get
            {
                if (SourceRectangle.HasValue)
                    return SourceRectangle.Value;
                else
                    return Rectangle.Empty;
            }
        }
                
        public override Vector2 Pivot
        {
            get { return _drawRequest.pivot; }
            set { _drawRequest.pivot = value; }
        }

        [BrowsableAttribute(false)]
        public virtual DrawingBlendingType BlendingType
        {
            get { return _blendingType; }
            set { _blendingType = value; }
        }

        [BrowsableAttribute(false)]
        public virtual bool UseTilingSafeBorders
        {
            get { return _useTilingSafeBorders; }
            set { _useTilingSafeBorders = value; }
        }

        public override Vector2 BoundingRectSize
        {
            get
            {
                if (Material != null && Material.Texture != null)
                {
                    Vector2 sourceSize = Vector2.Zero;
                    if (this.SourceRectangle.HasValue)
                    {
                        sourceSize.X = this.SourceRectangle.Value.Width;
                        sourceSize.Y = this.SourceRectangle.Value.Height;
                    }
                    else
                    {
                        sourceSize.X = this.Material.Texture.Width;
                        sourceSize.Y = this.Material.Texture.Height;
                    }
                    return sourceSize;                    
                }
                return base.BoundingRectSize;
            }
            set
            {
                base.BoundingRectSize = value;
            }
        }

        public bool AutoScroll
        {
            get;
            set;
        }

        public float AutoScrollSpeed
        {
            get;
            set;
        }

        public bool AutoScrollVertical
        {
            get;
            set;
        }

        public int AutoScrollArea
        {
            get;
            set;
        }

        public bool AutoScrollMirroring
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public Sprite()
            : base()
        {
            _blendingType = DrawingBlendingType.Alpha;           
            _drawRequest = default(DrawRequest);
            _drawRequest.tint = Color.White;
            _drawRequest.scaleRatio = Vector2.One;
            this.MaterialArea = "";
            _useTilingSafeBorders = false;
            this.AutoScroll = false;
            this.AutoScrollSpeed = 0;
            this.AutoScrollArea = 0;
            this.AutoScrollVertical = false;
            this.AutoScrollMirroring = false;
        }

        #endregion

        #region Methods

        public override void CopyValuesTo(SceneItem target)
        {
            base.CopyValuesTo(target);
            Sprite sprite = target as Sprite;            
            sprite.Material = this.Material;
            sprite.BoundingRect = this.BoundingRect;
            sprite.BlendingType = this.BlendingType;       
            sprite.SourceRectangle = this.SourceRectangle;
            sprite.MaterialArea = this.MaterialArea;
            sprite.Tint = this.Tint;
            sprite.UseTilingSafeBorders = this.UseTilingSafeBorders;
        }

        public virtual List<String> GetSubItemsList()
        {
            List<String> retVal = new List<String>();
            foreach (String key in this.Material.Areas.Keys)
            {
                retVal.Add(key);
            }
            return retVal;
        }

        public string GetCurrentSubItem()
        {
            return (Material != null) ? MaterialArea : string.Empty;
        }

        public virtual void SetCurrentSubItem(string subItem)
        {
            if (this.Material.Areas.ContainsKey(subItem))
            {
                this.MaterialArea = subItem;
            }
        }

        public void DrawSpriteAtPos(Vector2 position, bool hFlip, bool vFlip)
        {
            _drawRequest.texture = this.Material.Texture;
            _drawRequest.position = position;
            _drawRequest.rotation = this.Rotation;
            _drawRequest.scaleRatio = this.Scale;
            _drawRequest.tint = this.Tint;
            _drawRequest.isPivotRelative = this.IsPivotRelative;
            _drawRequest.hFlip = hFlip;
            _drawRequest.vFlip = vFlip;
            _drawRequest.sourceRectangle = SourceRectangle;
            DrawingManager.DrawOnLayer(_drawRequest, this.Layer, _blendingType);
        }

        public override void Draw(float elapsed)
        {
            if (!Visible || this.Material ==null || this.Material.Texture == null)
            {
                return;
            }
            if (!IsInView())
                return;
            
            if (this.AutoScroll == false)
            {
                DrawSpriteAtPos(this.Position, this.FlipHorizontal, this.FlipVertical);                
            }
            else
            {
                float size = this.BoundingRect.Width;                
                Point drawOffsets = new Point(0, 1);
                if (_autoScrollOffset < 0)
                {
                    drawOffsets.X = (int)Math.Floor(_autoScrollOffset / (double)size);
                }
                //else if (_autoScrollOffset > 0 && this.AutoScrollSpeed < 0) {
                //    drawOffsets.X = 0;// (int)Math.Floor(_autoScrollOffset / (double)size);
                //}


                if (this.AutoScrollSpeed >= 0) {
                    drawOffsets.Y = (int)((this.AutoScrollArea + _autoScrollOffset) / (double)size);
                }
                else {
                    //TODO parallax scrolling "negative" values
                    //this really isn't that great of an implementation for "negative" scrolling values and is 
                    //likely to not be very optimal, as well as break when I figure out how to move the camera around
                    drawOffsets.Y = (int)Math.Round((this.AutoScrollArea + _autoScrollOffset + Math.Abs(this.BoundingRect.Left)) / (double)size);
                }

                //Console.WriteLine("offset: " + drawOffsets);
                for (int x = drawOffsets.X; x <= drawOffsets.Y; x++)
                {
                    bool hFlip = this.FlipHorizontal;
                    bool isEven = (Math.Abs(x) % 2 == 0);
                    
                    if (this.AutoScrollMirroring == true && isEven == false)
                    {
                        hFlip = !hFlip;
                    }
                    /*
                    if (this.AutoScrollMirroring == true && isEven)
                    {
                        this.Tint = Color.Black;
                    }
                    else
                    {
                        this.Tint = Color.White;
                    }*/
                    Vector2 pos = this.Position - new Vector2(_autoScrollOffset - x*size, 0);
                    DrawSpriteAtPos(pos, hFlip, this.FlipVertical);
                }            
            }
            base.Draw(elapsed);
        }               

        public override void Update(float elapsed)
        {            
            UpdateBoundingRect();
            if (this.AutoScroll == true)
            {
                this.Rotation = 0;
                _autoScrollOffset -= this.AutoScrollSpeed;
                Vector2 size = new Vector2(this.BoundingRect.Width, this.BoundingRect.Height);
                if (this.AutoScrollMirroring == true)
                {
                    size *= 2;
                }

                    float hDiff = (size.X - _autoScrollOffset);
                    if (hDiff <= 0) {
                        _autoScrollOffset = hDiff;
                    }
                    else {
                        hDiff = size.X + _autoScrollOffset;
                        if (hDiff <= 0) {
                            _autoScrollOffset = hDiff;
                        }
                    }
            }
            base.Update(elapsed);
        }

        internal bool IsInView()
        {
            return true;
            //TODO
            //**************************
            //Not yet finished
            //**************************
            /* Commented this unreachable code to avoid warnings
             * 
            if (SceneParent == null || IgnoreCameraPosition)
                return true;
            //Vector2 _drawPosition = Position;
            //Vector2 _res=SceneManager.GlobalDataHolder.ScreenResolution;
            //Vector2 _tl = _drawPosition - Pivot;
            //Vector2 _br= _drawPosition + Pivot;
            //Rectangle _rect=new Rectangle(
            //    (int)_tl.X,
            //    (int)_tl.Y,
            //    BoundingRect.Width,
            //    BoundingRect.Height);

            if (SceneParent.ActiveCameras[0].BoundingRect.Intersects(BoundingRect))
                return true;
            else
                return false;
             */
        }
        
        #endregion
    }
}
