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

namespace IceCream.QuadTree
{
    public struct FRect
    {
        private Vector2 topLeft;
        private Vector2 bottomRight;

        #region Properties

        
        public Vector2 TopLeft
        {
            get { return topLeft; }
            set { topLeft = value; }
        }

        public Vector2 TopRight
        {
            get { return new Vector2(bottomRight.X, topLeft.Y); }
            set
            {
                bottomRight.X = value.X;
                topLeft.Y = value.Y;
            }
        }

        public Vector2 BottomRight
        {
            get { return bottomRight; }
            set { bottomRight = value; }
        }

        public Vector2 BottomLeft
        {
            get { return new Vector2(topLeft.X, bottomRight.Y); }
            set
            {
                topLeft.X = value.X;
                bottomRight.Y = value.Y;
            }
        }

        public float Top
        {
            get { return TopLeft.Y; }
            set { topLeft.Y = value; }
        }

        public float Left
        {
            get { return TopLeft.X; }
            set { topLeft.X = value; }
        }

        public float Bottom
        {
            get { return BottomRight.Y; }
            set { bottomRight.Y = value; }
        }

        public float Right
        {
            get { return BottomRight.X; }
            set { bottomRight.X = value; }
        }

        public float Width
        {
            get { return bottomRight.X - topLeft.X; }
        }

        public float Height
        {
            get { return bottomRight.Y - topLeft.Y; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Floating-point rectangle constructor
        /// </summary>
        /// <param name="topleft">The top left point of the rectangle</param>
        /// <param name="bottomright">The bottom right point of the rectangle</param>
        public FRect(Vector2 topleft, Vector2 bottomright)
        {
            topLeft = topleft;
            bottomRight = bottomright;
        }

        /// <summary>
        /// Floating-point rectangle constructor
        /// </summary>
        /// <param name="top">The top of the rectangle</param>
        /// <param name="left">The left of the rectangle</param>
        /// <param name="bottom">The bottom of the rectangle</param>
        /// <param name="right">The right of the rectangle</param>
        public FRect(float top, float left, float bottom, float right)
        {
            topLeft = new Vector2(left, top);
            bottomRight = new Vector2(right, bottom);
        }

        #endregion

        #region Methods

        public bool Contains(Vector2 Point)
        {
            return (topLeft.X <= Point.X && bottomRight.X >= Point.X &&
                    topLeft.Y <= Point.Y && bottomRight.Y >= Point.Y);
        }

        public bool Intersects(FRect Rect)
        {
            return (!(Bottom < Rect.Top ||
                       Top > Rect.Bottom ||
                       Right < Rect.Left ||
                       Left > Rect.Right));
        }

        #endregion
    }
}
