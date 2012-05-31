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
using MilkshakeLibrary;
using Milkshake.SelectorDialogs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XPTable;
using XPTable.Models;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;
using XnaColor = Microsoft.Xna.Framework.Color;
using IceCream.SceneItems.TileGridClasses;

namespace Milkshake.Editors.TileGrids
{
    public partial class TileGridEditor : SceneItemEditor
    {
        public static TileGridEditor Instance;

        private bool _ignoreNumericUpDownEvent = false;

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
                TileGrid = base.SceneItem as TileGrid;
            }
        }

        private TileGrid TileGrid
        {
            get { return tileGridEditorControl.TileGrid; }
            set 
            { 
                tileGridEditorControl.TileGrid = value;
                tileGridMinimapControl.ParentGrid = value;
                tileSelectionControl.ParentGrid = value;
            }
        }

        private int _selectedBrushTile = 0;
        internal int SelectedBrushTile
        {
            get { return _selectedBrushTile; }
            set { _selectedBrushTile = value; }
        }

        internal int SelectedLayer
        {
            get
            {
                int layer = 0;
                if (tableLayers.SelectedIndicies.Length > 0)
                {
                    layer = TileGridEditor.Instance.tableLayers.SelectedIndicies[0];
                }
                return layer;
            }
        }        

        public TileGridEditor()
        {
            InitializeComponent();
            this.TileGrid = new TileGrid();
            TileGridEditor.Instance = this;
        }

        private void TileGridEditor_Load(object sender, EventArgs e)
        {
            tileGridEditorControl.ParentEditor = this;
            ZoomBox = new ZoomBox();
            ZoomBox.SetToolStripButtomZoomIn(toolStripButtonZoomIn);
            ZoomBox.SetToolStripButtomZoomOut(toolStripButtonZoomOut);
            ZoomBox.SetToolStripButtomZoomNormal(toolStripButtonZoomNormal);
            labelTextureName.Text = TileGrid.Material.ToString();
            // load all the Blending Type values in the combo box
            for (int i = 0; i < (int)DrawingBlendingType.EnumSize; i++)
            {
                comboBoxBlendingType.Items.Add(((DrawingBlendingType)i).ToString());
            }
            comboBoxBlendingType.SelectedIndex = (int)TileGrid.BlendingType;
            pictureBoxTint.BackColor = MilkshakeForm.GetGDIColor(TileGrid.Tint);
            _ignoreNumericUpDownEvent = true;
            numericUpDownTintRed.Value = TileGrid.Tint.R;
            numericUpDownTintGreen.Value = TileGrid.Tint.G;
            numericUpDownTintBlue.Value = TileGrid.Tint.B;
            _ignoreNumericUpDownEvent = false;
            trackBarOpacity.Value = (int)TileGrid.Tint.A;
            labelOpacity.Text = trackBarOpacity.Value.ToString();
            checkBoxUseSafeBorder.Checked = TileGrid.UseTilingSafeBorders;
            textBoxTileWidth.Text = TileGrid.TileSize.X.ToString(CultureInfo.InvariantCulture);
            textBoxTileHeight.Text = TileGrid.TileSize.Y.ToString(CultureInfo.InvariantCulture);            
            textBoxSizeCols.Text = TileGrid.TileCols.ToString(CultureInfo.InvariantCulture);
            textBoxSizeRows.Text = TileGrid.TileRows.ToString(CultureInfo.InvariantCulture);
            ApplyPaintModeSetting();
            AdjustTableLayersColumnSize();
            RefreshLayerTable();
        }     

        #region Toolbar

        private void toolStripButtonToggleGrid_Click(object sender, EventArgs e)
        {
            tileGridEditorControl.DrawGrid = toolStripButtonToggleGrid.Checked;
        }

        private void ApplyPaintModeSetting()
        {
            toolStripButtonPaintEdit.Checked = (tileGridEditorControl.PaintMode == TileGridPaintMode.Edit);
            toolStripButtonPaintBrush.Checked = (tileGridEditorControl.PaintMode == TileGridPaintMode.Brush);
            toolStripButtonPaintBucket.Checked = (tileGridEditorControl.PaintMode == TileGridPaintMode.Bucket);
            toolStripButtonPaintEraser.Checked = (tileGridEditorControl.PaintMode == TileGridPaintMode.Eraser);
            // remove all curently selected tiles
            tileGridEditorControl.ResetStatus();
            
            SetEnableStateEditIcons(false);
            if (tileGridEditorControl.PaintMode == TileGridPaintMode.Edit)
            {
                SetEnableStatePasteIcon(tileGridEditorControl.IsClipboardNotEmpty());
            }
            else
            {
                SetEnableStatePasteIcon(false);
            }
        }

        private void toolStripButtonPaintBrush_Click(object sender, EventArgs e)
        {
            tileGridEditorControl.PaintMode = TileGridPaintMode.Brush;
            ApplyPaintModeSetting();
        }

        private void toolStripButtonPaintBucket_Click(object sender, EventArgs e)
        {
            tileGridEditorControl.PaintMode = TileGridPaintMode.Bucket;
            ApplyPaintModeSetting();
        }

        private void toolStripButtonPaintEraser_Click(object sender, EventArgs e)
        {
            tileGridEditorControl.PaintMode = TileGridPaintMode.Eraser;
            ApplyPaintModeSetting();
        }

        private void toolStripButtonPaintEdit_Click(object sender, EventArgs e)
        {
            tileGridEditorControl.PaintMode = TileGridPaintMode.Edit;
            ApplyPaintModeSetting();
        }       

        private void toolStripButtonRotateLeftTile_Click(object sender, EventArgs e)
        {
            foreach (Point tileCoord in tileGridEditorControl.SelectedTiles)
            {
                int layer = SelectedLayer;
                byte currentRot = TileGrid.TileLayers[layer].Tiles[tileCoord.X][tileCoord.Y].Rotation;
                if (currentRot >= 3)
                {
                    currentRot = 0;
                }
                else
                {
                    currentRot++;
                }
                TileGrid.TileLayers[layer].Tiles[tileCoord.X][tileCoord.Y].Rotation = currentRot; 
            }
        }

        private void toolStripButtonRotateRightTile_Click(object sender, EventArgs e)
        {
            foreach (Point tileCoord in tileGridEditorControl.SelectedTiles)
            {
                int layer = SelectedLayer;
                byte currentRot = TileGrid.TileLayers[layer].Tiles[tileCoord.X][tileCoord.Y].Rotation;
                if (currentRot <= 0)
                {
                    currentRot = 3;
                }
                else
                {
                    currentRot--;
                }
                TileGrid.TileLayers[layer].Tiles[tileCoord.X][tileCoord.Y].Rotation = currentRot;
            }
        }

        private void toolStripButtonHFlipTile_Click(object sender, EventArgs e)
        {
            foreach (Point tileCoord in tileGridEditorControl.SelectedTiles)
            {
                int layer = SelectedLayer;               
                TileGrid.TileLayers[layer].Tiles[tileCoord.X][tileCoord.Y].HFlip 
                    = toolStripButtonHFlipTile.Checked;
            }
        }

        private void toolStripButtonVFlipTile_Click(object sender, EventArgs e)
        {
            foreach (Point tileCoord in tileGridEditorControl.SelectedTiles)
            {
                int layer = SelectedLayer;
                TileGrid.TileLayers[layer].Tiles[tileCoord.X][tileCoord.Y].VFlip
                    = toolStripButtonVFlipTile.Checked;
            }
        }

        public void SetEnableStateEditIcons(bool isEnabled)
        {
            toolStripButtonHFlipTile.Enabled = isEnabled;
            toolStripButtonRotateLeftTile.Enabled = isEnabled;
            toolStripButtonRotateRightTile.Enabled = isEnabled;
            toolStripButtonVFlipTile.Enabled = isEnabled;
            toolStripButtonCut.Enabled = isEnabled;
            toolStripButtonCopy.Enabled = isEnabled;            
            toolStripButtonDelete.Enabled = isEnabled;            
        }

        public void SetEnableStatePasteIcon(bool isEnabled)
        {
            toolStripButtonPaste.Enabled = isEnabled;
        }        

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            tileGridEditorControl.ClearSelectedTiles();
        }

        private void toolStripButtonCut_Click(object sender, EventArgs e)
        {
            tileGridEditorControl.CutSelectedTiles();
        }

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            tileGridEditorControl.CopySelectedTiles();
        }

        private void toolStripButtonPaste_Click(object sender, EventArgs e)
        {
            tileGridEditorControl.StartPastingTiles();
        }

        #endregion        

        #region Layers

        private void AdjustTableLayersColumnSize()
        {
            columnModelLayers.Columns[1].Width = tableLayers.Width - columnModelLayers.Columns[0].Width - 6;
        }

        private void tableLayers_Resize(object sender, EventArgs e)
        {
            AdjustTableLayersColumnSize();
        }

        private void RefreshLayerTable()
        {
            tableLayers.TableModel.Rows.Clear();
            foreach (TileLayer layer in TileGrid.TileLayers)
            {
                Row newRow = new Row();
                tableLayers.TableModel.Rows.Add(newRow);
                newRow.Cells.Add(new Cell("", layer.Visible));
                newRow.Cells.Add(new Cell(layer.Name));
            }
            if (TileGrid.TileLayers.Count == 0)
            {
                toolStripButtonLayerDelete.Enabled = false;
                toolStripButtonLayerMoveUp.Enabled = false;
                toolStripButtonLayerMoveDown.Enabled = false;
                toolStripButtonLayerOpacity.Enabled = false;
            }
        }

        private void RefreshLayerToolbarStatus()
        {
            if (tableLayers.SelectedIndicies.Length > 0)
            {
                toolStripButtonLayerDelete.Enabled = true;
                toolStripButtonLayerMoveUp.Enabled = (tableLayers.SelectedIndicies[0] > 0);
                toolStripButtonLayerMoveDown.Enabled = (tableLayers.SelectedIndicies[0] < TileGrid.TileLayers.Count - 1);
                toolStripButtonLayerOpacity.Enabled = true;
            }
            else
            {
                toolStripButtonLayerDelete.Enabled = false;
                toolStripButtonLayerMoveUp.Enabled = false;
                toolStripButtonLayerMoveDown.Enabled = false;
                toolStripButtonLayerOpacity.Enabled = false;
            }
        }

        private void tableLayers_SelectionChanged(object sender, XPTable.Events.SelectionEventArgs e)
        {
            RefreshLayerToolbarStatus();
        }

        private void tableLayers_CellCheckChanged(object sender, XPTable.Events.CellCheckBoxEventArgs e)
        {
            TileGrid.TileLayers[e.Row].Visible = e.Cell.Checked;
        }

        private void tableLayers_EditingStopped(object sender, XPTable.Events.CellEditEventArgs e)
        {            
            if (String.IsNullOrEmpty(e.Cell.Text))
            {
                e.Cancel = true;
            }      
        }

        private void toolStripButtonLayerAdd_Click(object sender, EventArgs e)
        {
            TileLayer newLayer = new TileLayer(TileGrid.TileCols, TileGrid.TileRows);
            newLayer.Name = "New Layer";
            newLayer.Visible = true;
            newLayer.Opacity = 255;
            newLayer.Parent = this.TileGrid;
            TileGrid.TileLayers.Insert(0, newLayer);
            RefreshLayerTable();
        }

        private void toolStripButtonLayerDelete_Click(object sender, EventArgs e)
        {
            if (tableLayers.SelectedIndicies.Length > 0)
            {
                TileGrid.TileLayers.RemoveAt(tableLayers.SelectedIndicies[0]);
                RefreshLayerTable();
            }
        }

        private void toolStripButtonLayerMoveUp_Click(object sender, EventArgs e)
        {
            if (tableLayers.SelectedIndicies.Length > 0
                && tableLayers.SelectedIndicies[0] > 0)
            {
                TileLayer selectedLayer = TileGrid.TileLayers[tableLayers.SelectedIndicies[0]];
                TileGrid.TileLayers.Remove(selectedLayer);
                int newRow = tableLayers.SelectedIndicies[0] - 1;
                TileGrid.TileLayers.Insert(newRow, selectedLayer);               
                RefreshLayerTable();
                tableLayers.SelectedIndicies[0] = newRow;
            }
        }

        private void toolStripButtonLayerMoveDown_Click(object sender, EventArgs e)
        {
            if (tableLayers.SelectedIndicies.Length > 0 
                && tableLayers.SelectedIndicies[0] < TileGrid.TileLayers.Count - 1)
            {
                TileLayer selectedLayer = TileGrid.TileLayers[tableLayers.SelectedIndicies[0]];
                TileGrid.TileLayers.Remove(selectedLayer);
                int newRow = tableLayers.SelectedIndicies[0] + 1;
                TileGrid.TileLayers.Insert(newRow, selectedLayer);              
                RefreshLayerTable();
                tableLayers.SelectedIndicies[0] = newRow;
            }
        }

        private void toolStripButtonLayerOpacity_Click(object sender, EventArgs e)
        {
            if (tableLayers.SelectedIndicies.Length > 0)
            {
                
            }
        }

        private void toolStripButtonLayerClear_Click(object sender, EventArgs e)
        {
            if (MilkshakeForm.ShowWarningQuestion("Do you really want to clear the layer?"))
            {
                int layer = SelectedLayer;
                for (int y = 0; y < TileGrid.TileRows; y++)
                {
                    for (int x = 0; x < TileGrid.TileCols; x++)
                    {
                        TileGrid.TileLayers[layer].Tiles[x][y].Index = -1;
                        TileGrid.TileLayers[layer].Tiles[x][y].Rotation = 0;
                        TileGrid.TileLayers[layer].Tiles[x][y].HFlip = false;
                        TileGrid.TileLayers[layer].Tiles[x][y].VFlip = false;
                    }
                }
            }
        }          

        #endregion        

        #region Properties tab event

        private void buttonSelectTexture_Click(object sender, EventArgs e)
        {
            MaterialSelectorDialog materialSelectorDialog = new MaterialSelectorDialog();
            materialSelectorDialog.SelectedMaterial = TileGrid.Material;
            materialSelectorDialog.ShowLocalTextures = ItemIsLocal;
            if (materialSelectorDialog.ShowDialog() == DialogResult.OK)
            {
                TileGrid.Material = materialSelectorDialog.SelectedMaterial;                                
                labelTextureName.Text = materialSelectorDialog.SelectedMaterial.ToString();
            }
        }

        private void comboBoxBlendingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            TileGrid.BlendingType = (DrawingBlendingType)comboBoxBlendingType.SelectedIndex;
        }

        private void OpenTintSelectionDialog()
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.FullOpen = true;
            colorDialog.Color = MilkshakeForm.GetGDIColor(TileGrid.Tint);
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBoxTint.BackColor = colorDialog.Color;
                TileGrid.Tint = new Microsoft.Xna.Framework.Color(colorDialog.Color.R,
                    colorDialog.Color.G, colorDialog.Color.B, TileGrid.Tint.A);
                _ignoreNumericUpDownEvent = true;
                numericUpDownTintRed.Value = (decimal)colorDialog.Color.R;
                numericUpDownTintGreen.Value = (decimal)colorDialog.Color.G;
                numericUpDownTintBlue.Value = (decimal)colorDialog.Color.B;
                _ignoreNumericUpDownEvent = false;
            }
        }

        private void pictureBoxTint_Click(object sender, EventArgs e)
        {
            OpenTintSelectionDialog();
        }

        private void ApplyUpDownColors()
        {
            if (_ignoreNumericUpDownEvent == false)
            {
                System.Drawing.Color color = System.Drawing.Color.FromArgb((int)numericUpDownTintRed.Value,
                    (int)numericUpDownTintGreen.Value, (int)numericUpDownTintBlue.Value);
                pictureBoxTint.BackColor = color;
                TileGrid.Tint = new Microsoft.Xna.Framework.Color(color.R,
                        color.G, color.B, TileGrid.Tint.A);
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

        private void trackBarOpacity_Scroll(object sender, EventArgs e)
        {
            labelOpacity.Text = trackBarOpacity.Value.ToString();
            XnaColor newColor = TileGrid.Tint;
            newColor.A = (byte)trackBarOpacity.Value;
            TileGrid.Tint = newColor;
        }

        private void checkBoxUseSafeBorder_CheckedChanged(object sender, EventArgs e)
        {
            TileGrid.UseTilingSafeBorders = checkBoxUseSafeBorder.Checked;
        }

        private void textBoxTileWidth_Validated(object sender, EventArgs e)
        {
            try
            {
                int newW = int.Parse(textBoxTileWidth.Text, System.Globalization.CultureInfo.InvariantCulture);
                TileGrid.TileSize = new Point(newW, TileGrid.TileSize.Y);
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    textBoxTileWidth.Text = TileGrid.TileSize.X.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void textBoxTileHeight_Validated(object sender, EventArgs e)
        {
            try
            {
                int newH = int.Parse(textBoxTileHeight.Text, System.Globalization.CultureInfo.InvariantCulture);
                TileGrid.TileSize = new Point(TileGrid.TileSize.X, newH);
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    textBoxTileHeight.Text = TileGrid.TileSize.Y.ToString(CultureInfo.InvariantCulture);
                }
            }
        }      

        private void textBoxSizeCols_Validated(object sender, EventArgs e)
        {
            try
            {
                int newW = int.Parse(textBoxSizeCols.Text, System.Globalization.CultureInfo.InvariantCulture);
                if (newW != TileGrid.TileCols && newW > 0)
                {
                    TileGrid.TileCols = newW;
                }
                else
                {
                    throw new Exception("");
                }
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    textBoxSizeCols.Text = TileGrid.TileCols.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void textBoxSizeRows_Validated(object sender, EventArgs e)
        {
            try
            {
                int newH = int.Parse(textBoxSizeRows.Text, System.Globalization.CultureInfo.InvariantCulture);
                if (newH != TileGrid.TileRows && newH > 0)
                {
                    TileGrid.TileRows = newH;
                }
                else
                {
                    throw new Exception("");
                }
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    textBoxSizeRows.Text = TileGrid.TileRows.ToString(CultureInfo.InvariantCulture);
                }
            }
        }  
        #endregion                      

        private void buttonSpriteSheet_Click(object sender, EventArgs e)
        {
            TileSheetSelectorDialog materialSelectorDialog = new TileSheetSelectorDialog();
            materialSelectorDialog.TileSheet = TileGrid.TileSheet;
            materialSelectorDialog.ShowLocalTextures = ItemIsLocal;
            if (materialSelectorDialog.ShowDialog() == DialogResult.OK)
            {
                this.TileGrid.TileSheet = materialSelectorDialog.TileSheet;
                labelSpriteSheetName.Text = materialSelectorDialog.TileSheet.ToString();
            }
        }
    }
}