#if !XNATOUCH
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



using System;
using System.Collections.Generic;
using System.Text;
using IceCream.Drawing;

namespace IceCream.Drawing
{
    public struct PostProcessRequest
    {
        public IceEffect IceEffect;
        public int? Layer;
        public IceEffectParameters IceEffectParameters;
        public bool OwnLayerOnly;

        public PostProcessRequest(IceEffect iceEffect)
        {
            this.IceEffect = iceEffect;
            this.Layer = null;
            this.OwnLayerOnly = false;
            IceEffectParameters = new IceEffectParameters();
            IceEffectParameters.Parameter1 = 1;
            IceEffectParameters.Parameter2 = 1;
            IceEffectParameters.Parameter3 = 1;
            IceEffectParameters.Parameter4 = 1;
            IceEffectParameters.Parameter5 = 1;
            IceEffectParameters.Parameter6 = 1;
            IceEffectParameters.Parameter7 = 1;
            IceEffectParameters.Parameter8 = 1;
        }
    }
}
#endif