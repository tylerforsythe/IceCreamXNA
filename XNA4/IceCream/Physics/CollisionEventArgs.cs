using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace IceCream.Physics
{
    public class CollisionEventArgs : EventArgs
    {
        private SceneItem _collidedSceneItemA;
        public SceneItem CollidedSceneItemA
        {
            get { return _collidedSceneItemA; }
            set { _collidedSceneItemA = value; }
        }
        private SceneItem _collidedSceneItemB;
        public SceneItem CollidedSceneItemB {
            get { return _collidedSceneItemB; }
            set { _collidedSceneItemB = value; }
        }

        public CollisionEventArgs()
        {
            _collidedSceneItemA = null;
            _collidedSceneItemB = null;
        }
    }
}
