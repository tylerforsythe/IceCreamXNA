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
using IceCream.Drawing;
using System.Diagnostics;

namespace IceCream.SceneItems.ParticlesClasses
{
    public class Particle
    {
        #region Static Fields and Methods

        /// <summary>
        /// Get the linear interpolation value with an added random variation
        /// </summary>
        /// <param name="values">The key values</param>
        /// <param name="variations">The key variation values</param>
        /// <param name="lerpLife">The life, between 0 and 1</param>
        /// <returns></returns>
        public static float GetLerpValueWithVariation(List<Vector2> values, List<Vector2> variations, float lerpLife)
        {            
            return GetLerpValue(values, lerpLife) + Particle.GetRandomVariationValue(Particle.GetLerpValue(variations, lerpLife));
        }

        /// <summary>
        /// Get the liner interpolation value corresponding to the current life
        /// </summary>
        /// <param name="values">The key values</param>
        /// <param name="life">The life, between 0 and 1</param>
        /// <returns></returns>
        public static float GetLerpValue(List<Vector2> values, float life)
        {            
            // return 0 by default for an empty set
            if (values == null || values.Count == 0)
            {
                return 0;
            }
            // if there is only one value, return it
            else if (values.Count == 1)
            {
                return values[0].Y;
            }           
            if (life > values[values.Count - 1].X)
            {
                life = values[values.Count - 1].X;
            }
            for (int i = 0; i < values.Count -1; i++)
            {
                if (life <= values[i + 1].X || i == values.Count - 2)
                {                    
                    // return the linear interpolation between this key and the next one
                    return MathHelper.Lerp(values[i].Y, values[i + 1].Y, (life - values[i].X) / (values[i + 1].X - values[i].X));
                }
            }
            throw new Exception("Error with the Lerp values request!");
        }

        /// <summary>
        /// get a random value between -value/2 and value/2
        /// </summary>
        public static float GetRandomVariationValue(float value)
        {
            if (value == 0)
            {
                return 0;
            }
            else if (value < 0)
            {
                value = -value;
            }            
            return Randomizer.Float(-value / 2f, value / 2f);
        }

        #endregion

        #region Fields

        private DrawRequest _drawRequest;

        private bool _isActive;
        private bool _keepAspectRatio;
        private float _maxLife;
        private float _currentLife;
        private ParticleOrientationType _particleOrientationType;

        // initial values        
        private float _initialWidth;
        private float _initialHeight;
        private float _initialVelocity;        
        private float _initialWeight;
        private float _initialSpin;
        private float _initialMotionRandom;
        private float _initialDirection;
        private Vector4 _initialColorFloatValues;

        // working values (for over life changes)
        private float _velocity;
        private float _weight;
        private float _spin;
        private float _motionRandom;
        private Vector2 _deltaPos;
        private Vector2 _deltaPosAttractor;
        private Vector2 _gravity;
        
        private OverLifeSettings overLifeSettings;

        private ParticleType _parent;
        private float? _initialDistanceFromAttractor;

        private float _animationTotalTimeElapsed = 0;
        private int _currentTile = -1;
        private bool _animationIsStopped = false;

        #endregion

