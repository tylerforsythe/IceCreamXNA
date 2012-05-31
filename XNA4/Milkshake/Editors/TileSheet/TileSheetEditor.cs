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

namespace Milkshake.Editors.TileSheetEditor
{
    public partial class TileSheetEditor : SceneItemEditor
    {
        public static TileSheetEditor Instance;

        public override SceneItem SceneItem
        {
            get
            {
                return base.SceneItem;
            }
            set
            {
                base.SceneItem = value;                
            }
        }

        internal TileSheet TileSheet
        {
            get;
            set;
        }


        private int _selectedBrushTile = 0;
        internal int SelectedBrushTile
        {
            get { return _selectedBrushTile; }
            set { _selectedBrushTile = value; }
        }


        public TileSheetEditor()
        {
            InitializeComponent();
            this.TileSheet = new TileSheet();
            TileSheetEditor.Instance = this;
        }

        private void TileGridEditor_Load(object sender, EventArgs e)
        {         
            labelTextureName.Text = TileSheet.Material.ToString();        
            checkBoxUseSafeBorder.Checked = TileSheet.UseTilingSafeBorders;
            textBoxTileWidth.Text = TileSheet.TileSize.X.ToString(CultureInfo.InvariantCulture);
            textBoxTileHeight.Text = TileSheet.TileSize.Y.ToString(CultureInfo.InvariantCulture);
        }        

        #region Properties tab event

        private void buttonSelectTexture_Click(object sender, EventArgs e)
        {
            MaterialSelectorDialog materialSelectorDialog = new MaterialSelectorDialog();
            materialSelectorDialog.SelectedMaterial = this.TileSheet.Material;
            materialSelectorDialog.ShowLocalTextures = ItemIsLocal;
            if (materialSelectorDialog.ShowDialog() == DialogResult.OK)
            {
                this.TileSheet.Material = materialSelectorDialog.SelectedMaterial;                                
                labelTextureName.Text = materialSelectorDialog.SelectedMaterial.ToString();
            }
        }

        private void checkBoxUseSafeBorder_CheckedChanged(object sender, EventArgs e)
        {
            this.TileSheet.UseTilingSafeBorders = checkBoxUseSafeBorder.Checked;
        }

        private void textBoxTileWidth_Validated(object sender, EventArgs e)
        {
            try
            {
                int newW = int.Parse(textBoxTileWidth.Text, System.Globalization.CultureInfo.InvariantCulture);
                this.TileSheet.TileSize = new Point(newW, this.TileSheet.TileSize.Y);
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    textBoxTileWidth.Text = this.TileSheet.TileSize.X.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void textBoxTileHeight_Validated(object sender, EventArgs e)
        {
            try
            {
                int newH = int.Parse(textBoxTileHeight.Text, System.Globalization.CultureInfo.InvariantCulture);
                this.TileSheet.TileSize = new Point(this.TileSheet.TileSize.X, newH);
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    textBoxTileHeight.Text = this.TileSheet.TileSize.Y.ToString(CultureInfo.InvariantCulture);
                }
            }
        }      
 
        #endregion                      
    }
}