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
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using IceCream;
using IceCream.Drawing;
using IceCream.SceneItems;

namespace Milkshake.GraphicsDeviceControls
{
    public partial class SceneEditorControl : GraphicsDeviceControl
    {        
        bool shouldMakeCopy;
        //internal bool _showPoly;   
        private Vector2 _mousePos = Vector2.Zero;
        private SceneItem _highlightedItem;
        public SceneItem HighlightedItem
        {
            get { return _highlightedItem; }
            set { _highlightedItem = value; }
        }
        private List<SceneItem> _selectedItems = new List<SceneItem>();
        internal List<SceneItem> SelectedItems
        {
            get { return _selectedItems; }
            set { _selectedItems = value; }
        }
        private SceneItem _previewedItem = null;
        internal SceneItem PreviewedItem
        {
            get { return _previewedItem; }
            set 
            {                 
                _previewedItem = value;
                if (_previewedItem is IAnimationDirector)
                {
                    IAnimationDirector animDirector = _previewedItem as IAnimationDirector;
                    animDirector.PlayAnimation(animDirector.DefaultAnimation);
                }
            }
        }
        private Vector2 _dragItemStartPos = Vector2.Zero;
        private Vector2 _dragSceneStartPos = Vector2.Zero;
        private bool _isDraggingItem = false;
        private bool _isDraggingScene = false;
        private Vector2 _realMousePos = Vector2.Zero;
        public Vector2 RealMousePos
        {
            get { return _realMousePos; }
            set
            {
                _realMousePos = value;
                ConvertRealMousePosToScenePos();
                if (MilkshakeForm.Instance != null)
                {
                    MilkshakeForm.Instance.RefreshStatusStrip();
                }
            }
        }
        private Vector2 _sceneMousePos = Vector2.Zero;
        public Vector2 SceneMousePos
        {
            get { return _sceneMousePos; }
            set
            {
                _sceneMousePos = value;
                if (MilkshakeForm.Instance != null)
                {
                    MilkshakeForm.Instance.RefreshStatusStrip();
                }
            }
        }
        private bool _refreshSceneItemProperties = false;
        public MilkshakePreferences Preferences
        {
            get;
            set;
        }

        public IceScene CurrentScene
        {
            get { return SceneManager.ActiveScene; }
        }

        public SceneEditorControl()
        {
            
        }

        protected override void Initialize()
        {
            // Start the animation timer.
            //_timer = Stopwatch.StartNew();
            IceCream.Drawing.DrawingManager.Intialize();
            DrawingManager.LoadContent(GraphicsDevice);                
            this.AutoScroll = false;
            
        }
        
