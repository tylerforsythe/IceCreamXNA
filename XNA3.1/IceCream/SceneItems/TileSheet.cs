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
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using IceCream.SceneItems;
using IceCream.Drawing;
using IceCream.SceneItems.TileGridClasses;

namespace IceCream
{
    public class TileSheet : IceAsset
    {
        #region Fields

        private Material _material;
        private Point _tileSize;
        private int _columnsCountOnTexture = -1;
        private int _rowsCountOnTexture = -1;  
        
        #endregion

        #region Properties

        public List<Polygon> Polygons
        {
            get;
            set;
        }
        /* FARSEER 2.0 code to update to 3.0
        internal List<Vertices> HFlippedVertices
        {
            get;
            set;
        }

        internal List<Vertices> VFlippedVertices
        {
            get;
            set;
        }

        internal List<Vertices> BothFlippedVertices
        {
            get;
            set;
        }*/

        public virtual bool UseTilingSafeBorders
        {
            get;
            set;
        }

        public bool EnableCollisionByDefault
        {
            get;
            set;
        }

        public Point TileSize
        {
            get { return _tileSize; }
            set 
            { 
                _tileSize = value;             
            }
        }

        public Material Material
        {
            get
            {
                return _material;
            }
            set
            {
                if (_material != value)
                {
                    _material = value;
                    // reset the count of columns
                    _columnsCountOnTexture = 0;
                }
            }
        }

        public int ColumnsCountOnTexture
        {
            get 
            {
                // update the columns count if needed
                if (_columnsCountOnTexture < 1)
                {
                    UpdateColumnsCountFromTexture();
                }
                return _columnsCountOnTexture; 
            }
            set { _columnsCountOnTexture = value; }
        }

        public int RowsCountOnTexture
        {
            get
            {
                // update the rows count if needed
                if (_rowsCountOnTexture < 1)
                {
                    UpdateRowsCountFromTexture();
                }
                return _rowsCountOnTexture;
            }
            set { _rowsCountOnTexture = value; }
        }

        #endregion

        #region Constructor

        public TileSheet()
        {
            this.UseTilingSafeBorders = true;
            this.Polygons = new List<Polygon>();
            /*
            this.VFlippedVertices = new List<Vertices>();
            this.HFlippedVertices = new List<Vertices>();
            this.BothFlippedVertices = new List<Vertices>();*/
        }

        #endregion

        #region Methods

        public void CopyValuesTo(TileSheet tileSheet)
        {
            tileSheet.TileSize = this.TileSize;   
        }

        public void CreateDefaultPolygons()
        {
            /* FARSEER
            this.Polygons.Clear();
            for (int x = 0; x < this.ColumnsCountOnTexture; x++)
            {
                for (int y = 0; y < this.RowsCountOnTexture; y++)
                {
                    this.Polygons.Add(                        
                            Polygon.FromRectangle(new Rectangle(-1, -1, this.TileSize.X + 2, this.TileSize.Y + 2)
                        ));
                    Console.WriteLine("Poly: [" + this.Polygons[this.Polygons.Count - 1].ToString() + "]");
                }
            }*/
        }

        public void CreateFlippedPolygons()
        {
            /* FARSEER 
            if (this.Polygons != null)
            {
                foreach (Polygon poly in this.Polygons)
                {
                    if (poly != null)
                    {
                        this.HFlippedVertices.Add(poly.GetHFlipVertices());
                        this.VFlippedVertices.Add(poly.GetVFlipVertices());
                        this.BothFlippedVertices.Add(poly.GetBothFlipVertices());
                    }
                    else
                    {
                        this.HFlippedVertices.Add(null);
                        this.VFlippedVertices.Add(null);
                        this.BothFlippedVertices.Add(null);
                    }
                }
            }*/
        }

        private void UpdateColumnsCountFromTexture()
        {
            if (this.Material != null && this.Material.Texture != null)
            {
                //Get number of tiles columns in the source texture
                int textureSize = this.Material.Texture.Width;
                if (this.UseTilingSafeBorders)
                {
                    _columnsCountOnTexture = (textureSize + 2) / (_tileSize.X + 2);
                }
                else
                {
                    _columnsCountOnTexture = textureSize / _tileSize.X;
                }
                if (_columnsCountOnTexture < 1)
                {
                    throw new Exception("The tiles columns count of a texture cannot be 0");
                }
            }            
        }

        private void UpdateRowsCountFromTexture()
        {
            if (this.Material != null && this.Material.Texture != null)
            {
                // get number of tiles rows in the source texture
                int textureSize = this.Material.Texture.Height;
                if (this.UseTilingSafeBorders)
                {
                    _rowsCountOnTexture = (textureSize + 2) / (_tileSize.Y + 2);

                }
                else
                {
                    _rowsCountOnTexture = textureSize / _tileSize.Y;
                }
                if (_rowsCountOnTexture < 1)
                {
                    throw new Exception("The tiles rows count of a texture cannot be 0");
                }
            }
        }

        public Rectangle GetSourceRectOfTile(int tileIndex)
        {            
            Point tilePos = new Point();
            tilePos.X = (int)Math.Floor(tileIndex / (double)ColumnsCountOnTexture);
            tilePos.Y = (int)(tileIndex % ColumnsCountOnTexture);
            Point tilePosOnTexture = new Point();
            tilePosOnTexture.Y = TileSize.Y * tilePos.X;
            tilePosOnTexture.X = TileSize.X * tilePos.Y;
            if (this.UseTilingSafeBorders)
            {
                tilePosOnTexture.X += 2 * tilePos.Y;
                tilePosOnTexture.Y += 2 * tilePos.X;
                return new Rectangle(tilePosOnTexture.X, tilePosOnTexture.Y, TileSize.X, TileSize.Y);
            }
            return new Rectangle(tilePosOnTexture.X, tilePosOnTexture.Y,TileSize.X, TileSize.Y);
        }
  
        #endregion
    }   
}
