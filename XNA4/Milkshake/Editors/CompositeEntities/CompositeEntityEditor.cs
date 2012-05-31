using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MilkshakeLibrary;
using Milkshake.Editors;
using Milkshake.Selectors;
using Microsoft.Xna.Framework;
using IceCream;
using IceCream.SceneItems.CompositeEntityClasses;
using IceCream.Components;
using IceCream.Attributes;
using IceCream.SceneItems;
using XPTable.Models;
using XPTable;
using System.Globalization;

namespace Milkshake.Editors.CompositeEntities
{
    public partial class CompositeEntityEditor : SceneItemEditor
    {
        #region Fields

        private String _lastSelectedBone;
        private CompositeEntity _previewEntity;

        #endregion

        #region Properties

        public ZoomBox ZoomBox
        {
            get;
            set;
        }

        public bool UpdatePreview
        {
            get;
            set;
        }

        public override SceneItem SceneItem
        {
            get
            {
                return base.SceneItem;
            }
            set
            {
                base.SceneItem = value;
                CompositeEntity = base.SceneItem as CompositeEntity;
            }
        }

        internal CompositeEntity CompositeEntity
        {
            get { return compositeEntityEditorControl.CompositeEntity; }
            set { compositeEntityEditorControl.CompositeEntity = value; }
        }

        public CompositeAnimation SelectedCompositeAnimation
        {
            get
            {
                if (listViewAnimations.SelectedIndices.Count == 0)
                {
                    return null;
                }
                else
                {
                    int selectedAnim = listViewAnimations.SelectedIndices[0];
                    return CompositeEntity.Animations[selectedAnim];
                }
            }
        }
        
        public CompositeKeyFrame SelectedCompositeKeyFrame
        {
            get
            {
                CompositeAnimation anim = SelectedCompositeAnimation;
                if (anim == null || anim.KeyFrames.Count == 0 || tableKeyFrames.SelectedIndicies.Length == 0
                    || tableKeyFrames.SelectedIndicies[0] > anim.KeyFrames.Count - 1)
                {
                    return null;
                }
                int selectedFrame = tableKeyFrames.SelectedIndicies[0];
                return anim.KeyFrames[selectedFrame];
            }
        }

        public CompositeBoneTransform SelectedCompositeBoneTransform 
        {
            get
            {
                CompositeAnimation anim = SelectedCompositeAnimation;
                CompositeKeyFrame selectedFrame = SelectedCompositeKeyFrame;
                if (anim == null || selectedFrame == null 
                    || tableBoneTransforms.SelectedIndicies.Length == 0)
                {
                    return null;
                }
                return selectedFrame.BoneTransforms
                    [tableBoneTransforms.SelectedIndicies[0]];                
            }
        }

        public CompositeBone SelectedCompositeBone
        {
            get
            {
                TreeNode node = treeViewBones.SelectedNode;
                return treeViewBones.Tag as CompositeBone;
            }
        }

        public bool IgnoreBoneTransformSelectionEvent
        {
            get;
            set;
        }

        private CompositeKeyFrame CompositeKeyFrameClipBoard
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        public CompositeEntityEditor()
        {
            InitializeComponent();
            this.treeViewSceneItems.ImageList = MilkshakeForm.Instance.imageListTreeview;
            InitializeSceneItemsTreeViewNodes();
            compositeEntityEditorControl.ParentEditor = this;    
            ZoomBox = new ZoomBox();
            ZoomBox.Camera.Pivot = new Vector2(0.5f);
            ZoomBox.SetToolStripButtomZoomIn(toolStripButtonZoomIn);
            ZoomBox.SetToolStripButtomZoomOut(toolStripButtonZoomOut);
            ZoomBox.SetToolStripButtomZoomNormal(toolStripButtonZoomNormal);
            IgnoreBoneTransformSelectionEvent = false;
            tableKeyFrames.HeaderRenderer = new XPTable.Renderers.FlatHeaderRenderer();
            tableBoneTransforms.HeaderRenderer = new XPTable.Renderers.FlatHeaderRenderer();
            sceneItemPreviewControl.SceneItem = null;
            sceneItemPreviewCompositeAnimationPreview.SceneItem = null;
            sceneItemPreviewCompositeAnimationPreview.Camera.Zoom = new Vector2(0.4f);
            // clear the lists/table
            listViewAnimations.Clear();
            tableBoneTransforms.TableModel.Rows.Clear();
            tableKeyFrames.TableModel.Rows.Clear();
            propertyGridCompositeBoneTransform.SelectedObject = null;
            _previewEntity = new CompositeEntity();
            _previewEntity.Animations.Add(new CompositeAnimation(_previewEntity));            
        }

        private void CompositeEntityEditor_Load(object sender, EventArgs e)
        {                    
            RefreshTreeviewSceneItemBank();
            RefreshTreeViewBoneStructure(null);
            RefreshListAnims();
        }              

        #endregion

        #region Methods

        #endregion

        #region Misc Events