        protected override void Draw()
        {          
            Update(1f / 60f);            
            if (CurrentScene != null)
            {
                DrawingManager.ViewPortSize = new Microsoft.Xna.Framework.Point(this.Width, this.Height);
                IceCore.DrawIceCreamScene(GraphicsDevice, 0.16f, SceneManager.ActiveScene);
                if (_highlightedItem != null)
                {
                    Microsoft.Xna.Framework.Rectangle re = _highlightedItem.BoundingRect;
                    if (_highlightedItem.IgnoreCameraPosition)
                    {
                        Vector2 offset = CurrentScene.ActiveCameras[0].Position;
                        //offset *= -1;
                        re.Offset((int)offset.X, (int)offset.Y);
                    }
                    DebugShapes.DrawRectangle(re, Color.Yellow);
                }
                foreach (SceneItem item in _selectedItems)
                {
                    Microsoft.Xna.Framework.Rectangle re = item.BoundingRect;
                    if (item.IgnoreCameraPosition)
                    {
                        Vector2 offset = CurrentScene.ActiveCameras[0].Position;
                        //offset *= -1;
                        re.Offset((int)offset.X, (int)offset.Y);
                    }
                    DebugShapes.DrawRectangle(re, Color.White);
                }
                if (PreviewedItem != null)
                {                    
                    Vector2 oldPosition = PreviewedItem.Position;
                    Vector2 oldPivot = PreviewedItem.Pivot;
                    int oldLayer = PreviewedItem.Layer;
                    PreviewedItem.Position = Vector2.Zero;
                    PreviewedItem.Layer = 1;
                    PreviewedItem.Pivot = new Vector2(PreviewedItem.BoundingRect.Width / 2f, PreviewedItem.BoundingRect.Height / 2f);
                    PreviewedItem.Update(1f / 60f);
                    PreviewedItem.Draw(1f / 60f);
                    PreviewedItem.Position = oldPosition;
                    PreviewedItem.Layer = oldLayer;
                    PreviewedItem.Pivot = oldPivot;                   
                }
                if (Preferences.ShowCameraBounds == true)
                {
                    Point size = SceneManager.GlobalDataHolder.NativeResolution;
                    Point safeSize = Point.Zero;
                    safeSize.X = (int)(size.X * 0.8f);
                    safeSize.Y = (int)(size.Y * 0.8f);
                    IceCream.Drawing.DebugShapes.DrawRectangle(
                        new Microsoft.Xna.Framework.Rectangle(-size.X / 2, -size.Y / 2, size.X, size.Y),
                            new Color(255, 0, 255, 200));
                    IceCream.Drawing.DebugShapes.DrawRectangle(
                        new Microsoft.Xna.Framework.Rectangle(-safeSize.X / 2, -safeSize.Y / 2, safeSize.X, safeSize.Y),
                            new Color(255, 255, 255, 100));
                }
                if (Preferences.ShowGrid == true)
                {
                    Point gridSize = Preferences.GridSizes[Preferences.SelectedGrid - 1];
                    Microsoft.Xna.Framework.Rectangle screenBounds = SceneManager.ActiveScene.ActiveCameras[0].BoundingRect;
                    Point start = new Point((int)Math.Floor((double)screenBounds.X / (double)gridSize.X),
                        (int)Math.Floor((double)screenBounds.Y / (double)gridSize.X));
                    Point end = new Point(1 + (int)Math.Floor((double)screenBounds.Right / (double)gridSize.X),
                        1 + (int)Math.Floor((double)screenBounds.Bottom / (double)gridSize.Y));
                    for (int x = start.X; x <= end.X; x++)
                    {
                        IceCream.Drawing.DebugShapes.DrawLine(new Vector2(x * gridSize.X, start.Y * gridSize.Y),
                            new Vector2(x * gridSize.X, end.Y * gridSize.Y), (x == 0) ? Color.Black : Preferences.GridColor);
                    }
                    for (int y = start.Y; y <= end.Y; y++)
                    {
                        IceCream.Drawing.DebugShapes.DrawLine(new Vector2(start.X * gridSize.X, y * gridSize.Y),
                            new Vector2(end.X * gridSize.X, y * gridSize.Y), (y == 0) ? Color.Black : Preferences.GridColor);
                    }
                }
                IceCore.RenderIceCream();                
            }            
        }

        private void Update(float elapsedTime)
        {
            if (CurrentScene != null)
            {
                if (SceneManager.ActiveScene.ActiveCameras.Count == 0)
                    return;
                // update the camera
                Camera camera = SceneManager.ActiveScene.ActiveCameras[0];
                camera.Update(elapsedTime);
                PresentationParameters pp = DrawingManager.GraphicsDevice.PresentationParameters;
                // full size by default
                DrawingManager.ViewPortSize = new Microsoft.Xna.Framework.Point(pp.BackBufferWidth, pp.BackBufferHeight);                
                
                // update all scene items
                for (int i = 0; i < SceneManager.ActiveScene.SceneItems.Count; i++)
                {
                    SceneManager.ActiveScene.SceneItems[i].Update(elapsedTime);
                }
                if (_refreshSceneItemProperties == true)
                {
                    MilkshakeForm.Instance.UpdatePropertyGrid();
                    _refreshSceneItemProperties = false;
                }
            }
        }

