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
using IceCream.Components;
using System.Xml.Serialization;
using System.Xml;
using IceCream.SceneItems;
using IceCream.SceneItems.AnimationClasses;

namespace IceCream
{
    public class IceScene : SceneBase
    {
        internal bool isInGame;     
        internal bool _hasBeenUpdatedOnce;

        #region Fields

        int _nextId = 0;
        internal List<SceneItem> _itemsToRegister = new List<SceneItem>();
        private List<Camera> _activeCameras = new List<Camera>();
        private Color _clearColor = Color.CornflowerBlue;
        private bool _willRenderNotActive = true;
        
        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or Sets the value of whether the scene will be rendered even if its not the active scene. 
        /// </summary>
        /// <remarks>Useful for background scenes.</remarks>
        public bool WillRenderNotActive 
        { 
            get { return _willRenderNotActive; } 
            set { _willRenderNotActive = value; } 
        }

        /// <summary>
        /// Gets or Sets the value of whether the Scene Is Enabled.
        /// </summary>
        /// <remarks>When a scene is not enabled it's items update will not be called</remarks>
        public bool Enabled 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the active cameras in the scene.
        /// <remarks>An IceScene should always have at least 1 camera.</remarks>
        /// </summary>
        public List<Camera> ActiveCameras
        {
            get { return _activeCameras; }
            set { _activeCameras = value; }
        }

