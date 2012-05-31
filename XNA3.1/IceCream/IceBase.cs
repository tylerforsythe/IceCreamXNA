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
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace IceCream
{
    /// <summary>
    /// A base class for anything with a name in the IceCream framework
    /// </summary>
    public class IceBase
    {
        #region Fields

        private string _name;
        internal int id;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the name used to indentify the item
        /// </summary>
        #if WINDOWS 
        [CategoryAttribute("Design"), DescriptionAttribute("Indicates the name used to indentify the item"),Browsable(true) ]
        #endif
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion
    }
}
