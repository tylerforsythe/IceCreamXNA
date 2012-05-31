using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using XnaPoint = Microsoft.Xna.Framework.Point;

namespace Milkshake.Selectors
{
    public partial class ProjectSettingsForm : Form
    {
        public XnaPoint NativeResolution
        {
            get;
            set;
        }

        public bool AutoSignIntoLive
        {
            get;
            set;
        }

        public string ContentFolderPath
        {
            get;
            set;
        }

        public ProjectSettingsForm()
        {
            InitializeComponent();
        }

        private void ProjectSettingsForm_Load(object sender, EventArgs e)
        {
            textBoxNativeResWidth.Text = this.NativeResolution.X.ToString(CultureInfo.InvariantCulture);
            textBoxNativeResHeight.Text = this.NativeResolution.Y.ToString(CultureInfo.InvariantCulture);

            checkBoxAutoLoginToLive.Checked = this.AutoSignIntoLive;

            textBoxContentPath.Text = this.ContentFolderPath.ToString(CultureInfo.InvariantCulture);
        }

        #region Events

        private void checkBoxAutoLoginToLive_CheckedChanged(object sender, EventArgs e) {
            this.AutoSignIntoLive = checkBoxAutoLoginToLive.Checked;
        }

        private void textBoxRelativeContentPath_TextChanged(object sender, EventArgs e) {
            this.ContentFolderPath = textBoxContentPath.Text;
        }

        private void textBoxNativeResWidth_Validated(object sender, EventArgs e)
        {
            try
            {
                int newSize = Int32.Parse(((TextBox)sender).Text, CultureInfo.InvariantCulture);
                this.NativeResolution = new XnaPoint(newSize, this.NativeResolution.Y);
            }
            catch
            {
                ((TextBox)sender).Text = this.NativeResolution.X.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void textBoxNativeResHeight_Validated(object sender, EventArgs e)
        {
            try
            {
                int newSize = Int32.Parse(((TextBox)sender).Text, CultureInfo.InvariantCulture);
                this.NativeResolution = new XnaPoint(this.NativeResolution.X, newSize);
            }
            catch
            {
                ((TextBox)sender).Text = this.NativeResolution.Y.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void textBoxNativeResWidth_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.textBoxNativeResWidth_Validated(sender, EventArgs.Empty);
            }
        }

        private void textBoxNativeResHeight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.textBoxNativeResHeight_Validated(sender, EventArgs.Empty);
            }
        }

        

        #endregion
    }
}
