#if WINDOWS

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.ComponentModel;
using System.Windows.Forms.Design;

namespace IceCream.SceneItems.CompositeEntityClasses
{
    // SOURCE: http://www.codeproject.com/KB/cpp/dropdownproperties.aspx
    public class SubItemRefConverter : StringConverter
    {
        public static List<String> SubItemsRefs = new List<String>();

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            //true means show a combobox

            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            //true will limit to list. false will show the list, 
            //but allow free-form entry
            return false;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection
               GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(SubItemRefConverter.SubItemsRefs);
        }
    }
}

#endif
