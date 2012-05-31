namespace Milkshake.Editors
{
    partial class MaterialEditor
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Materials");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialEditor));
            this.treeViewMaterials = new System.Windows.Forms.TreeView();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStripMaterial = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddMaterial = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDeleteMaterial = new System.Windows.Forms.ToolStripButton();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonBrowsePath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panelTexture = new System.Windows.Forms.Panel();
            this.labelSRdefinition = new System.Windows.Forms.Label();
            this.buttonBrowseXML = new System.Windows.Forms.Button();
            this.textBoxSourceRectanglesXML = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.materialPreviewControl = new Milkshake.GraphicsDeviceControls.MaterialDisplayControl();
            this.toolStripMaterial.SuspendLayout();
            this.panelTexture.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewMaterials
            // 
            this.treeViewMaterials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewMaterials.HideSelection = false;
            this.treeViewMaterials.ImageIndex = 0;
            this.treeViewMaterials.ImageList = this.imageListTreeView;
            this.treeViewMaterials.Location = new System.Drawing.Point(12, 29);
            this.treeViewMaterials.Name = "treeViewMaterials";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Materials";
            this.treeViewMaterials.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeViewMaterials.SelectedImageIndex = 0;
            this.treeViewMaterials.Size = new System.Drawing.Size(191, 479);
            this.treeViewMaterials.TabIndex = 0;
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
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(632, 485);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(713, 485);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Preview:";
            // 
            // toolStripMaterial
            // 
            this.toolStripMaterial.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripMaterial.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMaterial.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddMaterial,
            this.toolStripButtonDeleteMaterial});
            this.toolStripMaterial.Location = new System.Drawing.Point(154, 1);
            this.toolStripMaterial.Name = "toolStripMaterial";
            this.toolStripMaterial.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripMaterial.Size = new System.Drawing.Size(49, 25);
            this.toolStripMaterial.TabIndex = 6;
            this.toolStripMaterial.Text = "toolStrip1";
            // 
            // toolStripButtonAddMaterial
            // 
            this.toolStripButtonAddMaterial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddMaterial.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddMaterial.Image")));
            this.toolStripButtonAddMaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddMaterial.Name = "toolStripButtonAddMaterial";
            this.toolStripButtonAddMaterial.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAddMaterial.Text = "Add Material";
            this.toolStripButtonAddMaterial.Click += new System.EventHandler(this.toolStripButtonAddMaterial_Click);
            // 
            // toolStripButtonDeleteMaterial
            // 
            this.toolStripButtonDeleteMaterial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDeleteMaterial.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDeleteMaterial.Image")));
            this.toolStripButtonDeleteMaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteMaterial.Name = "toolStripButtonDeleteMaterial";
            this.toolStripButtonDeleteMaterial.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDeleteMaterial.Text = "toolStripButton2";
            this.toolStripButtonDeleteMaterial.ToolTipText = "Delete Material";
            this.toolStripButtonDeleteMaterial.Click += new System.EventHandler(this.toolStripButtonDeleteMaterial_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.Location = new System.Drawing.Point(38, 38);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(457, 20);
            this.textBoxPath.TabIndex = 7;
            this.textBoxPath.TextChanged += new System.EventHandler(this.textBoxPath_TextChanged);
            // 
            // buttonBrowsePath
            // 
            this.buttonBrowsePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowsePath.Location = new System.Drawing.Point(501, 36);
            this.buttonBrowsePath.Name = "buttonBrowsePath";
            this.buttonBrowsePath.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowsePath.TabIndex = 8;
            this.buttonBrowsePath.Text = "Browse...";
            this.buttonBrowsePath.UseVisualStyleBackColor = true;
            this.buttonBrowsePath.Click += new System.EventHandler(this.buttonBrowsePath_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Path";
            // 
            // panelTexture
            // 
            this.panelTexture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTexture.Controls.Add(this.labelSRdefinition);
            this.panelTexture.Controls.Add(this.buttonBrowseXML);
            this.panelTexture.Controls.Add(this.textBoxSourceRectanglesXML);
            this.panelTexture.Controls.Add(this.label4);
            this.panelTexture.Controls.Add(this.txtName);
            this.panelTexture.Controls.Add(this.materialPreviewControl);
            this.panelTexture.Controls.Add(this.label3);
            this.panelTexture.Controls.Add(this.label2);
            this.panelTexture.Controls.Add(this.buttonBrowsePath);
            this.panelTexture.Controls.Add(this.textBoxPath);
            this.panelTexture.Location = new System.Drawing.Point(209, 1);
            this.panelTexture.Name = "panelTexture";
            this.panelTexture.Size = new System.Drawing.Size(579, 478);
            this.panelTexture.TabIndex = 10;
            // 
            // labelSRdefinition
            // 
            this.labelSRdefinition.AutoSize = true;
            this.labelSRdefinition.Enabled = false;
            this.labelSRdefinition.Location = new System.Drawing.Point(3, 66);
            this.labelSRdefinition.Name = "labelSRdefinition";
            this.labelSRdefinition.Size = new System.Drawing.Size(214, 13);
            this.labelSRdefinition.TabIndex = 14;
            this.labelSRdefinition.Text = "Source Rectangles definition XML (optional)";
            // 
            // buttonBrowseXML
            // 
            this.buttonBrowseXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseXML.Enabled = false;
            this.buttonBrowseXML.Location = new System.Drawing.Point(501, 62);
            this.buttonBrowseXML.Name = "buttonBrowseXML";
            this.buttonBrowseXML.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseXML.TabIndex = 13;
            this.buttonBrowseXML.Text = "Browse...";
            this.buttonBrowseXML.UseVisualStyleBackColor = true;
            this.buttonBrowseXML.Click += new System.EventHandler(this.buttonBrowseXML_Click);
            // 
            // textBoxSourceRectanglesXML
            // 
            this.textBoxSourceRectanglesXML.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSourceRectanglesXML.Enabled = false;
            this.textBoxSourceRectanglesXML.Location = new System.Drawing.Point(223, 64);
            this.textBoxSourceRectanglesXML.Name = "textBoxSourceRectanglesXML";
            this.textBoxSourceRectanglesXML.Size = new System.Drawing.Size(272, 20);
            this.textBoxSourceRectanglesXML.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(38, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(185, 20);
            this.txtName.TabIndex = 10;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // materialPreviewControl
            // 
            this.materialPreviewControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.materialPreviewControl.AutoScroll = true;
            this.materialPreviewControl.AutoScrollMinSize = new System.Drawing.Size(2000, 2000);
            this.materialPreviewControl.Location = new System.Drawing.Point(0, 119);
            this.materialPreviewControl.Name = "materialPreviewControl";
            this.materialPreviewControl.Size = new System.Drawing.Size(579, 359);
            this.materialPreviewControl.StretchTexture = false;
            this.materialPreviewControl.TabIndex = 2;
            this.materialPreviewControl.Text = "Material Preview";
            this.materialPreviewControl.Texture = null;
            // 
            // MaterialEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 520);
            this.Controls.Add(this.panelTexture);
            this.Controls.Add(this.toolStripMaterial);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeViewMaterials);
            this.Name = "MaterialEditor";
            this.Text = "Material Editor";
            this.Load += new System.EventHandler(this.MaterialEditor_Load);
            this.toolStripMaterial.ResumeLayout(false);
            this.toolStripMaterial.PerformLayout();
            this.panelTexture.ResumeLayout(false);
            this.panelTexture.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewMaterials;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imageListTreeView;
        private Milkshake.GraphicsDeviceControls.MaterialDisplayControl materialPreviewControl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStrip toolStripMaterial;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeleteMaterial;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddMaterial;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonBrowsePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelTexture;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label labelSRdefinition;
        private System.Windows.Forms.Button buttonBrowseXML;
        private System.Windows.Forms.TextBox textBoxSourceRectanglesXML;
    }
}