#if !XNATOUCH
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using System;
using System.Collections.Generic;
using System.Text;
using IceCream.Drawing;

namespace IceCream.Drawing
{
    public abstract class IceEffect : IceAsset
    {
        #region Fields

        private Effect[] _effects;
        private LinearProperty[] _parametersProperties;

        #endregion

        #region Properties

        public Effect[] Effects
        {
            get { return _effects; }
            set { _effects = value; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return DrawingManager.GraphicsDevice; }
        }

        public LinearProperty[] ParametersProperties
        {
            get { return _parametersProperties; }
            set { _parametersProperties = value; }
        }

        #endregion

        #region Constructor

        public IceEffect()
        {

        }

        public IceEffect(AssetScope scope, string name)
        {
            this.Scope = scope;
            this.Name = name;
        }


        #endregion

        #region Methods

        public override string ToString()
        {
            return Scope + " | " + this.Name;
        }

        public void Load(ContentManager contentManager, String[] assetNames)
        {
            _effects = new Effect[assetNames.Length];
            for (int i = 0; i < assetNames.Length; i++)
            {
                _effects[i] = contentManager.Load<Effect>(assetNames[i]);
            }
            LoadParameters();
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Load every EffectParameter instances
        /// </summary>
        public abstract void LoadParameters();

        public void DrawFullScreenQuad(SpriteBatch spriteBatch, Texture2D texture, Effect effect)
        {
            spriteBatch.Begin(SpriteBlendMode.None,
                    SpriteSortMode.Immediate, SaveStateMode.SaveState);
            if (effect != null)
            {
                effect.Begin();
                effect.CurrentTechnique.Passes[0].Begin();
            }
            // only render the viewport part
            spriteBatch.Draw(texture, new Rectangle(0, 0,
                DrawingManager.ViewPortSize.X, DrawingManager.ViewPortSize.Y),
                new Rectangle(0, 0, DrawingManager.ViewPortSize.X, DrawingManager.ViewPortSize.Y),
                 Color.White);
            if (effect != null)
            {
                effect.CurrentTechnique.Passes[0].End();
                effect.End();
            }
            spriteBatch.End();
        }

        public abstract void SetParameters(IceEffectParameters parameters);

        #endregion
    }
}
#endif