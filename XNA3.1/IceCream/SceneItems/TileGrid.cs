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

namespace IceCream.SceneItems
{
    public class TileGrid : Sprite
    {
        #region Fields

        private int _tileCols;
        private int _tileRows;
        private Point _tileSize;
        private List<TileLayer> _layers;
        private Rectangle _visibleTiles;
        private bool _visibilityChanged;
        private bool _positionChanged;
        private int _columnsCountOnTexture;
        private Matrix _rotationMatrix;
        private Vector2 _tilePivot;

        private Vector2 _scaledTileSize;
        private Vector2 _tileScale;
        private Vector2 _rotatedScale;
        private Vector2 _startDrawPos;
        private Vector2 _scaledPivot;
        private Vector2 _scaledTilePivot;
        
        #endregion

        #region Properties

        public int TileCols
        {
            get { return _tileCols; }
            set 
            {
                if (_tileCols != value)
                {
                    ResizeTileGrid(value, TileRows);                    
                }
            }
        }

        public int TileRows
        {
            get { return _tileRows; }
            set 
            {
                if (_tileRows != value)
                {
                    ResizeTileGrid(TileCols, value);
                }
            }
        }

        public Point TileSize
        {
            get { return _tileSize; }
            set 
            { 
                _tileSize = value;
                // store the Pivot of a single tile
                _tilePivot = new Vector2(_tileSize.X / 2.0f,
                    _tileSize.Y / 2.0f);               
            }
        }

        public List<TileLayer> TileLayers
        {
            get { return _layers; }
            set { _layers = value; }
        }

        public override float Rotation
        {
            get { return base.Rotation; }
            set 
            {
                if (this.Rotation != value)
                {
                    base.Rotation = value;
                    _visibilityChanged = true;
                }
            }
        }

