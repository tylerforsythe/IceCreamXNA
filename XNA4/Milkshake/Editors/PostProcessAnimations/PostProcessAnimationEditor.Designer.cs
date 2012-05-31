namespace Milkshake.Editors.PostProcessAnimations
{
    partial class PostProcessAnimationEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostProcessAnimationEditor));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Embedded IceEffects");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Global IceEffects");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Local IceEffects");
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPlay = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.treeviewEffects = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLoopAmount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLife = new System.Windows.Forms.TextBox();
            this.comboBoxParameters = new System.Windows.Forms.ComboBox();
            this.labelParameters = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.linearPropertyControl = new Milkshake.Editors.LinearPropertyControl();
            this.postProcessAnimControl = new Milkshake.Editors.PostProcessAnimations.PostProcessAnimationControl();
            this.defaultControlPanel = new MilkshakeLibrary.DefaultControlPanel();
            this.toolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolBar
            // 
            this.toolBar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPlay,
            this.toolStripButtonPause,
            this.toolStripButtonStop});
            this.toolBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolBar.Size = new System.Drawing.Size(845, 25);
            this.toolBar.TabIndex = 13;
            this.toolBar.Text = "toolStrip1";
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
            // treeviewEffects
            // 
            this.treeviewEffects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeviewEffects.HideSelection = false;
            this.treeviewEffects.Location = new System.Drawing.Point(4, 44);
            this.treeviewEffects.Name = "treeviewEffects";
            treeNode1.Name = "NodeEmbedded";
            treeNode1.Text = "Embedded IceEffects";
            treeNode2.Name = "NodeGlobal";
            treeNode2.Text = "Global IceEffects";
            treeNode3.Name = "NodeLocal";
            treeNode3.Text = "Local IceEffects";
            this.treeviewEffects.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.treeviewEffects.Size = new System.Drawing.Size(256, 192);
            this.treeviewEffects.TabIndex = 10;
            this.treeviewEffects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeviewEffects_AfterSelect);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 245);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Life:";
            // 
            // textBoxLoopAmount
            // 
            this.textBoxLoopAmount.AcceptsReturn = true;
            this.textBoxLoopAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxLoopAmount.Location = new System.Drawing.Point(89, 266);
            this.textBoxLoopAmount.Name = "textBoxLoopAmount";
            this.textBoxLoopAmount.Size = new System.Drawing.Size(48, 20);
            this.textBoxLoopAmount.TabIndex = 19;
            this.textBoxLoopAmount.Text = "0";
            this.textBoxLoopAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxLoopAmount.Validated += new System.EventHandler(this.textBoxLoopAmount_Validated);
            this.textBoxLoopAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxLoopAmount_KeyDown);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 269);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Loop Amount:";
            // 
            // textBoxLife
            // 
            this.textBoxLife.AcceptsReturn = true;
            this.textBoxLife.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxLife.Location = new System.Drawing.Point(89, 242);
            this.textBoxLife.Name = "textBoxLife";
            this.textBoxLife.Size = new System.Drawing.Size(48, 20);
            this.textBoxLife.TabIndex = 20;
            this.textBoxLife.Text = "4";
            this.textBoxLife.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxLife.Validated += new System.EventHandler(this.textBoxLife_Validated);
            this.textBoxLife.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxLife_KeyDown);
            // 
            // comboBoxParameters
            // 
            this.comboBoxParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxParameters.FormattingEnabled = true;
            this.comboBoxParameters.Location = new System.Drawing.Point(89, 292);
            this.comboBoxParameters.Name = "comboBoxParameters";
            this.comboBoxParameters.Size = new System.Drawing.Size(121, 21);
            this.comboBoxParameters.TabIndex = 22;
            this.comboBoxParameters.SelectedIndexChanged += new System.EventHandler(this.comboBoxParameters_SelectedIndexChanged);
            // 
            // labelParameters
            // 
            this.labelParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelParameters.AutoSize = true;
            this.labelParameters.Location = new System.Drawing.Point(10, 295);
            this.labelParameters.Name = "labelParameters";
            this.labelParameters.Size = new System.Drawing.Size(63, 13);
            this.labelParameters.TabIndex = 23;
            this.labelParameters.Text = "Parameters:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "IceEffect:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // linearPropertyControl
            // 
            this.linearPropertyControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linearPropertyControl.Location = new System.Drawing.Point(4, 317);
            this.linearPropertyControl.Name = "linearPropertyControl";
            this.linearPropertyControl.Size = new System.Drawing.Size(256, 256);
            this.linearPropertyControl.TabIndex = 12;
            this.linearPropertyControl.Text = "LinearPropertyControl";
            // 
            // postProcessAnimControl
            // 
            this.postProcessAnimControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.postProcessAnimControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.postProcessAnimControl.Location = new System.Drawing.Point(266, 28);
            this.postProcessAnimControl.Name = "postProcessAnimControl";
            this.postProcessAnimControl.Size = new System.Drawing.Size(574, 507);
            this.postProcessAnimControl.TabIndex = 9;
            this.postProcessAnimControl.Text = "postProcessAnim";
            // 
            // defaultControlPanel
            // 
            this.defaultControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultControlPanel.Location = new System.Drawing.Point(640, 541);
            this.defaultControlPanel.Name = "defaultControlPanel";
            this.defaultControlPanel.Size = new System.Drawing.Size(200, 24);
            this.defaultControlPanel.TabIndex = 25;
            // 
            // PostProcessAnimationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 576);
            this.Controls.Add(this.defaultControlPanel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelParameters);
            this.Controls.Add(this.comboBoxParameters);
            this.Controls.Add(this.textBoxLife);
            this.Controls.Add(this.textBoxLoopAmount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.linearPropertyControl);
            this.Controls.Add(this.treeviewEffects);
            this.Controls.Add(this.postProcessAnimControl);
            this.Name = "PostProcessAnimationEditor";
            this.Text = "PostProcessAnimation Editor";
            this.Load += new System.EventHandler(this.PostProcessAnimationEditor_Load);
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolBar;
        internal System.Windows.Forms.ToolStripButton toolStripButtonPlay;
        internal System.Windows.Forms.ToolStripButton toolStripButtonPause;
        internal System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private LinearPropertyControl linearPropertyControl;
        private System.Windows.Forms.TreeView treeviewEffects;
        private Milkshake.Editors.PostProcessAnimations.PostProcessAnimationControl postProcessAnimControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLoopAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLife;
        private System.Windows.Forms.ComboBox comboBoxParameters;
        private System.Windows.Forms.Label labelParameters;
        private System.Windows.Forms.Label label4;
        private MilkshakeLibrary.DefaultControlPanel defaultControlPanel;
    }
}