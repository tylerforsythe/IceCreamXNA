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

namespace IceCream.SceneItems.ParticlesClasses
{
    /// <summary>
    /// Over life percentages variations for a particle type
    /// </summary>
    public class OverLifeSettings
    {
        public LinearProperty widthOverLife;
        public LinearProperty heightOverLife;
        public LinearProperty velocityOverLife;
        public LinearProperty weightOverLife;
        public LinearProperty spinOverLife;
        public LinearProperty motionRandomOverLife;
        public LinearProperty opacityOverLife;
        public LinearProperty redTintOverLife;
        public LinearProperty greenTintOverLife;
        public LinearProperty blueTintOverLife;

        public OverLifeSettings()
        {
            float defaultValue = 1.0f;
            widthOverLife = new LinearProperty(defaultValue, "Width Over Life", 0, 10);
            heightOverLife = new LinearProperty(defaultValue, "Height Over Life", 0, 10);
            velocityOverLife = new LinearProperty(defaultValue, "Velocity Over Life", 0, 10);
            weightOverLife = new LinearProperty(defaultValue, "Weight Over Life", 0, 10);
            spinOverLife = new LinearProperty(defaultValue, "Spin Over Life", 0, 10);
            motionRandomOverLife = new LinearProperty(defaultValue, "Motion Random Over Life", 0, 10);
            opacityOverLife = new LinearProperty(defaultValue, "Opacity Over Life", 0, 10);
            redTintOverLife = new LinearProperty(defaultValue, "Red Tint Over Life", 0, 10);
            greenTintOverLife = new LinearProperty(defaultValue, "Green Tint Over Life", 0, 10);
            blueTintOverLife = new LinearProperty(defaultValue, "Blue Tint Over Life", 0, 10);
        }

        public void CopyValuesTo(OverLifeSettings overLifeSettings)
        {            
            this.widthOverLife.CopyValuesTo(overLifeSettings.widthOverLife);
            this.heightOverLife.CopyValuesTo(overLifeSettings.heightOverLife);
            this.velocityOverLife.CopyValuesTo(overLifeSettings.velocityOverLife);
            this.spinOverLife.CopyValuesTo(overLifeSettings.spinOverLife);
            this.motionRandomOverLife.CopyValuesTo(overLifeSettings.motionRandomOverLife);
            this.opacityOverLife.CopyValuesTo(overLifeSettings.opacityOverLife);
            this.redTintOverLife.CopyValuesTo(overLifeSettings.redTintOverLife);
            this.greenTintOverLife.CopyValuesTo(overLifeSettings.greenTintOverLife);
            this.blueTintOverLife.CopyValuesTo(overLifeSettings.blueTintOverLife);
        }
    }
}
