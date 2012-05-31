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
using System.Xml.Serialization;
using System.ComponentModel;
using IceCream.Attributes;

namespace IceCream.SceneItems
{
    public class CompositeAnimation : IAnimation
    {
        #region Fields

        private List<CompositeKeyFrame> _keyFrames;
        private String _name;
        private float _currentLife;
        private int _currentKeyFrameIndex;
        private float _speed;
        private int _loopMax;
        private int _loopCounter;
        private bool _isPaused;
        private bool _isStopped;

        #endregion

        #region Properties

        [XmlIgnore, Browsable(false)]
        public CompositeEntity Parent { get; set; }

        [Browsable(false)]
        public List<CompositeKeyFrame> KeyFrames
        {
            get { return _keyFrames; }
            set { _keyFrames = value; }
        }

        [Browsable(false), CategoryAttribute("Animation"), DescriptionAttribute("The name of the animation")]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [CategoryAttribute("Animation"), DescriptionAttribute("Playback speed multiplier of CompositeAnimation")]
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

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

        [CategoryAttribute("Animation"), DescriptionAttribute("Determines if the animation will be visible or not once stopped/finishing")]
        public bool HideWhenStopped { get; set; }

        /// <summary>
        /// Flag to enable or disable interpolation of the last keyframe of an animation with the first keyframe
        /// </summary>
        public bool LerpLastFrameWithFirst
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        public CompositeAnimation(CompositeEntity parent)
        {
            _keyFrames = new List<CompositeKeyFrame>();
            _name = "New Animation";
            _currentLife = 0;
            _currentKeyFrameIndex = 0;
            _loopMax = 0;
            _loopCounter = 0;
            _speed = 1;
            this.Parent = parent;
            this.AutoPlay = true;
            this.LerpLastFrameWithFirst = true;
        }

        public CompositeAnimation()
            : this(null)
        {
            
        }

        #endregion

        #region Methods

        public void CopyValuesTo(CompositeAnimation target, CompositeEntity newParent)
        {
            target.Parent = newParent;
            target.Name = this.Name;
            target.LerpLastFrameWithFirst = this.LerpLastFrameWithFirst;
            target.IsPaused = this.IsPaused;
            target.IsStopped = this.IsStopped;
            target.LoopMax = this.LoopMax;
            target.Speed = this.Speed;
            target.AutoPlay = this.AutoPlay;
            target.HideWhenStopped = this.HideWhenStopped;
            // copy keyframes
            for (int i = 0; i < this._keyFrames.Count; i++)
            {
                // if no particle type is available
                if (target.KeyFrames.Count <= i)
                {
                    target.KeyFrames.Add(new CompositeKeyFrame(target));
                }
                this.KeyFrames[i].CopyValuesTo(target.KeyFrames[i], target);                
            }
            // Remove remaining types (can cause garbage!)
            for (int i = target.KeyFrames.Count; i > this.KeyFrames.Count; i--)
            {
                target.KeyFrames.RemoveAt(i-1);
            }
            target.Reset();
        }

        public void Stop()
        {
            _isStopped = true;
        }

        public void Play()
        {
            if (_isPaused == true)
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
            _currentKeyFrameIndex = 0;
            _currentLife = 0;
            _loopCounter = 0;
        }

        /// <summary>
        /// Reset the animation to the beginning of the specific keyFrame
        /// </summary>
        public void ResetToKeyFrame(int keyFrameIndex)
        {
            Reset();
            _isPaused = false;
            _isStopped = false;
            _currentKeyFrameIndex = keyFrameIndex;
            for (int i = 0; i < keyFrameIndex; i++)
            {               
                int duration = _keyFrames[i].Duration;
                if (duration < 1)
                {
                    duration = 1;
                }
                _currentLife += duration;                
            }
        }

        public void Draw(float elapsed)
        {
            if (_keyFrames.Count > 0)
            {
                _keyFrames[_currentKeyFrameIndex].Draw(elapsed);
            }
        }

        private void EndAnim()
        {   
            this.IsStopped = true;
            this.IsPaused = false;
        }

        public void Update(float elapsed)
        {
            if (Parent == null)
            {
                throw new Exception("The Parent of this animation isn't set");
            }
            int lastFrameIndex = _currentKeyFrameIndex;
            if (_keyFrames.Count > 0 && this.IsPlaying == true)
            {
                // Get the current frame index
                int _lifeAccumulator = 0;
                for (int i = 0; i < _keyFrames.Count; i++)
                {
                    _currentKeyFrameIndex = i;
                    int duration = _keyFrames[i].Duration;
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
                    else if (i == _keyFrames.Count - 1)
                    {
                        _currentLife = _lifeAccumulator;
                        break;
                    }
                }                
                CompositeKeyFrame currentKeyFrame = _keyFrames[_currentKeyFrameIndex];
                if (_currentKeyFrameIndex != lastFrameIndex)
                {
                    this.Parent.OnKeyFrameReached(currentKeyFrame);
                }
                CompositeKeyFrame nextKeyFrame = null;
                // get the next key frame
                if (_currentKeyFrameIndex < _keyFrames.Count - 1)
                {
                    nextKeyFrame = _keyFrames[_currentKeyFrameIndex + 1];
                }                
                // if we've reached the last frame
                else if (_currentKeyFrameIndex == _keyFrames.Count - 1)
                {
                    if (this.LerpLastFrameWithFirst == true)
                    {
                        nextKeyFrame = _keyFrames[0];
                    }
                    else
                    {
                        nextKeyFrame = currentKeyFrame;
                    }
                }
                int currentKeyFrameDuration = currentKeyFrame.Duration;
                if (currentKeyFrameDuration < 1)
                {
                    currentKeyFrameDuration = 1;
                }
                float life = _currentLife - (_lifeAccumulator - currentKeyFrameDuration);
                //if (Parent.CurrentAnimationID == 1)
                //{
                //    Console.WriteLine(Parent.CurrentAnimation.Name + "] Update key: "
                //        + _currentKeyFrameIndex + "] life: " + life + "/" + currentKeyFrameDuration);
                //}
                currentKeyFrame.LerpKeyFrameWith(nextKeyFrame, life);
                currentKeyFrame.Update(elapsed);

                _currentLife += _speed;
                // if we reach the end of the last keyframe, we need to loop
                if (_currentKeyFrameIndex == _keyFrames.Count - 1 && _lifeAccumulator <= _currentLife)                    
                {
                    if (_loopMax > 0 && _loopCounter >= _loopMax - 1)
                    {
                        EndAnim();
                    }
                    else
                    {
                        _loopCounter++;
                        _currentLife = 0;
                    }                    
                    this.Parent.OnEndOfAnimLoopReached(this);
                }
            }
        }

        #endregion
    }
}
