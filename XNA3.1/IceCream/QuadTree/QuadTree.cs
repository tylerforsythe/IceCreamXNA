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


namespace IceCream.QuadTree
{
    public class QuadTree<T>
    {
        public void Draw()
        {
            IceCream.Drawing.DebugShapes.DrawRectangle(
                GetRectangle(WorldRect), Color.White);


            DrawNode(headNode);
        }

        private void DrawNode(QuadTreeNode<T> node)
        {
            node.Draw();   
        }
        public static Rectangle GetRectangle(FRect rect)
        {
            return new Rectangle((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
        }
        public void Count()
        {
            
        }

        #region Properties

        /// <summary>
        /// The head node of the QuadTree
        /// </summary>
        protected QuadTreeNode<T> headNode;

        /// <summary>
        /// Gets the world rectangle
        /// </summary>
        public FRect WorldRect
        {
            get { return headNode.Rect; }
        }

        /// <summary>
        /// The maximum number of items in any node before partitioning
        /// </summary>
        protected int maxItems;

        #endregion

        #region Initialization

        /// <summary>
        /// QuadTree constructor
        /// </summary>
        /// <param name="worldRect">The world rectangle for this QuadTree (a rectangle containing all items at all times)</param>
        /// <param name="maxItems">Maximum number of items in any cell of the QuadTree before partitioning</param>
        public QuadTree(FRect worldRect, int maxItems)
        {
            this.headNode = new QuadTreeNode<T>(worldRect, maxItems, Resize);
            this.maxItems = maxItems;
        }

        /// <summary>
        /// QuadTree constructor
        /// </summary>
        /// <param name="size">The size of the QuadTree (i.e. the bottom-right with a top-left of (0,0))</param>
        /// <param name="maxItems">Maximum number of items in any cell of the QuadTree before partitioning</param>
        /// <remarks>This constructor is for ease of use</remarks>
        public QuadTree(Vector2 size, int maxItems)
            : this(new FRect(Vector2.Zero, size), maxItems)
        {
            // Nothing extra to initialize
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts an item into the QuadTree
        /// </summary>
        /// <param name="item">The item to insert</param>
        /// <remarks>Checks to see if the world needs resizing and does so if needed</remarks>
        public void Insert(QuadTreePositionItem<T> item)
        {
            // check if the world needs resizing
            if (!headNode.ContainsRect(item.Rect))
            {
                Resize(new FRect(
                    Vector2.Min(headNode.Rect.TopLeft, item.Rect.TopLeft) * 2,
                    Vector2.Max(headNode.Rect.BottomRight, item.Rect.BottomRight) * 2));
            }
            item.node = null;
            item.quadTree = this;
            headNode.Insert(item);
        }

        /// <summary>
        /// Inserts an item into the QuadTree
        /// </summary>
        /// <param name="parent">The parent of the new position item</param>
        /// <param name="position">The position of the new position item</param>
        /// <param name="size">The size of the new position item</param>
        /// <returns>A new position item</returns>
        /// <remarks>Checks to see if the world needs resizing and does so if needed</remarks>
        public QuadTreePositionItem<T> Insert(T parent, Vector2 position, Vector2 size)
        {
            QuadTreePositionItem<T> item = new QuadTreePositionItem<T>(parent, position, size);

            // check if the world needs resizing
            if (!headNode.ContainsRect(item.Rect))
            {
                Resize(new FRect(
                    Vector2.Min(headNode.Rect.TopLeft, item.Rect.TopLeft) * 2,
                    Vector2.Max(headNode.Rect.BottomRight, item.Rect.BottomRight) * 2));
            }
            item.node = null;
            item.quadTree = this;
            headNode.Insert(item);

            return item;
        }

        /// <summary>
        /// Resizes the Quadtree field
        /// </summary>
        /// <param name="newWorld">The new field</param>
        /// <remarks>This is an expensive operation, so try to initialize the world to a big enough size</remarks>
        public void Resize(FRect newWorld)
        {
            // Get all of the items in the tree
            List<QuadTreePositionItem<T>> Components = new List<QuadTreePositionItem<T>>();
            GetAllItems(ref Components);

            // Destroy the head node
            headNode.Destroy();
            headNode = null;

            // Create a new head
            headNode = new QuadTreeNode<T>(newWorld, maxItems, Resize);

            // Reinsert the items
            foreach (QuadTreePositionItem<T> m in Components)
            {
                headNode.Insert(m);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Gets a list of items containing a specified point
        /// </summary>
        /// <param name="Point">The point</param>
        /// <param name="ItemsFound">The list to add found items to (list will not be cleared first)</param>
        public void GetItems(Vector2 Point, ref List<QuadTreePositionItem<T>> ItemsList)
        {
            if (ItemsList != null)
            {
                headNode.GetItems(Point, ref ItemsList);
            }
        }

        /// <summary>
        /// Gets a list of items intersecting a specified rectangle
        /// </summary>
        /// <param name="Rect">The rectangle</param>
        /// <param name="ItemsFound">The list to add found items to (list will not be cleared first)</param>
        public void GetItems(FRect Rect, ref List<QuadTreePositionItem<T>> ItemsList)
        {
            if (ItemsList != null)
            {
                headNode.GetItems(Rect, ref ItemsList);
            }
        }

        /// <summary>
        /// Get a list of all items in the quadtree
        /// </summary>
        /// <param name="ItemsFound">The list to add found items to (list will not be cleared first)</param>
        public void GetAllItems(ref List<QuadTreePositionItem<T>> ItemsList)
        {
            if (ItemsList != null)
            {
                headNode.GetAllItems(ref ItemsList);
            }
        }

        #endregion

        #region ISceneItem Members

        public Vector2 Position
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public float Scale
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public bool Visible
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public float DisplayWidth
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public float DisplayHeight
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int Layer
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion
    }


}
