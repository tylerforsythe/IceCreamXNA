using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using IceCream;
using IceCream.Drawing;
using IceCream.SceneItems;
using IceCream.Drawing.IceEffects;
using MilkshakeLibrary;
using Milkshake.GraphicsDeviceControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Milkshake.Editors.PostProcessAnimations
{
    public partial class PostProcessAnimationEditor : SceneItemEditor
    {
        internal ZoomBox ZoomBox
        {
            get;
            set;
        }

        public override SceneItem SceneItem
        {
            get
            {
                return base.SceneItem;
            }
            set
            {
                base.SceneItem = value;
                PostProcessAnimation = base.SceneItem as PostProcessAnimation;
            }
        }

        internal PostProcessAnimation PostProcessAnimation
        {
            get { return postProcessAnimControl.PostProcessAnimation; }
            set { postProcessAnimControl.PostProcessAnimation = value; }
        }

        public PostProcessAnimationEditor()
        {
            this.ZoomBox = new ZoomBox();
            this.ZoomBox.Camera.Pivot = Vector2.Zero;
            this.ZoomBox.Camera.IsPivotRelative = false;
            InitializeComponent();
            postProcessAnimControl.ParentEditor = this;
        }

        private void PostProcessAnimationEditor_Load(object sender, EventArgs e)
        {
            // populate the iceeffects treeview nodes
            for (int i = 0; i < (int)EmbeddedIceEffectType.SizeOfEnum; i++)
            {
                TreeNode lastNode = treeviewEffects.Nodes[0].Nodes.Add(((EmbeddedIceEffectType)i).ToString());
                lastNode.Tag = DrawingManager.EmbeddedIceEffects[i];
            }

            for (int i = 0; i < (int)SceneManager.GlobalDataHolder.Effects.Count; i++)
            {
                IceEffect eff = SceneManager.GlobalDataHolder.Effects[i];
                TreeNode lastNode = treeviewEffects.Nodes[1].Nodes.Add(eff.Name);
                lastNode.Tag =eff;
            }
            // select the corresponding effect node
            treeviewEffects.SelectedNode = GetEffectNode(PostProcessAnimation.IceEffect);
            textBoxLife.Text = PostProcessAnimation.Life.ToString(CultureInfo.InvariantCulture);
            textBoxLoopAmount.Text = PostProcessAnimation.LoopMax.ToString(CultureInfo.InvariantCulture);
            // load the background sample image
            postProcessAnimControl.LoadBackground(Texture2D.FromFile(
                postProcessAnimControl.GraphicsDevice, Application.StartupPath + "\\Resources\\ppBackground.jpg"));
        }

        #region Events

        private void treeviewEffects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is IceEffect)
            {
                SelectNewEffect((IceEffect)e.Node.Tag);
            }
        }        

        private void comboBoxParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = comboBoxParameters.SelectedIndex;
            linearPropertyControl.SelectedLinearProperty = PostProcessAnimation.LinearProperties[i];
        }

        private void textBoxLife_Validated(object sender, EventArgs e)
        {
            try
            {
                int newLife = Int32.Parse(textBoxLife.Text, System.Globalization.CultureInfo.InvariantCulture);
                if (newLife <= 0)
                {
                    throw new Exception("The life of the animation must be greater than 0");
                }
                PostProcessAnimation.Life = newLife;
            }
            catch (Exception ex)
            {
                textBoxLife.Text = PostProcessAnimation.Life.ToString(CultureInfo.InvariantCulture);
                MilkshakeForm.ShowErrorMessage(ex.Message);
            }
        }

        private void textBoxLoopAmount_Validated(object sender, EventArgs e)
        {
            try
            {
                int newLoop = int.Parse(textBoxLoopAmount.Text, System.Globalization.CultureInfo.InvariantCulture);
                if (newLoop < 0)
                {
                    throw new Exception("The loop amount of the animation must be greater than or equal to 0 (0 = infinite)");
                }
                PostProcessAnimation.LoopMax = newLoop;
            }
            catch (Exception ex)
            {
                textBoxLoopAmount.Text = PostProcessAnimation.LoopMax.ToString(CultureInfo.InvariantCulture);
                MilkshakeForm.ShowErrorMessage(ex.Message);
            }
        }

        private void textBoxLife_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxLife_Validated(sender, EventArgs.Empty);
            }
        }

        private void textBoxLoopAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxLoopAmount_Validated(sender, EventArgs.Empty);
            }
        }

        public void toolStripButtonPlay_Click(object sender, EventArgs e)
        {
            PostProcessAnimation.Reset();
            PostProcessAnimation.Play();
            toolStripButtonPlay.Enabled = false;
            toolStripButtonPause.Enabled = true;
            toolStripButtonStop.Enabled = true;
        }

        public void toolStripButtonPause_Click(object sender, EventArgs e)
        {
            PostProcessAnimation.Pause();
            toolStripButtonPlay.Enabled = true;
            toolStripButtonPause.Enabled = false;
            toolStripButtonStop.Enabled = true;
        }

        public void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            PostProcessAnimation.Stop();
            toolStripButtonPlay.Enabled = true;
            toolStripButtonPause.Enabled = false;
            toolStripButtonStop.Enabled = false;
        }

        #endregion

        #region Methods

        private TreeNode GetEffectNode(IceEffect effect)
        {
            // check all the embedded effects first
            for (int i = 0; i < DrawingManager.EmbeddedIceEffects.Length; i++)
            {
                if (DrawingManager.EmbeddedIceEffects[i] == effect)
                {
                    return treeviewEffects.Nodes[0].Nodes[i];
                }
            }
            // return the first embedded ice effect if none was found
            return treeviewEffects.Nodes[0].Nodes[0];
        }

        private void SelectNewEffect(IceEffect effect)
        {
            bool paramtersBoxEnabled = false;            
            comboBoxParameters.Items.Clear();
            comboBoxParameters.SelectedItem = null;
            comboBoxParameters.SelectedText = "";
            comboBoxParameters.Text = "";
            // if the effect is a new one
            if (effect != PostProcessAnimation.IceEffect)
            {
                PostProcessAnimation.IceEffect = effect;
                // load the default properties for this effect
                for (int i = 0; i < 8; i++)
                {
                    LinearProperty selectedLinearProperty = PostProcessAnimation.LinearProperties[i];
                    if (effect.ParametersProperties != null && i < effect.ParametersProperties.Length 
                        && effect.ParametersProperties[i] != null)
                    {
                        // use the effect's default linear property 
                        effect.ParametersProperties[i].CopyValuesTo(selectedLinearProperty);
                    }
                    else
                    {
                        selectedLinearProperty = new LinearProperty(0, String.Empty, 0, 10);
                    }
                }                
            }
            if (PostProcessAnimation.IceEffect.ParametersProperties != null)
            {
                for (int i = 0; i < PostProcessAnimation.IceEffect.ParametersProperties.Length; i++)
                {
                    if (PostProcessAnimation.IceEffect.ParametersProperties[i] != null)
                    {
                        comboBoxParameters.Items.Add(PostProcessAnimation.IceEffect.ParametersProperties[i].Description);
                    }
                }
                // select the first index if possible
                if (PostProcessAnimation.IceEffect.ParametersProperties.Length >= 1)
                {
                    comboBoxParameters.SelectedIndex = 0;
                    paramtersBoxEnabled = true;
                }                
            }
            if (paramtersBoxEnabled == false)
            {
                comboBoxParameters.Enabled = false;
                labelParameters.Enabled = false;
                linearPropertyControl.Visible = false;
            }
            else
            {
                comboBoxParameters.Enabled = true;
                labelParameters.Enabled = true;
                linearPropertyControl.Visible = true;
            }
        }

        #endregion

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}