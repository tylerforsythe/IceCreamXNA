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

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.Globalization;
using System.IO;

namespace IceCream
{
    public class Material : IceAsset
    {
        protected Texture2D _texture;
        protected Dictionary<String, Rectangle> _areas;

        [XmlIgnore]
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        [XmlIgnore]
        public String AreasDefinitionFilename
        {
            get;
            set;
        }

        [XmlIgnore]
        public Dictionary<String, Rectangle> Areas
        {
            get { return _areas; }
            set { _areas = value; }
        }

        public Material()
            : this("", null, AssetScope.Local)
        {

        }

        public Material(String name, Texture2D texture, AssetScope scope)
        {
            this.Name = name;
            this._texture = texture;
            this.Filename = "";
            this.AreasDefinitionFilename = "";
            this.Scope = scope;
            this.Areas = new Dictionary<String, Rectangle>();
        }

        public override string ToString()
        {
            return "[" + Scope + "] " + this.Name;
        }

        public void LoadAreasDefinition(String filename)                   
        {            
            StreamReader stream = new StreamReader(filename);
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            XmlNode rootNode = doc.SelectSingleNode("Areas");
            this.Areas.Clear();
            foreach (XmlNode rectNode in rootNode.ChildNodes)
            {
                String key = rectNode.Attributes["Key"].InnerText;
                Rectangle newSourceRect = new Rectangle(
                            int.Parse(rectNode.Attributes["X"].InnerText, CultureInfo.InvariantCulture),
                            int.Parse(rectNode.Attributes["Y"].InnerText, CultureInfo.InvariantCulture),
                            int.Parse(rectNode.Attributes["W"].InnerText, CultureInfo.InvariantCulture),
                            int.Parse(rectNode.Attributes["H"].InnerText, CultureInfo.InvariantCulture));
                this.Areas.Add(key, newSourceRect);
            }
            stream.Close();
            stream.Dispose();
            stream = null;
            doc = null;
        }

    }
}
