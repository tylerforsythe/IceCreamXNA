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
using System.Xml.Serialization;
using System.Text;
using System.ComponentModel;
using IceCream.Drawing;
using IceCream.SceneItems;
using IceCream.SceneItems.AnimationClasses;
using IceCream.Attributes;

namespace IceCream.SceneItems.AnimationClasses
{
    /// <summary>
    /// Holds information about an Animation
    /// </summary>
    /// <remarks>Used for seperating animations on an AnimatedSprite such as Run, Walk, Jump</remarks>
    public class AnimationInfo : IAnimation
    {
        #region Fields

        private int _currentLife;
        private int _loopMax;
        private int _loopCounter;
        private bool _isPaused;
        private bool _isStopped;
        private int _currentFrameIndex;

        #endregion

        #region Properties

        [Browsable(false), CategoryAttribute("Animation"), DescriptionAttribute("The name of the animation")]       
        public String Name { get; set; }

        public List<AnimationFrame> AnimationFrames { get; set; }

        [Browsable(false), CategoryAttribute("Animation"), DescriptionAttribute("Unused for CompositeAnimation")]
        public int Life
        {
            get { return -1; }
            set { }
        }
        [CategoryAttribute("Animation"), DescriptionAttribute("The maximum amount of loop to perform until stopping (use 0 for infinite)")]
        public int LoopMax
        {
            get { return _loopMax; }
            set { _loopMax = value; }
        }

        [Browsable(false), CategoryAttribute("Animation"), DescriptionAttribute("Is the animation stopped")]
        public bool IsStopped
        {
            get { return _isStopped; }
            set { _isStopped = value; }
        }
        [Browsable(false), CategoryAttribute("Animation"), DescriptionAttribute("Is the animation paused")]
        public bool IsPaused
        {
            get { return _isPaused; }
            set { _isPaused = value; }
        }

        [Browsable(false), CategoryAttribute("Animation"), DescriptionAttribute("Is the animation playing")]
        public bool IsPlaying
        {
            get { return !IsPaused && !IsStopped; }
        }

        [CategoryAttribute("Animation"), DescriptionAttribute("Automatically play the animation upon loading the SceneItem")]
        public bool AutoPlay
        {
            get;
            set;
        }

        public int CurrentFrameIndex 
        { 
            get { return _currentFrameIndex; } 
        }

        public AnimatedSprite Parent { get; set; }

        [CategoryAttribute("Animation"), DescriptionAttribute("Determines if the animation will be visible or not once stopped/finishing")]
        public bool HideWhenStopped { get; set; }

        #endregion

        #region Constructor

        public AnimationInfo(String name)
        {
            this.AnimationFrames = new List<AnimationFrame>();
            this.AutoPlay = true;
            this.Name = name;
        }

        #endregion

        #region Methods
        
        public void CopyValuesTo(AnimationInfo target, AnimatedSprite newParent)
        {
            target.Parent = newParent;
            target.Name = Name;
            target.IsPaused = this.IsPaused;
            target.IsStopped = this.IsStopped;
            target.LoopMax = this.LoopMax;
            target.AutoPlay = this.AutoPlay;
            target.HideWhenStopped = this.HideWhenStopped;
            // copy frames (it's a struct, so it's copied by value)
            target.AnimationFrames.Clear();
            for (int i = 0; i < this.AnimationFrames.Count; i++)
            {
                target.AnimationFrames.Add(this.AnimationFrames[i]);
            }
            target.Reset();
        }

        public void Update(float elapsed)
        {
            if (Parent == null)
            {
                throw new Exception("The Parent of this animation isn't set");
            }
            if (this.AnimationFrames.Count > 0 && this.IsPlaying == true)
            {
                // Get the current frame index
                int _lifeAccumulator = 0;
                for (int i = 0; i < this.AnimationFrames.Count; i++)
                {
                    _currentFrameIndex = i;
                    int duration = this.AnimationFrames[i].Duration;
                    if (duration < 1)
                    {
                        duration = 1;
                    }
                    _lifeAccumulator += duration;
                    if (_lifeAccumulator > _currentLife)
                    {
                        break;
                    }
                    // if we went over the maxium
                    else if (i == this.AnimationFrames.Count - 1)
                    {
                        _currentLife = _lifeAccumulator;
                        break;
                    }
                }
                _currentLife += 1;
                AnimationFrame currentAnimationFrame = this.AnimationFrames[_currentFrameIndex];
                if (_currentFrameIndex == this.AnimationFrames.Count - 1 && _lifeAccumulator <= _currentLife)
                {
                    _loopCounter++;
                    if (_loopMax > 0 && _loopCounter == _loopMax)
                    {
                        this.IsStopped = true;
                        this.IsPaused = false;
                    }
                    else
                    {
                        _currentFrameIndex = 0;
                        _currentLife = 0;
                    }
                    this.Parent.OnEndOfAnimLoopReached(this);
                }
            }
        }

        public void Draw(float elapsed)
        {
            if (this.Parent.Material == null)
            {
                return;
            }
            DrawRequest _drawRequest = new DrawRequest();
            _drawRequest.texture = this.Parent.Material.Texture;
            _drawRequest.position = this.Parent.Position;
            _drawRequest.rotation = this.Parent.Rotation;
            _drawRequest.scaleRatio = this.Parent.Scale;
            if (this.AnimationFrames != null && this.AnimationFrames.Count > _currentFrameIndex &&
                this.Parent.Material.Areas.ContainsKey(this.AnimationFrames[_currentFrameIndex].Area))
            {
                _drawRequest.sourceRectangle = this.Parent.Material.
                    Areas[this.AnimationFrames[_currentFrameIndex].Area];
            }
            else
            {
                _drawRequest.sourceRectangle = null;
            }
            _drawRequest.pivot = this.Parent.Pivot;
            _drawRequest.isPivotRelative = this.Parent.IsPivotRelative;
            _drawRequest.tint = this.Parent.Tint;
            _drawRequest.hFlip = this.Parent.FlipHorizontal;
            _drawRequest.vFlip = this.Parent.FlipVertical;
            DrawingManager.DrawOnLayer(_drawRequest, this.Parent.Layer, this.Parent.BlendingType);
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
            if (_isStopped == true)
            {
                _isStopped = false;
                Reset();
            }
        }

        public void Pause()
        {
            _isPaused = true;
        }

        /// <summary>
        /// Reset the animation to the beginning
        /// </summary>
        public void Reset()
        {
            _currentLife = 0;
            _loopCounter = 0;
        }

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}
