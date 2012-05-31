namespace Milkshake.Tools
{
    partial class SpriteSheetGenerator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpriteSheetGenerator));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxInputDir = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonBrowseInput = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxName = new System.Windows.Forms.ComboBox();
            this.comboBoxOutputDir = new System.Windows.Forms.ComboBox();
            this.checkBoxAlphaCorrection = new System.Windows.Forms.CheckBox();
            this.checkBoxPowerOf2 = new System.Windows.Forms.CheckBox();
            this.checkBoxOverrideBaseColor = new System.Windows.Forms.CheckBox();
            this.pictureBoxTint = new System.Windows.Forms.PictureBox();
            this.numericUpDownSafeBorderSize = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxFileFormat = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonBrowseOutput = new System.Windows.Forms.Button();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSafeBorderSize)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.comboBoxInputDir);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonBrowseInput);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(560, 53);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input";
            // 
            // comboBoxInputDir
            // 
            this.comboBoxInputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxInputDir.FormattingEnabled = true;
            this.comboBoxInputDir.Location = new System.Drawing.Point(103, 20);
            this.comboBoxInputDir.Name = "comboBoxInputDir";
            this.comboBoxInputDir.Size = new System.Drawing.Size(370, 21);
            this.comboBoxInputDir.TabIndex = 4;
            this.comboBoxInputDir.SelectedValueChanged += new System.EventHandler(this.comboBoxInputDir_SelectedValueChanged);
            this.comboBoxInputDir.TextChanged += new System.EventHandler(this.textBoxInputDir_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Sprites Folder";
            // 
            // buttonBrowseInput
            // 
            this.buttonBrowseInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseInput.Location = new System.Drawing.Point(479, 18);
            this.buttonBrowseInput.Name = "buttonBrowseInput";
            this.buttonBrowseInput.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseInput.TabIndex = 1;
            this.buttonBrowseInput.Text = "Browse...";
            this.buttonBrowseInput.UseVisualStyleBackColor = true;
            this.buttonBrowseInput.Click += new System.EventHandler(this.buttonBrowseInput_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.comboBoxName);
            this.groupBox2.Controls.Add(this.comboBoxOutputDir);
            this.groupBox2.Controls.Add(this.checkBoxAlphaCorrection);
            this.groupBox2.Controls.Add(this.checkBoxPowerOf2);
            this.groupBox2.Controls.Add(this.checkBoxOverrideBaseColor);
            this.groupBox2.Controls.Add(this.pictureBoxTint);
            this.groupBox2.Controls.Add(this.numericUpDownSafeBorderSize);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBoxFileFormat);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.buttonBrowseOutput);
            this.groupBox2.Location = new System.Drawing.Point(13, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(560, 201);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // comboBoxName
            // 
            this.comboBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxName.FormattingEnabled = true;
            this.comboBoxName.Location = new System.Drawing.Point(103, 45);
            this.comboBoxName.Name = "comboBoxName";
            this.comboBoxName.Size = new System.Drawing.Size(451, 21);
            this.comboBoxName.TabIndex = 14;
            this.comboBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // comboBoxOutputDir
            // 
            this.comboBoxOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxOutputDir.FormattingEnabled = true;
            this.comboBoxOutputDir.Location = new System.Drawing.Point(103, 20);
            this.comboBoxOutputDir.Name = "comboBoxOutputDir";
            this.comboBoxOutputDir.Size = new System.Drawing.Size(370, 21);
            this.comboBoxOutputDir.TabIndex = 4;
            this.comboBoxOutputDir.TextChanged += new System.EventHandler(this.textBoxOutputDir_TextChanged);
            // 
            // checkBoxAlphaCorrection
            // 
            this.checkBoxAlphaCorrection.AutoSize = true;
            this.checkBoxAlphaCorrection.Checked = true;
            this.checkBoxAlphaCorrection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAlphaCorrection.Location = new System.Drawing.Point(9, 125);
            this.checkBoxAlphaCorrection.Name = "checkBoxAlphaCorrection";
            this.checkBoxAlphaCorrection.Size = new System.Drawing.Size(232, 17);
            this.checkBoxAlphaCorrection.TabIndex = 13;
            this.checkBoxAlphaCorrection.Text = "Correct nearby transparent pixels base color";
            this.checkBoxAlphaCorrection.UseVisualStyleBackColor = true;
            // 
            // checkBoxPowerOf2
            // 
            this.checkBoxPowerOf2.AutoSize = true;
            this.checkBoxPowerOf2.Location = new System.Drawing.Point(9, 170);
            this.checkBoxPowerOf2.Name = "checkBoxPowerOf2";
            this.checkBoxPowerOf2.Size = new System.Drawing.Size(229, 17);
            this.checkBoxPowerOf2.TabIndex = 12;
            this.checkBoxPowerOf2.Text = "Pad output texture\'s size to next power of 2";
            this.checkBoxPowerOf2.UseVisualStyleBackColor = true;
            // 
            // checkBoxOverrideBaseColor
            // 
            this.checkBoxOverrideBaseColor.AutoSize = true;
            this.checkBoxOverrideBaseColor.Checked = true;
            this.checkBoxOverrideBaseColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOverrideBaseColor.Location = new System.Drawing.Point(9, 147);
            this.checkBoxOverrideBaseColor.Name = "checkBoxOverrideBaseColor";
            this.checkBoxOverrideBaseColor.Size = new System.Drawing.Size(227, 17);
            this.checkBoxOverrideBaseColor.TabIndex = 11;
            this.checkBoxOverrideBaseColor.Text = "Override transparent RGB with Base Color:";
            this.checkBoxOverrideBaseColor.UseVisualStyleBackColor = true;
            // 
            // pictureBoxTint
            // 
            this.pictureBoxTint.BackColor = System.Drawing.Color.White;
            this.pictureBoxTint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxTint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxTint.Location = new System.Drawing.Point(242, 144);
            this.pictureBoxTint.Name = "pictureBoxTint";
            this.pictureBoxTint.Size = new System.Drawing.Size(20, 20);
            this.pictureBoxTint.TabIndex = 10;
            this.pictureBoxTint.TabStop = false;
            this.pictureBoxTint.Click += new System.EventHandler(this.pictureBoxTint_Click);
            // 
            // numericUpDownSafeBorderSize
            // 
            this.numericUpDownSafeBorderSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownSafeBorderSize.Location = new System.Drawing.Point(103, 99);
            this.numericUpDownSafeBorderSize.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownSafeBorderSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSafeBorderSize.Name = "numericUpDownSafeBorderSize";
            this.numericUpDownSafeBorderSize.Size = new System.Drawing.Size(451, 20);
            this.numericUpDownSafeBorderSize.TabIndex = 8;
            this.numericUpDownSafeBorderSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Safe Border Size";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Image Format";
            // 
            // comboBoxFileFormat
            // 
            this.comboBoxFileFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFileFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFileFormat.FormattingEnabled = true;
            this.comboBoxFileFormat.Location = new System.Drawing.Point(103, 71);
            this.comboBoxFileFormat.Name = "comboBoxFileFormat";
            this.comboBoxFileFormat.Size = new System.Drawing.Size(451, 21);
            this.comboBoxFileFormat.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Spritesheet Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Output Folder";
            // 
            // buttonBrowseOutput
            // 
            this.buttonBrowseOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseOutput.Location = new System.Drawing.Point(479, 18);
            this.buttonBrowseOutput.Name = "buttonBrowseOutput";
            this.buttonBrowseOutput.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseOutput.TabIndex = 1;
            this.buttonBrowseOutput.Text = "Browse...";
            this.buttonBrowseOutput.UseVisualStyleBackColor = true;
            this.buttonBrowseOutput.Click += new System.EventHandler(this.buttonBrowseOutput_Click);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGenerate.Location = new System.Drawing.Point(158, 284);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(269, 23);
            this.buttonGenerate.TabIndex = 3;
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // SpriteSheetGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 319);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpriteSheetGenerator";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "SpriteSheet Generator";
            this.Load += new System.EventHandler(this.SpriteSheetGenerator_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSafeBorderSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonBrowseInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonBrowseOutput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxFileFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownSafeBorderSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.PictureBox pictureBoxTint;
        private System.Windows.Forms.CheckBox checkBoxPowerOf2;
        private System.Windows.Forms.CheckBox checkBoxOverrideBaseColor;
        private System.Windows.Forms.CheckBox checkBoxAlphaCorrection;
        private System.Windows.Forms.ComboBox comboBoxInputDir;
        private System.Windows.Forms.ComboBox comboBoxName;
        private System.Windows.Forms.ComboBox comboBoxOutputDir;
    }
}