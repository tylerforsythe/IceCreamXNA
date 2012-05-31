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
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Globalization;
using IceCream.Components;
using IceCream.SceneItems.ParticlesClasses;
using IceCream.Drawing;
using IceCream.SceneItems;
using System.Diagnostics;
using IceCream.SceneItems.AnimationClasses;
using IceCream.SceneItems.TileGridClasses;

namespace IceCream.Serialization
{
    public delegate void StatusUpdateHandler(string update, int progress);
    public class SceneSerializer
    {
#if WINDOWS
        public static event StatusUpdateHandler StatusUpdate;
#endif
        public static void FireStatusUpdate(string update, int progress)
        {
#if WINDOWS
            if (StatusUpdate != null)
                StatusUpdate(update, progress);
#endif
        }
        static class NodeNames
        {
            public const string ICESCENE = "IceScene";
            public const string ASSETS = "Assets";
            public const string MATERIAL = "Material";
            public const string FONT = "Font";
            public const string COMPONENTS = "Components";
            public const string SCENEITEMS = "SceneItems";
            public const string TEMPLATES = "Templates";
            public const string ANIMATEDSPRITE = "AnimatedSprite";
            public const string PARTICLEEFFECT = "ParticleEffect";
            public const string TILESHEET = "TileSheet";
            public const string TILEGRID = "TileGrid";
            public const string COMPOSITEENTITY = "CompositeEntity";
            public const string SPRITE = "Sprite";
            public const string SCENEITEM = "SceneItem";
            public const string POSTPROCESSANIMATION = "PostProcessAnimation";
            public const string TEXTITEM = "TextItem";


        }
        static SceneBase _loadingScene;
        internal static String _rootPath = "";

        #region Serializations

