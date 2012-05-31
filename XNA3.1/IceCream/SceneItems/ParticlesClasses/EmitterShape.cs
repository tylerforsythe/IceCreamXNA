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

namespace IceCream.SceneItems.ParticlesClasses
{  
    public struct EmitterShape
    {
        #region Fields

        private EmitterShapeType type;
        private Vector2 position;
        private Vector2 offset;
        private Vector2 size;
        private Vector2[] maskPoints;

        #endregion

        #region Properties

        public EmitterShapeType Type
        {
            get { return type; }
            set { type = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }
        public Vector2[] MaskPoints
        {
            get { return maskPoints; }
            set { maskPoints = value; }
        }        

        #endregion

        #region Constructor

        public EmitterShape(EmitterShapeType type)
            : this(type, new Vector2(100), Vector2.Zero, Vector2.Zero)
        {            
        }
        public EmitterShape(EmitterShapeType type, Vector2 size, Vector2 position, Vector2 offset)
        {
            this.type = type;
            this.size = size;
            this.position = position;
            this.offset = offset;
            this.maskPoints = null;
        }

        #endregion

        #region Methods

        public Vector2 GetNewEmissionPoint()
        {
            Vector2 emissionPointOffset = Vector2.Zero;            
            switch (type)
            {
                case EmitterShapeType.Rectangle:
                    emissionPointOffset.X = Randomizer.Float(-size.X / 2.0f, size.X / 2.0f);
                    emissionPointOffset.Y = Randomizer.Float(-size.Y / 2.0f, size.Y / 2.0f);
                    break;
                case EmitterShapeType.Circle:
                    Vector2 radius = new Vector2(Randomizer.Float(0f, size.X), Randomizer.Float(0f, size.Y));
                    float angle = Randomizer.AngleInRadians();
                    emissionPointOffset.X = radius.X * IceMath.CosFromRadians(angle);
                    emissionPointOffset.Y = radius.Y * IceMath.SinFromRadians(angle); 
                    break;
                case EmitterShapeType.CircleOutline:
                    float angle2 = Randomizer.AngleInRadians();
                    emissionPointOffset.X = size.X * IceMath.CosFromRadians(angle2);
                    emissionPointOffset.Y = size.Y * IceMath.SinFromRadians(angle2);
                    break;
                case EmitterShapeType.TextureMask:
                    // choose an array index at random
                    if (maskPoints.Length > 0)
                    {
                        int arrayIndex = Randomizer.Integer(0, maskPoints.Length - 1);                    
                        emissionPointOffset = maskPoints[arrayIndex];
                    }
                    break;
                default:
                    break;
            }
            return emissionPointOffset + offset + position;
        }

        #endregion    
       
        #region Bitmap Shape Methods

        private int GetArrayIndexFromPixel(int x, int y, int w)
        {
            return y * w + x;
        }

        private bool IsPixelValid(int x, int y, int w, ref Color[] source)
        {
            if (source[GetArrayIndexFromPixel(x, y, w)].A == 255)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsPixelAtBorder(int x, int y, int w, int h, ref Color[] source,
            bool shouldCheckLeftPixels, bool shouldCheckRightPixels, 
            bool shouldCheckTopPixels, bool shouldCheckBottomPixels)
        {
            // safe check for boundaries
            bool checkLeftPixels = (x > 0);
            bool checkRightPixels = (x < w - 1);
            bool checkTopPixels = (y > 0);
            bool checkBottomPixels = (y < h - 1);
            // use the actual settings
            checkBottomPixels &= shouldCheckBottomPixels;
            checkLeftPixels &= shouldCheckLeftPixels;
            checkRightPixels &= shouldCheckRightPixels;
            checkTopPixels &= shouldCheckTopPixels;
         
            if (source[GetArrayIndexFromPixel(x, y, w)].A == 255)
            {
                // check the left column of pixels if possible
                if (checkLeftPixels)
                {
                    if ((checkTopPixels && source[GetArrayIndexFromPixel(x - 1, y - 1, w)].A != 255)
                        || (source[GetArrayIndexFromPixel(x - 1, y, w)].A != 255)
                        || (checkBottomPixels && source[GetArrayIndexFromPixel(x - 1, y + 1, w)].A != 255))
                    {
                        return true;
                    }
                }
                // check the right column of pixels if possible
                if (checkRightPixels)
                {
                    if ((checkTopPixels && source[GetArrayIndexFromPixel(x + 1, y - 1, w)].A != 255)
                        || (source[GetArrayIndexFromPixel(x + 1, y, w)].A != 255)
                        || (checkBottomPixels && source[GetArrayIndexFromPixel(x + 1, y + 1, w)].A != 255))
                    {
                        return true;
                    }
                }
                // check the current column of pixels if possible
                if ((checkTopPixels && source[GetArrayIndexFromPixel(x, y - 1, w)].A != 255)
                        || (checkBottomPixels && source[GetArrayIndexFromPixel(x, y + 1, w)].A != 255))
                {
                    return true;
                }
            }
            return false;
        }

        public void CreateTextureMaskFromTexture2D(Texture2D texture, 
            bool shouldCheckWholeShape, bool shouldCheckLeftPixels, bool shouldCheckRightPixels,
            bool shouldCheckTopPixels, bool shouldCheckBottomPixels)
        {
			#if !XNATOUCH
            int maskWidth = texture.Width;
            int maskHeight = texture.Height;
            // create a new array of pixels
            Color[] dataTexture = new Color[maskWidth * maskHeight];
            List<Vector2> resultList = new List<Vector2>();
            // load all the pixel values from the texture
            Drawing.DrawingManager.GraphicsDevice.Textures[0] = null;
            texture.GetData<Color>(dataTexture);
            for (int i = 0; i < maskWidth; i++)
            {
                for (int j = 0; j < maskHeight; j++)
                {
                    if (shouldCheckWholeShape == true)
                    {
                        if (IsPixelValid(i, j, maskWidth, ref dataTexture))
                        {
                            resultList.Add(new Vector2(i, j));
                        }
                    }
                    else
                    {
                        if (IsPixelAtBorder(i, j, maskWidth, maskHeight, ref dataTexture,
                            shouldCheckLeftPixels, shouldCheckRightPixels, shouldCheckTopPixels, shouldCheckBottomPixels))
                        {
                            resultList.Add(new Vector2(i, j));
                        }
                    }
                }
            }
            maskPoints = resultList.ToArray();
#endif
        }

        public void Initialize()
        {
            
        }

        #endregion
    }
}
