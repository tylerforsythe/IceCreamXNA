namespace Milkshake.Editors.Sprites
{
    partial class SpriteEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpriteEditor));
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.groupBoxTint = new System.Windows.Forms.GroupBox();
            this.textBoxColorHTML = new System.Windows.Forms.TextBox();
            this.numericUpDownTintBlue = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownTintGreen = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownTintRed = new System.Windows.Forms.NumericUpDown();
            this.pictureBoxTint = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBoxSourceRectangle = new System.Windows.Forms.GroupBox();
            this.numericUpDownRectW = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDownRectX = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRectH = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRectY = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.comboBoxBlendingType = new System.Windows.Forms.ComboBox();
            this.groupBoxTexture = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxArea = new System.Windows.Forms.ComboBox();
            this.buttonSelectTexture = new System.Windows.Forms.Button();
            this.labelTextureName = new System.Windows.Forms.Label();
            this.groupBoxProperties = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonUseFullTexture = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAutoDetect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomNormal = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShowWholeImage = new System.Windows.Forms.ToolStripButton();
            this.defaultControlPanel = new MilkshakeLibrary.DefaultControlPanel();
            this.spriteEditorControl = new Milkshake.Editors.Sprites.SpriteEditorControl();
            this.groupBoxTint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTintBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTintGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTintRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTint)).BeginInit();
            this.groupBoxSourceRectangle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRectW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRectX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRectH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRectY)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBoxTexture.SuspendLayout();
            this.groupBoxProperties.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeView.Images.SetKeyName(0, "folder_picture.png");
            this.imageListTreeView.Images.SetKeyName(1, "picture.png");
            // 
            // groupBoxTint
            // 
            this.groupBoxTint.Controls.Add(this.textBoxColorHTML);
            this.groupBoxTint.Controls.Add(this.numericUpDownTintBlue);
            this.groupBoxTint.Controls.Add(this.numericUpDownTintGreen);
            this.groupBoxTint.Controls.Add(this.numericUpDownTintRed);
            this.groupBoxTint.Controls.Add(this.pictureBoxTint);
            this.groupBoxTint.Controls.Add(this.label5);
            this.groupBoxTint.Controls.Add(this.label3);
            this.groupBoxTint.Controls.Add(this.label4);
            this.groupBoxTint.Location = new System.Drawing.Point(6, 178);
            this.groupBoxTint.Name = "groupBoxTint";
            this.groupBoxTint.Size = new System.Drawing.Size(188, 105);
            this.groupBoxTint.TabIndex = 4;
            this.groupBoxTint.TabStop = false;
            this.groupBoxTint.Text = "Tint Color";
            // 
            // textBoxColorHTML
            // 
            this.textBoxColorHTML.Location = new System.Drawing.Point(16, 79);
            this.textBoxColorHTML.Name = "textBoxColorHTML";
            this.textBoxColorHTML.Size = new System.Drawing.Size(72, 20);
            this.textBoxColorHTML.TabIndex = 4;
            this.textBoxColorHTML.Text = "#FFFFFF";
            this.textBoxColorHTML.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxColorHTML.Validated += new System.EventHandler(this.textBoxColorHTML_Validated);
            this.textBoxColorHTML.Click += new System.EventHandler(this.textBoxColorHTML_Click);
            this.textBoxColorHTML.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxColorHTML_KeyDown);
            this.textBoxColorHTML.Enter += new System.EventHandler(this.textBoxColorHTML_Enter);
            // 
            // numericUpDownTintBlue
            // 
            this.numericUpDownTintBlue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownTintBlue.Location = new System.Drawing.Point(144, 79);
            this.numericUpDownTintBlue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownTintBlue.Name = "numericUpDownTintBlue";
            this.numericUpDownTintBlue.Size = new System.Drawing.Size(41, 20);
            this.numericUpDownTintBlue.TabIndex = 12;
            this.numericUpDownTintBlue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownTintBlue.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownTintBlue.ValueChanged += new System.EventHandler(this.numericUpDownTintBlue_ValueChanged);
            // 
            // numericUpDownTintGreen
            // 
            this.numericUpDownTintGreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownTintGreen.Location = new System.Drawing.Point(144, 53);
            this.numericUpDownTintGreen.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownTintGreen.Name = "numericUpDownTintGreen";
            this.numericUpDownTintGreen.Size = new System.Drawing.Size(41, 20);
            this.numericUpDownTintGreen.TabIndex = 11;
            this.numericUpDownTintGreen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownTintGreen.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownTintGreen.ValueChanged += new System.EventHandler(this.numericUpDownTintGreen_ValueChanged);
            // 
            // numericUpDownTintRed
            // 
            this.numericUpDownTintRed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownTintRed.Location = new System.Drawing.Point(144, 26);
            this.numericUpDownTintRed.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownTintRed.Name = "numericUpDownTintRed";
            this.numericUpDownTintRed.Size = new System.Drawing.Size(41, 20);
            this.numericUpDownTintRed.TabIndex = 10;
            this.numericUpDownTintRed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownTintRed.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownTintRed.ValueChanged += new System.EventHandler(this.numericUpDownTintRed_ValueChanged);
            // 
            // pictureBoxTint
            // 
            this.pictureBoxTint.BackColor = System.Drawing.Color.White;
            this.pictureBoxTint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxTint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxTint.Location = new System.Drawing.Point(28, 26);
            this.pictureBoxTint.Name = "pictureBoxTint";
            this.pictureBoxTint.Size = new System.Drawing.Size(48, 48);
            this.pictureBoxTint.TabIndex = 9;
            this.pictureBoxTint.TabStop = false;
            this.pictureBoxTint.Click += new System.EventHandler(this.pictureBoxTint_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(110, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Blue";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(102, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Green";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(111, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Red";
            // 
            // groupBoxSourceRectangle
            // 
            this.groupBoxSourceRectangle.Controls.Add(this.numericUpDownRectW);
            this.groupBoxSourceRectangle.Controls.Add(this.label9);
            this.groupBoxSourceRectangle.Controls.Add(this.numericUpDownRectX);
            this.groupBoxSourceRectangle.Controls.Add(this.numericUpDownRectH);
            this.groupBoxSourceRectangle.Controls.Add(this.numericUpDownRectY);
            this.groupBoxSourceRectangle.Controls.Add(this.label6);
            this.groupBoxSourceRectangle.Controls.Add(this.label7);
            this.groupBoxSourceRectangle.Controls.Add(this.label8);
            this.groupBoxSourceRectangle.Location = new System.Drawing.Point(6, 289);
            this.groupBoxSourceRectangle.Name = "groupBoxSourceRectangle";
            this.groupBoxSourceRectangle.Size = new System.Drawing.Size(188, 72);
            this.groupBoxSourceRectangle.TabIndex = 13;
            this.groupBoxSourceRectangle.TabStop = false;
            this.groupBoxSourceRectangle.Text = "Source Rectangle";
            // 
            // numericUpDownRectW
            // 
            this.numericUpDownRectW.Location = new System.Drawing.Point(33, 46);
            this.numericUpDownRectW.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.numericUpDownRectW.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownRectW.Name = "numericUpDownRectW";
            this.numericUpDownRectW.Size = new System.Drawing.Size(55, 20);
            this.numericUpDownRectW.TabIndex = 14;
            this.numericUpDownRectW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownRectW.Value = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.numericUpDownRectW.ValueChanged += new System.EventHandler(this.numericUpDownRectW_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "W";
            // 
            // numericUpDownRectX
            // 
            this.numericUpDownRectX.Location = new System.Drawing.Point(33, 19);
            this.numericUpDownRectX.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.numericUpDownRectX.Name = "numericUpDownRectX";
            this.numericUpDownRectX.Size = new System.Drawing.Size(55, 20);
            this.numericUpDownRectX.TabIndex = 12;
            this.numericUpDownRectX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownRectX.ValueChanged += new System.EventHandler(this.numericUpDownRectX_ValueChanged);
            // 
            // numericUpDownRectH
            // 
            this.numericUpDownRectH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownRectH.Location = new System.Drawing.Point(134, 46);
            this.numericUpDownRectH.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.numericUpDownRectH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownRectH.Name = "numericUpDownRectH";
            this.numericUpDownRectH.Size = new System.Drawing.Size(51, 20);
            this.numericUpDownRectH.TabIndex = 11;
            this.numericUpDownRectH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownRectH.Value = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.numericUpDownRectH.ValueChanged += new System.EventHandler(this.numericUpDownRectH_ValueChanged);
            // 
            // numericUpDownRectY
            // 
            this.numericUpDownRectY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownRectY.Location = new System.Drawing.Point(134, 19);
            this.numericUpDownRectY.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.numericUpDownRectY.Name = "numericUpDownRectY";
            this.numericUpDownRectY.Size = new System.Drawing.Size(51, 20);
            this.numericUpDownRectY.TabIndex = 10;
            this.numericUpDownRectY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownRectY.ValueChanged += new System.EventHandler(this.numericUpDownRectY_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "X";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(116, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "H";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(116, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Y";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.comboBoxBlendingType);
            this.groupBox5.Location = new System.Drawing.Point(6, 123);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(188, 49);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Blending Type";
            // 
            // comboBoxBlendingType
            // 
            this.comboBoxBlendingType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBlendingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBlendingType.FormattingEnabled = true;
            this.comboBoxBlendingType.Location = new System.Drawing.Point(6, 19);
            this.comboBoxBlendingType.Name = "comboBoxBlendingType";
            this.comboBoxBlendingType.Size = new System.Drawing.Size(176, 21);
            this.comboBoxBlendingType.TabIndex = 0;
            this.comboBoxBlendingType.SelectedIndexChanged += new System.EventHandler(this.comboBoxBlendingType_SelectedIndexChanged);
            // 
            // groupBoxTexture
            // 
            this.groupBoxTexture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTexture.Controls.Add(this.label1);
            this.groupBoxTexture.Controls.Add(this.comboBoxArea);
            this.groupBoxTexture.Controls.Add(this.buttonSelectTexture);
            this.groupBoxTexture.Controls.Add(this.labelTextureName);
            this.groupBoxTexture.Location = new System.Drawing.Point(6, 19);
            this.groupBoxTexture.Name = "groupBoxTexture";
            this.groupBoxTexture.Size = new System.Drawing.Size(182, 98);
            this.groupBoxTexture.TabIndex = 0;
            this.groupBoxTexture.TabStop = false;
            this.groupBoxTexture.Text = "Texture";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Area";
            // 
            // comboBoxArea
            // 
            this.comboBoxArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxArea.Enabled = false;
            this.comboBoxArea.FormattingEnabled = true;
            this.comboBoxArea.Location = new System.Drawing.Point(41, 69);
            this.comboBoxArea.Name = "comboBoxArea";
            this.comboBoxArea.Size = new System.Drawing.Size(135, 21);
            this.comboBoxArea.TabIndex = 2;
            this.comboBoxArea.SelectedIndexChanged += new System.EventHandler(this.comboBoxArea_SelectedIndexChanged);
            // 
            // buttonSelectTexture
            // 
            this.buttonSelectTexture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectTexture.Location = new System.Drawing.Point(46, 40);
            this.buttonSelectTexture.Name = "buttonSelectTexture";
            this.buttonSelectTexture.Size = new System.Drawing.Size(85, 23);
            this.buttonSelectTexture.TabIndex = 1;
            this.buttonSelectTexture.Text = "Select Texture";
            this.buttonSelectTexture.UseVisualStyleBackColor = true;
            this.buttonSelectTexture.Click += new System.EventHandler(this.buttonSelectTexture_Click);
            // 
            // labelTextureName
            // 
            this.labelTextureName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTextureName.AutoEllipsis = true;
            this.labelTextureName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTextureName.Location = new System.Drawing.Point(3, 16);
            this.labelTextureName.MaximumSize = new System.Drawing.Size(173, 21);
            this.labelTextureName.Name = "labelTextureName";
            this.labelTextureName.Size = new System.Drawing.Size(173, 21);
            this.labelTextureName.TabIndex = 0;
            this.labelTextureName.Text = "LG | Big Texture ";
            this.labelTextureName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxProperties
            // 
            this.groupBoxProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxProperties.Controls.Add(this.groupBoxTexture);
            this.groupBoxProperties.Controls.Add(this.groupBox5);
            this.groupBoxProperties.Controls.Add(this.groupBoxSourceRectangle);
            this.groupBoxProperties.Controls.Add(this.groupBoxTint);
            this.groupBoxProperties.Location = new System.Drawing.Point(469, 28);
            this.groupBoxProperties.Name = "groupBoxProperties";
            this.groupBoxProperties.Size = new System.Drawing.Size(200, 364);
            this.groupBoxProperties.TabIndex = 3;
            this.groupBoxProperties.TabStop = false;
            this.groupBoxProperties.Text = "Properties";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonUseFullTexture,
            this.toolStripButtonAutoDetect,
            this.toolStripSeparator6,
            this.toolStripButtonZoomOut,
            this.toolStripButtonZoomNormal,
            this.toolStripButtonZoomIn,
            this.toolStripButtonShowWholeImage});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(679, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonUseFullTexture
            // 
            this.toolStripButtonUseFullTexture.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUseFullTexture.Enabled = false;
            this.toolStripButtonUseFullTexture.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUseFullTexture.Image")));
            this.toolStripButtonUseFullTexture.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUseFullTexture.Name = "toolStripButtonUseFullTexture";
            this.toolStripButtonUseFullTexture.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonUseFullTexture.Text = "toolStripButton1";
            this.toolStripButtonUseFullTexture.ToolTipText = "Use the full texture area";
            this.toolStripButtonUseFullTexture.Click += new System.EventHandler(this.toolStripButtonUseFullTexture_Click);
            // 
            // toolStripButtonAutoDetect
            // 
            this.toolStripButtonAutoDetect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAutoDetect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAutoDetect.Image")));
            this.toolStripButtonAutoDetect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAutoDetect.Name = "toolStripButtonAutoDetect";
            this.toolStripButtonAutoDetect.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAutoDetect.Text = "toolStripButton1";
            this.toolStripButtonAutoDetect.ToolTipText = "Auto detect a cell from grid";
            this.toolStripButtonAutoDetect.Click += new System.EventHandler(this.toolStripButtonAutoDetect_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
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
            // toolStripButtonShowWholeImage
            // 
            this.toolStripButtonShowWholeImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonShowWholeImage.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonShowWholeImage.Image")));
            this.toolStripButtonShowWholeImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowWholeImage.Name = "toolStripButtonShowWholeImage";
            this.toolStripButtonShowWholeImage.Size = new System.Drawing.Size(113, 22);
            this.toolStripButtonShowWholeImage.Text = "Show Whole Image";
            this.toolStripButtonShowWholeImage.Click += new System.EventHandler(this.toolStripButtonShowWholeImage_Click);
            // 
            // defaultControlPanel
            // 
            this.defaultControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultControlPanel.Location = new System.Drawing.Point(469, 398);
            this.defaultControlPanel.Name = "defaultControlPanel";
            this.defaultControlPanel.Size = new System.Drawing.Size(200, 24);
            this.defaultControlPanel.TabIndex = 6;
            // 
            // spriteEditorControl
            // 
            this.spriteEditorControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.spriteEditorControl.Location = new System.Drawing.Point(0, 28);
            this.spriteEditorControl.Name = "spriteEditorControl";
            this.spriteEditorControl.SelectedRectangle = null;
            this.spriteEditorControl.SelectionMode = Milkshake.Editors.Sprites.SpriteEditorSelectionMode.Normal;
            this.spriteEditorControl.Size = new System.Drawing.Size(463, 394);
            this.spriteEditorControl.SpriteRectangles = null;
            this.spriteEditorControl.TabIndex = 1;
            this.spriteEditorControl.Text = "spriteEditorControl";
            this.spriteEditorControl.Zoom = 0F;
            this.spriteEditorControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.spriteEditorControl_MouseMove);
            this.spriteEditorControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.spriteEditorControl_MouseClick);
            // 
            // SpriteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 433);
            this.Controls.Add(this.defaultControlPanel);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.spriteEditorControl);
            this.Controls.Add(this.groupBoxProperties);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 469);
            this.Name = "SpriteEditor";
            this.Text = "Sprite Editor";
            this.Load += new System.EventHandler(this.SpriteEditor_Load);
            this.groupBoxTint.ResumeLayout(false);
            this.groupBoxTint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTintBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTintGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTintRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTint)).EndInit();
            this.groupBoxSourceRectangle.ResumeLayout(false);
            this.groupBoxSourceRectangle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRectW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRectX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRectH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRectY)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBoxTexture.ResumeLayout(false);
            this.groupBoxTexture.PerformLayout();
            this.groupBoxProperties.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageListTreeView;
        public SpriteEditorControl spriteEditorControl;
        private System.Windows.Forms.GroupBox groupBoxTint;
        private System.Windows.Forms.NumericUpDown numericUpDownTintBlue;
        private System.Windows.Forms.NumericUpDown numericUpDownTintGreen;
        private System.Windows.Forms.NumericUpDown numericUpDownTintRed;
        private System.Windows.Forms.PictureBox pictureBoxTint;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBoxSourceRectangle;
        private System.Windows.Forms.NumericUpDown numericUpDownRectW;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDownRectX;
        private System.Windows.Forms.NumericUpDown numericUpDownRectH;
        private System.Windows.Forms.NumericUpDown numericUpDownRectY;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox comboBoxBlendingType;
        private System.Windows.Forms.GroupBox groupBoxTexture;
        private System.Windows.Forms.Button buttonSelectTexture;
        private System.Windows.Forms.Label labelTextureName;
        private System.Windows.Forms.GroupBox groupBoxProperties;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonUseFullTexture;
        private System.Windows.Forms.ToolStripButton toolStripButtonAutoDetect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomOut;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomNormal;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomIn;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowWholeImage;
        private System.Windows.Forms.TextBox textBoxColorHTML;
        private MilkshakeLibrary.DefaultControlPanel defaultControlPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxArea;
    }
}