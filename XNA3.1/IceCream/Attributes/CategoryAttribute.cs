﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IceCream.Attributes
{
#if XBOX
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class CategoryAttribute : Attribute
    {
        public CategoryAttribute(string val)
        {
        }
    }
#endif
}
