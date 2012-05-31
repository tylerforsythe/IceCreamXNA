using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IceCream.Attributes
{
#if (XBOX || WINDOWS_PHONE)
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class BrowsableAttribute : Attribute
    {
        public BrowsableAttribute(bool val)
        {
        }
    }
#endif
}
