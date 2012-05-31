namespace Milkshake.Editors.TileSheetEditor
{
    partial class TileSheetEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TileSheetEditor));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxUseSafeBorder = new System.Windows.Forms.CheckBox();
            this.textBoxTileWidth = new System.Windows.Forms.TextBox();
            this.textBoxTileHeight = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBoxTexture = new System.Windows.Forms.GroupBox();
            this.buttonSelectTexture = new System.Windows.Forms.Button();
            this.labelTextureName = new System.Windows.Forms.Label();
            this.columnVisibility = new XPTable.Models.CheckBoxColumn();
            this.columnName = new XPTable.Models.TextColumn();
            this.defaultControlPanel = new MilkshakeLibrary.DefaultControlPanel();
            this.tabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxTexture.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(263, 250);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(255, 224);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Properties";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBoxTexture);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(249, 215);
            this.panel1.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBoxUseSafeBorder);
            this.groupBox2.Controls.Add(this.textBoxTileWidth);
            this.groupBox2.Controls.Add(this.textBoxTileHeight);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(3, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(241, 68);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tile Size";
            // 
            // checkBoxUseSafeBorder
            // 
            this.checkBoxUseSafeBorder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxUseSafeBorder.AutoSize = true;
            this.checkBoxUseSafeBorder.Checked = true;
            this.checkBoxUseSafeBorder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseSafeBorder.Location = new System.Drawing.Point(111, 45);
            this.checkBoxUseSafeBorder.Name = "checkBoxUseSafeBorder";
            this.checkBoxUseSafeBorder.Size = new System.Drawing.Size(130, 17);
            this.checkBoxUseSafeBorder.TabIndex = 8;
            this.checkBoxUseSafeBorder.Text = "Use tiling safe borders";
            this.checkBoxUseSafeBorder.UseVisualStyleBackColor = true;
            this.checkBoxUseSafeBorder.CheckedChanged += new System.EventHandler(this.checkBoxUseSafeBorder_CheckedChanged);
            // 
            // textBoxTileWidth
            // 
            this.textBoxTileWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTileWidth.Location = new System.Drawing.Point(146, 19);
            this.textBoxTileWidth.Name = "textBoxTileWidth";
            this.textBoxTileWidth.Size = new System.Drawing.Size(30, 20);
            this.textBoxTileWidth.TabIndex = 6;
            this.textBoxTileWidth.Text = "1";
            this.textBoxTileWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxTileWidth.Validated += new System.EventHandler(this.textBoxTileWidth_Validated);
            // 
            // textBoxTileHeight
            // 
            this.textBoxTileHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTileHeight.Location = new System.Drawing.Point(205, 19);
            this.textBoxTileHeight.Name = "textBoxTileHeight";
            this.textBoxTileHeight.Size = new System.Drawing.Size(30, 20);
            this.textBoxTileHeight.TabIndex = 7;
            this.textBoxTileHeight.Text = "1";
            this.textBoxTileHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxTileHeight.Validated += new System.EventHandler(this.textBoxTileHeight_Validated);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(122, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "W";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(184, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "H";
            // 
            // groupBoxTexture
            // 
            this.groupBoxTexture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTexture.Controls.Add(this.buttonSelectTexture);
            this.groupBoxTexture.Controls.Add(this.labelTextureName);
            this.groupBoxTexture.Location = new System.Drawing.Point(3, 3);
            this.groupBoxTexture.Name = "groupBoxTexture";
            this.groupBoxTexture.Size = new System.Drawing.Size(241, 72);
            this.groupBoxTexture.TabIndex = 1;
            this.groupBoxTexture.TabStop = false;
            this.groupBoxTexture.Text = "Texture";
            // 
            // buttonSelectTexture
            // 
            this.buttonSelectTexture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectTexture.Location = new System.Drawing.Point(150, 40);
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
            this.labelTextureName.Location = new System.Drawing.Point(6, 16);
            this.labelTextureName.MaximumSize = new System.Drawing.Size(166, 21);
            this.labelTextureName.Name = "labelTextureName";
            this.labelTextureName.Size = new System.Drawing.Size(166, 21);
            this.labelTextureName.TabIndex = 0;
            this.labelTextureName.Text = "LG | Big Texture ";
            this.labelTextureName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // columnVisibility
            // 
            this.columnVisibility.Alignment = XPTable.Models.ColumnAlignment.Center;
            this.columnVisibility.DrawText = false;
            this.columnVisibility.Image = ((System.Drawing.Image)(resources.GetObject("columnVisibility.Image")));
            this.columnVisibility.Selectable = false;
            this.columnVisibility.Sortable = false;
            this.columnVisibility.Text = "";
            this.columnVisibility.ToolTipText = "Visibiliy";
            this.columnVisibility.Width = 22;
            // 
            // columnName
            // 
            this.columnName.Resizable = false;
            this.columnName.Sortable = false;
            this.columnName.Text = "Name";
            // 
            // defaultControlPanel
            // 
            this.defaultControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultControlPanel.Location = new System.Drawing.Point(322, 242);
            this.defaultControlPanel.Name = "defaultControlPanel";
            this.defaultControlPanel.Size = new System.Drawing.Size(200, 24);
            this.defaultControlPanel.TabIndex = 2;
            // 
            // TileSheetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 274);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.defaultControlPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TileSheetEditor";
            this.Text = "TileGrid Editor";
            this.Load += new System.EventHandler(this.TileGridEditor_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxTexture.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private XPTable.Models.CheckBoxColumn columnVisibility;
        private XPTable.Models.TextColumn columnName;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBoxTexture;
        private System.Windows.Forms.CheckBox checkBoxUseSafeBorder;
        private System.Windows.Forms.TextBox textBoxTileHeight;
        private System.Windows.Forms.Button buttonSelectTexture;
        private System.Windows.Forms.TextBox textBoxTileWidth;
        private System.Windows.Forms.Label labelTextureName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private MilkshakeLibrary.DefaultControlPanel defaultControlPanel;
    }
}