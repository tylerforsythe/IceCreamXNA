namespace Milkshake.SelectorDialogs
{
    partial class MaterialSelectorDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Embedded Materials");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Global Materials");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Local Materials");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialSelectorDialog));
            this.treeViewMaterials = new System.Windows.Forms.TreeView();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.materialPreviewControl = new Milkshake.GraphicsDeviceControls.MaterialDisplayControl();
            this.SuspendLayout();
            // 
            // treeViewMaterials
            // 
            this.treeViewMaterials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewMaterials.ImageIndex = 0;
            this.treeViewMaterials.ImageList = this.imageListTreeView;
            this.treeViewMaterials.Location = new System.Drawing.Point(12, 27);
            this.treeViewMaterials.Name = "treeViewMaterials";
            treeNode1.Name = "NodeEmbedded";
            treeNode1.Text = "Embedded Materials";
            treeNode2.Name = "NodeGlobal";
            treeNode2.Text = "Global Materials";
            treeNode3.Name = "NodeLocal";
            treeNode3.Text = "Local Materials";
            this.treeViewMaterials.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.treeViewMaterials.SelectedImageIndex = 0;
            this.treeViewMaterials.Size = new System.Drawing.Size(191, 442);
            this.treeViewMaterials.TabIndex = 0;
            this.treeViewMaterials.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewMaterials_NodeMouseDoubleClick);
            this.treeViewMaterials.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewTextures_AfterSelect);
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeView.Images.SetKeyName(0, "folder_picture.png");
            this.imageListTreeView.Images.SetKeyName(1, "picture.png");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Materials:";
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(632, 446);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(713, 446);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(206, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Preview:";
            // 
            // materialPreviewControl
            // 
            this.materialPreviewControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.materialPreviewControl.AutoScroll = true;
            this.materialPreviewControl.AutoScrollMinSize = new System.Drawing.Size(2000, 2000);
            this.materialPreviewControl.Location = new System.Drawing.Point(209, 27);
            this.materialPreviewControl.Name = "materialPreviewControl";
            this.materialPreviewControl.Size = new System.Drawing.Size(579, 413);
            this.materialPreviewControl.StretchTexture = false;
            this.materialPreviewControl.TabIndex = 2;
            this.materialPreviewControl.Text = "Material Preview";
            this.materialPreviewControl.Texture = null;
            // 
            // MaterialSelectorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 481);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.materialPreviewControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeViewMaterials);
            this.Name = "MaterialSelectorDialog";
            this.ShowInTaskbar = false;
            this.Text = "Select a Material";
            this.Load += new System.EventHandler(this.TextureSelectorDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewMaterials;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imageListTreeView;
        private Milkshake.GraphicsDeviceControls.MaterialDisplayControl materialPreviewControl;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label2;
    }
}