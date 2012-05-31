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
    public class Camera : SceneItem
    {
        #region Fields
                
        private Vector2 _zoom;
        private Vector2 _position;
        private Point _viewPortPosition;
        private Point _viewPortSize;
        private Point _lastViewPortSize;
        private float _rotation;
        private Matrix _matrix;
        private bool _computeMatrix;
        
        #endregion

        #region Properties

        public Matrix Matrix
        {
            get { return _matrix; }
            set { _matrix = value; }
        }

        public Vector2 Zoom
        {
            get { return _zoom; }
            set
            {            
                if (_zoom != value)
                {
                    _zoom = value;
                    this.Scale = Vector2.One / value;
                    _computeMatrix = true;                    
                }
            }
        }

        public override Vector2 Pivot
        {
            get { return base.Pivot; }
            set
            {
                if (base.Pivot != value)
                {
                    base.Pivot = value;
                    _computeMatrix = true;
                }
            }
        }

        public new float Rotation
        {
            get { return _rotation; }
            set 
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    _computeMatrix = true;                  
                }
            }
        }

        public new void Move(Vector2 vec)
        {
            Position += vec;
        }

        public override Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    _computeMatrix = true;                   
                }
            }
        }

        public override float PositionX
        {
            get { return _position.X; }
            set
            {
                if (_position.X != value)
                {
                    _position.X = value;
                    _computeMatrix = true;                  
                }
            }
        }

        public override float PositionY
        {
            get { return _position.Y; }
            set
            {
                if (_position.Y != value)
                {
                    _position.Y = value;
                    _computeMatrix = true;                    
                }
            }
        }

        public Point ViewPortPosition
        {
            get { return _viewPortPosition; }
            set { _viewPortPosition = value; }
        }

        public override Vector2 BoundingRectSize
        {
            get
            {                
                return new Vector2(DrawingManager.ViewPortSize.X, DrawingManager.ViewPortSize.Y);
            }
            set
            {
                base.BoundingRectSize = value;
            }
        }

        #endregion

        #region Constructor

        public Camera()
        {
            _position = Vector2.Zero;
            _viewPortPosition = Point.Zero;
            _zoom = Vector2.One;
            _viewPortSize = new Point(1280, 720);
            _rotation = 0f;
			#if XNATOUCH
			_matrix = Matrix.Identity;
			#else
            _matrix = Matrix.CreateScale(1.0f, 1.0f, 1.0f);
			#endif
            _computeMatrix = true;
            this.IsPivotRelative = true;
            this.Pivot = new Vector2(0.5f);
            if (DrawingManager.GraphicsDevice != null)
            {
				#if XNATOUCH
				_viewPortSize = new Point(DrawingManager.GraphicsDevice.Viewport.Width, 
				                          DrawingManager.GraphicsDevice.Viewport.Height);
				#else
				PresentationParameters pp = DrawingManager.GraphicsDevice.PresentationParameters;
                // full size by default
                _viewPortSize = new Point(pp.BackBufferWidth, pp.BackBufferHeight);
				#endif
            }
        }

        #endregion

        #region Methods

        public override void Update(float elapsed)
        {
            if (_computeMatrix || _lastViewPortSize != DrawingManager.ViewPortSize)
            {
                _matrix = GetMatrix(Vector3.One);
            }
            base.Update(elapsed);
        }

        public Matrix GetMatrix(Vector3 parallax)
		{
            Vector2 offSet = Vector2.Zero;
            Vector2 camPos = Vector2.Zero;
            Vector2 transformedZoom = _zoom;
            transformedZoom.X = 1 + (_zoom.X - 1) * parallax.Z;
            transformedZoom.Y = 1 + (_zoom.Y - 1) * parallax.Z;

            if (this.IsPivotRelative == true)
            {
                offSet.X = this.Pivot.X * DrawingManager.ViewPortSize.X;
                offSet.Y = this.Pivot.Y * DrawingManager.ViewPortSize.Y;
            }
            else
            {
                offSet.X = this.Pivot.X;
                offSet.Y = this.Pivot.Y;
            }
            camPos.X = _position.X * parallax.X;
            camPos.Y = _position.Y * parallax.Y;
            return Matrix.CreateTranslation(-camPos.X, -camPos.Y, 0) *
                Matrix.CreateScale(transformedZoom.X, transformedZoom.Y, 1.0f) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateTranslation(offSet.X, offSet.Y, 0);
        }

        /// <summary>
        /// Translate an absolute screen position (i.e. mouse coord) into a world position, using this Camera
        /// </summary>
        public Vector2 ConvertToWorldPos(Vector2 position)
        {           
            return Vector2.Transform(position, Matrix.Invert(_matrix));
        }

        /// <summary>
        /// Translate a world position into an absolute screen position, using this Camera
        /// </summary>
        public Vector2 ConvertToScreenPos(Vector2 position)
        {
            return Vector2.Transform(position, _matrix);
        }

        #endregion
    }
}
