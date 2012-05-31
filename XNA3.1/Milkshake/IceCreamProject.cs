using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using IceCream.Components;
using System.Reflection;
using Microsoft.Build.BuildEngine;

namespace Milkshake
{
    public class IceCreamProject
    {
        #region Static Methods
        public static IceCreamProject Instance;
        public static IceCreamProject OpenProjectFile(string filename)
        {
            XmlSerializer _ser = new XmlSerializer(typeof(IceCreamProject));
            Stream _stream = File.Open(filename, FileMode.Open);
            IceCreamProject _proj = (IceCreamProject)_ser.Deserialize(_stream);
            //_proj.LoadAssemblies(__proj.RootDIR + @"bin\x86\Debug\");
            Instance = _proj;
            _stream.Close();
            return _proj;
        }

        #endregion

        #region Fields

        private String _filename;
        private String _path;
        private String _name;
        private String _description;
        private String _visualStudioProjectPath;
        private String _contentFolderRelativePath;
        private String _binaryFolderRelativePath;
        private String _lastOpenedScene;
        private String _currentSceneFile;

        #endregion

        #region Properties
        [XmlIgnore]
        public Project VSProj { get; set; }
        [XmlIgnore]
        public string GameExe { get; set; }

        [XmlIgnore]
        public String Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        [XmlIgnore]
        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }
        [XmlIgnore]
        public Dictionary<Type,Assembly> IceSceneComponents { get; set; }
        [XmlIgnore]
        public Dictionary<Type, Assembly> IceSceneItemComponents { get; set; }

        public String Name
        {
            get { return _name;  }
            set { _name = value; }
        }

        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public String VisualStudioProjectPath
        {
            get { return _visualStudioProjectPath; }
            set { _visualStudioProjectPath = value; }
        }

        public String ContentFolderRelativePath
        {
            get { return _contentFolderRelativePath; }
            set { _contentFolderRelativePath = value; }
        }

        public String BinaryFolderRelativePath
        {
            get { return _binaryFolderRelativePath; }
            set { _binaryFolderRelativePath = value; }
        }

        public String LastOpenedScene
        {
            get { return _lastOpenedScene; }
            set { _lastOpenedScene = value; }
        }

        [XmlIgnore]
        public String CurrentSceneFile
        {
            get { return _currentSceneFile; }
            set { _currentSceneFile = value; }
        }

        #endregion

        #region Constructor

        public IceCreamProject()
        {
            _filename = "";
            _path = "";
            _name = "New Project";
            _description = "This is a new project";
            _visualStudioProjectPath = "";
            _contentFolderRelativePath = "";
            _binaryFolderRelativePath = "";
            _lastOpenedScene = "";
            _currentSceneFile = "";
            IceSceneComponents = new Dictionary<Type, Assembly>();
            IceSceneItemComponents= new Dictionary<Type, Assembly>();
            Instance = this;
        }

        #endregion

        #region Methods

        public void Save()
        {
            SaveToFile(_path + "\\" + _filename);
        }

        private void SaveToFile(string filename)
        {
            Stream _stream = File.Open(filename, FileMode.Create, FileAccess.Write);
            XmlSerializer _serializer = new XmlSerializer(typeof(IceCreamProject));
            _serializer.Serialize(_stream, this);
            _stream.Close();
        }

        #endregion
    }

   
}
