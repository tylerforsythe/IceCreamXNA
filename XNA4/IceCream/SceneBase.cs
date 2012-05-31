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
using System.ComponentModel;
using System.Collections.Generic;
using IceCream.Components;
using IceCream.Drawing;

namespace IceCream
{
    public class SceneBase
    {
        #region Fields

        private List<IceSceneComponent> _sceneComponents = new List<IceSceneComponent>();
        internal ContentManager _content;
        private List<Material> _materials = new List<Material>();
        private List<SceneItem> _sceneItems = new List<SceneItem>();
        private List<SceneItem> _templateItems = new List<SceneItem>();
        private List<IceFont> _fonts = new List<IceFont>();
        #if !XNATOUCH
		private List<IceEffect> _effects = new List<IceEffect>();
#endif
		private List<TileSheet> _tileSheets = new List<TileSheet>();

		
        #endregion

        #region Properties

        public List<IceSceneComponent> SceneComponents
        {
            get { return _sceneComponents; }
            set { _sceneComponents = value; }
        }
        public ContentManager ContentManager
        {
            get { return _content; }
        }
        public List<Material> Materials
        {
            get { return _materials; }
        }
        public List<SceneItem> SceneItems
        {
            get { return _sceneItems; }
        }
        public List<SceneItem> TemplateItems
        {
            get { return _templateItems; }
        }
        public List<IceFont> Fonts
        {
            get { return _fonts; }
            set { _fonts = value; }
        }
		#if !XNATOUCH
        public List<IceEffect> Effects
        {
            get { return _effects; }
            set { _effects = value; }
        }
#endif
        public List<TileSheet> TileSheets
        {
            get { return _tileSheets; }
            set { _tileSheets = value; }
        }

        #endregion

        #region Methods

        public void InitializeContent(IServiceProvider services)
        {
            _content = new ContentManager(services);
        }

        public void InitializeContent(IServiceProvider services, string rootpath)
        {
            _content = new ContentManager(services, rootpath);
        }

        public virtual Material GetMaterial(string name)
        {
            foreach (Material _material in _materials)
            {
                if (_material.Name == name)
                {
                    return _material;
                }
            }
            return null;
        }

        public virtual IceFont GetFont(string name)
        {
            foreach (IceFont _font in _fonts)
            {
                if (_font.Name == name)
                {
                    return _font;
                }
            }
            return null;
        }

        public virtual TileSheet GetTileSheet(string name)
        {
            foreach (TileSheet tile in _tileSheets)
            {
                if (tile.Name == name)
                {
                    return tile;
                }
            }
            return null;
        }

        public virtual Material LoadMaterial(string location, string name)
        {
            return null;
        }

        #endregion
    }
}
