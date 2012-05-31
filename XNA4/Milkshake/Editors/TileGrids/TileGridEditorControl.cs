using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IceCream;
using IceCream.Drawing;
using Milkshake.GraphicsDeviceControls;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;

namespace Milkshake.Editors.TileGrids
{
    using Color = Microsoft.Xna.Framework.Color;
    using IceCream.SceneItems;

    public enum TileGridPaintMode
    {
        Edit,
        Brush,
        Bucket,
        Eraser,
    }

    public partial class TileGridEditorControl : GraphicsDeviceControl
    {
        private SpriteBatch _spriteBatch;
        private TileGrid _tileGrid;
        internal TileGrid TileGrid
        {
            get { return _tileGrid; }
            set { _tileGrid = value; }
        }
        private TileGridEditor _parent;
        internal TileGridEditor ParentEditor
        {
            get { return _parent; }
            set { _parent = value; }
        }
        private TileGridPaintMode _paintMode;
        internal TileGridPaintMode PaintMode
        {
            get { return _paintMode; }
            set { _paintMode = value; }
        }
        private Texture2D _checkerTexture;
        private Texture2D _crossTexture;
        private bool _drawGrid = true;
        internal bool DrawGrid
        {
            get { return _drawGrid; }
            set { _drawGrid = value; }
        }
        private Point _brushStartPoint;
        internal Point BrushStartPoint
        {
            get { return _brushStartPoint; }
            set { _brushStartPoint = value; }
        }
        private Point _brushSize;
        private Point _brushSizeExpandOrigin;
        private List<Point> _selectedTiles;
        internal List<Point> SelectedTiles
        {
            get { return _selectedTiles; }
            set { _selectedTiles = value; }
        }
        private Point _selectionExpandOrigin;
        internal Point SelectionExpandOrigin
        {
            get { return _selectionExpandOrigin; }
            set { _selectionExpandOrigin = value; }
        }
        private bool _isSelectingArea;
        internal bool IsSelectingArea
        {
            get { return _isSelectingArea; }
            set { _isSelectingArea = value; }
        }
        private List<TileCopy> _clipboardTiles;
        private bool _isPastingTiles;
        private Point _drawPosPastingSelection;
        
        protected override void Initialize()
        {                      
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _checkerTexture = Texture2D.FromStream(GraphicsDevice, System.IO.File.OpenRead(Application.StartupPath + "\\Resources\\checker.png"));
            _crossTexture = Texture2D.FromStream(GraphicsDevice, System.IO.File.OpenRead(Application.StartupPath + "\\Resources\\cross.png"));          
            this.VScroll = true;
            this.HScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(0, 0);

            _drawGrid = true;
            _paintMode = TileGridPaintMode.Brush;
            _brushStartPoint = new Point(4, 4);
            _brushSize = new Point(1, 1);
            _brushSizeExpandOrigin = new Point(-1, -1);
            _selectedTiles = new List<Point>();
            _selectionExpandOrigin = new Point(-1, -1);
            _isSelectingArea = false;
            _clipboardTiles = new List<TileCopy>();
            _isPastingTiles = false;
            _drawPosPastingSelection = new Point(0, 0);
            InitializeTileGrid();
        }

        #region Methods

        protected void InitializeTileGrid()
        {
            _tileGrid.Position = Vector2.Zero;
        }

        protected void Update(float elapsedTime)
        {
            TileGrid.Update(elapsedTime);
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                if (PaintMode == TileGridPaintMode.Brush || PaintMode == TileGridPaintMode.Eraser)
                {
                    if (_brushSizeExpandOrigin.X == -1 || _brushSizeExpandOrigin.Y == -1)
                    {
                        _brushSizeExpandOrigin = _brushStartPoint;
                        _brushSize = new Point(1, 1);
                    }
                }
            }
            else
            {
                _brushSizeExpandOrigin = new Point(-1, -1);                
            }            
        }

