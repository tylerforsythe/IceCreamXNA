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
        protected bool _autoSignIntoLive = true;
        protected string _contentFolderPath = "Content";
        
        #endregion

        #region Properties

        public Point NativeResolution
        {
            get { return _nativeResolution; }
            set { _nativeResolution = value; }
        }

        public bool AutoSignIntoLive {
            get { return _autoSignIntoLive; }
            set { _autoSignIntoLive = value; }
        }

        public string ContentFolderPath {
            get { return _contentFolderPath; }
            set { _contentFolderPath = value; }
        }
       
        #endregion

        #region Methods

        #endregion
    }
}
