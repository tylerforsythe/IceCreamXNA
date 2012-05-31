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
using IceCream.SceneItems.AnimationClasses;

namespace IceCream.SceneItems
{
    public class AnimatedSprite : Sprite, IAnimationDirector, ISubItemCollection
    {
        #region Fields

        private List<AnimationInfo> _animations;
        private int _currentAnimationID;
        private int? _queuedAnimationID;
        private Vector2 _boundingRectSize;
        private bool _drawSinceLastUpdate;
        private float _lastUpdateTime;

        #endregion

        #region Properties

        public event EventHandler EndOfAnimLoopReached;
        public List<AnimationInfo> Animations { get { return _animations; } }

        public bool AutoPlay { get; set; }

        public String DefaultAnimation { get; set; }

        public int CurrentAnimationID
        {
            get { return _currentAnimationID; }
            set { _currentAnimationID = value; }
        }

        public IAnimation CurrentAnimation
        {
            get
            {
                if (_animations == null || _animations.Count == 0)
                {
                    return null;
                }
                else
                {
                    return _animations[_currentAnimationID] as IAnimation;
                }
            }
        }

        public override Vector2 BoundingRectSize
        {
            get { return _boundingRectSize; }
            set { base.BoundingRectSize = value; }
        }

        #endregion

        #region Constructor

        public AnimatedSprite()
        {
            _animations = new List<AnimationInfo>();
            _currentAnimationID = 0;
            _boundingRectSize = new Vector2(64);
        }

        #endregion

        #region Methods

        public override void CopyValuesTo(SceneItem target)
        {
            base.CopyValuesTo(target);
            AnimatedSprite animatedSprite = target as AnimatedSprite;
            // copy animations
            for (int i = 0; i < this.Animations.Count; i++)
            {
                // if no animation is available
                if (animatedSprite.Animations.Count <= i)
                {
                    animatedSprite.AddAnimation(new AnimationInfo(String.Empty));
                }
                this.Animations[i].CopyValuesTo(animatedSprite.Animations[i], animatedSprite);
                animatedSprite.Animations[i].Parent = animatedSprite;
            }
            // Remove remaining animations (can cause garbage!)
            for (int i = animatedSprite.Animations.Count; i > this.Animations.Count; i--)
            {
                animatedSprite.Animations.RemoveAt(i - 1);
            }
            animatedSprite.AutoPlay = this.AutoPlay;            
            animatedSprite.DefaultAnimation = this.DefaultAnimation;
            animatedSprite.UpdateBoundingRectSize();
        }

        internal void OnEndOfAnimLoopReached(AnimationInfo animation)
        {
            if (_queuedAnimationID.HasValue)
            {
                animation.IsStopped = true;
                animation.Reset();
                _currentAnimationID = _queuedAnimationID.Value;
                _animations[_queuedAnimationID.Value].Play();
                UpdateBoundingRectSize();
                _queuedAnimationID = null;
                
            }
            if (EndOfAnimLoopReached != null)
            {
                EndOfAnimLoopReached(animation, EventArgs.Empty);
            }
        }

        internal void UpdateBoundingRectSize()
        {
            AnimationInfo anim = this.Animations[_currentAnimationID];
            int maxW = 0;
            int maxH = 0;
            for (int i = 0; i < anim.AnimationFrames.Count; i++)
            {
                if (this.Material.Areas.ContainsKey(anim.AnimationFrames[i].Area))
                {
                    maxW = IceMath.Max(maxW,
                        this.Material.Areas[anim.AnimationFrames[i].Area].Width);
                    maxH = IceMath.Max(maxH,
                        this.Material.Areas[anim.AnimationFrames[i].Area].Height);
                }
                else
                {
                    maxW = this.Material.Texture.Width;
                    maxH = this.Material.Texture.Height;
                    break;
                }
            }
            _boundingRectSize = new Vector2(maxW, maxH);
            this.UpdateBoundingRect();
        }

        public override List<String> GetSubItemsList()
        {
            List<String> retVal = new List<String>();
            for (int i = 0; i < this.Animations.Count; i++)
            {
                retVal.Add(this.Animations[i].Name);
            }
            return retVal;
        }

        public new string GetCurrentSubItem()
        {
            IAnimation currAnim = CurrentAnimation;
            return (currAnim != null) ? currAnim.Name : string.Empty;
        }

        public override void SetCurrentSubItem(string subItem)
        {
            SetAnimation(subItem);
        }

        public void SetAnimation(String animationName)
        {
            for (int i = 0; i < this.Animations.Count; i++)
            {
                if (this.Animations[i].Name == animationName)
                {
                    this.CurrentAnimationID = i;
                    UpdateBoundingRectSize();
                    break;
                }
            }
        }

        public void EnqueueAnimation(String animationName)
        {
            for (int i = 0; i < _animations.Count; i++)
            {
                if (_animations[i].Name == animationName && _currentAnimationID != i)
                {
                    if (_currentAnimationID < _animations.Count
                        && _animations[_currentAnimationID].IsStopped == true)
                    {
                        _currentAnimationID = i;
                        UpdateBoundingRectSize();
                        _animations[i].Play();
                    }
                    else
                    {
                        _queuedAnimationID = i;
                    }
                    break;
                }
            }
        }

        public void PlayAnimation(String animationName)
        {
            int oldAnim = _currentAnimationID;
            SetAnimation(animationName);
            if (_currentAnimationID < _animations.Count)
            {
                if (oldAnim != _currentAnimationID && oldAnim < _animations.Count
                    && _animations[oldAnim].IsStopped == false)
                {
                    _animations[oldAnim].IsStopped = true;
                    _animations[oldAnim].Reset();
                }
                if (_queuedAnimationID.HasValue)
                {
                    _queuedAnimationID = null;
                }
                _animations[_currentAnimationID].Play();
                if (_drawSinceLastUpdate == false)
                {
                    _animations[_currentAnimationID].Update(_lastUpdateTime);
                }
            }
        }

        public override void Update(float elapsed)
        {
            base.Update(elapsed);
            _lastUpdateTime = elapsed;
            int _oldAnim = _currentAnimationID;
            if (_animations.Count > 0 && _currentAnimationID < _animations.Count)
            {
                _animations[_currentAnimationID].Update(elapsed);
                // if the animation has been changed after update (= queuing)
                if (_oldAnim != _currentAnimationID)
                {
                    _animations[_currentAnimationID].Update(elapsed);
                }
                _drawSinceLastUpdate = false;
            }
            UpdateBoundingRect();
        }

        public override void Draw(float elapsed)
        {
            if (this.Visible == false)
            {
                return;
            }
            if (_animations.Count > 0 && _currentAnimationID < _animations.Count)
            {
                _drawSinceLastUpdate = true;
                AnimationInfo currentAnim = _animations[_currentAnimationID];
                if (currentAnim.IsStopped == false || currentAnim.HideWhenStopped == false)
                {
                    currentAnim.Draw(elapsed);
                }
            }
        }

        public void AddAnimation(AnimationInfo newAnimation)
        {
            _animations.Add(newAnimation);
            newAnimation.Parent = this;
            UpdateBoundingRectSize();
        }

        #endregion
    }
}