        protected Point GetTileRelativeMousePos(int x, int y)
        {
            x += (int)ParentEditor.ZoomBox.Camera.Position.X;
            y += (int)ParentEditor.ZoomBox.Camera.Position.Y;
            Point _relativeMousePos = new Point();
            _relativeMousePos.X = (int)Math.Floor(x / ((double)TileGrid.TileSize.X * TileGrid.Scale.X * ParentEditor.ZoomBox.Camera.Zoom.X));
            _relativeMousePos.Y = (int)Math.Floor(y / ((double)TileGrid.TileSize.Y * TileGrid.Scale.Y * ParentEditor.ZoomBox.Camera.Zoom.Y));             
            if (_relativeMousePos.X >= TileGrid.TileCols || _relativeMousePos.Y >= TileGrid.TileRows)
            {
                _relativeMousePos = new Point(-1, -1);
            }
            return _relativeMousePos;
        }

        protected void SelectTile(Point tileCoord)
        {
            if (tileCoord.X < 0 || tileCoord.Y < 0)
            {
                throw new Exception("You can't select a tile outside of the tilegrid");
            }
            if (_selectedTiles.Contains(tileCoord) == false)
            {
                _selectedTiles.Add(tileCoord);
                TileGridEditor.Instance.SetEnableStateEditIcons(true);
            }
        }

        public void ClearSelection()
        {
            _selectedTiles.Clear();
        }

        protected void ApplyBrushPaint(int brushTileIndex)
        {
            int layer = TileGridEditor.Instance.SelectedLayer;
            for (int x = 0; x < _brushSize.X; x++)
            {
                for (int y = 0; y < _brushSize.Y; y++)
                {
                    Point tileIndex = new Point(x + _brushStartPoint.X, y + _brushStartPoint.Y);
                    if (tileIndex.X < TileGrid.TileCols && tileIndex.Y < TileGrid.TileRows)
                    {
                        TileGrid.TileLayers[layer].Tiles[tileIndex.X][tileIndex.Y].Index = brushTileIndex;
                        TileGrid.TileLayers[layer].Tiles[tileIndex.X][tileIndex.Y].Rotation = 0;
                        TileGrid.TileLayers[layer].Tiles[tileIndex.X][tileIndex.Y].HFlip = false;
                        TileGrid.TileLayers[layer].Tiles[tileIndex.X][tileIndex.Y].VFlip = false;
                    }
                }
            }
        }

        protected void ApplyFillBucketPaint(int x, int y, int brushTileIndex)
        {
            int layer = TileGridEditor.Instance.SelectedLayer;
            int oldIndex = TileGrid.TileLayers[layer].Tiles[x][y].Index;
            if (brushTileIndex == oldIndex)
            {
                return;
            }
            TileGrid.TileLayers[layer].Tiles[x][y].Index = brushTileIndex;
            TileGrid.TileLayers[layer].Tiles[x][y].Rotation = 0;
            TileGrid.TileLayers[layer].Tiles[x][y].HFlip = false;
            TileGrid.TileLayers[layer].Tiles[x][y].VFlip = false;

            if (x > 0 && TileGrid.TileLayers[layer].Tiles[x - 1][y].Index == oldIndex)
            {
                ApplyFillBucketPaint(x - 1, y, brushTileIndex);
            }
            if (x < TileGrid.TileCols - 1 && TileGrid.TileLayers[layer].Tiles[x + 1][y].Index == oldIndex)
            {
                ApplyFillBucketPaint(x + 1, y, brushTileIndex);
            }
            if (y > 0 && TileGrid.TileLayers[layer].Tiles[x][y - 1].Index == oldIndex)
            {
                ApplyFillBucketPaint(x, y - 1, brushTileIndex);
            }
            if (y < TileGrid.TileRows - 1 && TileGrid.TileLayers[layer].Tiles[x][y + 1].Index == oldIndex)
            {
                ApplyFillBucketPaint(x, y + 1, brushTileIndex);
            }
        }

        protected Point ClampTile(Point tileCoord)
        {
            tileCoord.X = IceMath.Clamp(tileCoord.X, 0, TileGrid.TileCols - 1);
            tileCoord.Y = IceMath.Clamp(tileCoord.Y, 0, TileGrid.TileRows - 1);
            return tileCoord;
        }

        protected void ExpandBrushSize(Point newTilePos)
        {
            if (newTilePos.X > _brushSizeExpandOrigin.X
                || newTilePos.Y > _brushSizeExpandOrigin.Y)
            {
                _brushSize = new Point(1, 1);
            }
            else
            {
                _brushSize.X = _brushSizeExpandOrigin.X - newTilePos.X +1;
                _brushSize.Y = _brushSizeExpandOrigin.Y - newTilePos.Y +1;
                _brushStartPoint = newTilePos;
            }
        }

