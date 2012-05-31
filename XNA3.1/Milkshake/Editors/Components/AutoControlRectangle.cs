using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Milkshake.Editors.Components
{
    public partial class AutoControlRectangle : AutoControl
    {
        public AutoControlRectangle()
            : base("{X:0 Y:0 Width:0 Height:0}")
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

        private int[] ParseRect(String value)
        {
            int[] ints = new int[4];
            value = value.Replace("{X:", "").Replace("Y:", "")
                .Replace("Width:", "").Replace("Height:", "").Replace("}", "");
            int delimiter = value.LastIndexOf(' ');
            String[] separator = { " " };
            String[] v = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            ints[0] = int.Parse(v[0], CultureInfo.InvariantCulture);
            ints[1] = int.Parse(v[1], CultureInfo.InvariantCulture);
            ints[2] = int.Parse(v[2], CultureInfo.InvariantCulture);
            ints[3] = int.Parse(v[3], CultureInfo.InvariantCulture);
            return ints;
        }

        private string CombineInts()
        {
            return "{X:" + textBoxValueX.Text + " Y:" + textBoxValueY.Text
                + " Width:" + textBoxValueW.Text + " Height:" + textBoxValueH.Text + "}";
        }


        public override void SetValue(String value)
        {            
            if (IsValueValid(value))
            {
                int[] values = ParseRect(value);
                textBoxValueX.Text = values[0].ToString(CultureInfo.InvariantCulture);
                textBoxValueY.Text = values[1].ToString(CultureInfo.InvariantCulture);
                textBoxValueW.Text = values[2].ToString(CultureInfo.InvariantCulture);
                textBoxValueH.Text = values[3].ToString(CultureInfo.InvariantCulture);
                this.OnValueChanged(EventArgs.Empty);
                base.SetValue(value);
            }
            else
            {
                RestoreValue("Cannot convert \"" + value + "\" to Rectangle");
            }            
        }

        protected override bool IsValueValid(String value)
        {
            try
            {
                ParseRect(value);
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
                int[] values = ParseRect(currentValue);
                return new Microsoft.Xna.Framework.Rectangle(values[0], values[1], values[2], values[3]);
            }
            else
            {
                throw new Exception("Invalid Rectangle value: \"" + currentValue + "\"");
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
