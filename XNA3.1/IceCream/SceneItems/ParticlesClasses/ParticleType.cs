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
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using IceCream.Drawing;

namespace IceCream.SceneItems.ParticlesClasses
{
    [Serializable]
    public class ParticleType
    {
        #region Fields

        private Material _material;
        private Vector2 _texturePivot;
        private Emitter _parent;               
        private List<Particle> _particles = new List<Particle>();
        private String _name;
        // the queue of free particles for recycling
        private Queue<Particle> _freeParticles;
        private float _emissionTimerAccumulator;

        // settings
        #if WINDOWS
        [CategoryAttribute("Test"), DescriptionAttribute("Name of the Particle Type")]
        #endif
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private DrawingBlendingType _blendingType;
        public DrawingBlendingType BlendingType
        {
            get { return _blendingType; }
            set { _blendingType = value; }
        }
        private bool _useEmitterAngle;
        public bool UseEmitterAngle
        {
            get { return _useEmitterAngle; }
            set { _useEmitterAngle = value; }
        }
        private bool _keepAspectRatio;
        public bool KeepAspectRatio
        {
            get { return _keepAspectRatio; }
            set { _keepAspectRatio = value; }
        }
        private bool _attachParticlesToEmitter;
        public bool AttachParticlesToEmitter
        {
            get { return _attachParticlesToEmitter; }
            set { _attachParticlesToEmitter = value; }
        }
        private ParticleOrientationType _particleOrientationType;
        public ParticleOrientationType ParticleOrientationType
        {
            get { return _particleOrientationType; }
            set { _particleOrientationType = value; }
        }
        private float _fixedParticleOrientationAngle;
        public float FixedParticleOrientationAngle
        {
            get { return _fixedParticleOrientationAngle; }
            set { _fixedParticleOrientationAngle = value; }
        }
        private bool _useSingleParticle;
        public bool UseSingleParticle
        {
            get { return _useSingleParticle; }
            set 
            {                 
                if (value != _useSingleParticle)
                {
                    _singleParticleEmitted = false;
                    _useSingleParticle = value;
                }
            }
        }
        private bool _singleParticleEmitted;

        // animation and/or tile props
        private int _animationFrameDelay = -1;
        public int AnimationFrameDelay
        {
            get { return _animationFrameDelay; }
            set { _animationFrameDelay = value; }
        }
        private bool _animationLoop = true;
        public bool AnimationLoop
        {
            get { return _animationLoop; }
            set { _animationLoop = value; }
        }
        private int _tileDefault = 0;
        public int TileDefault
        {
            get { return _tileDefault; }
            set { _tileDefault = value; }
        }
        private bool _tileUseRandom = false;
        public bool TileUseRandom
        {
            get { return _tileUseRandom; }
            set { _tileUseRandom = value; }
        }
        private int _tileWidth = 0;
        public int TileWidth
        {
            get { return _tileWidth; }
            set { _tileWidth = value; }
        }
        private int _tileHeight = 0;
        public int TileHeight
        {
            get { return _tileHeight; }
            set { _tileHeight = value; }
        }
        private int _tileCols = 0;
        public int TileCols
        {
            get { return _tileCols; }
            set { _tileCols = value; }
        }
        private int _tileRows = 0;
        public int TileRows
        {
            get { return _tileRows; }
            set { _tileRows = value; }
        }
        private int _tileTotalOffset = 0;
        public int TileTotalOffset
        {
            get { return _tileTotalOffset; }
            set { _tileTotalOffset = value; }
        }
        
        // emitter set properties
        public float _emitterEmissionAngle;
        public float _emitterEmissionRange;

        #region Particles Settings

