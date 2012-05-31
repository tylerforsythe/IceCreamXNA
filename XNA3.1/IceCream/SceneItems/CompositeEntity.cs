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
using System.Reflection;
using IceCream.Drawing;
using System.Xml.Serialization;
using System.Xml;
using IceCream.Components;
using IceCream.Attributes;
using IceCream.SceneItems;

namespace IceCream.SceneItems
{
    public class CompositeEntity : SceneItem, IAnimationDirector, ISubItemCollection
    {
        #region Fields

        /// <summary>
        /// Event fired when the animation has finished its current loop normally (wether or not it will loop again)
        /// </summary>
        public event EventHandler EndOfAnimLoopReached;
        public event EventHandler KeyFrameReached;
        private Dictionary<String, SceneItem> _sceneItemBank;
        private CompositeBone _rootBone;
        private List<CompositeAnimation> _animations;
        private int _currentAnimationID;
        private int? _queuedAnimationID;
        private bool _drawSinceLastUpdate;
        private float _lastUpdateTime;

        #endregion

        #region Properties

        public Dictionary<String, SceneItem> SceneItemBank
        {
            get { return _sceneItemBank; }
            set { _sceneItemBank = value; }
        }

        public List<CompositeAnimation> Animations
        {
            get { return _animations; }
            set { _animations = value; }
        }

        public CompositeBone RootBone
        {
            get { return _rootBone; }
            internal set { _rootBone = value; }
        }        

        public int CurrentAnimationID
        {
            get { return _currentAnimationID; }
            set { _currentAnimationID = value; }
        }

        public bool AutoPlay { get; set; }

        public String DefaultAnimation { get; set; }

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
                    return _animations[_currentAnimationID];
                }
            }
        }

        #endregion

        #region Constructors

        public CompositeEntity()
            : base()
        {
            _sceneItemBank = new Dictionary<String, SceneItem>();
            _animations = new List<CompositeAnimation>();
            _rootBone = new CompositeBone();
            _rootBone.Name = "Root";
            this.AutoPlay = true;
            _queuedAnimationID = null;
        }

        public override void UpdateBoundingRect()
        {
            int halfSize = 32;
            this.BoundingRect = new Rectangle((int)(Position.X) - halfSize, (int)(Position.Y) - halfSize, halfSize*2, halfSize*2);
        }

        #endregion

        #region Methods

        public override void CopyValuesTo(SceneItem target)
        {
            base.CopyValuesTo(target);
            CompositeEntity compositeEntity = target as CompositeEntity;
            if (this.RootBone == null)
            {
                compositeEntity.RootBone = null;
            }
            else
            {
                this.RootBone.CopyValuesTo(compositeEntity.RootBone, null, compositeEntity);
            }
            
            // copy animations
            for (int i = 0; i < this._animations.Count; i++)
            {
                // if no animation is available
                if (compositeEntity.Animations.Count <= i)
                {
                    compositeEntity.Animations.Add(new CompositeAnimation(compositeEntity));
                }
                this.Animations[i].CopyValuesTo(compositeEntity.Animations[i], compositeEntity);
                compositeEntity.Animations[i].Parent = compositeEntity;
            }
            // Remove remaining animations (can cause garbage!)
            for (int i = compositeEntity.Animations.Count; i > this.Animations.Count; i--)
            {
                compositeEntity.Animations.RemoveAt(i-1);
            }
            // copy bank
            foreach (String key in this.SceneItemBank.Keys)
            {       
                // if the entry is not available, create it
                if (compositeEntity.SceneItemBank.ContainsKey(key) == false ||
                    this.SceneItemBank[key].GetType() != compositeEntity.SceneItemBank[key].GetType())
                {                    
                    //Can't just create a blank sceneitem. it won't create a new instance of AnimatedSprite for instance
                    SceneItem newItem = CreateNewInstaceCopyOf(this.SceneItemBank[key]);
                    compositeEntity.SceneItemBank[key] = newItem;
                }
                this.SceneItemBank[key].CopyValuesTo(compositeEntity.SceneItemBank[key]);
            }
            // Remove remaining unused key (can cause garbage!)
            List<String> keysToRemove = new List<string>();
            foreach (String key in compositeEntity.SceneItemBank.Keys)
            {
                if (this.SceneItemBank.ContainsKey(key) == false)
                {
                    keysToRemove.Add(key);                    
                }
            }
            foreach (String key in keysToRemove)
            {
                compositeEntity.SceneItemBank.Remove(key);
            }            
        }

        internal void OnEndOfAnimLoopReached(CompositeAnimation compositeAnimation)
        {
            if (EndOfAnimLoopReached != null)
            {
                EndOfAnimLoopReached(compositeAnimation, EventArgs.Empty);
            }
            if (_queuedAnimationID.HasValue)
            {
                compositeAnimation.IsStopped = true;
                compositeAnimation.Reset();
                _currentAnimationID = _queuedAnimationID.Value;
                _animations[_queuedAnimationID.Value].Play();
                _queuedAnimationID = null;                
            }            
        }

        internal void OnKeyFrameReached(CompositeKeyFrame keyFrame)
        {
            if (KeyFrameReached != null)
            {
                KeyFrameReached(keyFrame, EventArgs.Empty);
            }
        }

        private SceneItem CreateNewInstaceCopyOf(SceneItem item)
        {
            SceneItem copy = (SceneItem)item.GetType().Assembly.CreateInstance(item.GetType().FullName);
            item.CopyValuesTo(copy);
            return copy;
        }

        public void SetAnimation(String animationName)
        {
            _currentAnimationID = 0;
            for (int i = 0; i < _animations.Count; i++)
            {
                if (_animations[i].Name == animationName)
                {
                    _currentAnimationID = i;
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

        public override void Draw(float elapsed)
        {
            if (this.Visible == true && _currentAnimationID >= 0 && _currentAnimationID < _animations.Count
                && (_animations[_currentAnimationID].IsStopped == false
                || _animations[_currentAnimationID].HideWhenStopped == false))
            {
                _drawSinceLastUpdate = true;
                _animations[_currentAnimationID].Draw(elapsed);
            }
            base.Draw(elapsed);
        }

        public override void Update(float elapsed)
        {
            _lastUpdateTime = elapsed;
            int _oldAnim = _currentAnimationID;
            if (_currentAnimationID >= 0 && _currentAnimationID < _animations.Count)
            {
                _animations[_currentAnimationID].Update(elapsed);
                // if the animation has been changed after update (= queuing)
                if (_oldAnim != _currentAnimationID)
                {
                    _animations[_currentAnimationID].Update(elapsed);
                }
                _drawSinceLastUpdate = false;
            }
            base.Update(elapsed);
        }

        /// <summary>
        /// Recurisvely search every bones of the entity for this name and return it,
        /// or null if didn't find any match
        /// </summary>
        public CompositeBone FindBone(String boneName)
        {
            return this.RootBone.FindBone(boneName);
        }

        internal override void OnRegister()
        {
            base.OnRegister();            
        }

        public List<String> GetSubItemsList()
        {
            List<String> retVal = new List<String>();
            for (int i = 0; i < this.Animations.Count; i++)
            {
                retVal.Add(this.Animations[i].Name);
            }
            return retVal;
        }

        public string GetCurrentSubItem()
        {
            IAnimation currAnim = CurrentAnimation;
            return (currAnim != null) ? currAnim.Name : string.Empty;
        }

        public void SetCurrentSubItem(string subItem)
        {
            SetAnimation(subItem);
        }

        #endregion
    }
}
