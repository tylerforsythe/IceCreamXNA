#if !XNATOUCH
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCream.Drawing.IceEffects
{
    public class NoEffect : IceEffect
    {
        public NoEffect()
            : base(AssetScope.Embedded,"NoEffect")
        {

        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            
        }

        public override void LoadParameters()
        {
            
        }

        public override void SetParameters(IceEffectParameters parameters)
        {
            
        }
    }
}
#endif