        // particles properties
        private LinearProperty _life;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty Life
        {
            get { return _life; }
            set { _life = value; }
        }
        private LinearProperty _quantity;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        private LinearProperty _width;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private LinearProperty _height;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty Height
        {
            get { return _height; }
            set { _height = value; }
        }
        private LinearProperty _velocity;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        private LinearProperty _weight;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
        private LinearProperty _spin;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty Spin
        {
            get { return _spin; }
            set { _spin = value; }
        }
        private LinearProperty _motionRandom;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty MotionRandom
        {
            get { return _motionRandom; }
            set { _motionRandom = value; }
        }
        private LinearProperty _opacity;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }
        private LinearProperty _emissionAngle;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty EmissionAngle
        {
            get { return _emissionAngle; }
            set { _emissionAngle = value; }
        }
        private LinearProperty _emissionRange;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty EmissionRange
        {
            get { return _emissionRange; }
            set { _emissionRange = value; }
        }
        private LinearProperty _redTint;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty RedTint
        {
            get { return _redTint; }
            set { _redTint = value; }
        }
        private LinearProperty _greenTint;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty GreenTint
        {
            get { return _greenTint; }
            set { _greenTint = value; }
        }
        private LinearProperty _blueTint;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty BlueTint
        {
            get { return _blueTint; }
            set { _blueTint = value; }
        }        

        #endregion

        #region Particles Variation Settings

        // random value variation
        private LinearProperty _lifeVariation;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty LifeVariation
        {
            get { return _lifeVariation; }
            set { _lifeVariation = value; }
        }
        private LinearProperty _quantityVariation;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty QuantityVariation
        {
            get { return _quantityVariation; }
            set { _quantityVariation = value; }
        }
        private LinearProperty _widthVariation;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty WidthVariation
        {
            get { return _widthVariation; }
            set { _widthVariation = value; }
        }
        private LinearProperty _heightVariation;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty HeightVariation
        {
            get { return _heightVariation; }
            set { _heightVariation = value; }
        }
        private LinearProperty _velocityVariation;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty VelocityVariation
        {
            get { return _velocityVariation; }
            set { _velocityVariation = value; }
        }
        private LinearProperty _weightVariation;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty WeightVariation
        {
            get { return _weightVariation; }
            set { _weightVariation = value; }
        }
        private LinearProperty _spinVariation;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty SpinVariation
        {
            get { return _spinVariation; }
            set { _spinVariation = value; }
        }
        private LinearProperty _motionRandomVariation;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty MotionRandomVariation
        {
            get { return _motionRandomVariation; }
            set { _motionRandomVariation = value; }
        }
        private LinearProperty _opacityVariation;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty OpacityVariation
        {
            get { return _opacityVariation; }
            set { _opacityVariation = value; }
        }
        private LinearProperty _redTintVariation;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty RedTintVariation
        {
            get { return _redTintVariation; }
            set { _redTintVariation = value; }
        }
        private LinearProperty _greenTintVariation;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty GreenTintVariation
        {
            get { return _greenTintVariation; }
            set { _greenTintVariation = value; }
        }
        private LinearProperty _blueTintVariation;
#if WINDOWS 
        [BrowsableAttribute(false)] 
#endif
        public LinearProperty BlueTintVariation
        {
            get { return _blueTintVariation; }
            set { _blueTintVariation = value; }
        }

        #endregion

        // overlife value variation
        public OverLifeSettings overLifeSettings;

        #endregion

        #region Properties

        #if WINDOWS
        [BrowsableAttribute(false),XmlIgnore()]
        #endif
        public Emitter Parent
        {
            get { return _parent; }
            set { _parent = value; }
        } 

        #if WINDOWS
        [ContentSerializerIgnore, XmlIgnore(), BrowsableAttribute(false)]
        #endif
        public Material Material
        {
            get { return _material; }
            set 
            { 
                _material = value;
            }
        }

        #if WINDOWS
        [BrowsableAttribute(false)]
        #endif
        public Vector2 Pivot
        {
            get { return _texturePivot; }
            set { _texturePivot = value; }
        }  

        #if WINDOWS 
        [BrowsableAttribute(false)] 
        #endif
        public int FreeParticleCount
        {
            get { return _freeParticles.Count; }
        }   

        #endregion

        #region Constructors