        public static void SerializeScene(SceneBase scene, string filename)
        {
            try
            {
                XmlDocument _doc = new XmlDocument();
                XmlNode _rootNode = _doc.CreateElement(NodeNames.ICESCENE);
                _doc.AppendChild(_rootNode);
                if (scene.Equals(SceneManager.GlobalDataHolder))
                {
                    GlobalDataHolder globalDataHolder = scene as GlobalDataHolder;
                    XmlNode _res = _doc.CreateElement("NativeResolution");
                    _res.AppendChild(_doc.CreateElement("X")).InnerText = globalDataHolder.NativeResolution.X.ToString();
                    _res.AppendChild(_doc.CreateElement("Y")).InnerText = globalDataHolder.NativeResolution.Y.ToString();
                    _rootNode.AppendChild(_res);
                }
                _rootNode.AppendChildIfNotNull(WriteAssets(_doc, scene));
                _rootNode.AppendChildIfNotNull(WriteSceneComponents(_doc, scene));
                _rootNode.AppendChildIfNotNull(WriteSceneItems(_doc, scene));
                _rootNode.AppendChildIfNotNull(WriteTemplateItems(_doc, scene));

                _doc.Save(filename);
                _doc = null;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private static XmlNode WriteAssets(XmlDocument doc, SceneBase scene)
        {
            XmlNode assetsNode = doc.CreateElement(NodeNames.ASSETS);
            XmlNode assetNode = null;
            foreach (Material mat in scene.Materials)
            {
                assetNode = doc.CreateElement(NodeNames.MATERIAL);
                WriteMaterial(doc, assetNode, mat);
                assetsNode.AppendChildIfNotNull(assetNode);
            }
            foreach (IceFont font in scene.Fonts)
            {
                assetNode = doc.CreateElement(NodeNames.FONT);
                SerializeAsset(doc, assetNode, font);
                assetsNode.AppendChildIfNotNull(assetNode);
            }
			#if !XNATOUCH
            foreach (IceEffect mat in scene.Effects)
            {
                assetNode = doc.CreateElement("Effect");
                SerializeAsset(doc, assetNode, mat);
                string name = mat.Effects[0].GetType().Name;
                assetNode.Attributes.Append(doc.CreateAttribute("type")).InnerText = mat.GetType().FullName;
                assetsNode.AppendChildIfNotNull(assetNode);
            }
#endif
            foreach (TileSheet tileSheet in scene.TileSheets)
            {
                assetNode = doc.CreateElement(NodeNames.TILESHEET);
                WriteTileSheet(assetNode, doc, tileSheet);
                assetsNode.AppendChildIfNotNull(assetNode);
            }
            return assetsNode;
        }

        private static void SerializeAsset(XmlDocument doc, XmlNode assetNode, IceAsset asset)
        {
            assetNode.Attributes.Append(doc.CreateAttribute("name")).InnerText = asset.Name;
            assetNode.Attributes.Append(doc.CreateAttribute("location")).InnerText = asset.Filename;
        }

        private static void WriteMaterial(XmlDocument doc, XmlNode materialNode, Material material)
        {
            SerializeAsset(doc, materialNode, (IceAsset)material);
            if (String.IsNullOrEmpty(material.AreasDefinitionFilename) == false)
            {
                materialNode.Attributes.Append(doc.CreateAttribute("areas_definition"))
                    .InnerText = material.AreasDefinitionFilename;
            }
        }

        private static void WriteTileSheet(XmlNode itemNode, XmlDocument doc, TileSheet tileSheet)
        {
            itemNode.Attributes.Append(doc.CreateAttribute("materialSource")).InnerText = GetAssetSource(tileSheet.Material);
            itemNode.Attributes.Append(doc.CreateAttribute("materialRef")).InnerText = tileSheet.Material.Name;
            SerializeAsset(doc, itemNode, tileSheet);
            itemNode.AppendChildIfNotNull(WriteProperty("TileSize", tileSheet, doc));
            itemNode.AppendChildIfNotNull(WriteProperty("EnableCollisionByDefault", tileSheet, doc));
            itemNode.AppendChildIfNotNull(WriteProperty("UseTilingSafeBorders", tileSheet, doc));
            XmlNode _polys = itemNode.AppendChild(doc.CreateElement("Polygons"));
            foreach (Polygon polygon in tileSheet.Polygons)
            {
                XmlNode _poly = _polys.AppendChild(doc.CreateElement("Polygon"));
                if (polygon == null)
                {
                    _poly.InnerText = "n";
                }
                else
                {
                    _poly.InnerText = polygon.ToString();
                }
            }
        }

        private static XmlNode WriteSceneComponents(XmlDocument doc, SceneBase scene)
        {
            XmlNode _componentsNode = doc.CreateElement(NodeNames.COMPONENTS);
            foreach (var comp in scene.SceneComponents)
            {
                XmlNode _componentNode = doc.CreateElement(comp.GetType().FullName);
                WriteSceneComponent(_componentNode, comp, doc);
                _componentsNode.AppendChildIfNotNull(_componentNode);
            }
            return _componentsNode;
        }

        private static XmlNode WriteSceneItems(XmlDocument doc, SceneBase scene)
        {
            XmlNode _sceneItemsNode = doc.CreateElement(NodeNames.SCENEITEMS);
            WritesSceneCollection(doc, _sceneItemsNode, scene.SceneItems);
            return _sceneItemsNode;
        }

        private static XmlNode WriteTemplateItems(XmlDocument doc, SceneBase scene)
        {
            XmlNode _templatesNode = doc.CreateElement(NodeNames.TEMPLATES);
            WritesSceneCollection(doc, _templatesNode, scene.TemplateItems);
            return _templatesNode;
        }

        private static void WriteSceneComponent(XmlNode componentNode, IceSceneComponent comp, XmlDocument doc)
        {
            foreach (var _property in comp.GetType().GetProperties())
            {
                if (_property.GetCustomAttributes(typeof(IceCream.Attributes.IceComponentPropertyAttribute), true).Length > 0)
                {
                    componentNode.AppendChildIfNotNull(WriteProperty(_property.Name, comp, doc));
                }
            }
        }

        private static void WriteSceneComponent(XmlNode sceneComponent, IceComponent comp, XmlDocument doc)
        {
            foreach (var _property in comp.GetType().GetProperties())
            {
                if (_property.GetCustomAttributes(typeof(IceCream.Attributes.IceComponentPropertyAttribute), true).Length > 0)
                {
                    sceneComponent.AppendChildIfNotNull(WriteProperty(_property.Name, comp, doc));
                }
            }
        }

        private static void PrintPropertiesOfType(Type type, String logo)
        {
            PropertyInfo[] _properties = type.GetProperties();
            foreach (PropertyInfo _prop in _properties)
            {
                Type _propType = _prop.PropertyType.UnderlyingSystemType;
                object[] _attribs = _prop.GetCustomAttributes(typeof(System.Xml.Serialization.XmlIgnoreAttribute), true);
                if (_attribs.Length == 0 && _propType.Name != "Parent")
                {
                    if (_prop.CanWrite == true && _prop.CanRead == true)
                        PrintPropertiesOfType(_propType, logo + "-");
                }
            }
        }

        private static void WritesSceneCollection(XmlDocument doc, XmlNode rootnode, List<SceneItem> sceneItems)
        {
            XmlNode _itemNode = null;
            foreach (SceneItem item in sceneItems)
            {
                Type _sceneItemType = item.GetType();
                _itemNode = WriteSceneItem(doc, rootnode, _itemNode, item);
            }
        }

        private static XmlNode WriteSceneItem(XmlDocument doc, XmlNode rootnode, XmlNode _itemNode, SceneItem item)
        {
            //PrintPropertiesOfType(_sceneItemType, "-");
            _itemNode = null;

            if (item is AnimatedSprite)
            {
                _itemNode = doc.CreateElement(NodeNames.ANIMATEDSPRITE);
                WriteSceneItemBase(_itemNode, doc, item);
                WriteSpriteItemBase(_itemNode, doc, (Sprite)item);
                WriteAnimatedSpriteItemBase(_itemNode, doc, (AnimatedSprite)item);
            }
            else if (item is ParticleEffect)
            {
                _itemNode = doc.CreateElement(NodeNames.PARTICLEEFFECT);

                WriteParticleEffect(_itemNode, doc, (ParticleEffect)item);
                WriteSceneItemBase(_itemNode, doc, item);
            }
            else if (item is PostProcessAnimation)
            {
                _itemNode = doc.CreateElement(NodeNames.POSTPROCESSANIMATION);

                WriteSceneItemBase(_itemNode, doc, item);
                WritePostProcess(_itemNode, doc, item);
            }            
            else if (item is TileGrid)
            {
                _itemNode = doc.CreateElement(NodeNames.TILEGRID);
                WriteSceneItemBase(_itemNode, doc, item);
                WriteSpriteItemBase(_itemNode, doc, (Sprite)item);
                WriteTileGrid(_itemNode, doc, (TileGrid)item);
            }
            else if (item is CompositeEntity)
            {
                _itemNode = doc.CreateElement(NodeNames.COMPOSITEENTITY);
                WriteSceneItemBase(_itemNode, doc, item);
                WriteCompositeEntity(_itemNode, doc, (CompositeEntity)item);
            }
            else if (item is Sprite)
            {
                _itemNode = doc.CreateElement(NodeNames.SPRITE);
                WriteSceneItemBase(_itemNode, doc, item);
                WriteSpriteItemBase(_itemNode, doc, (Sprite)item);
            }
            else if (item is TextItem)
            {
                _itemNode = doc.CreateElement(NodeNames.TEXTITEM);
                WriteSceneItemBase(_itemNode, doc, item);
                WriteTextItem(_itemNode, doc, (TextItem)item);
            }
            else
            {
                _itemNode = doc.CreateElement(NodeNames.SCENEITEM);
                WriteSceneItemBase(_itemNode, doc, item);
            }

            _itemNode.AppendChildIfNotNull(WriteSceneItemComponents(doc, item));
            _itemNode.AppendChildIfNotNull(WriteSceneItemLinkPoints(doc, item));
            _itemNode.AppendChildIfNotNull(WriteSceneItemMounts(doc, item));
            rootnode.AppendChildIfNotNull(_itemNode);
            return _itemNode;
        }

        private static void WritePostProcess(XmlNode _itemNode, XmlDocument doc, SceneItem item)
        {
			#if !XNATOUCH
            PostProcessAnimation pp = (PostProcessAnimation)item;
            XmlAttribute att = doc.CreateAttribute("effectSource");
            att.Value = pp.IceEffect.Scope.ToString().ToUpper();
            _itemNode.Attributes.Append(att);

            XmlAttribute att1 = doc.CreateAttribute("effectRef");
            att1.Value = pp.IceEffect.Name.ToString();
            _itemNode.Attributes.Append(att1);

            XmlAttribute att2 = doc.CreateAttribute("effectType");
            att2.Value = pp.IceEffect.GetType().FullName;
            _itemNode.Attributes.Append(att2);

            WriteIAnimationProperties(doc, _itemNode, pp);

            XmlNode _properties = _itemNode.AppendChild(doc.CreateElement("Properties"));
            foreach (var item1 in pp.LinearProperties)
            {
                XmlNode _node = _properties.AppendChild(doc.CreateElement("Property"));


                LinearProperty lin = item1;
                _node.AppendChild(doc.CreateElement("Description")).InnerText = lin.Description;
                _node.AppendChild(doc.CreateElement("LowerBound")).InnerText = lin.LowerBound.ToString(CultureInfo.InvariantCulture);
                _node.AppendChild(doc.CreateElement("UpperBound")).InnerText = lin.UpperBound.ToString(CultureInfo.InvariantCulture);
                XmlNode _vals = _node.AppendChild(doc.CreateElement("Values"));
                foreach (Vector2 item2 in lin.Values)
                {
                    XmlNode _val = _vals.AppendChild(doc.CreateElement("Value"));
                    _val.AppendChild(doc.CreateElement("X")).InnerText = item2.X.ToString(CultureInfo.InvariantCulture);
                    _val.AppendChild(doc.CreateElement("Y")).InnerText = item2.Y.ToString(CultureInfo.InvariantCulture);
                }
            }
#endif
        }

        private static XmlNode WriteSceneItemMounts(XmlDocument doc, SceneItem item)
        {
            XmlNode sceneItemsNode = doc.CreateElement("Mounts");

            //foreach (var comp in item.IsMounted)
            //{
            //    XmlNode compNode = _doc.CreateElement("Mount");
            //    if (comp.linkChild._isTemplate)
            //    {
            //        //compNode.Attributes.Append(_doc.CreateAttribute("templateItemRef")).InnerText = comp.linkChild._ownerName;
            //    }
            //    else
            //    {
            //        //compNode.Attributes.Append(_doc.CreateAttribute("sceneItemRef")).InnerText = comp.linkChild._ownerName;
            //    }
            //    compNode.Attributes.Append(_doc.CreateAttribute("targetLink")).InnerText = comp.linkChild.Name;
            //    compNode.Attributes.Append(_doc.CreateAttribute("sourceLink")).InnerText = comp.linkOwner.Name;
            //    sceneItemsNode.AppendChild(compNode);
            //}
            return sceneItemsNode;
        }

        private static XmlNode WriteSceneItemLinkPoints(XmlDocument doc, SceneItem item)
        {
            XmlNode sceneItemsNode = doc.CreateElement("LinkPoints");
            foreach (var comp in item.LinkPoints)
            {
                XmlNode compNode = doc.CreateElement("LinkPoint");
                compNode.AppendChildIfNotNull(WriteProperty("Name", comp, doc));
                compNode.AppendChildIfNotNull(WriteProperty("Offset", comp, doc));
                if (comp.MountedChildLinkPoints.Count > 0)
                {
                    XmlNode mounts = compNode.AppendChild(doc.CreateElement("Mounts"));
                    foreach (var mount in comp.MountedChildLinkPoints)
                    {
                        XmlNode mountnode = mounts.AppendChild(doc.CreateElement("Mount"));
                        mountnode.AppendChild(doc.CreateElement("ChildSceneItem")).InnerText = mount.Owner.Name;
                        mountnode.AppendChild(doc.CreateElement("ChildLinkPoint")).InnerText = mount.Name;
                    }
                }
                //compNode.AppendChild(WriteProperty("LinkPosition", comp, _doc));
                //compNode.AppendChild(WriteProperty("LinkRotation", comp, _doc));
                sceneItemsNode.AppendChildIfNotNull(compNode);
            }
            return sceneItemsNode;
        }

        private static void WriteTextItem(XmlNode itemNode, XmlDocument doc, TextItem text)
        {
            string _source = GetAssetSource(text.Font);
            itemNode.Attributes.Append(doc.CreateAttribute("fontSource")).InnerText = _source;
            itemNode.Attributes.Append(doc.CreateAttribute("fontRef")).InnerText = text.Font.Name;
            itemNode.AppendChildIfNotNull(WriteProperty("Text", text, doc));
            itemNode.AppendChildIfNotNull(WriteProperty("AutoCenterPivot", text, doc));
            itemNode.AppendChildIfNotNull(WriteProperty("Scale", text, doc));
            itemNode.AppendChildIfNotNull(WriteProperty("Tint", text, doc));
            itemNode.AppendChildIfNotNull(WriteProperty("Shadow", text, doc));
        }

        private static string GetAssetSource(IceAsset asset)
        {
            if (asset.Parent == SceneManager.GlobalDataHolder)
                return "GLOBAL";
            else if (asset.Parent == null)
                return "EMBEDDED";
            else if (asset.Parent == SceneManager.ActiveScene)
                return "LOCAL";
            else
                return "NULL";
        }

        private static XmlNode WriteSceneItemComponents(XmlDocument doc, SceneItem scene)
        {
            XmlNode sceneItemsNode = doc.CreateElement(NodeNames.COMPONENTS);
            foreach (var _comp in scene.Components)
            {
                XmlNode _compNode = doc.CreateElement(_comp.GetType().FullName);
                WriteSceneComponent(_compNode, _comp, doc);
                sceneItemsNode.AppendChildIfNotNull(_compNode);
            }
            return sceneItemsNode;
        }

        private static void WriteTileGrid(XmlNode itemNode, XmlDocument doc, TileGrid tileGrid)
        {
            if (tileGrid.TileSheet != null)
            {                
                String scope;
                if (tileGrid.TileSheet.Scope == AssetScope.Global)
                {
                    scope = "GLOBAL";
                }
                else
                {
                    scope = "LOCAL";
                }
                itemNode.Attributes.Append(doc.CreateAttribute("tileSheetSource")).Value = scope;
                itemNode.Attributes.Append(doc.CreateAttribute("tileSheetRef")).Value = tileGrid.TileSheet.Name;                
            }
            XmlNode _gridSettings = itemNode.AppendChild(doc.CreateElement("TileGridSettings"));
            _gridSettings.AppendChildIfNotNull(WriteProperty("UseTilingSafeBorders", tileGrid, doc));
            _gridSettings.AppendChildIfNotNull(WriteProperty("TileRows", tileGrid, doc));
            _gridSettings.AppendChildIfNotNull(WriteProperty("TileCols", tileGrid, doc));
            _gridSettings.AppendChildIfNotNull(WriteProperty("TileSize", tileGrid, doc));

            XmlNode _layers = _gridSettings.AppendChild(doc.CreateElement("TileLayers"));
            foreach (TileLayer layer in tileGrid.TileLayers)
            {
                XmlNode _layer = _layers.AppendChild(doc.CreateElement("TileLayer"));
                _layer.Attributes.Append(doc.CreateAttribute("Name")).Value = layer.Name;
                _layer.Attributes.Append(doc.CreateAttribute("Order")).Value = "1";
                _layer.Attributes.Append(doc.CreateAttribute("Visible")).Value = layer.Visible.ToString();
                layer.ConstructData();
                _layer.InnerText = layer.TileData;
            }
        }

        private static void WriteCompositeEntity(XmlNode itemNode, XmlDocument doc, CompositeEntity compositeEntity)
        {
            XmlNode entity = itemNode.AppendChild(doc.CreateElement("CompositeEntityData"));
            entity.AppendChildIfNotNull(WriteProperty("Animations", compositeEntity, doc));
            entity.AppendChildIfNotNull(WriteProperty("SceneItemBank", compositeEntity, doc));
            entity.AppendChildIfNotNull(WriteProperty("RootBone", compositeEntity, doc));
            WriteIAnimationDirectorProperties(doc, entity, compositeEntity as IAnimationDirector);
        }

        private static void WriteParticleEffect(XmlNode itemNode, XmlDocument doc, ParticleEffect particleEffect)
        {

            itemNode.AppendChildIfNotNull(WriteProperty("EditorBackgroundColor", particleEffect, doc));
            WriteIAnimationProperties(doc, itemNode, particleEffect);
            //XmlNode _emitternode=itemNode.AppendChild(_doc.CreateElement("Emitter"));
            try
            {

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                MemoryStream ms = new MemoryStream();
                XmlSerializer ser = new XmlSerializer(typeof(Emitter), "");
                XmlWriterSettings sett = new XmlWriterSettings();
                sett.OmitXmlDeclaration = true;
                sett.Indent = true;
                XmlWriter writer = XmlTextWriter.Create(ms, sett);
                ser.Serialize(writer, particleEffect.Emitter, ns);
                ms.Flush();
                ms.Position = 0;

                XmlDocument doc1 = new XmlDocument();
                doc1.Load(ms);

                XmlNode newNode = doc.ImportNode(doc1.SelectSingleNode("Emitter"), true);

                int i = 0;
                XmlNode _node = newNode.SelectSingleNode("ParticleTypes");
                foreach (XmlNode _child in _node.ChildNodes)
                {
                    ParticleType _type = particleEffect.Emitter.ParticleTypes[i];
                    XmlNode _mat = WriteProperty("Material", _type, doc);
                    _mat.Attributes.Append(doc.CreateAttribute("materialSource")).InnerText = GetAssetSource(_type.Material);

                    _child.AppendChildIfNotNull(_mat);
                    //_node.AppendChild(_child);
                    i++;
                }

                itemNode.AppendChildIfNotNull(newNode);
                ms.Close();

            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
                throw er;
            }
        }

        private static void WriteAnimatedSpriteItemBase(XmlNode itemNode, XmlDocument _doc, AnimatedSprite item)
        {
            XmlNode _animList = itemNode.AppendChild(_doc.CreateElement("Animations"));
            foreach (AnimationInfo info in item.Animations)
            {
                WriteAnimationInfo(_doc, _animList, info);
            }
            WriteIAnimationDirectorProperties(_doc, itemNode, item as IAnimationDirector);
        }

        private static void WriteAnimationInfo(XmlDocument _doc, XmlNode itemNode, AnimationInfo animationInfo)
        {
            XmlNode _anim = itemNode.AppendChild(_doc.CreateElement("Animation"));
            _anim.Attributes.Append(_doc.CreateAttribute("Name")).InnerText = animationInfo.Name;
            WriteIAnimationProperties(_doc, _anim, animationInfo as IAnimation);
            _anim.AppendChildIfNotNull(WriteProperty("AnimationFrames", animationInfo, _doc));
        }

        private static void WriteSpriteItemBase(XmlNode itemNode, XmlDocument doc, Sprite item)
        {
            itemNode.Attributes.Append(doc.CreateAttribute("materialSource")).InnerText = GetAssetSource(item.Material);
            itemNode.Attributes.Append(doc.CreateAttribute("materialRef")).InnerText = item.Material.Name;
            if (String.IsNullOrEmpty(item.MaterialArea) == false)
            {
                itemNode.Attributes.Append(doc.CreateAttribute("materialArea")).InnerText = item.MaterialArea;
            }
            else if (item.SourceRectangle != null)
            {
                itemNode.AppendChildIfNotNull(WriteProperty("SourceRectangle", item, doc));
            }
            itemNode.AppendChildIfNotNull(WriteProperty("Tint", item, doc));
            itemNode.AppendChildIfNotNull(WriteProperty("BlendingType", item, doc));
            itemNode.AppendChildIfNotNull(WriteProperty("UseTilingSafeBorders", item, doc));
        }

        private static void WriteSceneItemBase(XmlNode node, XmlDocument doc, SceneItem item)
        {
            node.Attributes.Append(doc.CreateAttribute("name")).InnerText = item.Name;
            node.AppendChildIfNotNull(WriteProperty("Position", item, doc));
            node.AppendChildIfNotNull(WriteProperty("Pivot", item, doc));
            node.AppendChildIfNotNull(WriteProperty("IsPivotRelative", item, doc));
            node.AppendChildIfNotNull(WriteProperty("IgnoreCameraPosition", item, doc));
            node.AppendChildIfNotNull(WriteProperty("Rotation", item, doc));
            node.AppendChildIfNotNull(WriteProperty("Scale", item, doc));
            node.AppendChildIfNotNull(WriteProperty("Opacity", item, doc));
            node.AppendChildIfNotNull(WriteProperty("IsTemplate", item, doc));
            node.AppendChildIfNotNull(WriteProperty("Layer", item, doc));
            node.AppendChildIfNotNull(WriteProperty("Visible", item, doc));
            node.AppendChildIfNotNull(WriteProperty("FlipHorizontal", item, doc));
            node.AppendChildIfNotNull(WriteProperty("FlipVertical", item, doc));
        }

        private static void WriteIAnimationDirectorProperties(XmlDocument _doc, XmlNode itemNode,
            IAnimationDirector director)
        {
            itemNode.AppendChildIfNotNull(WriteProperty("AutoPlay", director, _doc));
            itemNode.AppendChildIfNotNull(WriteProperty("DefaultAnimation", director, _doc));
        }

        private static void WriteIAnimationProperties(XmlDocument _doc, XmlNode itemNode, IAnimation animation)
        {
            itemNode.AppendChildIfNotNull(WriteProperty("LoopMax", animation, _doc));
            itemNode.AppendChildIfNotNull(WriteProperty("Life", animation, _doc));
            itemNode.AppendChildIfNotNull(WriteProperty("AutoPlay", animation, _doc));
            itemNode.AppendChildIfNotNull(WriteProperty("HideWhenStopped", animation, _doc));            
        }

        private static XmlNode WriteProperty(string name, object item, XmlDocument doc)
        {
            return WriteProperty(name, item, doc, null);
        }

        private static XmlNode WriteProperty(string name, object item, XmlDocument doc, object defaultValue)
        {
            XmlNode _node = doc.CreateElement(name);
            PropertyInfo _prop = item.GetType().GetProperty(name);
            if (_prop.CanRead && _prop.CanWrite)
            {
                object objectValue = _prop.GetValue(item, null);
                if (defaultValue != null)
                {
                    if (objectValue.Equals(defaultValue))
                    return null;
                }
                if (_prop.PropertyType == typeof(Vector2))
                {
                    Vector2 _vector = (Vector2)_prop.GetValue(item, null);
                    if (defaultValue == null && _vector.X == 0 && _vector.Y == 0)
                    {
                        return null;
                    }
                    _node.AppendChild(doc.CreateElement("X")).InnerText = _vector.X.ToString(CultureInfo.InvariantCulture);
                    _node.AppendChild(doc.CreateElement("Y")).InnerText = _vector.Y.ToString(CultureInfo.InvariantCulture);
                }
                else if (_prop.PropertyType == typeof(List<Vector2>))
                {
                    XmlNode _con = _node;
                    List<Vector2> poly = (List<Vector2>)_prop.GetValue(item, null);
                    foreach (Vector2 vec in poly)
                    {
                        XmlNode _vecNode = _con.AppendChild(doc.CreateElement("Vector"));
                        _vecNode.AppendChild(doc.CreateElement("X")).InnerText = vec.X.ToString(CultureInfo.InvariantCulture);
                        _vecNode.AppendChild(doc.CreateElement("Y")).InnerText = vec.Y.ToString(CultureInfo.InvariantCulture);
                    }
                }
                else if (_prop.PropertyType == typeof(List<CompositeAnimation>))
                {
                    XmlNode _con = _node;
                    List<CompositeAnimation> compList = (List<CompositeAnimation>)_prop.GetValue(item, null);
                    foreach (CompositeAnimation vec in compList)
                    {
                        XmlNode _vecNode = _con.AppendChild(doc.CreateElement("CompositeAnimation"));
                        _vecNode.Attributes.Append(doc.CreateAttribute("Name")).InnerText = vec.Name;
                        _vecNode.AppendChildIfNotNull(WriteProperty("LerpLastFrameWithFirst", vec, doc));
                        _vecNode.AppendChildIfNotNull(WriteProperty("Speed", vec, doc));
                        WriteIAnimationProperties(doc, _vecNode, vec as IAnimation);
                        _vecNode.AppendChildIfNotNull(WriteProperty("KeyFrames", vec, doc));
                    }
                }
                else if (_prop.PropertyType == typeof(Dictionary<string, SceneItem>))
                {
                    XmlNode _con = _node;
                    Dictionary<string, SceneItem> compList = (Dictionary<string, SceneItem>)_prop.GetValue(item, null);
                    foreach (var vec in compList)
                    {
                        XmlNode _vecNode = _con.AppendChild(doc.CreateElement("SceneItemBankItem"));
                        _vecNode.Attributes.Append(doc.CreateAttribute("Key")).InnerText = vec.Key;

                        XmlNode _spriteNode = null;
                        WriteSceneItem(doc, _vecNode, _spriteNode, vec.Value);
                    }
                }
                else if (_prop.PropertyType == typeof(List<AnimationFrame>))
                {          
                    List<AnimationFrame> frameList = (List<AnimationFrame>)_prop.GetValue(item, null);
                    foreach (AnimationFrame frame in frameList)
                    {
                        XmlNode frameNode = _node.AppendChild(doc.CreateElement("Frame"));
                        frameNode.Attributes.Append(doc.CreateAttribute("Duration")).InnerText
                            = frame.Duration.ToString(CultureInfo.InvariantCulture);
                        frameNode.Attributes.Append(doc.CreateAttribute("Area")).InnerText
                            = frame.Area;
                    }
                }
                else if (_prop.PropertyType == typeof(List<CompositeKeyFrame>))
                {
                    XmlNode _con = _node;// _node.AppendChild(doc.CreateElement(_prop.Name));
                    List<CompositeKeyFrame> compList = (List<CompositeKeyFrame>)_prop.GetValue(item, null);
                    foreach (CompositeKeyFrame vec in compList)
                    {
                        XmlNode _vecNode = _con.AppendChild(doc.CreateElement("CompositeKeyFrame"));
                        _vecNode.Attributes.Append(doc.CreateAttribute("Name")).InnerText = vec.Name;
                        _vecNode.Attributes.Append(doc.CreateAttribute("Duration")).InnerText
                            = vec.Duration.ToString(CultureInfo.InvariantCulture);
                        XmlNode boneTransNode = _vecNode.AppendChild(doc.CreateElement("BoneTransforms"));
                        foreach (var btrans in vec.BoneTransforms)
                        {
                            XmlNode _newNode = boneTransNode.AppendChild(doc.CreateElement("Transform"));
                            _newNode.AppendChildIfNotNull(WriteProperty("SceneItem", btrans, doc));
                            _newNode.AppendChildIfNotNull(WriteProperty("SubItem", btrans, doc));
                            _newNode.AppendChildIfNotNull(WriteProperty("IsVisible", btrans, doc, true));
                            _newNode.AppendChildIfNotNull(WriteProperty("Position", btrans, doc));
                            _newNode.AppendChildIfNotNull(WriteProperty("Rotation", btrans, doc));
                            if (btrans.Opacity.HasValue == true)
                            {
                                _newNode.AppendChildIfNotNull(WriteProperty("Opacity", btrans, doc));
                            }
                            _newNode.AppendChildIfNotNull(WriteProperty("Scale", btrans, doc, Vector2.One));
                            _newNode.AppendChildIfNotNull(WriteProperty("BoneReference", btrans, doc));
                            _newNode.AppendChildIfNotNull(WriteProperty("InheritPosition", btrans, doc));
                            _newNode.AppendChildIfNotNull(WriteProperty("InheritRotation", btrans, doc));
                            _newNode.AppendChildIfNotNull(WriteProperty("InheritScale", btrans, doc));
                            _newNode.AppendChildIfNotNull(WriteProperty("InheritVisibility", btrans, doc));
                            _newNode.AppendChildIfNotNull(WriteProperty("FlipHorizontal", btrans, doc));
                            _newNode.AppendChildIfNotNull(WriteProperty("FlipVertical", btrans, doc));
                        }
                    }
                }
                else if (_prop.PropertyType == typeof(CompositeBone))
                {
                    CompositeBone bone = (CompositeBone)_prop.GetValue(item, null);
                    WriteCompositeBoneProperty(_node, bone, doc);
                }                
                else if (_prop.PropertyType == typeof(Point))
                {
                    Point _vector = (Point)_prop.GetValue(item, null);
                    if (defaultValue == null && _vector.X == 0 && _vector.Y == 0)
                    {
                        return null;
                    }
                    _node.AppendChild(doc.CreateElement("X")).InnerText = _vector.X.ToString(CultureInfo.InvariantCulture);
                    _node.AppendChild(doc.CreateElement("Y")).InnerText = _vector.Y.ToString(CultureInfo.InvariantCulture);

                }
                else if (_prop.PropertyType == typeof(Rectangle))
                {
                    Rectangle rect = (Rectangle)_prop.GetValue(item, null);
                    _node.Attributes.Append(doc.CreateAttribute("X")).InnerText = rect.X.ToString(CultureInfo.InvariantCulture);
                    _node.Attributes.Append(doc.CreateAttribute("Y")).InnerText = rect.Y.ToString(CultureInfo.InvariantCulture);
                    _node.Attributes.Append(doc.CreateAttribute("Width")).InnerText = rect.Width.ToString(CultureInfo.InvariantCulture);
                    _node.Attributes.Append(doc.CreateAttribute("Height")).InnerText = rect.Height.ToString(CultureInfo.InvariantCulture);

                }
                else if (_prop.PropertyType == typeof(Rectangle?))
                {
                    Rectangle? rect1 = (Rectangle?)_prop.GetValue(item, null);
                    _node.Attributes.Append(doc.CreateAttribute("X")).InnerText = rect1.Value.X.ToString(CultureInfo.InvariantCulture);
                    _node.Attributes.Append(doc.CreateAttribute("Y")).InnerText = rect1.Value.Y.ToString(CultureInfo.InvariantCulture);
                    _node.Attributes.Append(doc.CreateAttribute("Width")).InnerText = rect1.Value.Width.ToString(CultureInfo.InvariantCulture);
                    _node.Attributes.Append(doc.CreateAttribute("Height")).InnerText = rect1.Value.Height.ToString(CultureInfo.InvariantCulture);

                }
                else if (_prop.PropertyType == typeof(LinearProperty))
                {
                    LinearProperty lin = (LinearProperty)_prop.GetValue(item, null);
                    _node.AppendChild(doc.CreateElement("Description")).InnerText = lin.Description;
                    _node.AppendChild(doc.CreateElement("LowerBound")).InnerText = lin.LowerBound.ToString(CultureInfo.InvariantCulture);
                    _node.AppendChild(doc.CreateElement("UpperBound")).InnerText = lin.UpperBound.ToString(CultureInfo.InvariantCulture);
                    XmlNode _vals = _node.AppendChild(doc.CreateElement("Values"));
                    foreach (Vector2 item1 in lin.Values)
                    {
                        XmlNode _val = _vals.AppendChild(doc.CreateElement("Value"));
                        _val.AppendChild(doc.CreateElement("X")).InnerText = item1.X.ToString(CultureInfo.InvariantCulture);
                        _val.AppendChild(doc.CreateElement("Y")).InnerText = item1.Y.ToString(CultureInfo.InvariantCulture);
                    }
                }
                else if (_prop.PropertyType == typeof(float))
                {
                    float value = (float)_prop.GetValue(item, null);
                    if (defaultValue == null && value == 0f)
                    {
                        return null;
                    }
                    _node.InnerText = value.ToString(CultureInfo.InvariantCulture);
                }
                else if (_prop.PropertyType == typeof(Color))
                {                   
                    _node.InnerText = ParseColor((Color)_prop.GetValue(item, null));
                }
                else if (_prop.PropertyType == typeof(bool))
                {
                    bool value = (bool)_prop.GetValue(item, null);
                    if (defaultValue == null && value == false)
                    {
                        return null;
                    }
                    _node.InnerText = value.ToString();
                }
                else if (_prop.PropertyType == typeof(int))
                {
                    int value = (int)_prop.GetValue(item, null);
                    if (defaultValue == null && value == 0)
                    {
                        return null;
                    }
                    _node.InnerText = value.ToString();
                }
                else if (_prop.PropertyType == typeof(String))
                {
                    object value = _prop.GetValue(item, null);
                    String text = "";
                    if (defaultValue == null && value != null)
                    {
                        text = value.ToString();
                    }
                    if (String.IsNullOrEmpty(text))
                    {
                        return null;
                    }
                    _node.InnerText = text;
                }
                else if (_prop.PropertyType.BaseType == typeof(Enum))
                {
                    string str = _prop.GetValue(item, null).ToString();
                    _node.InnerText = str;
                }
                else if (_prop.PropertyType == typeof(Material))
                {
                    Material mat = (Material)_prop.GetValue(item, null);
                    if (mat != null)
                        _node.InnerText = mat.Name;
                }
                else if (_prop.PropertyType == typeof(bool?))
                {
                    bool? value = (bool?)_prop.GetValue(item, null);
                    if (value.HasValue)
                    {
                        _node.InnerText = value.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    _node.InnerText = _prop.GetValue(item, null).ToString();
                }
            }
            return _node;
        }

        private static void WriteCompositeBoneProperty(XmlNode _node, CompositeBone bone, XmlDocument doc)
        {
            _node.AppendChildIfNotNull(WriteProperty("Name", bone, doc));
            _node.AppendChildIfNotNull(WriteProperty("SceneItem", bone, doc));
            _node.AppendChildIfNotNull(WriteProperty("SubItem", bone, doc));
            _node.AppendChildIfNotNull(WriteProperty("MasterVisibility", bone, doc));
            _node.AppendChildIfNotNull(WriteProperty("InheritPosition", bone, doc));
            _node.AppendChildIfNotNull(WriteProperty("InheritScale", bone, doc));
            _node.AppendChildIfNotNull(WriteProperty("InheritRotation", bone, doc));
            _node.AppendChildIfNotNull(WriteProperty("InheritVisibility", bone, doc));
            _node.AppendChildIfNotNull(WriteProperty("Interpolate", bone, doc));
            XmlNode childBonesNode = _node.AppendChild(doc.CreateElement("ChildBones"));            
            foreach (var childBone in bone.ChildBones)
            {
                XmlNode childNode = childBonesNode.AppendChild(doc.CreateElement("Bone"));
                WriteCompositeBoneProperty(childNode, childBone, doc);
            }
        }

        #endregion

        #region DeSerialization

        public static void DeSerializeScene(String filename, SceneBase scene)
        {
            Console.WriteLine("**************************************************");
            Console.WriteLine("Deserializing scene");
            Console.WriteLine(filename);
            Console.WriteLine("**************************************************");

            _loadingScene = scene;
            scene.SceneComponents.Clear();
            scene.Fonts.Clear();
            scene.SceneItems.Clear();
            scene.Materials.Clear();
            scene.TemplateItems.Clear();

            string fileRoot = Path.GetDirectoryName(filename);

            FireStatusUpdate("Loading File", 50);
            StreamReader _sr = new StreamReader(filename);
            XmlDocument doc = new XmlDocument();
            doc.Load(_sr);

            if (scene is GlobalDataHolder)
            {
                GlobalDataHolder gdh = scene as GlobalDataHolder;
                XmlNode _resNode = doc.SelectSingleNode("IceScene/NativeResolution");
                if (_resNode != null)
                {
                    gdh.NativeResolution = new Point(
                        Int32.Parse(_resNode.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture),
                        Int32.Parse(_resNode.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture));
                }
            }
            TraceLogger.TraceInfo("LOADING ASSETS");
            FireStatusUpdate("Loading Assets", 100);
            LoadAssets(doc, scene);

            TraceLogger.TraceInfo("LOADING SCENE COMPONENTS");
            FireStatusUpdate("Loading Scene Components", 0);
            LoadSceneComponents(doc, scene);

            TraceLogger.TraceInfo("LOADING SCENE ITEMS");

            FireStatusUpdate("Loading Scene Items", 0);
            LoadSceneItems(doc, scene);

            TraceLogger.TraceInfo("LOADING SCENE TEMPLATE ITEMS");

            FireStatusUpdate("Loading Scene Templates", 0);
            LoadTemplateItems(doc, scene);

            FireStatusUpdate("Loading Scene Templates", 100);
            _sr.Close();
            _sr.Dispose();
            _sr = null;
            doc = null;

        }

        public static IceScene DeSerializeScene(String filename, String rootPath)
        {
            IceScene scene = new IceScene();
            FireStatusUpdate("Loading Scene", 5);
            DeSerializeScene(filename, scene);
            return scene;
        }

        private static void LoadSceneComponents(XmlDocument doc, SceneBase scene)
        {
            XmlNode _componentsNode = doc.SelectSingleNode("IceScene/Components");
            if (_componentsNode == null)
                return;

            List<IceSceneComponent> _components = new List<IceSceneComponent>();
            foreach (XmlNode _compNode in _componentsNode.ChildNodes)
            {
                TraceLogger.TraceInfo("Loading Scene Component Of Type" + _compNode.Name);

                IceSceneComponent _comp = CreateSceneComponentInstance(_compNode.Name);
                if (_comp != null)
                {
                    foreach (XmlNode nd1 in _compNode)
                    {
                        SetProperty(nd1.Name, _comp, _compNode);
                    }

                    _comp.SetOwner(scene as IceScene);
                    _components.Add(_comp);
                }
            }
            scene.SceneComponents = _components;
        }

        private static void LoadTemplateItems(XmlDocument doc, SceneBase scene)
        {
            try
            {
                XmlNode _el = doc.SelectSingleNode("IceScene/Templates");
                LoadSceneCollection(scene, _el, scene.TemplateItems);
            }
            catch (Exception err)
            {
                throw new ArgumentException("Error in load template items: " + err);
            }
        }

        private static void LoadSceneItems(XmlDocument doc, SceneBase scene)
        {
            try
            {
                XmlNode _el = doc.SelectSingleNode("IceScene/SceneItems");
                LoadSceneCollection(scene, _el, scene.SceneItems);

            }
            catch (Exception err)
            {
                throw new ArgumentException("Error in load scene items: " + err);
            }
        }

        private static void LoadSceneCollection(SceneBase scene, XmlNode _el, List<SceneItem> sceneitems)
        {
            try
            {
                int _count = _el.ChildNodes.Count;
                int _current = 0;

                foreach (XmlNode _node in _el.ChildNodes)
                {
                    _current++;
                    int perc = (int)((double)_current / (double)_count * 100);
                    FireStatusUpdate(string.Format("Loading Scene Item {0}/{1}", _current, _count), perc);

                    SceneItem _item = null;
                    //string _type = _node.Name;
                    _item = LoadSceneItem(_node, scene as IceScene);
                    sceneitems.Add(_item);
                }
            }
            catch (Exception e)
            {
                TraceLogger.TraceError("Error occurred in LoadSceneCollection" + Environment.NewLine + e.ToString());
                throw;
            }
        }
        private static SceneItem LoadSceneItem(XmlNode node, IceScene scene)
        {
            SceneItem _item = null;
            string _type = node.Name;
            switch (_type)
            {
                case NodeNames.SPRITE:
                    _item = LoadSprite(node, scene);
                    break;
                case NodeNames.ANIMATEDSPRITE:
                    _item = LoadAnimatedSprite(node, scene);
                    break;
                case NodeNames.TEXTITEM:
                    _item = LoadTextItem(node, scene);
                    break;
                case NodeNames.PARTICLEEFFECT:
                    _item = LoadParticleEffect(node, scene);
                    break;
                case NodeNames.TILEGRID:
                    _item = LoadTileGrid(node, scene);
                    break;                
                case NodeNames.COMPOSITEENTITY:
                    _item = LoadCompositeEntity(node, scene);
                    break;
                case NodeNames.POSTPROCESSANIMATION:
                    _item = LoadPostProcessAnimation(node, scene);
                    break;
                case NodeNames.SCENEITEM:
                    _item = new SceneItem();
                    LoadBaseSceneItem(node, _item);
                    break;
                default:
                    throw new Exception("Item type \"" + _type + "\" is not supported");
            }
            _item.Name = node.Attributes["name"].InnerText;
            newid++;
            _item.id = newid;
            return _item;
        }

        private static SceneItem LoadPostProcessAnimation(XmlNode _node, SceneBase scene)
        {
            PostProcessAnimation pp = null;
			#if !XNATOUCH
            string effectSource = _node.Attributes["effectSource"].Value;
            string effectRef = _node.Attributes["effectRef"].Value;
            if (effectSource.ToUpper() == "EMBEDDED")
            {
                foreach (var item in DrawingManager.EmbeddedIceEffects)
                {
                    if (item.Name == effectRef)
                    {
                        pp = new PostProcessAnimation(item);
                        break;
                    }
                }
            }
            if (effectSource.ToUpper() == "GLOBAL")
            {
                foreach (var item in SceneManager.GlobalDataHolder.Effects)
                {
                    if (item.Name == effectRef)
                    {
                        pp = new PostProcessAnimation(item);
                        break;
                    }
                }
            }
            if (effectSource.ToUpper() == "LOCAL")
            {
                foreach (var item in scene.Effects)
                {
                    if (item.Name == effectRef)
                    {
                        pp = new PostProcessAnimation(item);
                        break;
                    }
                }
            }


            LoadBaseSceneItem(_node, pp);
            SetIAnimationProperties(_node, pp);
            XmlNode gridsettings = _node.SelectSingleNode("Properties");
            int i = 0;
            foreach (XmlNode item in gridsettings.ChildNodes)
            {
                LinearProperty lin = new LinearProperty();
                XmlNode _linnode = item;
                lin.Description = _linnode.SelectSingleNode("Description").InnerText;
                lin.LowerBound = int.Parse(_linnode.SelectSingleNode("LowerBound").InnerText,
                    CultureInfo.InvariantCulture);
                lin.UpperBound = int.Parse(_linnode.SelectSingleNode("UpperBound").InnerText,
                    CultureInfo.InvariantCulture);

                foreach (XmlNode item1 in _linnode.SelectSingleNode("Values").ChildNodes)
                {
                    lin.Values.Add(new Vector2(
                        float.Parse(item1.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture),
                        float.Parse(item1.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture)));
                }
                pp.LinearProperties[i] = lin;
                i++;
            }
#endif
            return pp;
        }

        static int newid = 0;
              

        private static SceneItem LoadTileGrid(XmlNode _node, SceneBase scene)
        {
            TileGrid tilegrid = new TileGrid();
            LoadBaseSceneItem(_node, tilegrid);                        
            XmlNode gridsettings = _node.SelectSingleNode("TileGridSettings");            
            if (gridsettings != null)
            {
                LoadTileGridSettings(gridsettings, tilegrid);
            }
            tilegrid.TileSheet = LoadTileSheetFromNode(_node, scene, tilegrid);
            if (tilegrid.TileSheet == null)
            {
                tilegrid.Material = GetMaterialAssetFromNode(_node, scene);
            }
            return tilegrid;
        }

        private static Material GetMaterialAssetFromNode(XmlNode node, SceneBase scene)
        {
            string _materialSource = node.Attributes["materialSource"].InnerText;
            string _matRef = "";
            if (node.Attributes["materialRef"] != null)
                _matRef = node.Attributes["materialRef"].InnerText;
            else
                _matRef = node.InnerText;
            if (_materialSource.ToUpper() == "LOCAL")
                return scene.GetMaterial(_matRef);
            else if (_materialSource.ToUpper() == "GLOBAL")
                return SceneManager.GlobalDataHolder.GetMaterial(_matRef);
            else if (_materialSource.ToUpper() == "EMBEDDED")
                return SceneManager.GetEmbeddedMaterial(_matRef);
            else
                return null;
        }

        private static void LoadTileGridSettings(XmlNode gridsettings, TileGrid tilegrid)
        {
            tilegrid.UseTilingSafeBorders = bool.Parse(gridsettings.SelectSingleNode("UseTilingSafeBorders").InnerText);
            tilegrid.TileRows = int.Parse(gridsettings.SelectSingleNode("TileRows").InnerText, CultureInfo.InvariantCulture);
            tilegrid.TileCols = int.Parse(gridsettings.SelectSingleNode("TileCols").InnerText, CultureInfo.InvariantCulture);
            SetProperty("TileSize", tilegrid, gridsettings);
            XmlNode _layers = gridsettings.SelectSingleNode("TileLayers");
            if (_layers != null)
            {
                tilegrid.TileLayers = new List<TileLayer>();
                foreach (XmlNode _layerNode in _layers.ChildNodes)
                {
                    TileLayer layer = new TileLayer(tilegrid.TileCols, tilegrid.TileRows);
                    layer.Name = _layerNode.Attributes["Name"].InnerText;
                    layer.Visible = bool.Parse(_layerNode.Attributes["Visible"].InnerText);
                    layer.TileData = _layerNode.InnerText;
                    layer.Parent = tilegrid;
                    tilegrid.TileLayers.Add(layer);
                }
            }
        }
        //TODO: fix this horrible hack
        static IceScene _storedScene;
        private static SceneItem LoadCompositeEntity(XmlNode node, SceneBase scene)
        {
            TraceLogger.TraceInfo("Beginning LoadCompositeAnimation");
            _storedScene = scene as IceScene;
            CompositeEntity _compositeEntity = new CompositeEntity();
            LoadBaseSceneItem(node, _compositeEntity);            
            XmlNode _node = node.SelectSingleNode("CompositeEntityData");
            SetProperty("SceneItemBank", _compositeEntity, _node);
            SetProperty("RootBone", _compositeEntity, _node);
            SetProperty("Animations", _compositeEntity, _node);
            SetIAnimationDirectorProperties(_node, _compositeEntity as IAnimationDirector);
            TraceLogger.TraceInfo("Ending LoadCompositeEntity");
            return _compositeEntity;
        }

        private static SceneItem LoadParticleEffect(XmlNode node, SceneBase scene)
        {
            TraceLogger.TraceInfo("Beginning LoadParticleEffect");

            ParticleEffect _particle = new ParticleEffect();
            LoadBaseSceneItem(node, _particle);

            XmlNode emitterNode = node.SelectSingleNode("Emitter");
            if (emitterNode != null)
            {
                LoadEmitter(emitterNode, _particle, scene);
            }
            SetProperty("EditorBackgroundColor", _particle, node);
            SetIAnimationProperties(node, _particle);

            TraceLogger.TraceInfo("Ending LoadParticleEffect");
            return _particle;
        }

        private static void LoadEmitter(XmlNode emitterNode, ParticleEffect particle, SceneBase scene)
        {
            try
            {
                TraceLogger.TraceInfo("Beginning LoadEmitter");

                particle.Emitter = new IceCream.SceneItems.ParticlesClasses.Emitter();


                XmlDocument doc = new XmlDocument();
                doc.LoadXml(emitterNode.OuterXml);
                XmlReaderSettings sett = new XmlReaderSettings();
                StringReader stringReader = new StringReader(emitterNode.OuterXml);
                XmlTextReader xmlReader = new XmlTextReader(stringReader);

                XmlSerializer ser = new XmlSerializer(typeof(Emitter));
                particle.Emitter = (Emitter)ser.Deserialize(xmlReader);

                XmlNode _types = emitterNode.SelectSingleNode("ParticleTypes");
                if (_types != null)
                {
                    int i = 0;
                    foreach (XmlNode _type in _types.ChildNodes)
                    {
                        ParticleType ptype = particle.Emitter.ParticleTypes[i];
                        XmlNode _matNode = _type.SelectSingleNode("Material");
                        if (_matNode != null)
                        {
                            ptype.Material = GetMaterialAssetFromNode(_matNode, scene);
                            if (ptype.Material == null)
                                throw new Exception("Material Not Found For " + particle.Name);
                        }
                        else
                        {
                            ptype.Material = SceneManager.GetEmbeddedParticleMaterial();
                            TraceLogger.TraceWarning("Particle Effect Node Type Has No Material Set");
                        }
                        i++;
                    }
                }
                TraceLogger.TraceInfo("Ending LoadEmitter");
            }
            catch (Exception err)
            {
                TraceLogger.TraceError("Error Occurred In LoadEmitter" + Environment.NewLine + err.ToString());
                throw err;
            }
        }

        private static void SetIAnimationDirectorProperties(XmlNode _node, IAnimationDirector director)
        {
            try
            {
                SetProperty("AutoPlay", director, _node);
                SetProperty("DefaultAnimation", director, _node);
            }
            catch (Exception err)
            {
                string errMessage = "Error Occurred In SetIAnimationDirectorProperties";
                errMessage += Environment.NewLine + _node.InnerText;
                TraceLogger.TraceError(errMessage);
                throw new InvalidOperationException(errMessage, err);
            }
        }

        private static void SetIAnimationProperties(XmlNode _node, IAnimation animation)
        {
            try
            {
                SetProperty("LoopMax", animation, _node);
                SetProperty("Life", animation, _node);
                SetProperty("AutoPlay", animation, _node);
                SetProperty("HideWhenStopped", animation, _node);        
            }
            catch (Exception err)
            {
                string errMessage = "Error Occurred In SetIAnimationProperties";
                errMessage += Environment.NewLine + _node.InnerText;
                TraceLogger.TraceError(errMessage);
                throw new InvalidOperationException(errMessage, err);
            }
        }

        private static AnimatedSprite LoadAnimatedSprite(XmlNode _node, SceneBase scene)
        {
            try
            {
                TraceLogger.TraceInfo("Beginning Serialize AnimatedSprite");
                AnimatedSprite _sprite = new AnimatedSprite();
                LoadBaseSceneItem(_node, _sprite);
                SetIAnimationDirectorProperties(_node, _sprite);
                _sprite.Material = GetMaterialAssetFromNode(_node, scene);
                TraceLogger.TraceInfo("Loading AnimationInfo Data");
                XmlNode _anList = _node.SelectSingleNode("Animations");
                if (_anList != null)
                {
                    foreach (XmlNode item in _anList.ChildNodes)
                    {
                        _sprite.AddAnimation(GetAnimationInfoFromNode(item));
                    }
                }
                TraceLogger.TraceInfo("Ending Serialize AnimatedSprite");
                return _sprite;
            }
            catch (Exception err)
            {
                string errMessage = "Error Occurred In LoadAnimatedSprite";
                errMessage += Environment.NewLine + _node.InnerText;
                TraceLogger.TraceError(errMessage);
                throw new InvalidOperationException(errMessage, err);
            }
        }

        private static AnimationInfo GetAnimationInfoFromNode(XmlNode node)
        {
            AnimationInfo animInfo = new AnimationInfo(String.Empty);
            SetIAnimationProperties(node, animInfo as IAnimation);
            animInfo.Name = node.Attributes["Name"].Value;
            SetProperty("AnimationFrames", animInfo, node);
            return animInfo;
        }

        private static void SetProperty(string p, object targetObject, XmlNode node)
        {
            SetProperty(p, targetObject, node, null);
        }

        private static void SetProperty(string p, object targetObject, XmlNode node, object defaultValue)
        {
            if (targetObject is SceneItem)
            {
                TraceLogger.TraceVerbose("Setting Property " + p + " On SceneItem - " + ((SceneItem)targetObject).Name);
            }
            else
            {
                TraceLogger.TraceVerbose("Setting Property " + p + " On Object - " + targetObject.GetType().Name);
            }

            PropertyInfo _prop = targetObject.GetType().GetProperty(p);
            if (_prop == null)
            {
                return;
            }         
            XmlNode _newNode = node.SelectSingleNode(_prop.Name);
            if (_newNode == null)
            {
                if (defaultValue != null)
                {
                    _prop.SetValue(targetObject, defaultValue, null);
                    return;
                }
                if (_prop.PropertyType == typeof(bool))
                {
                    _prop.SetValue(targetObject, false, null);
                }
                else if (_prop.PropertyType == typeof(bool?))
                {
                    _prop.SetValue(targetObject, null, null);
                }
                else if (_prop.PropertyType == typeof(int))
                {
                    _prop.SetValue(targetObject, 0, null);
                }
                else if (_prop.PropertyType == typeof(String))
                {
                    _prop.SetValue(targetObject, String.Empty, null);
                }
                else if (_prop.PropertyType == typeof(Vector2))
                {
                    _prop.SetValue(targetObject, Vector2.Zero, null);
                }
                else if (_prop.PropertyType == typeof(Point))
                {
                    _prop.SetValue(targetObject, Point.Zero, null);
                }
                return;
            }
            TraceLogger.TraceVerbose("Property Type [" + _prop.PropertyType.Name + "]");

            if (_prop.PropertyType == typeof(Vector2))
            {
                _prop.SetValue(targetObject, ParseVector(_newNode), null);
            }
            else if (_prop.PropertyType == typeof(List<Vector2>))
            {
                
                List<Vector2> list = new List<Vector2>();
                foreach (XmlNode vecnode in _newNode.ChildNodes)
                {
                    list.Add(ParseVector(vecnode));
                }
                _prop.SetValue(targetObject, list, null);
            }
            /* FARSEER tile polygone serialization
            else if (_prop.PropertyType == typeof(List<Polygon>))
            {                
                List<Polygon> list = new List<Polygon>();
                foreach (XmlNode vecnode in _newNode.ChildNodes)
                {
                    list.Add(Polygon.FromString(vecnode.InnerText));
                }
                _prop.SetValue(targetObject, list, null);
            }*/
            else if (_prop.PropertyType == typeof(Rectangle?))
            {
                if (_newNode.Attributes.Count == 4)
                {
                    _prop.SetValue(targetObject, ParseRectangleNullable(_newNode), null);
                }
                else
                {
                    _prop.SetValue(targetObject, null, null);
                }
                
            }
            else if (_prop.PropertyType == typeof(Rectangle))
            {
                if (_newNode.Attributes.Count == 4)
                    {
                        _prop.SetValue(targetObject, ParseRectangle(_newNode), null);
                    }                
            }
            else if (_prop.PropertyType == typeof(Point))
            {
                Point _point = new Point(
                    int.Parse(node.SelectSingleNode(_prop.Name + "/X").InnerText, CultureInfo.InvariantCulture),
                    int.Parse(node.SelectSingleNode(_prop.Name + "/Y").InnerText, CultureInfo.InvariantCulture));
                _prop.SetValue(targetObject, _point, null);
            }
            else if (_prop.PropertyType == typeof(float))
            {
                _prop.SetValue(targetObject, Single.Parse(_newNode.InnerText,
                    CultureInfo.InvariantCulture), null);
            }
            else if (_prop.PropertyType == typeof(Color))
            {
                Color newColor = (Color)ParseToColor(_newNode.InnerText);
                    _prop.SetValue(targetObject, newColor, null);
               
            }
            else if (_prop.PropertyType == typeof(bool))
            { 
                _prop.SetValue(targetObject, bool.Parse(_newNode.InnerText), null);
            }
            else if (_prop.PropertyType == typeof(bool?))
            {
                if (String.IsNullOrEmpty(_newNode.InnerText) == false)
                {
                    _prop.SetValue(targetObject, bool.Parse(_newNode.InnerText), null);
                }
                else
                {
                    _prop.SetValue(targetObject, null, null);
                }
            }
            else if (_prop.PropertyType == typeof(byte))
            {
#if(WINDOWS)
                _prop.SetValue(targetObject, byte.Parse(_newNode.InnerText,
                    CultureInfo.InvariantCulture), null);
#else
                byte b = (byte)int.Parse(_newNode.InnerText);
                _prop.SetValue(targetObject, b, null);
#endif
            }
            else if (_prop.PropertyType == typeof(byte?))
            {
                if (String.IsNullOrEmpty(_newNode.InnerText) == false)
                {
#if(WINDOWS)
                    _prop.SetValue(targetObject, byte.Parse(_newNode.InnerText, CultureInfo.InvariantCulture), null);
#else
                    byte b = (byte)int.Parse(_newNode.InnerText);
                    _prop.SetValue(targetObject, b, null);
#endif
                }
            }
            else if (_prop.PropertyType == typeof(int))
            {
                _prop.SetValue(targetObject, int.Parse(_newNode.InnerText, CultureInfo.InvariantCulture), null);
            }
            else if (_prop.PropertyType == typeof(string))
            {
                _prop.SetValue(targetObject, _newNode.InnerText, null);
            }
            else if (_prop.PropertyType.BaseType == typeof(Enum))
            {
#if(WINDOWS)
                Enum val = (Enum)Enum.Parse(_prop.PropertyType, _newNode.InnerText);
                _prop.SetValue(targetObject, Convert.ChangeType(val, _prop.PropertyType), null);
#else

                Enum val = (Enum)Enum.Parse(_prop.PropertyType, _newNode.InnerText, true);
                _prop.SetValue(targetObject, Convert.ChangeType(val, _prop.PropertyType, null), null);
#endif
            }
            else if (_prop.PropertyType == typeof(CompositeBone))
            {
                CompositeBone bone = GetCompositeBone(_newNode);
                _prop.SetValue(targetObject, bone, null);
            }
            else if (_prop.PropertyType == typeof(List<CompositeBone>))
            {
                CompositeBone parent = targetObject as CompositeBone;
                foreach (XmlNode vecnode in _newNode.ChildNodes)
                {
                    CompositeBone anim = GetCompositeBone(vecnode);
                    anim.ParentBone = parent;
                    parent.ChildBones.Add(anim);
                }
            }            
            else if (_prop.PropertyType == typeof(Dictionary<string, SceneItem>))
            {

                Dictionary<string, SceneItem> dict = new Dictionary<string, SceneItem>();
                foreach (XmlNode vecnode in _newNode.ChildNodes)
                {
                    string type = vecnode.Attributes[0].InnerText;
                    XmlNode _itemNode = vecnode.ChildNodes[0];
                    SceneItem item = LoadSceneItem(_itemNode, _storedScene);
                    dict.Add(type, item);
                }
                _prop.SetValue(targetObject, dict, null);
            }
            else if (_prop.PropertyType == typeof(List<CompositeAnimation>))
            {
                List<CompositeAnimation> list = new List<CompositeAnimation>();
                foreach (XmlNode vecnode in _newNode.ChildNodes)
                {
                    CompositeAnimation anim = new CompositeAnimation();
                    anim.Parent = targetObject as CompositeEntity;
                    anim.Name = vecnode.Attributes["Name"].Value;
                    SetProperty("LerpLastFrameWithFirst", anim, vecnode);
                    SetProperty("Speed", anim, vecnode);
                    SetProperty("KeyFrames", anim, vecnode);
                    SetIAnimationProperties(vecnode, anim as IAnimation);
                    list.Add(anim);
                }

                _prop.SetValue(targetObject, list, null);
            }
            else if (_prop.PropertyType == typeof(List<CompositeKeyFrame>))
            {

                List<CompositeKeyFrame> list = new List<CompositeKeyFrame>();
                foreach (XmlNode vecnode in _newNode.ChildNodes)
                {
                    CompositeKeyFrame animKeyFrame = new CompositeKeyFrame();
                    animKeyFrame.Parent = targetObject as CompositeAnimation;
                    animKeyFrame.Name = vecnode.Attributes["Name"].Value;
                    animKeyFrame.Duration = int.Parse(vecnode.Attributes["Duration"].Value, CultureInfo.InvariantCulture);
                    //Bone transforms
                    XmlNode _boneTransNode = vecnode.SelectSingleNode("BoneTransforms");
                    foreach (XmlNode transNode in _boneTransNode.ChildNodes)
                    {
                        CompositeBoneTransform boneTransform = new CompositeBoneTransform();
                        boneTransform.Parent = animKeyFrame;
                        SetProperty("BoneReference", boneTransform, transNode);
                        SetProperty("SceneItem", boneTransform, transNode);
                        SetProperty("SubItem", boneTransform, transNode);
                        SetProperty("IsVisible", boneTransform, transNode, true);
                        SetProperty("Position", boneTransform, transNode, Vector2.Zero);
                        SetProperty("Rotation", boneTransform, transNode, 0);
                        SetProperty("Opacity", boneTransform, transNode);
                        SetProperty("Scale", boneTransform, transNode, Vector2.One);
                        SetProperty("InheritPosition", boneTransform, transNode);
                        SetProperty("InheritRotation", boneTransform, transNode);
                        SetProperty("InheritScale", boneTransform, transNode);
                        SetProperty("InheritVisibility", boneTransform, transNode);
                        SetProperty("FlipHorizontal", boneTransform, transNode);
                        SetProperty("FlipVertical", boneTransform, transNode);
                        animKeyFrame.AddCompositeBoneTransform(boneTransform);
                    }
                    list.Add(animKeyFrame);
                }

                _prop.SetValue(targetObject, list, null);
            }
            else if (_prop.PropertyType == typeof(List<AnimationFrame>))
            {

                List<AnimationFrame> listFrames = new List<AnimationFrame>();
                foreach (XmlNode vecnode in _newNode.ChildNodes)
                {
                    AnimationFrame frame = new AnimationFrame();
                    frame.Area = vecnode.Attributes["Area"].Value;
                    frame.Duration = int.Parse(vecnode.Attributes["Duration"].Value,
                        CultureInfo.InvariantCulture);
                    listFrames.Add(frame);
                }
                _prop.SetValue(targetObject, listFrames, null);
            }
            else if (_prop.PropertyType == typeof(LinearProperty))
            {
                LinearProperty lin = (LinearProperty)_prop.GetValue(targetObject, null);
                lin.Description = _newNode.SelectSingleNode("Description").InnerText;
                lin.LowerBound = int.Parse(_newNode.SelectSingleNode("LowerBound").InnerText, CultureInfo.InvariantCulture);
                lin.UpperBound = int.Parse(_newNode.SelectSingleNode("UpperBound").InnerText, CultureInfo.InvariantCulture);

                foreach (XmlNode item in _newNode.SelectSingleNode("Values").ChildNodes)
                {
                    lin.Values.Add(new Vector2(
                        float.Parse(item.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture),
                        float.Parse(item.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture)));
                }
            }
            else
            {
                TraceLogger.TraceWarning("Not Set By SetProperty - " + p);
            }
        }

        private static object ParseRectangle(XmlNode rectNode)
        {
            return new Rectangle(
                            int.Parse(rectNode.Attributes["X"].InnerText, CultureInfo.InvariantCulture),
                            int.Parse(rectNode.Attributes["Y"].InnerText, CultureInfo.InvariantCulture),
                            int.Parse(rectNode.Attributes["Width"].InnerText, CultureInfo.InvariantCulture),
                            int.Parse(rectNode.Attributes["Height"].InnerText, CultureInfo.InvariantCulture));
        }

        private static object ParseRectangleNullable(XmlNode rectNode)
        {
            return new Rectangle(
                            int.Parse(rectNode.Attributes["X"].InnerText, CultureInfo.InvariantCulture),
                            int.Parse(rectNode.Attributes["Y"].InnerText, CultureInfo.InvariantCulture),
                            int.Parse(rectNode.Attributes["Width"].InnerText, CultureInfo.InvariantCulture),
                            int.Parse(rectNode.Attributes["Height"].InnerText, CultureInfo.InvariantCulture));
        }

        private static Vector2 ParseVector(XmlNode xmlNode)
        {
            return new Vector2(
                float.Parse(GetTextFromChildNode(xmlNode, "X"), CultureInfo.InvariantCulture),
                float.Parse(GetTextFromChildNode(xmlNode, "Y"), CultureInfo.InvariantCulture));
        }

        private static void AutoSetPropertiesOnObject(object obj, XmlNode xmlNode)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            foreach (var item in properties)
            {
                item.SetValue(obj, GetTextFromChildNode(xmlNode, item.Name), null);
            }
        }

        private static string GetTextFromChildNode(XmlNode node, string childNode)
        {
            return node.SelectSingleNode(childNode).InnerText;
        }

        private static CompositeBone GetCompositeBone(XmlNode node)
        {
            CompositeBone newbone = new CompositeBone();
            SetProperty("Name", newbone, node);
            SetProperty("ChildBones", newbone, node);
            SetProperty("SceneItem", newbone, node);
            SetProperty("SubItem", newbone, node);
            SetProperty("MasterVisibility", newbone, node);
            SetProperty("InheritPosition", newbone, node);
            SetProperty("InheritScale", newbone, node);
            SetProperty("InheritRotation", newbone, node);
            SetProperty("InheritVisibility", newbone, node);
            SetProperty("Interpolate", newbone, node);
            return newbone;
        }

        private static Sprite LoadSprite(XmlNode _node, SceneBase scene)
        {
            TraceLogger.TraceInfo("Loading Sprite");
            Sprite _sprite = new Sprite();
            LoadBaseSceneItem(_node, _sprite);
            _sprite.Material = LoadMaterialFromNode(_node, scene, _sprite);
            if (_node.Attributes.GetNamedItem("materialArea") != null)
            {
                _sprite.MaterialArea = _node.Attributes["materialArea"].InnerText;
            }
            else
            {
                SetProperty("SourceRectangle", _sprite, _node);
            }
            return _sprite;
        }

        private static Material LoadMaterialFromNode(XmlNode _node, SceneBase scene, SceneItem item)
        {
            String _materialSource = _node.Attributes["materialSource"].InnerText;
            String _materialRef = _node.Attributes["materialRef"].InnerText;
            Material _returnMaterial = null;
            if (_materialSource.ToUpper() == "LOCAL")
            {
                _returnMaterial = scene.GetMaterial(_materialRef);
            }
            else if (_materialSource.ToUpper() == "GLOBAL")
            {
                _returnMaterial = SceneManager.GlobalDataHolder.GetMaterial(_materialRef);
            }
            else if (_materialSource.ToUpper() == "EMBEDDED")
            {
                _returnMaterial = SceneManager.GetEmbeddedMaterial(_materialRef);
            }
            if (_returnMaterial != null)
            {
                return _returnMaterial;
            }
            else
            {
                throw new Exception("The SceneItem \"" + item.Name + "\" is trying to use an invalid material: ["
                    + _materialSource.ToUpper() + "] \"" + _materialRef + "\"");
            }
        }

        private static TileSheet LoadTileSheetFromNode(XmlNode _node, SceneBase scene, SceneItem item)
        {
            if (_node.Attributes.GetNamedItem("tileSheetSource") != null
                && _node.Attributes.GetNamedItem("tileSheetRef") != null)
            {
                String tileScope = _node.Attributes["tileSheetSource"].InnerText;
                String tileRef = _node.Attributes["tileSheetRef"].InnerText;
                TileSheet returnTilesheet = null;
                if (tileScope.ToUpper() == "LOCAL")
                {
                    returnTilesheet = scene.GetTileSheet(tileRef);
                }
                else if (tileScope.ToUpper() == "GLOBAL")
                {
                    returnTilesheet = SceneManager.GlobalDataHolder.GetTileSheet(tileRef);
                }
                if (returnTilesheet != null)
                {
                    return returnTilesheet;
                }
                else
                {
                    throw new Exception("The SceneItem \"" + item.Name + "\" is trying to use an invalid TileSheet: ["
                        + tileScope.ToUpper() + "] \"" + tileRef + "\"");
                }
            }
            else
            {
                return null;
            }
        }

        private static TextItem LoadTextItem(XmlNode node, SceneBase scene)
        {
            TraceLogger.TraceInfo("Loading TextItem");
            TextItem _textItem = new TextItem();
            LoadBaseSceneItem(node, _textItem);

            _textItem.Font = LoadFontFromNode(node, scene);

            SetProperty("Text", _textItem, node);
            SetProperty("AutoCenterPivot", _textItem, node);
            SetProperty("Shadow", _textItem, node);
            SetProperty("Tint", _textItem, node);
            return _textItem;
        }

        private static IceFont LoadFontFromNode(XmlNode _node, SceneBase scene)
        {
            string _materialSource = _node.Attributes["fontSource"].InnerText;
            if (_materialSource.ToUpper() == "LOCAL")
                return scene.GetFont(_node.Attributes["fontRef"].InnerText);
            else if (_materialSource.ToUpper() == "GLOBAL")
                return SceneManager.GlobalDataHolder.GetFont(_node.Attributes["fontRef"].InnerText);
            else if (_materialSource.ToUpper() == "EMBEDDED")
                return SceneManager.GetEmbeddedFont(_node.Attributes["fontRef"].InnerText);
            else
                return null;

        }

        private static Camera LoadCamera(XmlNode _node)
        {
            TraceLogger.TraceInfo("Loading Camera");
            Camera _camera = new Camera();
            LoadBaseSceneItem(_node, _camera);
            SetProperty("ViewPortSize", _camera, _node);
            return _camera;
        }

        private static void LoadBaseSceneItem(XmlNode node, object item)
        {
            TraceLogger.TraceInfo("Beginning Serialize Base Scene Item");
            XmlNode nd = node.Attributes.GetNamedItem("name");
            ((SceneItem)item).Name = node.Attributes["name"].InnerText;
            foreach (XmlNode _siNode in node.ChildNodes)
            {
                PropertyInfo _prop = item.GetType().GetProperty(_siNode.Name);
                if (_prop != null)
                {
                    string t = _prop.PropertyType.Name;
                    if (_prop.PropertyType.IsGenericType)
                    {
                        if (_prop.Name == "SourceRectangle")
                        {
                            SetProperty(_prop.Name, item, node);
                        }
                        if (_prop.Name == "LinkPoints")
                        {
                            #region LinkPoints

                            XmlNode _node = node.SelectSingleNode("LinkPoints");
                            List<LinkPoint> _linkpnts = new List<LinkPoint>();
                            foreach (XmlNode _compNode in _node.ChildNodes)
                            {
                                LinkPoint linkPoint = new LinkPoint();
                                foreach (XmlNode nd1 in _compNode)
                                {
                                    SetProperty(nd1.Name, linkPoint, _compNode);
                                }
                                XmlNode _mounts = _compNode.SelectSingleNode("Mounts");
                                if (_mounts != null)
                                {
                                    foreach (XmlNode mount in _mounts)
                                    {
                                        linkPoint.Mounts.Add(
                                            mount.SelectSingleNode("ChildSceneItem").InnerText,
                                            mount.SelectSingleNode("ChildLinkPoint").InnerText);
                                    }
                                }
                                linkPoint.Owner = item as SceneItem;
                                _linkpnts.Add(linkPoint);
                            }
                            if (_linkpnts.Count > 0)
                            {
                                _prop.SetValue(item, _linkpnts, null);
                            }

                            #endregion
                        }
                        if (_prop.Name == "Components")
                        {
                            #region Components

                            XmlNode _node = node.SelectSingleNode("Components");
                            List<IceComponent> _components = new List<IceComponent>();
                            foreach (XmlNode _compNode in _node.ChildNodes)
                            {
                                TraceLogger.TraceInfo("Load Component");
                                TraceLogger.TraceInfo(_compNode.Name);
                                Components.IceComponent _comp = CreateComponentInstance(_compNode.Name);
                                if (_comp == null)
                                    TraceLogger.TraceWarning("Couldnt Find Component (" + _compNode.Name + ")");
                                else
                                {
                                    foreach (XmlNode nd1 in _compNode)
                                    {
                                        SetProperty(nd1.Name, _comp, _compNode);
                                    }
                                    if (_comp != null)
                                    {
                                        _comp.SetOwner((SceneItem)item);
                                        _components.Add(_comp);
                                    }
                                }
                            }
                            _prop.SetValue(item, _components, null);

                            #endregion
                        }
                    }
                    else
                    {
                        SetProperty(_prop.Name, item, node);
                    }
                }
                else if (_siNode.Name.ToUpper() == "MOUNTS")
                {
                    #region Mounts
                    XmlNode _node = node.SelectSingleNode("Mounts");
                    //List<LinkPoint> _linkpnts = new List<LinkPoint>();
                    foreach (XmlNode _compNode in _node.ChildNodes)
                    {
                        SceneItem it = (SceneItem)item;
                        string itemName = "";
                        bool isTemplate = false;

                        XmlAttribute b = _compNode.Attributes["sceneItemRef"];
                        if (b == null)
                        {
                            isTemplate = true;
                            itemName = _compNode.Attributes["templateItemRef"].InnerText;
                        }
                        else
                            itemName = b.InnerText;


                        it.Mount(itemName,
                        _compNode.Attributes["targetLink"].InnerText,
                        _compNode.Attributes["sourceLink"].InnerText,
                        isTemplate);

                    }
                    #endregion
                }

            }
            TraceLogger.TraceInfo("Ending Serialize Base Scene Item");
        }

        private static IceCream.Components.IceComponent CreateComponentInstance(string p)
        {
            return (IceComponent)ComponentTypeContainer.CreateNewInstance(p);
        }
		#if !XNATOUCH
        private static IceEffect CreateEffectInstance(string p)
        {
            return (IceEffect)ComponentTypeContainer.CreateNewInstance(p);
        }
#endif

        private static IceSceneComponent CreateSceneComponentInstance(string p)
        {
            IceSceneComponent comp1 = (IceSceneComponent)ComponentTypeContainer.CreateNewInstance(p);
            return comp1;
        }

        private static object ParseToColor(string color)
        {
            string[] vals = color.Split(',');
#if(WINDOWS)
            Color col = new Color(
                byte.Parse(vals[0], CultureInfo.InvariantCulture),
                byte.Parse(vals[1], CultureInfo.InvariantCulture),
                byte.Parse(vals[2], CultureInfo.InvariantCulture),
                byte.Parse(vals[3], CultureInfo.InvariantCulture));
#else
            Color col = new Color(
                byte.Parse(vals[0]),
                byte.Parse(vals[1]),
                byte.Parse(vals[2]),
                byte.Parse(vals[3]));
#endif
            return col;
        }

        private static string ParseColor(Color color)
        {
            string s = string.Format("{0},{1},{2},{3}", color.R, color.G, color.B, color.A);
            return s;
        }

        private static void LoadAssets(XmlDocument doc, SceneBase scene)
        {
            string _lastValidName = "";
            try
            {
                XmlNode _el = doc.SelectSingleNode("IceScene/Assets");
                int _count = _el.ChildNodes.Count;
                int _current = 0;
                foreach (XmlNode _node in _el.ChildNodes)
                {
                    _current++;
                    int perc = (int)((double)_current / (double)_count * 100); ;
                    FireStatusUpdate(string.Format("Loading Asset {0}/{1}", _current, _count), perc);

                    IceAsset asset = null;
                    if (_node.Name.ToUpper() == "MATERIAL")
                    {
                        Material newMat = new Material();
                        asset = newMat;
                        scene.Materials.Add(newMat);
                        TraceLogger.TraceInfo("Loading Material");
                        if (_node.Attributes.GetNamedItem("areas_definition") != null)
                        {
                            newMat.AreasDefinitionFilename
                                = _node.Attributes["areas_definition"].InnerText;
                            String definitionPath = Path.Combine(SceneSerializer._rootPath, newMat.AreasDefinitionFilename);
                            TraceLogger.TraceVerbose("Loading AreasDefinitionFilename: " + newMat.AreasDefinitionFilename);
                            newMat.LoadAreasDefinition(definitionPath);
                        }
                    }
                    else if (_node.Name.ToUpper() == "FONT")
                    {
                        asset = new IceFont();
                        scene.Fonts.Add(asset as IceFont);
                        TraceLogger.TraceInfo("Loading Font");
                    }
                    else if (_node.Name.ToUpper() == "EFFECT")
                    {
						#if !XNATOUCH
                        asset = CreateEffectInstance(_node.Attributes["type"].InnerText);
                        scene.Effects.Add(asset as IceEffect);
                        TraceLogger.TraceInfo("Loading Effect");
#endif
                    }
                    else if (_node.Name.ToUpper() == "TILESHEET")
                    {
                        TileSheet newTileSheet = LoadTileSheet(_node, scene);
                        asset = newTileSheet;
                        scene.TileSheets.Add(newTileSheet);                       
                        TraceLogger.TraceInfo("Loading TileSheet");
                    }
                    asset.Scope = AssetScope.Local;
                    if (scene == SceneManager.GlobalDataHolder)
                    {
                        asset.Scope = AssetScope.Global;
                    }
                    asset.Parent = scene;
                    asset.Name = _node.Attributes["name"].InnerText;
                    _lastValidName = asset.Name;
                    if (_node.Attributes.GetNamedItem("location") != null)
                    {
                        asset.Filename = _node.Attributes["location"].InnerText;
                    }
                    else
                    {
                        asset.Filename = "";
                    }
                    TraceLogger.TraceInfo("Name : " + asset.Name);
                    TraceLogger.TraceVerbose("FileName : " + asset.Filename);
                    TraceLogger.TraceVerbose("Scope : " + asset.Scope.ToString());
                }
            }
            catch (Exception err)
            {
                if (err != null)
                {
                    #if(WINDOWS)
                    Trace.TraceError("ERROR In Load Assets - " + _lastValidName);
                    #endif
                    throw new ArgumentException("Error in load materials: " + err.Message);
                }
            }
        }

        private static TileSheet LoadTileSheet(XmlNode _node, SceneBase scene)
        {
            TileSheet tileSheet = new TileSheet();            
            tileSheet.Material = GetMaterialAssetFromNode(_node, scene);
            SetProperty("EnableCollisionByDefault", tileSheet, _node);
            SetProperty("TileSize", tileSheet, _node);
            SetProperty("Polygons", tileSheet, _node);
            tileSheet.CreateFlippedPolygons();
            return tileSheet;
        }

        #endregion
    }

    public static class TraceLogger
    {
#if(WINDOWS)
        public static TraceSwitch SerializationSwitch = null;
#endif
        static TraceLogger()
        {

#if(WINDOWS)
            SerializationSwitch = new TraceSwitch("SerializationSwitch", "Serialization Switch");
#endif
        }
        public static void TraceVerbose(string message)
        {

#if(WINDOWS)
            if (SerializationSwitch.TraceVerbose)
                Trace.WriteLine(message);
#endif
        }
        public static void TraceInfo(string message)
        {

#if(WINDOWS)
            if (SerializationSwitch.TraceInfo)
                Trace.WriteLine(message);
#endif
        }
        public static void TraceWarning(string message)
        {

#if(WINDOWS)
            ConsoleColor col = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (SerializationSwitch.TraceWarning)
            {
                Trace.WriteLine("******************************");
                Trace.WriteLine("           WARNING");
                Trace.WriteLine("******************************");
                Trace.WriteLine(message);
                Trace.WriteLine("******************************");
            }
            Console.ForegroundColor = col;
#endif
        }
        public static void TraceError(string message)
        {

#if(WINDOWS)

            ConsoleColor col = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Trace.WriteLine("******************************");
            Trace.WriteLine("       EXCEPTION ERROR        ");
            Trace.WriteLine("******************************");
            if (SerializationSwitch.TraceError)
                Trace.WriteLine(message);
            Trace.WriteLine("******************************");

            Console.ForegroundColor = col;
#endif
        }
    }

}
