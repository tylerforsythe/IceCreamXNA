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
using IceCream.Farseer;
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
using IceCream.Components;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;
using IceCream.SceneItems;
using IceCream.Attributes;

namespace IceCream
{
    public class SceneItem : IceBase
    {
        #region Fields

        private bool _visible = true;
        internal int spatialIndex = 0;
        private float _rotation;
        private int _layer;
        private bool _markForDelete;
        private bool _ignoreCamera;
        private bool _isTemplate = false;
        private bool _isRegistered;
        private Vector2 _lastposition;
        private Vector2 _position;
        private Vector2 _pivot;
        private bool _isPivotRelative;
        private Vector2 _scale = Vector2.One;
        private List<IceComponent> _components = new List<IceComponent>();
        private IceScene _sceneParent = null; // null if global for now        
        private Rectangle _boundingRect = new Rectangle(0, 0, 20, 20);
        private uint _objectType;
        internal List<LinkFuse> fuses = new List<LinkFuse>();
        private List<string> _tags = new List<string>();
        private int _factionId = 0;

        // Link points and mount specifics
        private List<LinkPoint> _linkPoints = new List<LinkPoint>();
        private SceneItem _mountOwner = null;
        private LinkPoint _mountedLinkPoint = null;
        private LinkPoint _mountedTargetLinkPoint = null;
        internal CollisionComponent _collision;
        //internal PhysicsComponent _physics;
        private IceFarseerEntity _iceFarseerEntity;
        public IceFarseerEntity IceFarseerEntity
        {
            get { return _iceFarseerEntity; }
        }

        private static Vector2[] verticesBuffer = new Vector2[4];

        #endregion

        #region Properties

        [BrowsableAttribute(false), XmlIgnore]
        public IceScene SceneParent
        {
            get { return _sceneParent; }
            set { _sceneParent = value; }
        }

        public bool LockedPosition { get; set; }

        /// <summary>
        /// Gets the value of wether this SceneItem is Mounted to another object
        /// </summary>
        [BrowsableAttribute(false)]
        public bool IsMounted
        {
            get { return (_mountOwner != null); }
        }

        [BrowsableAttribute(false), XmlIgnore]
        public bool IsRegistered { get { return _isRegistered; } set { _isRegistered = value; } }

        [BrowsableAttribute(false), XmlIgnore]
        public SceneItem MountOwner
        {
            get { return _mountOwner; }
        }

        [BrowsableAttribute(false), XmlIgnore]
        public bool MarkForDelete
        {
            get { return _markForDelete; }
            set { _markForDelete = value; }
        }

        [CategoryAttribute("Layout"), DescriptionAttribute("The position of the item inside the scene")]
        public virtual Vector2 Position
        {
            get { return _position; }
            set
            {
                if (LockedPosition)
                {
                    return;
                }
                _position = value;
                UpdateBoundingRect();

                if (_iceFarseerEntity != null)
                    _iceFarseerEntity.BodyPosition = _position;
            }
        }

        [BrowsableAttribute(false), XmlIgnore]
        public virtual Vector2 CenterOfPosition {
            get {
                Vector2 center = Position;
                center.X += BoundingRectSize.X / 2;
                center.Y += BoundingRectSize.Y / 2;
                return center;
            }
        }

        [CategoryAttribute("Layout"), DescriptionAttribute("The pivot point around which the rotation will be performed")]
        public virtual Vector2 Pivot
        {
            get { return _pivot; }
            set
            {
                _pivot = value;
                UpdateBoundingRect();
            }
        }

        [CategoryAttribute("Layout"), DescriptionAttribute("The scale factor of the SceneItem on both axis")]
        public virtual Vector2 Scale
        {
            get { return _scale; }
            set 
            { 
                _scale = value;
                UpdateBoundingRect();
            }
        }

        [CategoryAttribute("Layout"), DescriptionAttribute("The opacity of the SceneItem, if applicable, from 0 to 255")]
        public virtual byte Opacity
        {
            get;
            set;
        }

        [CategoryAttribute("Layout"), DescriptionAttribute("If set to true, the scene items position will be absoloute on the screen")]
        public bool IgnoreCameraPosition
        {
            get { return _ignoreCamera; }
            set
            {
                _ignoreCamera = value;
            }
        }

        [XmlIgnore()]
        public virtual float PositionY
        {
            get { return _position.Y; }
            set
            {
                if (LockedPosition)
                    return;
                _position.Y = value;
                UpdateBoundingRect();

                if (_iceFarseerEntity != null)
                    _iceFarseerEntity.BodyPosition = _position;
            }
        }
        