        public ParticleType()
        {
            _name = "New particle type";
            _material = IceCream.SceneManager.GlobalDataHolder.GetMaterial("DefaultMaterial");
            _texturePivot = new Vector2(0.5f);
            _freeParticles = new Queue<Particle>();
            _emissionTimerAccumulator = 0;
            _useEmitterAngle = false;
            _keepAspectRatio = true;
            _attachParticlesToEmitter = false;
            _blendingType = DrawingBlendingType.Alpha;
            _particleOrientationType = ParticleOrientationType.Fixed;
            _fixedParticleOrientationAngle = 0;
            InitializeValues();            
        }

        #endregion

        #region Methods

        public void CopyValuesTo(ParticleType particleType)
        {           
            particleType.AttachParticlesToEmitter = this.AttachParticlesToEmitter;
            particleType.BlendingType = this.BlendingType;
            particleType.Material = this.Material;
            particleType.FixedParticleOrientationAngle = this.FixedParticleOrientationAngle;
            particleType.KeepAspectRatio = this.KeepAspectRatio;
            particleType.Name = this.Name;
            particleType.ParticleOrientationType = this.ParticleOrientationType;
            particleType.UseEmitterAngle = this.UseEmitterAngle;
            particleType.UseSingleParticle = this.UseSingleParticle;
            particleType.TileDefault = this.TileDefault;
            particleType.TileUseRandom = this.TileUseRandom;
            particleType.TileWidth = this.TileWidth;
            particleType.TileHeight = this.TileHeight;
            particleType.TileRows = this.TileRows;
            particleType.TileCols = this.TileCols;
            particleType.AnimationFrameDelay = this.AnimationFrameDelay;
            particleType.AnimationLoop = this.AnimationLoop;
            particleType.TileTotalOffset = this.TileTotalOffset;
            this.BlueTint.CopyValuesTo(particleType.BlueTint);
            this.BlueTintVariation.CopyValuesTo(particleType.BlueTintVariation);
            this.EmissionAngle.CopyValuesTo(particleType.EmissionAngle);
            this.EmissionRange.CopyValuesTo(particleType.EmissionRange);
            this.GreenTint.CopyValuesTo(particleType.GreenTint);
            this.GreenTintVariation.CopyValuesTo(particleType.GreenTintVariation);
            this.Height.CopyValuesTo(particleType.Height);
            this.HeightVariation.CopyValuesTo(particleType.HeightVariation);
            this.Life.CopyValuesTo(particleType.Life);
            this.LifeVariation.CopyValuesTo(particleType.LifeVariation);
            this.MotionRandom.CopyValuesTo(particleType.MotionRandom);
            this.MotionRandomVariation.CopyValuesTo(particleType.MotionRandomVariation);
            this.Opacity.CopyValuesTo(particleType.Opacity);
            this.OpacityVariation.CopyValuesTo(particleType.OpacityVariation);
            this.Quantity.CopyValuesTo(particleType.Quantity);
            this.QuantityVariation.CopyValuesTo(particleType.QuantityVariation);
            this.RedTint.CopyValuesTo(particleType.RedTint);
            this.RedTintVariation.CopyValuesTo(particleType.RedTintVariation);
            this.Spin.CopyValuesTo(particleType.Spin);
            this.SpinVariation.CopyValuesTo(particleType.SpinVariation);
            this.Velocity.CopyValuesTo(particleType.Velocity);
            this.VelocityVariation.CopyValuesTo(particleType.VelocityVariation);
            this.Weight.CopyValuesTo(particleType.Weight);
            this.WeightVariation.CopyValuesTo(particleType.WeightVariation);
            this.Width.CopyValuesTo(particleType.Width);
            this.WidthVariation.CopyValuesTo(particleType.WidthVariation);
            this.overLifeSettings.CopyValuesTo(particleType.overLifeSettings);            
        }

