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
    public class CompositeBone
    {
        #region Fields

        private String _name;
        private List<CompositeBone> _childBones;
        private CompositeBone _parentBone;
        private String _sceneItem;
        private String _subItem;
                
        #endregion

        #region Properties

        [BrowsableAttribute(false), XmlIgnore]
        public CompositeEntity Parent
        {
            get;
            set;
        }

        [BrowsableAttribute(false)]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [BrowsableAttribute(false)]
        public List<CompositeBone> ChildBones
        {
            get { return _childBones; }
        }

        [BrowsableAttribute(false), XmlIgnore]
        public CompositeBone ParentBone
        {
            get { return _parentBone; }
            set { _parentBone = value; }
        }

        #if WINDOWS
        [TypeConverter(typeof(CompositeEntityClasses.SceneItemRefConverter))]
        #endif
        public String SceneItem
        {
            get { return _sceneItem; }
            set { _sceneItem = value; }
        }

        #if WINDOWS
        [TypeConverter(typeof(CompositeEntityClasses.SubItemRefConverter))]
        #endif
        public String SubItem
        {
            get { return _subItem; }
            set { _subItem = value; }
        } 

        public bool? MasterVisibility
        {
            get;
            set;
        }

        public bool InheritPosition
        {
            get;
            set;
        }

        public bool InheritScale
        {
            get;
            set;
        }

        public bool InheritRotation
        {
            get;
            set;
        }

        public bool InheritVisibility
        {
            get;
            set;
        }

        public bool Interpolate
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        public CompositeBone(String sceneItemBankEntry, String name)
        {
            _name = name;
            _parentBone = null;
            _sceneItem = sceneItemBankEntry;
            _subItem = String.Empty;
            _childBones = new List<CompositeBone>();
            this.MasterVisibility = null;
            this.Interpolate = true;
            this.InheritPosition = true;
            this.InheritRotation = true;
            this.InheritScale = true;
            this.InheritVisibility = true;
        }

        public CompositeBone()
            : this(null, "")
        {
            
        }

        #endregion

        #region Methods

        public void CopyValuesTo(CompositeBone target, CompositeBone parentBone,
            CompositeEntity newParentEntity)
        {
            target.Parent = newParentEntity;
            target.ParentBone = parentBone;
            target.Name = this.Name;
            target.SceneItem = this.SceneItem;
            target.SubItem = this.SubItem;
            target.InheritPosition = this.InheritPosition;
            target.InheritRotation = this.InheritRotation;
            target.InheritScale = this.InheritScale;
            target.InheritVisibility = this.InheritVisibility;
            target.MasterVisibility = this.MasterVisibility;
            target.Interpolate = this.Interpolate;
            // copy bones
            for (int i = 0; i < this.ChildBones.Count; i++)
            {
                CompositeBone targetCBone;
                bool addToList = false;
                // if no child bone is available, create a new one
                if (target.ChildBones.Count <= i)
                {
                    targetCBone = new CompositeBone();
                    addToList = true;
                }
                else
                {
                    targetCBone = target.ChildBones[i];
                }
                this.ChildBones[i].CopyValuesTo(targetCBone, target, newParentEntity);
                if (addToList == true)
                {
                    target.AddChildBone(targetCBone);
                }
            }
            // Remove remaining types (can cause garbage!)
            for (int i = target.ChildBones.Count; i > this.ChildBones.Count; i--)
            {                
                target.ChildBones.RemoveAt(i-1);
            }
        }

        /// <summary>
        /// Removes a children from this bone
        /// </summary>
        public void RemoveChildBone(CompositeBone childBone)
        {
            // sync transforms
            for (int i = 0; i < Parent.Animations.Count; i++)
            {
                // loop through every keyframe to sync them
                for (int j = 0; j < Parent.Animations[i].KeyFrames.Count; j++)
                {
                    CompositeKeyFrame keyframe = Parent.Animations[i].KeyFrames[j];
                    // remove the entry AND its children entries
                    RemoveBoneTransformEntry(keyframe.BoneTransforms, childBone);
                }
            }
            // remove the bone
            this.ChildBones.Remove(childBone);
        }

        private void RemoveBoneTransformEntry(List<CompositeBoneTransform> boneTransforms, CompositeBone bone)
        {
            String boneRefToRemove = bone.Name;
            // remove this bone's entry from the list
            for (int i = 0; i < boneTransforms.Count; i++)
            {
                if (boneTransforms[i].BoneReference == boneRefToRemove)
                {
                    Console.WriteLine("Removing bone |" + boneRefToRemove + "| from transforms");
                    boneTransforms.RemoveAt(i);
                    break;
                }
            }
            // remove each child bone too
            for (int i = 0; i < bone.ChildBones.Count; i++)
            {
                RemoveBoneTransformEntry(boneTransforms, bone.ChildBones[i]);
            }
        }        

        /// <summary>
        /// Insert a children bone at specific index
        /// </summary>
        public void InsertChildBone(int index, CompositeBone childBone)
        {
            index = IceMath.Clamp(index, 0, _childBones.Count);
            _childBones.Insert(index, childBone);
            childBone.ParentBone = this;
            childBone.Parent = this.Parent;
            CompositeBone precedingBone;
            if (index == 0)
            {
                precedingBone = this;
            }
            else
            {
                precedingBone = this.ChildBones[index - 1];
            }                        
            // sync transforms
            for (int i = 0; i < Parent.Animations.Count; i++)
            {
                // loop through every keyframe to sync them
                for (int j = 0; j < Parent.Animations[i].KeyFrames.Count; j++)
                {
                    CompositeKeyFrame keyframe = Parent.Animations[i].KeyFrames[j];
                    // loop to find the previous bone
                    for (int k = 0; k < keyframe.BoneTransforms.Count; k++)
                    {
                        CompositeBoneTransform transform = keyframe.BoneTransforms[k];                                               
                        if (transform.Bone.Equals(precedingBone))
                        {
                            CompositeBoneTransform newTransform = new CompositeBoneTransform();
                            newTransform.Parent = keyframe;
                            newTransform.BoneReference = childBone.Name;         
                            // insert the new bone just after the preceding bone
                            keyframe.BoneTransforms.Insert(k + 1, newTransform);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add a children to this bone
        /// </summary>
        public void AddChildBone(CompositeBone childBone)
        {
            // insert the children at the end
            InsertChildBone(_childBones.Count, childBone);
        }

        /// <summary>
        /// Recurisvely search the bone and its children for this name, and return null if didn't find any match
        /// </summary>
        internal CompositeBone FindBone(String boneName)
        {
            if (this.Name == boneName)
            {
                return this;
            }
            foreach (CompositeBone childBone in this.ChildBones)
            {
                CompositeBone testBone = childBone.FindBone(boneName);
                if (testBone != null)
                {
                    return testBone;
                }
            }
            return null;
        }

        public override string ToString()
        {
            return _name;
        }

        #endregion
    }
}