        [XmlIgnore()]
        public virtual float PositionX
        {
            get { return _position.X; }
            set
            {
                if (LockedPosition)
                    return;
                _position.X = value;
                UpdateBoundingRect();

                if (_iceFarseerEntity != null)
                    _iceFarseerEntity.BodyPosition = _position;
            }
        }

        [BrowsableAttribute(false),XmlIgnore]
        public virtual Rectangle BoundingRect
        {
            get { return _boundingRect; }
            set { _boundingRect = value; }
        }

        [CategoryAttribute("Layout"), DescriptionAttribute("The rotation angle of the scene item")]
        public virtual float Rotation
        {
            get { return _rotation; }
            set {
                _rotation = value;

                if (_iceFarseerEntity != null && _iceFarseerEntity.Body != null)
                    _iceFarseerEntity.Body.Rotation = _rotation;
            }
        }

        [CategoryAttribute("Layout"), DescriptionAttribute("The layer in which the item is located inside the scene")]
        #if WINDOWS
        [TypeConverter(typeof(IceCream.SceneItems.TypeConverters.LayerConverter))]
        #endif
        public virtual int Layer
        {
            get { return _layer; }
            set { _layer = value; }
        }

        [BrowsableAttribute(false)]
        public virtual bool IsTemplate
        {
            get { return _isTemplate; }
            set { _isTemplate = value; }
        }

        [BrowsableAttribute(false)]
        public List<IceComponent> Components
        {
            get { return _components; }
            set { _components = value; }
        }

