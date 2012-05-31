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

namespace IceCream
{
    public class LinkPoint
    {
        #region Fields
        internal Dictionary<string, string> Mounts{get;set;}
        private SceneItem _owner;
        private string _name;
        internal bool _isTemplate = false;
        private Vector2 _offset;
        private bool _linkRotation = true;
        private bool _linkPositionToOwnerRotation = true;
        private List<LinkPoint> _mountedChildLinkPoints = new List<LinkPoint>();
        private float _lastOwnerRotation = 0;
        private Vector2 _ownerInducedRotationOffset = Vector2.Zero;
        private float _offsetVectorLenght = 0;
        private float _offsetVectorAngle = 0;
        private bool _calculateOwnerInducedRotationOffset = false;
        private float _rotation = 0;

        #endregion

        #region Properties

        [XmlIgnore]
        public SceneItem Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public Vector2 Offset
        {
            get { return _offset; }
            set 
            {
                if (_offset != value)
                {
                    _offset = value;
                    if (LinkPositionToOwnerRotation == true)
                    {
                        _offsetVectorLenght = _offset.Length();
                        _offsetVectorAngle = (float)Math.Atan2((float)_offset.Y, (float)_offset.X);
                        _calculateOwnerInducedRotationOffset = true;
                    }
                }
            }
        }

        public Vector2 Position
        {
            get
            {
                Vector2 offset = _offset;
                Vector2 _worldPos=IceVectorUtil.VectorToWorldSpace
                                    (
                                    offset, 
                                    IceVectorUtil.ConvertToCartesianCoordinates(1, Owner.Rotation)
                                    );
                
                if (LinkPositionToOwnerRotation == true)
                {
                    offset = _ownerInducedRotationOffset;
                }      
                return Owner.Position + _worldPos;
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Link the link point's position and rotation to the owner's rotation value
        /// </summary>
        public bool LinkRotationToOwnerRotation
        {
            get { return _linkRotation; }
            set 
            {
                if (_linkRotation != value)
                {
                    _linkRotation = value;
                    this.Rotation = this.Rotation;
                }
            }
        }

        /// <summary>
        /// Link the link point's position and rotation to the owner's rotation value
        /// </summary>
        public bool LinkPositionToOwnerRotation
        {
            get { return _linkPositionToOwnerRotation; }
            set
            {
                if (_linkPositionToOwnerRotation != value)
                {
                    _linkPositionToOwnerRotation = value;
                    this.Offset = this.Offset;
                }
            }
        }

        public List<LinkPoint> MountedChildLinkPoints { get { return _mountedChildLinkPoints; } }
        #endregion

        #region Constructor

        public LinkPoint()
            : this(null, String.Empty)
        {

        }

        public LinkPoint(SceneItem owner, String name)
        {
            this.Owner = owner;
            this.Name = name;
            this.Offset = Vector2.Zero;
            Mounts = new Dictionary<string,string>();
        }

        #endregion

        #region Methods

        public void AddMountedChildLinkPoint(LinkPoint linkPoint)
        {
            if (_mountedChildLinkPoints.Contains(linkPoint))
            {
                throw new Exception("The LinkPoint \"" + linkPoint.Name + "\" is already attached to this SceneItem");
            }
            Mounts[linkPoint.Owner.Name] = linkPoint.Name;
            _mountedChildLinkPoints.Add(linkPoint);
            this.UpdateChild(linkPoint);
        }

        public void RemoveMountedChildLinkPoint(LinkPoint linkPoint)
        {
            if (_mountedChildLinkPoints.Contains(linkPoint) == false)
            {
                throw new Exception("The LinkPoint \"" + linkPoint.Name + "\" is not attached to this SceneItem");
            }
            _mountedChildLinkPoints.Remove(linkPoint);            
        }

        public void Update()
        {
            if (LinkRotationToOwnerRotation == true)
            {
                this.Rotation = Owner.Rotation;
            }
            if (LinkPositionToOwnerRotation == true 
                && (_calculateOwnerInducedRotationOffset == true || _lastOwnerRotation != Owner.Rotation))
            {
                float angle = Owner.Rotation + _offsetVectorAngle;
                _ownerInducedRotationOffset.X = _offsetVectorLenght * IceMath.CosFromRadians(angle);
                _ownerInducedRotationOffset.Y = _offsetVectorLenght * IceMath.SinFromRadians(angle);
                _calculateOwnerInducedRotationOffset = false;
                _lastOwnerRotation = Owner.Rotation;
            }
            for (int i = 0; i < _mountedChildLinkPoints.Count; i++)
            {
                UpdateChild(_mountedChildLinkPoints[i]);
            }
        }

        public void UpdateChild(LinkPoint targetLinkPoint)
        {
            
            SceneItem targetChild = targetLinkPoint.Owner;
            targetChild.Position = this.Position - targetLinkPoint.Offset;
            targetChild.Rotation = this.Rotation;

            
            //targetChild.Position = GetWorldLinkPosition( this._mountedTo.Val.Position, this._mountedTo.Val.Rotation, this._mountedTo.Val.Offset);

        }

        #endregion
    }
}
