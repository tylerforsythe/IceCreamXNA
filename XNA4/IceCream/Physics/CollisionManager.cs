//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using IceCream;
//using IceCream.Physics;
//using IceCream.Components;

//namespace IceCream.Physics
//{
//    class CollisionManager
//    {           
//        private static float futureCheck = 1.0f;
//        private static CollisionEventArgs collisionEventArgs = new CollisionEventArgs();

//        /// <summary>
//        /// How far in the future to check. (s)
//        /// </summary>
//        public static float FutureCheck
//        {
//            get
//            {
//                return futureCheck;
//            }
//            set
//            {
//                if (value < 0)
//                {
//                    futureCheck = (-1) * value;
//                }
//                else
//                {
//                    futureCheck = value;
//                }
//            }
//        }

//        public static void CheckAndProcessCollisions()
//        {            
//            futureCheck = 1.0f;
//            SceneItem _item1, _item2;
//            for (int i = 0; i < SceneManager.ActiveScene.SceneItems.Count; i++)
//            {
//                _item1 = SceneManager.ActiveScene.SceneItems[i];
//                for (int j = i + 1; j < SceneManager.ActiveScene.SceneItems.Count; j++)
//                {
//                    _item2 = SceneManager.ActiveScene.SceneItems[j];
//                    if (_item1._collision != null && _item1._physics != null &&
//                        _item2._collision != null && _item2._physics != null)
//                    {
//                        PhysicsComponent physA = SceneManager.ActiveScene.SceneItems[i].GetComponent<PhysicsComponent>();
//                        PhysicsComponent physB = SceneManager.ActiveScene.SceneItems[j].GetComponent<PhysicsComponent>();
//                        CollisionComponent colA = SceneManager.ActiveScene.SceneItems[i].GetComponent<CollisionComponent>();
//                        CollisionComponent colB = SceneManager.ActiveScene.SceneItems[j].GetComponent<CollisionComponent>();
//                        //if (physA != null && physB != null && colA != null && colB != null)
//                        //{
//                            if (physA.Velocity != Vector2.Zero || physB.Velocity != Vector2.Zero)
//                            {
//                                if (Collide(physA, physB))
//                                {
//                                    CollisionManager.collisionEventArgs.CollidedSceneItem = physB.Owner;
//                                    colA.OnCollision(CollisionManager.collisionEventArgs);

//                                    CollisionManager.collisionEventArgs.CollidedSceneItem = physA.Owner;
//                                    colB.OnCollision(CollisionManager.collisionEventArgs);

//#if(DEBUG)
//                                    colA.Polygon.OutlineColor = colB.Polygon.OutlineColor = Color.Red;
//#endif
//                                }
//                                else
//                                {
//#if(DEBUG)
//                                    colA.Polygon.OutlineColor = colB.Polygon.OutlineColor = Color.Black;
//#endif
//                                }
//                            }
//                        //}
//                    }
//                }
//            }            
//        }

//        private static bool Collide(PhysicsComponent a, PhysicsComponent b)
//        {
//            CollisionComponent colA = a.Owner.GetComponent<CollisionComponent>();
//            CollisionComponent colB = b.Owner.GetComponent<CollisionComponent>();
//            Vector2 posOffset = (a.Owner.Position) - (b.Owner.Position);
//            Vector2 centerOffset = (a.Owner.Position + colA.Polygon.Center) - (b.Owner.Position + colB.Polygon.Center);
//            Vector2 velOffset = a.Velocity - b.Velocity;
//            Vector2 mtd = Vector2.Zero;
//            Vector2 N = Vector2.Zero;
//            float t = 0.0f;

//            Vector2 jSpeed = b.Velocity;

