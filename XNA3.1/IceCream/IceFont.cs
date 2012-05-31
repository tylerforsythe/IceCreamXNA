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
    public class IceFont:IceAsset
    {
        #region Fields

        private SpriteFont _font;

        #endregion

        #region Properties

        public SpriteFont Font
        {
            get { return _font; }
            set
            {
                _font = value;
                if (_font != null && _font.DefaultCharacter.HasValue == false)
                {
                    _font.DefaultCharacter = '?';
                }
                if (FontSet != null)
                {
                    FontSet(this, EventArgs.Empty);
                }
            }
        }

        #endregion


        #region Constructor

        public IceFont()
        {

        }

        public IceFont(String name, SpriteFont font, AssetScope scope)
        {
            this.Name = name;
            this._font= font;
            this.Scope = scope;
        }
        #endregion

        #region Events

        public event EventHandler FontSet;

        #endregion
    }
}
