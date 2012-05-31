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
using IceCream.SceneItems.ParticlesClasses;
using IceCream.Drawing;
using System.Diagnostics;

namespace IceCream.SceneItems
{
    public class ParticleEffect : SceneItem, IAnimation
    {        
        #region Fields

        private Vector2 _position;
        private Emitter emitter;
        private int life;
        private Color _editorBackgroundColor;
        private int _loopMax;

        #endregion

        #region Properties

        #if WINDOWS 
        [BrowsableAttribute(false)] 
        #endif
        public Emitter Emitter
        {
            get { return emitter; }
            set { emitter = value; }
        }

        [CategoryAttribute("Animation"), DescriptionAttribute("The amount of life spent since the animation started")]
        public int Life
        {
            get { return life; }
            set { life = value; }
        }

        [CategoryAttribute("Animation"), DescriptionAttribute("The maxium amount of life")]
        public int MaxLife
        {
            get;
            set;
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                if (_position != value)
                {
                    emitter.Position = value;
                    _position = value;
                }
            }
        }

        [CategoryAttribute("Animation"), DescriptionAttribute("The maximum amount of loop to perform until stopping (use 0 for infinite)")]
        public int LoopMax
        {
            get { return _loopMax; }
            set { _loopMax = value; }
        }

        #if WINDOWS
        [BrowsableAttribute(false)]
        #endif
        public Color EditorBackgroundColor
        {
            get { return _editorBackgroundColor; }
            set { _editorBackgroundColor = value; }
        }

        [CategoryAttribute("Animation"), DescriptionAttribute("Is the effect stopped")]
        public bool IsStopped
        {
            get { return emitter.IsStopped; }
            set { emitter.IsStopped = value; }
        }
        [CategoryAttribute("Animation"), DescriptionAttribute("Is the effect paused")]
        public bool IsPaused
        {
            get { return emitter.IsPaused; }
            set { emitter.IsPaused = value; }
        }

        [CategoryAttribute("Animation"), DescriptionAttribute("Is the effect playing")]
        public bool IsPlaying
        {
            get { return !IsPaused && !IsStopped; }
        }
        [CategoryAttribute("Animation"), DescriptionAttribute("Automatically play the effect upon loading the SceneItem")]
        public bool AutoPlay
        {
            get;
            set;
        }

        [CategoryAttribute("Animation"), DescriptionAttribute("Determines if the animation will be visible or not once stopped/finishing")]
        public bool HideWhenStopped { get; set; }

        #endregion

        #region Constructor

        public ParticleEffect()
            : base()
        {            
            emitter = new Emitter();
            life = 60;
            _loopMax = 0;
            _editorBackgroundColor = new Color(0, 0, 0, 255);
        }

        #endregion

        #region Methods

        public override void CopyValuesTo(SceneItem target)
        {
            base.CopyValuesTo(target);
            ParticleEffect effect = target as ParticleEffect;
            effect.Life = this.Life;
            effect.LoopMax = this.LoopMax;
            effect.MaxLife = this.MaxLife;
            effect.AutoPlay = this.AutoPlay;
            effect.HideWhenStopped = this.HideWhenStopped;
            effect.EditorBackgroundColor = this.EditorBackgroundColor;
            this.Emitter.CopyValuesTo(effect.Emitter);
        }

        public void Stop()
        {
            emitter.IsStopped = true;
        }

        public void Play()
        {
            if (emitter.IsPaused)
            {
                emitter.IsPaused = false;
            }
            if (emitter.IsStopped)
            {
                emitter.IsStopped = false;
                this.Reset();
            }
        }

        public void Pause()
        {
            emitter.IsPaused = true;
        }

        public void Reset()
        {
            emitter.Initialize(this.Life);
        }

        public override void Update(float elapsed)
        {
            base.Update(elapsed);
            if (emitter.Parent != this)
            {
                emitter.Parent = this;
            }
            emitter.Update(elapsed);
            UpdateBoundingRect();
        }

        internal override void OnRegister()
        {
            this.Reset();
            base.OnRegister();
        }

        internal override void OnUnRegister()
        {            
            base.OnUnRegister();
        }

        public override void UpdateBoundingRect()
        {
            int size = 40;
            this.BoundingRect = new Rectangle((int)(Position.X - 40 / 2), (int)(Position.Y - 40 / 2), size, size);
            
        }

        public override void Draw(float elapsed)
        {
            if (this.Visible == false)
            {
                return;
            }
            if (this.IsStopped == false || this.HideWhenStopped == false)
            {
                emitter.Draw();
            }
            base.Draw(elapsed);
        }

        public override string ToString()
        {
            String status = "ParticleEffect: { ";
            status += emitter.ToString();

            status += " }";
            return status;
        }

        #endregion        
    }
}