        public void InitializeValues()
        {
            _useSingleParticle = false;
            overLifeSettings = new OverLifeSettings();

            _life = new LinearProperty(45, "Life", 0, 100);
            _quantity = new LinearProperty(100, "Quantity", 0, 100);
            _width = new LinearProperty(1, "Width", 0, 10);
            _height = new LinearProperty(1, "Height", 0, 10);
            _weight = new LinearProperty(0, "Weight", -10, 10);
            _velocity = new LinearProperty(4, "Velocity", 0, 10);
            _spin = new LinearProperty(0, "Spin", -10, 10);
            _motionRandom = new LinearProperty(0, "Motion Random", 0, 100);
            _opacity = new LinearProperty(1, "Opacity", 0, 1);
            _emissionAngle = new LinearProperty(0, "Emission Angle", 0, 360);
            _emissionRange = new LinearProperty(360, "Emission Range", 0, 360);
            _redTint = new LinearProperty(1, "Red Tint", 0, 1);
            _greenTint = new LinearProperty(1, "Green Tint", 0, 1);
            _blueTint = new LinearProperty(1, "Blue Tint", 0, 1);

            _lifeVariation = new LinearProperty(0, "Life Variation", 0, 200);
            _quantityVariation = new LinearProperty(0, "Quantity Variation", 0, 250);
            _widthVariation = new LinearProperty(0, "Width Variation", 0, 10);
            _heightVariation = new LinearProperty(0, "Height Variation", 0, 10);
            _weightVariation = new LinearProperty(0, "Weight Variation", 0, 10);
            _velocityVariation = new LinearProperty(0, "Velocity Variation", 0, 10);
            _spinVariation = new LinearProperty(0, "Spin Variation", 0, 10);
            _motionRandomVariation = new LinearProperty(0, "Motion Random Variation", 0, 10);
            _opacityVariation = new LinearProperty(0, "Opacity Variation", 0, 4);
            _redTintVariation = new LinearProperty(0, "Red Tint Variation", 0, 4);
            _greenTintVariation = new LinearProperty(0, "Green Tint Variation", 0, 4);
            _blueTintVariation = new LinearProperty(0, "Blue Tint Variation", 0, 4);          
        }

        public void OnEmissionFinished()
        {
            if (_emissionTimerAccumulator > 0)
            {
                SpawnParticle(1.0f, _parent.Shape.GetNewEmissionPoint());
                _emissionTimerAccumulator = 0;
            }
        }

