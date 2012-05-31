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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;

namespace IceCream
{
    public static class IceCreamExtensions 
    {
        public static void AppendChildIfNotNull(this XmlNode node, XmlNode childNode)
        {
            if (childNode != null)
            {
                node.AppendChild(childNode);
            }
        }

        public static Vector2 GetSide(this Vector2 vector2)
        {
            return new Vector2(-vector2.Y, vector2.X);
        }

        public static float Angle(this Vector2 vector2)
        {
            return (float)Math.Atan2((float)vector2.Y, (float)vector2.X);
        }

        public static Vector2 Size(this Texture2D tex)
        {
            return new Vector2(tex.Width, tex.Height);
        }

        public static Vector3 ToVector3(this Vector2 vector2)
        {
            Vector3 vector3 = new Vector3();
            vector3.X = vector2.X;
            vector3.Y = vector2.Y;
            return vector3;
        }
        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.X, vector3.Y);
        }

        /// <summary>
        /// Checks if a float value is equal to another value within a given threshold limit, with extremes excluded
        /// </summary>
        /// <param name="comparisonValue">The value to compare to</param>
        /// <param name="threshold">The thresold above and below the comparaison value</param>
        /// <returns>True if the value is contained within (comparisonValue-threshold and comparisonValue+threshold)</returns>
        public static bool IsWithinValue(this float value, float comparisonValue, float threshold)
        {
            if (value < comparisonValue + threshold && value > comparisonValue - threshold)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the PlayerIndex is allowed to purchase content from the marketplace
        /// </summary>
        public static bool CanBuyGame(this PlayerIndex player)
        {
            SignedInGamer gamer = Gamer.SignedInGamers[player];
            if (gamer == null)
            {
                return false;
            }
            return gamer.Privileges.AllowPurchaseContent;
        }
    }
}