//            if (CheckSATSweptCollision(colA.Polygon.TransformedVertices, colB.Polygon.TransformedVertices, 
//                ref posOffset, ref centerOffset, ref velOffset, ref mtd, ref t))
//            {
//                if (t < 0.0f)
//                {
//                    CollisionManager.collisionEventArgs.OverLapping = true;
//                    ProcessOverlap(a, b, mtd * -t);                    
//                }
//                else
//                {
//                    CollisionManager.collisionEventArgs.OverLapping = false;
//                    Vector2 oldVelocity = b.Velocity;                                     
//                    ProcessCollision(a, b, mtd, t);
//                }               
//                return true;
//            }
//            return false;
//        }

//        #region Collision Detection

//        private static bool CheckSATSweptCollision(List<Vector2> vA, List<Vector2> vB, ref Vector2 xOffset, ref Vector2 centerOffset, ref Vector2 xVel, ref Vector2 N, ref float t)
//        {

//            // All the separation axes
//            List<Vector2> xAxis = new List<Vector2>();
//            List<float> tAxis = new List<float>();

//            float fVel2 = Vector2.Dot(xVel, xVel);
//            // test separation axes of the velocity if it's moving
//            if (fVel2 > 0.000001f)
//            {                
//                xAxis.Add(new Vector2(-xVel.Y, xVel.X));
//                if (!IntervalIntersect(vA, vB, xAxis[xAxis.Count - 1], ref xOffset, ref xVel, ref tAxis))
//                {
//                    return false;
//                }                
//            }

//            // test separation axes of A
//            for (int j = vA.Count - 1, i = 0; i < vA.Count; j = i, i++)
//            {
//                Vector2 E0 = vA[j];
//                Vector2 E1 = vA[i];
//                Vector2 E = E1 - E0;               
//                xAxis.Add(new Vector2(-E.Y, E.X));

//                if (!IntervalIntersect(vA, vB, xAxis[xAxis.Count - 1], ref xOffset, ref xVel, ref tAxis))
//                    return false;
//            }

//            // test separation axes of B
//            for (int j = vB.Count - 1, i = 0; i < vB.Count; j = i, i++)
//            {
//                Vector2 E0 = vB[j];
//                Vector2 E1 = vB[i];
//                Vector2 E = E1 - E0;
//                xAxis.Add(new Vector2(-E.Y, E.X));

//                if (!IntervalIntersect(vA, vB, xAxis[xAxis.Count - 1], ref xOffset, ref xVel, ref tAxis))
//                    return false;
//            }

//            if (!FindMTD(xAxis, tAxis, ref N, ref t))
//                return false;

//            // make sure the polygons gets pushed away from each other.            
//            if (Vector2.Dot(N, centerOffset) < 0.0f)
//            {
//                N = -N;
//            }
//            return true;
//        }

//        // calculate the projection range of a polygon along an axis
//        private static void GetInterval(List<Vector2> axVertices, Vector2 xAxis, ref float min, ref float max)
//        {
//            min = max = Vector2.Dot(axVertices[0], xAxis);

//            for (int i = 1; i < axVertices.Count; i++)
//            {
//                float d = Vector2.Dot(axVertices[i], xAxis);
//                if (d < min) min = d;
//                else if (d > max) max = d;
//            }
//        }

//        private static bool IntervalIntersect(List<Vector2> vA, List<Vector2> vB, Vector2 xAxis, ref Vector2 xOffset, ref Vector2 xVel, ref List<float> tAxis)
//        {
//            float min0 = 0, max0 = 0;
//            float min1 = 0, max1 = 0;
//            GetInterval(vA, xAxis, ref min0, ref max0);
//            GetInterval(vB, xAxis, ref min1, ref max1);

//            float h = Vector2.Dot(xOffset, xAxis);
//            min0 += h;
//            max0 += h;

//            float d0 = min0 - max1;
//            float d1 = min1 - max0;

//            if (d0 > 0.0f || d1 > 0.0f)
//            {
//                double v = Vector2.Dot(xVel, xAxis);
//                // small velocity, so only the overlap test will be relevant
//                if (Math.Abs(v) < 0.0000001)
//                    return false;

