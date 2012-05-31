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
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IceCream
{
    /// <summary>
    /// The enum represents the Scene scope of the Asset
    /// </summary>
    public enum AssetScope
    {
        Embedded,
        Global,
        Local,
    }
    /// <summary>
    /// The IceAsset class is a base class for any IceAsset in the IceCream framework.
    /// </summary>
    public abstract class IceAsset
    {
        /// <summary>
        /// Gets or Sets the Name of the Asset
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or Sets the Scope of the Asset
        /// </summary>
        public AssetScope Scope { get; set; }  
        /// <summary>
        /// Gets or Sets the Filename of the Asset
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// Gets or Sets the Parent SceneBase of the Asset
        /// </summary>
        [XmlIgnore]
        public SceneBase Parent { get; set; }
    }
}
