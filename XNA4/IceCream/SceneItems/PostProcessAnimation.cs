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
    public class PostProcessAnimation : SceneItem, IAnimation
    {
        #region Fields

        /// <summary>
        /// Event fired when the animation has finished its current loop normally (wether or not it will loop again)
        /// </summary>
        public event EventHandler EndOfAnimLoopReached;
        #if !XNATOUCH
		private IceEffect _iceEffect;
        private IceEffectParameters _iceEffectParameters;
        private PostProcessRequest _ppRequest;
#endif
		private LinearProperty[] _linearProperties;
        private int _life;
        private int _maxLife;
        private int _currentLife;
        private int _loopMax;
        private int _loopCounter;
        private bool _isPaused;
        private bool _isStopped;

       #endregion

        #region Properties

        [CategoryAttribute("Animation"), DescriptionAttribute("The duration of the animation")]
        public int Life
        {
            get { return _life; }
            set
            {
                _life = value;
                _maxLife = value * 60;
                _currentLife = IceMath.Clamp(_currentLife, 0, _maxLife);
            }
        }
        [CategoryAttribute("Animation"), DescriptionAttribute("The maximum amount of loop to perform until stopping (use 0 for infinite)")]
        public int LoopMax
        {
            get { return _loopMax; }
            set { _loopMax = value; }
        }

		#if !XNATOUCH
        public IceEffect IceEffect
        {
            get { return _iceEffect; }
            set { _iceEffect = value; }
        }
        #endif

        public LinearProperty[] LinearProperties
        {
            get { return _linearProperties; }
            set { _linearProperties = value; }
        }

        [CategoryAttribute("Animation"), DescriptionAttribute("Is the animation stopped")]
        public bool IsStopped
        {
            get { return _isStopped; }
            set { _isStopped = value; }
        }
        [CategoryAttribute("Animation"), DescriptionAttribute("Is the animation paused")]
        public bool IsPaused
        {
            get { return _isPaused; }
            set { _isPaused = value; }
        }

        [CategoryAttribute("Animation"), DescriptionAttribute("Is the animation playing")]
        public bool IsPlaying
        {
            get { return IsPaused || IsStopped; }
        }

        [CategoryAttribute("Animation"), DescriptionAttribute("Automatically play the animation upon loading the SceneItem")]
        public bool AutoPlay
        {
            get;
            set;
        }

        [CategoryAttribute("Animation"), DescriptionAttribute("Determines if the animation will be visible or not once stopped/finishing")]
        public bool HideWhenStopped { get; set; }

        public bool OwnLayerOnly
        {
            get;
            set;
        }

        #endregion

        #region Constructor

		#if !REACH
        public PostProcessAnimation()
            : this(DrawingManager.NoEffect)
        {
            
        }
        #endif
        internal override void OnRegister()
        {
            base.OnRegister();
        }
		
        #if !XNATOUCH
		public PostProcessAnimation(IceEffect iceEffect)
        {
            _iceEffect = iceEffect;
            _iceEffectParameters = new IceEffectParameters();
            _ppRequest = new PostProcessRequest();
            _ppRequest.IceEffect = _iceEffect;
            _ppRequest.IceEffectParameters = _iceEffectParameters;
            _linearProperties = new LinearProperty[8];
            for (int i = 0; i < _linearProperties.Length; i++)
            {
                _linearProperties[i] = new LinearProperty(1.0f, "Parameter " + i, 0, 10);
            }
            _currentLife = 0;
            _life = 1;
            _currentLife = 0;
            _maxLife = _life * 60;
            _loopMax = 0;
            _loopCounter = 0;
            _isPaused = false;
            _isStopped = false;
            this.AutoPlay = true;
            this.HideWhenStopped = true;
            this.OwnLayerOnly = false;
        }
        #endif
        #endregion

        #region Methods

        public override void CopyValuesTo(SceneItem target)
        {
            base.CopyValuesTo(target);
            PostProcessAnimation ppAnim = target as PostProcessAnimation;
			#if !XNATOUCH
            ppAnim.IceEffect = this.IceEffect;
            #endif
            ppAnim.Life = this.Life;
            ppAnim.LoopMax = this.LoopMax;
            ppAnim.AutoPlay = this.AutoPlay;
            ppAnim.HideWhenStopped = this.HideWhenStopped;
            ppAnim.OwnLayerOnly = this.OwnLayerOnly;
            for (int i = 0; i < _linearProperties.Length; i++)
            {
                this.LinearProperties[i].CopyValuesTo(ppAnim.LinearProperties[i]);
            }
        }

        public override void Update(float elapsed)
        {
            base.Update(elapsed);
			#if !XNATOUCH
            if (this.Visible == true && _isPaused == false && _isStopped == false)
            {
                float lerpLife = _currentLife / (float)_maxLife;
                _iceEffectParameters.Parameter1 = Particle.GetLerpValue(_linearProperties[0].Values, lerpLife);
                _iceEffectParameters.Parameter2 = Particle.GetLerpValue(_linearProperties[1].Values, lerpLife);
                _iceEffectParameters.Parameter3 = Particle.GetLerpValue(_linearProperties[2].Values, lerpLife);
                _iceEffectParameters.Parameter4 = Particle.GetLerpValue(_linearProperties[3].Values, lerpLife);
                _iceEffectParameters.Parameter5 = Particle.GetLerpValue(_linearProperties[4].Values, lerpLife);
                _iceEffectParameters.Parameter6 = Particle.GetLerpValue(_linearProperties[5].Values, lerpLife);
                _iceEffectParameters.Parameter7 = Particle.GetLerpValue(_linearProperties[6].Values, lerpLife);
                _iceEffectParameters.Parameter8 = Particle.GetLerpValue(_linearProperties[7].Values, lerpLife);              
                _ppRequest.IceEffect = _iceEffect;
                _ppRequest.IceEffectParameters = _iceEffectParameters;
                _ppRequest.OwnLayerOnly = this.OwnLayerOnly;
                _currentLife++;
                if (_currentLife > _maxLife)
                {
                    // reset the life
                    _currentLife = 0;
                    // increment the loop counter
                    _loopCounter++;
                    OnEndOfAnimLoopReached();
                    // check if we need to stop
                    if (_loopMax > 0 && _loopCounter >= _loopMax)
                    {
                        _isStopped = true;
                    }  
                }
            }
            #endif
        }

        public override void Draw(float elapsed)
        {
            if (this.Visible == false)
            {
                return;
            }
			#if !XNATOUCH
            if (_isStopped == false || this.HideWhenStopped == false)
            {
                DrawingManager.ApplyPostProcess(_ppRequest, this.Layer);
            }
            #endif
            base.Draw(elapsed);
        }

        public void Stop()
        {
            _isStopped = true;
        }

        public void Play()
        {
            if (_isPaused)
            {
                _isPaused = false;
            }
            if (_isStopped)
            {
                _isStopped = false;
                Reset();
            }
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Reset()
        {
            _loopCounter = 0;
            _currentLife = 0;
        }

        internal void OnEndOfAnimLoopReached()
        {
            if (EndOfAnimLoopReached != null)
            {
                EndOfAnimLoopReached(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
