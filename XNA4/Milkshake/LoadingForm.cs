using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Milkshake
{
    public partial class LoadingForm : Form
    {
        static bool isLoaded;
        public static LoadingForm Instance;

        public LoadingForm()
        {
            isLoaded = true;
            InitializeComponent();
            TopMost = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            isLoaded = false;
            base.OnClosing(e);
        }
        delegate void voidDelegate();
        public static void ShowForm()
        {
            if (!isLoaded)
            {
                Instance = new LoadingForm();
                Instance.StartPosition = FormStartPosition.CenterScreen;
                Instance.Show();
                Instance.BringToFront();
            }
            else
            {
                if (Instance.InvokeRequired)
                    Instance.Invoke(new voidDelegate(ShowForm));
                else            
                    Instance.BringToFront();
            }
        }
        public static void HideForm()
        {
            if (!Instance.InvokeRequired)
                Instance.Close();
            else
                Instance.Invoke(new voidDelegate(HideForm));
        }
        private void StatusChange(string update, int progress)
        {
            if (InvokeRequired)
            {
                IceCream.Serialization.StatusUpdateHandler handler = new IceCream.Serialization.StatusUpdateHandler(StatusChange);
                Invoke(handler,update,progress);
            }
            else
            {
                progressBarLoading.Value = progress;
                labelLoadingInfo.Text = update;
            }

        }
        internal static void ChangeStatus(string update, int progress)
        {
            Instance.StatusChange(update, progress);
        }
        delegate void stringHandler(string name);
        internal static void ChangeHeader(string name)
        {
            if (Instance.InvokeRequired)
            {
                
                Instance.Invoke(new stringHandler(ChangeHeader),name);
            }
            else
            {
                Instance.Text = name;
            }
        }
    }
}
