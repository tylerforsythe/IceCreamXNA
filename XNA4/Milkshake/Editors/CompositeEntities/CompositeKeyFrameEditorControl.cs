using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IceCream;
using IceCream.Drawing;
using Milkshake.GraphicsDeviceControls;

namespace Milkshake.Editors.CompositeEntities
{
    using Color = Microsoft.Xna.Framework.Color;
    using IceCream.SceneItems; 
  
    public partial class CompositeKeyFrameEditorControl : GraphicsDeviceControl
    {
        private CompositeEntityEditor _parent;
        private SpriteBatch _spriteBatch;
        private Stopwatch _timer;
        private Texture2D _checkerTexture;
        private Texture2D _pivotTexture;

        #region Properties

        public String HighlightedBone
        {
            get;
            set;
        }

        public List<String> _selectedBones;
        public List<String> SelectedBones
        {
            get { return _selectedBones; }
            set
            {
                _selectedBones = value;
            }
        }

        public CompositeEntity CompositeEntity
        {
            get;
            set;
        }

        internal CompositeEntityEditor ParentEditor
        {
            get { return _parent; }
            set { _parent = value; }
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
                if (ParentEditor != null)
                {
                    ConvertRealMousePosToScenePos();
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
            }
        }
        private bool _refreshSceneItemProperties = false;

        #endregion

        #region Constructor

        public CompositeKeyFrameEditorControl()
        {
            this.HighlightedBone = "";
            this.SelectedBones = new List<String>();
        }

        // Timer controls the update.
        protected override void Initialize()
        {
            // Start the animation timer.
            _timer = Stopwatch.StartNew();                        
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _checkerTexture = Texture2D.FromStream(GraphicsDevice, System.IO.File.OpenRead(Application.StartupPath + "\\Resources\\checker.png"));
            _pivotTexture = Texture2D.FromStream(GraphicsDevice, System.IO.File.OpenRead(Application.StartupPath + "\\Resources\\pivot.png"));
            this.VScroll = true;
            this.HScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(0, 0);
        }

        #endregion

        #region Methods

        private void ConvertRealMousePosToScenePos()
        {
            Vector2 scenePos = Vector2.Zero;
            Camera camera = ParentEditor.ZoomBox.Camera;
            Vector2 mousePos = _realMousePos;
            scenePos = Vector2.Transform(mousePos, Matrix.Invert(camera.Matrix));
            scenePos.X = (float)Math.Round((double)scenePos.X);
            scenePos.Y = (float)Math.Round((double)scenePos.Y);
            SceneMousePos = scenePos;
        }

        public void DrawKeyFrame(CompositeKeyFrame keyFrame)
        {
            if (keyFrame != null)
            {
                CompositeAnimation anim = keyFrame.Parent;                
                anim.ResetToKeyFrame(ParentEditor.tableKeyFrames.SelectedIndicies[0]);
                CompositeEntity.Update(1 / 60f);
                CompositeEntity.Draw(1 / 60f);
            }
        }

        protected void DrawBoneSceneItemBoundingRect(String boneTransformRef, Color color)
        {
            CompositeBoneTransform boneTransform
                     = ParentEditor.SelectedCompositeKeyFrame.GetBoneTransformFromKeyFrame(
                         ParentEditor.SelectedCompositeKeyFrame, boneTransformRef);
            SceneItem boneTransformSceneItem = boneTransform.GetSceneItem();
            if (boneTransform.IsVisible == true &&
                boneTransformSceneItem != null)
            {
                Rectangle boundingRect = boneTransformSceneItem.BoundingRect;
                DebugShapes.DrawRectangle(boundingRect, color);
            }
        }

