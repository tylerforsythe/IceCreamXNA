using System;
using System.Collections.Generic;
using System.Text;
using IceCream;
using IceCream.SceneItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IceCream.SceneItems.TileGridClasses;

namespace Milkshake.Editors.TileGrids
{
    public struct TileCopy
    {
        public Tile Tile
        {
            get;
            set;
        }
        public Point Displacement
        {
            get;
            set;
        }
    }
}
