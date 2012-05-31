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

namespace IceCream.Drawing
{
    public struct DebugLine
    {
        public Vector2 vertex;
        public Color color;

        public DebugLine(Vector2 vector2, Color color)
        {
            this.vertex = vector2;
            this.color = color;
        }
    }

    public static class DebugShapes
    {
        public static List<DebugLine> linesList = new List<DebugLine>();

        public static void DrawPolygon(Vector2[] vertices, Color color)
        {
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                linesList.Add(new DebugLine(vertices[i], color));
                linesList.Add(new DebugLine(vertices[i + 1], color));
            }
        }

        public static void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            linesList.Add(new DebugLine(start, color));
            linesList.Add(new DebugLine(end, color));
        }

        public static void DrawRectangle(Rectangle rectangle, Color color)
        {
            int w = rectangle.Width - 1;
            int h = rectangle.Height - 1;
            linesList.Add(new DebugLine(new Vector2(rectangle.X, rectangle.Y), color));
            linesList.Add(new DebugLine(new Vector2(rectangle.X + w, rectangle.Y), color));
            linesList.Add(new DebugLine(new Vector2(rectangle.X + w, rectangle.Y), color));
            linesList.Add(new DebugLine(new Vector2(rectangle.X + w, rectangle.Y + h), color));
            linesList.Add(new DebugLine(new Vector2(rectangle.X + w, rectangle.Y + h), color));
            linesList.Add(new DebugLine(new Vector2(rectangle.X, rectangle.Y + h), color));
            linesList.Add(new DebugLine(new Vector2(rectangle.X, rectangle.Y + h), color));
            linesList.Add(new DebugLine(new Vector2(rectangle.X, rectangle.Y), color));
        }

        public static void DrawCircle(Vector2 center, float radius, Color color)
        {
            float k_segments;
            // choose a good number of segments depending on the radius
            if (radius < 32)
            {
                k_segments = 16;
            }
            else if (radius < 250)
            {
                k_segments = 32;
            }
            else
            {
                k_segments = 64;
            }            
            float k_increment = 2.0f * (float)Math.PI / k_segments;
            float theta = 0.0f;
            Vector2 firstVector = Vector2.Zero;
            Vector2 lastVector = Vector2.Zero;            
            for (int i = 0; i < k_segments; i++)
            {
                Vector2 v = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                if (i > 1)
                {
                    linesList.Add(new DebugLine(lastVector, color));
                }
                else if (i == 0)
                {
                    firstVector = v;
                }
                lastVector = v;
                linesList.Add(new DebugLine(v, color));
                if (i == k_segments - 1)
                {
                    linesList.Add(new DebugLine(v, color));
                    linesList.Add(new DebugLine(firstVector, color));
                }
                theta += k_increment;
            }            
        }

    }
}