        public override Material Material
        {
            get
            {
                return base.Material;
            }
            set
            {
                if (base.Material != value)
                {
                    base.Material = value;
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

        public override Vector2 BoundingRectSize
        {
            get
            {
                return new Vector2(TileSize.X * Scale.X * TileCols, 
                    TileSize.Y * Scale.Y * TileRows);
            }
            set
            {
                base.BoundingRectSize = value;
            }
        }

        public TileSheet TileSheet
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        public TileGrid()
        {
            this.UseTilingSafeBorders = true;
            _layers = new List<TileLayer>();
            _visibilityChanged = true;
            this.Scale = Vector2.One;
            this.TileSheet = null;
        }

        #endregion

        #region Methods

        public override void CopyValuesTo(SceneItem target)
        {
            base.CopyValuesTo(target);
            TileGrid tileGrid = target as TileGrid;
            tileGrid.TileSize = this.TileSize;
            tileGrid.TileCols = this.TileCols;           
            tileGrid.TileRows = this.TileRows;
            tileGrid.TileLayers = new List<TileLayer>();
            for (int i = 0; i < this.TileLayers.Count; i++)
            {
                TileLayer newLayer = new TileLayer(this.TileCols, this.TileRows);
                newLayer.Parent = tileGrid;
                this.TileLayers[i].CopyValuesTo(newLayer);
                tileGrid.TileLayers.Add(newLayer);
            }
            tileGrid.TileSheet = this.TileSheet;
        }

        internal override void OnRegister()
        {   
            LoadData(true);
            base.OnRegister();
        }

        public override void Update(float elapsed)
        {
            _visibilityChanged = true;
            if (_visibilityChanged == true)
            {
                DetermineVisibility();
            }
            _scaledTileSize = new Vector2((float)TileSize.X * Scale.X, (float)TileSize.Y * Scale.Y);
            _tileScale = new Vector2(_tileSize.X / (float)_tileSize.Y, _tileSize.Y / (float)_tileSize.X);
            _rotatedScale = new Vector2(Scale.Y * _tileScale.Y, Scale.X * _tileScale.X);
            _startDrawPos = this.Position;
            _scaledPivot = this.Pivot;
            if (this.IsPivotRelative == true)
            {
                _scaledPivot = new Vector2(this.Pivot.X * BoundingRectSize.X,
                        this.Pivot.Y * BoundingRectSize.Y);
            }
            _scaledPivot *= this.Scale;
            if (this.Rotation != 0)
            {
                _scaledPivot = Vector2.Transform(_scaledPivot, _rotationMatrix);
            }
            _scaledTilePivot = _tilePivot * this.Scale;
            _positionChanged = true;
            if (_positionChanged == true && this.TileSheet != null && this.TileSheet.Polygons.Count > 0)
            {
                for (int i = TileLayers.Count - 1; i >= 0; i--)
                {
                    TileLayer layer = TileLayers[i];
                    for (int y = _visibleTiles.Top; y < _visibleTiles.Bottom; y++)
                    {
                        for (int x = _visibleTiles.Left; x < _visibleTiles.Right; x++)
                        {
                            Tile tile = layer.Tiles[x][y];
                            if (tile.Index != -1)
                            {
                                Vector2 drawPos = _scaledTilePivot;
                                drawPos.X += (float)x * _scaledTileSize.X;
                                drawPos.Y += (float)y * _scaledTileSize.Y;
                                if (this.Rotation != 0)
                                {
                                    drawPos = Vector2.Transform(drawPos, _rotationMatrix);
                                }
                                drawPos -= _scaledPivot;
                                /*  FARSEER
                                if (tile.FarseerEntity != null)
                                {
                                    layer.Tiles[x][y].FarseerEntity.Position = this.Position + drawPos;
                                }*/
                            }
                        }
                    }
                }
            }
            base.Update(elapsed);            
        }

        /// <summary>
        /// This function determines which tiles are visible on the screen,
        /// given the current camera position, rotation, zoom, and tile scale
        /// </summary>
        private void DetermineVisibility()
        {
            if (this.SceneParent == null)
            {
                return;
            }
            _rotationMatrix = Matrix.CreateRotationZ(this.Rotation);

            _visibleTiles.X = 0;
            _visibleTiles.Y = 0;
            _visibleTiles.Width = TileCols;
            _visibleTiles.Height = TileRows;
            _visibilityChanged = false;
            return;

            /*
            
            //create the view rectangle
            Vector2 upperLeft = SceneManager.GlobalDataHolder.ScreenResolution / 2 - 
                (SceneManager.GlobalDataHolder.ScreenResolution  / SceneParent.ActiveCameras[0].Zoom.X);
                            
            Vector2 upperRight =new Vector2(
                SceneManager.GlobalDataHolder.ScreenResolution.X/2,
                -SceneManager.GlobalDataHolder.ScreenResolution.Y/2);
            upperRight.X += (SceneManager.GlobalDataHolder.ScreenResolution.X / SceneParent.ActiveCameras[0].Zoom.X);
            upperRight.Y -= (SceneManager.GlobalDataHolder.ScreenResolution.Y / SceneParent.ActiveCameras[0].Zoom.Y);
            Vector2 lowerLeft = Vector2.Zero;
            Vector2 lowerRight = Vector2.Zero;
            Vector2 displaySize = SceneManager.GlobalDataHolder.ScreenResolution / SceneParent.ActiveCameras[0].Zoom.X;
            Vector2 cameraPostionValue = SceneParent.ActiveCameras[0].Position;
            float zoomValue = SceneParent.ActiveCameras[0].Zoom.X;
            
            lowerRight.X = ((displaySize.X/2 ) / zoomValue);
            lowerRight.Y = ((displaySize.Y / 2) / zoomValue);
            upperRight.X = lowerRight.X;
            upperRight.Y = -lowerRight.Y;
            lowerLeft.X = -lowerRight.X;
            lowerLeft.Y = lowerRight.Y;
            upperLeft.X = -lowerRight.X;
            upperLeft.Y = -lowerRight.Y;


            //rotate the view rectangle appropriately
            //Vector2.Transform(ref upperLeft, ref rotationMatrix, out upperLeft);
            //Vector2.Transform(ref lowerRight, ref rotationMatrix, out lowerRight);
            //Vector2.Transform(ref upperRight, ref rotationMatrix, out upperRight);
            //Vector2.Transform(ref lowerLeft, ref rotationMatrix, out lowerLeft);

            lowerLeft += (cameraPostionValue);
            lowerRight += (cameraPostionValue);
            upperRight += (cameraPostionValue);
            upperLeft += (cameraPostionValue);


            Vector2 worldOffset = Position;
            //the idea here is to figure out the smallest square
            //(in tile space) that contains tiles
            //the offset is calculated before scaling
            float top = MathHelper.Min(
                MathHelper.Min(upperLeft.Y, lowerRight.Y),
                MathHelper.Min(upperRight.Y, lowerLeft.Y)) -
                worldOffset.Y;

            float bottom = MathHelper.Max(
                MathHelper.Max(upperLeft.Y, lowerRight.Y),
                MathHelper.Max(upperRight.Y, lowerLeft.Y)) -
                worldOffset.Y;
            float right = MathHelper.Max(
                MathHelper.Max(upperLeft.X, lowerRight.X),
                MathHelper.Max(upperRight.X, lowerLeft.X)) -
                worldOffset.X;
            float left = MathHelper.Min(
                MathHelper.Min(upperLeft.X, lowerRight.X),
                MathHelper.Min(upperRight.X, lowerLeft.X)) -
                worldOffset.X;

            int cellWidth = TileSize.X;
            int cellHeight= TileSize.Y;

            //now figure out where we are in the tile sheet
            float scaledTileWidth = (float)cellWidth;
            float scaledTileHeight = (float)cellHeight;

            //get the visible tiles
            _visibleTiles.X = (int)(left / (scaledTileWidth));
            _visibleTiles.Y = (int)(top / (scaledTileWidth));

            //get the number of visible tiles
            _visibleTiles.Height =
                (int)((bottom) / (scaledTileHeight)) - _visibleTiles.Y + 1;
            _visibleTiles.Width =
                (int)((right) / (scaledTileWidth)) - _visibleTiles.X + 1;

            //clamp the "upper left" values to 0
            if (_visibleTiles.X < 0) _visibleTiles.X = 0;
            if (_visibleTiles.X > (TileCols - 1)) _visibleTiles.X = TileCols;
            if (_visibleTiles.Y < 0) _visibleTiles.Y = 0;
            if (_visibleTiles.Y > (TileRows - 1)) _visibleTiles.Y = TileRows;


            //clamp the "lower right" values to the gameboard size
            if (_visibleTiles.Right > (TileCols - 1))
                _visibleTiles.Width = (TileCols - _visibleTiles.X);

            if (_visibleTiles.Right < 0) _visibleTiles.Width = 0;

            if (_visibleTiles.Bottom > (TileRows- 1))
                _visibleTiles.Height = (TileRows - _visibleTiles.Y);

            if (_visibleTiles.Bottom < 0) _visibleTiles.Height = 0;
            */            
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
                tilePosOnTexture.X += 2 * tilePos.Y + 1;
                tilePosOnTexture.Y += 2 * tilePos.X + 1;
                return new Rectangle(tilePosOnTexture.X, tilePosOnTexture.Y, TileSize.X, TileSize.Y);
            }
            return new Rectangle(tilePosOnTexture.X, tilePosOnTexture.Y,TileSize.X, TileSize.Y);
        }

        public override void Draw(float elapsed)
        {
            if (!this.Visible)
            {
                return;
            }
            
            // Draw in reverse order from the list
            for (int i = TileLayers.Count - 1; i >= 0; i--)
            {
                TileLayer layer = TileLayers[i];
                if (!layer.Visible)
                {
                    continue;
                }
                for (int y = _visibleTiles.Top; y < _visibleTiles.Bottom; y++)
                {
                    for (int x = _visibleTiles.Left; x < _visibleTiles.Right; x++)
                    {
                        Tile tile = layer.Tiles[x][y];
                        if (tile.Index != -1)
                        {
                            Vector2 drawPos = _scaledTilePivot;
                            drawPos.X += (float)x * _scaledTileSize.X;
                            drawPos.Y += (float)y * _scaledTileSize.Y;
                            if (this.Rotation != 0)
                            {
                                drawPos = Vector2.Transform(drawPos, _rotationMatrix);
                            }
                            drawPos -= _scaledPivot;

                            DrawRequest _drawRequest = new DrawRequest();
                            _drawRequest.texture = this.Material.Texture;
                            _drawRequest.position = this.Position + drawPos;
                            _drawRequest.rotation = tile.Rotation * MathHelper.PiOver2 + this.Rotation;
                            _drawRequest.pivot = _tilePivot;
                            _drawRequest.isPivotRelative = false;
                            _drawRequest.hFlip = tile.HFlip;
                            _drawRequest.vFlip = tile.VFlip;
                            _drawRequest.sourceRectangle = GetSourceRectOfTile(tile.Index);
                            
                            if (tile.Rotation == 1 || tile.Rotation == 3)
                            {
                                _drawRequest.scaleRatio = _rotatedScale;
                            }
                            else
                            {
                                _drawRequest.scaleRatio = Scale;
                            }                            
                            _drawRequest.tint = this.Tint;
                            DrawingManager.DrawOnLayer(_drawRequest, this.Layer, this.BlendingType);
                        }
                    }
                }
            }
        }

        //Loads all the layers data from its serialized string of data
        public void LoadData(bool initCollision)
        {
            if (this.TileSheet != null)
            {
                this.Material = this.TileSheet.Material;
                this.TileSize = this.TileSheet.TileSize;
                //this.TileSheet.CreateDefaultPolygons();
            }
            foreach (TileLayer layer in TileLayers)
            {
                layer._parent = this;
                if (!string.IsNullOrEmpty(layer.TileData))
                {
                    layer.LoadData();
                }
                if (this.TileSheet != null && initCollision == true)
                {
                    CreateCollisionEntitiesForLayer(layer);
                }
            }
        }

        private void CreateCollisionEntitiesForLayer(TileLayer layer)
        {
            for (int i = 0; i < _tileCols; i++)
            {
                for (int j = 0; j < _tileRows; j++)
                {
                    layer.CreateCollisionDataForTile(i, j);
                }
            }
        }

        private void ResizeTileGrid(int columns, int rows)
        {
            if (this._tileCols != columns || this._tileRows != rows)
            {                                  
                for (int t = 0; t < TileLayers.Count; t++)
                {
                    Tile[][] newTiles = new Tile[columns][];
                    for (int i = 0; i < columns; i++)
                    {
                        newTiles[i] = new Tile[rows];
                        for (int j = 0; j < rows; j++)
                        {
                            if (i < this.TileCols && j < this.TileRows)
                            {
                                newTiles[i][j] = this.TileLayers[t].Tiles[i][j];
                            }
                            else
                            {
                                newTiles[i][j] = new Tile(-1);
                            }
                        }
                    }
                    this._layers[t].Tiles = newTiles;
                }
                _visibilityChanged = true;
                this._tileCols = columns;
                this._tileRows = rows;
            }
        }
        
        #endregion
    }   
}
