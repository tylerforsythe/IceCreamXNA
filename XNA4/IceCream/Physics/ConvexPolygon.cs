using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using IceCream;
using IceCream.Drawing;

namespace IceCream.Physics
{
    public class ConvexPolygon
    {
        #region Static Methods

        /// <summary>
        /// Check if a given simple closed list of vertices (with clockwise winding) is a forming convex polygon. 
        /// The polygon must always be a correct simple polygon (no self intersecting vertex)
        /// </summary>
        /// <param name="vertices">List of vertices</param>
        /// <returns>True if convex, false otherwise</returns>
        public static bool IsSimpleClockWisePolygonConvex(List<Vector2> vertices)
        {            
            // any triangle, line or point is convex, but we'll still check triangles to catch any anticlockwork winding case (treated as non convex)
            if (vertices.Count < 3)
            {
                return true;
            }
            else
            {
                // check the dot product of each angle between 2 vertex, as it must always be 0 or positive to be convex
                for (int i = 1; i <= vertices.Count; i++)
                {
                    Vector2 v1 = vertices[i % vertices.Count] - vertices[i - 1];
                    Vector2 v2 = vertices[(i + 1) % vertices.Count] - vertices[i % vertices.Count];
                    float cross = v1.X * v2.Y - v1.Y * v2.X;
                    if (cross < 0)
                    {
                        return false;
                    }
                }
                return true;
            }            
        }

        #endregion

        #region Fields

        private List<Vector2> _vertices;
        private List<Vector2> _transformedVertices;
        private Vector2 _position;
        private Vector2 _center;
        private float _rotation;
        private Color _outlineColor;

        #endregion

        #region Properties

        public Vector2 Center
        {
            get
            {
                return _center;
            }
        }

        public Color OutlineColor
        {
            get
            {
                return _outlineColor;
            }
            set
            {
                _outlineColor = value;
            }
        }

        /// <summary>
        /// Rotation angle in degrees of the polygon
        /// </summary>
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    ApplyChanges();
                }
            }
        }
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        /// <summary>
        /// List of vertice coordinates relative to the polygon's position
        /// </summary>
        public List<Vector2> Vertices
        {
            get { return _vertices; }
            set
            {
                if (_vertices != value)
                {
                    _vertices = value;
                    ApplyChanges();
                }
            }
        }

        public List<Vector2> TransformedVertices
        {
            get { return _transformedVertices; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an empty polygon (0 vertice)
        /// </summary>
        public ConvexPolygon()
        {
            this._vertices = new List<Vector2>();
            this._transformedVertices = new List<Vector2>();
            _center = Vector2.Zero;
            _outlineColor = Color.Fuchsia;
            ApplyChanges();
        }

        /// <summary>
        /// Creates a polygon from a list of vertices. The polygon must be simple (no self-intersecting sides) and convex.
        /// </summary>
        /// <param name="vertices"></param>
        public ConvexPolygon(List<Vector2> vertices)
        {
            this._vertices = vertices;
            this._transformedVertices = new List<Vector2>();
            ApplyChanges();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Apply the changes and construct a polygon after modification of its vertices. 
        /// Note that this method is called automatically after a modification of the rotation angle
        /// </summary>
        public void ApplyChanges()
        {
            _transformedVertices.Clear();
            for (int i = 0; i < _vertices.Count; i++)
            {
                _transformedVertices.Add(Vector2.Transform(_vertices[i], Matrix.CreateRotationZ(MathHelper.ToRadians(_rotation))));
            }
            // calculate the center
            float totalX = 0;
            float totalY = 0;
            for (int i = 0; i < _transformedVertices.Count; i++)
            {
                totalX += _transformedVertices[i].X;
                totalY += _transformedVertices[i].Y;
            }
            _center = new Vector2(totalX / (float)_transformedVertices.Count, totalY / (float)_transformedVertices.Count);
        } 

        /// <summary>
        /// Render the polygon using debug lines
        /// </summary>
        /// <param name="position"></param>
        /// <param name="verticesColor"></param>
        public void Draw(Vector2 position, Color verticesColor)
        {           
            for (int i = 0; i < _vertices.Count; i++)
            {
                Color tint = OutlineColor;
                DebugShapes.DrawLine(_transformedVertices[i] + position, 
                    _transformedVertices[(i + 1) % _transformedVertices.Count] + position, tint);
            }
            int centerLineSize = 3;
            DebugShapes.DrawLine(new Vector2(_center.X - centerLineSize, _center.Y - centerLineSize) + position,
                    new Vector2(_center.X + centerLineSize, _center.Y + centerLineSize) + position, verticesColor);
            DebugShapes.DrawLine(new Vector2(_center.X - centerLineSize, _center.Y + centerLineSize) + position,
                    new Vector2(_center.X + centerLineSize, _center.Y - centerLineSize) + position, verticesColor);
        }      

        #endregion

    }

}
