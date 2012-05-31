using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IceCream;
using IceCream.Drawing;
using Milkshake.SelectorDialogs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Milkshake.Editors
{
    public partial class MaterialEditor : Form
    {
        #region Static Methods

        public static void LoadMaterialsInTreeNode(List<Material> materials, TreeNode rootNode, String imageKey)
        {
            rootNode.Nodes.Clear();
            foreach (Material material in materials)
            {
                TreeNode newNode = rootNode.Nodes.Add(rootNode.Text + "Node" + material.Name,
                    material.Name, imageKey, imageKey);
                newNode.Tag = material;
            }
        }

        public static void LoadItemsInTreeView(TreeView treeView, bool showEmbeddedMaterials, bool showLocalMaterials)
        {
            // Load the Embedded items if needed
            if (showEmbeddedMaterials)
            {
                MaterialEditor.LoadMaterialsInTreeNode(
                    SceneManager.EmbeddedMaterials, treeView.Nodes[0], "picture.png");
            }
            // Load the Global items
            MaterialEditor.LoadMaterialsInTreeNode(
                SceneManager.GlobalDataHolder.Materials, treeView.Nodes[1], "picture.png");
            treeView.Nodes[1].Expand();
            // Load the Local items if needed
            if (showLocalMaterials)
            {
                MaterialEditor.LoadMaterialsInTreeNode(
                 SceneManager.ActiveScene.Materials, treeView.Nodes[2], "picture.png");
                treeView.Nodes[2].Expand();
            }
            else
            {
                // remove the local textures node
                treeView.Nodes.RemoveAt(2);
            }
            // remove the Embedded node if it is not needed
            if (!showEmbeddedMaterials)
            {
                treeView.Nodes.RemoveAt(0);
            }
        }

        #endregion

        #region Fields

        private Material _selectedMaterial;
        private bool _showLocalTextures = false;
        private List<Material> _materialList;

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

        public MaterialEditor()
        {
            InitializeComponent();            
        }

        private void MaterialEditor_Load(object sender, EventArgs e)
        {
            panelTexture.Visible = false;
            LoadNodes();
            CheckStatusOfXMLControls();
        }
        private void LoadNodes()
        {
            LoadNodes(false);
        }
        private void LoadNodes(bool selectnew)
        {
            treeViewMaterials.Nodes[0].Nodes.Clear();
            if (_showLocalTextures)
            {
                _materialList = SceneManager.ActiveScene.Materials;
            }
            else
            {
                _materialList = SceneManager.GlobalDataHolder.Materials;
            }
            LoadMaterialsInTreeNode(_materialList, treeViewMaterials.Nodes[0], "picture.png");
            treeViewMaterials.Nodes[0].Expand();
            if (selectnew)
            {
                foreach (TreeNode node in treeViewMaterials.Nodes[0].Nodes)
                {
                    if (node.Tag == _materialList[_materialList.Count - 1])
                    {
                        treeViewMaterials.SelectedNode = node;
                        break;
                    }
                }
            }
        }
        
        private void SelectTexture(Material material)
        {
            _selectedMaterial = material;
            txtName.Text = material.Name;
            PreviewMaterial(material);
        }

        private void PreviewMaterial(Material material)
        {
            textBoxSourceRectanglesXML.Text = _selectedMaterial.AreasDefinitionFilename;
            materialPreviewControl.Texture = material.Texture;
        }

        #region Events

        private TreeNode GetRootParentNode(TreeNode node)
        {            
            while (node.Parent != null)
            {
                // if it's a tagged subnode, retrieve the tag's parent node
                node = node.Parent;
            }
            return node;
        }

        private void treeViewTextures_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // if it's a material
            if (e.Node.Tag is Material)
            {
                SelectTexture(e.Node.Tag as Material);
                toolStripButtonDeleteMaterial.Enabled = true;
                panelTexture.Visible = true;
            }
            else
            {
                toolStripButtonDeleteMaterial.Enabled = false;
                panelTexture.Visible = false;
            }
        }

        private void toolStripButtonDeleteMaterial_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeViewMaterials.SelectedNode;
            if (selectedNode.Tag != null &&
                selectedNode.Tag is Material)
            {                
                if (_materialList.Contains(selectedNode.Tag as Material))
                {
                    _materialList.Remove(selectedNode.Tag as Material);
                    selectedNode.Remove();                    
                }
            }
        }

        private void toolStripButtonAddMaterial_Click(object sender, EventArgs e)
        {
            String newMaterialName;
            // TO-DO: replace this with a unique name generator
            newMaterialName = "New Material";
            Material newMaterial = new Material(newMaterialName, 
                SceneManager.EmbeddedMaterials[0].Texture,
                (_showLocalTextures?AssetScope.Local:AssetScope.Global));
            if (newMaterial.Scope == AssetScope.Local)
            {
                newMaterial.Parent = SceneManager.ActiveScene;
            }
            else
            {
                newMaterial.Parent = SceneManager.GlobalDataHolder;
            }
            _materialList.Add(newMaterial);
            LoadNodes(true);
        }

        private void buttonBrowsePath_Click(object sender, EventArgs e)
        {
            OpenLoadImageDialog();
        }

        private void OpenLoadImageDialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            /*
            // Default to the directory which contains our content files.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "./");
            string contentPath = Path.GetFullPath(relativePath);

            fileDialog.InitialDirectory = contentPath;    
             */
            fileDialog.Title = "Load texture";
            fileDialog.Filter = "PNG Files (*.png)|*.png|" +
                                "TGA Files (*.tga)|*.tga|" +
                                "BMP Files (*.bmp)|*.bmp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if (_selectedMaterial != null)
                {

                    string _tempFilename = fileDialog.FileName;
                    string _rootPath = Path.Combine(IceCreamProject.Instance.Path, 
                        IceCreamProject.Instance.ContentFolderPath);
                    if (!_tempFilename.Contains(_rootPath))
                    {
                        //Copy the file
                        string _destFile = Path.Combine(_rootPath, Path.GetFileName(_tempFilename));
                        File.Copy(_tempFilename, _destFile, true);
                        _tempFilename = _destFile;
                    }
                    //TODO: check if it exists already and if not add it to the content project. (tricky)

                    _tempFilename = _tempFilename.Replace(IceCreamProject.Instance.Path, "")
                        .Replace("\\", "/").TrimStart('/');
                    _selectedMaterial.Filename = _tempFilename;   
                    _selectedMaterial.Texture = Texture2D
                        .FromStream(DrawingManager.GraphicsDevice, System.IO.File.OpenRead(fileDialog.FileName));
                    //TODO: Need ability to choose where to have material
                    MilkshakeForm.Instance.AddFileToContentProject(_selectedMaterial.Filename, true, false, 
                        "TextureProcessor", "TextureImporter");
                    _selectedMaterial.Scope = AssetScope.Local;
                    textBoxPath.Text = _selectedMaterial.Filename;
                    PreviewMaterial(_selectedMaterial);
                }
            }
        }

        private void OpenLoadXMLDialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            /*
            // Default to the directory which contains our content files.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "./");
            string contentPath = Path.GetFullPath(relativePath);

            fileDialog.InitialDirectory = contentPath;    
             */
            fileDialog.Title = "Load texture areas definition file";
            fileDialog.Filter = "XML Files (*.xml)|*.xml";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if (_selectedMaterial != null)
                {
                    string _tempFilename = fileDialog.FileName;
                    _selectedMaterial.LoadAreasDefinition(_tempFilename);
                    string _rootPath = Path.Combine(IceCreamProject.Instance.Path, 
                        IceCreamProject.Instance.ContentFolderPath);
                    if (!_tempFilename.Contains(_rootPath))
                    {
                        //Copy the file
                        string _destFile = Path.Combine(_rootPath, Path.GetFileName(_tempFilename));
                        File.Copy(_tempFilename, _destFile, true);
                        _tempFilename = _destFile;
                    }
                    //TODO: check if it exists already and if not add it to the content project. (tricky)

                    _tempFilename = _tempFilename.Replace(IceCreamProject.Instance.Path, "")
                        .Replace("\\", "/").TrimStart('/');
                    _selectedMaterial.AreasDefinitionFilename = _tempFilename;                    
                    textBoxSourceRectanglesXML.Text = _tempFilename;
                    MilkshakeForm.Instance.AddFileToContentProject(_tempFilename, false, true, null, null);
                    PreviewMaterial(_selectedMaterial);
                }
            }
        }

        #endregion       

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            _selectedMaterial.Name = txtName.Text;
            treeViewMaterials.SelectedNode.Text = txtName.Text;
            textBoxPath.Text = _selectedMaterial.Filename;
        }

        private void CheckStatusOfXMLControls()
        {
            bool enabled = true;
            if (String.IsNullOrEmpty(textBoxPath.Text.Trim()))
            {
                enabled = false;
            }
            textBoxSourceRectanglesXML.Enabled = enabled;
            labelSRdefinition.Enabled = enabled;
            buttonBrowseXML.Enabled = enabled;
        }

        private void buttonBrowseXML_Click(object sender, EventArgs e)
        {
            OpenLoadXMLDialog();
        }

        private void textBoxPath_TextChanged(object sender, EventArgs e)
        {
            CheckStatusOfXMLControls();
        }


    } 
}