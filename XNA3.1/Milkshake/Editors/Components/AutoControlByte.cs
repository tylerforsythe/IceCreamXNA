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
    public partial class AutoControlByte : AutoControl
    {
        public AutoControlByte()
            : base("0")
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
            numericUpDownValue.Location = new Point(width + LabelPadding, numericUpDownValue.Location.Y);
            int valueWidth = this.Size.Width - (width + LabelPadding);
            if (valueWidth > 0)
            {
                numericUpDownValue.Size = new Size(valueWidth, numericUpDownValue.Size.Height);
            }
        }

        public override void SetValue(String value)
        {            
            if (IsValueValid(value))
            {
                numericUpDownValue.Value = decimal.Parse(value, CultureInfo.InvariantCulture);
                this.OnValueChanged(EventArgs.Empty);
                base.SetValue(value);
            }
            else
            {
                RestoreValue("Cannot convert \"" + value + "\" to float");
            }            
        }

        protected override bool IsValueValid(String value)
        {
            try
            {
                byte testValue = (byte)decimal.Parse(value, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override object GetValue()
        {
            if (IsValueValid(numericUpDownValue.Value.ToString(CultureInfo.InvariantCulture)))
            {
                return (byte)decimal.Parse(numericUpDownValue.Value.ToString(CultureInfo.InvariantCulture),
                    CultureInfo.InvariantCulture);
            }
            else
            {
                throw new Exception("Invalid byte value: \"" + numericUpDownValue.Value + "\"");
            }          
        }

        private void numericUpDownValue_Validated(object sender, EventArgs e)
        {
            SetValue(numericUpDownValue.Value.ToString(CultureInfo.InvariantCulture));
        }

        private void numericUpDownValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetValue(numericUpDownValue.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void numericUpDownValue_ValueChanged(object sender, EventArgs e)
        {            
            SetValue(numericUpDownValue.Value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
