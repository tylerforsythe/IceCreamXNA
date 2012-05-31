/*
 * WARNING: This ParticleEditor was a test project as my first WinForm ever, 
 * and as such a lot of code is redudant and needs to be cleaned.
 * 
 * / Epsicode   
 */

#region Using Statements

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using IceCream;
using IceCream.Drawing;
using IceCream.SceneItems;
using IceCream.SceneItems.ParticlesClasses;
using MilkshakeLibrary;
using Milkshake.SelectorDialogs;

#endregion

namespace Milkshake.Editors.Particles
{
    public partial class ParticleEffectEditor : SceneItemEditor
    {
        private String lastTextureMaskPath;

        public override SceneItem SceneItem
        {
            get
            {
                return base.SceneItem;
            }
            set
            {
                base.SceneItem = value;
                ParticleEffect = base.SceneItem as ParticleEffect;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal ParticleEffect ParticleEffect
        {
            get { return particleEffectControl.ParticleEffect; }
            set { particleEffectControl.ParticleEffect = value; }        
        }

        internal ParticleEffectControl ParticleEffectControl
        {
            get { return particleEffectControl; }
        }

        public ZoomBox ZoomBox
        {
            get;
            set;
        }

        /// <summary>
        /// Constructs the main form.
        /// </summary>
        public ParticleEffectEditor()
        {
            InitializeComponent();                           
        }

        private void toolStripButtonImage_Click(object sender, EventArgs e)
        {
            OpenTextureSelectionDialog();
        } 

        private void OpenTextureSelectionDialog()
        {
            ParticleType pType = null;
            // check each particle types
            for (int i = 0; i < particleEffectControl.ParticleEffect.Emitter.ParticleTypes.Count; i++)
            {
                // check the index + 1 (index 0 is Global Settings)
                if (particleEffectProperties.SelectedNode == particleEffectProperties.Nodes[0].Nodes[0].Nodes[i + 1])
                {
                    pType = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i];
                }
            }
            // if the selected node was a particle type
            if (pType != null)
            {
                MaterialSelectorDialog materialSelectorDialog = new MaterialSelectorDialog();
                materialSelectorDialog.SelectedMaterial = pType.Material;
                materialSelectorDialog.ShowLocalTextures = ItemIsLocal;
                if (materialSelectorDialog.ShowDialog() == DialogResult.OK)
                {
                    pType.Material = materialSelectorDialog.SelectedMaterial;                    
                }  
            }      
        }

        private void toolStripColorButton_Click(object sender, EventArgs e)
        {
            OpenColorSelectionDialog();
        }

        private void OpenColorSelectionDialog()
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.FullOpen = true;
            colorDialog.Color = System.Drawing.Color.FromArgb(particleEffectControl.BackgroundColor.R,
                particleEffectControl.BackgroundColor.G, particleEffectControl.BackgroundColor.B);
            if (colorDialog.ShowDialog() != DialogResult.Cancel)
            {
                particleEffectControl.BackgroundColor = new Microsoft.Xna.Framework.Color(
                    colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                particleEffectControl.BackColor = colorDialog.Color;
                ParticleEffect.EditorBackgroundColor = particleEffectControl.BackgroundColor;
            }
        }

        private void OpenLoadBackgroundDialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Load image";
            fileDialog.Filter = "PNG Files (*.png)|*.png|" +
                                "TGA Files (*.tga)|*.tga|" +
                                "BMP Files (*.bmp)|*.bmp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {           
                Texture2D newTexture;
                newTexture = Texture2D.FromStream(particleEffectControl.GraphicsDevice, System.IO.File.OpenRead(fileDialog.FileName));
                particleEffectControl.LoadBackground(newTexture);
            }
        }

        private void toolStripButtonBackground_Click(object sender, EventArgs e)
        {
            OpenLoadBackgroundDialog();
        }

