namespace Milkshake
{
    partial class LoadingForm
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
            this.progressBarLoading = new System.Windows.Forms.ProgressBar();
            this.labelLoadingInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBarLoading
            // 
            this.progressBarLoading.Location = new System.Drawing.Point(13, 13);
            this.progressBarLoading.Name = "progressBarLoading";
            this.progressBarLoading.Size = new System.Drawing.Size(267, 23);
            this.progressBarLoading.TabIndex = 0;
            // 
            // labelLoadingInfo
            // 
            this.labelLoadingInfo.Location = new System.Drawing.Point(10, 39);
            this.labelLoadingInfo.Name = "labelLoadingInfo";
            this.labelLoadingInfo.Size = new System.Drawing.Size(270, 23);
            this.labelLoadingInfo.TabIndex = 1;
            this.labelLoadingInfo.Text = "Loading:";
            this.labelLoadingInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 74);
            this.Controls.Add(this.labelLoadingInfo);
            this.Controls.Add(this.progressBarLoading);
            this.Name = "LoadingForm";
            this.Text = "LoadingForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarLoading;
        private System.Windows.Forms.Label labelLoadingInfo;
    }
}