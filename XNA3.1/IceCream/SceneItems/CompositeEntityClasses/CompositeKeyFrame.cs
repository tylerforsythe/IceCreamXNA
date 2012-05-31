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
using System.Xml.Serialization;
using IceCream;
using IceCream.Drawing;
using IceCream.SceneItems;

namespace IceCream.SceneItems
{
    public class CompositeKeyFrame
    {
        #region Fields

        private List<CompositeBoneTransform> _boneTransforms;
        private String _name;
        private int _duration;

        #endregion

        #region Properties

        [XmlIgnore]
        public CompositeAnimation Parent
        {
            get;
            set;
        }

        public List<CompositeBoneTransform> BoneTransforms
        {
            get { return _boneTransforms; }
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        #endregion

        #region Constructor

        public CompositeKeyFrame(CompositeAnimation parentAnimation)
        {
            _name = "New keyFrame";
            _boneTransforms = new List<CompositeBoneTransform>();
            _duration = 100;
            Parent = parentAnimation;
        }

        public CompositeKeyFrame()
            : this(null)
        {

        }

        #endregion

        #region Methods

        public void CopyValuesTo(CompositeKeyFrame target, CompositeAnimation newParent)
        {
            target.Parent = newParent;
            target.Name = this.Name;
            target.Duration = this.Duration;
            // copy CompositeBoneTransforms            
            for (int i = 0; i < this.BoneTransforms.Count; i++)
            {
                // if no transform is available
                if (target.BoneTransforms.Count <= i)
                {
                    target.BoneTransforms.Add(new CompositeBoneTransform());
                }
                this.BoneTransforms[i].CopyValuesTo(target.BoneTransforms[i], target);
            }
            // Remove remaining types (can cause garbage!)
            for (int i = target.BoneTransforms.Count; i > this.BoneTransforms.Count; i--)
            {
                target.BoneTransforms.RemoveAt(i - 1);
            }
        }

        private void AddBoneTransformFromBone(CompositeBone bone)
        {
            CompositeBoneTransform newBoneTrans = new CompositeBoneTransform("");
            newBoneTrans.Parent = this;
            newBoneTrans.BoneReference = bone.Name;
            _boneTransforms.Add(newBoneTrans);
            for (int i = 0; i < bone.ChildBones.Count; i++)
            {
                AddBoneTransformFromBone(bone.ChildBones[i]);
            }
        }

        public void GenerateDefaultBoneTransformsList()
        {
            _boneTransforms.Clear();
            CompositeBone root = this.Parent.Parent.RootBone;
            if (root != null)
            {
                AddBoneTransformFromBone(root);
            }
        }

        public CompositeBoneTransform GetBoneTransformFromKeyFrame(CompositeKeyFrame keyFrame, String boneReference)
        {
            foreach (CompositeBoneTransform boneTrans in keyFrame.BoneTransforms)
            {
                if (boneTrans.BoneReference == boneReference)
                {
                    return boneTrans;
                }
            }
            throw new Exception("BoneReference \"" + boneReference
                + "\" not found in keyframe \"" + keyFrame.Name + "\"");
        }

        public void LerpKeyFrameWith(CompositeKeyFrame nextFrame, float currentLife)
        {
            float amount = MathHelper.Clamp(currentLife / this._duration, 0, 1);            
            //Console.WriteLine("--- Lerping frame with amount: " + amount);
            if (Parent.Parent.RootBone != null)
            {
                // call the LerpBone on the root, and it will spread to all child bones using Recursivity
                CompositeBoneTransform boneTransform = GetBoneTransformFromKeyFrame(this, Parent.Parent.RootBone.Name);
                LerpBone(boneTransform, nextFrame, amount);
            }
        }

        public void LerpBone(CompositeBoneTransform boneTransform, CompositeKeyFrame nextFrame, float amount)
        {
            CompositeBoneTransform nextTransform;
            if (boneTransform.Bone.Interpolate == true)
            {
                nextTransform = GetBoneTransformFromKeyFrame(nextFrame, boneTransform.BoneReference);
            }
            else
            {
                nextTransform = GetBoneTransformFromKeyFrame(
                    this.Parent.Parent.Animations[0].KeyFrames[0], boneTransform.BoneReference);
            }            
            boneTransform.LerpSceneItemWith(nextTransform, amount, !boneTransform.Bone.Interpolate);
            foreach (CompositeBone bone in boneTransform.Bone.ChildBones)
            {
                CompositeBoneTransform childBoneTransform = GetBoneTransformFromKeyFrame(this, bone.Name);
                LerpBone(childBoneTransform, nextFrame, amount);
            }
        }

        public void Draw(float elapsed)
        {
            // Simply draw in the List's order, as it is the draw order
            foreach (var boneTransform in _boneTransforms)
            {
                boneTransform.Draw(elapsed);
            }
        }

        public void Update(float elapsed)
        {
            if (Parent == null)
            {
                throw new Exception("The Parent of this key frame isn't set");
            }
        }

        public void AddCompositeBoneTransform(CompositeBoneTransform compositeBoneTransform)
        {
            BoneTransforms.Add(compositeBoneTransform);
            compositeBoneTransform.Parent = this;
        }

        public CompositeBoneTransform GetBoneTransformContainingSceneItem(String sceneItem)
        {
            foreach (CompositeBoneTransform boneTrans in this.BoneTransforms)
            {
                if ((String.IsNullOrEmpty(boneTrans.SceneItem) && boneTrans.Bone.SceneItem == sceneItem) 
                    || boneTrans.SceneItem == sceneItem)
                {
                    return boneTrans;
                }
            }
            return null;
        }

        #endregion
    }
}
