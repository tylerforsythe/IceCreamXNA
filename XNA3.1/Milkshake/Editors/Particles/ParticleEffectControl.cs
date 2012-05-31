using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using IceCream;
using IceCream.Drawing;
using IceCream.SceneItems;
using IceCream.SceneItems.ParticlesClasses;
using Milkshake.GraphicsDeviceControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Milkshake.Editors.Particles
{
    public class ParticleEffectControl : GraphicsDeviceControl
    {
        #region Fields

        ParticleEffect particleEffect = new ParticleEffect();
        Vector2 updatedPosition;
        Color backgroundColor;
        Texture2D background;
        // Timer controls the update.
        Stopwatch timer;
        ParticleEffectEditor _parent;

        #endregion

        #region Properties

        internal ParticleEffect ParticleEffect
        {
            get
            {
                return particleEffect;
            }
            set
            {
                particleEffect = value;
            }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        private Vector2 _realMousePos = Vector2.Zero;
        public Vector2 RealMousePos
        {
            get { return _realMousePos; }
            set
            {
                _realMousePos = value;
                if (ParentEditor != null)
                {
                    _sceneMousePos = this.ParentEditor.ZoomBox.Camera.ConvertToWorldPos(_realMousePos);
                }
            }
        }
        private Vector2 _sceneMousePos = Vector2.Zero;
        public Vector2 SceneMousePos
        {
            get { return _sceneMousePos; }
            set
            {
                _sceneMousePos = value;
            }
        }

        public ParticleEffectEditor ParentEditor
        {
            get;
            set;
        }

        #endregion        

        #region Constructor

        public ParticleEffectControl()
        {
            particleEffect = new ParticleEffect();            
        }

        #endregion

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            // Start the animation timer.
            timer = Stopwatch.StartNew();
            backgroundColor = Color.Black;                

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        public void AddNewParticleType()
        {
            ParticleType newParticleType = new ParticleType();
            newParticleType.Material = SceneManager.GetEmbeddedParticleMaterial();
            particleEffect.Emitter.ParticleTypes.Add(newParticleType);
            particleEffect.Play();
            // add a fade in effect
            particleEffect.Emitter.ParticleTypes[particleEffect.Emitter.ParticleTypes.Count - 1].
                overLifeSettings.opacityOverLife.Values.Add(new Vector2(1, 0));            
        }

        public void RemoveParticleType(int index)
        {
            if (particleEffect.Emitter.ParticleTypes.Count > index)
            {
                particleEffect.Emitter.ParticleTypes.RemoveAt(index);
            }
        }

        public void InitializeParticleEffect()
        {
            particleEffect.Play();
            updatedPosition = Vector2.Zero;
            backgroundColor = particleEffect.EditorBackgroundColor;
        }

        public void LoadBackground(Texture2D texture)
        {
            if (texture != null)
            {
                background = texture;
            }
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            ParentEditor.ZoomBox.Camera.Update(1 / 60f);   
            if (particleEffect.Position != updatedPosition)
            {
                particleEffect.Position = updatedPosition;
            }
            particleEffect.Update(1 / 60f);
            _parent = Parent as ParticleEffectEditor;
            // if the animation was paused externally, pause it in the GUI
            if (particleEffect.Emitter.IsPaused == true && _parent.toolStripButtonPause.Enabled == true)
            {
                _parent.toolStripButtonPause_Click(null, EventArgs.Empty);
            }
            // if the animation was stopped externally, stop it in the GUI
            if (particleEffect.Emitter.IsStopped == true && _parent.toolStripButtonStop.Enabled == true)
            {
                _parent.toolStripButtonStop_Click(null, EventArgs.Empty);
            }
            if (background != null)
            {
                DrawRequest backgroundRequest = new DrawRequest(background, Vector2.Zero, false,
                    null, 0, Vector2.One, new Vector2(0.5f), true, Color.White, false, false, null);
                DrawingManager.DrawOnLayer(backgroundRequest, 10, DrawingBlendingType.Alpha);
            }
            particleEffect.Draw(1f);
            GraphicsDevice.Clear(backgroundColor);

            DrawingManager.ViewPortSize = new Point(this.Width, this.Height);
            MilkshakeForm.SwapCameraAndRenderScene(ParentEditor.ZoomBox.Camera);
            ParentEditor.Update(1 / 60f);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.RealMousePos = new Vector2(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                updatedPosition = this.SceneMousePos;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            updatedPosition = this.SceneMousePos;
            base.OnMouseDown(e);
        }

    }
}
