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
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace IceCream
{
    public static class IceMath
    {
        #region Static Methods

        /// <summary>
        /// Returns the sin from an angle in degrees 
        /// </summary>
        /// <param name="angle">Angle value between 0 and 360 degrees</param>
        /// <returns>Sin value</returns>
        public static float SinFromDegree(float angle)
        {
            return (float)Math.Sin((double)MathHelper.ToRadians(angle));
        }

        /// <summary>
        /// Returns the cos from an angle in degrees 
        /// </summary>
        /// <param name="angle">Angle value between 0 and 360 degrees</param>
        /// <returns>Cos value</returns>
        public static float CosFromDegree(float angle)
        {
            return (float)Math.Cos((double)MathHelper.ToRadians(angle));
        }

        /// <summary>
        /// Returns the sin from an angle in radians 
        /// </summary>
        /// <param name="angle">Angle value between 0 and 2 PI radians</param>
        /// <returns>Sin value</returns>
        public static float SinFromRadians(float angle)
        {
            return (float)Math.Sin(angle);
        }

        /// <summary>
        /// Returns the cos from an angle in radians 
        /// </summary>
        /// <param name="angle">Angle value between 0 and 2 PI radians</param>
        /// <returns>Cos value</returns>
        public static float CosFromRadians(float angle)
        {
            return (float)Math.Cos(angle);
        }

        /// <summary>
        /// Check if a given point is contained inside a rectangle
        /// </summary>
        /// <returns>True if the point is contained inside the rectangle</returns>
        public static bool IsPointInsideRectangle(Point point, Rectangle rectangle)
        {
            return (point.X >= rectangle.Left && point.X <= rectangle.Right
                && point.Y >= rectangle.Top && point.Y <= rectangle.Bottom);
        }

        /// <summary>
        /// Clamp an integer value
        /// </summary>
        /// <returns>Clamped integer value</returns>
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
            return value;
        }

        /// <summary>
        /// Compares 2 integers and return the smallest value of the 2
        /// </summary>
        /// <returns></returns>
        public static int Min(int intA, int intB)
        {            
            if (intA <= intB)
            {
                return intA;
            }
            else
            {
                return intB;
            }
        }

        /// <summary>
        /// Compares 2 integers and return the biggest value of the 2
        /// </summary>
        public static int Max(int intA, int intB)
        {
            if (intA >= intB)
            {
                return intA;
            }
            else
            {
                return intB;
            }
        }

        public static float Floor(float value)
        {
            return (float)Math.Floor((double)value);
        }

        public static float Ceiling(float value)
        {
            return (float)Math.Ceiling((double)value);
        }

        /// <summary>
        /// Convert a Color to its uint representation
        /// </summary>
        public static uint GetPackedValueFromColor(Color color)
        {
            return (uint)(color.A * 16777216 + color.R * 65536 + color.G * 256 + color.B);
        }

        public static byte Lerp(byte value1, byte value2, float amount)
        {
            if (amount == 0)
            {
                return value1;
            }
            else if (amount == 1)
            {
                return value2;
            }
            return (byte)(value1 + (value2 - value1) * amount);
        }

        /// <summary>
        /// Gets the number of elements in an Enum (using Reflection on Xbox360 as Enum.GetValues() isn't available
        /// </summary>
        public static int GetEnumValuesCount(Type enumerationType)
        {            
            #if WINDOWS
            return System.Enum.GetValues(enumerationType).Length;
            #else
            object valAux = Activator.CreateInstance(enumerationType);
            FieldInfo[] fieldInfoArray = enumerationType.GetFields(BindingFlags.Static | BindingFlags.Public);
            return fieldInfoArray.Length;
            #endif
        }



        private static float _180OverPi = (180 / MathHelper.Pi);
        private static float _PiOver180 = (MathHelper.Pi / 180);

        public static float RadiansToDegrees(float radians) {
            return radians * _180OverPi;
        }

        public static float DegreesToRadians(float degrees) {
            return degrees * _PiOver180;
        }

        #endregion
    }
}
