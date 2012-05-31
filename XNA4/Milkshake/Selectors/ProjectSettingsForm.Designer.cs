namespace Milkshake.Selectors
{
    partial class ProjectSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectSettingsForm));
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxNativeResWidth = new System.Windows.Forms.TextBox();
            this.textBoxNativeResHeight = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoLoginToLive = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxContentPath = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(335, 131);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(416, 131);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "W";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(87, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "H";
            // 
            // textBoxNativeResWidth
            // 
            this.textBoxNativeResWidth.Location = new System.Drawing.Point(31, 19);
            this.textBoxNativeResWidth.Name = "textBoxNativeResWidth";
            this.textBoxNativeResWidth.Size = new System.Drawing.Size(45, 20);
            this.textBoxNativeResWidth.TabIndex = 5;
            this.textBoxNativeResWidth.Text = "1280";
            this.textBoxNativeResWidth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxNativeResWidth_KeyDown);
            this.textBoxNativeResWidth.Validated += new System.EventHandler(this.textBoxNativeResWidth_Validated);
            // 
            // textBoxNativeResHeight
            // 
            this.textBoxNativeResHeight.Location = new System.Drawing.Point(105, 19);
            this.textBoxNativeResHeight.Name = "textBoxNativeResHeight";
            this.textBoxNativeResHeight.Size = new System.Drawing.Size(45, 20);
            this.textBoxNativeResHeight.TabIndex = 6;
            this.textBoxNativeResHeight.Text = "720";
            this.textBoxNativeResHeight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxNativeResHeight_KeyDown);
            this.textBoxNativeResHeight.Validated += new System.EventHandler(this.textBoxNativeResHeight_Validated);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxNativeResHeight);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxNativeResWidth);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(161, 50);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Native Resolution";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxAutoLoginToLive);
            this.groupBox2.Location = new System.Drawing.Point(179, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(161, 50);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sign Into Live";
            // 
            // checkBoxAutoLoginToLive
            // 
            this.checkBoxAutoLoginToLive.AutoSize = true;
            this.checkBoxAutoLoginToLive.Location = new System.Drawing.Point(6, 22);
            this.checkBoxAutoLoginToLive.Name = "checkBoxAutoLoginToLive";
            this.checkBoxAutoLoginToLive.Size = new System.Drawing.Size(77, 17);
            this.checkBoxAutoLoginToLive.TabIndex = 7;
            this.checkBoxAutoLoginToLive.Text = "Auto-Login";
            this.checkBoxAutoLoginToLive.UseVisualStyleBackColor = true;
            this.checkBoxAutoLoginToLive.CheckedChanged += new System.EventHandler(this.checkBoxAutoLoginToLive_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.textBoxContentPath);
            this.groupBox3.Location = new System.Drawing.Point(12, 68);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(479, 50);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Content Path";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Content Path";
            // 
            // textBoxContentPath
            // 
            this.textBoxContentPath.Enabled = false;
            this.textBoxContentPath.Location = new System.Drawing.Point(85, 22);
            this.textBoxContentPath.Name = "textBoxContentPath";
            this.textBoxContentPath.Size = new System.Drawing.Size(388, 20);
            this.textBoxContentPath.TabIndex = 8;
            this.textBoxContentPath.Text = "Content";
            this.textBoxContentPath.TextChanged += new System.EventHandler(this.textBoxRelativeContentPath_TextChanged);
            // 
            // ProjectSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 166);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProjectSettingsForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Project Settings";
            this.Load += new System.EventHandler(this.ProjectSettingsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxNativeResWidth;
        private System.Windows.Forms.TextBox textBoxNativeResHeight;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxAutoLoginToLive;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxContentPath;
    }
}