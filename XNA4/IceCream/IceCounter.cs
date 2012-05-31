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
using IceCream.Components;
using IceCream.Attributes;
using IceCream;

namespace IceCream
{
    /// <summary>
    /// Defines a counter that you can fill with an integer
    /// and decrease during the logic loop. It acts as a timer if
    /// the logic rate is a fixed value.
    /// </summary>
    public class IceCounter
    {
        #region Fields

        private int _initialValue;
        private int _currentValue;
        private int _decreaseStep;

        #endregion

        #region Properties

        public int InitialValue
        {
            get { return _initialValue; }           
        }
        public int CurrentValue
        {
            get { return _currentValue; }          
        }
        public int DecreaseStep
        {
            get { return _decreaseStep; }
            set { _decreaseStep = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Set the counter's value
        /// </summary>
        /// <param name="count">The initial value</param>
        public IceCounter(int count)
        {
            _initialValue = _currentValue = count;
            _decreaseStep = 1;
        }

        /// <summary>
        /// Set the counter's value and empty it if needed
        /// </summary>
        public IceCounter(int count, bool empty)
        {
            _initialValue = _currentValue = count;
            _decreaseStep = 1;
            if (empty == true)
            {
                _currentValue = 0;
            }
        }

        /// <summary>
        /// Set the counter and the decrease's step values
        /// </summary>
        /// <param name="count">The initial value</param>
        /// <param name="step">The default step used by the Decrement() method</param>
        public IceCounter(int count, int step)
        {
            _initialValue = _currentValue = count;
            _decreaseStep = step;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Decrement the counter's current value with the default step value
        /// </summary>
        /// <returns>True if the counter has reached 0, else returns false</returns>
        public bool Decrement()
        {
            _currentValue -= _decreaseStep;
            if (_currentValue <= 0)
            {
                _currentValue = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Decrement the counter's current value with the specified step value
        /// </summary>
        /// <param name="step">The desired step value</param>
        /// <returns>True if the counter has reached 0, else returns false</returns>
        public bool Decrement(int step)
        {
            _currentValue -= step;
            if (_currentValue <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the Current Value to 0
        /// </summary>
        public void Empty()
        {
            _currentValue = 0;
        }

        /// <summary>
        /// Reset the counter to it's initial value
        /// </summary>
        public void Reset()
        {
            _currentValue = _initialValue;
        }

        /// <summary>
        /// Reset the counter and change its initial value
        /// </summary>
        public void ResetTo(int newInitialValue)
        {
            _currentValue = _initialValue = newInitialValue;
        }

        /// <summary>
        /// Check if the current value is zero
        /// </summary>
        /// <returns>True if zero, false for any other value</returns>
        public bool IsZero()
        {
            if (_currentValue == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}

