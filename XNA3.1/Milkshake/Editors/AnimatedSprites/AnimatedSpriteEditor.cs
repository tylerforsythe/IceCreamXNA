using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using IceCream;
using IceCream.Drawing;
using IceCream.SceneItems.AnimationClasses;
using Milkshake;
using Milkshake.Editors.Sprites;
using MilkshakeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Milkshake.SelectorDialogs;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using XnaColor = Microsoft.Xna.Framework.Graphics.Color;
using Color = System.Drawing.Color;
using IceCream.SceneItems;
using XPTable.Models;
using XPTable.Editors;
using XPTable;

namespace Milkshake.Editors.AnimatedSprites
{
    public partial class AnimatedSpriteEditor : SceneItemEditor
    {
        #region Fields and Properties

        private bool _ignoreNumericUpDownEvent = false;
        private bool _ignoreHTMLBoxClick = false;

        public ZoomBox ZoomBox
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
                this.AnimatedSprite = base.SceneItem as AnimatedSprite;
            }
        }

        internal AnimatedSprite AnimatedSprite
        {
            get;
            set;
        }

        internal AnimationInfo SelectedAnimationInfo
        {
            get
            {
                if (listViewAnimations.SelectedIndices.Count > 0 
                    && this.AnimatedSprite.Animations.Count > 0)
                {
                    return this.AnimatedSprite.Animations[listViewAnimations.SelectedIndices[0]];
                }
                else
                {
                    return null;
                }
            }
        }

        internal int SelectedAnimationFrameIndex
        {
            get
            {
                if (this.SelectedAnimationInfo != null && this.SelectedAnimationInfo.AnimationFrames.Count > 0
                    && tableFrames.SelectedIndicies.Length > 0)
                {
                    return tableFrames.SelectedIndicies[0];                   
                }
                else
                {
                    return -1;
                }
            }
        }

        #endregion

        #region Constructor

        public AnimatedSpriteEditor()
        {
            InitializeComponent();
        }

        private void AnimatedSpriteEditor_Load(object sender, EventArgs e)
        {
            this.ZoomBox = new ZoomBox();
            this.ZoomBox.SetToolStripButtomZoomIn(toolStripButtonZoomIn);
            this.ZoomBox.SetToolStripButtomZoomOut(toolStripButtonZoomOut);
            this.ZoomBox.SetToolStripButtomZoomNormal(toolStripButtonZoomNormal);
            this.ZoomBox.Camera.Pivot = new Vector2(0.5f);
            this.ZoomBox.Camera.IsPivotRelative = true;
            if (this.AnimatedSprite.Material != null)
            {
                labelTextureName.Text = this.AnimatedSprite.Material.ToString();
            }
            // hack around designer bug that resize the control
            labelTextureName.Size = new Size(labelTextureName.Size.Width, 22);
            // load all the Blending Type values in the combo box
            for (int i = 0; i < (int)DrawingBlendingType.EnumSize; i++)
            {
                comboBoxBlendingType.Items.Add(((DrawingBlendingType)i).ToString());
            }
            comboBoxBlendingType.SelectedIndex = (int)this.AnimatedSprite.BlendingType;
            pictureBoxTint.BackColor = MilkshakeForm.GetGDIColor(this.AnimatedSprite.Tint);
            _ignoreNumericUpDownEvent = true;
            numericUpDownTintRed.Value = this.AnimatedSprite.Tint.R;
            numericUpDownTintGreen.Value = this.AnimatedSprite.Tint.G;
            numericUpDownTintBlue.Value = this.AnimatedSprite.Tint.B;
            textBoxColorHTML.Text = SpriteEditor.GetHTMLFromColor(this.AnimatedSprite.Tint);
            _ignoreNumericUpDownEvent = false;
            sceneItemPreviewControl.SceneItem = this.AnimatedSprite;
            sceneItemPreviewControl.Camera = this.ZoomBox.Camera;
            RefreshAreaComboList();
            LoadAnimationsList();
        }

        #endregion        

        #region Methods

        private void OpenTintSelectionDialog()
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.FullOpen = true;
            colorDialog.Color = MilkshakeForm.GetGDIColor(this.AnimatedSprite.Tint);
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBoxTint.BackColor = colorDialog.Color;
                this.AnimatedSprite.Tint = new XnaColor(colorDialog.Color.R,
                    colorDialog.Color.G, colorDialog.Color.B, this.AnimatedSprite.Tint.A);
                _ignoreNumericUpDownEvent = true;
                numericUpDownTintRed.Value = (decimal)colorDialog.Color.R;
                numericUpDownTintGreen.Value = (decimal)colorDialog.Color.G;
                numericUpDownTintBlue.Value = (decimal)colorDialog.Color.B;
                _ignoreNumericUpDownEvent = false;
                ApplyUpDownColors();
            }
        }

        private void ApplyUpDownColors()
        {
            if (_ignoreNumericUpDownEvent == false)
            {
                Color color = Color.FromArgb((int)numericUpDownTintRed.Value,
                    (int)numericUpDownTintGreen.Value, (int)numericUpDownTintBlue.Value);
                pictureBoxTint.BackColor = color;
                this.AnimatedSprite.Tint = new XnaColor(color.R,
                        color.G, color.B, this.AnimatedSprite.Tint.A);
                textBoxColorHTML.Text = SpriteEditor.GetHTMLFromColor(this.AnimatedSprite.Tint);
            }
        }

        private void LoadAnimationsList()
        {
            for (int i = 0; i < this.AnimatedSprite.Animations.Count; i++)
            {                
                listViewAnimations.Items.Add(this.AnimatedSprite.Animations[i].Name);
            }
            listViewAnimations.SelectedIndices.Add(this.AnimatedSprite.CurrentAnimationID);
        }

        private void LoadFrameList()
        {
            tableFrames.TableModel.Rows.Clear();
            AnimationInfo selectedAnim = this.SelectedAnimationInfo;
            if (selectedAnim != null)
            {
                for (int i = 0; i < selectedAnim.AnimationFrames.Count; i++)
                {
                    AnimationFrame frame = selectedAnim.AnimationFrames[i];
                    Cell[] cells = new Cell[2];
                    cells[0] = new Cell(frame.Area);                    
                    cells[1] = new Cell((object)frame.Duration);
                    Row newRow = new Row(cells);
                    tableFrames.TableModel.Rows.Add(newRow);
                }
                tableFrames.TableModel.Selections.AddCell(0, 1);
            }
        }

        private bool IsAnimationNameUnique(String name)
        {
            foreach (AnimationInfo anim in this.AnimatedSprite.Animations)
            {
                if (anim.Name == name)
                {
                    return false;
                }
            }
            return true;
        }

        private String GetNewAnimationName(String baseName)
        {
            int i = 1;
            while (true)
            {
                String formattedNumber = i.ToString("00");
                String compName = baseName + formattedNumber;
                if (IsAnimationNameUnique(compName))
                {
                    return compName;
                }
                i++;
            }
        }

        private String[] GetAreasList()
        {
            List<String> returnList = new List<String>();
            foreach (String area in this.AnimatedSprite.Material.Areas.Keys)
            {
                returnList.Add(area);
            }
            return returnList.ToArray();
        }

        private void RefreshAreaComboList()
        {
            ComboBoxCellEditor editor = new ComboBoxCellEditor();
            editor.DropDownStyle = DropDownStyle.DropDownList;
            editor.Items.AddRange(GetAreasList());
            columnModelFrames.Columns[0].Editor = editor;
        }

        #endregion

        #region Events

        private void buttonSelectTexture_Click(object sender, EventArgs e)
        {
            MaterialSelectorDialog materialSelectorDialog = new MaterialSelectorDialog();
            materialSelectorDialog.SelectedMaterial = this.AnimatedSprite.Material;
            materialSelectorDialog.ShowLocalTextures = ItemIsLocal;
            if (materialSelectorDialog.ShowDialog() == DialogResult.OK
                && this.AnimatedSprite.Material != materialSelectorDialog.SelectedMaterial)
            {
                if (materialSelectorDialog.SelectedMaterial.Areas.Keys.Count == 0)
                {
                    MilkshakeForm.ShowErrorMessage("The selected Material \"" + 
                        materialSelectorDialog.SelectedMaterial.ToString() + "\" does contain any defined Areas.\n" +
                        "To use the animations you need to load an area definition file with this Material");
                }
                else
                {
                    this.AnimatedSprite.Material = materialSelectorDialog.SelectedMaterial;
                    this.AnimatedSprite.SourceRectangle = null;
                    this.AnimatedSprite.MaterialArea = "";
                    labelTextureName.Text = materialSelectorDialog.SelectedMaterial.ToString();
                    RefreshAreaComboList();
                }              
            }
        }

        private void comboBoxBlendingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.AnimatedSprite.BlendingType = (DrawingBlendingType)comboBoxBlendingType.SelectedIndex;
        }

        private void pictureBoxTint_Click(object sender, EventArgs e)
        {
            OpenTintSelectionDialog();
        }

        private void numericUpDownTintRed_ValueChanged(object sender, EventArgs e)
        {
            ApplyUpDownColors();
        }

        private void numericUpDownTintGreen_ValueChanged(object sender, EventArgs e)
        {
            ApplyUpDownColors();
        }

        private void numericUpDownTintBlue_ValueChanged(object sender, EventArgs e)
        {
            ApplyUpDownColors();
        }

        private void textBoxColorHTML_Validated(object sender, EventArgs e)
        {
            _ignoreHTMLBoxClick = false;
            String text = textBoxColorHTML.Text.Trim().ToUpperInvariant();
            if (text.Length == 7 && text.Substring(0, 1) == "#")
            {
                // ignore the leading char '#'
                text = text.Substring(1);
            }
            if (text.Length == 6)
            {
                try
                {
                    decimal red = Int32.Parse(text.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                    decimal green = Int32.Parse(text.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                    decimal blue = Int32.Parse(text.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                    _ignoreNumericUpDownEvent = true;
                    numericUpDownTintRed.Value = red;
                    numericUpDownTintGreen.Value = green;
                    numericUpDownTintBlue.Value = blue;
                    _ignoreNumericUpDownEvent = false;
                    ApplyUpDownColors();
                }
                catch
                {

                }
            }
            textBoxColorHTML.Text = SpriteEditor.GetHTMLFromColor(this.AnimatedSprite.Tint);
        }

        private void textBoxColorHTML_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxColorHTML_Validated(sender, EventArgs.Empty);
            }
        }

        private void textBoxColorHTML_Enter(object sender, EventArgs e)
        {
            textBoxColorHTML.SelectAll();
        }

        private void textBoxColorHTML_Click(object sender, EventArgs e)
        {
            if (_ignoreHTMLBoxClick == false)
            {
                textBoxColorHTML.SelectAll();
                _ignoreHTMLBoxClick = true;
            }
        }

        private void listViewAnimations_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enabledState = this.SelectedAnimationInfo != null;
            groupBoxKeyFrames.Enabled = enabledState;
            toolStripButtonAddKeyFrame.Enabled = enabledState;
            toolStripButtonDeleteKeyFrame.Enabled = false;
            toolStripButtonDelAnimation.Enabled = enabledState;
            toolStripButtonCopyAnimation.Enabled = enabledState;
            if (enabledState == true)
            {
                this.AnimatedSprite.PlayAnimation(this.SelectedAnimationInfo.Name);
                LoadFrameList();
            }
            else
            {
                tableFrames.TableModel.Rows.Clear();
            }
        }

        private void listViewAnimations_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null)
            {
                return;
            }
            String oldName = listViewAnimations.Items[e.Item].Text;
            String newName = e.Label.Trim();
            if (oldName != newName)
            {
                if (String.IsNullOrEmpty(newName) == false && IsAnimationNameUnique(newName) == true)
                {
                    this.AnimatedSprite.Animations[e.Item].Name = newName;
                }
                else
                {
                    e.CancelEdit = true;
                }
            }
        }

        private void tableFrames_CellPropertyChanged(object sender, XPTable.Events.CellEventArgs e)
        {
            AnimationFrame newFrame = new AnimationFrame();
            // Duration
            if (e.Cell.Data != null)
            {
                newFrame.Duration = Int32.Parse(e.Cell.Data.ToString());
                newFrame.Area = this.SelectedAnimationInfo.AnimationFrames[this.SelectedAnimationFrameIndex].Area;
                SelectedAnimationInfo.Reset();
            }
            else
            {
                newFrame.Area = e.Cell.Text;
                newFrame.Duration =
                    this.SelectedAnimationInfo.AnimationFrames[this.SelectedAnimationFrameIndex].Duration;
            }
            this.SelectedAnimationInfo.AnimationFrames[this.SelectedAnimationFrameIndex] = newFrame;
        }

        private void toolStripButtonAddKeyFrame_Click(object sender, EventArgs e)
        {
            if (this.SelectedAnimationInfo != null)
            {
                AnimationFrame newFrame;
                if (this.SelectedAnimationFrameIndex > -1)
                {
                    newFrame = this.SelectedAnimationInfo.AnimationFrames[this.SelectedAnimationFrameIndex];
                }
                else
                {
                    String[] areas = GetAreasList();
                    String areaName = String.Empty;
                    if (areas.Length > 0)
                    {
                        areaName = areas[0];
                    }
                    newFrame = new AnimationFrame(10, areaName);
                }
                this.SelectedAnimationInfo.AnimationFrames.Add(newFrame);
                LoadFrameList();
            }
        }

        private void toolStripButtonDeleteKeyFrame_Click(object sender, EventArgs e)
        {
            if (this.SelectedAnimationFrameIndex > -1)
            {
                this.SelectedAnimationInfo.AnimationFrames.RemoveAt(this.SelectedAnimationFrameIndex);
                LoadFrameList();
            }
        }

        private void toolStripButtonAddAnimation_Click(object sender, EventArgs e)
        {
            AnimationInfo newAnim = new AnimationInfo(GetNewAnimationName("New Animation "));
            this.AnimatedSprite.AddAnimation(newAnim);
            listViewAnimations.Items.Add(newAnim.Name);
            listViewAnimations.SelectedIndices.Clear();
            listViewAnimations.SelectedIndices.Add(this.AnimatedSprite.Animations.Count - 1);             
        }


        private void toolStripButtonCopyAnimation_Click(object sender, EventArgs e)
        {
            int selCnt = listViewAnimations.SelectedIndices.Count;
            int[] selIndices = new int[selCnt];
            listViewAnimations.SelectedIndices.CopyTo(selIndices, 0);
            for (int i = 0; i < selCnt; i++)
            {
                AnimationInfo selAnim = this.AnimatedSprite.Animations[selIndices[i]];
                string newAnimName = GetNewAnimationName(selAnim.Name + "_");
                AnimationInfo newAnim = new AnimationInfo(newAnimName);
                selAnim.CopyValuesTo(newAnim, this.AnimatedSprite);
                newAnim.Name = newAnimName;
                this.AnimatedSprite.AddAnimation(newAnim);
                listViewAnimations.Items.Add(newAnim.Name);
                if (i == 0)
                {
                    listViewAnimations.SelectedIndices.Clear();
                }
                listViewAnimations.SelectedIndices.Add(this.AnimatedSprite.Animations.Count - 1); 
            }   
        }

        private void toolStripButtonDelAnimation_Click(object sender, EventArgs e)
        {
            int selCnt = listViewAnimations.SelectedIndices.Count;
            for (int i = 0; i < selCnt; i++)
            {
                int animIndex = listViewAnimations.SelectedIndices[0];
                listViewAnimations.Items.RemoveAt(animIndex);
                this.AnimatedSprite.Animations.RemoveAt(animIndex);
            }          
        }

        #endregion        

        private void tableFrames_SelectionChanged(object sender, XPTable.Events.SelectionEventArgs e)
        {
            bool enabledState = (this.SelectedAnimationFrameIndex > -1);
            toolStripButtonDeleteKeyFrame.Enabled = enabledState;
        }
    }
}
