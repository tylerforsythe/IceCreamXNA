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
using Milkshake;
using MilkshakeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Milkshake.SelectorDialogs;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using XnaColor = Microsoft.Xna.Framework.Graphics.Color;
using Color = System.Drawing.Color;
using IceCream.SceneItems;

namespace Milkshake.Editors.Sprites
{
    public partial class SpriteEditor : SceneItemEditor
    {
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
                Sprite = base.SceneItem as Sprite;
            }
        }

        private Sprite Sprite
        {
            get { return spriteEditorControl.Sprite; }
            set { spriteEditorControl.Sprite = value; }
        }

        public SpriteEditor()
        {
            InitializeComponent();
        }

        #region Texture Selection

        private void buttonSelectTexture_Click(object sender, EventArgs e)
        {
            MaterialSelectorDialog materialSelectorDialog = new MaterialSelectorDialog();
            materialSelectorDialog.SelectedMaterial = spriteEditorControl.Sprite.Material;
            materialSelectorDialog.ShowLocalTextures = ItemIsLocal;
            if (materialSelectorDialog.ShowDialog() == DialogResult.OK
                && spriteEditorControl.Sprite.Material != materialSelectorDialog.SelectedMaterial)
            {
                this.Sprite.Material = materialSelectorDialog.SelectedMaterial;
                this.Sprite.SourceRectangle = null;
                this.Sprite.MaterialArea = "";
                LoadMaterialAreas();
                spriteEditorControl.SelectionMode = SpriteEditorSelectionMode.Normal;
                UpdateSourceRectangleControls();
                labelTextureName.Text = materialSelectorDialog.SelectedMaterial.ToString();
            }
        }

        #endregion

        private void SpriteEditor_Load(object sender, EventArgs e)
        {
            spriteEditorControl.ParentEditor = this;
            ZoomBox = new ZoomBox();
            ZoomBox.SetToolStripButtomZoomIn(toolStripButtonZoomIn);
            ZoomBox.SetToolStripButtomZoomOut(toolStripButtonZoomOut);
            ZoomBox.SetToolStripButtomZoomNormal(toolStripButtonZoomNormal);
            if (spriteEditorControl.Sprite.SourceRectangle != null)
            {
                toolStripButtonUseFullTexture.Enabled = true;
            }
            if (this.Sprite.Material != null)
            {
                labelTextureName.Text = spriteEditorControl.Sprite.Material.ToString();
            }
            LoadMaterialAreas();
            // hack around designer bug that resize the control
            labelTextureName.Size = new Size(labelTextureName.Size.Width, 22);
            // load all the Blending Type values in the combo box
            for (int i = 0; i < (int)DrawingBlendingType.EnumSize; i++)
            {
                comboBoxBlendingType.Items.Add(((DrawingBlendingType)i).ToString());
            }
            comboBoxBlendingType.SelectedIndex = (int)spriteEditorControl.Sprite.BlendingType;
            pictureBoxTint.BackColor = MilkshakeForm.GetGDIColor(spriteEditorControl.Sprite.Tint);
            _ignoreNumericUpDownEvent = true;
            numericUpDownTintRed.Value = spriteEditorControl.Sprite.Tint.R;
            numericUpDownTintGreen.Value = spriteEditorControl.Sprite.Tint.G;
            numericUpDownTintBlue.Value = spriteEditorControl.Sprite.Tint.B;
            textBoxColorHTML.Text = GetHTMLFromColor(Sprite.Tint);
            _ignoreNumericUpDownEvent = false;            
            _ignoreNumericUpDownEvent = true;
            _ignoreNumericUpDownEvent = false;
            UpdateSourceRectangleControls();
        }

        #region Area autodetection

        private void LoadMaterialAreas()
        {
            comboBoxArea.Enabled = (this.Sprite.Material.Areas.Keys.Count > 0);
            comboBoxArea.Items.Clear();
            comboBoxArea.Items.Add("<none>");
            foreach (String key in this.Sprite.Material.Areas.Keys)
            {
                comboBoxArea.Items.Add(key);
            }
            RefreshAreaCombo();
        }

        public void RefreshAreaCombo()
        {
            if (String.IsNullOrEmpty(this.Sprite.MaterialArea))
            {
                comboBoxArea.SelectedIndex = 0;
                groupBoxSourceRectangle.Enabled = true;
            }
            else
            {
                groupBoxSourceRectangle.Enabled = false;
                if (comboBoxArea.Items.Contains(this.Sprite.MaterialArea))
                {
                    comboBoxArea.SelectedItem = this.Sprite.MaterialArea;
                }
            }
            UpdateSourceRectangleControls();
        }

        public void UpdateNumericUpDownBoundaries()
        {
            numericUpDownRectX.Maximum = (decimal)spriteEditorControl.Sprite.Material.Texture.Width - 1;
            numericUpDownRectY.Maximum = (decimal)spriteEditorControl.Sprite.Material.Texture.Height - 1;
            numericUpDownRectW.Maximum = (decimal)spriteEditorControl.Sprite.Material.Texture.Width - numericUpDownRectX.Value;
            numericUpDownRectH.Maximum = (decimal)spriteEditorControl.Sprite.Material.Texture.Height - numericUpDownRectY.Value;
        }

        public void UpdateSourceRectangleControls()
        {
            _ignoreNumericUpDownEvent = true;
            toolStripButtonAutoDetect.Enabled = true;
            if (spriteEditorControl.Sprite.SourceRectangle != null)
            {
                toolStripButtonUseFullTexture.Enabled = true;
                numericUpDownRectX.Value = (decimal)spriteEditorControl.Sprite.SourceRectangle.Value.X;
                numericUpDownRectY.Value = (decimal)spriteEditorControl.Sprite.SourceRectangle.Value.Y;
                numericUpDownRectW.Maximum = (decimal)spriteEditorControl.Sprite.Material.Texture.Width - numericUpDownRectX.Value;
                numericUpDownRectH.Maximum = (decimal)spriteEditorControl.Sprite.Material.Texture.Height - numericUpDownRectY.Value;
                numericUpDownRectW.Value = (decimal)IceMath.Clamp(spriteEditorControl.Sprite.SourceRectangle.Value.Width,
                    (int)numericUpDownRectW.Minimum, (int)numericUpDownRectW.Maximum);
                numericUpDownRectH.Value = (decimal)IceMath.Clamp(spriteEditorControl.Sprite.SourceRectangle.Value.Height,
                    (int)numericUpDownRectH.Minimum, (int)numericUpDownRectH.Maximum);
            }
            else
            {
                toolStripButtonUseFullTexture.Enabled = false;
                numericUpDownRectX.Value = 0;
                numericUpDownRectY.Value = 0;
                if (spriteEditorControl.Sprite.Material != null)
                {
                    numericUpDownRectW.Maximum = (decimal)spriteEditorControl.Sprite.Material.Texture.Width;
                    numericUpDownRectH.Maximum = (decimal)spriteEditorControl.Sprite.Material.Texture.Height;
                    numericUpDownRectW.Value = (decimal)spriteEditorControl.Sprite.Material.Texture.Width;
                    numericUpDownRectH.Value = (decimal)spriteEditorControl.Sprite.Material.Texture.Height;
                }
            }
            _ignoreNumericUpDownEvent = false;
            toolStripButtonShowWholeImage.Visible = spriteEditorControl.Sprite.SourceRectangle.HasValue;

        }

        private void numericUpDownRectX_ValueChanged(object sender, EventArgs e)
        {
            AdjustRectangleFromUpDowns();
        }
        private void numericUpDownRectY_ValueChanged(object sender, EventArgs e)
        {
            AdjustRectangleFromUpDowns();
        }
        private void numericUpDownRectH_ValueChanged(object sender, EventArgs e)
        {
            AdjustRectangleFromUpDowns();
        }
        private void numericUpDownRectW_ValueChanged(object sender, EventArgs e)
        {
            AdjustRectangleFromUpDowns();
        }

        private void AdjustRectangleFromUpDowns()
        {
            if (_ignoreNumericUpDownEvent == false)
            {
                UpdateNumericUpDownBoundaries();
                if (numericUpDownRectX.Value == 0 && numericUpDownRectY.Value == 0
                    && numericUpDownRectW.Value == (decimal)spriteEditorControl.Sprite.Material.Texture.Width
                    && numericUpDownRectH.Value == (decimal)spriteEditorControl.Sprite.Material.Texture.Height)
                {
                    spriteEditorControl.Sprite.SourceRectangle = null;
                }
                else
                {

                    spriteEditorControl.Sprite.SourceRectangle = new Rectangle((int)numericUpDownRectX.Value,
                        (int)numericUpDownRectY.Value, (int)numericUpDownRectW.Value, (int)numericUpDownRectH.Value);
                    UpdateSourceRectangleControls();
                }
            }
        }


        private void toolStripButtonUseFullTexture_Click(object sender, EventArgs e)
        {
            this.Sprite.SourceRectangle = null;
            this.Sprite.MaterialArea = "";
            spriteEditorControl.SelectionMode = SpriteEditorSelectionMode.Normal;
            RefreshAreaCombo();
        }

        private void toolStripButtonAutoDetect_Click(object sender, EventArgs e)
        {
            spriteEditorControl.Sprite.SourceRectangle = null;
            // if we are not already selecting a tile
            if (spriteEditorControl.SelectionMode != SpriteEditorSelectionMode.SelectingTile)
            {
                spriteEditorControl.SpriteRectangles = this.Sprite.Material.Areas;
                if (spriteEditorControl.SpriteRectangles.Count >= 1)
                {
                    String firstKey = null;
                    foreach (String key in this.Sprite.Material.Areas.Keys)
                    {
                        firstKey = key;
                        break;
                    }
                    spriteEditorControl.SelectionMode = SpriteEditorSelectionMode.SelectingTile;
                    spriteEditorControl.SelectedRectangle = firstKey;
                    toolStripButtonUseFullTexture.Enabled = true;
                    toolStripButtonAutoDetect.Enabled = false;
                }
                else
                {
                    MilkshakeForm.ShowErrorMessage("This texture has no detected grid");
                }
            }
        }

        private void spriteEditorControl_MouseMove(object sender, MouseEventArgs e)
        {
            spriteEditorControl.CheckMousePosition(new Microsoft.Xna.Framework.Point(e.X, e.Y));
        }

        private void spriteEditorControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && spriteEditorControl.SelectionMode == SpriteEditorSelectionMode.SelectingTile)
            {
                spriteEditorControl.MouseClicked(new Microsoft.Xna.Framework.Point(e.X, e.Y));
                // if a tile was selected
                if (spriteEditorControl.SelectionMode == SpriteEditorSelectionMode.Tiled)
                {
                    UpdateSourceRectangleControls();
                }
            }
        }

        #endregion

        #region Events

        private void comboBoxBlendingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            spriteEditorControl.Sprite.BlendingType = (DrawingBlendingType)comboBoxBlendingType.SelectedIndex;
        }

        private void pictureBoxTint_Click(object sender, EventArgs e)
        {
            OpenTintSelectionDialog();
        }

        private void OpenTintSelectionDialog()
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.FullOpen = true;
            colorDialog.Color = MilkshakeForm.GetGDIColor(spriteEditorControl.Sprite.Tint);
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBoxTint.BackColor = colorDialog.Color;
                spriteEditorControl.Sprite.Tint = new XnaColor(colorDialog.Color.R,
                    colorDialog.Color.G, colorDialog.Color.B, spriteEditorControl.Sprite.Tint.A);
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
                spriteEditorControl.Sprite.Tint = new XnaColor(color.R,
                        color.G, color.B, spriteEditorControl.Sprite.Tint.A);
                textBoxColorHTML.Text = GetHTMLFromColor(Sprite.Tint);
            }
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
            textBoxColorHTML.Text = GetHTMLFromColor(Sprite.Tint);
        }

        private void textBoxColorHTML_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxColorHTML_Validated(sender, EventArgs.Empty);
            }
        }

        internal static String GetHTMLFromColor(XnaColor color)
        {
            String html = "#";
            html += color.R.ToString("X2", CultureInfo.InvariantCulture);
            html += color.G.ToString("X2", CultureInfo.InvariantCulture);
            html += color.B.ToString("X2", CultureInfo.InvariantCulture);
            return html;
        }

        private void textBoxColorHTML_Enter(object sender, EventArgs e)
        {
            textBoxColorHTML.SelectAll();
        }

        private void toolStripButtonShowWholeImage_Click(object sender, EventArgs e)
        {
            if (toolStripButtonShowWholeImage.Text == "Show Whole Image")
            {
                toolStripButtonShowWholeImage.Text = "Show Partial Image";
                spriteEditorControl.ShowWholeImage = true;
            }
            else
            {
                toolStripButtonShowWholeImage.Text = "Show Whole Image";
                spriteEditorControl.ShowWholeImage = false;
            }
        }

        private void textBoxColorHTML_Click(object sender, EventArgs e)
        {
            if (_ignoreHTMLBoxClick == false)
            {
                textBoxColorHTML.SelectAll();
                _ignoreHTMLBoxClick = true;
            }
        }

        private void comboBoxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxArea.SelectedIndex == 0)
            {
                if (String.IsNullOrEmpty(this.Sprite.MaterialArea) == false)
                {
                    this.Sprite.MaterialArea = "";
                    this.Sprite.SourceRectangle = null;
                }
            }
            else
            {
                this.Sprite.MaterialArea = comboBoxArea.SelectedItem.ToString();
            }
            RefreshAreaCombo();
        }

        #endregion
    }
}