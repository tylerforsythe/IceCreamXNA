using System;
using System.Collections.Generic;
using System.Text;

namespace IceCream
{
    public interface IDeepCopy
    {
        void CopyValuesTo(object target);
    }
}
