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

namespace IceCream
{
    /// <summary>
    /// A static class to generate pseudo random numbers
    /// </summary>
    static public class Randomizer
    {
        // The generator, must be "seeded"
        static private Random generator = new Random(1337);

        /// <summary>
        /// Return a (float) random number, with decimals, between a and b included
        /// </summary>
        static public float Float(float a, float b)
        {
            return MathHelper.Lerp(a, b, (float)generator.NextDouble());
        }

        /// <summary>
        /// Returns a random integer number between a and b included
        /// </summary>
        static public int Integer(int a, int b)
        {
            // b is exclusive and a inclusive, so we add 1 to b
            return generator.Next(a, b+1);
        }

        /// <summary>
        /// Returns a random angle in degree between 0° and 360°
        /// </summary>
        /// <returns></returns>
        static public float AngleInDegrees()
        {
            return 360.0f * (float)generator.NextDouble();
        }

        /// <summary>
        /// Returns a random angle in radians between 0 and 2PI
        /// </summary>
        /// <returns></returns>
        static public float AngleInRadians()
        {
            return (2 * (float)Math.PI) * (float)generator.NextDouble();
        }

    }
}
