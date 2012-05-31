using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Content;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using IceCream;
using GdiColor = System.Drawing.Color;

namespace Milkshake
{
    public enum MilkshakePreferencesEditorStartupAction
    {
        DoNothing,
        OpenLastEditedProject,
        OpenStartupWizard,
    }

    public class MilkshakePreferences
    {
        #region Static Methods

        public static string GetPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+ "\\IceCream\\settings.xml";            
        }

        internal static MilkshakePreferences Load()
        {
            try
            {
                XmlSerializer _ser = new XmlSerializer(typeof(MilkshakePreferences));
                if (!File.Exists(GetPath()))
                    return new MilkshakePreferences();
                Stream _stream = File.Open(GetPath(), FileMode.Open);
                MilkshakePreferences pref = (MilkshakePreferences)_ser.Deserialize(_stream);
                _stream.Close();
                return pref;
            }
            catch (Exception err)
            {
                Console.WriteLine("Error while trying to load the settings file: " + err);
                return new MilkshakePreferences();
            }
        }

        #endregion

        #region Fields

        private int _selectedGrid;

        #endregion

        #region Properties

        public MilkshakePreferencesEditorStartupAction EditorStartupAction
        {
            get;
            set;
        }

        public String LastOpenedProject
        {
            get;
            set;
        }

        public bool IgnoreModificationWarning
        {
            get;
            set;
        }

        public bool AutoLoadLastOpenedScene
        {
            get;
            set;
        }

        public bool ConfirmBeforeObjectDelete
        {
            get;
            set;
        }

        public bool ForceWindowParametersAtStartup
        {
            get;
            set;
        }

        public Point LastEditorPosition
        {
            get;
            set;
        }

        public Point LastEditorSize
        {
            get;
            set;
        }

        public bool LastEditorMaximizedState
        {
            get;
            set;
        }

        public bool ShowGrid
        {
            get;
            set;
        }

        public int SelectedGrid
        {
            get { return _selectedGrid; }
            set
            {
                _selectedGrid = IceMath.Clamp(value, 1, 3);
            }
        }

        public bool SnapToGrid
        {
            get;
            set;
        }

        public Point[] GridSizes
        {
            get;
            set;
        }

        public bool ShowCameraBounds
        {
            get;
            set;
        }

        public int[] GridAttractionZones
        {
            get;
            set;
        }

        public Color GridColor
        {
            get;
            set;
        }

        public String ToolSpritesheetLastInputFolder { get; set; }
        public String ToolSpritesheetLastOutputFolder { get; set; }
        public String ToolSpritesheetLastOutputName { get; set; }
        public int ToolSpritesheetLastSafeBorderSize { get; set; }
        public bool ToolSpritesheetLastReplaceBaseColor { get; set; }
        public bool ToolSpritesheetLastCorrectTransparencyColor { get; set; }
        public bool ToolSpritesheetLastPadTexturePowerOfTwo { get; set; }
        public Color ToolSpritesheetLastBaseColor { get; set; }

        #endregion

        #region Constructor

        public MilkshakePreferences()
        {
            this.EditorStartupAction = MilkshakePreferencesEditorStartupAction.OpenLastEditedProject;
            this.IgnoreModificationWarning = false;
            this.AutoLoadLastOpenedScene = true;
            this.ConfirmBeforeObjectDelete = true;
            this.ForceWindowParametersAtStartup = true;
            this.LastEditorPosition = new Point(100, 100);
            this.LastEditorSize = new Point(800, 600);
            this.LastEditorMaximizedState = false;
            this.ShowCameraBounds = true;
            this.GridSizes = new Point[3];
            this.GridAttractionZones = new int[3];
            this.GridColor = new Color(0, 0, 0, 128);
            this.ShowGrid = false;
            this.SelectedGrid = 1;            
            this.SnapToGrid = true;
            this.GridSizes[0] = new Point(64, 64);
            this.GridAttractionZones[0] = 16;
            this.GridSizes[1] = new Point(32, 32);
            this.GridAttractionZones[1] = 8;
            this.GridSizes[2] = new Point(16, 16);
            this.GridAttractionZones[2] = 4;
            
            this.ToolSpritesheetLastSafeBorderSize = 1;
            this.ToolSpritesheetLastPadTexturePowerOfTwo = false;
            this.ToolSpritesheetLastReplaceBaseColor = true;
            this.ToolSpritesheetLastCorrectTransparencyColor = true;
        }

        #endregion

        #region Methods

        public void Save()
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(GetPath())))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(GetPath()));
                }
                Stream _stream = File.Open(GetPath(), FileMode.Create, FileAccess.Write);
                XmlSerializer _serializer = new XmlSerializer(this.GetType());
                _serializer.Serialize(_stream, this);
                _stream.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine("Error while trying to save the settings file: " + err);             
            }
        }

        #endregion
    }
}
