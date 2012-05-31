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
using IceCream.Attributes;

namespace IceCream.Components
{
    [Serializable]
    public abstract class IceSceneComponent : IDeepCopy
    {
        #region Fields
        private IceScene _owner;
        private bool _enabled;

        #endregion

        #region Properties

        public IceScene Owner
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

        public IceSceneComponent()
        {

        }

        #endregion

        #region Methods

        public virtual void CopyValuesTo(object target)
        {
            IceSceneComponent component = target as IceSceneComponent;
            component.Enabled = this.Enabled;
        }

        public abstract void OnRegister();

        public abstract void Update(float elapsedTime);

        internal void SetOwner(IceScene owner)
        {
            _owner = owner;
        }

        public virtual void OnUnRegister()
        {

        }

        public virtual object GetCopy()
        {
            return ComponentTypeContainer.DeepCopyIceSceneComponent(this.GetType(), this);
        }

        #endregion
    }
}
