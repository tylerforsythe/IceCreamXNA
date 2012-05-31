using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using IceCream.Components;
using System.Reflection;
using IceCream.Attributes;
using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Milkshake.Editors.Components
{
    public partial class GenericComponentControl : ScrollableControl
    {
        #region Fields

        private IceComponent _component = null;
        private int _lastTop = 0;
        private int _padding = 5;

        #endregion

        #region Properties

        internal IceComponent SelectedComponent
        {
            get { return _component; }
            set 
            { 
                _component = value;
                if (value != null)
                {
                    BuildForm();
                }
                else
                {
                    this.SuspendLayout();
                    this.Controls.Clear();
                    this.ResumeLayout();
                }
            }
        }

        #endregion        

        #region Constructors

        public GenericComponentControl()
        {
            InitializeComponent();         
        }

        #endregion

        #region Methods

        public void SetTextMargin()
        {
            int maxWidth = 0;
            if (_component != null)
            {
                foreach (PropertyInfo _info in _component.GetType().GetProperties())
                {
                    if (_info.IsDefined(typeof(IceComponentPropertyAttribute), true))
                    {
                        object[] attributes =
                       _info.GetCustomAttributes(
                            typeof(IceComponentPropertyAttribute), false);
                        IceComponentPropertyAttribute attribute = attributes[0] as IceComponentPropertyAttribute;
                        Font font = this.Font;
                        int textWidth = 2 + (int)this.CreateGraphics().MeasureString(attribute.FriendlyName, font).Width;
                        // keep the biggest string's width
                        if (textWidth > maxWidth)
                        {
                            maxWidth = textWidth;
                        }
                    }
                }
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    AutoControl control = this.Controls[i] as AutoControl;
                    control.SetFriendlyNameWidth(maxWidth);
                }
            }
        }

        public void BuildForm()
        {
            _lastTop = _padding;
            this.SuspendLayout();
            this.Enabled = false;
            this.Controls.Clear();           
            foreach (PropertyInfo _info in _component.GetType().GetProperties())
            {
                if (_info.IsDefined(typeof(IceComponentPropertyAttribute), true))
                {                    
                    AddNewControl(_info);
                }
            }
            SetTextMargin();
            this.Enabled = true;
            this.ResumeLayout();
        }        

        private void AddNewControl(PropertyInfo _info)
        {
            AutoControl control = null;
            control = CreateCustomAutoControlInstance(_info);
            if (control != null)
            {
                object[] attributes;
                attributes =
                   _info.GetCustomAttributes(
                        typeof(IceComponentPropertyAttribute), false);
                IceComponentPropertyAttribute attribute = attributes[0] as IceComponentPropertyAttribute;
                object value = _info.GetValue(_component, null);
                control.SetFriendlyName(attribute.FriendlyName);
                if (value == null)
                {
                    if (attribute.DefaultValue != null)
                    {
                        control.SetValue(attribute.DefaultValue);
                    }
                    else
                    {                        
                        control.SetValue(null);
                    }
                }
                else
                {
                    // special case for enums, convert to int first
                    if (value is Enum)
                    {
                        value = (int)value;
                    }                    
                    control.SetValue(value.ToString());
                }
                //control.BackColor = Color.RoyalBlue;
                control.Location = new System.Drawing.Point(0, _lastTop);
                control.Size = new Size(this.Size.Width, control.Size.Height);
                control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                control.Tag = _info;
                control.ValueChanged += ValueChangedEvent;
                this.Controls.Add(control);
                _lastTop += control.Height + _padding;
            }
        }

        private AutoControl CreateCustomAutoControlInstance(PropertyInfo _info)
        {        
            object valueObject = _info.GetValue(_component, null);
            if (_info.PropertyType == typeof(float))
            {
                return new AutoControlFloat();               
            }
            else if (_info.PropertyType == typeof(bool))
            {
                return new AutoControlBool();
            }
            else if (_info.PropertyType == typeof(int))
            {
                return new AutoControlInt();
            }
            else if (_info.PropertyType == typeof(byte))
            {
                return new AutoControlByte();
            }
            else if (_info.PropertyType == typeof(Vector2))
            {
                return new AutoControlVector2();
            }
            else if (_info.PropertyType == typeof(Point))
            {
                return new AutoControlPoint();
            }
            else if (_info.PropertyType == typeof(Rectangle))
            {
                return new AutoControlRectangle();
            }
            else if (_info.PropertyType == typeof(String))
            {
                return new AutoControlString();
            }
            else if (valueObject is Enum)
            {
                String[] names = Enum.GetNames(_info.PropertyType);
                if (names.Length < 1)
                {
                    return null;
                }
                else
                {
                    AutoControlEnum enumControl = new AutoControlEnum();
                    enumControl.SetComboBoxValues(names);
                    return enumControl;
                }
            }
            return null;
        }

        #endregion

        #region Events

        private void ValueChangedEvent(object sender, EventArgs e)
        {
            AutoControl control = sender as AutoControl;
            PropertyInfo prop = control.Tag as PropertyInfo;
            object valueObject = prop.GetValue(_component, null);
            object newValue = control.GetValue();
            // special case for Enum, we need to convert back the int value to object
            if (valueObject is Enum)
            {
                newValue = Enum.ToObject(prop.PropertyType, newValue);
            }
            // if the value has really been changed, modify the component
            if (newValue.Equals(prop.GetValue(_component, null)) == false)
            {
                Console.WriteLine("Value of prop \"" + prop.Name + "\" has changed to " + newValue.ToString());
                prop.SetValue(_component, newValue, null);
            }
        }

        #endregion

        private void GenericComponentControl_Resize(object sender, EventArgs e)
        {
            SetTextMargin();
        }
    }
}
