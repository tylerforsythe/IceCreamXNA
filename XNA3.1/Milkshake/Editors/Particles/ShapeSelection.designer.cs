namespace Milkshake.Editors.Particles
{
    partial class ShapeSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShapeSelection));
            this.comboBoxShapes = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.panelSize = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
            this.panelMask = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxImagePath = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.radioButtonFilled = new System.Windows.Forms.RadioButton();
            this.radioButtonOutlined = new System.Windows.Forms.RadioButton();
            this.checkBoxLeft = new System.Windows.Forms.CheckBox();
            this.checkBoxRight = new System.Windows.Forms.CheckBox();
            this.checkBoxTop = new System.Windows.Forms.CheckBox();
            this.checkBoxBottom = new System.Windows.Forms.CheckBox();
            this.panelSize.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).BeginInit();
            this.panelMask.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxShapes
            // 
            this.comboBoxShapes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxShapes.FormattingEnabled = true;
            this.comboBoxShapes.Location = new System.Drawing.Point(12, 12);
            this.comboBoxShapes.Name = "comboBoxShapes";
            this.comboBoxShapes.Size = new System.Drawing.Size(238, 21);
            this.comboBoxShapes.TabIndex = 0;
            this.comboBoxShapes.SelectedIndexChanged += new System.EventHandler(this.comboBoxShapes_SelectedIndexChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(173, 222);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(92, 222);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // panelSize
            // 
            this.panelSize.Controls.Add(this.groupBox1);
            this.panelSize.Location = new System.Drawing.Point(12, 39);
            this.panelSize.Name = "panelSize";
            this.panelSize.Size = new System.Drawing.Size(233, 112);
            this.panelSize.TabIndex = 3;
            this.panelSize.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDownHeight);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numericUpDownWidth);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 88);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Size properties";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Width";
            // 
            // numericUpDownHeight
            // 
            this.numericUpDownHeight.Location = new System.Drawing.Point(65, 55);
            this.numericUpDownHeight.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new System.Drawing.Size(75, 20);
            this.numericUpDownHeight.TabIndex = 1;
            this.numericUpDownHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownHeight.ValueChanged += new System.EventHandler(this.numericUpDownHeight_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Height";
            // 
            // numericUpDownWidth
            // 
            this.numericUpDownWidth.Location = new System.Drawing.Point(65, 26);
            this.numericUpDownWidth.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new System.Drawing.Size(75, 20);
            this.numericUpDownWidth.TabIndex = 0;
            this.numericUpDownWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownWidth.ValueChanged += new System.EventHandler(this.numericUpDownWidth_ValueChanged);
            // 
            // panelMask
            // 
            this.panelMask.Controls.Add(this.groupBox2);
            this.panelMask.Location = new System.Drawing.Point(12, 39);
            this.panelMask.Name = "panelMask";
            this.panelMask.Size = new System.Drawing.Size(233, 182);
            this.panelMask.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxBottom);
            this.groupBox2.Controls.Add(this.checkBoxTop);
            this.groupBox2.Controls.Add(this.checkBoxRight);
            this.groupBox2.Controls.Add(this.checkBoxLeft);
            this.groupBox2.Controls.Add(this.radioButtonOutlined);
            this.groupBox2.Controls.Add(this.radioButtonFilled);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxImagePath);
            this.groupBox2.Controls.Add(this.buttonBrowse);
            this.groupBox2.Location = new System.Drawing.Point(0, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(233, 175);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Texture Mask";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Source image";
            // 
            // textBoxImagePath
            // 
            this.textBoxImagePath.Location = new System.Drawing.Point(12, 44);
            this.textBoxImagePath.Name = "textBoxImagePath";
            this.textBoxImagePath.Size = new System.Drawing.Size(212, 20);
            this.textBoxImagePath.TabIndex = 1;
            this.textBoxImagePath.TextChanged += new System.EventHandler(this.textBoxImagePath_TextChanged);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(155, 70);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(69, 23);
            this.buttonBrowse.TabIndex = 0;
            this.buttonBrowse.Text = "&Browse...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // radioButtonFilled
            // 
            this.radioButtonFilled.AutoSize = true;
            this.radioButtonFilled.Location = new System.Drawing.Point(12, 149);
            this.radioButtonFilled.Name = "radioButtonFilled";
            this.radioButtonFilled.Size = new System.Drawing.Size(140, 17);
            this.radioButtonFilled.TabIndex = 3;
            this.radioButtonFilled.Text = "Use the complete shape";
            this.radioButtonFilled.UseVisualStyleBackColor = true;
            this.radioButtonFilled.CheckedChanged += new System.EventHandler(this.radioButtonFilled_CheckedChanged);
            // 
            // radioButtonOutlined
            // 
            this.radioButtonOutlined.AutoSize = true;
            this.radioButtonOutlined.Checked = true;
            this.radioButtonOutlined.Location = new System.Drawing.Point(12, 103);
            this.radioButtonOutlined.Name = "radioButtonOutlined";
            this.radioButtonOutlined.Size = new System.Drawing.Size(126, 17);
            this.radioButtonOutlined.TabIndex = 4;
            this.radioButtonOutlined.TabStop = true;
            this.radioButtonOutlined.Text = "Use outline only from:";
            this.radioButtonOutlined.UseVisualStyleBackColor = true;
            this.radioButtonOutlined.CheckedChanged += new System.EventHandler(this.radioButtonOutlined_CheckedChanged);
            // 
            // checkBoxLeft
            // 
            this.checkBoxLeft.AutoSize = true;
            this.checkBoxLeft.Checked = true;
            this.checkBoxLeft.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLeft.Location = new System.Drawing.Point(22, 126);
            this.checkBoxLeft.Name = "checkBoxLeft";
            this.checkBoxLeft.Size = new System.Drawing.Size(40, 17);
            this.checkBoxLeft.TabIndex = 5;
            this.checkBoxLeft.Text = "left";
            this.checkBoxLeft.UseVisualStyleBackColor = true;
            // 
            // checkBoxRight
            // 
            this.checkBoxRight.AutoSize = true;
            this.checkBoxRight.Checked = true;
            this.checkBoxRight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRight.Location = new System.Drawing.Point(68, 126);
            this.checkBoxRight.Name = "checkBoxRight";
            this.checkBoxRight.Size = new System.Drawing.Size(46, 17);
            this.checkBoxRight.TabIndex = 6;
            this.checkBoxRight.Text = "right";
            this.checkBoxRight.UseVisualStyleBackColor = true;
            // 
            // checkBoxTop
            // 
            this.checkBoxTop.AutoSize = true;
            this.checkBoxTop.Checked = true;
            this.checkBoxTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTop.Location = new System.Drawing.Point(114, 126);
            this.checkBoxTop.Name = "checkBoxTop";
            this.checkBoxTop.Size = new System.Drawing.Size(41, 17);
            this.checkBoxTop.TabIndex = 7;
            this.checkBoxTop.Text = "top";
            this.checkBoxTop.UseVisualStyleBackColor = true;
            // 
            // checkBoxBottom
            // 
            this.checkBoxBottom.AutoSize = true;
            this.checkBoxBottom.Checked = true;
            this.checkBoxBottom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxBottom.Location = new System.Drawing.Point(161, 126);
            this.checkBoxBottom.Name = "checkBoxBottom";
            this.checkBoxBottom.Size = new System.Drawing.Size(58, 17);
            this.checkBoxBottom.TabIndex = 8;
            this.checkBoxBottom.Text = "bottom";
            this.checkBoxBottom.UseVisualStyleBackColor = true;
            // 
            // ShapeSelection
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(260, 251);
            this.Controls.Add(this.panelMask);
            this.Controls.Add(this.panelSize);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.comboBoxShapes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShapeSelection";
            this.Text = "Selecter an Emitter Shape";
            this.Load += new System.EventHandler(this.ShapeSelection_Load);
            this.panelSize.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).EndInit();
            this.panelMask.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Panel panelSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox comboBoxShapes;
        public System.Windows.Forms.NumericUpDown numericUpDownHeight;
        public System.Windows.Forms.NumericUpDown numericUpDownWidth;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panelMask;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TextBox textBoxImagePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButtonFilled;
        private System.Windows.Forms.CheckBox checkBoxBottom;
        private System.Windows.Forms.CheckBox checkBoxTop;
        private System.Windows.Forms.CheckBox checkBoxRight;
        private System.Windows.Forms.CheckBox checkBoxLeft;
        private System.Windows.Forms.RadioButton radioButtonOutlined;
    }
}