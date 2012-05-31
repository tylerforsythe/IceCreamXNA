using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MilkshakeLibrary
{
    public partial class DefaultControlPanel : UserControl
    {
        public DefaultControlPanel()
        {
            InitializeComponent();
        }

        private void checkBoxPreview_CheckedChanged(object sender, EventArgs e)
        {
            SceneItemEditor parent = (SceneItemEditor)Parent;
            parent.PreviewItemOnScene = checkBoxPreview.Checked;            
        }

        private void DefaultControlPanel_Load(object sender, EventArgs e)
        {
            SceneItemEditor parent = (SceneItemEditor)Parent;
            checkBoxPreview.Checked = parent.PreviewItemOnScene;
        }
    }
}
