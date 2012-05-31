namespace Milkshake.Editors.Particles
{
    partial class ParticleEffectEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParticleEffectEditor));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("DefaultEffect");
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.particleEffectProperties = new System.Windows.Forms.TreeView();
            this.imageListToolbar = new System.Windows.Forms.ImageList(this.components);
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddPartType = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelPartType = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripColorButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonBackground = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonViewShape = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPlay = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomNormal = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomIn = new System.Windows.Forms.ToolStripButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.LinearPropertyControl = new Milkshake.Editors.LinearPropertyControl();
            this.particleEffectControl = new Milkshake.Editors.Particles.ParticleEffectControl();
            this.defaultControlPanel = new MilkshakeLibrary.DefaultControlPanel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeView.Images.SetKeyName(0, "bullet_star.png");
            this.imageListTreeView.Images.SetKeyName(1, "particle_linear.png");
            this.imageListTreeView.Images.SetKeyName(2, "particle_linear_selected.png");
            this.imageListTreeView.Images.SetKeyName(3, "bullet_blue.png");
            this.imageListTreeView.Images.SetKeyName(4, "bullet_yellow.png");
            this.imageListTreeView.Images.SetKeyName(5, "bullet_black.png");
            this.imageListTreeView.Images.SetKeyName(6, "bullet_green.png");
            this.imageListTreeView.Images.SetKeyName(7, "bullet_orange.png");
            this.imageListTreeView.Images.SetKeyName(8, "bullet_pink.png");
            this.imageListTreeView.Images.SetKeyName(9, "bullet_purple.png");
            this.imageListTreeView.Images.SetKeyName(10, "bullet_red.png");
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.propertyGrid.Location = new System.Drawing.Point(0, 489);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGrid.Size = new System.Drawing.Size(256, 252);
            this.propertyGrid.TabIndex = 4;
            this.propertyGrid.ToolbarVisible = false;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // particleEffectProperties
            // 
            this.particleEffectProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.particleEffectProperties.ImageIndex = 0;
            this.particleEffectProperties.ImageList = this.imageListTreeView;
            this.particleEffectProperties.Location = new System.Drawing.Point(0, 28);
            this.particleEffectProperties.Name = "particleEffectProperties";
            treeNode1.Name = "NodeEffect";
            treeNode1.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode1.Text = "DefaultEffect";
            this.particleEffectProperties.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.particleEffectProperties.SelectedImageIndex = 0;
            this.particleEffectProperties.Size = new System.Drawing.Size(256, 451);
            this.particleEffectProperties.TabIndex = 2;
            this.particleEffectProperties.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.particleEffectProperties_AfterSelect);
            // 
            // imageListToolbar
            // 
            this.imageListToolbar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListToolbar.ImageStream")));
            this.imageListToolbar.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListToolbar.Images.SetKeyName(0, "folder.png");
            this.imageListToolbar.Images.SetKeyName(1, "disk.png");
            this.imageListToolbar.Images.SetKeyName(2, "door_in.png");
            this.imageListToolbar.Images.SetKeyName(3, "color_wheel.png");
            this.imageListToolbar.Images.SetKeyName(4, "cake.png");
            this.imageListToolbar.Images.SetKeyName(5, "heart.png");
            // 
            // toolBar
            // 
            this.toolBar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddPartType,
            this.toolStripButtonImage,
            this.toolStripButtonDelPartType,
            this.toolStripSeparator3,
            this.toolStripColorButton,
            this.toolStripButtonBackground,
            this.toolStripSeparator4,
            this.toolStripButtonViewShape,
            this.toolStripSeparator5,
            this.toolStripButtonPlay,
            this.toolStripButtonPause,
            this.toolStripButtonStop,
            this.toolStripSeparator1,
            this.toolStripButtonZoomOut,
            this.toolStripButtonZoomNormal,
            this.toolStripButtonZoomIn});
            this.toolBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolBar.Size = new System.Drawing.Size(1016, 25);
            this.toolBar.TabIndex = 6;
            this.toolBar.Text = "toolStrip1";
            // 
            // toolStripButtonAddPartType
            // 
            this.toolStripButtonAddPartType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddPartType.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddPartType.Image")));
            this.toolStripButtonAddPartType.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddPartType.Name = "toolStripButtonAddPartType";
            this.toolStripButtonAddPartType.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAddPartType.Text = "toolStripButton3";
            this.toolStripButtonAddPartType.ToolTipText = "Add a Particle type";
            this.toolStripButtonAddPartType.Click += new System.EventHandler(this.toolStripButtonAddPartType_Click);
            // 
            // toolStripButtonImage
            // 
            this.toolStripButtonImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImage.Enabled = false;
            this.toolStripButtonImage.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonImage.Image")));
            this.toolStripButtonImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImage.Name = "toolStripButtonImage";
            this.toolStripButtonImage.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonImage.Text = "toolStripButton2";
            this.toolStripButtonImage.ToolTipText = "Load an image for this Particle Type";
            this.toolStripButtonImage.Click += new System.EventHandler(this.toolStripButtonImage_Click);
            // 
            // toolStripButtonDelPartType
            // 
            this.toolStripButtonDelPartType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelPartType.Enabled = false;
            this.toolStripButtonDelPartType.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelPartType.Image")));
            this.toolStripButtonDelPartType.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelPartType.Name = "toolStripButtonDelPartType";
            this.toolStripButtonDelPartType.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDelPartType.Text = "toolStripButton4";
            this.toolStripButtonDelPartType.ToolTipText = "Delete selected Particle type";
            this.toolStripButtonDelPartType.Click += new System.EventHandler(this.toolStripButtonDelPartType_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripColorButton
            // 
            this.toolStripColorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColorButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripColorButton.Image")));
            this.toolStripColorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColorButton.Name = "toolStripColorButton";
            this.toolStripColorButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripColorButton.Text = "toolStripButton1";
            this.toolStripColorButton.ToolTipText = "Change the background color";
            this.toolStripColorButton.Click += new System.EventHandler(this.toolStripColorButton_Click);
            // 
            // toolStripButtonBackground
            // 
            this.toolStripButtonBackground.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBackground.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonBackground.Image")));
            this.toolStripButtonBackground.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBackground.Name = "toolStripButtonBackground";
            this.toolStripButtonBackground.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonBackground.Text = "toolStripButton1";
            this.toolStripButtonBackground.ToolTipText = "Load a background image";
            this.toolStripButtonBackground.Click += new System.EventHandler(this.toolStripButtonBackground_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonViewShape
            // 
            this.toolStripButtonViewShape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonViewShape.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonViewShape.Image")));
            this.toolStripButtonViewShape.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonViewShape.Name = "toolStripButtonViewShape";
            this.toolStripButtonViewShape.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonViewShape.Text = "toolStripButton3";
            this.toolStripButtonViewShape.ToolTipText = "Select a new Emission Shape";
            this.toolStripButtonViewShape.Click += new System.EventHandler(this.toolStripButtonViewShape_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonPlay
            // 
            this.toolStripButtonPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPlay.Enabled = false;
            this.toolStripButtonPlay.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPlay.Image")));
            this.toolStripButtonPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPlay.Name = "toolStripButtonPlay";
            this.toolStripButtonPlay.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPlay.Text = "toolStripButton1";
            this.toolStripButtonPlay.ToolTipText = "Play";
            this.toolStripButtonPlay.Click += new System.EventHandler(this.toolStripButtonPlay_Click);
            // 
            // toolStripButtonPause
            // 
            this.toolStripButtonPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPause.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPause.Image")));
            this.toolStripButtonPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPause.Name = "toolStripButtonPause";
            this.toolStripButtonPause.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPause.Text = "toolStripButton2";
            this.toolStripButtonPause.ToolTipText = "Pause";
            this.toolStripButtonPause.Click += new System.EventHandler(this.toolStripButtonPause_Click);
            // 
            // toolStripButtonStop
            // 
            this.toolStripButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStop.Image")));
            this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonStop.Text = "toolStripButton3";
            this.toolStripButtonStop.ToolTipText = "Stop";
            this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
            // 
            // toolStripButtonZoomOut
            // 
            this.toolStripButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomOut.Image")));
            this.toolStripButtonZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomOut.Name = "toolStripButtonZoomOut";
            this.toolStripButtonZoomOut.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonZoomOut.Text = "Zoom Out";
            // 
            // toolStripButtonZoomNormal
            // 
            this.toolStripButtonZoomNormal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomNormal.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomNormal.Image")));
            this.toolStripButtonZoomNormal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomNormal.Name = "toolStripButtonZoomNormal";
            this.toolStripButtonZoomNormal.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonZoomNormal.Text = "Normal Zoom";
            // 
            // toolStripButtonZoomIn
            // 
            this.toolStripButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomIn.Image")));
            this.toolStripButtonZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomIn.Name = "toolStripButtonZoomIn";
            this.toolStripButtonZoomIn.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonZoomIn.Text = "Zoom In";
            // 
            // LinearPropertyControl
            // 
            this.LinearPropertyControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LinearPropertyControl.Location = new System.Drawing.Point(0, 485);
            this.LinearPropertyControl.Name = "LinearPropertyControl";
            this.LinearPropertyControl.Size = new System.Drawing.Size(256, 256);
            this.LinearPropertyControl.TabIndex = 5;
            this.LinearPropertyControl.Text = "LinearPropertyControl";
            // 
            // particleEffectControl
            // 
            this.particleEffectControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.particleEffectControl.BackgroundColor = new Microsoft.Xna.Framework.Graphics.Color(((byte)(0)), ((byte)(0)), ((byte)(0)), ((byte)(0)));
            this.particleEffectControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.particleEffectControl.Location = new System.Drawing.Point(262, 28);
            this.particleEffectControl.Name = "particleEffectControl";
            this.particleEffectControl.ParentEditor = null;
            this.particleEffectControl.RealMousePos = new Microsoft.Xna.Framework.Vector2(0F, 0F);
            this.particleEffectControl.SceneMousePos = new Microsoft.Xna.Framework.Vector2(0F, 0F);
            this.particleEffectControl.Size = new System.Drawing.Size(754, 662);
            this.particleEffectControl.TabIndex = 1;
            this.particleEffectControl.Text = "particleEffectControl";
            // 
            // defaultControlPanel
            // 
            this.defaultControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultControlPanel.Location = new System.Drawing.Point(804, 705);
            this.defaultControlPanel.Name = "defaultControlPanel";
            this.defaultControlPanel.Size = new System.Drawing.Size(200, 24);
            this.defaultControlPanel.TabIndex = 7;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ParticleEffectEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 741);
            this.Controls.Add(this.defaultControlPanel);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.LinearPropertyControl);
            this.Controls.Add(this.particleEffectProperties);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.particleEffectControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "ParticleEffectEditor";
            this.Text = "Particle Effect Editor";
            this.Load += new System.EventHandler(this.ParticleEffectEditor_Load);
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageListTreeView;
        private ParticleEffectControl particleEffectControl;
        private System.Windows.Forms.TreeView particleEffectProperties;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private LinearPropertyControl LinearPropertyControl;
        private System.Windows.Forms.ImageList imageListToolbar;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddPartType;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelPartType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripColorButton;
        private System.Windows.Forms.ToolStripButton toolStripButtonImage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        internal System.Windows.Forms.ToolStripButton toolStripButtonPlay;
        internal System.Windows.Forms.ToolStripButton toolStripButtonPause;
        internal System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButtonViewShape;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripButton toolStripButtonBackground;
        private MilkshakeLibrary.DefaultControlPanel defaultControlPanel;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomOut;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomNormal;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomIn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

    }
}

