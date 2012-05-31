using System;
using System.Collections.Generic;
using System.Text;
using IceCream;
using IceCream.Drawing;
using IceCream.Attributes;
using IceCream.Components;
using IceCream.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IceCream.Farseer;

namespace IceCream.Components
{
    // A delegate type for hooking up collision notifications
    public delegate void CollisionEventHandler(object sender, CollisionEventArgs e);

    [IceComponentAttribute("CollisionComponent")]
    public class CollisionComponent : IceComponent
    {
        public static event CollisionEventHandler CollisionCallback;

        private CollisionBehavior _collisionBehavior;

        [IceComponentProperty("Collision Behavior")]
        public CollisionBehavior CollisionBehavior
        {
            get { return _collisionBehavior; }
            set { _collisionBehavior = value; }
        }        


        public CollisionComponent()
        {
            _collisionBehavior = CollisionBehavior.None;
        }

        public override void CopyValuesTo(object target) {
            base.CopyValuesTo(target);
            CollisionComponent component = target as CollisionComponent;
            component.CollisionBehavior = this.CollisionBehavior;
        }

        public static CollisionComponent Generate() {
            return new CollisionComponent();
        }

        public override void OnRegister()
        {
            Enabled = true;
            Owner._collision= this;
        }

        public override void Update(float elapsedTime)
        {
            if (Owner.IceFarseerEntity != null && Owner.IceFarseerEntity.Body != null)
                Owner.IceFarseerEntity.Body.Awake = true;
        }

        public void Draw(float elapsedTime)
        {
            
        }        


        public void OnCollision(CollisionEventArgs e) {
            if (CollisionBehavior == CollisionBehavior.ProcessReaction) {
                if (CollisionCallback != null)
                    CollisionCallback(this, e);
            }
        }

    }
}
