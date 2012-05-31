using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IceCream;
using IceCream.Drawing;
using Milkshake.Editors;

namespace Milkshake.SelectorDialogs
{
    public partial class MaterialSelectorDialog : Form
    {
        #region Fields

        private Material _selectedMaterial;
        private bool _showLocalTextures = false;

        #endregion

        public bool ShowLocalTextures
        {
            get { return _showLocalTextures; }
            set { _showLocalTextures = value;  }
        }

        public Material SelectedMaterial
        {
            get { return _selectedMaterial; }
            set { _selectedMaterial = value; }
        } 

        public MaterialSelectorDialog()
        {
            InitializeComponent();            
        }

        private void TextureSelectorDialog_Load(object sender, EventArgs e)
        {
            MaterialEditor.LoadItemsInTreeView(treeViewMaterials, true, _showLocalTextures);
            SelectTextureNode();
        }

        private void SelectTextureNode()
        {
            /*
            TreeNode selectedNode = null;
            if (_selectedMaterial.TextureScope == TextureScope.Default)
            {
                selectedNode = treeViewTextures.Nodes[0];
            }
            else if (_selectedMaterial.TextureScope == TextureScope.Global)
            {
                foreach (TreeNode node in treeViewTextures.Nodes[1].Nodes)
                {
                    // if it's an untagged texture
                    if (node.ImageIndex == 1)
                    {
                        if (node.Text == _selectedMaterial.UID)
                        {
                            selectedNode = node;
                            break;
                        }
                    }
                    // check the tagged textures
                    else
                    {
                        foreach (TreeNode subNode in node.Nodes)
                        {
                            if (subNode.Text == _selectedMaterial.UID)
                            {
                                selectedNode = subNode;
                                break;
                            }
                        }
                    }
                }
                if (selectedNode == null)
                {
                    IceMaker.DisplayErrorMessage("the global texture with UID \"" + _selectedMaterial.UID + "\" wasn't found in the tree");
                }
            }
            else if (_selectedMaterial.TextureScope == TextureScope.Local)
            {
                if (_showLocalTextures == true)
                {
                    IceMaker.DisplayErrorMessage("The texture reference's scope is set to Local but the showLocalTextures option is set to false");
                }
                else
                {
                    foreach (TreeNode node in treeViewTextures.Nodes[2].Nodes)
                    {
                        // if it's an untagged texture
                        if (node.ImageIndex == 1)
                        {
                            if (node.Text == _selectedMaterial.UID)
                            {
                                selectedNode = node;
                                break;
                            }
                        }
                        // check the tagged textures
                        else
                        {
                            foreach (TreeNode subNode in node.Nodes)
                            {
                                if (subNode.Text == _selectedMaterial.UID)
                                {
                                    selectedNode = subNode;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (selectedNode == null)
                {
                    IceMaker.DisplayErrorMessage("the local texture with UID \"" + _selectedMaterial.UID + "\" wasn't found in the tree");
                }
            }
            // select the node
            treeViewTextures.SelectedNode = selectedNode;
            */
        }
        
        private void SelectTexture(Material material)
        {
            _selectedMaterial = material;
            materialPreviewControl.Texture = material.Texture;          
        }

        private void treeViewTextures_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // if it's a material
            if (e.Node.Tag is Material)
            {                
                SelectTexture(e.Node.Tag as Material);
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