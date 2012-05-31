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
    using Color = Microsoft.Xna.Framework.Graphics.Color;
    using IceCream.SceneItems;

    public partial class TileSelectionControl : GraphicsDeviceControl
    {
        private Camera _camera = new Camera();
        private TileGrid _parentGrid;
        public TileGrid ParentGrid
        {
            get { return _parentGrid; }
            set { _parentGrid = value; }
        }
        private int _padding = 4;
        private int _rowsCountOnTexture = -1;        
        private int _highlightedTile = -1;

        protected override void Initialize()
        {
            _camera.IsPivotRelative = false;
            _camera.Pivot = Vector2.Zero;            
            this.VScroll = true;
            this.HScroll = true;
            TileGridEditor.Instance.SelectedBrushTile = 0;
            _highlightedTile = -1;
            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
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

        protected void UpdateTileSelection()
        {
            if (ParentGrid != null)
            {
                UpdateRowsCountFromTexture();
                int scrollMinWidth = ParentGrid.TileSize.X + 2 * _padding;
                int scrollMinHeight = ParentGrid.TileSize.Y + _padding;
                Point lastTilePos = GetPosOfTile(_rowsCountOnTexture * ParentGrid.ColumnsCountOnTexture - 1);
                scrollMinHeight += lastTilePos.Y;
                this.AutoScrollMinSize = new Size(scrollMinWidth, scrollMinHeight);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point mousePos = new Point(e.X - this.AutoScrollPosition.X, e.Y - this.AutoScrollPosition.Y);
            _highlightedTile = -1;
            for (int i = 0; i < _rowsCountOnTexture * ParentGrid.ColumnsCountOnTexture; i++)
            {
                Point tilePos = GetPosOfTile(i);
                Rectangle tileRect = new Rectangle(tilePos.X, tilePos.Y, ParentGrid.TileSize.X, ParentGrid.TileSize.Y);               
                if (IceMath.IsPointInsideRectangle(mousePos, tileRect))
                {
                    _highlightedTile = i;
                    return;
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _highlightedTile = -1;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (_highlightedTile > -1)
            {
                TileGridEditor.Instance.SelectedBrushTile = _highlightedTile;
            }
        }

        #region Drawing

        protected override void Draw()
        {
            UpdateTileSelection();            
            GraphicsDevice.Clear(Color.LightGray);
            DrawTiles();
            MilkshakeForm.SwapCameraAndRenderScene(_camera);
        }

        protected void DrawTiles()
        {
            if (_highlightedTile > -1)
            {
                Point pos = GetPosOfTile(_highlightedTile);
                int halfPadding = _padding / 2;
                pos.X -= halfPadding - this.AutoScrollPosition.X;
                pos.Y -= halfPadding - this.AutoScrollPosition.Y;
                DrawingManager.DrawFilledRectangle(ParentGrid.Layer, new Vector2(pos.X, pos.Y),
                    new Vector2(ParentGrid.TileSize.X + _padding, ParentGrid.TileSize.Y + _padding),
                    Color.DarkGray, ParentGrid.BlendingType);
            }
            if (TileGridEditor.Instance.SelectedBrushTile > -1)
            {
                Point pos = GetPosOfTile(TileGridEditor.Instance.SelectedBrushTile);
                int halfPadding = _padding / 2;
                pos.X -= halfPadding - this.AutoScrollPosition.X;
                pos.Y -= halfPadding - this.AutoScrollPosition.Y;
                DrawingManager.DrawFilledRectangle(ParentGrid.Layer, new Vector2(pos.X, pos.Y),
                    new Vector2(ParentGrid.TileSize.X + _padding, ParentGrid.TileSize.Y + _padding),
                    Color.White, ParentGrid.BlendingType);                
            }
            for (int i = 0; i < _rowsCountOnTexture * ParentGrid.ColumnsCountOnTexture; i++)
            {                
                DrawRequest drawRequest = default(DrawRequest);
                drawRequest.scaleRatio = Vector2.One;
                drawRequest.tint = Color.White;
                drawRequest.texture = ParentGrid.Material.Texture;
                Point pos = GetPosOfTile(i);
                drawRequest.position.X = pos.X + this.AutoScrollPosition.X;
                drawRequest.position.Y = pos.Y + this.AutoScrollPosition.Y;
                drawRequest.sourceRectangle = ParentGrid.GetSourceRectOfTile(i);
                DrawingManager.DrawOnLayer(drawRequest, 1, DrawingBlendingType.Alpha);
            }
        }

        protected Point GetPosOfTile(int tileIndex)
        {
            int columnsAvailable = (int)Math.Floor(this.ClientSize.Width / (double)ParentGrid.TileSize.X);
            // all the safe borders minus 2 (the edges)
            int paddingBorderSize = (columnsAvailable * _padding + _padding);
            columnsAvailable = (int)Math.Floor((this.ClientSize.Width - paddingBorderSize) / (double)ParentGrid.TileSize.X);
            if (columnsAvailable < 1)
            {
                columnsAvailable = 1;
            }
            Point tilePos = new Point();
            tilePos.X = (int)Math.Floor(tileIndex / (double)columnsAvailable);
            tilePos.Y = (int)(tileIndex % columnsAvailable);
            Point position = new Point();
            position.Y = (tilePos.X * ParentGrid.TileSize.Y) + ((tilePos.X + 1) * _padding);
            position.X = (tilePos.Y * ParentGrid.TileSize.X) + ((tilePos.Y + 1) * _padding);
            return position;
        }

        private void UpdateRowsCountFromTexture()
        {
            if (ParentGrid.Material != null && ParentGrid.Material.Texture != null)
            {
                // get number of tiles rows in the source texture
                int textureSize = ParentGrid.Material.Texture.Height;
                if (ParentGrid.UseTilingSafeBorders)
                {
                    _rowsCountOnTexture = (textureSize + 2) / (ParentGrid.TileSize.Y + 2);

                }
                else
                {
                    _rowsCountOnTexture = textureSize / ParentGrid.TileSize.Y;
                }
                if (_rowsCountOnTexture < 1)
                {
                    throw new Exception("The tiles rows count of a texture cannot be 0");
                }
            }
        }

        #endregion
    }
}

