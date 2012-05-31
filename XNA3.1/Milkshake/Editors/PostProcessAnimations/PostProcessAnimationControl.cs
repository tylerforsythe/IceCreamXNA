using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using IceCream;
using IceCream.Drawing;
using IceCream.SceneItems;
using Milkshake.GraphicsDeviceControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IceCream.Drawing.IceEffects;

namespace Milkshake.Editors.PostProcessAnimations
{
    public class PostProcessAnimationControl : GraphicsDeviceControl
    {
        private static NoEffect _noEffect = new NoEffect();

        #region Fields

        PostProcessAnimation _ppAnim;
        Texture2D _background;
        private PostProcessAnimationEditor _parent;

        #endregion

        #region Properties

        internal PostProcessAnimation PostProcessAnimation
        {
            get { return _ppAnim; }
            set { _ppAnim = value; }
        }

        internal Texture2D Background
        {
            get { return _background; }
            set { _background = value; }
        }

        internal PostProcessAnimationEditor ParentEditor
        {
            get;
            set;
        }

        #endregion        

        #region Constructor

        public PostProcessAnimationControl()
        {
            _ppAnim = new PostProcessAnimation(_noEffect);
        }

        #endregion

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {            
            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }        

        public void LoadBackground(Texture2D texture)
        {
            if (texture != null)
            {
                _background = texture;
            }
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.Black);
            if (_background != null)
            {
                float scaledWidth = this.Width / (float)_background.Width;
                float scaledHeight = this.Height / (float)_background.Height;
                DrawRequest backgroundRequest = new DrawRequest(_background, Vector2.Zero,false,
                    null, 0, new Vector2(scaledWidth, scaledHeight), Vector2.Zero, false, Color.White, false, false, null);
                DrawingManager.DrawOnLayer(backgroundRequest, 10, DrawingBlendingType.Alpha);
            }
            _ppAnim.Update(1 / 60f);
            _parent = Parent as PostProcessAnimationEditor;
            // if the animation was paused externally, pause it in the GUI
            if (_ppAnim.IsPaused == true && _parent.toolStripButtonPause.Enabled == true)
            {
                _parent.toolStripButtonPause_Click(null, EventArgs.Empty);
            }
            // if the animation was stopped externally, stop it in the GUI
            if (_ppAnim.IsStopped == true && _parent.toolStripButtonStop.Enabled == true)
            {
                _parent.toolStripButtonStop_Click(null, EventArgs.Empty);
            }
            int oldLayer = _ppAnim.Layer;
            _ppAnim.Draw(1 / 60f);
            _ppAnim.Layer = oldLayer;
            DrawingManager.ViewPortSize = new Point(this.Width, this.Height);            
            MilkshakeForm.SwapCameraAndRenderScene(ParentEditor.ZoomBox.Camera);
            ((PostProcessAnimationEditor)this.Parent).Update(1/60f);
        }
    }
}
