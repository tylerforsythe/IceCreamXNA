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

namespace Milkshake.Editors.TileGrids
{
    using Color = Microsoft.Xna.Framework.Color;
    using IceCream.SceneItems;

    public partial class TileGridMinimapControl : GraphicsDeviceControl
    {
        private Camera _camera = new Camera();
        private TileGrid _parentGrid;
        public TileGrid ParentGrid
        {
            set { _parentGrid = value; }
        }

        protected override void Initialize()
        {
            _camera.IsPivotRelative = false;
            _camera.Pivot = Vector2.Zero;           
            this.VScroll = true;
            this.HScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(0, 0);
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

        protected void DrawGridLines()
        {
            Color lineColor = Color.White;
            
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.LightGray);
            Vector2 oldPosition = _parentGrid.Position;
            Vector2 oldScale = _parentGrid.Scale;
            _parentGrid.Position = Vector2.Zero;
            _parentGrid.Scale = Vector2.One;
            _parentGrid.UpdateBoundingRect();
            _parentGrid.Scale = new Vector2(
                (float)this.Size.Width / (float)_parentGrid.BoundingRect.Width,
                (float)this.Size.Height / (float)_parentGrid.BoundingRect.Height);            
            _parentGrid.Draw(1 / 60f);
            MilkshakeForm.SwapCameraAndRenderScene(_camera);
            _parentGrid.Position = oldPosition;
            _parentGrid.Scale = oldScale;
            _parentGrid.UpdateBoundingRect();
        }

    }
}

