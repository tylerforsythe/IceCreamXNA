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
    /// The GlobalDataHolder class
    /// </summary>
    /// <remarks></remarks>
    public class GlobalDataHolder : SceneBase
    {
        #region Fields

        protected Point _nativeResolution = new Point(1280, 720);
        
        #endregion

        #region Properties

        public Point NativeResolution
        {
            get { return _nativeResolution; }
            set { _nativeResolution = value; }
        }
       
        #endregion

        #region Methods

        #endregion
    }
}