        [BrowsableAttribute(true), XmlIgnore]
#if(WINDOWS)
        [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
        public List<LinkPoint> LinkPoints
        {
            get { return _linkPoints; }
            set { _linkPoints = value; }
        }

        [CategoryAttribute("Options"), DescriptionAttribute("The type the object is. Flags value, use CheckObjectType to check a value.")]
        public uint ObjectType
        {
            get { return _objectType; }
            set { _objectType = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        [CategoryAttribute("Layout"), DescriptionAttribute("Determine if the pivot is relative to the size of the sprite (ex: 0.5 for 50%) or if it is in absolute pixel values")]
        public bool IsPivotRelative
        {
            get { return _isPivotRelative; }
            set { _isPivotRelative = value; }
        }

        public bool FlipHorizontal
        {
            get;
            set;
        }

        public bool FlipVertical
        {
            get;
            set;
        }

        /// <summary>
        /// Values used to calculate the bounding rect
        /// X: default width (unrotated, unscaled)
        /// Y: default height (unrotated, unscaled)
        /// </summary>
        [BrowsableAttribute(false)]
        public virtual Vector2 BoundingRectSize
        {
            get;
            set;
        }

        public Object Tag
        {
            get;
            set;
        }

        [CategoryAttribute("Options"), DescriptionAttribute("Any tags associated with this object. Used for grouping and/or marking attributes.")]
        //from http://msdn.microsoft.com/en-us/library/42737xf2(v=vs.80).aspx
        [Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        public List<string> Tags {
            get { return _tags; }
            set { }
        }

        [CategoryAttribute("Options"), DescriptionAttribute("Integer to put objects into specific factions or any other sort of meta-group.")]
        public int FactionId {
            get { return _factionId; }
            set { _factionId = value; }
        }

        #endregion

        #region Constructor

        public SceneItem()
        {
            this.Name = "";
            _isPivotRelative = false;
            _layer = 5;                        
            this.BoundingRectSize = new Vector2(64, 64);
            this.Opacity = 255;
            
            if (IceFarseerManager.FarseerEnabled)
                this._iceFarseerEntity = new IceFarseerEntity(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Perform a deep copy of the object to the target
        /// </summary>
        /// <param name="target">The target that will receive all the values</param>
        public virtual void CopyValuesTo(SceneItem target)
        {
            target.BoundingRectSize = this.BoundingRectSize;
            target.BoundingRect = this.BoundingRect;
            target.Layer = this.Layer;
            target.MarkForDelete = this.MarkForDelete;
            target.Name = this.Name;
            target.ObjectType = this.ObjectType;
            target.Position = this.Position;
            target.Rotation = this.Rotation;
            target.SceneParent = this.SceneParent;
            target.Scale = this.Scale;
            target.Pivot = this.Pivot;
            target.Opacity = this.Opacity;
            target.IsPivotRelative = this.IsPivotRelative;
            target.LockedPosition = this.LockedPosition;
            target.Visible = this.Visible;
            target.IsTemplate = this.IsTemplate;
            target.IgnoreCameraPosition = IgnoreCameraPosition;
            target.FlipHorizontal = this.FlipHorizontal;
            target.FlipVertical = this.FlipVertical;
            target.LinkPoints.Clear();
            target.Components.Clear();
            foreach (var item in Components)
            {
                target.AddComponent((IceComponent)item.GetCopy());
            }

            target.Tags.Clear();
            foreach (string tag in this.Tags) {
                target.Tags.Add(tag);
            }
            target.FactionId = this.FactionId;
        }

        /// <summary>
        /// Returns a component of specified type. Use this to quickly get a component from the object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : IceComponent
        {
            T local = default(T);
            foreach (IceComponent component in Components)
            {
                local = component as T;
                if (local != null)
                {
                    return local;
                }
            }
            return null;
        }

        public void Move(Vector2 vec)
        {
            Position += vec;
        }

        public virtual void Draw(float elapsed)
        {

        }

        public virtual void Update(float elapsed)
        {
            for (int i = 0; i < LinkPoints.Count; i++)
            {
                LinkPoints[i].Update();
            }
            _lastposition = _position;
        }

        public virtual void UpdateBoundingRect()
        {
            int width = (int)(BoundingRectSize.X * Scale.X);
            int height = (int)(BoundingRectSize.Y * Scale.Y);
            Vector2 tmpPivot = this.Pivot;
            if (this.IsPivotRelative == true)
            {
                    tmpPivot = new Vector2(this.Pivot.X * BoundingRectSize.X, 
                        this.Pivot.Y * BoundingRectSize.Y);
            }
            tmpPivot.X = tmpPivot.X * this.Scale.X;
            tmpPivot.Y = tmpPivot.Y * this.Scale.Y;
            Vector2 topLeft;
            if (this.Rotation != 0)
            {               
                Matrix rotationMatrix = Matrix.CreateRotationZ(this.Rotation);
                tmpPivot = Vector2.Transform(tmpPivot, rotationMatrix);
                verticesBuffer[0] = Vector2.Transform(new Vector2(0, 0), rotationMatrix) - tmpPivot;
                verticesBuffer[1] = Vector2.Transform(new Vector2(width, 0), rotationMatrix) - tmpPivot;
                verticesBuffer[2] = Vector2.Transform(new Vector2(width, height), rotationMatrix) - tmpPivot;
                verticesBuffer[3] = Vector2.Transform(new Vector2(0, height), rotationMatrix) - tmpPivot;
                Vector4 bounds = new Vector4(float.PositiveInfinity, float.PositiveInfinity,
                    float.NegativeInfinity, float.NegativeInfinity);
                for (int i = 0; i < verticesBuffer.Length; i++)
                {
                    bounds.X = MathHelper.Min(verticesBuffer[i].X, bounds.X);
                    bounds.Y = MathHelper.Min(verticesBuffer[i].Y, bounds.Y);
                    bounds.W = MathHelper.Max(verticesBuffer[i].X, bounds.W);
                    bounds.Z = MathHelper.Max(verticesBuffer[i].Y, bounds.Z);
                }
                topLeft = this.Position + new Vector2(bounds.X, bounds.Y);
                width = (int)Math.Ceiling((double)(bounds.W - bounds.X));
                height = (int)(bounds.Z - bounds.Y);
            }
            else
            {
                topLeft = this.Position - tmpPivot;
            }
            this.BoundingRect = new Rectangle((int)topLeft.X, (int)(topLeft.Y), width, height);

            //spatialIndex=  (Math.Floor(PositionX/SceneParent._spatialGrid.cellSize)) + 
            //              (Math.Floor(PositionY/SceneParent._spatialGrid.cellSize)) *

        }
        
        internal virtual void OnRegister()
        {
            SetupLinkFuses();
            UpdateBoundingRect();
            foreach (IceComponent _component in Components)
            {
                _component.SetOwner(this);
                _component.OnRegister();
            }
            _isRegistered = true;


            if (IceFarseerManager.FarseerEnabled)
                _iceFarseerEntity.InitFromTexture();
        }

        internal virtual void OnUnRegister()
        {
            foreach (IceComponent _component in Components)
            {
                _component.OnUnRegister();
            }
            if (UnRegister != null)
                UnRegister(this);
            _isRegistered = false;

            if (IceFarseerManager.FarseerEnabled && _iceFarseerEntity != null)
                _iceFarseerEntity.Clear();
        }

        public void AddComponent(IceCream.Components.IceComponent component)
        {
            component.SetOwner(this);
            if (IsRegistered)
                component.OnRegister();
            _components.Add(component);
        }

        public LinkPoint GetLinkPoint(string name)
        {
            foreach (LinkPoint point in LinkPoints)
            {
                if (point.Name == name)
                {
                    return point;
                }
            }
            return null;
        }

        public void Mount(SceneItem target, string targetLinkPoint, string linkPoint)
        {
            if (IsMounted)
            {
                throw new Exception("This object is already mounted to another SceneItem");
            }
            if (target == null)
            {
                throw new Exception("This target SceneItem is null");
            }
            //if((!target.IsTemplate && !this.IsTemplate))
              //  throw new Exception("You cannot mount a sceneitem if it is not a Template");

            _mountedTargetLinkPoint = target.GetLinkPoint(targetLinkPoint);
            _mountedLinkPoint = GetLinkPoint(linkPoint);
            if (_mountedTargetLinkPoint == null)
            {
                throw new Exception("The target LinkPoint \"" + targetLinkPoint + "\" does not exists");
            }
            if (_mountedLinkPoint == null)
            {
                throw new Exception("The local LinkPoint \"" + linkPoint + "\" does not exists");
            }
            _mountedTargetLinkPoint.AddMountedChildLinkPoint(_mountedLinkPoint);
            _mountOwner = target;
        }

        public void Mount(string target, string targetLinkPoint, string linkPoint,bool istemplate)
        {
            
        }

        public void UnMount()
        {
            if (IsMounted == false)
            {
                throw new Exception("This object is not mounted to a SceneItem");
            }
            if (_mountOwner == null)
            {
                throw new Exception("The Mount Owner of this object is null");
            }
            if (_mountedTargetLinkPoint == null)
            {
                throw new Exception("The Mount Owner target LinkPoint is null");
            }
            if (_mountedLinkPoint == null)
            {
                throw new Exception("The local mounted LinkPoint is null");
            }
            _mountedTargetLinkPoint.RemoveMountedChildLinkPoint(_mountedLinkPoint);
            _mountedTargetLinkPoint = null;
            _mountedLinkPoint = null;
            _mountOwner = null;
        }

        public void Destroy()
        {
            if (IsMounted == true)
            {
                UnMount();
            }
        }

        public void SetupLinkFuses()
        {
            //Trying something out with loading linkpoints
            foreach (var item in LinkPoints)
            {
                foreach (var mount in item.Mounts)
                {
                    SceneItem sceneitem = SceneParent.GetTemplate(mount.Key);
                    if (sceneitem != null)
                    {
                        sceneitem.Mount(this, item.Name, mount.Value);
                    }
                }
            }
        }

        internal bool HasMoved()
        {
            return _lastposition != _position;
        }

        /// <summary>
        /// Checks if the sceneitem has an objecttype of the given value
        /// </summary>
        /// <param name="type">The type of object</param>
        /// <returns>True if the type is found to be set on the SceneItems ObjectType property</returns>
        public bool CheckType(uint type)
        {
            return On(type);
        }

        private bool On(uint bt)
        {
            return (ObjectType & bt) == bt;
        }
        
        /// <summary>
        /// Checks if the sceneitem has an objecttype of the given values
        /// </summary>
        /// <param name="objectTypeEnum">The types of object</param>
        /// <returns>True if the type is found to be set on the SceneItems ObjectType property</returns>
        public bool CheckType(uint[] objectTypeEnum)
        {
            foreach (var item in objectTypeEnum)
            {
                if (CheckType(item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the sceneitem has a tag matching the parameter
        /// </summary>
        /// <param name="searchTag">The tag to search for</param>
        /// <returns>True if the tag is found to be in the SceneItem's Tags list</returns>
        public bool CheckForTag(string searchTag) {
            foreach (string t in Tags) {
                if (t == searchTag)
                    return true;
            }
            return false;
        }

        public virtual Vector2 GetAbsolutePivot(bool includeTransforms)
        {
            if (this.IsPivotRelative == false)
            {
                return this.Pivot;
            }
            else
            {
               Vector2 retVec = new Vector2(this.Pivot.X * (this.BoundingRectSize.X), 
                   this.Pivot.Y * (this.BoundingRectSize.Y));     
                if (includeTransforms == true)
                {
                    retVec /= this.Scale;
                }
                return retVec;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        #endregion

        #region Events
        /// <summary>
        /// This event is no longer used.
        /// </summary>
        [Obsolete]
        public event SceneItemEventHandler UnRegister;

        #endregion
    }

    public delegate void SceneItemEventHandler(SceneItem item);
}
