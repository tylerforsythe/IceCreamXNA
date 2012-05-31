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
    public class SceneItemRefConverter : StringConverter
    {
        private static List<String> sceneItemsRefs = new List<String>();
        public static void UpdateListSceneItemsRefs(Dictionary<String, SceneItem>.KeyCollection keys)
        {
            sceneItemsRefs.Clear();
            foreach (String key in keys)
            {
                sceneItemsRefs.Add(key);
            }
        }

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
            return new StandardValuesCollection(SceneItemRefConverter.sceneItemsRefs);
        }

    }
}

#endif
