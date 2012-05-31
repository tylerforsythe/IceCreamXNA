using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using System.Windows.Forms;
using IceCream;

namespace MilkshakeLibrary
{
    public class SceneItemEditor : Form
    {
        #region Fields

        private bool _itemIsSceneInstance;
        private Vector2 _itemInitialPosition;
        private Vector2 _itemInitialPivot;
        private Vector2 _itemInitialScale;
        private float _itemInitialRotation;
        private bool _initialVisibility;
        private bool _previewItemOnScene;
        private SceneItem _sceneItemCopy;
        private SceneItem _sceneItemBackup;
        private SceneItem _originalSceneItemReference;        

        #endregion

        #region Properties

        public virtual SceneItem SceneItem
        {
            get { return _sceneItemCopy; }
            set 
            { 
                _originalSceneItemReference = value;
                StoreCopyOfSceneItem(_originalSceneItemReference);
                StoreInitialValues(_originalSceneItemReference);
                _sceneItemCopy.Visible = true;
                _sceneItemCopy.Position = Vector2.Zero;
                _sceneItemCopy.Scale = Vector2.One;
                _sceneItemCopy.Rotation = 0;
                _sceneItemCopy.Pivot = Vector2.One;
            }
        }

        public virtual bool PreviewItemOnScene
        {
            get { return _previewItemOnScene; }
            set 
            {
                if (value != _previewItemOnScene)
                {
                    _previewItemOnScene = value;
                    // if disabling, restore the backup
                    if (value == false && _originalSceneItemReference != null)
                    {
                        _sceneItemBackup.CopyValuesTo(_originalSceneItemReference);
                    }
                }
            }
        }

        public bool ItemIsLocal
        {
            get { return _itemIsSceneInstance; }
            set { _itemIsSceneInstance = value; }
        }


        #endregion

        #region Constructor

        public SceneItemEditor()
        {
            _previewItemOnScene = false;
        }

        #endregion

        #region Methods

        public virtual void Update(float elapsedTime)
        {
            if (this.PreviewItemOnScene == true)
            {
                ApplyCopyToOriginal();
            }
        }

        private void StoreInitialValues(SceneItem item)
        {
            _itemInitialPosition = item.Position;
            _itemInitialPivot = item.Pivot;
            _initialVisibility = item.Visible;
            _itemInitialRotation = item.Rotation;
            _itemInitialScale = item.Scale;
        }

        private void StoreCopyOfSceneItem(SceneItem item)
        {
            _sceneItemCopy = (SceneItem)item.GetType().Assembly.CreateInstance(
                item.GetType().FullName, true);
            _sceneItemBackup = (SceneItem)item.GetType().Assembly.CreateInstance(
                item.GetType().FullName, true);
            item.CopyValuesTo(_sceneItemCopy);
            item.CopyValuesTo(_sceneItemBackup);
        }

        protected void ApplyCopyToOriginal()
        {            
            // Copy the values of the edited item in the original one
            _sceneItemCopy.CopyValuesTo(_originalSceneItemReference);
            // Restore initial values 
            _originalSceneItemReference.Pivot = _itemInitialPivot;
            _originalSceneItemReference.Position = _itemInitialPosition;
            _originalSceneItemReference.Visible = _initialVisibility;
            _originalSceneItemReference.Rotation = _itemInitialRotation;
            _originalSceneItemReference.Scale = _itemInitialScale;
        }

        protected void CancelEdition()
        {
            _sceneItemBackup.CopyValuesTo(_originalSceneItemReference);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                ApplyCopyToOriginal();
            }
            else
            {
                CancelEdition();
            }
            base.OnClosed(e);
        }

        #endregion
    }
}
