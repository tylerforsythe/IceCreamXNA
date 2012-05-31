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
using System.Xml.Serialization;

namespace IceCream.SceneItems.TileGridClasses
{
    public class TileLayer
    {
        #region Fields

        private String _name;
        private bool _visible;        
        private Tile[][] _tiles;
        internal TileGrid _parent;
        private String _tileData;
        byte _opacity;

        #endregion

        #region Properties
                
        [XmlIgnore]
        public TileGrid Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        // Serializer property used for loading the data on register
        public string TileData 
        {
            get { return _tileData; }
            set { _tileData = value; }
        }

        public Tile[][] Tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }

        public byte Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }

        #endregion

        #region Constructor

        public TileLayer()
        {

        }

        public TileLayer(int numCols, int numRows)
        {
            InitTiles(numCols, numRows);
        }              

        #endregion

        #region Methods

        public void CopyValuesTo(TileLayer target)
        {
            target.Name = this.Name;
            target.Opacity = this.Opacity;
            target.TileData = this.TileData;
            target.Visible = this.Visible;
            // we assume that the layer's array is already
            // correctly sized and initialized
            for (int i = 0; i < this._parent.TileCols; i++)
            {
                for (int j = 0; j < this._parent.TileRows; j++)
                {
                    target.Tiles[i][j] = this.Tiles[i][j];
                }
            }
        }  

        public override string ToString()
        {
            return _name;
        }

        private void InitTiles(int numCols, int numRows)
        {
            // creates a 2d array of tiles
            _tiles = new Tile[numCols][];
            TileData = "";
            for (int x = 0; x < numCols; x++)
            {
                _tiles[x] = new Tile[numRows];
                for (int y = 0; y < numRows; y++)
                {
                    _tiles[x][y] = new Tile(-1);                    
                }
            }            
        }

        public void CreateCollisionDataForTile(int x, int y)
        {
            /* FARSEER 2.0 code to update
            TileSheet tileSheet = this.Parent.TileSheet;
            Tile tile = _tiles[x][y];
            if (tileSheet.EnableCollisionByDefault ^ tile.CollisionMask == true)
            {
                if (tile.Index >= 0 && tileSheet.Polygons.Count > tile.Index)
                {                   
                    Polygon polygon = tileSheet.Polygons[tile.Index];
                    if (polygon != null)
                    {
                        FarseerEntity entity = new FarseerEntity();
                        Vector2 offset = polygon.InitialCentroid;
                        Vertices vertices = polygon.Vertices;
                        if (tile.HFlip == true && tile.VFlip == true)
                        {
                            offset = -offset;
                            vertices = tileSheet.BothFlippedVertices[tile.Index];
                        }
                        else if (tile.HFlip == true)
                        {
                            offset.X = -offset.X;
                            vertices = tileSheet.HFlippedVertices[tile.Index];
                        }
                        else if (tile.VFlip == true)
                        {
                            offset.Y = -offset.Y;
                            vertices = tileSheet.VFlippedVertices[tile.Index];
                        }
                        entity.Offset = offset;
                        entity.InitFromVertices(vertices);                    
                        entity.SetStatic();
                        _tiles[x][y].FarseerEntity = entity;                        
                    }
                }
            }*/
        }

        public void ConstructData()
        {
            _tileData = "";
            for (int y = 0; y < Parent.TileRows; y++)
            {
                for (int x = 0; x < Parent.TileCols; x++)
                {
                    // add a leading comma except for the first record
                    if (x != 0 || y != 0)
                    {
                        _tileData += ",";
                        if (x == 0 && y > 0)
                        {
                            // append a new line character for ease of reading
                            _tileData += Environment.NewLine;
                        }
                    }
                    // store the index
                    _tileData += _tiles[x][y].Index.ToString();
                    // if there is some tile alteration setting
                    if (_tiles[x][y].Rotation > 0 
                        || _tiles[x][y].HFlip == true || _tiles[x][y].VFlip == true)
                    {
                        _tileData += "(";
                        if (_tiles[x][y].Rotation > 0)
                        {
                            TileData += _tiles[x][y].Rotation.ToString();
                        }
                        if (_tiles[x][y].HFlip == true)
                        {
                            _tileData += "H";
                        }
                        if (_tiles[x][y].VFlip == true)
                        {
                            _tileData += "V";
                        }
                        _tileData += ")";
                    }
                }                
            }

        }

        internal void LoadData()
        {            
            int _count = 0;            
            string[] _layerData;           
            _layerData = TileData.Split(',');
            //Kill any new line characters
            for (int i = 0; i < _layerData.Length; i++)
            {
                _layerData[i] = _layerData[i].Replace(Environment.NewLine, "").Trim();
            }
            if (_layerData.Length != _parent.TileCols * _parent.TileRows)
            {
                throw new Exception("The serialized data count (" + _layerData.Length 
                    + ") is different from the grid's tile count (" + (_parent.TileCols * _parent.TileRows) + ")");
            }
            for (int y = 0; y < _parent.TileRows; y++)
            {
                for (int x = 0; x < _parent.TileCols; x++)
                {                                               
                    string _dataItem = _layerData[_count];
                    int _tilenum = -1;
                    bool hflip = false;
                    bool vflip = false;
                    if (_dataItem == null)
                    {
                        //set a default value of blank
                        _dataItem = "-1";
                        _layerData[_count] = "-1";
                    }
                    byte _tileRotation = 0;
                    if (_dataItem.Contains("("))
                    {
                        //get the tile numbe and its rotation
                        _tilenum = int.Parse(_dataItem.Substring(0, _dataItem.IndexOf("(")));
                        if (_dataItem.Contains("H"))
                        {
                            hflip = true;
                            _dataItem = _dataItem.Replace("H", "");
                        }
                        if (_dataItem.Contains("V"))
                        {
                            vflip = true;
                            _dataItem = _dataItem.Replace("V", "");
                        }
                        if (!_dataItem.EndsWith("()"))
                        {
                            _tileRotation = byte.Parse(_dataItem.Substring(_dataItem.IndexOf("(") + 1, 1));
                        }
                    }
                    else
                    {
                        _tilenum = int.Parse(_dataItem); //get the tile number
                    }
                    // New tile
                    Tile _newTile = new Tile(_tilenum);        
                    _newTile.Rotation = _tileRotation;                    
                    _newTile.VFlip = vflip;
                    _newTile.HFlip = hflip;
                    _tiles[x][y] = _newTile;
                    _count++;
                }
            }            
        }

        #endregion        
    }
}
