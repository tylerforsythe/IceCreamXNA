using System;
using System.Collections.Generic;
using System.Text;

namespace IceCream.SceneItems
{
    public interface ISubItemCollection
    {
        List<String> GetSubItemsList();
        string GetCurrentSubItem();
        void SetCurrentSubItem(string subItem);
    }
}
