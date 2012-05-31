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
using System.ComponentModel;
using IceCream.Drawing;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml;
using IceCream.Components;
using IceCream.SceneItems.ParticlesClasses;
#if !XNATOUCH
using IceCream.Drawing.IceEffects;
#endif

namespace IceCream.SceneItems
{
    public interface IAnimation
    {
        [CategoryAttribute("Animation"), DescriptionAttribute("The name of the animation")]
        String Name { get; set; }
        [CategoryAttribute("Animation"), DescriptionAttribute("The duration of the animation")]
        int Life { get; set; }
        [CategoryAttribute("Animation"), DescriptionAttribute("The maximum amount of loop to perform until stopping (use 0 for infinite)")]
        int LoopMax { get; set; }
        [CategoryAttribute("Animation"), DescriptionAttribute("Automatically play the animation upon loading the SceneItem")]
        bool AutoPlay { get; set; }
        [CategoryAttribute("Animation"), DescriptionAttribute("Is the animation paused")]
        bool IsPaused { get; }
        [CategoryAttribute("Animation"), DescriptionAttribute("Is the animation stopped")]
        bool IsStopped { get; }
        [CategoryAttribute("Animation"), DescriptionAttribute("Is the animation playing")]
        bool IsPlaying { get; }
        [CategoryAttribute("Animation"), DescriptionAttribute("Determines if the animation will be visible or not once stopped/finishing")]
        bool HideWhenStopped { get; set; }

        void Play();
        void Pause();
        void Stop();
        void Reset();
    }
}