        public void Update(float elapsed, float lerpLife, EmitterShape emitterShape, float opacityModifier, float emitterEmissionAngle, float emitterEmissionRange)
        {
            // if we are starting for the first time in this lifespan
            if (lerpLife == 0)
            {
                _singleParticleEmitted = false;
            }
            this._emitterEmissionAngle = emitterEmissionAngle;
            this._emitterEmissionRange = emitterEmissionRange;
            if ( _parent.IsStopped == false)
            {
                if (_useSingleParticle == true)
                {
                    if (_singleParticleEmitted == false)
                    {
                        SpawnParticle(lerpLife, emitterShape.GetNewEmissionPoint());
                        _singleParticleEmitted = true;
                    }
                }
                else
                {
                    float _newQuantity = Particle.GetLerpValueWithVariation(_quantity.Values, _quantityVariation.Values, lerpLife);
                    _newQuantity = _newQuantity / 60.0f;                    
                    _emissionTimerAccumulator += _newQuantity;
                    while (_emissionTimerAccumulator >= 1)
                    {
                        SpawnParticle(lerpLife, emitterShape.GetNewEmissionPoint());
                        _emissionTimerAccumulator -= 1;
                    }
                }
            }            
            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i].IsActive)
                {
                    _particles[i].Update(elapsed, opacityModifier);
                    // if it's dead now
                    if (_particles[i].IsActive == false)
                    {
                        // add it to the recycling queue
                        _freeParticles.Enqueue(_particles[i]);
                    }
                }
            }
        }

        private void SpawnParticle(float lerpLife, Vector2 position)
        {
            float _newLife = Particle.GetLerpValueWithVariation(_life.Values, _lifeVariation.Values, lerpLife);
            float _newQuantity = Particle.GetLerpValueWithVariation(_quantity.Values, _quantityVariation.Values, lerpLife);
            float _newWidth = Particle.GetLerpValueWithVariation(_width.Values, _widthVariation.Values, lerpLife);
            float _newHeight;
            if (_keepAspectRatio == false)
            {
                _newHeight = Particle.GetLerpValueWithVariation(_height.Values, _heightVariation.Values, lerpLife);
            }
            else
            {
                _newHeight = _newWidth;
            }
            float _newVelocity = Particle.GetLerpValueWithVariation(_velocity.Values, _velocityVariation.Values, lerpLife);
            float _newWeight = Particle.GetLerpValueWithVariation(_weight.Values, _weightVariation.Values, lerpLife);
            float _newSpin = Particle.GetLerpValueWithVariation(_spin.Values, _spinVariation.Values, lerpLife);
            float _newMotionRandom = Particle.GetLerpValueWithVariation(_motionRandom.Values, _motionRandomVariation.Values, lerpLife);
            float _newOpacity = Particle.GetLerpValueWithVariation(_opacity.Values, _opacityVariation.Values, lerpLife);            
            float _newEmissionAngle;
            if (_useEmitterAngle)
            {
                _newEmissionAngle = MathHelper.ToRadians((_emitterEmissionAngle + Particle.GetRandomVariationValue(_emitterEmissionRange)));
            }
            else
            {
                _newEmissionAngle = MathHelper.ToRadians(Particle.GetLerpValueWithVariation(_emissionAngle.Values, _emissionRange.Values, lerpLife));
            }
            float _newRedTint = MathHelper.Clamp(Particle.GetLerpValueWithVariation(_redTint.Values, _redTintVariation.Values, lerpLife), 0, 1);
            float _newGreenTint = MathHelper.Clamp(Particle.GetLerpValueWithVariation(_greenTint.Values, _greenTintVariation.Values, lerpLife), 0, 1);
            float _newBlueTint = MathHelper.Clamp(Particle.GetLerpValueWithVariation(_blueTint.Values, _blueTintVariation.Values, lerpLife), 0, 1);
            float _rotation;
            if (_particleOrientationType == ParticleOrientationType.Random)
            {
                _rotation = Randomizer.AngleInDegrees();
            }
            else
            {
                _rotation = _fixedParticleOrientationAngle;
            }
            
            Particle _newParticle;
            // if no more place, add a new particle
            if (_freeParticles.Count == 0)
            {
                _newParticle = new Particle();
                _particles.Add(_newParticle);
            }
            //else use a free slot
            else
            {
                _newParticle = _freeParticles.Dequeue();
            }           
            int tile = _tileDefault;
            if (TileUseRandom == true)
            {
                int tileTotal = TileRows * TileCols - TileTotalOffset;
                tile = Randomizer.Integer(0, tileTotal-1);
            }
            // change the settings
            _newParticle.ModifiyParticle(this, _material, tile, _keepAspectRatio, _particleOrientationType, _newLife, _newWidth, _newHeight, _newVelocity, _newWeight, _newSpin, _newMotionRandom, _newEmissionAngle,
                position, _rotation, _newRedTint, _newGreenTint, _newBlueTint, _newOpacity, _texturePivot, true, overLifeSettings);
            // activate it
            _newParticle.IsActive = true;
        }

        /// <summary>
        /// Updates each particles position with a displacement value
        /// </summary>
        /// <param name="displacement"></param>
        public void UpdateParticlesPosition(Vector2 displacement)
        {
            // only update if attached to emitter
            if (_attachParticlesToEmitter == true)
            {
                for (int i = 0; i < _particles.Count; i++)
                {     
                    _particles[i].Position += displacement;
                }
            }
        }

        /// <summary>
        /// Draw each particle on the selected layer
        /// </summary>
        public void Draw()
        {
            if (_material != null)
            {
                for (int i = 0; i < _particles.Count; i++)
                {
                    _particles[i].Draw(Parent.Parent.Layer, _blendingType);
                }
            }
        }

        public override string ToString()
        {
            int count = 0;
            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i].IsActive)
                {
                    count++;
                }
            }
            return "[ParticleType] Actives: " + count + " Free: " + _freeParticles.Count;
        }

        #endregion
    }

}