        protected void ExpandSelectionSize(Point newTilePos)
        {            
            Point topLeft = new Point();
            Point bottomRight = new Point();
            topLeft.X = Math.Min(newTilePos.X, _selectionExpandOrigin.X);
            topLeft.Y = Math.Min(newTilePos.Y, _selectionExpandOrigin.Y);
            bottomRight.X = Math.Max(newTilePos.X, _selectionExpandOrigin.X);
            bottomRight.Y = Math.Max(newTilePos.Y, _selectionExpandOrigin.Y);
            if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
            {
                ClearSelection();
            }
            for (int x = topLeft.X; x <= bottomRight.X; x++)
            {
                for (int y = topLeft.Y; y <= bottomRight.Y; y++)
                {
                    SelectTile(new Point(x, y));
                }
            }
            _isSelectingArea = true;            
        }

        #endregion

        #region Events

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point _relativeMousePos = GetTileRelativeMousePos(e.X, e.Y);            
            if (e.Button == MouseButtons.Left)
            {
                _brushStartPoint = _relativeMousePos;
                if (PaintMode == TileGridPaintMode.Brush)
                {
                    if (_brushStartPoint.X > -1 && _brushStartPoint.Y > -1)
                    {
                        ApplyBrushPaint(TileGridEditor.Instance.SelectedBrushTile);
                    }
                }
                else if (PaintMode == TileGridPaintMode.Eraser)
                {
                    if (_brushStartPoint.X > -1 && _brushStartPoint.Y > -1)
                    {
                        ApplyBrushPaint(-1);
                    }
                }
                else if (PaintMode == TileGridPaintMode.Edit)
                {
                    if (_selectionExpandOrigin.X > -1 && _selectionExpandOrigin.Y > -1 
                        && _relativeMousePos.X > -1 && _relativeMousePos.Y > -1)
                    {
                        ExpandSelectionSize(_relativeMousePos);
                    }
                }
            }
            else if (e.Button == MouseButtons.None)
            {
                if (PaintMode == TileGridPaintMode.Brush || PaintMode == TileGridPaintMode.Eraser)
                {
                    if (_brushSizeExpandOrigin.X != -1 && _brushSizeExpandOrigin.Y != -1
                        && ((Control.ModifierKeys & Keys.Control) == Keys.Control
                            || (Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                    {
                        ExpandBrushSize(_relativeMousePos);
                    }
                    else
                    {
                        _brushStartPoint = _relativeMousePos;
                    }
                }
                else if (PaintMode == TileGridPaintMode.Bucket)
                {
                    _brushStartPoint = _relativeMousePos;                    
                }                
            }
            if (PaintMode == TileGridPaintMode.Edit)
            {
                if (_isPastingTiles)
                {
                    _drawPosPastingSelection = _relativeMousePos;
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Point _relativeMousePos = GetTileRelativeMousePos(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                if (PaintMode == TileGridPaintMode.Edit)
                {
                    if (_isPastingTiles == false)
                    {
                        _selectionExpandOrigin = _relativeMousePos;
                        _isSelectingArea = false;
                    }
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
            {
                if (PaintMode == TileGridPaintMode.Brush)
                {                    
                    _brushStartPoint = GetTileRelativeMousePos(e.X, e.Y);
                    // if clicked in the tile grid zone
                    if (_brushStartPoint.X > -1 && _brushStartPoint.Y > -1)
                    {
                        ApplyBrushPaint(TileGridEditor.Instance.SelectedBrushTile);
                    }                    
                }
                else if (PaintMode == TileGridPaintMode.Eraser)
                {
                    _brushStartPoint = GetTileRelativeMousePos(e.X, e.Y);
                    // if clicked in the tile grid zone
                    if (_brushStartPoint.X > -1 && _brushStartPoint.Y > -1)
                    {
                        ApplyBrushPaint(-1);
                    }
                }
                else if (PaintMode == TileGridPaintMode.Bucket)
                {
                    _brushStartPoint = GetTileRelativeMousePos(e.X, e.Y);
                    // if clicked in the tile grid zone
                    if (_brushStartPoint.X > -1 && _brushStartPoint.Y > -1)
                    {
                        ApplyFillBucketPaint(_brushStartPoint.X, _brushStartPoint.Y, TileGridEditor.Instance.SelectedBrushTile);
                    }
                }
                else if (PaintMode == TileGridPaintMode.Edit)
                {
                    _brushStartPoint = GetTileRelativeMousePos(e.X, e.Y);
                    if (_isPastingTiles && _brushStartPoint.X > -1 && _brushStartPoint.Y > -1)
                    {
                        PasteTilesAtPos(_brushStartPoint);
                        _isPastingTiles = false;
                    }
                    else
                    {
                        // if clicked in the tile grid zone and tile is not selected already
                        if (_brushStartPoint.X > -1 && _brushStartPoint.Y > -1 && _isSelectingArea == false)
                        {
                            // if holding Ctrl down, add the tile to the others
                            if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                            {
                                ClearSelection();
                            }
                            SelectTile(_brushStartPoint);
                        }
                        _isSelectingArea = false;
                        _selectionExpandOrigin = new Point(-1, -1);
                    }
                }               
            }            
        }


        /// <summary>
        /// Get the WndProc messages to prevent flickering when scrolling
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            // WM_HSCROLL or WM_VSCROLL
            if (m.Msg == 0x114 || m.Msg == 0x115)
            {
                this.Invalidate();
            }
            base.WndProc(ref m);
        }

        #endregion

        #region Selection Actions

        private void StoreCopyOfSelectedTiles()
        {
            int layer = TileGridEditor.Instance.SelectedLayer;
            if (_selectedTiles.Count > 0)
            {                
                _clipboardTiles.Clear();
                
                Point topLeft = new Point(TileGrid.TileCols - 1, TileGrid.TileRows - 1);
                // locate the top left most coords of the group
                for (int i = 0; i < _selectedTiles.Count; i++)
                {
                    topLeft.X = IceMath.Min(topLeft.X, _selectedTiles[i].X);
                    topLeft.Y = IceMath.Min(topLeft.Y, _selectedTiles[i].Y);
                }
                Console.WriteLine("TopLeft: " + topLeft);
                for (int i = 0; i < _selectedTiles.Count; i++)
                {
                    TileCopy tileCopy = new TileCopy();
                    tileCopy.Tile = TileGrid.TileLayers[layer]
                        .Tiles[_selectedTiles[i].X][_selectedTiles[i].Y];
                    Point disp = new Point();
                    disp.X = _selectedTiles[i].X - topLeft.X;
                    disp.Y = _selectedTiles[i].Y - topLeft.Y;
                    tileCopy.Displacement = disp;
                    _clipboardTiles.Add(tileCopy);
                }

                Console.WriteLine("======== CLIPBOARD ========");
                for (int i = 0; i < _clipboardTiles.Count; i++)
                {
                    Console.WriteLine(i + "] " + _clipboardTiles[i].Tile + ", Disp: " + _clipboardTiles[i].Displacement);
                }
                TileGridEditor.Instance.SetEnableStatePasteIcon(true);
            }
        }

        public void ClearSelectedTiles()
        {
            for (int i = 0; i < _selectedTiles.Count; i++)
            {
                int layer = TileGridEditor.Instance.SelectedLayer;
                TileGrid.TileLayers[layer].Tiles[_selectedTiles[i].X][_selectedTiles[i].Y].Index = -1;
                TileGrid.TileLayers[layer].Tiles[_selectedTiles[i].X][_selectedTiles[i].Y].Rotation = 0;
                TileGrid.TileLayers[layer].Tiles[_selectedTiles[i].X][_selectedTiles[i].Y].HFlip = false;
                TileGrid.TileLayers[layer].Tiles[_selectedTiles[i].X][_selectedTiles[i].Y].VFlip = false;
            }
        }

        public void CutSelectedTiles()
        {
            StoreCopyOfSelectedTiles();
            ClearSelectedTiles();
        }

        public void CopySelectedTiles()
        {
            StoreCopyOfSelectedTiles(); 
        }

        public void StartPastingTiles()
        {
            _isPastingTiles = true;
            _drawPosPastingSelection = new Point(-1, -1);
        }

        public void PasteTilesAtPos(Point pastePos)
        {
            int layer = TileGridEditor.Instance.SelectedLayer;
            foreach (TileCopy tileCopy in _clipboardTiles)
            {
                Point tilePos = new Point();
                tilePos.X = pastePos.X + tileCopy.Displacement.X;
                tilePos.Y = pastePos.Y + tileCopy.Displacement.Y;
                if (tilePos.X >= 0 && tilePos.Y >= 0 
                    && tilePos.X < TileGrid.TileCols && tilePos.Y < TileGrid.TileRows)
                {
                    TileGrid.TileLayers[layer].Tiles[tilePos.X][tilePos.Y] = tileCopy.Tile;
                }
            }            
        }

        public void ResetStatus()
        {
            ClearSelection();
            _isSelectingArea = false;
            _selectionExpandOrigin = new Point(-1, -1);
            _brushStartPoint = new Point(-1, -1);
            _isPastingTiles = false;
        }

        public bool IsClipboardNotEmpty()
        {
            return (_clipboardTiles.Count >= 1);
        }

        #endregion

        #region Drawing

        protected void DrawGridLines()
        {
            Color lineColor = Color.White;            
            for (int i = 1; i < TileGrid.TileCols; i++)
            {
                float x = i * _tileGrid.TileSize.X * _tileGrid.Scale.X;
                DebugShapes.DrawLine(new Vector2(x, 0), 
                    new Vector2(x, _tileGrid.BoundingRect.Height), lineColor);
            }
            for (int i = 1; i < TileGrid.TileRows; i++)
            {
                float y = i * _tileGrid.TileSize.Y * _tileGrid.Scale.Y;
                DebugShapes.DrawLine(new Vector2(0, y),
                    new Vector2(_tileGrid.BoundingRect.Width, y), lineColor);
            }
        }

        protected void DrawBrushTileHighlight(Color color)
        {
            if (_brushStartPoint.X > -1 && _brushStartPoint.Y > -1)
            {                         
                Point clampedSize = _brushSize;
                clampedSize.X = IceMath.Clamp(_brushStartPoint.X + clampedSize.X,
                    0, TileGrid.TileCols) - _brushStartPoint.X;
                clampedSize.Y = IceMath.Clamp(_brushStartPoint.Y + clampedSize.Y,
                    0, TileGrid.TileRows) - _brushStartPoint.Y;

                Vector2 pos = new Vector2(_brushStartPoint.X * TileGrid.TileSize.X * TileGrid.Scale.X,
                    _brushStartPoint.Y * TileGrid.TileSize.Y * TileGrid.Scale.Y);
                Vector2 size = new Vector2(clampedSize.X * TileGrid.TileSize.X * TileGrid.Scale.X,
                    clampedSize.Y * TileGrid.TileSize.Y * TileGrid.Scale.Y);
                DrawingManager.DrawFilledRectangle(TileGrid.Layer, pos, size,
                    color, TileGrid.BlendingType);
            }
        }

        protected void DrawTileSelection(int tileX, int tileY)
        {
            Color fillColor = new Color(80, 100, 150, 150);           
            Vector2 pos = new Vector2(tileX * TileGrid.TileSize.X * TileGrid.Scale.X,
                    tileY * TileGrid.TileSize.Y * TileGrid.Scale.Y);
            Vector2 size = new Vector2(TileGrid.TileSize.X * TileGrid.Scale.X,
                TileGrid.TileSize.Y * TileGrid.Scale.Y);
            DrawingManager.DrawFilledRectangle(TileGrid.Layer, pos, size,
                    fillColor, TileGrid.BlendingType);
            DebugShapes.DrawRectangle(new Rectangle((int)(pos.X + 1), (int)(pos.Y + 1), (int)size.X - 2, (int)size.Y - 2),
                    Color.Aquamarine);
        }

        protected void DrawPastingSelection(int tileX, int tileY)
        {            
            if (tileX == -1 || tileY == -1)
            {
                return;
            }
            Color fillColor = new Color(200, 0, 0, 128);
            foreach (TileCopy tileCopy in _clipboardTiles)
            {
                Point tilePos = new Point();
                tilePos.X = tileX + tileCopy.Displacement.X;
                tilePos.Y = tileY + tileCopy.Displacement.Y;
                if (tilePos.X < TileGrid.TileCols && tilePos.Y < TileGrid.TileRows)
                {
                    Vector2 pos = new Vector2(tilePos.X * TileGrid.TileSize.X * TileGrid.Scale.X,
                        tilePos.Y * TileGrid.TileSize.Y * TileGrid.Scale.Y);
                    Vector2 size = new Vector2(TileGrid.TileSize.X * TileGrid.Scale.X,
                        TileGrid.TileSize.Y * TileGrid.Scale.Y);
                    DrawingManager.DrawFilledRectangle(TileGrid.Layer, pos, size,
                        fillColor, TileGrid.BlendingType);
                }
            }
        }

        protected override void Draw()
        {            
            DrawingManager.ViewPortSize = new Point(this.Width, this.Height);
            Update(1 / 60f);
            ParentEditor.ZoomBox.Camera.Position = new Vector2(-this.AutoScrollPosition.X, -this.AutoScrollPosition.Y);
            ParentEditor.ZoomBox.Camera.Update(1 / 60f);
            Vector2 sizeRatio = new Vector2(
                (float)this.Size.Width / ((float)this.Size.Width * ParentEditor.ZoomBox.Camera.Zoom.X), 
                (float)this.Size.Height / ((float)this.Size.Height * ParentEditor.ZoomBox.Camera.Zoom.Y));                       
            GraphicsDevice.Clear(Color.LightBlue);
            // Use SpriteSortMode.Immediate, so we can apply custom renderstates.
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);           
            Rectangle fullRect = new Rectangle(0, 0, this.Width, this.Height);
            // Draw a tiled crossed pattern in the background.
            _spriteBatch.Draw(_crossTexture, fullRect, fullRect, Color.White);
            _spriteBatch.Draw(_checkerTexture, new Rectangle(0, 0, (int)(_tileGrid.BoundingRect.Width * ParentEditor.ZoomBox.Camera.Zoom.X),
                (int)(_tileGrid.BoundingRect.Height * ParentEditor.ZoomBox.Camera.Zoom.Y)),
                new Rectangle(0, 0, (int)(_tileGrid.BoundingRect.Width * ParentEditor.ZoomBox.Camera.Zoom.X),
                (int)(_tileGrid.BoundingRect.Height * ParentEditor.ZoomBox.Camera.Zoom.Y)), Color.White);
            _spriteBatch.End();

            this.AutoScrollMinSize = new System.Drawing.Size((int)(_tileGrid.BoundingRect.Width * ParentEditor.ZoomBox.Camera.Zoom.X),
                (int)(_tileGrid.BoundingRect.Height * ParentEditor.ZoomBox.Camera.Zoom.Y));            

            Vector2 pos = _tileGrid.Position;
            _tileGrid.Position = Vector2.Zero;
            _tileGrid.Pivot = Vector2.Zero;
            _tileGrid.Draw(1 / 60f);
            _tileGrid.Position = pos;
            //_camera.Position += _tileGrid.Position;
            if (PaintMode == TileGridPaintMode.Brush)
            {
                DrawBrushTileHighlight(new Color(25, 255, 100, 150));
            }
            else if (PaintMode == TileGridPaintMode.Bucket)
            {
                DrawBrushTileHighlight(new Color(25, 100, 255, 150));
                _brushSize = new Point(1, 1);
            }
            else if (PaintMode == TileGridPaintMode.Eraser)
            {
                DrawBrushTileHighlight(new Color(255, 0, 0, 150));
            }
            else if (PaintMode == TileGridPaintMode.Edit)
            {
                foreach (Point tile in _selectedTiles)
                {
                    DrawTileSelection(tile.X, tile.Y);
                }
                if (_isPastingTiles)
                {
                    DrawPastingSelection(_drawPosPastingSelection.X, 
                        _drawPosPastingSelection.Y);
                }
            }
            if (_drawGrid == true)
            {
                DrawGridLines();
            }
            MilkshakeForm.SwapCameraAndRenderScene(ParentEditor.ZoomBox.Camera);
            _parent.Update(1 / 60f);
        }

        #endregion
    }
}
