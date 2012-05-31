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
    public partial class AutoControlEnum : AutoControl
    {
        public AutoControlEnum()
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
            comboBoxValue.Location = new Point(width + LabelPadding, comboBoxValue.Location.Y);
            int valueWidth = this.Size.Width - (width + LabelPadding);
            if (valueWidth > 0)
            {
                comboBoxValue.Size = new Size(valueWidth, comboBoxValue.Size.Height);
            }
        }

        public void SetComboBoxValues(String[] values)
        {
            comboBoxValue.Items.Clear();
            comboBoxValue.Items.AddRange(values);
        }

        public override void SetValue(String value)
        {            
            if (IsValueValid(value))
            {
                comboBoxValue.SelectedIndex = int.Parse(value, CultureInfo.InvariantCulture);
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
                int i = int.Parse(value, CultureInfo.InvariantCulture);
                // check if it is within bounds
                if (i >= comboBoxValue.Items.Count)
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

            if (IsValueValid(comboBoxValue.SelectedIndex.ToString(CultureInfo.InvariantCulture)))
            {
                return int.Parse(comboBoxValue.SelectedIndex.ToString(CultureInfo.InvariantCulture),
                    CultureInfo.InvariantCulture);
            }
            else
            {
                throw new Exception("Invalid int enum value: \"" + comboBoxValue.SelectedIndex + "\"");
            }          
        }

        private void comboBoxValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetValue(comboBoxValue.SelectedIndex.ToString(CultureInfo.InvariantCulture));
        }        
    }
}
