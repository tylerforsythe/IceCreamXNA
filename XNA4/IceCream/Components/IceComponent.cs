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
using IceCream.Attributes;
	
namespace IceCream.Components
{
    [Serializable]
    public abstract class IceComponent : IDeepCopy
    {
        #region Fields

        private SceneItem _owner;
        private bool _enabled;

        #endregion

        #region Properties

        [XmlIgnore()]
        public SceneItem Owner
        {
            get { return _owner; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        #endregion

        #region Constructor

        public IceComponent()
        {

        }

        #endregion
     
        #region Methods

        public virtual void CopyValuesTo(object target)
        {
            IceComponent component = target as IceComponent;
            component.Enabled = this.Enabled;
        }

        public virtual object GetCopy()
        {
            return ComponentTypeContainer.DeepCopyIceComponent(this.GetType(), this);
        }        

        public abstract void OnRegister();

        public abstract void Update(float elapsedTime);

        internal void SetOwner(SceneItem owner)
        {       
            _owner = owner;
        }

        public virtual void OnUnRegister()
        {

        }

        #endregion    
    }
}
