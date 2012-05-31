#if XNATOUCH
using XnaTouch.Framework;
using XnaTouch.Framework.Audio;
using XnaTouch.Framework.Content;
using XnaTouch.Framework.GamerServices;
using XnaTouch.Framework.Graphics;
using XnaTouch.Framework.Input;
using XnaTouch.Framework.Media;
using XnaTouch.Framework.Net;
using XnaTouch.Framework.Storage;
#else
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using IceCream.Drawing;

namespace IceCream
{
    public class LinearProperty
    {
        #region Fields

        private List<Vector2> _values=new List<Vector2>();
        private String _description;
        private float _defaultValue;
        private int _lowerBound;
        private int _upperBound;

        #endregion

        #region Properties

        public List<Vector2> Values
        {
            get { return _values; }
            set { _values = value; }
        }
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public int LowerBound
        {
            get { return _lowerBound; }
            set { _lowerBound = value; }
        }
        public int UpperBound
        {
            get { return _upperBound; }
            set { _upperBound = value; }
        }

        #endregion

        #region Constructor

        public LinearProperty()            
        {

        }

        public LinearProperty(float defaultValue, String description, int lowerBound, int upperBound)
        {
            this._defaultValue = defaultValue;
            _values = new List<Vector2>();
            _values.Add(new Vector2(0, defaultValue));
            this._description = description;
            this._lowerBound = lowerBound;
            this._upperBound = upperBound;
        }

        #endregion

        #region Methods

        public void CopyValuesTo(LinearProperty linearTarget)
        {            
            linearTarget.Description = this.Description;
            linearTarget.LowerBound = this.LowerBound;
            linearTarget.UpperBound = this.UpperBound;
            // deep copy the values
            linearTarget.Values = new List<Vector2>(this.Values.ToArray());
        }

        public void Reset()
        {
            _values.Clear();
            _values.Add(new Vector2(0, _defaultValue));
        }

        #endregion
    }
}
