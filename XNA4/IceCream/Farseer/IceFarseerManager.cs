using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.DebugViews;
using FarseerPhysics.Dynamics;
using IceCream.Components;
using IceCream.Attributes;
using IceCream.Physics;
using IceCream.SceneItems;
using Microsoft.Xna.Framework;
using IceCream;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics;
using IceCream.Drawing;
using IceCream.Debug;
using System.Collections;

namespace IceCream.Farseer
{
    public class IceFarseerManager
    {
        public static bool FarseerEnabled = true;

        private static IceFarseerManager _instance;
        public static IceFarseerManager Instance {
            get {
                if (_instance == null)
                    _instance = new IceFarseerManager();
                return _instance;
            }
        }
        //private World _world;
        //public World World { get { return _world; } }

        public static World StaticWorld;

        public DebugViewXNA DebugView { get; set; }

        public static float UpdateRate = 1 / 30f;

        public bool IsPaused { get; set; }


        private IceFarseerManager()
        {
            StaticWorld = new World(Vector2.Zero);
            Settings.EnableDiagnostics = true;
            Settings.ContinuousPhysics = true;
            this.IsPaused = false;
        }


        internal static void Initialize() {
            if (_instance == null)
                _instance = new IceFarseerManager();
        }
        

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content, SpriteFont font) {
            if (!FarseerEnabled)
                return;

            //_view.LoadContent(graphicsDevice, SceneManager.GlobalContent);
            if (DebugView == null) {
                DebugView = new DebugViewXNA(StaticWorld);
                DebugView.AppendFlags(DebugViewFlags.Shape);
                DebugView.AppendFlags(DebugViewFlags.PolygonPoints);
                DebugView.AppendFlags(DebugViewFlags.PerformanceGraph);
                DebugView.AppendFlags(DebugViewFlags.AABB);
                DebugView.DefaultShapeColor = Color.White;
                DebugView.SleepingShapeColor = Color.LightGray;
                DebugView.LoadContent(graphicsDevice, content, font);
            }
        }

        public void Reset() {
            if (!FarseerEnabled)
                return;

            StaticWorld.Clear();
        }

        public void Update(float elapsedTime) {
            if (!FarseerEnabled)
                return;

            if (this.IsPaused == false)
            {
                try
                {
                    StaticWorld.Step(IceFarseerManager.UpdateRate);
                }
                catch (ArithmeticException ae)
                {
                    Console.WriteLine("Farseer Arithmetic Error: " + ae);
                }
            }
        }

        
        private void EnableOrDisableFlag(DebugViewFlags flag)
        {
            if ((DebugView.Flags & flag) == flag)
            {
                DebugView.RemoveFlags(flag);
            }
            else
            {
                DebugView.AppendFlags(flag);
            }
        }

        public void DrawDebug(SpriteBatch batch, Camera camera)
        {
            if (!FarseerEnabled)
                return;
            //Matrix projection = camera.Matrix;
            //Matrix view = camera.Matrix;
            //Matrix projection = camera.GetMatrix(DebugShapes.Parallax);
            Matrix projection = Matrix.CreateOrthographicOffCenter(camera.BoundingRect.Left, camera.BoundingRect.Right,
                camera.BoundingRect.Bottom, camera.BoundingRect.Top, 0, 1);

            Matrix view = camera.GetMatrix(Vector3.One);

            DebugView.RenderDebugData(ref projection);

            //DebugView.RenderDebugData(ref projection, ref view);
        }


        
        private List<string> _collisionTable = new List<string>(50);
        public void ResetCollisions() {
            _collisionTable = new List<string>(50);
        }
        public bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact) {
            SceneItem dataA = ((SceneItem)fixtureA.Body.UserData);
            SceneItem dataB = ((SceneItem)fixtureB.Body.UserData);

            if (HaveTheseCollided(dataA, dataB))
                return false;

            CollisionComponent collisionComponentA = dataA.GetComponent<CollisionComponent>();
            CollisionComponent collisionComponentB = dataB.GetComponent<CollisionComponent>();
            if (collisionComponentA != null) {
                CollisionEventArgs eventArgs = new CollisionEventArgs();
                eventArgs.CollidedSceneItemA = dataB;
                eventArgs.CollidedSceneItemB = dataA;
                collisionComponentA.OnCollision(eventArgs);
            }
            if (collisionComponentB != null) {
                CollisionEventArgs eventArgs = new CollisionEventArgs();
                eventArgs.CollidedSceneItemA = dataA;
                eventArgs.CollidedSceneItemB = dataB;
                collisionComponentB.OnCollision(eventArgs);
            }

            OnScreenStats.AddStat(string.Format("BODY! {0} {1}", dataA.Name, dataB.Name));
            return false;
        }

        private bool HaveTheseCollided(SceneItem dataA, SceneItem dataB) {
            string hashA = CollisonHash(dataA, dataB);
            string hashB = CollisonHash(dataB, dataA);

            if (_collisionTable.Contains(hashA) || _collisionTable.Contains(hashB)) {
                return true;
            }
            _collisionTable.Add(hashA);
            return false;
        }

        private string CollisonHash(SceneItem dataA, SceneItem dataB) {
            return string.Format("{0}{1}", dataA.id, dataB.id);
        }
    }
}
