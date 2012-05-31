using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using IceCream.SceneItems;
using Microsoft.Xna.Framework;
using IceCream.Components;
using IceCream.Attributes;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Milkshake.Tools
{
    public class SpriteInfo
    {
        public String Name
        {
            get;
            set;
        }
        public Texture2D Texture2D
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }

        public uint[] PixelData
        {
            get;
            set;
        }

        public Rectangle Area
        {
            get;
            set;
        }

        public SpriteInfo(String name)
        {
            this.Name = name;
            this.X = 0;
            this.Y = 0;
        }

        public void CopyPixels(uint[] source, int sourceTextureWidth, Rectangle sourceArea, 
            ref uint[] destination, int destTextureWidth, Point position, uint baseColor, bool overrideTransColor)
        {
            for (int x = 0; x < sourceArea.Width; x++)
            {
                for (int y = 0; y < sourceArea.Height; y++)
                {
                    Point sourcePos = new Point(sourceArea.X + x, sourceArea.Y + y);
                    uint sourceIndex = (uint)(sourcePos.Y * sourceTextureWidth + sourcePos.X);
                    Point destPos = new Point(position.X + x, position.Y + y);
                    uint destIndex = (uint)(destPos.Y * destTextureWidth + destPos.X);
                    
                    destination[destIndex] = source[sourceIndex];
                    // if the alpha value is 0, override the RGB color
                    if (overrideTransColor == true && destination[destIndex] < 16777216)
                    {
                        destination[destIndex] = baseColor;
                        
                    }
                }
            }
        }
    }
}
