namespace Milkshake.Editors.Components
{
    partial class AutoControlEnum
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
            this.comboBoxValue = new System.Windows.Forms.ComboBox();
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
            // comboBoxValue
            // 
            this.comboBoxValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxValue.FormattingEnabled = true;
            this.comboBoxValue.Location = new System.Drawing.Point(102, 0);
            this.comboBoxValue.Name = "comboBoxValue";
            this.comboBoxValue.Size = new System.Drawing.Size(98, 21);
            this.comboBoxValue.TabIndex = 2;
            this.comboBoxValue.SelectedIndexChanged += new System.EventHandler(this.comboBoxValue_SelectedIndexChanged);
            // 
            // AutoControlEnum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.comboBoxValue);
            this.Controls.Add(this.labelFriendlyName);
            this.Name = "AutoControlEnum";
            this.Size = new System.Drawing.Size(200, 22);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelFriendlyName;
        private System.Windows.Forms.ComboBox comboBoxValue;
    }
}