        protected override void Draw()
        {
            DrawingManager.ViewPortSize = new Point(this.Width, this.Height);            
            ParentEditor.ZoomBox.Camera.Update(1 / 60f);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Use SpriteSortMode.Immediate, so we can apply custom renderstates.
            // Set the texture addressing mode to wrap, so we can repeat
            // many copies of our tiled checkerboard texture.
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);            
            Rectangle fullRect = new Rectangle(0, 0, this.Width, this.Height);
            // Draw a tiled checkerboard pattern in the background.
            _spriteBatch.Draw(_checkerTexture, fullRect, fullRect, new Color(64, 64, 92));
            if (CompositeEntity == null)
            {
                _spriteBatch.End();
            }
            else
            {
                _spriteBatch.End();        

                float gridSize = 100000;
                DrawingManager.DrawFilledRectangle(10, new Vector2(-gridSize, 0), 
                    new Vector2(gridSize * 2, 1), new Color(128, 128, 128, 40), DrawingBlendingType.Alpha);
                DrawingManager.DrawFilledRectangle(10, new Vector2(0, -gridSize),
                    new Vector2(1, gridSize * 2), new Color(128, 128, 128, 40), DrawingBlendingType.Alpha);

                
                DrawingManager.DrawFilledRectangle(10, new Vector2(-gridSize, 213),
                    new Vector2(gridSize * 2, 1), new Color(255, 0, 255, 0), DrawingBlendingType.Alpha);

                //GraphicsDevice.Clear(new Color(52, 52, 52));
                

                CompositeEntity.Position = new Vector2(0);
                if (ParentEditor.SelectedCompositeKeyFrame != null)
                {
                    DrawKeyFrame(ParentEditor.SelectedCompositeKeyFrame);

                    if (String.IsNullOrEmpty(this.HighlightedBone) == false)
                    {
                        DrawBoneSceneItemBoundingRect(this.HighlightedBone, Color.Gray);
                    }
                    foreach (String boneTransformRef in this.SelectedBones)
                    {
                        DrawBoneSceneItemBoundingRect(boneTransformRef, Color.Blue);
                    }

                    if (_refreshSceneItemProperties == true)
                    {
                        ParentEditor.propertyGridCompositeBoneTransform.SelectedObject
                            = this.ParentEditor.SelectedCompositeBoneTransform;
                        _refreshSceneItemProperties = false;
                    }
                }
                MilkshakeForm.SwapCameraAndRenderScene(ParentEditor.ZoomBox.Camera);

                // draw Pivot
                if (String.IsNullOrEmpty(this.HighlightedBone) != true)
                {
                    _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    Vector2 offset = new Vector2(this.Width / 2f, this.Height / 2f);
                    CompositeBoneTransform boneTransform = ParentEditor.SelectedCompositeKeyFrame.GetBoneTransformFromKeyFrame(
                     ParentEditor.SelectedCompositeKeyFrame, this.HighlightedBone);
                    SceneItem boneTransformSceneItem = boneTransform.GetSceneItem();
                    if (boneTransform.IsVisible == true &&
                        boneTransformSceneItem != null)
                    {
                        Rectangle boundingRect = boneTransformSceneItem.BoundingRect;
                        Vector2 pivotPos = offset + boneTransformSceneItem.GetAbsolutePivot(true)
                            + new Vector2(boundingRect.X, boundingRect.Y);
                        _spriteBatch.Draw(_pivotTexture, pivotPos, null, Color.White, 0, new Vector2(6.5f),
                            Vector2.One, SpriteEffects.None, 1);
                    }
                    _spriteBatch.End();
                }
            }
            if (ParentEditor.UpdatePreview == true)
            {
                ParentEditor.UpdatePreview = false;
                ParentEditor.PreviewAnimation();
            }
            ParentEditor.Update(1 / 60f);
        }

        #endregion 
       
