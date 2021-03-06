﻿namespace Milkshake.Editors.Components
{
    partial class AutoControlPoint
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelFriendlyName = new System.Windows.Forms.Label();
            this.panelValues = new System.Windows.Forms.Panel();
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.textBoxValueX = new System.Windows.Forms.TextBox();
            this.textBoxValueY = new System.Windows.Forms.TextBox();
            this.panelValues.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelFriendlyName
            // 
            this.labelFriendlyName.Location = new System.Drawing.Point(0, 3);
            this.labelFriendlyName.Name = "labelFriendlyName";
            this.labelFriendlyName.Size = new System.Drawing.Size(93, 13);
            this.labelFriendlyName.TabIndex = 0;
            this.labelFriendlyName.Text = "labelFriendlyName";
            // 
            // panelValues
            // 
            this.panelValues.Controls.Add(this.labelX);
            this.panelValues.Controls.Add(this.labelY);
            this.panelValues.Controls.Add(this.textBoxValueX);
            this.panelValues.Controls.Add(this.textBoxValueY);
            this.panelValues.Location = new System.Drawing.Point(102, 0);
            this.panelValues.Name = "panelValues";
            this.panelValues.Size = new System.Drawing.Size(134, 20);
            this.panelValues.TabIndex = 6;
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(0, 3);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(14, 13);
            this.labelX.TabIndex = 8;
            this.labelX.Text = "X";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(65, 3);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(14, 13);
            this.labelY.TabIndex = 7;
            this.labelY.Text = "Y";
            // 
            // textBoxValueX
            // 
            this.textBoxValueX.Location = new System.Drawing.Point(17, 0);
            this.textBoxValueX.Name = "textBoxValueX";
            this.textBoxValueX.Size = new System.Drawing.Size(42, 20);
            this.textBoxValueX.TabIndex = 6;
            this.textBoxValueX.Text = "1.0";
            this.textBoxValueX.Validated += new System.EventHandler(this.textBoxValue_Validated);
            this.textBoxValueX.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxValue_KeyDown);
            // 
            // textBoxValueY
            // 
            this.textBoxValueY.Location = new System.Drawing.Point(82, 0);
            this.textBoxValueY.Name = "textBoxValueY";
            this.textBoxValueY.Size = new System.Drawing.Size(42, 20);
            this.textBoxValueY.TabIndex = 5;
            this.textBoxValueY.Text = "1.0";
            this.textBoxValueY.Validated += new System.EventHandler(this.textBoxValue_Validated);
            this.textBoxValueY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxValue_KeyDown);
            // 
            // AutoControlPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panelValues);
            this.Controls.Add(this.labelFriendlyName);
            this.Name = "AutoControlPoint";
            this.Size = new System.Drawing.Size(236, 20);
            this.panelValues.ResumeLayout(false);
            this.panelValues.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelFriendlyName;
        private System.Windows.Forms.Panel panelValues;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.TextBox textBoxValueX;
        private System.Windows.Forms.TextBox textBoxValueY;
    }
}