        const int WM_MOUSEWHEEL = 0x20A;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_MOUSEWHEEL && compositeEntityEditorControl.Focused == true)
            {
                if ((int)m.WParam > 0)
                {
                    this.ZoomBox.ZoomIn();
                }
                else
                {
                    this.ZoomBox.ZoomOut();
                }            
            }
        }

        #endregion

        #region Events Scene Items Bank

        private void InitializeSceneItemsTreeViewNodes()
        {
            // bank            
            treeViewSceneItems.Nodes.Clear();
            toolStripSplitButtonAddSceneItem.DropDownItems.Clear();
            toolStripSplitButtonAddSceneItem.Tag = SceneItemType.Default;
            foreach (SceneItemTypeStruct item in MilkshakeForm.Instance.SceneItemsTypeTable)
            {
                treeViewSceneItems.Nodes.Add("SceneItemInstance" + item.pluralName, item.pluralName, item.icon, item.icon);
                ToolStripItem dropItem;
                dropItem = toolStripSplitButtonAddSceneItem.DropDownItems.Add(item.name, item.image, sceneItemToolStripMenuItem_Click);
                dropItem.Tag = item.type;  
            }
        }

        private void RefreshTreeviewSceneItemBank()
        {
            for (int i = 0; i < treeViewSceneItems.Nodes.Count; i++)
            {
                treeViewSceneItems.Nodes[i].Nodes.Clear();
            }
            foreach (String key in CompositeEntity.SceneItemBank.Keys)
            {
                SceneItem item = CompositeEntity.SceneItemBank[key];
                SceneItemType itemType = MilkshakeForm.GetTypeOfSceneItem(item);
                TreeNode newNode = treeViewSceneItems.Nodes[(int)itemType].Nodes.Add(key);
                String icon = MilkshakeForm.GetIconFromType(itemType);
                newNode.SelectedImageKey = newNode.ImageKey = icon;
                newNode.Tag = item;
            }
            treeViewSceneItems.ExpandAll();
            SceneItemRefConverter.UpdateListSceneItemsRefs(CompositeEntity.SceneItemBank.Keys);                      
        }

        private void sceneItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripItem)
            {
                ToolStripItem itemNode = sender as ToolStripItem;
                if (itemNode.Tag != null)
                {
                    SceneItemType type = (SceneItemType)itemNode.Tag;
                    SceneItem newItem = MilkshakeForm.Instance.InstanciateNewItemOfType(type);
                    newItem.Name = GetNewSceneItemName(MilkshakeForm.Instance.SceneItemsTypeTable[(int)type].name);
                    newItem.SceneParent = SceneManager.ActiveScene;
                    newItem.Pivot = new Vector2(0.5f);
                    newItem.IsPivotRelative = true;
                    CompositeEntity.SceneItemBank.Add(newItem.Name, newItem);
                    RefreshTreeviewSceneItemBank();
                }               
            }
        }

        private String GetNewSceneItemName(String baseName)
        {
            String boneName = "New " + baseName + " ";
            int i = 1;
            while (true)
            {
                String formattedNumber = i.ToString("00");
                String compName = boneName + formattedNumber;
                if (CompositeEntity.SceneItemBank.ContainsKey(compName) == false)
                {
                    return compName;
                }
                i++;
            }
        }
              
        private void treeViewSceneItems_AfterSelect(object sender, TreeViewEventArgs e)
        {
            bool enableStatus = false;
            if (e.Node.Tag != null && e.Node.Tag is SceneItem)
            {
                SceneItem sceneItem = e.Node.Tag as SceneItem;
                sceneItemPreviewControl.SceneItem = sceneItem;
                textBoxPivot.Text = sceneItem.Pivot.X.ToString(CultureInfo.InvariantCulture) + ", "
                    + sceneItem.Pivot.Y.ToString(CultureInfo.InvariantCulture);
                if (sceneItem.IsPivotRelative == true)
                {
                    comboBoxIsPivotRelative.SelectedIndex = 0;
                }
                else
                {
                    comboBoxIsPivotRelative.SelectedIndex = 1;
                }
                enableStatus = true;
            }
            toolStripButtonDeleteSceneItem.Enabled = enableStatus;
            toolStripButtonNewBoneFromItem.Enabled = enableStatus;
            toolStripSplitButtonCopySceneItem.Enabled = enableStatus;
            groupBoxSceneItemProperties.Enabled = enableStatus;
        }

        private void treeViewSceneItems_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            String oldName = e.Node.Text;
            String newName = e.Label;
            if (e.Node.Tag != null && e.Node.Tag is SceneItem
                && String.IsNullOrEmpty(newName) == false &&
                    CompositeEntity.SceneItemBank.ContainsKey(newName) == false)
            {                
                // TO-DO: Sync boneTransforms!
                CompositeEntity.SceneItemBank.Add(newName, CompositeEntity.SceneItemBank[oldName]);
                CompositeEntity.SceneItemBank.Remove(oldName);
                CompositeEntity.SceneItemBank[newName].Name = newName;
                IceCream.SceneItems.CompositeEntityClasses.SceneItemRefConverter.
                    UpdateListSceneItemsRefs(CompositeEntity.SceneItemBank.Keys);
            }
            else
            {
                e.CancelEdit = true;
            }
        }

        private void treeViewSceneItems_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag is SceneItem)
            {
                SceneItem item = e.Node.Tag as SceneItem;
                MilkshakeForm.Instance.OpenSceneItemInEditor(item,
                    this.ItemIsLocal);
            }
        }

        private void toolStripButtonDeleteSceneItem_Click(object sender, EventArgs e)
        {
            if (treeViewSceneItems.SelectedNode != null && treeViewSceneItems.SelectedNode.Tag != null)
            {
                // TO-DO: Sync boneTransforms!
                CompositeEntity.SceneItemBank.Remove(treeViewSceneItems.SelectedNode.Text);
                treeViewSceneItems.SelectedNode.Remove();
            }
        }

        private void toolStripSplitButtonCopySceneItem_Click(object sender, EventArgs e)
        {
            if (treeViewSceneItems.SelectedNode != null && treeViewSceneItems.SelectedNode.Tag != null)
            {
                SceneItem selItem = CompositeEntity.SceneItemBank[treeViewSceneItems.SelectedNode.Text];
                SceneItem copy = (SceneItem)selItem.GetType().Assembly.CreateInstance(selItem.GetType().FullName);
                selItem.CopyValuesTo(copy);
                copy.Name = GetNewSceneItemName(treeViewSceneItems.SelectedNode.Text);
                CompositeEntity.SceneItemBank.Add(copy.Name, copy);
                RefreshTreeviewSceneItemBank();
            }
        }

        private void textBoxPivot_Validated(object sender, EventArgs e)
        {            
            try
            {
                SceneItem item = treeViewSceneItems.SelectedNode.Tag as SceneItem;
                char[] separator = { ',' };
                String[] pivots = textBoxPivot.Text.Split(separator);
                item.Pivot = new Vector2(float.Parse(pivots[0], CultureInfo.InvariantCulture), 
                    float.Parse(pivots[1], CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    SceneItem item = treeViewSceneItems.SelectedNode.Tag as SceneItem;
                    textBoxPivot.Text = item.Pivot.X.ToString(CultureInfo.InvariantCulture) + ", "
                        + item.Pivot.Y.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void textBoxPivot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxPivot_Validated(sender, EventArgs.Empty);
            }
        }
        

        private void comboBoxIsPivotRelative_SelectedIndexChanged(object sender, EventArgs e)
        {
            SceneItem item = treeViewSceneItems.SelectedNode.Tag as SceneItem;
            if (comboBoxIsPivotRelative.SelectedIndex == 0)
            {
                item.IsPivotRelative = true;
            }
            else
            {
                item.IsPivotRelative = false;
            }
        }

        #endregion        

        #region Events Animations

        private void RefreshListAnims()
        {
            listViewAnimations.Items.Clear();
            foreach (CompositeAnimation anim in CompositeEntity.Animations)
            {
                listViewAnimations.Items.Add(anim.Name);
            }
            int selectedAnim = CompositeEntity.CurrentAnimationID;
            if (selectedAnim >= 0 && CompositeEntity.Animations.Count > selectedAnim)
            {
                listViewAnimations.SelectedIndices.Add(selectedAnim);
            }
        }

        public void PreviewAnimation()
        {
            propertyGridAnimation.SelectedObject = SelectedCompositeAnimation;
            CompositeEntity.CopyValuesTo(_previewEntity);
            _previewEntity.PlayAnimation(SelectedCompositeAnimation.Name);
            sceneItemPreviewCompositeAnimationPreview.SceneItem = _previewEntity;                     
        }

        private void toolStripButtonAddAnimation_Click(object sender, EventArgs e)
        {            
            CompositeAnimation newAnim = new CompositeAnimation(CompositeEntity);
            newAnim.Name = GetNewAnimationName("Animation ");
            CompositeEntity.Animations.Add(newAnim);
            listViewAnimations.Items.Add(newAnim.Name);
        }

        private void toolStripButtonDuplicateAnim_Click(object sender, EventArgs e)
        {
            if (listViewAnimations.SelectedIndices.Count > 0)
            {
                int selectedAnim = listViewAnimations.SelectedIndices[0];
                CompositeAnimation duplicate = new CompositeAnimation();
                this.SelectedCompositeAnimation.CopyValuesTo(duplicate, this.SelectedCompositeAnimation.Parent);
                duplicate.Name = GetNewAnimationName(duplicate.Name + " ");
                CompositeEntity.Animations.Add(duplicate);
                listViewAnimations.Items.Add(duplicate.Name);
            }
        }

        private void toolStripButtonDelAnimation_Click(object sender, EventArgs e)
        {
            if (listViewAnimations.SelectedIndices.Count > 0)
            {
                int selectedAnim = listViewAnimations.SelectedIndices[0];
                if (CompositeEntity.CurrentAnimationID == selectedAnim)
                {
                    CompositeEntity.CurrentAnimationID = -1;
                }
                splitContainerRightTop.Enabled = false;
                tableKeyFrames.TableModel.Rows.Clear();
                tableBoneTransforms.TableModel.Rows.Clear();
                CompositeEntity.Animations.RemoveAt(selectedAnim);
                listViewAnimations.Items.RemoveAt(selectedAnim);               
            }
        }        

        private void listViewAnimations_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enabledState = false;
            if (listViewAnimations.SelectedIndices.Count > 0)
            {
                enabledState = true;
                splitContainerRightTop.Enabled = true;
                int selectedAnim = listViewAnimations.SelectedIndices[0];
                CompositeEntity.CurrentAnimationID = selectedAnim;
                RefreshListKeyFrames();        
                PreviewAnimation();
            }
            else
            {
                splitContainerRightTop.Enabled = false;
                tableKeyFrames.TableModel.Rows.Clear();
                tableBoneTransforms.TableModel.Rows.Clear();
                groupBoxBoneTransforms.Enabled = false;
                toolStripButtonDeleteKeyFrame.Enabled = false;
                toolStripButtonKeyFrameCopy.Enabled = false;
                toolStripButtonKeyFramePaste.Enabled = (CompositeKeyFrameClipBoard != null);
            }
            
            toolStripButtonAddKeyFrame.Enabled = enabledState;
            toolStripButtonDelAnimation.Enabled = enabledState;
            toolStripButtonDuplicateAnim.Enabled = enabledState;
        }

        private void listViewAnimations_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null)
            {
                return;
            }
            String oldName = listViewAnimations.Items[e.Item].Text;
            String newName = e.Label.Trim();
            if (oldName != newName)
            {                
                if (String.IsNullOrEmpty(newName) == false && IsAnimationNameUnique(newName) == true)
                {
                    CompositeEntity.Animations[e.Item].Name = newName;
                }
                else
                {
                    e.CancelEdit = true;
                }
            }
        }

        private bool IsAnimationNameUnique(String name)
        {
            foreach (CompositeAnimation anim in CompositeEntity.Animations)
            {
                if (anim.Name == name)
                {
                    return false;    
                }
            }
            return true;
        }

        private String GetNewAnimationName(String nameBase)
        {            
            int i = 1;
            while (true)
            {
                String formattedNumber = i.ToString("00");
                String compName = nameBase + formattedNumber;
                if (IsAnimationNameUnique(compName))
                {
                    return compName;
                }
                i++;                                
            }  
        }

        #endregion

        #region Events KeyFrames

        private void RefreshListKeyFrames()
        {
            tableKeyFrames.TableModel.Rows.Clear();
            CompositeAnimation selectedAnim = SelectedCompositeAnimation;
            foreach (CompositeKeyFrame keyFrame in selectedAnim.KeyFrames)
            {
                Cell[] cells = new Cell[2];
                cells[0] = new Cell(keyFrame.Name);
                cells[1] = new Cell((object)keyFrame.Duration);
                Row newRow = new Row(cells);
                tableKeyFrames.TableModel.Rows.Add(newRow);
            }
            tableBoneTransforms.TableModel.Selections.Clear();            
            tableKeyFrames.TableModel.Selections.AddCell(0, 1);
            this.UpdatePreview = true;
        } 

        private void tableKeyFrames_CellPropertyChanged(object sender, XPTable.Events.CellEventArgs e)
        {
            // Duration
            if (e.Cell.Data != null)
            {
                Console.WriteLine("KF duration modified from " + e.OldValue + " to " + e.Cell.Data);
                CompositeKeyFrame selectedKeyFrame = SelectedCompositeKeyFrame;
                selectedKeyFrame.Duration = Int32.Parse(e.Cell.Data.ToString());
                SelectedCompositeAnimation.Reset();
                this.UpdatePreview = true;
            }
            else
            {
                Console.WriteLine("KF name modified from " + e.OldValue + " to " + e.Cell.Text);
                SelectedCompositeKeyFrame.Name = e.Cell.Text;
            }
        }

        private void tableKeyFrames_SelectionChanged(object sender, XPTable.Events.SelectionEventArgs e)
        {
            bool enabledState = false;
            if (this.SelectedCompositeKeyFrame != null)
            {
                enabledState = true;
                RefreshTableCompositeBoneTransforms();
            }
            else
            {
                tableBoneTransforms.TableModel.Rows.Clear();
            }
            groupBoxBoneTransforms.Enabled = enabledState;
            toolStripButtonDeleteKeyFrame.Enabled = enabledState;
            toolStripButtonKeyFrameCopy.Enabled = enabledState;
            toolStripButtonKeyFramePaste.Enabled = (CompositeKeyFrameClipBoard != null);
            tableKeyFrames.Invalidate();
        }

        private void toolStripButtonAddKeyFrame_Click(object sender, EventArgs e)
        {
            CompositeAnimation anim = SelectedCompositeAnimation;
            CompositeKeyFrame newFrame = new CompositeKeyFrame(anim);
            if (SelectedCompositeKeyFrame != null)
            {
                SelectedCompositeKeyFrame.CopyValuesTo(newFrame, SelectedCompositeKeyFrame.Parent);
            }
            else
            {
                newFrame.GenerateDefaultBoneTransformsList();
            }
            anim.KeyFrames.Add(newFrame);
            RefreshListKeyFrames();
        }

        private void toolStripButtonDeleteKeyFrame_Click(object sender, EventArgs e)
        {
            CompositeAnimation anim = SelectedCompositeAnimation;
            anim.KeyFrames.Remove(SelectedCompositeKeyFrame);
            anim.Reset();
            RefreshListKeyFrames();
            // update selection event when there are no items left, XPTable hack fix
            if (this.SelectedCompositeAnimation.KeyFrames.Count == 0)
            {
                tableKeyFrames_SelectionChanged(this, null);
            }
        }

        private void toolStripButtonKeyFrameCopy_Click(object sender, EventArgs e)
        {
            CompositeAnimation anim = SelectedCompositeAnimation;
            CompositeKeyFrameClipBoard = new CompositeKeyFrame();
            SelectedCompositeKeyFrame.CopyValuesTo(CompositeKeyFrameClipBoard, anim);
            toolStripButtonKeyFramePaste.Enabled = true;
        }

        private void toolStripButtonKeyFramePaste_Click(object sender, EventArgs e)
        {
            CompositeAnimation anim = SelectedCompositeAnimation;
            int insertIndex = -1;
            int[] selectedIndicies = tableKeyFrames.SelectedIndicies;
            if (anim.KeyFrames.Count > 0 && selectedIndicies.Length > 0)
            {
                insertIndex = selectedIndicies[0];
            }
            CompositeKeyFrame newInstance = new CompositeKeyFrame();
            CompositeKeyFrameClipBoard.CopyValuesTo(newInstance, anim);
            anim.KeyFrames.Insert(insertIndex + 1, newInstance);   
            RefreshListKeyFrames();
        }

        #endregion

        #region Bone Transforms Events

        private void RefreshTableCompositeBoneTransforms()
        {
            tableBoneTransforms.TableModel.Rows.Clear();            
            CompositeKeyFrame selectedKeyFrame = SelectedCompositeKeyFrame;
            foreach (CompositeBoneTransform boneTransform in selectedKeyFrame.BoneTransforms)
            {
                Cell[] cells = new Cell[2];
                cells[0] = new Cell(boneTransform.BoneReference);
                cells[1] = new Cell("", boneTransform.IsVisible, null);
                Row newRow = new Row(cells);
                tableBoneTransforms.TableModel.Rows.Add(newRow);
            }
            if (selectedKeyFrame.BoneTransforms.Count > 0)
            {
                if (String.IsNullOrEmpty(_lastSelectedBone))
                {
                    tableBoneTransforms.TableModel.Selections.Clear();                                  
                }
                else
                {
                    for (int i = 0; i < selectedKeyFrame.BoneTransforms.Count; i++)
                    {
                        if (selectedKeyFrame.BoneTransforms[i].BoneReference
                            == _lastSelectedBone)
                        {
                            tableBoneTransforms.TableModel.Selections.Clear();
                            tableBoneTransforms.TableModel.Selections.AddCell(i, 1);
                        }
                    }
                }
            }
            tableBoneTransforms.Invalidate();
        }

        private void tableBoneTransforms_SelectionChanged(object sender, XPTable.Events.SelectionEventArgs e)
        {
            bool enabledState = false;
            if (tableBoneTransforms.SelectedIndicies.Length > 0)
            {
                SceneItem sceneItem = SelectedCompositeBoneTransform.GetSceneItem();
                if (sceneItem != null && sceneItem is ISubItemCollection)
                {
                    SubItemRefConverter.SubItemsRefs = ((ISubItemCollection)sceneItem).GetSubItemsList();
                }
                propertyGridCompositeBoneTransform.SelectedObject = SelectedCompositeBoneTransform;
                _lastSelectedBone = SelectedCompositeBoneTransform.BoneReference;
                enabledState = true;
            }
            if (this.IgnoreBoneTransformSelectionEvent == false)
            {
                IgnoreBoneTransformSelectionEvent = true;
                SelectBoneTransformsOnSceneFromTree();
                IgnoreBoneTransformSelectionEvent = false;
            }
            groupBoxCompositeBoneProperties.Enabled = enabledState;
            toolStripButtonLevelDownBoneTransform.Enabled = enabledState
                && tableBoneTransforms.SelectedIndicies.Length > 0
                && tableBoneTransforms.SelectedIndicies[0] != 
                    (SelectedCompositeKeyFrame.BoneTransforms.Count - 1);
            toolStripButtonLevelUpBoneTransform.Enabled = enabledState
                && tableBoneTransforms.SelectedIndicies.Length > 0
                && tableBoneTransforms.SelectedIndicies[0] != 0;
        }

        private void tableBoneTransforms_CellPropertyChanged(object sender, XPTable.Events.CellEventArgs e)
        {
            if (e.EventType == XPTable.Events.CellEventType.CheckStateChanged)
            {
                int index = e.CellPos.Row;
                CompositeKeyFrame selectedFrame = SelectedCompositeKeyFrame;
                CompositeBoneTransform boneTransform = selectedFrame.BoneTransforms[index];
                boneTransform.IsVisible = (bool)e.Cell.Checked;
            }
        }
       
        private void toolStripButtonLevelUpBoneTransform_Click(object sender, EventArgs e)
        {
            CompositeAnimation anim = SelectedCompositeAnimation;
            CompositeKeyFrame selectedFrame = SelectedCompositeKeyFrame;
            CompositeBoneTransform boneTransform = SelectedCompositeBoneTransform;
            int index = tableBoneTransforms.SelectedIndicies[0];
            if (index > 0)
            {
                selectedFrame.BoneTransforms.RemoveAt(index);
                selectedFrame.BoneTransforms.Insert(index - 1, boneTransform);
                _lastSelectedBone = boneTransform.BoneReference;
                RefreshTableCompositeBoneTransforms();
            }
        }

        private void toolStripButtonLevelDownBoneTransform_Click(object sender, EventArgs e)
        {
            CompositeAnimation anim = SelectedCompositeAnimation;
            CompositeKeyFrame selectedFrame = SelectedCompositeKeyFrame;
            CompositeBoneTransform boneTransform = SelectedCompositeBoneTransform;
            int index = tableBoneTransforms.SelectedIndicies[0];
            if (index < selectedFrame.BoneTransforms.Count - 1)
            {
                selectedFrame.BoneTransforms.RemoveAt(index);
                selectedFrame.BoneTransforms.Insert(index + 1, boneTransform);
                _lastSelectedBone = boneTransform.BoneReference;
                RefreshTableCompositeBoneTransforms();
            }
        }

        public void SelectBoneTransformsOnTreeFromScene()
        {
            tableBoneTransforms.TableModel.Selections.Clear();
            List<String> selectedBoneTransforms = compositeEntityEditorControl.SelectedBones;
            for (int i = 0; i < SelectedCompositeKeyFrame.BoneTransforms.Count; i++)
            {
                if (selectedBoneTransforms.Contains(
                    SelectedCompositeKeyFrame.BoneTransforms[i].BoneReference))
                {
                    tableBoneTransforms.TableModel.Selections.AddCell(i, 1);
                }
            }
        }

        public void SelectBoneTransformsOnSceneFromTree()
        {
            compositeEntityEditorControl.SelectedBones.Clear();
            foreach (int i in tableBoneTransforms.SelectedIndicies)
            {
                String bone = SelectedCompositeKeyFrame.BoneTransforms[i].BoneReference;
                compositeEntityEditorControl.SelectedBones.Add(bone);
            }
        }

        private void toolStripButtonUsePivot_Click(object sender, EventArgs e)
        {
            CompositeBoneTransform boneTransform = SelectedCompositeBoneTransform;
            SceneItem sceneItem = boneTransform.GetSceneItem();
            if (sceneItem != null)
            {
                sceneItem.Pivot = boneTransform.Position;
                sceneItem.IsPivotRelative = false;
            }
        }

        private void toolStripButtonSyncTransform_Click(object sender, EventArgs e)
        {
            CompositeBoneTransform boneTrans = SelectedCompositeBoneTransform;
            foreach (CompositeKeyFrame keyFrame in SelectedCompositeAnimation.KeyFrames)
            {
                if (keyFrame != SelectedCompositeKeyFrame)
                {
                     CompositeBoneTransform boneTransTarget = keyFrame.GetBoneTransformFromKeyFrame(keyFrame, boneTrans.BoneReference);
                     boneTrans.CopyValuesTo(boneTransTarget, keyFrame);
                }
            }
        }

        #endregion

        #region Item Properties

        private void propertyGridCompositeBoneTransform_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            this.UpdatePreview = true;
        }

        #endregion

        #region Bones

        private bool IsBoneNodeNameUnique(CompositeBone bone, String name)
        {
            if (bone != null)
            {
                if (bone.Name == name)
                {
                    return false;
                }
                foreach (CompositeBone childBone in bone.ChildBones)
                {
                    if (IsBoneNodeNameUnique(childBone, name) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private String GetUniqueBoneNodeName(String name)
        {
            String boneName = name;
            int i = 1;
            while (true)
            {
                if (IsBoneNodeNameUnique(CompositeEntity.RootBone, boneName) == true)
                {
                    return boneName;
                }
                String formattedNumber = i.ToString("00");
                boneName = name + formattedNumber;
                i++;
            }
        }

        private String GetNewBoneNodeName()
        {
            String boneName = "New Bone ";
            int i = 1;
            while (true)
            {
                String formattedNumber = i.ToString("00");
                String compName = boneName + formattedNumber;
                if (IsBoneNodeNameUnique(CompositeEntity.RootBone, compName) == true)
                {
                    return compName;
                }
                i++;
            }
        }

        private TreeNode InsertBoneNodeInNode(TreeNodeCollection parentNodes, CompositeBone bone, CompositeBone selectedBone)
        {
            String icon = (bone == CompositeEntity.RootBone) ? "anchor.png" : "link.png";
            TreeNode returnNode = null;
            TreeNode newNode = parentNodes.Add(
                "Node" + bone.Name, bone.Name, icon, icon);
            newNode.Tag = bone;
            if (bone.Equals(selectedBone))
            {                
                returnNode = newNode;
            }            
            foreach (CompositeBone childBone in bone.ChildBones)
            {
                TreeNode childNodeReturnValue = InsertBoneNodeInNode(newNode.Nodes, childBone, selectedBone);
                if (childNodeReturnValue != null)
                {
                    returnNode = childNodeReturnValue;
                }
            }
            return returnNode;
        }

        private void RefreshTreeViewBoneStructure(CompositeBone selectedBone)
        {
            treeViewBones.Nodes.Clear();
            if (CompositeEntity.RootBone != null)
            {
                TreeNode selectedNode = InsertBoneNodeInNode(treeViewBones.Nodes, CompositeEntity.RootBone, selectedBone);
                treeViewBones.ExpandAll();
                if (CompositeEntity.RootBone == selectedBone)
                {
                    treeViewBones.SelectedNode = treeViewBones.Nodes[0];
                }
                else
                {
                    treeViewBones.SelectedNode = selectedNode;
                }
            }            
            if (treeViewBones.SelectedNode == null)
            {
                treeViewBones_AfterSelect(treeViewBones, null);
                if (CompositeEntity.RootBone != null)
                {
                    treeViewBones.SelectedNode = treeViewBones.Nodes[0];
                }
            }
        }

        private void toolStripButtonAddRootBone_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeViewBones.SelectedNode;
            CompositeBone selectedBone = null;
            CompositeBone parentBone = null;
            CompositeBone newBone = new CompositeBone("", GetNewBoneNodeName());
            newBone.Parent = CompositeEntity;
            if (selectedNode != null)
            {
                selectedBone = selectedNode.Tag as CompositeBone;
                if (selectedBone == CompositeEntity.RootBone)
                {
                    parentBone = selectedBone;
                }
                else
                {
                    parentBone = selectedBone.ParentBone;
                }
            }
            else
            {
                if (CompositeEntity.RootBone == null)
                {
                    throw new Exception("CompositeEntity.RootBone is null");
                }
                else
                {
                    parentBone = CompositeEntity.RootBone;
                }
            }
            parentBone.AddChildBone(newBone);
            RefreshTreeViewBoneStructure(null);
        }

        private void toolStripButtonAddChildBone_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeViewBones.SelectedNode;
            if (selectedNode != null)
            {
                CompositeBone selectedBone = null;
                CompositeBone newBone = new CompositeBone("", GetNewBoneNodeName());
                newBone.Parent = CompositeEntity;
                selectedBone = selectedNode.Tag as CompositeBone;
                selectedBone.AddChildBone(newBone);
                RefreshTreeViewBoneStructure(null);
            }
        }

        private void CleanBoneInSceneControl(String boneRef)
        {
            if (compositeEntityEditorControl.SelectedBones.Contains(boneRef))
            {
                compositeEntityEditorControl.SelectedBones.Remove(boneRef);
            }
            if (compositeEntityEditorControl.HighlightedBone == boneRef)
            {
                compositeEntityEditorControl.HighlightedBone = null;
            }
            if (_lastSelectedBone == boneRef)
            {
                _lastSelectedBone = null;
            }
        }

        private void toolStripButtonDeleteBone_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeViewBones.SelectedNode;
            CompositeBone selectedBone = null;
            if (selectedNode != null)
            {
                selectedBone = selectedNode.Tag as CompositeBone;
                if (selectedBone == CompositeEntity.RootBone)
                {
                    return;
                }
                else
                {
                    selectedBone.ParentBone.RemoveChildBone(selectedBone);
                    CleanBoneInSceneControl(selectedBone.Name);
                }
                RefreshTreeViewBoneStructure(null);
            }
        }

        private void treeViewBones_AfterSelect(object sender, TreeViewEventArgs e)
        {
            bool enabledState = false;
            CompositeBone selectedBone = null;
            if (treeViewBones.SelectedNode != null)
            {
                enabledState = true;
                selectedBone = e.Node.Tag as CompositeBone;
                if (String.IsNullOrEmpty(selectedBone.SceneItem) == false 
                    && CompositeEntity.SceneItemBank.ContainsKey(selectedBone.SceneItem))
                {
                    SceneItem sceneItem = CompositeEntity.SceneItemBank[selectedBone.SceneItem];
                    if (sceneItem != null && sceneItem is ISubItemCollection)
                    {
                        SubItemRefConverter.SubItemsRefs = ((ISubItemCollection)sceneItem).GetSubItemsList();
                    }
                }
                propertyGridBoneProperties.SelectedObject = selectedBone;
            }
            else
            {
                propertyGridBoneProperties.SelectedObject = null; 
            }
            toolStripButtonAddChildBone.Enabled = enabledState;
            toolStripButtonDeleteBone.Enabled = enabledState
                && treeViewBones.SelectedNode != treeViewBones.Nodes[0];
            groupBoxBoneProps.Enabled = enabledState;
            toolStripButtonLevelUpBone.Enabled = enabledState && selectedBone != CompositeEntity.RootBone 
                && selectedBone != CompositeEntity.RootBone.ChildBones[0];
            toolStripButtonLevelDownBone.Enabled = enabledState && selectedBone != CompositeEntity.RootBone;
        }

        private void treeViewBones_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node != null)
            {
                String oldName = e.Node.Text;
                String newName = e.Label;
                Console.WriteLine("Renamed node " + oldName + " to " + newName);
                if (String.IsNullOrEmpty(newName) == false &&
                    IsBoneNodeNameUnique(CompositeEntity.RootBone, newName))
                {
                    CompositeBone bone = e.Node.Tag as CompositeBone;
                    bone.Name = newName;
                    CleanBoneInSceneControl(oldName);
                    // sync bone transforms with the new Bone name
                    for (int i = 0; i < CompositeEntity.Animations.Count; i++)
                    {
                        // loop through every keyframe to sync them
                        for (int j = 0; j < CompositeEntity.Animations[i].KeyFrames.Count; j++)
                        {
                            CompositeKeyFrame keyframe = CompositeEntity.Animations[i].KeyFrames[j];
                            // loop to find the previous bone
                            for (int k = 0; k < keyframe.BoneTransforms.Count; k++)
                            {
                                CompositeBoneTransform transform = keyframe.BoneTransforms[k];
                                if (transform.BoneReference == oldName)
                                {
                                    transform.BoneReference = bone.Name;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    e.CancelEdit = true;
                }
            }
        }

        private void toolStripButtonLevelUpBone_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeViewBones.SelectedNode;
            CompositeBone selectedBone = null;
            if (selectedNode != null)
            {
                selectedBone = selectedNode.Tag as CompositeBone;
                if (selectedBone == CompositeEntity.RootBone)
                {
                    return;
                }
                else
                {                   
                    int index = selectedBone.ParentBone.ChildBones.IndexOf(selectedBone);
                    // if it's the first element, upgrade the bone to the parent's level
                    if (index == 0)
                    {
                        CompositeBone grandpa = selectedBone.ParentBone.ParentBone;
                        int indexParent = grandpa.ChildBones.IndexOf(selectedBone.ParentBone);
                        selectedBone.ParentBone.ChildBones.Remove(selectedBone);
                        grandpa.ChildBones.Insert(indexParent, selectedBone);
                        selectedBone.ParentBone = grandpa;
                    }
                    // if it's inside the list, simply place it one level up in the list
                    else
                    {
                        selectedBone.ParentBone.ChildBones.Remove(selectedBone);
                        selectedBone.ParentBone.ChildBones.Insert(index - 1, selectedBone);                       
                    }
                }
                RefreshTreeViewBoneStructure(selectedBone);
            }
        }

        private void toolStripButtonLevelDownBone_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeViewBones.SelectedNode;
            CompositeBone selectedBone = null;
            if (selectedNode != null)
            {
                selectedBone = selectedNode.Tag as CompositeBone;
                if (selectedBone == CompositeEntity.RootBone)
                {
                    return;
                }
                else
                {                    
                    int index = selectedBone.ParentBone.ChildBones.IndexOf(selectedBone);
                    // if it's the last element, upgrade the bone to the parent's level
                    if (index == selectedBone.ParentBone.ChildBones.Count - 1)
                    {
                        CompositeBone grandpa = selectedBone.ParentBone.ParentBone;
                        int indexParent = grandpa.ChildBones.IndexOf(selectedBone.ParentBone);
                        selectedBone.ParentBone.ChildBones.Remove(selectedBone);
                        grandpa.ChildBones.Insert(indexParent, selectedBone);
                        selectedBone.ParentBone = grandpa;
                    }
                    // if it's inside the list, simply place it one level down in the list
                    else
                    {
                        selectedBone.ParentBone.ChildBones.Remove(selectedBone);
                        selectedBone.ParentBone.ChildBones.Insert(index + 1, selectedBone);
                    }
                }
                RefreshTreeViewBoneStructure(selectedBone);
            }
        }

        #endregion

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableKeyFrames.TableModel.Selections.Clear();
            tableBoneTransforms.TableModel.Rows.Clear();
        }

        private void toolStripButtonNewBoneFromItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeViewBones.SelectedNode;
            if (selectedNode == null)
            {
                selectedNode = treeViewBones.Nodes[0];
            }
            SceneItem selectedItem = treeViewSceneItems.SelectedNode.Tag as SceneItem;       
            CompositeBone newBone = new CompositeBone(selectedItem.Name, 
                GetUniqueBoneNodeName(selectedItem.Name));
            newBone.Parent = CompositeEntity;
            CompositeBone selectedBone = selectedNode.Tag as CompositeBone;
            selectedBone.AddChildBone(newBone);
            RefreshTreeViewBoneStructure(null);            
        }       
    }   
}