        void UpdateParticleEffectTree()
        {
            toolStripButtonDelPartType.Enabled = false;
            toolStripButtonImage.Enabled = false;            
            particleEffectProperties.Nodes[0].Nodes.Clear();
            particleEffectProperties.Nodes[0].Text = particleEffectControl.ParticleEffect.Name;
            particleEffectProperties.Nodes[0].Nodes.Add("NodeEmitter", "Emitter", 5, 5);
            CreateEmitterSubNodes(particleEffectProperties.Nodes[0].Nodes[0], particleEffectControl.ParticleEffect.Emitter);            
            particleEffectProperties.Nodes[0].Expand();
            particleEffectProperties.Nodes[0].Nodes[0].Expand();
        }

        void CreateEmitterSubNodes(TreeNode rootNode, Emitter emitter)
        {
            rootNode.Nodes.Clear();
            rootNode.Nodes.Add("NodeGlobalSettings", "Global settings");
            rootNode.Nodes[0].Nodes.Add("NodeGlobalOpacityModifier", "Global opacity modifier", 1, 2);
            rootNode.Nodes[0].Nodes.Add("NodeEmissionAngle", "Emission angle", 1, 2);
            rootNode.Nodes[0].Nodes.Add("NodeEmissionRange", "Emission range", 1, 2);
            for (int i = 0; i < emitter.ParticleTypes.Count; i++)
            {
                rootNode.Nodes.Add("", emitter.ParticleTypes[i].Name, 3, 3);
                CreateParticleTypeSubNodes(rootNode.Nodes[rootNode.Nodes.Count -1], emitter.ParticleTypes[i]);
            }            
        }

