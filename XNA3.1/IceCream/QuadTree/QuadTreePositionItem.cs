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

namespace IceCream.QuadTree
{
    /// <summary>
    /// A position item in a quadtree
    /// </summary>
    /// <typeparam name="T">The type of the QuadTree item's parent</typeparam>
    public class QuadTreePositionItem<T>
    {
        public QuadTreeNode<T> node;
        internal QuadTree<T> quadTree;
        #region Events and Event Handlers

        /// <summary>
        /// Handles the move event
        /// </summary>
        internal void OnMove()
        {
            // Update rectangles
            rect.TopLeft = position  ;// - (size * .5f);
            rect.BottomRight = position+size;// +(size * .5f);

            // Call event handler
            // if (Move != null) Move(this);
            hasMoved = true;
            if (node != null) node.ItemMove(this);
        }
        internal bool hasMoved = false;
        /// <summary>
        /// Handles the destroy event
        /// </summary>
        protected void OnDestroy()
        {
            //if (Destroy != null) Destroy(this);
            node.RemoveItem(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The center position of this item
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// Gets or sets the center position of this item
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                OnMove();
                if (node != null)
                {
                    if (node.ContainsRect(this.rect))
                        return;
                  //  node.RemoveItem(this);
                }
               // if (quadTree!= null)
                 //   quadTree.Insert(this);
                
            }
        }

        /// <summary>
        /// The size of this item
        /// </summary>
        private Vector2 size;

        /// <summary>
        /// Gets or sets the size of this item
        /// </summary>
        public Vector2 Size
        {
            get { return size; }
            set
            {
                size = value;
                rect.TopLeft = position - (size / 2f);
                rect.BottomRight = position + (size / 2f);
                OnMove();
            }
        }

        /// <summary>
        /// The rectangle containing this item
        /// </summary>
        private FRect rect;

        /// <summary>
        /// Gets a rectangle containing this item
        /// </summary>
        public FRect Rect
        {
            get { return rect; }
        }

        /// <summary>
        /// The parent of this item
        /// </summary>
        /// <remarks>The Parent accessor is used to gain access to the item controlling this position item</remarks>
        private T parent;

        /// <summary>
        /// Gets the parent of this item
        /// </summary>
        public T Parent
        {
            get { return parent; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Creates a position item in a QuadTree
        /// </summary>
        /// <param name="parent">The parent of this item</param>
        /// <param name="position">The position of this item</param>
        /// <param name="size">The size of this item</param>
        public QuadTreePositionItem(T parent, Vector2 position, Vector2 size)
        {
            this.rect = new FRect(0f, 0f, 1f, 1f);

            this.parent = parent;
            this.position = position ;
            this.size = size;
            OnMove();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Destroys this item and removes it from the QuadTree
        /// </summary>
        public void Delete()
        {
            OnDestroy();
        }

        #endregion
    }

}