//                double t0 = -d0 / v;
//                double t1 = d1 / v;
//                if (t0 > t1)
//                {
//                    double temp = t0;
//                    t0 = t1;
//                    t1 = temp;
//                }
//                float tAxisAdd = (t0 > 0.0) ? (float)t0 : (float)t1;
//                tAxis.Add(tAxisAdd);
//                // if it does not collide backward in time or too late in the future
//                if (tAxisAdd < 0.0f || tAxisAdd > futureCheck)
//                {
//                    return false;
//                }

//                return true;
//            }
//            else
//            {
//                // overlap
//                float temp = (d0 > d1) ? d0 : d1;
//                tAxis.Add(temp);
//                return true;
//            }
//        }

//        private static bool FindMTD(List<Vector2> xAxis, List<float> tAxis, ref Vector2 N, ref float t)
//        {
            
//            // Find the collision due to velocity first
//            bool isMTDFound = false;            
//            t = 0.0f;
//            N = Vector2.Zero;            
//            for (int i = 0; i < xAxis.Count; i++)
//            {
//                if (tAxis[i] > 0 && tAxis[i] > t)
//                {
//                    isMTDFound = true;
//                    t = tAxis[i];
//                    N = Vector2.Normalize(xAxis[i]);                    
//                }
//            }
//            // Collision found forward in time, no need to look further
//            if (isMTDFound)
//            {
//                return true;
//            }           
//            // no collision in time, find overlaps
//            for (int i = 0; i < xAxis.Count; i++)
//            {
//                // store the lenght before normalizing it
//                float n = xAxis[i].Length();                
//                tAxis[i] /= n;
//                if (tAxis[i] > t || !isMTDFound)
//                {
//                    isMTDFound = true;
//                    t = tAxis[i];
//                    N = Vector2.Normalize(xAxis[i]);
//                }
//            }

//            if (isMTDFound == false)
//            {
//                throw new Exception("No MTD found! There was no collision");
//            }
//            return true;
//        }

//        #endregion

//        #region Collision Response

//        private static void ProcessCollision(PhysicsComponent a, PhysicsComponent b, Vector2 N, float t)
//        {
//            CollisionManager.collisionEventArgs.Normal = N;
//            Vector2 D = a.Velocity - b.Velocity;
//            float n = Vector2.Dot(D, N);
//            Vector2 Dn = N * n;
//            Vector2 Dt = D - Dn;
//            if (n > 0.0f)
//            {
//                Dn = Vector2.Zero;
//            }
//            float dt = Vector2.Dot(Dt, Dt);
//            float cof = a.Friction * b.Friction;       
//            if (dt < a.Glue * b.Glue)
//            {
//                cof = 1.01f;
//            }
//            // apply elasticity and friction
//            D = -(1.0f + (a.Elasticity * b.Elasticity)) * Dn - (cof) * Dt;
//            float mA = a.InverseMass;
//            float mB = b.InverseMass;
//            float m = mA + mB;
//            Vector2 s0 = D * (mA / m);
//            Vector2 s1 = D * (mB / m);

//            // separate the polygons
//            SeparateObjects(a, b, D * -t);
            
//            // adjust the velocity
//            a.ImpulseForces.Add(s0);
//            b.ImpulseForces.Add(-s1);
//        }

//        private static void SeparateObjects(PhysicsComponent a, PhysicsComponent b, Vector2 mtd)
//        {
//            if (!a.IsMoveable)
//            {
//                b.Owner.Position -= mtd;
//            }
//            else if (!b.IsMoveable)
//            {
//                a.Owner.Position += mtd;
//            }
//            else
//            {
//                a.Owner.Position += mtd * (a.InverseMass / (a.InverseMass + b.InverseMass));
//                b.Owner.Position -= mtd * (b.InverseMass / (a.InverseMass + b.InverseMass));
//            }
//        }

//        private static void ProcessOverlap(PhysicsComponent a, PhysicsComponent b, Vector2 mtd)
//        {
//            SeparateObjects(a, b, mtd);
//            ProcessCollision(a, b, Vector2.Normalize(mtd), 0.0f);
//        }
//        #endregion
//    }
//}