        void CreateParticleTypeSubNodes(TreeNode rootNode, ParticleType particleType)
        {            
            rootNode.Nodes.Clear();
            rootNode.Nodes.Add("NodeParticleTypeLife", "Life", 1, 2);
            rootNode.Nodes.Add("NodeParticleTypeQuantity", "Quantity", 1, 2);
            rootNode.Nodes.Add("NodeParticleTypeWidth", "Width", 1, 2);
            rootNode.Nodes.Add("NodeParticleTypeHeight", "Height", 1, 2);
            rootNode.Nodes.Add("NodeParticleTypeVelocity", "Velocity", 1, 2);
            rootNode.Nodes.Add("NodeParticleTypeWeight", "Weight", 1, 2);            
            rootNode.Nodes.Add("NodeParticleTypeSpin", "Spin", 1, 2);
            rootNode.Nodes.Add("NodeParticleTypeMotionRandom", "Motion Random", 1, 2);            
            rootNode.Nodes.Add("NodeParticleTypeOpacity", "Opacity", 1, 2);
            rootNode.Nodes.Add("NodeParticleTypeEmissionAngle", "EmissionAngle", 1, 2);
            rootNode.Nodes.Add("NodeParticleTypeEmissionRange", "EmissionRange", 1, 2);
            rootNode.Nodes.Add("NodeParticleTypeRedTint", "Red tint", 1, 2);
            rootNode.Nodes.Add("NodeParticleTypeGreenTint", "Green tint", 1, 2);
            rootNode.Nodes.Add("NodeParticleTypeBlueTint", "Blue tint", 1, 2);
            rootNode.Nodes.Add("NodeVariationSettings", "Variation settings", 4, 4);
            int i = rootNode.Nodes.Count - 1;
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeVariationLife", "Life", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeVariationQuantity", "Quantity", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeVariationWidth", "Width", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeVariationHeight", "Height", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeVariationVelocity", "Velocity", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeVariationWeight", "Weight", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeVariationSpin", "Spin", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeVariationMotionRandom", "Motion Random", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeVariationOpacity", "Opacity", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeVariationRedTint", "Red tint", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeVariationGreenTint", "Green tint", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeVariationBlueTint", "Blue tint", 1, 2);            
            rootNode.Nodes.Add("NodeOverLifeSettings", "Over life settings", 6, 6);
            i = rootNode.Nodes.Count - 1;
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeOverLifeWidth", "Width", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeOverLifeHeight", "Height", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeOverLifeVelocity", "Velocity", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeOverLifeWeight", "Weight", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeOverLifeSpin", "Spin", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeOverLifeMotionRandom", "Motion Random", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeOverLifeOpacity", "Opacity", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeOverLifeRedTint", "Red tint", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeOverLifeGreenTint", "Green tint", 1, 2);
            rootNode.Nodes[i].Nodes.Add("NodeParticleTypeOverLifeBlueTint", "Blue tint", 1, 2);
            rootNode.Collapse();
        }        

        private void ParticleEffectEditor_Load(object sender, EventArgs e)
        {
            ZoomBox = new ZoomBox();
            ZoomBox.SetToolStripButtomZoomIn(toolStripButtonZoomIn);
            ZoomBox.SetToolStripButtomZoomOut(toolStripButtonZoomOut);
            ZoomBox.SetToolStripButtomZoomNormal(toolStripButtonZoomNormal);
            ZoomBox.Camera.Pivot = new Vector2(0.5f);
            ZoomBox.Camera.IsPivotRelative = true;
            particleEffectControl.ParentEditor = this;
            particleEffectControl.InitializeParticleEffect();
            UpdateParticleEffectTree();
            particleEffectProperties.SelectedNode = particleEffectProperties.Nodes[0];  
        }

        private void ProcessNodeSelection(TreeNode selectedNode)
        {
            toolStripButtonDelPartType.Enabled = false;
            toolStripButtonImage.Enabled = false;
            // if the selection is the particle effect root
            if (selectedNode == particleEffectProperties.Nodes[0])
            {
                LinearPropertyControl.Visible = false;
                propertyGrid.Visible = false;
                //propertyGrid.SelectedObject = particleEffectControl.ParticleEffect;
                return;
            }
            else
            {
                // if the selection is the emitter
                if (selectedNode == particleEffectProperties.Nodes[0].Nodes[0])
                {
                    LinearPropertyControl.Visible = false;
                    propertyGrid.Visible = true;
                    propertyGrid.SelectedObject = particleEffectControl.ParticleEffect.Emitter;
                    return;
                }
                else
                {
                    // check global emmitter settings
                    for (int i = 0; i < 3; i++)
                    {
                        if (selectedNode == particleEffectProperties.Nodes[0].Nodes[0].Nodes[0].Nodes[i])
                        {
                            LinearPropertyControl.Visible = true;
                            propertyGrid.Visible = false;
                            LinearProperty LinearProperty;
                            switch (i)
                            {
                                case 0:
                                    LinearProperty = particleEffectControl.ParticleEffect.Emitter.GlobalOpacityModifier;
                                    break;
                                case 1:
                                    LinearProperty = particleEffectControl.ParticleEffect.Emitter.EmissionAngle;
                                    break;
                                case 2:
                                    LinearProperty = particleEffectControl.ParticleEffect.Emitter.EmissionRange;
                                    break;
                                default:
                                    LinearProperty = particleEffectControl.ParticleEffect.Emitter.GlobalOpacityModifier;
                                    break;
                            }
                            LinearPropertyControl.SelectedLinearProperty = LinearProperty;
                            return;
                        }
                    }
                    // check particle types
                    for (int i = 0; i < particleEffectControl.ParticleEffect.Emitter.ParticleTypes.Count; i++)
                    {
                        // check the root 
                        if (selectedNode == particleEffectProperties.Nodes[0].Nodes[0].Nodes[i + 1])
                        {
                            toolStripButtonDelPartType.Enabled = true;
                            toolStripButtonImage.Enabled = true;
                            LinearPropertyControl.Visible = false;
                            propertyGrid.Visible = true;
                            propertyGrid.SelectedObject = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i];
                            return;
                        }


                        #region Sub Menu particle type

                        // check sub menus of particle types                        
                        for (int j = 0; j < 14; j++)
                        {
                            if (selectedNode == particleEffectProperties.Nodes[0].Nodes[0].Nodes[i + 1].Nodes[j])
                            {
                                LinearPropertyControl.Visible = true;
                                propertyGrid.Visible = false;
                                LinearProperty LinearProperty;
                                switch (j)
                                {
                                    case 0:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].Life;
                                        break;
                                    case 1:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].Quantity;
                                        break;
                                    case 2:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].Width;
                                        break;
                                    case 3:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].Height;
                                        break;
                                    case 4:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].Velocity;
                                        break;
                                    case 5:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].Weight;
                                        break;
                                    case 6:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].Spin;
                                        break;
                                    case 7:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].MotionRandom;
                                        break;
                                    case 8:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].Opacity;
                                        break;
                                    case 9:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].EmissionAngle;
                                        break;
                                    case 10:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].EmissionRange;
                                        break;
                                    case 11:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].RedTint;
                                        break;
                                    case 12:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].GreenTint;
                                        break;
                                    case 13:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].BlueTint;
                                        break;
                                    default:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].Weight;
                                        break;
                                }
                                LinearPropertyControl.SelectedLinearProperty = LinearProperty;
                                return;
                            }
                        }

                        #endregion

                        #region sub menu variations

                        // check sub menus of particle types                        
                        for (int j = 0; j < 12; j++)
                        {
                            if (selectedNode == particleEffectProperties.Nodes[0].Nodes[0].Nodes[i + 1].Nodes[14].Nodes[j])
                            {
                                LinearPropertyControl.Visible = true;
                                propertyGrid.Visible = false;
                                LinearProperty LinearProperty;
                                switch (j)
                                {
                                    case 0:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].LifeVariation;
                                        break;
                                    case 1:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].QuantityVariation;
                                        break;
                                    case 2:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].WidthVariation;
                                        break;
                                    case 3:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].HeightVariation;
                                        break;
                                    case 4:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].VelocityVariation;
                                        break;
                                    case 5:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].WeightVariation;
                                        break;
                                    case 6:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].SpinVariation;
                                        break;
                                    case 7:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].MotionRandomVariation;
                                        break;
                                    case 8:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].OpacityVariation;
                                        break;
                                    case 9:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].RedTintVariation;
                                        break;
                                    case 10:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].GreenTintVariation;
                                        break;
                                    case 11:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].BlueTintVariation;
                                        break;
                                    default:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].LifeVariation;
                                        break;
                                }
                                LinearPropertyControl.SelectedLinearProperty = LinearProperty;
                                return;
                            }
                        }
                        #endregion

                        #region sub menu over life

                        // check sub menus of particle types                        
                        for (int j = 0; j < 10; j++)
                        {
                            if (selectedNode == particleEffectProperties.Nodes[0].Nodes[0].Nodes[i + 1].Nodes[15].Nodes[j])
                            {
                                LinearPropertyControl.Visible = true;
                                propertyGrid.Visible = false;
                                LinearProperty LinearProperty;
                                switch (j)
                                {
                                    case 0:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].overLifeSettings.widthOverLife;
                                        break;
                                    case 1:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].overLifeSettings.heightOverLife;
                                        break;
                                    case 2:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].overLifeSettings.velocityOverLife;
                                        break;
                                    case 3:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].overLifeSettings.weightOverLife;
                                        break;
                                    case 4:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].overLifeSettings.spinOverLife;
                                        break;
                                    case 5:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].overLifeSettings.motionRandomOverLife;
                                        break;
                                    case 6:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].overLifeSettings.opacityOverLife;
                                        break;
                                    case 7:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].overLifeSettings.redTintOverLife;
                                        break;
                                    case 8:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].overLifeSettings.greenTintOverLife;
                                        break;
                                    case 9:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].overLifeSettings.blueTintOverLife;
                                        break;
                                    default:
                                        LinearProperty = particleEffectControl.ParticleEffect.Emitter.ParticleTypes[i].overLifeSettings.widthOverLife;
                                        break;
                                }
                                LinearPropertyControl.SelectedLinearProperty = LinearProperty;
                                return;
                            }
                        }
                        #endregion
                    }
                }
            }
            LinearPropertyControl.Visible = false;
            propertyGrid.Visible = false;
        }

        private void particleEffectProperties_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ProcessNodeSelection(e.Node);
        }

        private void toolStripButtonAddPartType_Click(object sender, EventArgs e)
        {
            particleEffectControl.AddNewParticleType();
            UpdateParticleEffectTree();
        }

        private void toolStripButtonDelPartType_Click(object sender, EventArgs e)
        {
            int index = -1;
            TreeNode selectedNode = particleEffectProperties.SelectedNode;
            // check particle types
            for (int i = 0; i < particleEffectControl.ParticleEffect.Emitter.ParticleTypes.Count; i++)
            {
                // check the root 
                if (selectedNode == particleEffectProperties.Nodes[0].Nodes[0].Nodes[i + 1])
                {
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {
                if (MilkshakeForm.ShowWarningQuestion("Do you really want to delete this particle type?") 
                    == true)
                {
                    particleEffectControl.RemoveParticleType(index);
                    UpdateParticleEffectTree();
                }
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name")
            {
                if (particleEffectProperties.SelectedNode == particleEffectProperties.Nodes[0])
                {
                    particleEffectProperties.Nodes[0].Text = e.ChangedItem.Value.ToString();
                }
                else
                {
                    for (int i = 0; i < particleEffectControl.ParticleEffect.Emitter.ParticleTypes.Count; i++)
                    {
                        TreeNode particleTypeNode = particleEffectProperties.Nodes[0].Nodes[0].Nodes[i + 1];
                        if (particleEffectProperties.SelectedNode == particleTypeNode)
                        {
                            particleTypeNode.Text = e.ChangedItem.Value.ToString();
                        }
                    }
                }
            }
            else if (e.ChangedItem.Label == "Life")
            {
                particleEffectControl.ParticleEffect.Play();
            }
        }        

        #region Shape

        private void toolStripButtonViewShape_Click(object sender, EventArgs e)
        {
            ShapeSelection shapeSelection = new ShapeSelection();
            EmitterShape currentShape = particleEffectControl.ParticleEffect.Emitter.Shape;
            shapeSelection.ShapeType = currentShape.Type;
            shapeSelection.ShapeOffset = currentShape.Offset;
            shapeSelection.ShapeSize = currentShape.Size;
            shapeSelection.TexturePath = lastTextureMaskPath;
            if (shapeSelection.ShowDialog() == DialogResult.OK)
            {
                EmitterShape shape = new EmitterShape(shapeSelection.ShapeType, shapeSelection.ShapeSize,
                    particleEffectControl.ParticleEffect.Position, shapeSelection.ShapeOffset);
                // if it's a mask, construct it
                if (shape.Type == EmitterShapeType.TextureMask)
                {
                    try
                    {
                        lastTextureMaskPath = shapeSelection.TexturePath;
                        Texture2D maskTexture = Texture2D.FromStream(DrawingManager.GraphicsDevice, System.IO.File.OpenRead(shapeSelection.TexturePath));
                        shape.CreateTextureMaskFromTexture2D(maskTexture, shapeSelection.UseFilled, shapeSelection.UseLeft,
                            shapeSelection.UseRight, shapeSelection.UseTop, shapeSelection.UseBottom);
                    }
                    catch
                    {
                        MessageBox.Show("Error: " + e.ToString(), "Error while loading texture", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);                        
                    }
                }
                shape.Initialize();
                particleEffectControl.ParticleEffect.Emitter.Shape = shape;
            }            
        }

        #endregion      

        #region Play buttons

        internal void toolStripButtonPlay_Click(object sender, EventArgs e)
        {
            particleEffectControl.ParticleEffect.Play();
            toolStripButtonPlay.Enabled = false;
            toolStripButtonPause.Enabled = true;
            toolStripButtonStop.Enabled = true;
        }

        internal void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            particleEffectControl.ParticleEffect.Stop();
            toolStripButtonPause.Enabled = false;
            toolStripButtonPlay.Enabled = true;
            toolStripButtonStop.Enabled = false;
        }

        internal void toolStripButtonPause_Click(object sender, EventArgs e)
        {
            particleEffectControl.ParticleEffect.Pause();
            toolStripButtonPause.Enabled = false;
            toolStripButtonPlay.Enabled = true;
            toolStripButtonStop.Enabled = true;
        }  

        #endregion      

        const int WM_MOUSEWHEEL = 0x20A;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_MOUSEWHEEL && particleEffectControl.Focused == true)
            {
                if ((int)m.WParam > 0)
                {
                    this.ZoomBox.ZoomIn();
                }
                else
                {
                    this.ZoomBox.ZoomOut();
                }
            }
        }

    }
}
