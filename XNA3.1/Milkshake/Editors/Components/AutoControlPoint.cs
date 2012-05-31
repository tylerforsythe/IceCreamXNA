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
    public partial class AutoControlPoint : AutoControl
    {
        public AutoControlPoint()
            : base("{X:0 Y:0}")
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
            panelValues.Location = new Point(width + LabelPadding, panelValues.Location.Y);
        }

        private int[] ParsePoint(String value)
        {
            int[] ints = new int[2];
            value = value.Replace("{X:", "").Replace("Y:", "").Replace("}", "");
            int delimiter = value.LastIndexOf(' ');
            string v1 = value.Substring(0, delimiter);
            string v2 = value.Substring(delimiter + 1);
            ints[0] = int.Parse(v1, CultureInfo.InvariantCulture);
            ints[1] = int.Parse(v2, CultureInfo.InvariantCulture);
            return ints;
        }

        private string CombineInts()
        {
            return "{X:" + textBoxValueX.Text + " Y:" + textBoxValueY.Text + "}";
        }


        public override void SetValue(String value)
        {            
            if (IsValueValid(value))
            {
                int[] values = ParsePoint(value);
                textBoxValueX.Text = values[0].ToString(CultureInfo.InvariantCulture);
                textBoxValueY.Text = values[1].ToString(CultureInfo.InvariantCulture);
                this.OnValueChanged(EventArgs.Empty);
                base.SetValue(value);
            }
            else
            {
                RestoreValue("Cannot convert \"" + value + "\" to Point");
            }            
        }

        protected override bool IsValueValid(String value)
        {
            try
            {
                ParsePoint(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override object GetValue()
        {
            string currentValue = CombineInts();
            if (IsValueValid(currentValue))
            {
                int[] values = ParsePoint(currentValue);
                return new Microsoft.Xna.Framework.Point(values[0], values[1]);
            }
            else
            {
                throw new Exception("Invalid Point value: \"" + currentValue + "\"");
            }          
        }
                
        private void textBoxValue_Validated(object sender, EventArgs e)
        {
            SetValue(CombineInts());
        }

        private void textBoxValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetValue(CombineInts());
            }
        }
    }
}
