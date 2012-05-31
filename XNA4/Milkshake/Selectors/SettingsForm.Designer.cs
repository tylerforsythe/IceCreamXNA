namespace Milkshake.Selectors
{
    partial class SettingsForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Environment", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Projects", new System.Windows.Forms.TreeNode[] {
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Scene Editor", new System.Windows.Forms.TreeNode[] {
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("SceneItems Editors", new System.Windows.Forms.TreeNode[] {
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Components", new System.Windows.Forms.TreeNode[] {
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Plugins", new System.Windows.Forms.TreeNode[] {
            treeNode11});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.treeViewOptions = new System.Windows.Forms.TreeView();
            this.panelControls = new System.Windows.Forms.Panel();
            this.panelNotImplemented = new System.Windows.Forms.Panel();
            this.labelNotImplemented = new System.Windows.Forms.Label();
            this.flowLayoutPanelSceneEditorGeneral = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBoxSceneEditorAskBeforeDelete = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanelEnvironmentGeneral = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBoxEnvironmentRememberWindowInfo = new System.Windows.Forms.CheckBox();
            this.checkBoxEnvironmentWarnIfUnsaved = new System.Windows.Forms.CheckBox();
            this.checkBoxEnvironmentOpenLastSceneOnLoad = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxEnvironmentAtStartup = new System.Windows.Forms.ComboBox();
            this.panelControls.SuspendLayout();
            this.panelNotImplemented.SuspendLayout();
            this.flowLayoutPanelSceneEditorGeneral.SuspendLayout();
            this.flowLayoutPanelEnvironmentGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(505, 331);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(424, 331);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // treeViewOptions
            // 
            this.treeViewOptions.HideSelection = false;
            this.treeViewOptions.Location = new System.Drawing.Point(12, 12);
            this.treeViewOptions.Name = "treeViewOptions";
            treeNode1.Name = "NodeEnvironmentGeneral";
            treeNode1.Text = "General";
            treeNode2.Name = "NodeEnvironment";
            treeNode2.Text = "Environment";
            treeNode3.Name = "Node0";
            treeNode3.Text = "General";
            treeNode4.Name = "Node2";
            treeNode4.Text = "Projects";
            treeNode5.Name = "Node1";
            treeNode5.Text = "General";
            treeNode6.Name = "Node3";
            treeNode6.Text = "Scene Editor";
            treeNode7.Name = "Node2";
            treeNode7.Text = "General";
            treeNode8.Name = "Node5";
            treeNode8.Text = "SceneItems Editors";
            treeNode9.Name = "Node3";
            treeNode9.Text = "General";
            treeNode10.Name = "Node6";
            treeNode10.Text = "Components";
            treeNode11.Name = "Node4";
            treeNode11.Text = "General";
            treeNode12.Name = "Node4";
            treeNode12.Text = "Plugins";
            this.treeViewOptions.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode4,
            treeNode6,
            treeNode8,
            treeNode10,
            treeNode12});
            this.treeViewOptions.Size = new System.Drawing.Size(183, 312);
            this.treeViewOptions.TabIndex = 4;
            this.treeViewOptions.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewOptions_AfterSelect);
            // 
            // panelControls
            // 
            this.panelControls.Controls.Add(this.flowLayoutPanelEnvironmentGeneral);
            this.panelControls.Controls.Add(this.panelNotImplemented);
            this.panelControls.Controls.Add(this.flowLayoutPanelSceneEditorGeneral);
            this.panelControls.Location = new System.Drawing.Point(202, 13);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(378, 311);
            this.panelControls.TabIndex = 5;
            // 
            // panelNotImplemented
            // 
            this.panelNotImplemented.Controls.Add(this.labelNotImplemented);
            this.panelNotImplemented.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNotImplemented.Location = new System.Drawing.Point(0, 0);
            this.panelNotImplemented.Name = "panelNotImplemented";
            this.panelNotImplemented.Size = new System.Drawing.Size(378, 311);
            this.panelNotImplemented.TabIndex = 3;
            // 
            // labelNotImplemented
            // 
            this.labelNotImplemented.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNotImplemented.Enabled = false;
            this.labelNotImplemented.Location = new System.Drawing.Point(0, 0);
            this.labelNotImplemented.Name = "labelNotImplemented";
            this.labelNotImplemented.Size = new System.Drawing.Size(378, 311);
            this.labelNotImplemented.TabIndex = 0;
            this.labelNotImplemented.Text = "This configuration section has not been implemented yet";
            this.labelNotImplemented.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanelSceneEditorGeneral
            // 
            this.flowLayoutPanelSceneEditorGeneral.Controls.Add(this.checkBoxSceneEditorAskBeforeDelete);
            this.flowLayoutPanelSceneEditorGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelSceneEditorGeneral.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelSceneEditorGeneral.Name = "flowLayoutPanelSceneEditorGeneral";
            this.flowLayoutPanelSceneEditorGeneral.Size = new System.Drawing.Size(378, 311);
            this.flowLayoutPanelSceneEditorGeneral.TabIndex = 6;
            // 
            // checkBoxSceneEditorAskBeforeDelete
            // 
            this.checkBoxSceneEditorAskBeforeDelete.AutoSize = true;
            this.checkBoxSceneEditorAskBeforeDelete.Location = new System.Drawing.Point(3, 3);
            this.checkBoxSceneEditorAskBeforeDelete.Name = "checkBoxSceneEditorAskBeforeDelete";
            this.checkBoxSceneEditorAskBeforeDelete.Size = new System.Drawing.Size(274, 17);
            this.checkBoxSceneEditorAskBeforeDelete.TabIndex = 2;
            this.checkBoxSceneEditorAskBeforeDelete.Text = "Always ask for confirmation before deleting an object";
            this.checkBoxSceneEditorAskBeforeDelete.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelEnvironmentGeneral
            // 
            this.flowLayoutPanelEnvironmentGeneral.Controls.Add(this.label1);
            this.flowLayoutPanelEnvironmentGeneral.Controls.Add(this.comboBoxEnvironmentAtStartup);
            this.flowLayoutPanelEnvironmentGeneral.Controls.Add(this.checkBoxEnvironmentRememberWindowInfo);
            this.flowLayoutPanelEnvironmentGeneral.Controls.Add(this.checkBoxEnvironmentWarnIfUnsaved);
            this.flowLayoutPanelEnvironmentGeneral.Controls.Add(this.checkBoxEnvironmentOpenLastSceneOnLoad);
            this.flowLayoutPanelEnvironmentGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelEnvironmentGeneral.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelEnvironmentGeneral.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelEnvironmentGeneral.Name = "flowLayoutPanelEnvironmentGeneral";
            this.flowLayoutPanelEnvironmentGeneral.Size = new System.Drawing.Size(378, 311);
            this.flowLayoutPanelEnvironmentGeneral.TabIndex = 0;
            // 
            // checkBoxEnvironmentRememberWindowInfo
            // 
            this.checkBoxEnvironmentRememberWindowInfo.AutoSize = true;
            this.checkBoxEnvironmentRememberWindowInfo.Location = new System.Drawing.Point(3, 43);
            this.checkBoxEnvironmentRememberWindowInfo.Name = "checkBoxEnvironmentRememberWindowInfo";
            this.checkBoxEnvironmentRememberWindowInfo.Size = new System.Drawing.Size(202, 17);
            this.checkBoxEnvironmentRememberWindowInfo.TabIndex = 0;
            this.checkBoxEnvironmentRememberWindowInfo.Text = "Remember windows size and position";
            this.checkBoxEnvironmentRememberWindowInfo.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnvironmentWarnIfUnsaved
            // 
            this.checkBoxEnvironmentWarnIfUnsaved.AutoSize = true;
            this.checkBoxEnvironmentWarnIfUnsaved.Location = new System.Drawing.Point(3, 66);
            this.checkBoxEnvironmentWarnIfUnsaved.Name = "checkBoxEnvironmentWarnIfUnsaved";
            this.checkBoxEnvironmentWarnIfUnsaved.Size = new System.Drawing.Size(289, 17);
            this.checkBoxEnvironmentWarnIfUnsaved.TabIndex = 1;
            this.checkBoxEnvironmentWarnIfUnsaved.Text = "Ignore warning when closing an unsaved scene/project";
            this.checkBoxEnvironmentWarnIfUnsaved.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnvironmentOpenLastSceneOnLoad
            // 
            this.checkBoxEnvironmentOpenLastSceneOnLoad.AutoSize = true;
            this.checkBoxEnvironmentOpenLastSceneOnLoad.Location = new System.Drawing.Point(3, 89);
            this.checkBoxEnvironmentOpenLastSceneOnLoad.Name = "checkBoxEnvironmentOpenLastSceneOnLoad";
            this.checkBoxEnvironmentOpenLastSceneOnLoad.Size = new System.Drawing.Size(173, 17);
            this.checkBoxEnvironmentOpenLastSceneOnLoad.TabIndex = 3;
            this.checkBoxEnvironmentOpenLastSceneOnLoad.Text = "Open last edited scene on load";
            this.checkBoxEnvironmentOpenLastSceneOnLoad.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "At startup:";
            // 
            // comboBoxEnvironmentAtStartup
            // 
            this.comboBoxEnvironmentAtStartup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxEnvironmentAtStartup.FormattingEnabled = true;
            this.comboBoxEnvironmentAtStartup.Items.AddRange(new object[] {
            "Do nothing",
            "Open last loaded project",
            "Open startup wizard"});
            this.comboBoxEnvironmentAtStartup.Location = new System.Drawing.Point(3, 16);
            this.comboBoxEnvironmentAtStartup.Name = "comboBoxEnvironmentAtStartup";
            this.comboBoxEnvironmentAtStartup.Size = new System.Drawing.Size(372, 21);
            this.comboBoxEnvironmentAtStartup.TabIndex = 5;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 366);
            this.Controls.Add(this.panelControls);
            this.Controls.Add(this.treeViewOptions);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "MilkShake Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.panelControls.ResumeLayout(false);
            this.panelNotImplemented.ResumeLayout(false);
            this.flowLayoutPanelSceneEditorGeneral.ResumeLayout(false);
            this.flowLayoutPanelSceneEditorGeneral.PerformLayout();
            this.flowLayoutPanelEnvironmentGeneral.ResumeLayout(false);
            this.flowLayoutPanelEnvironmentGeneral.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.TreeView treeViewOptions;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelEnvironmentGeneral;
        private System.Windows.Forms.CheckBox checkBoxEnvironmentRememberWindowInfo;
        private System.Windows.Forms.CheckBox checkBoxEnvironmentWarnIfUnsaved;
        private System.Windows.Forms.CheckBox checkBoxSceneEditorAskBeforeDelete;
        private System.Windows.Forms.CheckBox checkBoxEnvironmentOpenLastSceneOnLoad;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelSceneEditorGeneral;
        private System.Windows.Forms.Panel panelNotImplemented;
        private System.Windows.Forms.Label labelNotImplemented;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxEnvironmentAtStartup;
    }
}