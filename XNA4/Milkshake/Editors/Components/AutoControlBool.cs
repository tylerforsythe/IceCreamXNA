using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Milkshake.Editors.Components
{
    public partial class AutoControlBool : AutoControl
    {
        public AutoControlBool()
            : base("False")
        {
            InitializeComponent();
        }

        public override void SetFriendlyName(String value)
        {
            labelFriendlyName.Text = value;
        }

        public override void SetFriendlyNameWidth(int width)
        {
            labelFriendlyName.Size = new Size(width, labelFriendlyName.Size.Height);
            checkBoxValue.Location = new Point(width + LabelPadding, checkBoxValue.Location.Y);
            int valueWidth = this.Size.Width - (width + LabelPadding);
            if (valueWidth > 0)
            {
                checkBoxValue.Size = new Size(valueWidth, checkBoxValue.Size.Height);
            }
        }

        public override void SetValue(String value)
        {           
            if (IsValueValid(value))
            {                
                checkBoxValue.Checked = (value.ToLower() == "true");
                base.SetValue(value);
            }
            else
            {
                RestoreValue("cannot convert \"" + value + "\" to bool");
            }
        }

        protected override bool IsValueValid(String value)
        {
            if (value.ToLower() == "true" || value.ToLower() == "false")
            {
                return true;
            }
            return false;
        }

        public override object GetValue()
        {          
            return checkBoxValue.Checked;         
        }

        private void checkBoxValue_CheckedChanged(object sender, EventArgs e)
        {
            OnValueChanged(EventArgs.Empty);
        } 
    }
}
