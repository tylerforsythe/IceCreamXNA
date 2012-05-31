using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using XnaColor = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework;

namespace Milkshake.Selectors
{
    public partial class SettingsForm : Form
    {
        public MilkshakePreferences Preferences
        {
            get;
            set;
        }

        public SettingsForm()
        {
            InitializeComponent();
        }

        #region Form events

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            TagNodes(treeViewOptions.Nodes, panelNotImplemented);
            LoadEnvironmentData();
            LoadSceneEditorData();
            treeViewOptions.SelectedNode = treeViewOptions.Nodes[0];
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                SaveEnvironmentData();
                SaveSceneEditorData();
            }
            base.OnClosed(e);
        }

        private void treeViewOptions_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                Control panel = e.Node.Tag as Control;
                SelectPanel(panel);
            }
        }

        private void TagNodes(TreeNodeCollection nodes, object tag)
        {
            foreach (TreeNode node in nodes)
            {
                node.Tag = tag;
                TagNodes(node.Nodes, tag);                
            }
        }

        private void TagRootNode(int nodeIndex, object tag)
        {
            treeViewOptions.Nodes[nodeIndex].Tag = tag;
            treeViewOptions.Nodes[nodeIndex].Nodes[0].Tag = tag;
        }

        #endregion

        #region Panels

        private void SelectPanel(Control panel)
        {
            foreach (Control control in panelControls.Controls)
            {
                if (control == panel)
                {
                    control.Visible = true;
                }
                else
                {
                    control.Visible = false;
                }
            }            
        }

        #endregion

        #region Environment

        private void LoadEnvironmentData()
        {
            TagRootNode(0, flowLayoutPanelEnvironmentGeneral);
            comboBoxEnvironmentAtStartup.SelectedIndex = (int)this.Preferences.EditorStartupAction;
            checkBoxEnvironmentRememberWindowInfo.Checked =
                this.Preferences.ForceWindowParametersAtStartup;
            checkBoxEnvironmentWarnIfUnsaved.Checked = this.Preferences.IgnoreModificationWarning;
            checkBoxEnvironmentOpenLastSceneOnLoad.Checked = this.Preferences.AutoLoadLastOpenedScene;
        }

        private void SaveEnvironmentData()
        {
            this.Preferences.EditorStartupAction = 
                (MilkshakePreferencesEditorStartupAction)comboBoxEnvironmentAtStartup.SelectedIndex;
            this.Preferences.ForceWindowParametersAtStartup = 
                checkBoxEnvironmentRememberWindowInfo.Checked;
            this.Preferences.IgnoreModificationWarning = checkBoxEnvironmentWarnIfUnsaved.Checked;
            this.Preferences.AutoLoadLastOpenedScene = checkBoxEnvironmentOpenLastSceneOnLoad.Checked;
        }

        #endregion

        #region Scene Editor

        private void LoadSceneEditorData()
        {
            TagRootNode(2, flowLayoutPanelSceneEditorGeneral);
            checkBoxSceneEditorAskBeforeDelete.Checked = 
                this.Preferences.ConfirmBeforeObjectDelete;
        }

        private void SaveSceneEditorData()
        {
            this.Preferences.ConfirmBeforeObjectDelete = checkBoxSceneEditorAskBeforeDelete.Checked;
        }

        #endregion
    }
}
