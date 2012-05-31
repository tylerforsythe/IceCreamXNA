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
using IceCream.Attributes;

namespace IceCream.Components
{
#if(DEBUG)
    public enum TestEnum
    {
        EnumItem1,
        EnumItem2,
        EnumItem3,
        EnumItem4,
    }

    [IceComponentAttribute("TestComponent")]
    public class TestComponent:IceComponent
    {
        #region Properties

        [IceComponentProperty("Test Boolean", "true")]
        public bool TestBool
        {
            get;
            set;
        }

        [IceComponentProperty("Test Integer", "0")]
        public int TestInt
        {
            get;
            set;
        }

        [IceComponentProperty("Test Float", "0")]
        public float TestFloat
        {
            get;
            set;
        }

        [IceComponentProperty("Test Vector2")]
        public Vector2 TestVector2
        {
            get;
            set;
        }

        [IceComponentProperty("Test Point", "0")]
        public Point TestPoint
        {
            get;
            set;
        }

        [IceComponentProperty("Test String", "test string :)")]
        public String TestString
        {
            get;
            set;
        }

        [IceComponentProperty("Test Rectangle", "")]
        public Rectangle TestRectangle
        {
            get;
            set;
        }

        [IceComponentProperty("Test Nullable Rectangle", "")]
        public Rectangle? TestNullRectangle
        {
            get;
            set;
        }

        [IceComponentProperty("Test Enum", "2")]
        public TestEnum TestEnum
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public override void OnRegister()
        {
            Enabled = true;
        }

        public override void Update(float elapsedTime)
        {
            
        }

        #endregion
    }
#endif
}
