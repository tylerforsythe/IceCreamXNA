using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Milkshake.Editors.Components
{
    public delegate void ValueChangedEventHandler(object sender, EventArgs e);
    public class AutoControl : UserControl
    {
        private bool _initialized;
        private String _previousValue;
        private String _defaultValue;
        public ValueChangedEventHandler ValueChanged;

        protected int LabelPadding
        {
            get { return 0; }
        }

        public AutoControl()
        {

        }

        public AutoControl(String defaultValue)
        {
            _initialized = false;
            _defaultValue = defaultValue;
        }

        public virtual void SetFriendlyName(String value)
        {

        }

        public virtual void SetFriendlyNameWidth(int width)
        {

        }

        public virtual void SetValue(String value)
        {
            if (_initialized == true && _previousValue != null && _previousValue != value)
            {
                MilkshakeForm.Instance.SceneWasModified = true;                
            }
            _initialized = true;
            _previousValue = value;
        }

        protected virtual bool IsValueValid(string value)
        {
            return true;
        }

        protected virtual void RestoreValue(String errorMessage)
        {
            if (_initialized)
            {
                SetValue(_previousValue);
                MilkshakeForm.ShowErrorMessage(errorMessage);
            }
            else
            {
                SetValue(_defaultValue);
            }
        }

        public virtual object GetValue()
        {
            return null;
        }

        protected void OnValueChanged(EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            }
        }        
    }
}
