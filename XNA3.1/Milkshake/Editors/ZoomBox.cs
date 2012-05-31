using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IceCream;
using IceCream.Drawing;
using Milkshake.GraphicsDeviceControls;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;
using IceCream.SceneItems;

namespace Milkshake.Editors
{
    public class ZoomBox
    {
        #region Fields

        private ToolStripButton _toolStripButtonZoomIn;
        private ToolStripButton _toolStripButtonZoomOut;
        private ToolStripButton _toolStripButtonZoomNormal;

        private Camera _camera = new Camera();
        internal Camera Camera
        {
            get { return _camera; }
            set { _camera = value; }
        }
        private float _zoomDelta = 0.06f;
        private float _zoomFactor = 1f;
        internal float ZoomFactor
        {
            get { return _zoomFactor; }
            set
            {
                if (value < 0.05f)
                {
                    _zoomFactor = 0.05f;
                    if (_toolStripButtonZoomOut != null)
                    {
                        _toolStripButtonZoomOut.Enabled = false;
                    }
                }
                else
                {
                    _zoomFactor = value;
                    if (_toolStripButtonZoomOut != null)
                    {
                        _toolStripButtonZoomOut.Enabled = true;
                    }
                }
                if (_zoomFactor == 1.0f)
                {
                    _toolStripButtonZoomNormal.Enabled = false;
                }
                else
                {
                    _toolStripButtonZoomNormal.Enabled = true;
                }
                _camera.Zoom = new Vector2(_zoomFactor);
                _camera.Update(1 / 60f);
            }
        }        

        #endregion

        #region Constructor

        public ZoomBox()
        {
            _camera = new Camera();            
            _camera.Pivot = Vector2.Zero;
        }

        #endregion

        #region Methods

        public void ZoomIn()
        {
            ZoomFactor += _zoomDelta;
        }

        public void ZoomOut()
        {
            ZoomFactor -= _zoomDelta;
        }

        public void ZoomNormal()
        {
            ZoomFactor = 1f;
        }

        public void SetToolStripButtomZoomIn(ToolStripButton toolStripButton)
        {
            _toolStripButtonZoomIn = toolStripButton;
            _toolStripButtonZoomIn.Click += buttonZoomIn_Click;
        }

        public void SetToolStripButtomZoomOut(ToolStripButton toolStripButton)
        {
            _toolStripButtonZoomOut = toolStripButton;
            _toolStripButtonZoomOut.Click += buttonZoomOut_Click;
        }

        public void SetToolStripButtomZoomNormal(ToolStripButton toolStripButton)
        {
            _toolStripButtonZoomNormal = toolStripButton;
            _toolStripButtonZoomNormal.Click += buttonZoomNormal_Click;
            _toolStripButtonZoomNormal.Enabled = false;
        }

        #endregion

        #region Events

        private void buttonZoomIn_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void buttonZoomNormal_Click(object sender, EventArgs e)
        {
            ZoomNormal();
        }

        private void buttonZoomOut_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        #endregion
    }
}
