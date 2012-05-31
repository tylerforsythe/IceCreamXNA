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
    public partial class AutoControlVector2 : AutoControl
    {
        public AutoControlVector2()
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

        private float[] ParseVector2(String value)
        {
            float[] floats = new float[2];
            value = value.Replace("{X:", "").Replace("Y:", "").Replace("}", "");
            int delimiter = value.LastIndexOf(' ');
            string v1 = value.Substring(0, delimiter);
            string v2 = value.Substring(delimiter + 1);
            floats[0] = float.Parse(v1, CultureInfo.InvariantCulture);
            floats[1] = float.Parse(v2, CultureInfo.InvariantCulture);
            return floats;
        }

        private string CombineFloats()
        {
            return "{X:" + textBoxValueX.Text + " Y:" + textBoxValueY.Text + "}";
        }


        public override void SetValue(String value)
        {            
            if (IsValueValid(value))
            {
                float[] values = ParseVector2(value);
                textBoxValueX.Text = values[0].ToString(CultureInfo.InvariantCulture);
                textBoxValueY.Text = values[1].ToString(CultureInfo.InvariantCulture);
                this.OnValueChanged(EventArgs.Empty);
                base.SetValue(value);
            }
            else
            {
                RestoreValue("Cannot convert \"" + value + "\" to Vector2");
            }            
        }

        protected override bool IsValueValid(String value)
        {
            try
            {
                ParseVector2(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override object GetValue()
        {
            string currentValue = CombineFloats();
            if (IsValueValid(currentValue))
            {
                float[] values = ParseVector2(currentValue);
                return new Microsoft.Xna.Framework.Vector2(values[0], values[1]);
            }
            else
            {
                throw new Exception("Invalid Vector2 value: \"" + currentValue + "\"");
            }          
        }
                
        private void textBoxValue_Validated(object sender, EventArgs e)
        {
            SetValue(CombineFloats());
        }

        private void textBoxValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetValue(CombineFloats());
            }
        }
    }
}
