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
    public partial class AutoControlString : AutoControl
    {
        public AutoControlString()
            : base("BAAAAAh")
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
            textBoxValue.Location = new Point(width + LabelPadding, textBoxValue.Location.Y);
            int valueWidth = this.Size.Width - (width + LabelPadding);
            if (valueWidth > 0)
            {
                textBoxValue.Size = new Size(valueWidth, textBoxValue.Size.Height);
            }
        }

        public override void SetValue(String value)
        {            
            if (IsValueValid(value))
            {                
                textBoxValue.Text = value;
                this.OnValueChanged(EventArgs.Empty);
                base.SetValue(value);
            }
            else
            {
                RestoreValue("Cannot convert \"" + value + "\" to String");
            }           
        }

        protected override bool IsValueValid(String value)
        {
            try
            {
                if (value == null)
                {
                    throw new Exception();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override object GetValue()
        {            
            if (IsValueValid(textBoxValue.Text))
            {
                return textBoxValue.Text;
            }
            else
            {
                throw new Exception("Invalid String value: \"" + textBoxValue.Text + "\"");
            }          
        }

        private void textBoxValue_Validated(object sender, EventArgs e)
        {
            SetValue(textBoxValue.Text);
        }

        private void textBoxValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetValue(textBoxValue.Text);
            }
        }
    }
}
