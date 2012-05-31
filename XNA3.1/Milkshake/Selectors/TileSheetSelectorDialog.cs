using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IceCream;
using IceCream.SceneItems;
using IceCream.Drawing;
using Milkshake.Editors;

namespace Milkshake.SelectorDialogs
{
    public partial class TileSheetSelectorDialog : Form
    {
        #region Fields

        private TileSheet _selectedTileSheet;
        private bool _showLocalTextures = false;

        #endregion

        public bool ShowLocalTextures
        {
            get { return _showLocalTextures; }
            set { _showLocalTextures = value;  }
        }

        public TileSheet TileSheet
        {
            get { return _selectedTileSheet; }
            set { _selectedTileSheet = value; }
        }

        public TileSheetSelectorDialog()
        {
            InitializeComponent();            
        }

        private void LoadTreeNodeData(TreeNode rootNode, bool isGlobal)
        {
            rootNode.Nodes.Clear();
            SceneBase sceneBase = SceneManager.GlobalDataHolder;
            if (isGlobal == false)
            {
                sceneBase = SceneManager.ActiveScene;
            }
            /*
            foreach (SceneItem item in sceneBase.TemplateItems)
            {
                if (item is TileSheet)
                {
                    TreeNode newNode = rootNode.Nodes.Add(item.Name + "Node" + item.Name,
                        item.Name, "color_swatch.png", "color_swatch.png");
                    newNode.Tag = (TileSheet)item;
                }
            }*/
        }

        private void TextureSelectorDialog_Load(object sender, EventArgs e)
        {
            LoadTreeNodeData(treeViewTileSheets.Nodes[0], true);
            LoadTreeNodeData(treeViewTileSheets.Nodes[1], false);
        }
        
        private void SelectTileSheet(TileSheet tileSheet)
        {
            _selectedTileSheet = tileSheet;
            materialPreviewControl.Texture = tileSheet.Material.Texture;          
        }

        private void treeViewTextures_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // if it's a material
            if (e.Node.Tag is TileSheet)
            {
                SelectTileSheet(e.Node.Tag as TileSheet);
            }
        }

        private void treeViewMaterials_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // if it's a material
            if (e.Node.Tag is Material)
            {
                // direct close
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    } 
}