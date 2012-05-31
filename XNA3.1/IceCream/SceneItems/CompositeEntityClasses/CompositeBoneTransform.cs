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
using System.ComponentModel;
using IceCream.Attributes;

namespace IceCream.SceneItems
{
    public class CompositeBoneTransform
    {
        #region Fields

        private Vector2 _initialPosition;
        private Vector2 _initialScale;
        private float _initialRotation;
        private byte? _initialOpacity;
        private Vector2 _transformPivot;
        
        private Vector2 _position;
        private Vector2 _scale;
        private float _rotation;
        private byte? _opacity;
        private bool _currentVisibleState;
        private bool _flipHorizontal;
        private bool _flipVertical;

        private bool _isVisible;
        private String _sceneItem;
        private String _subItem;

        private CompositeBone _compositeBone;        
        internal String _boneReference;        

        #endregion

        #region Properties

        [BrowsableAttribute(false), XmlIgnore]
        public CompositeKeyFrame Parent
        {
            get;
            set;
        }

        internal CompositeBoneTransform ParentBoneTransform
        {
            get
            {
                if (this.Bone.ParentBone != null)
                {
                    return Parent.GetBoneTransformFromKeyFrame(this.Parent, this.Bone.ParentBone.Name);
                }
                return null;
            }
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

        public Vector2 Position
        {
            get { return _initialPosition; }
            set { _initialPosition = value; }
        }
        
        public Vector2 Scale
        {
            get { return _initialScale; }
            set { _initialScale = value; }
        }
        
        public float Rotation
        {
            get { return _initialRotation; }
            set { _initialRotation = value; }
        }

        public byte? Opacity
        {
            get { return _initialOpacity; }
            set { _initialOpacity = value; }
        }

        public bool FlipHorizontal
        {
            get { return _flipHorizontal; }
            set { _flipHorizontal = value; }
        }

        public bool FlipVertical
        {
            get { return _flipVertical; }
            set { _flipVertical = value; }
        }

        [BrowsableAttribute(false)]
        public bool IsVisible
        {
            get { return _isVisible; }
            set 
            { 
                _isVisible = value;
                _currentVisibleState = value;
            }
        }

        public Vector2 CurrentPosition
        {
            get { return _position; }
        }

        public Vector2 CurrentScale
        {
            get { return _scale; }
        }

        public float CurrentRotation
        {
            get { return _rotation; }
        }

        public float CurrentOpacity
        {
            get { return _opacity.Value; }
        }

        public bool CurrentVisibility
        {
            get { return _currentVisibleState; }
        }

        public bool? InheritPosition
        {
            get;
            set;
        }

        public bool? InheritScale
        {
            get;
            set;
        }

        public bool? InheritRotation
        {
            get;
            set;
        }

        public bool? InheritVisibility
        {
            get;
            set;
        }

        [BrowsableAttribute(false), XmlIgnore]
        public CompositeBone Bone
        {
            get { return _compositeBone; }
            private set 
            { 
                _compositeBone = value;                              
            }
        }

        [BrowsableAttribute(false)]
        public String BoneReference
        {
            get { return _boneReference; }
            set 
            {
                if (_boneReference != value)
                {
                    _boneReference = value;                    
                    this.Bone = FindBone(_boneReference, Parent.Parent.Parent.RootBone);
                    if (this.Bone == null)
                    {
                        throw new Exception("BoneReference \"" + _boneReference
                            + "\" not found in the Bones list");
                    }
                }
            }
        }

        private CompositeBone FindBone(String name, CompositeBone bone)
        {
            if (bone.Name == name)
            {
                return bone;
            }
            for (int i = 0; i < bone.ChildBones.Count; i++)
            {
                CompositeBone returnBone = FindBone(name, bone.ChildBones[i]);
                if (returnBone != null)
                {
                    return returnBone;
                }
            }
            return null;
        }

        public override string ToString()
        {
            return _boneReference;
        }

        #endregion

        #region Constructor

        public CompositeBoneTransform(String sceneItemBankEntry)
        {
            _initialPosition = _position = Vector2.Zero;
            _initialScale = _scale = Vector2.One;
            _initialRotation = _rotation = 0;
            _initialOpacity = null;
            _transformPivot = Vector2.Zero;
            _isVisible = true;
            Parent = null;
            _sceneItem = sceneItemBankEntry;
            _subItem = String.Empty;
            this.InheritPosition = null;
            this.InheritRotation = null;
            this.InheritScale = null;
            this.InheritVisibility = null;
        }

        public CompositeBoneTransform()
            : this("")
        {
            
        }

        #endregion

        #region Methods

        public void CopyValuesTo(CompositeBoneTransform target, CompositeKeyFrame newParent)
        {
            target.Parent = newParent;
            target.SceneItem = this.SceneItem;
            target.SubItem = this.SubItem;
            target.Position = this.Position;
            target.Scale = this.Scale;
            target.Rotation = this.Rotation;
            target.Opacity = this.Opacity;
            target.FlipHorizontal = this.FlipHorizontal;
            target.FlipVertical = this.FlipVertical;
            target.IsVisible = this.IsVisible;
            target.BoneReference = this.BoneReference;
            target.InheritPosition = this.InheritPosition;
            target.InheritRotation = this.InheritRotation;
            target.InheritScale = this.InheritScale;
            target.InheritVisibility = this.InheritVisibility;            
        }

        public bool GetVisibilityState(CompositeBoneTransform parentTransform)
        {
            if (this.Bone.MasterVisibility.HasValue == true)
            {
                return this.Bone.MasterVisibility.Value;
            }
            if (parentTransform != null && this.InheritVisibility.HasValue == false && this.Bone.InheritVisibility == true ||
                        this.InheritVisibility.HasValue == true && this.InheritVisibility == true)
            {
                if (parentTransform.Bone.MasterVisibility.HasValue == true)
                {
                    return parentTransform.Bone.MasterVisibility.Value;
                }
                else if (parentTransform.CurrentVisibility == false)
                {
                    return false;
                }
            }
            return _isVisible;
        }

        public void LerpSceneItemWith(CompositeBoneTransform nextState, float amount, bool nextStateOverride)
        {
            // original is only used when bone.Interpolate is set to false, and refer to the 1st KF of the 1st anim
            SceneItem item = GetSceneItem();
            String subItem = GetSubItem();
            if (String.IsNullOrEmpty(subItem) == false && item is ISubItemCollection)
            {
                ((ISubItemCollection)item).SetCurrentSubItem(subItem);
            }
            if (item != null)
            {
                item.Update(1/60f);
                amount = MathHelper.Clamp(amount, 0, 1);
                CompositeEntity parentEntity = Parent.Parent.Parent;
                CompositeBoneTransform parentTransform = this.ParentBoneTransform;
                _currentVisibleState = GetVisibilityState(parentTransform);
                if (_currentVisibleState == false)
                {
                    return;
                }
                bool nextStateVisibility = nextState.GetVisibilityState(nextState.ParentBoneTransform);
                // no lerping if the next state isnt visible
                if (nextStateVisibility == false)
                {
                    nextState = this;
                }
                if (nextStateOverride == true)
                {
                    _position = nextState.Position;
                    _scale = nextState.Scale;
                    _rotation = nextState.Rotation;
                }
                else
                {
                    _position = Vector2.Lerp(this.Position, nextState.Position, amount);
                    _scale = Vector2.Lerp(this.Scale, nextState.Scale, amount);
                    _rotation = MathHelper.Lerp(this.Rotation, nextState.Rotation, amount);
                }                
                if (this.Opacity.HasValue == true || nextState.Opacity.HasValue == true)
                {
                    if (this.Opacity.HasValue == true && nextState.Opacity.HasValue == false)
                    {
                        _opacity = this.Opacity.Value;
                    }
                    else if (this.Opacity.HasValue == false && nextState.Opacity.HasValue == true)
                    {
                        _opacity = nextState.Opacity.Value;
                    }
                    else
                    {
                        _opacity = IceMath.Lerp(this.Opacity.Value, nextState.Opacity.Value, amount);
                    }
                }
                else
                {
                    _opacity = null;
                }
                _transformPivot = item.GetAbsolutePivot(false);                
                item.FlipHorizontal = parentEntity.FlipHorizontal ^ this.FlipHorizontal;
                item.FlipVertical = parentEntity.FlipVertical ^ this.FlipVertical;
                if (parentEntity.FlipHorizontal == true)
                {
                    _position.X = -_position.X;
                    _transformPivot.X = item.BoundingRectSize.X - _transformPivot.X;
                    _rotation = -_rotation;
                }
                if (parentEntity.FlipVertical == true)
                {
                    _position.Y = -_position.Y;
                    _transformPivot.Y = item.BoundingRectSize.Y - _transformPivot.Y;
                    _rotation = -_rotation;
                }                
                if (parentEntity.Scale != Vector2.One)
                {
                    _position *= parentEntity.Scale;
                }
                if (parentEntity.Rotation != 0)
                {
                    Vector2 offset = _position;
                    float length = offset.Length();
                    double angle = Math.Atan2((float)offset.Y, (float)offset.X) + parentEntity.Rotation;
                    offset.X = (float)(length * Math.Cos(angle));
                    offset.Y = (float)(length * Math.Sin(angle));
                    _position = offset;
                }
                if (parentTransform != null)
                {                    
                    if (this.InheritRotation.HasValue == false && this.Bone.InheritRotation == true ||
                        this.InheritRotation.HasValue == true && this.InheritRotation == true)
                    {
                        _rotation += parentTransform.CurrentRotation;
                    }
                    if (this.InheritScale.HasValue == false && this.Bone.InheritScale == true ||
                        this.InheritScale.HasValue == true && this.InheritScale == true)
                    {
                        _scale *= parentTransform.CurrentScale;
                    }                    
                    if (this.InheritPosition.HasValue == false && this.Bone.InheritPosition == true ||
                        this.InheritPosition.HasValue == true && this.InheritPosition == true)
                    {
                        Vector2 offset = _position;
                        float length = offset.Length();
                        double angle = Math.Atan2((float)offset.Y, (float)offset.X) + parentTransform._rotation;
                        offset.X = (float)(length * Math.Cos(angle));
                        offset.Y = (float)(length * Math.Sin(angle));
                        _position = parentTransform.CurrentPosition + offset;
                    }
                }
            }
        }

        public void Draw(float elapsed)
        {
            if (Parent == null)
            {
                throw new Exception("The Parent of this boneTransform isn't set");
            }
            SceneItem item = GetSceneItem();
            if (item != null)
            {
                if (_currentVisibleState == true)
                {
                    CompositeEntity parentEntity = Parent.Parent.Parent;
                    Vector2 tmpPivot = item.Pivot;
                    bool tmpPivotRelative = item.IsPivotRelative;
                    item.Rotation = parentEntity.Rotation + _rotation;
                    item.Scale = parentEntity.Scale * _scale;
                    item.Visible = true;
                    item.Position = parentEntity.Position + _position;
                    float parentOpacityFactor = parentEntity.Opacity / 255.0f;
                    if (_opacity.HasValue)
                    {
                        item.Opacity = (byte)(_opacity.Value * parentOpacityFactor);
                    }
                    else
                    {
                        item.Opacity = parentEntity.Opacity;
                    }
                    item.Layer = parentEntity.Layer;
                    item.Pivot = _transformPivot;
                    item.IsPivotRelative = false;
                    item.Draw(elapsed);
                    item.Pivot = tmpPivot;
                    item.IsPivotRelative = tmpPivotRelative;
                }
                else
                {
                    item.Visible = false;
                }
            }
        }

        public SceneItem GetSceneItem()
        {
            String targetItem = null;
            // use the default if no local override
            if (String.IsNullOrEmpty(_sceneItem) == true)
            {
                targetItem = this.Bone.SceneItem;
            }
            else
            {
                targetItem = _sceneItem;
            }
            CompositeEntity parentEntity = Parent.Parent.Parent;
            if (String.IsNullOrEmpty(targetItem) == false 
                && parentEntity.SceneItemBank.ContainsKey(targetItem))
            {
                return parentEntity.SceneItemBank[targetItem];
            }
            return null;
        }

        public String GetSubItem()
        {
            String targetItem = null;
            // use the default if no local override
            if (String.IsNullOrEmpty(_subItem) == true)
            {
                targetItem = this.Bone.SubItem;
            }
            else
            {
                targetItem = _subItem;
            }
            return targetItem;
        }

        public bool IsPositionCurrentlyInherited()
        {
            if (this.InheritPosition.HasValue)
            {
                return this.InheritPosition.Value;
            }
            else
            {
                return Bone.InheritPosition;
            }
        }

        #endregion
    }
}
