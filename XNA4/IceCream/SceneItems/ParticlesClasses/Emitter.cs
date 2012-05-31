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
using System.ComponentModel;
using System.Xml.Serialization;
using IceCream.Drawing;

namespace IceCream.SceneItems.ParticlesClasses
{
    public class Emitter
    {
        #region Fields

        private List<ParticleType> particleTypes;
        private float maxLife;
        private float currentLife;
        private LinearProperty globalOpacityModifier;
        private LinearProperty emissionAngle;
        private LinearProperty emissionRange;
        private Vector2 position;
        private EmitterShape shape;
        private bool _isPaused;
        private bool _isStopped;
        private ParticleEffect _parent;
        private Vector2 _attractorPositionOffset;
        private Vector2 _attractorPosition;
        private bool _attractorEnabled;
        private float _attractorForce;
        private int _loopCounter;
       
        #endregion

        #region Properties

        #if WINDOWS
        [BrowsableAttribute(false)]
        #endif
        [XmlIgnore]
        public ParticleEffect Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

#if WINDOWS
        [BrowsableAttribute(false)]
#endif
        [XmlIgnore]
        public bool IsStopped
        {
            get { return _isStopped; }
            set { _isStopped = value; }
        }
#if WINDOWS
        [BrowsableAttribute(false)]
#endif
        [XmlIgnore]
        public bool IsPaused
        {
            get { return _isPaused; }
            set { _isPaused = value; }
        }
        #if WINDOWS 
        [BrowsableAttribute(false)] 
        #endif
        public List<ParticleType> ParticleTypes
        {
            get { return particleTypes; }
            set { particleTypes = value; }
        }
        #if WINDOWS 
        [BrowsableAttribute(false)] 
        #endif
        public EmitterShape Shape
        {
            get { return shape; }
            set { shape = value; }
        }        
        
        #if WINDOWS 
        [BrowsableAttribute(false)] 
        #endif
        public LinearProperty GlobalOpacityModifier
        {
            get { return globalOpacityModifier; }
            set { globalOpacityModifier = value; }
        }
        #if WINDOWS 
        [BrowsableAttribute(false)] 
        #endif
        public LinearProperty EmissionAngle
        {
            get { return emissionAngle; }
            set { emissionAngle = value; }
        }
        #if WINDOWS 
        [BrowsableAttribute(false)] 
        #endif
        public LinearProperty EmissionRange
        {
            get { return emissionRange; }
            set { emissionRange = value; }
        }
        #if WINDOWS 
        [BrowsableAttribute(false)] 
        #endif
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                if (position != value)
                {  
                    // update particles types
                    Vector2 displacement = value - position;
                    for (int i = 0; i < particleTypes.Count; i++)
                    {
                        particleTypes[i].UpdateParticlesPosition(displacement);
                    }
                    position = value;
                    shape.Position = position;
                    _attractorPosition = value + _attractorPositionOffset;
                }
            }
        }

        public Vector2 AttractorPosition
        {
            get { return _attractorPosition; }
        }
        public Vector2 AttractorPositionOffset
        {
            get { return _attractorPositionOffset; }
            set 
            { 
                _attractorPositionOffset = value;
                _attractorPosition = position + _attractorPositionOffset;
            }
        }
        public float AttractorForce
        {
            get { return _attractorForce; }
            set { _attractorForce = value; }
        }
        public bool AttractorEnabled
        {
            get { return _attractorEnabled; }
            set { _attractorEnabled = value; }
        }

        #endregion       

        #region Constructor

        public Emitter()
        {
            particleTypes = new List<ParticleType>();
            shape = new EmitterShape(EmitterShapeType.Point);
            emissionAngle = new LinearProperty(0, "Emission Angle", 0, 360);
            emissionRange = new LinearProperty(360, "Emission Range", 0, 360);
            globalOpacityModifier = new LinearProperty(1, "Global Opacity Modifier", 0, 1);
            _attractorEnabled = false;
            _attractorForce = 2f;
            _attractorPositionOffset = new Vector2(0, 0);
            _isStopped = false;
            _isPaused = false;
        }

        #endregion

        #region Methods

        public void CopyValuesTo(Emitter emitter)
        {   
            emitter.currentLife = this.currentLife;
            this.GlobalOpacityModifier.CopyValuesTo(emitter.GlobalOpacityModifier);
            this.EmissionAngle.CopyValuesTo(emitter.EmissionAngle);
            this.EmissionRange.CopyValuesTo(emitter.EmissionRange);            
            emitter.Shape = this.Shape;
            emitter.IsPaused = this.IsPaused;
            emitter.IsStopped = this.IsStopped;
            for (int i = 0; i < this.particleTypes.Count; i++)
            {
                // if no particle type is available
                if (emitter.ParticleTypes.Count <= i)
                {
                    emitter.ParticleTypes.Add(new ParticleType());
                }
                this.ParticleTypes[i].CopyValuesTo(emitter.ParticleTypes[i]);
            }
            // Remove remaining types (can cause garbage!)
            for (int i = emitter.ParticleTypes.Count; i > this.particleTypes.Count; i--)
            {
                emitter.ParticleTypes.RemoveAt(i-1);
            }
            emitter.AttractorEnabled = this.AttractorEnabled;
            emitter.AttractorForce = this.AttractorForce;            
            emitter.AttractorPositionOffset = this.AttractorPositionOffset;
        }

        public void Update(float elapsed)
        {
            if (_isPaused == false)
            {
                float lerpLife = currentLife / maxLife;
                // update the particles with the emitter properties
                for (int i = 0; i < particleTypes.Count; i++)
                {
                    if (particleTypes[i].Parent != this)
                    {
                        particleTypes[i].Parent = this;
                    }
                    float _emissionAngle = Particle.GetLerpValue(emissionAngle.Values, lerpLife);
                    float _emissionRange = Particle.GetLerpValue(emissionRange.Values, lerpLife);
                    float _opacityModifier = Particle.GetLerpValue(globalOpacityModifier.Values, lerpLife);
                    particleTypes[i].Update(elapsed, lerpLife, shape, _opacityModifier, _emissionAngle, _emissionRange);
                }
                currentLife += 1.0f;
                if (currentLife > maxLife)
                {
                    // reset the life
                    currentLife = 0.0f;
                    // increment the loop counter
                    _loopCounter++;
                    // check if we need to stop
                    if (_parent.LoopMax > 0 && _loopCounter >= _parent.LoopMax)
                    {
                        _isStopped = true;
                        for (int i = 0; i < particleTypes.Count; i++)
                        {
                            particleTypes[i].OnEmissionFinished();
                        }
                    }                    
                }
            }
        }

        public void Draw()
        {            
            for (int i = 0; i < particleTypes.Count; i++)
            {
                particleTypes[i].Draw();
            }
            /*
            if (this.AttractorEnabled == true)
            {
                DrawingManager.DrawFilledRectangle(_parent.Layer, AttractorPosition, new Vector2(2, 2),
                    Color.Red, DrawingBlendingType.Alpha);
            }*/
        }

        public void Initialize(float life)
        {            
            // life in number of seconds * fps
            maxLife = life;// *60;
            _loopCounter = 0;
            currentLife = 0;
        }

        public override string ToString()
        {
            String status = "Emitter: { ";
            status += "Position: " + position;
            for (int i = 0; i < particleTypes.Count; i++)
            {
                status += particleTypes[i].ToString();
                if (i != particleTypes.Count)
                {
                    status += " | ";
                }
            }
            status += " }"; 
            return status;
        }

        #endregion        
    }
}