        private void PasteTemplate()
        {
            TreeNode node = MilkshakeForm.Instance.treeViewResources.SelectedNode;
            if (node == null || node.Tag == null || (node.Tag is SceneItem) == false)
            {
                MilkshakeForm.ShowErrorMessage("You must select a Template first");
            }
            else
            {
                SceneItem template = node.Tag as SceneItem;
                if (template.IsTemplate == false)
                {
                    MilkshakeForm.ShowErrorMessage("The selected SceneItem is not a Template");
                }
                else
                {
                    SceneItem copy = MilkshakeForm.Instance.CreateNewInstaceCopyOf(template);                    
                    MilkshakeForm.Instance.AddNewSceneItemInstance(copy, SceneItemGroup.SceneInstances, false);
                    copy.Position = this.SceneMousePos;
                }
            }

        }

        #region User Input

        private void ConvertRealMousePosToScenePos()
        {
            Vector2 scenePos = Vector2.Zero;
            if (SceneManager.ActiveScene != null && SceneManager.ActiveScene.ActiveCameras.Count > 0
                && SceneManager.ActiveScene.ActiveCameras[0] != null)
            {
                Camera camera = SceneManager.ActiveScene.ActiveCameras[0];             
                Vector2 mousePos = _realMousePos;               
                scenePos = Vector2.Transform(mousePos, Matrix.Invert(camera.Matrix));                
            }
            SceneMousePos = scenePos;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (SceneManager.ActiveScene != null)
            {
                // update the mouse position (this will update the Scene mouse too)
                this.RealMousePos = new Vector2(e.X, e.Y);
                if (MilkshakeForm.Instance.SceneEditorTool == MilkshakeSceneEditorTool.Select)
                {
                    // if the user is dragging selected item(s)
                    if (_isDraggingItem == true && _selectedItems.Count > 0)
                    {
                        Vector2 dragDistance = SceneMousePos - _dragItemStartPos;
                        Vector2 diffDragDistance = Vector2.Zero;
                        dragDistance.X = (float)Math.Round(dragDistance.X);
                        dragDistance.Y = (float)Math.Round(dragDistance.Y);
                        //Console.WriteLine("dragDistance: " + dragDistance + " start: " + _dragItemStartPos);
                        if (dragDistance.LengthSquared() > 0)
                        {
                            if (shouldMakeCopy == true)
                            {
                                shouldMakeCopy = false;
                                MilkshakeForm.Instance.CopySelectedItems();
                                List<SceneItem> _newItems = MilkshakeForm.Instance.PasteSelectedItems();
                                _selectedItems.Clear();
                                foreach (var item in _newItems)
                                {
                                    _selectedItems.Add(item);
                                }
                            }
                            foreach (SceneItem item in _selectedItems)
                            {
                                Vector2 newPos = item.Position + dragDistance;
                                if (Preferences.ShowGrid == true && Preferences.SnapToGrid == true)
                                {
                                    #region SnapToGrid
                                    Point gridSize = Preferences.GridSizes[Preferences.SelectedGrid - 1];
                                    int snapZone = Preferences.GridAttractionZones[Preferences.SelectedGrid - 1];
                                    int left = item.BoundingRect.Left + (int)dragDistance.X;
                                    int right = item.BoundingRect.Right + (int)dragDistance.X;
                                    int top = item.BoundingRect.Top + (int)dragDistance.Y;
                                    int bottom = item.BoundingRect.Bottom + (int)dragDistance.Y;
                                    Vector2 halfGridSize = new Vector2(gridSize.X / 2, gridSize.Y);
                                    Vector2 leftSnap = new Vector2(
                                        IceMath.Floor(left / (float)gridSize.X),
                                        left % (float)gridSize.X);
                                    Vector2 rightSnap = new Vector2(
                                        IceMath.Ceiling(right / (float)gridSize.X),
                                        ((float)gridSize.X - right
                                        % (float)gridSize.X) % (float)gridSize.X);
                                    if (leftSnap.X < 0)
                                    {
                                        leftSnap.Y = (gridSize.X + leftSnap.Y) % gridSize.X;
                                    }
                                    if (leftSnap.Y > halfGridSize.X)
                                    {
                                        leftSnap.X += 1;
                                        leftSnap.Y = leftSnap.Y - gridSize.X;
                                    }
                                    if (rightSnap.Y > halfGridSize.X)
                                    {
                                        rightSnap.X -= 1;
                                        rightSnap.Y = rightSnap.Y - gridSize.X;
                                    }

                                    Vector2 snapX = Vector2.Zero;
                                    if (Math.Abs(leftSnap.Y) <= Math.Abs(rightSnap.Y))
                                    {
                                        snapX = leftSnap;
                                    }
                                    else
                                    {
                                        snapX = rightSnap;
                                    }
                                    //Console.WriteLine("L: " + leftSnap + " R: " + rightSnap + " ");
                                    if (Math.Abs(snapX.Y) <= (float)snapZone)
                                    {
                                        float diff = newPos.X - (float)left;
                                        float oldPos = newPos.X;
                                        newPos.X = snapX.X * gridSize.X + diff;
                                        if (snapX != leftSnap)
                                        {
                                            newPos.X -= right - left;
                                        }
                                        diffDragDistance.X = newPos.X - oldPos;
                                    }

                                    Vector2 topSnap = new Vector2(
                                        IceMath.Floor(top / (float)gridSize.Y),
                                        top % (float)gridSize.Y);
                                    Vector2 bottomSnap = new Vector2(
                                        IceMath.Ceiling(bottom / (float)gridSize.Y),
                                        ((float)gridSize.Y - bottom
                                        % (float)gridSize.Y) % (float)gridSize.Y);
                                    if (topSnap.X < 0)
                                    {
                                        topSnap.Y = (gridSize.Y + topSnap.Y) % gridSize.Y;
                                    }
                                    if (topSnap.Y > halfGridSize.X)
                                    {
                                        topSnap.X += 1;
                                        topSnap.Y = topSnap.Y - gridSize.Y;
                                    }
                                    if (bottomSnap.Y > halfGridSize.X)
                                    {
                                        bottomSnap.X -= 1;
                                        bottomSnap.Y = bottomSnap.Y - gridSize.Y;
                                    }

                                    Vector2 snapY = Vector2.Zero;
                                    if (Math.Abs(topSnap.Y) <= Math.Abs(bottomSnap.Y))
                                    {
                                        snapY = topSnap;
                                    }
                                    else
                                    {
                                        snapY = bottomSnap;
                                    }
                                    //Console.WriteLine("T: " + topSnap + " B: " + bottomSnap + " ");
                                    if (Math.Abs(snapY.Y) <= (float)snapZone)
                                    {
                                        float diff = newPos.Y - (float)top;
                                        float oldPos = newPos.Y;
                                        newPos.Y = snapY.X * gridSize.Y + diff;
                                        if (snapY != topSnap)
                                        {
                                            newPos.Y -= bottom - top;
                                        }
                                        diffDragDistance.Y = newPos.Y - oldPos;
                                    }
                                    #endregion
                                }
                                item.Position = newPos;                                
                                MilkshakeForm.Instance.SceneWasModified = true;
                            }
                            _refreshSceneItemProperties = true;
                        }
                        _dragItemStartPos = SceneMousePos;
                        if (diffDragDistance.X != 0)
                        {
                            _dragItemStartPos.X += diffDragDistance.X;
                        }
                        if (diffDragDistance.Y != 0)
                        {
                            _dragItemStartPos.Y += diffDragDistance.Y;
                        }
                    }
                    else
                    {
                        // check if the mouse is hovering an object
                        // and highlight it
                        _highlightedItem = null;
                        foreach (SceneItem item in SceneManager.ActiveScene.SceneItems)
                        {
                            if (item is PostProcessAnimation)
                                continue;
                            Microsoft.Xna.Framework.Point mousePoint
                                = new Microsoft.Xna.Framework.Point((int)SceneMousePos.X,
                                (int)SceneMousePos.Y);
                            if (IceMath.IsPointInsideRectangle(mousePoint, item.BoundingRect))
                            {
                                if (item.Visible)
                                    _highlightedItem = item;
                            }
                        }
                        // check the selected items and give them priority over the others
                        foreach (SceneItem item in _selectedItems)
                        {
                            Microsoft.Xna.Framework.Point mousePoint
                                = new Microsoft.Xna.Framework.Point((int)SceneMousePos.X,
                                (int)SceneMousePos.Y);
                            if (IceMath.IsPointInsideRectangle(mousePoint, item.BoundingRect))
                            {
                                if (item.Visible)
                                    _highlightedItem = item;
                            }
                        }
                    }
                }
                // Check for scene dragging with the mouse
                if (_isDraggingScene == true)
                {
                    Camera cam = SceneManager.ActiveScene.ActiveCameras[0];
                    Vector2 dragDistance = -(RealMousePos - _dragSceneStartPos) / cam.Zoom;
                    cam.Position += dragDistance;
                    _refreshSceneItemProperties = true;
                    _dragSceneStartPos = RealMousePos;                    
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            shouldMakeCopy = false;
            if (e.Button == MouseButtons.Left)
            {
                _isDraggingItem = false;
            }
            else if (e.Button == MouseButtons.Right)
            {
                _isDraggingScene = false;
                this.Cursor = Cursors.Default;
            }
            if (MilkshakeForm.Instance.SceneEditorTool == MilkshakeSceneEditorTool.TemplateBrush)
            {
                PasteTemplate();
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                shouldMakeCopy = true;
            }
            if (e.Button == MouseButtons.Left)
            {
                if (MilkshakeForm.Instance.SceneEditorTool == MilkshakeSceneEditorTool.Select)
                {
                    // if click on an item
                    if (_highlightedItem != null)
                    {
                        // new item to select
                        if (!_selectedItems.Contains(_highlightedItem))
                        {
                            // Checks to see that Control or Shift are not pressed
                            if ((Control.ModifierKeys & Keys.Control) != Keys.Control
                                && (Control.ModifierKeys & Keys.Shift) != Keys.Shift)
                            {
                                _selectedItems.Clear();
                                _selectedItems.Add(_highlightedItem);
                                MilkshakeForm.Instance.SelectItem(_highlightedItem);
                            }
                            // Multiple selection
                            else
                            {
                                // add it to the selection
                                _selectedItems.Add(_highlightedItem);

                            }
                            _isDraggingItem = false;
                        }

                        // if the item is already selected, start dragging
                        if (_selectedItems.Contains(_highlightedItem))
                        {
                            _dragItemStartPos = SceneMousePos;
                            _isDraggingItem = true;
                        }
                    }
                    // if click in the void
                    else
                    {
                        MilkshakeForm.Instance.UnselectItems();
                        _isDraggingItem = false;
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                _isDraggingScene = true;
                this.Cursor = Cursors.NoMove2D;
                _dragSceneStartPos = RealMousePos;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (MilkshakeForm.Instance.SceneEditorTool == MilkshakeSceneEditorTool.Select)
            {
                // if click on an item
                if (_highlightedItem != null && _selectedItems.Contains(_highlightedItem))
                {
                    MilkshakeForm.Instance.EditSceneItem(_highlightedItem);
                }
            }
            base.OnMouseDoubleClick(e);
        }        

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);            
            MilkshakeForm.Instance.HandleKeyPress(e);
        }       

        #endregion
    }
}