        #region Properties

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
            }
        }

        public Vector2 Position
        {
            get { return _drawRequest.position; }
            set { _drawRequest.position = value; }
        }

        #endregion

        #region Methods

        public void Draw(int layer, DrawingBlendingType blendingType)
        {
            if (_isActive)
            {                
                DrawingManager.DrawOnLayer(_drawRequest, layer, blendingType);                
            }
        }

        public Rectangle GetSourceRectOfTile()
        {
            if (_parent.TileCols > 0 && _parent.TileRows > 0)
            {
                int tileTotal = _parent.TileRows * _parent.TileCols - _parent.TileTotalOffset;
                if (_currentTile >= tileTotal)
                {
                    _currentTile = tileTotal - 1;
                }              
                // Extract the right area of the sprite for the given frame.
                int x = _currentTile % _parent.TileCols;
                int y = _currentTile / _parent.TileCols;
                Rectangle srcrect = new Rectangle(x * _parent.TileWidth + x * 2, y * _parent.TileHeight + y * 2,
                    _parent.TileWidth, _parent.TileHeight);                
                return srcrect;
            }
            return Rectangle.Empty;
        }

        /// <summary>
        /// Update the properties of a particle
        /// </summary>
        public void Update(float elapsed, float opacityModifier)
        {
            // get the percentage of life
            float lerpLife;
            Vector2 realDistanceVector = Vector2.Zero;
            //float dist = 0;
            if ( _initialDistanceFromAttractor != null)
            {
               
                //lerpLife = MathHelper.Clamp(1.0f - dist / _initialDistanceFromAttractor.Value, 0, 1);
                //if (dist <= 1.0f)
                //{
                //    // mark the particle as dead
                //    _isActive = false;
                //    return;
                //}
                if (_currentLife > _maxLife)
                {
                    // mark the particle as dead
                    _isActive = false;
                    return;
                }
                lerpLife = _currentLife / _maxLife;
            }
            else
            {
                if (_currentLife > _maxLife)
                {
                    // mark the particle as dead
                    _isActive = false;
                    return;
                }
                lerpLife = _currentLife / _maxLife;
            }
            // apply the tile source rectangle if needed
            if (_parent.TileWidth > 0 && _parent.TileHeight > 0)
            {
                if (_parent.AnimationFrameDelay > 0 && _animationIsStopped == false)
                {
                    int tileTotal = _parent.TileRows * _parent.TileCols - _parent.TileTotalOffset;
                    _animationTotalTimeElapsed += elapsed;
                    if (_animationTotalTimeElapsed > _parent.AnimationFrameDelay / 1000f)
                    {
                        _animationTotalTimeElapsed = 0;
                        _currentTile++;
                        if (_currentTile >= tileTotal)
                        {
                            if (_parent.AnimationLoop != true)
                            {
                                _animationIsStopped = true;                                
                            }
                            else
                            {
                                _currentTile = 0;
                            }
                        }
                    }
                }
                _drawRequest.sourceRectangle = GetSourceRectOfTile();
            }
            else
            {
                _drawRequest.sourceRectangle = null;
            }
            // apply Over Life settings
            this._drawRequest.scaleRatio.X = _initialWidth * GetLerpValue(overLifeSettings.widthOverLife.Values, lerpLife);
            if (_keepAspectRatio)
            {
                _drawRequest.scaleRatio.Y = _drawRequest.scaleRatio.X;
            }
            else
            {
                _drawRequest.scaleRatio.Y = _initialHeight * GetLerpValue(overLifeSettings.heightOverLife.Values, lerpLife);
            }
            _velocity = _initialVelocity * GetLerpValue(overLifeSettings.velocityOverLife.Values, lerpLife);
            _weight = _initialWeight * GetLerpValue(overLifeSettings.weightOverLife.Values, lerpLife);
            _spin = _initialSpin * GetLerpValue(overLifeSettings.spinOverLife.Values, lerpLife);
            _motionRandom = _initialMotionRandom * GetLerpValue(overLifeSettings.motionRandomOverLife.Values, lerpLife);
            _drawRequest.tint.R = (byte)(255 * (_initialColorFloatValues.X * GetLerpValue(overLifeSettings.redTintOverLife.Values, lerpLife)));
            _drawRequest.tint.G = (byte)(255 * (_initialColorFloatValues.Y * GetLerpValue(overLifeSettings.greenTintOverLife.Values, lerpLife)));
            _drawRequest.tint.B = (byte)(255 * (_initialColorFloatValues.Z * GetLerpValue(overLifeSettings.blueTintOverLife.Values, lerpLife)));
            _drawRequest.tint.A = (byte)(255 * (_initialColorFloatValues.W * GetLerpValue(overLifeSettings.opacityOverLife.Values, lerpLife) * opacityModifier));
            // calc the new position offset due to velocity
            _deltaPos = new Vector2(_velocity * (float)Math.Cos((double)_initialDirection), _velocity * (float)Math.Sin((double)_initialDirection));
            // if attraction is enabled
            if (_parent.Parent.AttractorEnabled)
            {
                realDistanceVector = _parent.Parent.AttractorPosition - _drawRequest.position;
                _deltaPosAttractor = realDistanceVector;
                // get the normal of the vector
                _deltaPosAttractor.Normalize();
                // multiply by the force to get the displacement vector
                _deltaPosAttractor *= _parent.Parent.AttractorForce;                
                float realDistance = realDistanceVector.LengthSquared();
                float newDistance = _deltaPosAttractor.LengthSquared();
                // if it has reached the attractor's point or past it
                if (realDistance <= newDistance)
                {                    
                    _isActive = false;
                    // make sure that the particle will be drawn in the attractor point
                    _deltaPosAttractor = realDistanceVector;
                }
            }
            // update the direction and angle according to the weight
            if (_weight != 0)
            {
                _gravity.Y += _initialWeight / 100.0f;
                _deltaPos += _gravity;
            }
            if (_particleOrientationType == ParticleOrientationType.FollowMovementAngle)
            {
                _drawRequest.rotation = (float)Math.Atan2((double)_deltaPos.Y, (double)_deltaPos.X);
            }
            else
            {
                _drawRequest.rotation += MathHelper.ToRadians(_spin);
            }
            _drawRequest.position += _deltaPos;
            if (_parent.Parent.AttractorEnabled)
            {
                _drawRequest.position += _deltaPosAttractor;               
            }
            // increment the life of the particle
            _currentLife++;
        }

        /// <summary>
        /// Modify the properties of a particles (recycling)
        /// </summary>
        public void ModifiyParticle(ParticleType parent, Material material, int tileDefault, bool keepAspectRatio, 
            ParticleOrientationType particleOrientation, float life, float width, float height, float velocity, 
            float weight, float spin, float motionRandom, float direction, Vector2 position, float rotation,
            float redTint, float greenTint, float blueTint, float opacity, Vector2 pivot, bool isPivotRelative, OverLifeSettings overLifeSettings)
        {
            this._drawRequest.texture = material.Texture;
            this._keepAspectRatio = keepAspectRatio;
            this._particleOrientationType = particleOrientation;
            this._maxLife = life;
            this._currentLife = 0;
            this._currentTile = tileDefault;
            this._animationTotalTimeElapsed = 0;
            this._animationIsStopped = false;
            this._initialWidth = width;
            this._initialHeight = height;
            this._initialVelocity = velocity;
            this._initialWeight = weight;
            this._initialSpin = spin;
            this._initialMotionRandom = motionRandom;
            this._initialDirection = direction;
            _drawRequest.position = position;
            _drawRequest.rotation = MathHelper.ToRadians(rotation);
            this._initialColorFloatValues.X = redTint;
            this._initialColorFloatValues.Y = greenTint;
            this._initialColorFloatValues.Z = blueTint;
            this._initialColorFloatValues.W = opacity;              
            _drawRequest.pivot = pivot;
            _drawRequest.isPivotRelative = isPivotRelative;
            // TO-DO: add pivot support
            this.overLifeSettings = overLifeSettings;
            this._gravity = Vector2.Zero;
            this._parent = parent;
            if (parent.Parent.AttractorEnabled)
            {
                Vector2 distanceVector = _parent.Parent.AttractorPosition - position;
                _initialDistanceFromAttractor = distanceVector.Length();
                if (_initialDistanceFromAttractor == 0.0f)
                {
                    _initialDistanceFromAttractor = 0.00001f;
                }
            }
            else
            {
                _initialDistanceFromAttractor = null;
            }
        }

        #endregion

        #region Constructor

        public Particle()
        {            
            this._isActive = false;
            this._initialColorFloatValues = new Vector4();
            this._drawRequest = default(DrawRequest);           
        }

        #endregion
    }
}
