using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using IceCream.Debug;
using Microsoft.Xna.Framework;
using IceCream.SceneItems;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;

namespace IceCream.Farseer
{
    public class IceFarseerEntity
    {
        private bool _isBodyRemoved;//to ensure we don't ask Farseer to remove the body from the world more than once when IceCream flushes on exit
        public Body Body { get; set; }
        //public Vector2 Offset { get; set; }
        public Vector2 BodyPosition
        {
            get
            {
                if (Body != null)
                    return Body.Position;
                else
                    return Vector2.Zero;
            }
            set
            {
                if (Body != null && !_isBodyRemoved)
                    Body.Position = value;
            }
        }
        public Vector2 InitialCentroid { get; set; }
        public SceneItem SceneItem { get; set; }


        public IceFarseerEntity(SceneItem item)
        {
            //this.Offset = offset;
            //this.Geom.OnCollision
            this.SceneItem = item;
            _isBodyRemoved = true;
        }

        public void InitFromTexture() {
            uint[] data = null;
            Rectangle materialAreaRect;
            if (this.SceneItem is AnimatedSprite) {
                AnimatedSprite s = SceneItem as AnimatedSprite;
                string areaName = s.Animations[0].AnimationFrames[0].Area;
                materialAreaRect = s.Material.Areas[areaName];
            }
            else if (this.SceneItem is Sprite) {
                Sprite s = SceneItem as Sprite;
                string areaName = s.MaterialArea;
                if (areaName == string.Empty)
                    materialAreaRect = s.Material.Texture.Bounds;
                else
                    materialAreaRect = s.Material.Areas[areaName];
            }
            else {
                return;//not even a sprite? Get outta town
            }

            if (materialAreaRect.Width < 2 || materialAreaRect.Height < 2
                    || materialAreaRect.Width > 512 || materialAreaRect.Height > 512)
                return;

            Texture2D wholeTexture = ((Sprite)this.SceneItem).Material.Texture;
            data = new uint[materialAreaRect.Width * materialAreaRect.Height];
            wholeTexture.GetData(0, materialAreaRect, data, 0, data.Length);

            //Find the vertices that makes up the outline of the shape in the texture
            Vertices textureVertices = PolygonTools.CreatePolygon(data, materialAreaRect.Width, false);

            //The tool return vertices as they were found in the texture.
            //We need to find the real center (centroid) of the vertices for 2 reasons:

            //1. To translate the vertices so the polygon is centered around the centroid.
            Vector2 centroid = -textureVertices.GetCentroid();
            textureVertices.Translate(ref centroid);

            //2. To draw the texture the correct place.
            SceneItem.Pivot = -centroid;
            //Offset = -centroid;

            //We simplify the vertices found in the texture.
            textureVertices = SimplifyTools.ReduceByDistance(textureVertices, 1f);

            //Since it is a concave polygon, we need to partition it into several smaller convex polygons
            List<Vertices> list = BayazitDecomposer.ConvexPartition(textureVertices);

            //scale the vertices from graphics space to sim space
            //float scale = ((Sprite)SceneItem).Scale;
            //float scale = 100.0f;//100 is 1-to-1 for pixels
            //Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1)) * scale;
            //foreach (Vertices vertices in list) {
            //    vertices.Scale(ref vertScale);
            //}

            //Create a single body with multiple fixtures
            Body = BodyFactory.CreateCompoundPolygon(IceFarseerManager.StaticWorld, list, 1f, SceneItem.Position);
            _isBodyRemoved = false;
            //Body = BodyFactory.CreateCompoundPolygon(IceFarseerManager.StaticWorld, list, 1f, BodyType.Dynamic);
            //Body = BodyFactory.CreateCircle(IceFarseerManager.StaticWorld, wholeTexture.Width * 0.5f, 1.0f, SceneItem.Position, SceneItem);
            Body.BodyType = BodyType.Dynamic;
            Body.UserData = SceneItem;

            Body.IsSensor = true;
            Body.IgnoreGravity = true;
            Body.Enabled = true;
            Body.Awake = true;
            Body.CollidesWith = Category.All;
            Body.CollisionCategories = Category.All;
            Body.OnCollision += new OnCollisionEventHandler(IceFarseerManager.Instance.Body_OnCollision);
        }

        //public void InitFromVertices(Vertices vertices)
        //{
        //    PhysicsSimulator sim = FarseerManager.Instance.PhysicsSimulator;
        //    this.Body = BodyFactory.Instance.CreatePolygonBody(sim, vertices, 1);
        //    this.Geom = GeomFactory.Instance.CreatePolygonGeom(sim, this.Body, vertices, this.Offset, 0, 4);            
        //}

        //public void InitFromPolygon(Polygon polygon)
        //{
        //    PhysicsSimulator sim = FarseerManager.Instance.PhysicsSimulator;
        //    this.Body = BodyFactory.Instance.CreatePolygonBody(sim, polygon.Vertices, 1);
        //    this.Geom = GeomFactory.Instance.CreatePolygonGeom(sim, this.Body, polygon.Vertices, this.Offset, 0, 4);           
        //    FinishInit();
        //}

        private void FinishInit()
        {
            
        }

        public void SetStatic()
        {
            //this.Body.IsStatic = true;
            this.Body.IgnoreGravity = true;
        }

        internal void Clear() {
            if (!_isBodyRemoved)
                IceFarseerManager.StaticWorld.RemoveBody(Body);
            _isBodyRemoved = true;
        }
    }
}
