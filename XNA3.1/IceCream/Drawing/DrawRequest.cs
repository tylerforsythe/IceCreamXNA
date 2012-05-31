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
using System.Diagnostics;


namespace IceCream.Drawing
{
    public struct DrawRequest
    {
        public bool IgnoreCamera ;
        public Texture2D texture;
        public Vector2 position;
        public Rectangle? sourceRectangle;
        public float rotation;
        public Vector2 scaleRatio;
        public Vector2 pivot;
        public bool isPivotRelative;
        public Color tint;
        public bool hFlip;
        public bool vFlip;
        public SpriteFont font;
        public bool isFont;
        public String text;
        public Vector2 textSize;

        public DrawRequest(Texture2D texture, Vector2 position, bool ignoreCamera, 
            Rectangle? sourceRectangle, float rotation, Vector2 scaleRatio, Vector2 pivot, 
            bool isPivotRelative, Color tint, bool hFlip, bool vFlip, SpriteFont font)
        {
            //Trace.Assert(texture != null && font != null, "You must specify a texture or a font, not both");
            //Trace.Assert(texture == null && font == null, "You must specify only a texture or a font");
            this.texture = texture;
            this.font = font;
            this.isFont = (font != null);
            this.text = "";
            this.texture = texture;
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.rotation = rotation;
            this.scaleRatio = scaleRatio;
            this.pivot = pivot;
            this.isPivotRelative = isPivotRelative;
            this.tint = tint;
            this.hFlip = hFlip;
            this.vFlip = vFlip;
            this.IgnoreCamera = ignoreCamera;
            this.textSize = Vector2.One;
        }
    }    
}