        #region Events

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            Console.WriteLine(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (ParentEditor.SelectedCompositeBoneTransform != null)
            {
                Vector2 displacement = Vector2.Zero;
                if (e.KeyCode == Keys.Up)
                {
                    displacement = new Vector2(0, -1);
                }
                else if (e.KeyCode == Keys.Down)
                {
                    displacement = new Vector2(0, 1);
                }
                else if (e.KeyCode == Keys.Left)
                {
                    displacement = new Vector2(-1, 0);
                }
                else if (e.KeyCode == Keys.Right)
                {
                    displacement = new Vector2(1, 0);
                }
                if (displacement != Vector2.Zero)
                {
                    // 10 pixels if the shift key is pressed
                    if (e.Shift == true)
                    {
                        displacement *= new Vector2(10);
                    }
                    foreach (String boneTransformRef in this.SelectedBones)
                    {
                        CompositeBoneTransform boneTransform
                            = this.ParentEditor.SelectedCompositeKeyFrame.GetBoneTransformFromKeyFrame(
                            this.ParentEditor.SelectedCompositeKeyFrame, boneTransformRef);
                        boneTransform.Position += displacement;
                    }
                    _refreshSceneItemProperties = true;
                    ParentEditor.UpdatePreview = true;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.ParentEditor.SelectedCompositeKeyFrame != null)
            {
                // update the mouse position (this will update the Scene mouse too)
                RealMousePos = new Vector2(e.X, e.Y);
                // if the user is dragging selected item(s)
                if (_isDraggingItem == true && this.SelectedBones.Count > 0)
                {                    
                    Vector2 dragDistance = this.SceneMousePos - _dragItemStartPos;
                    if (dragDistance.LengthSquared() > 0)
                    {
                        foreach (String boneTransformRef in this.SelectedBones)
                        {
                            CompositeBoneTransform boneTransform
                                    = this.ParentEditor.SelectedCompositeKeyFrame.GetBoneTransformFromKeyFrame(
                                    this.ParentEditor.SelectedCompositeKeyFrame, boneTransformRef);
                            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                            {
                                float originalAngle = _dragItemStartPos.Angle();
                                float targetAngle = this.SceneMousePos.Angle();
                                boneTransform.Rotation = MathHelper.WrapAngle((float)Math.Round((double)(boneTransform.Rotation + (targetAngle - originalAngle)), 2));
                            }
                            else
                            {                                
                                boneTransform.Position += dragDistance;
                            }
                        }
                        _refreshSceneItemProperties = true;
                        ParentEditor.UpdatePreview = true;
                    }
                    _dragItemStartPos = SceneMousePos;
                }
                else
                {
                    // check if the mouse is hovering an object
                    // and highlight it
                    this.HighlightedBone = null;
                    foreach (CompositeBoneTransform boneTransform
                        in ParentEditor.SelectedCompositeKeyFrame.BoneTransforms)
                    {
                        // ignore 'locked' bones
                        if (boneTransform.IsPositionCurrentlyInherited() == true)
                        {
                            continue;
                        }
                        SceneItem boneTransformSceneItem = boneTransform.GetSceneItem();
                        if (boneTransform.IsVisible == true && boneTransformSceneItem != null)
                        {
                            Rectangle boundingRect = boneTransformSceneItem.BoundingRect;
                            Microsoft.Xna.Framework.Point mousePoint
                                = new Microsoft.Xna.Framework.Point((int)SceneMousePos.X,
                                (int)SceneMousePos.Y);
                            if (IceMath.IsPointInsideRectangle(mousePoint, boundingRect))
                            {
                                this.HighlightedBone = boneTransform.BoneReference;
                            }
                        }
                    }
                    // check the selected items and give them priority over the others
                    foreach (String boneTransformRef in this.SelectedBones)
                    {
                        CompositeBoneTransform boneTransform
                            = ParentEditor.SelectedCompositeKeyFrame.GetBoneTransformFromKeyFrame(
                                ParentEditor.SelectedCompositeKeyFrame, boneTransformRef);
                        SceneItem boneTransformSceneItem = boneTransform.GetSceneItem();
                        if (boneTransform.IsVisible == true && boneTransformSceneItem != null)
                        {
                            Rectangle boundingRect = boneTransformSceneItem.BoundingRect;
                            Microsoft.Xna.Framework.Point mousePoint
                                = new Microsoft.Xna.Framework.Point((int)SceneMousePos.X,
                                (int)SceneMousePos.Y);                            
                            if (IceMath.IsPointInsideRectangle(mousePoint, boundingRect))
                            {
                                this.HighlightedBone = boneTransformRef;
                            }
                        }
                    }
                }
                // Check for scene dragging with the mouse
                if (_isDraggingScene == true)
                {
                    Vector2 dragDistance = -(RealMousePos - _dragSceneStartPos);
                    ParentEditor.ZoomBox.Camera.Position += dragDistance;                   
                    _dragSceneStartPos = RealMousePos;
                }
            }            
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.ParentEditor.SelectedCompositeKeyFrame != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _isDraggingItem = false;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    _isDraggingScene = false;
                    this.Cursor = Cursors.Default;
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            if (this.ParentEditor.SelectedCompositeKeyFrame != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    // if click on an item
                    if (this.HighlightedBone != null)
                    {
                        // new item to select
                        if (!this.SelectedBones.Contains(this.HighlightedBone))
                        {
                            // Checks to see that Control or Shift are not pressed
                            if ((Control.ModifierKeys & Keys.Control) != Keys.Control
                                && (Control.ModifierKeys & Keys.Shift) != Keys.Shift)
                            {
                                this.SelectedBones.Clear();
                                this.SelectedBones.Add(this.HighlightedBone);
                                ParentEditor.IgnoreBoneTransformSelectionEvent = true;
                                ParentEditor.SelectBoneTransformsOnTreeFromScene();
                                ParentEditor.IgnoreBoneTransformSelectionEvent = false;
                            }
                            // Multiple selection
                            else
                            {
                                // add it to the selection
                                this.SelectedBones.Add(this.HighlightedBone);
                                ParentEditor.IgnoreBoneTransformSelectionEvent = true;
                                ParentEditor.SelectBoneTransformsOnTreeFromScene();
                                ParentEditor.IgnoreBoneTransformSelectionEvent = false;
                            }
                            _isDraggingItem = false;
                        }

                        // if the item is already selected, start dragging
                        if (this.SelectedBones.Contains(this.HighlightedBone))
                        {
                            _dragItemStartPos = SceneMousePos;
                            _isDraggingItem = true;
                        }
                    }
                    // if click in the void
                    else
                    {
                        this.SelectedBones.Clear();
                        ParentEditor.IgnoreBoneTransformSelectionEvent = true;
                        ParentEditor.SelectBoneTransformsOnTreeFromScene();
                        ParentEditor.IgnoreBoneTransformSelectionEvent = false;
                        _isDraggingItem = false;
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    _isDraggingScene = true;
                    this.Cursor = Cursors.NoMove2D;
                    _dragSceneStartPos = this.RealMousePos;
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    // if the item is already selected, set pivot
                    if (this.SelectedBones.Contains(this.HighlightedBone))
                    {
                        CompositeBoneTransform boneTransform
                     = ParentEditor.SelectedCompositeKeyFrame.GetBoneTransformFromKeyFrame(
                         ParentEditor.SelectedCompositeKeyFrame, this.HighlightedBone);
                        SceneItem boneTransformSceneItem = boneTransform.GetSceneItem();
                        if (boneTransform.IsVisible == true &&
                            boneTransformSceneItem != null)
                        {
                            Rectangle boundingRect = boneTransformSceneItem.BoundingRect;
                            Vector2 diff = new Vector2(SceneMousePos.X - boundingRect.X, 
                                SceneMousePos.Y - boundingRect.Y);
                            Console.WriteLine("Diff " + boneTransformSceneItem.Name + "] " + diff);
                            boneTransformSceneItem.Pivot = diff;
                            boneTransformSceneItem.IsPivotRelative = false;
                        }

                    }
                }
                base.OnMouseDown(e);
            }
        }

        #endregion
    }
}