        /// <summary>
        /// Gets or Sets the value of the color the scene will clear as before drawing any items.
        /// </summary>
        public Color ClearColor
        {
            get { return _clearColor; }
            set { _clearColor = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Instiantiates an instance of IceScene
        /// </summary>
        public IceScene()
        {
            
        }
       
        #endregion

        #region Methods

        internal void RegisterItems()
        {
            for (int i = 0; i < _itemsToRegister.Count; i++)
            {
                SceneItem item = _itemsToRegister[i];
                AddToSceneItems(item);
                item.OnRegister();
                _itemsToRegister.RemoveAt(i);
                i--;
            }

        }

        internal void AddToSceneItems(SceneItem item)
        {
            item.id = GetNewID();
            SceneItems.Add(item);
        }

        private int GetNewID()
        {
            _nextId++;
            return _nextId;
        }

        internal void Unload()
        {
            for (int i = 0; i < SceneItems.Count; i++)
            {
                SceneItems[i].OnUnRegister();
            }
            foreach (var item in SceneComponents)
            {
                item.OnUnRegister();
            }
        }
         
        /// <summary>
        /// Registers a sceneitem in the scene immediately.
        /// </summary>
        /// <param name="sceneItem"></param>
        public void RegisterSceneItemNow(SceneItem sceneItem)
        {
            if (sceneItem.Name.Equals(string.Empty))
                throw new ArgumentException("Scene item must have a name set");
            sceneItem.IsTemplate = false;
            sceneItem.SceneParent = this;
            
            AddToSceneItems(sceneItem);
            sceneItem.OnRegister();
        }

        /// <summary>
        /// Registers a sceneitem in the scene at the end of this update.
        /// </summary>
        /// <param name="sceneItem"></param>
        public void RegisterSceneItem(SceneItem sceneItem)
        {
            if (sceneItem.Name.Equals(string.Empty))
                throw new ArgumentException("Scene item must have a name set");
            sceneItem.IsTemplate = false;
            sceneItem.SceneParent = this;
            _itemsToRegister.Add(sceneItem);
        }

        /// <summary>
        /// Loads a Material given the location and assigning it the given name
        /// </summary>
        /// <param name="location">The file location of the texture file, do not include the file extension</param>
        /// <param name="name">The name to give the material</param>
        /// <returns>A IceCream Material</returns>
        public override Material LoadMaterial(string location, string name)
        {
            Material _material = new Material();
            _material.Name = name;
            _material.Filename = location;
            _material.Texture = _content.Load<Texture2D>(location);
            Materials.Add(_material);
            return _material;
        }

        /// <summary>
        /// Gets a Template SceneItem
        /// </summary>
        /// <remarks>If the template is not in this scene, it will try to retrieve from the global cache</remarks>
        /// <param name="name">The name of the template</param>
        /// <returns>A SceneItem type, if the template is not found, null is returned</returns>
        public SceneItem GetTemplate(string name)
        {
            foreach (SceneItem _obj in TemplateItems)
            {
                if (_obj.Name == name && _obj.IsTemplate)
                    return _obj;
            }
            foreach (SceneItem _obj in SceneManager.GlobalDataHolder.TemplateItems)
            {
                if (_obj.Name == name && _obj.IsTemplate)
                {
                    return _obj;
                }
            }
            return null;
        }

        /// <summary>
        /// Creates a copy of a template with the given name
        /// </summary>
        /// <param name="name">The name of the template to copy</param>
        /// <returns>A copied SceneItem, if the Template is not found, null is returned</returns>
        public SceneItem CreateCopy(string name)
        {
            SceneItem item = GetTemplate(name);
            if (item != null)
            {                
                return (SceneItem)CreateNewInstaceCopyOf(item);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a new instance of a given SceneItem using Reflection
        /// </summary>
        public SceneItem CreateNewInstaceCopyOf(SceneItem item)
        {
            SceneItem copy = (SceneItem)item.GetType().Assembly.CreateInstance(item.GetType().FullName);
            item.CopyValuesTo(copy);
            return copy;
        }

        /// <summary>
        /// Creates a copy of a template with the given name
        /// </summary>
        /// <typeparam name="T">The generic type of sceneitem to create</typeparam>
        /// <param name="name">The name of the template to copy</param>
        /// <returns>The copied SceneItem as the specified SceneItem generic type</returns>
        public T CreateCopy<T>(string name) where T : SceneItem, new()
        {
            //conkerjo- This method is incositent with the above copy method. we should make them the same            
            T _newObject = new T();
            foreach (SceneItem _obj in TemplateItems)
            {
                if (_obj.Name == name && _obj.IsTemplate)
                {
                    T _tmpObject = new T();// _obj as T;
                    _obj.CopyValuesTo(_newObject as SceneItem);
                    _newObject.IsTemplate = false;
                    _newObject.Components.Clear();
                    foreach (IceComponent _comp in _obj.Components)
                    {
                        _newObject.AddComponent((IceComponent)_comp.GetCopy());
                    }                    
                    return _newObject as T;
                }
            }
            foreach (SceneItem _obj in SceneManager.GlobalDataHolder.TemplateItems)
            {
                if (_obj.Name == name && _obj.IsTemplate)
                {
                    T _tmpObject = new T();// _obj as T;
                    _obj.CopyValuesTo(_newObject as SceneItem);
                    _newObject.IsTemplate = false;
                    _newObject.Components.Clear();
                    foreach (IceComponent _comp in _obj.Components)
                    {
                        _newObject.AddComponent((IceComponent)_comp.GetCopy());
                    }
                    return _newObject as T;
                }
            }
            throw new ArgumentException("The template of name " + name + " cannot be found");
        }

        /// <summary>
        /// Adds a template sceneitem to the templates collection
        /// </summary>
        /// <param name="sceneItem">The scene item to add</param>
        public void AddTemplate(SceneItem sceneItem)
        {
            sceneItem.IsTemplate = true;
            TemplateItems.Add(sceneItem);
        }

        internal void RemoveItems()
        {
            for (int i = 0; i < SceneItems.Count; i++)
            {
                if (SceneItems[i].MarkForDelete)
                {
                    SceneItems[i].OnUnRegister();
                    SceneItems[i].SceneParent = null;
                    SceneItems.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Gets a SceneItem from the SceneItems collection given its name
        /// </summary>
        /// <param name="name">The name of the scene item to get</param>
        /// <returns></returns>
        public SceneItem GetSceneItem(string name)
        {
            foreach (SceneItem _obj in SceneItems)
            {
                if (_obj.Name == name && !_obj.IsTemplate)
                {
                    return _obj;
                }
            }
            throw new ArgumentException("SceneItem with the name " + name + " cannot be found");
        }

        /// <summary>
        /// Returns a component of specified type. Use this to quickly get a component from the object
        /// </summary>
        /// <typeparam name="T">The generic type of IceSceneComponent to get</typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : IceSceneComponent
        {
            T local = default(T);
            foreach (IceSceneComponent component in SceneComponents)
            {
                local = component as T;
                if (local != null)
                {
                    return local;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Gets a sceneitem from the SceneItems collection given its name and generic type
        /// </summary>
        /// <typeparam name="T">The generic type of SceneItem to get</typeparam>
        /// <param name="name">The name of the sceneitem to get</param>
        /// <returns>Returns the SceneItem given its name and type</returns>
        public T GetSceneItem<T>(string name) where T : SceneItem
        {
            T local = default(T);
            
            foreach (SceneItem _obj in SceneItems)
            {
                if (_obj.Name == name && !_obj.IsTemplate)
                {
                    local = _obj as T;
                    if (local != null)
                    {
                        return local;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Adds an IceSceneComponent to the Scenecomponents collection
        /// </summary>
        /// <param name="component">The IceSceneComponent to add</param>
        public void AddComponent(IceSceneComponent component)
        {
            //conkerjo. i think we should check if it's already owned by a scene and throw an exception here

            component.SetOwner(this);
            SceneComponents.Add(component);
        }

        #endregion        
    }    
}
