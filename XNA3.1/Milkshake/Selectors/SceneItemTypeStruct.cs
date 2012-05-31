using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Milkshake.Selectors
{
    internal struct SceneItemTypeStruct
    {
        internal SceneItemType type;
        internal String name;
        internal String pluralName;
        internal String icon;
        internal Bitmap image;
        internal bool requiresTemplate;

        internal SceneItemTypeStruct(SceneItemType type, String name, String pluralName, String icon, Bitmap image)
        {
            this.type = type;
            this.name = name;
            this.pluralName = pluralName;
            this.icon = icon;
            this.image = image;
            this.requiresTemplate = false;
        }

        internal SceneItemTypeStruct(SceneItemType type, String name, String pluralName, String icon, Bitmap image, bool requiresTemplates)
        {
            this.type = type;
            this.name = name;
            this.pluralName = pluralName;
            this.icon = icon;
            this.image = image;
            this.requiresTemplate = requiresTemplates;
        }
    }
}
