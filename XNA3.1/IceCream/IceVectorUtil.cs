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

namespace IceCream
{
    public static class IceVectorUtil
    {
        public static Vector2 PointToLocalSpace(Vector2 point,
                             Vector2 heading,
                             Vector2 side,
                             Vector2 position)
        {
            Vector2 TransPoint = point;
            float desiredAngle =WrapAngle((float)Math.Atan2(heading.Y, heading.X));
            Vector2 _pos = point - position;
            TransPoint = Vector2.Transform(_pos, Matrix.CreateRotationZ(desiredAngle * -1));
            
            return TransPoint;
        }
        
        public static Vector2 PointToWorldSpace(Vector2 point,
                                       Vector2 heading,
                                       Vector2 position)
        {
            Vector2 TransPoint = point;
            float desiredAngle = WrapAngle((float)Math.Atan2(heading.Y, heading.X));

            TransPoint = Vector2.Transform(point,
                                 Matrix.CreateRotationZ(desiredAngle) *
                                 Matrix.CreateTranslation(position.ToVector3())
                                 );

            return TransPoint;

        }
        public static Vector2 VectorToWorldSpace(Vector2 vec,
                                            Vector2 heading)
        {
            Vector2 TransPoint = vec;
            float desiredAngle1 = (float)Math.Atan2(heading.Y, heading.X);
            desiredAngle1 = WrapAngle(desiredAngle1);
            TransPoint = Vector2.Transform(vec, Matrix.CreateRotationZ(desiredAngle1));
            return TransPoint;
        }

        public static Vector2 VectorToLocalSpace(Vector2 vec,
                                     Vector2 heading)
        {
            Vector2 TransVec = vec;
            float desiredAngle1 = (float)Math.Atan2(heading.Y, heading.X);
            if (desiredAngle1 < 0)
                desiredAngle1 += MathHelper.TwoPi;
            TransVec = Vector2.Transform(vec, Matrix.CreateRotationZ(desiredAngle1 * -1));
            return TransVec;
        }

        public static float WrapAngle(float angle)
        {
            if (angle< 0)
                angle+= MathHelper.TwoPi;
            if (angle > MathHelper.TwoPi)
                angle -= MathHelper.TwoPi;
            return angle;
        }
        
        public static Vector2 ConvertToCartesianCoordinates(float length, float rotation)
        {
            return new Vector2(length * (float)Math.Cos(rotation), length * (float)Math.Sin(rotation));
        }

        public static Vector2 ConvertToPolarCoordinates(Vector2 vector2)
        {
            return new Vector2(vector2.Length(), (float)Math.Atan2((double)vector2.X, (double)vector2.Y));
        }
    }
}
