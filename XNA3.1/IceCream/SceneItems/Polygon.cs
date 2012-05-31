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
using System.Linq;
using System.Text;
using System.Globalization;

namespace IceCream.SceneItems
{
    public class Polygon
    {
        /*
        #region static

        public const String NULL_VALUE = "n";

        public static Polygon FromRectangle(Rectangle rect)
        {
            Vertices vertices = new Vertices(4);
            vertices.Add(new Vector2(rect.Left, rect.Top));
            vertices.Add(new Vector2(rect.Right, rect.Top));
            vertices.Add(new Vector2(rect.Right, rect.Bottom));
            vertices.Add(new Vector2(rect.Left, rect.Bottom));
            return new Polygon(vertices);
        }

        public static Polygon FromVector2List(List<Vector2> list)
        {
            Vertices vertices = new Vertices(list.Count);
            vertices.AddRange(list);
            return new Polygon(vertices);
        }

        public static Polygon FromString(String polygonData)
        {
            return FromString(polygonData, Vector2.Zero);
        }

        public static Polygon FromString(String polygonData, Vector2 offset)
        {
            if (String.IsNullOrEmpty(polygonData) || polygonData == NULL_VALUE)
            {
                return null;
            }
            Polygon poly = new Polygon();
            char[] sep = { ';' };
            String[] splitted = polygonData.Split(sep);
            foreach (String v in splitted)
            {
                char[] space_sep = { ' ' };
                String[] values = v.Split(space_sep);
                if (values.Length == 2)
                {
                    Vector2 vector2 = new Vector2(float.Parse(values[0], CultureInfo.InvariantCulture),
                        float.Parse(values[1], CultureInfo.InvariantCulture)) + offset;
                    poly.Vertices.Add(vector2);
                }
            }
            poly.InitialCentroid = poly.Vertices.GetCentroid();
            return poly;
        }

        #endregion

        public Vertices Vertices { get; set; }
        public Vector2 InitialCentroid { get; set; }

        public Polygon()
        {
            this.Vertices = new Vertices();            
        }

        public Polygon(Vertices vertices)
        {
            this.Vertices = vertices;
            this.InitialCentroid = vertices.GetCentroid();
        }

        public override string ToString()
        {
            String output = "";
            for (int i = 0; i < this.Vertices.Count; i++)
            {
                if (i > 0)
                {
                    output += ";";
                }
                Vector2 vertice = this.Vertices[i];
                output += vertice.X.ToString(CultureInfo.InvariantCulture) + " " 
                    + vertice.Y.ToString(CultureInfo.InvariantCulture); 
            }
            return output;
        }

        public Vertices GetHFlipVertices()
        {
            Vertices hVerts = new Vertices(this.Vertices.Count);
            foreach (Vector2 vertice in this.Vertices)
            {
                Vector2 hFlip = new Vector2(-vertice.X, vertice.Y);
                hVerts.Add(hFlip);
            }
            return hVerts;
        }

        public Vertices GetVFlipVertices()
        {
            Vertices vVerts = new Vertices(this.Vertices.Count);
            foreach (Vector2 vertice in this.Vertices)
            {
                Vector2 vFlip = new Vector2(vertice.X, -vertice.Y);
                vVerts.Add(vFlip);
            }
            return vVerts;
        }

        public Vertices GetBothFlipVertices()
        {
            Vertices vVerts = new Vertices(this.Vertices.Count);
            foreach (Vector2 vertice in this.Vertices)
            {
                Vector2 vFlip = new Vector2(-vertice.X, -vertice.Y);
                vVerts.Add(vFlip);
            }
            return vVerts;
        }
        */
    }
}
