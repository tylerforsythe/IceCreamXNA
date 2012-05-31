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
using System.IO;
using System.Reflection;
using System.Xml;
using System.Globalization;
using IceCream.Attributes;
using IceCream.Components;
using IceCream.Drawing;
using IceCream.Serialization;
using IceCream.SceneItems;

namespace IceCream
{
    public class ComponentTypeContainer
    {
#if WINDOWS
        static bool _appDomainSet;
#endif
        static Dictionary<int, Assembly> _assemblies;
        static List<string> _fileNames;
        public static Dictionary<Type, int> _types;
        static AppDomain _gameAppDomain;
        public static void SetAppDomain(AppDomain domain)
        {
            _gameAppDomain = domain;
#if WINDOWS
            _appDomainSet = true;
#endif
        }
        static ComponentTypeContainer()
        {
            _assemblies = new Dictionary<int, Assembly>();
            _types = new Dictionary<Type, int>();
            _fileNames = new List<string>();

        }
        public static void LoadAssemblyInformation(string root)
        {
#if WINDOWS
            //            if(_appDomainSet)
            //              _gameAppDomain.AssemblyResolve += new ResolveEventHandler(_gameAppDomain_AssemblyResolve);
#endif
            int _id = 0;
            string[] files = Directory.GetFiles(root);
            foreach (var item in files)
            {
                if (Path.GetExtension(item) == ".exe" ||
                    Path.GetExtension(item) == ".dll")
                {                    
                    if (!_fileNames.Contains(Path.GetFileName(item)))
                    {
                        Assembly asm = null;
                        asm = LoadAsm(item);
                        if (asm != null)
                        {
                            _fileNames.Add(Path.GetFileName(item));
                            _assemblies.Add(_id, asm);
                            LoadComponents(_id, asm);
                            _id++;
                        }
                    }
                }
            }
#if WINDOWS
            //if (_appDomainSet)
            //  _gameAppDomain.AssemblyResolve -= new ResolveEventHandler(_gameAppDomain_AssemblyResolve);
#endif
        }

        private static Assembly LoadAsm(string item)
        {

            Assembly asm = null;
#if !WINDOWS
            asm = Assembly.LoadFrom(item); 
#else
            if (_appDomainSet)
            {
                //StreamReader reader = new StreamReader(item, System.Text.Encoding.GetEncoding(1252), false);
                //byte[] _bytes = new byte[reader.BaseStream.Length];
                //reader.BaseStream.Read(_bytes, 0, System.Convert.ToInt32(reader.BaseStream.Length));
                //reader.Close();
                //reader.Dispose();
                ////byte[] assemblyBuffer = File.ReadAllBytes(item);
                //asm = _gameAppDomain.Load(_bytes); //_gameAppDomain.Load(AssemblyName.GetAssemblyName(item));
                //_bytes= null;
                throw new Exception("App Domain Features Are Currently Not Working, Do Not Set The AppDomain");
            }
            else
                asm = Assembly.LoadFile(item);
#endif
            return asm;
        }
#if WINDOWS
        static Assembly _gameAppDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return null;
            //foreach (var item in Directory.GetFiles(_tempRoot))
            //{
            //    if (item.EndsWith(".exe") || item.EndsWith(".dll"))
            //    {
            //        Assembly tmp = Assembly.LoadFile(item);
            //        if (tmp.FullName == args.Name)
            //            return tmp;
            //    }
            //}
            //return null;
        }
#endif

        private static void LoadComponents(int _id, Assembly asm)
        {           
            Type[] types = asm.GetTypes();
            foreach (Type t in types)
            {
                var _attribs = t.GetCustomAttributes(true);
                foreach (var item in _attribs)
                {
                    Attribute b = (Attribute)item;
                    if (b.GetType().Equals(typeof(IceComponentAttribute)) 
                        || b.GetType().Equals(typeof(IceEffectAttribute)))
                    {
                        _types.Add(t, _id);
                    }
                }
            }
        }

        internal static object CreateNewInstance(string p)
        {
            foreach (var item1 in _types)
            {
                Type t = (Type)item1.Key;
                if (t.FullName == p)
                {
                    try
                    {
                        Assembly asm = _assemblies[item1.Value];
#if !WINDOWS
                        return asm.CreateInstance(t.FullName);
#else
                        if (_appDomainSet)
                        {
                            return Activator.CreateInstance(t);
                        }
                        else
                        {
                            return Activator.CreateInstance(t);
                        }
#endif
                    }
                    catch (Exception err)
                    {
                        TraceLogger.TraceError(err.ToString());
                    }
                }
            }
            foreach (var item in _assemblies)
            {
                object oo = item.Value.CreateInstance(p);
                if (oo != null)
                    return oo;
            }
            return null;

        }

        public static void UnloadAppDomain()
        {
            if (_gameAppDomain != null)
                AppDomain.Unload(_gameAppDomain);
            _assemblies.Clear();
            _fileNames.Clear();
            _types.Clear();
            _gameAppDomain = null;
        }

        internal static object DeepCopyIceComponent(Type type, IceComponent cloneFrom)
        {
            //Create a new instance of the component
            IceComponent comp = (IceComponent)type.Assembly.CreateInstance(type.FullName);
            //CopyPropertiesTo(comp, cloneFrom);
            cloneFrom.CopyValuesTo(comp);
            return comp;
        }

        internal static object DeepCopyIceSceneComponent(Type type, IceSceneComponent cloneFrom)
        {
            //Create a new instance of the component
            IceSceneComponent comp = (IceSceneComponent)type.Assembly.CreateInstance(type.FullName);
            //CopyPropertiesTo(comp, cloneFrom);
            cloneFrom.CopyValuesTo(comp);
            return comp;
        }

        private static void CopyPropertiesTo(object to, object from)
        {
            //Just clone the public properties
            foreach (var item in to.GetType().GetProperties())
            {
                try
                {
                    if (item.CanWrite && item.CanRead)
                        item.SetValue(to, item.GetValue(from, null), null);
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while trying to copy this item" + Environment.NewLine + "Error was with property " + item.Name);
                }
            }

        }
    }

}
