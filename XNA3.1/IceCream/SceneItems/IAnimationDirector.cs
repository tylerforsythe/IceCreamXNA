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
using System.ComponentModel;
using IceCream.Drawing;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml;
using IceCream.Components;
using IceCream.Attributes;
using IceCream.SceneItems.ParticlesClasses;
#if !XNATOUCH
using IceCream.Drawing.IceEffects;
#endif

namespace IceCream.SceneItems
{
    public interface IAnimationDirector
    {
        [CategoryAttribute("Animation Director"), DescriptionAttribute("Automatically play the default animation upon loading the SceneItem")]
        bool AutoPlay { get; set; }
        [CategoryAttribute("Animation Director"), DescriptionAttribute("Default animation name")]
        String DefaultAnimation { get; set; }
        [CategoryAttribute("Animation Director"), DescriptionAttribute("Current animation used by the Director")]
        IAnimation CurrentAnimation { get; }

        void SetAnimation(String animationName);
        void PlayAnimation(String animationName);
        void EnqueueAnimation(String animationName);
    }
}
