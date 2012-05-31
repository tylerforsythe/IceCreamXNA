using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Milkshake.GraphicsDeviceControls;
using MilkshakeLibrary;
using Milkshake.Tools;
using Milkshake.Editors;
using Milkshake.Editors.Components;
using Milkshake.Editors.TileSheetEditor;
using Milkshake.Editors.TileGrids;
using Milkshake.Editors.Sprites;
using Milkshake.Editors.Particles;
using Milkshake.Editors.PostProcessAnimations;
using Milkshake.Editors.CompositeEntities;
using Milkshake.Editors.AnimatedSprites;
using Milkshake.Wizards;
using Milkshake.SelectorDialogs;
using Milkshake.Selectors;
using IceCream;
using IceCream.Drawing;
using IceCream.SceneItems;
using IceCream.SceneItems.AnimationClasses;
using Microsoft.Xna.Framework;
using GdiColor = System.Drawing.Color;
using XnaColor = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using IceCream.Components;
using IceCream.Attributes;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using IceCream.SceneItems.TileGridClasses;
using Microsoft.Build.Construction;
using Microsoft.Build.Execution;
using Microsoft.Build.Evaluation;
//using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Xml;

namespace Milkshake
{
    public partial class MilkshakeForm : Form
    {
        #region Static fields

        public static MilkshakeForm Instance;

        #endregion

        #region Static Methods
        delegate void errorMsgDelegate(Exception err);
        public static void ShowErrorMessage(Exception err)
        {
            if (Instance.InvokeRequired)
            {
                Instance.Invoke(new errorMsgDelegate(ShowErrorMessage),err);
            }
            else
            {
                ErrorForm frm = new ErrorForm(err);
                frm.Show();
            }
        }

        public static void ShowErrorMessage(String errorMessage)
        {
            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowInfoMessage(String infoMessage)
        {
            MessageBox.Show(infoMessage, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool ShowWarningQuestion(String errorMessage)
        {
            return (MessageBox.Show(errorMessage, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                == DialogResult.Yes);
        }

        public static GdiColor GetGDIColor(XnaColor color)
        {
            return System.Drawing.Color.FromArgb(255,
                (int)color.R, (int)color.G, (int)color.B);
        }

        public static void SwapCameraAndRenderScene(Camera camera, bool ignoreClearBeforeDrawing)
        {
            DrawingManager.IgnoreClearBeforeRendering = ignoreClearBeforeDrawing;
            Camera oldCamera = SceneManager.ActiveScene.ActiveCameras[0];
            SceneManager.ActiveScene.ActiveCameras[0] = camera;
            DrawingManager.RenderScene();
            SceneManager.ActiveScene.ActiveCameras[0] = oldCamera;
            DrawingManager.IgnoreClearBeforeRendering = false;
        }

        public static void SwapCameraAndRenderScene(Camera camera)
        {
            SwapCameraAndRenderScene(camera, true);
        }

        #endregion

        #region Fields

        delegate void voidDelegate();
        delegate void LoadSceneDelegate(string filename);
        string _projToLoad = string.Empty;
        private FileSystemWatcher _fileWatcher;
        private IceCreamProject _currentProject;
        private ContentBuilder _contentBuilder;
        private MilkshakePreferences _preferences;
        public MilkshakePreferences Preferences
        {
            get { return _preferences; }
            set { _preferences = value; }
        }
        private bool _projectWasModified = false;
        public bool ProjectWasModified
        {
            get { return _projectWasModified; }
            set
            {
                _projectWasModified = value;
                RefreshMenuStripAndToolStripStatus();
            }
        }

        private bool _sceneWasModified = false;
        public bool SceneWasModified
        {
            get { return _sceneWasModified; }
            set
            {
                _sceneWasModified = value;
                RefreshMenuStripAndToolStripStatus();
            }
        }
        private float _zoomFactor = 1f;
        public float ZoomFactor
        {
            get { return _zoomFactor; }
            set
            {
                Console.WriteLine("ZoomFactor: " + value);
                if (value < _zoomDelta)
                {
                    _zoomFactor = _zoomDelta;
                    toolStripButtonZoomOut.Enabled = false;
                }
                else
                {
                    _zoomFactor = value;
                    toolStripButtonZoomOut.Enabled = true;
                    if (value > 0.9998f && value < 1.0001f)
                    {
                        toolStripButtonNoZoom.Enabled = false;
                    }
                    else
                    {
                        toolStripButtonNoZoom.Enabled = true;
                    }
                }
                if (SceneManager.ActiveScene != null)
                {
                    SceneManager.ActiveScene.ActiveCameras[0].Zoom = new Vector2(_zoomFactor);
                }
            }
        }
        private SceneItemTypeStruct[] _sceneItemsTypeTable;
        internal SceneItemTypeStruct[] SceneItemsTypeTable
        {
            get { return _sceneItemsTypeTable; }
        }
        private float _zoomDelta = 0.06f;
        private Camera _sceneCamera;
        private Dictionary<SceneItem, TreeNode> _sceneItemNodes = new Dictionary<SceneItem, TreeNode>();
        public Dictionary<SceneItem, TreeNode> SceneItemNodes
        {
            get { return _sceneItemNodes; }
            set { _sceneItemNodes = value; }
        }
        internal List<SceneItem> clipBoardSceneItems = new List<SceneItem>();
        private object _objecToSelect = null;
        private MilkshakeSceneEditorTool _sceneEditorTool;
        public MilkshakeSceneEditorTool SceneEditorTool
        {
            get
            {
                return _sceneEditorTool;
            }
            set
            {
                _sceneEditorTool = value;
                toolStripButtonToolCamera.Checked = (value == MilkshakeSceneEditorTool.Camera);
                toolStripButtonToolSelect.Checked = (value == MilkshakeSceneEditorTool.Select);
                toolStripButtonToolTemplateBrush.Checked = (value == MilkshakeSceneEditorTool.TemplateBrush);
                UnselectItems();
            }
        }

        #endregion

        #region Constructor

        public MilkshakeForm(string projToLoad)
        {
            //Sprite testPlanet = new Sprite();
            //testPlanet.AddComponent(new FarseerBodyCircle());
            //XmlWriterSettings xmlSettings = new XmlWriterSettings();
            //xmlSettings.Indent = true;

            //using (XmlWriter xmlWriter = XmlWriter.Create("test.xml", xmlSettings))
            //{
            //    IntermediateSerializer.Serialize(xmlWriter, testPlanet, null);
            //}

            InitializeComponent();

            Instance = this;
            _currentProject = null;
            _contentBuilder = new ContentBuilder();
            _sceneItemsTypeTable = GetSceneItemTypesTable();
            Trace.WriteLine("Startup", "MilkShake Main");
            //Trace.Listeners.Add(new DebugTraceListener());            
            _fileWatcher = new FileSystemWatcher();
            _fileWatcher.Changed += new FileSystemEventHandler(_fileWatcher_Changed);
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileWatcher.Filter = "*.exe";
            _projToLoad = projToLoad;
        }

        void _fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (_currentProject != null)
            {
                //DialogResult _res = MessageBox.Show("New binaries have been detected, would you like to reload the project?", 
                //    "New Binaries Detected",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

                //if(_res==DialogResult.Yes)
                //    LoadProject(_currentProject.Filename);
            }

        }

        private void MilkshakeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !ConfirmMilkshakeExit();

        }

        private void MilkshakeForm_Load(object sender, EventArgs e)
        {
            InitializeNodes();            
            _preferences = MilkshakePreferences.Load();
            sceneEditorControl.Preferences = _preferences;
            this.SceneEditorTool = MilkshakeSceneEditorTool.Select;
            if (_preferences.ForceWindowParametersAtStartup == true)
            {
                this.Left = _preferences.LastEditorPosition.X;
                this.Top = _preferences.LastEditorPosition.Y;
                this.Size = new Size(_preferences.LastEditorSize.X, _preferences.LastEditorSize.Y);
                this.WindowState = _preferences.LastEditorMaximizedState ? FormWindowState.Maximized : FormWindowState.Normal;
            }
            if (_projToLoad != string.Empty)
            {
                LoadProject(_projToLoad);
            }
            else
            {
                if (_preferences.EditorStartupAction == MilkshakePreferencesEditorStartupAction.OpenStartupWizard)
                {
                    _currentProject = new IceCreamProject();
                    _projectWasModified = true;
                }
                else if (_preferences.EditorStartupAction == MilkshakePreferencesEditorStartupAction.OpenLastEditedProject
                    && String.IsNullOrEmpty(_preferences.LastOpenedProject) == false)
                {
                    try
                    {
                        LoadProject(_preferences.LastOpenedProject);
                    }
                    catch
                    {

                    }
                }
            }
            RefreshEditorStatus();
            // focus by default on the scene
            sceneEditorControl.Focus();
        }

        #endregion

        #region Form events

        private void MilkshakeForm_LocationChanged(object sender, EventArgs e)
        {
            if (_preferences != null)
            {
                _preferences.LastEditorPosition = new Microsoft.Xna.Framework.Point(this.Left, this.Top);
            }
        }

        private void MilkshakeForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && _preferences != null)
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    _preferences.LastEditorSize = new Microsoft.Xna.Framework.Point(this.Size.Width, this.Size.Height);
                }
                _preferences.LastEditorMaximizedState = (this.WindowState == FormWindowState.Maximized);
            }
        }

        #endregion

        #region Menu Strip events

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitMilkshake();
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not yet available, " + Environment.NewLine +
                "Create a new project from within Visual Studio and open the Game.icproj file");
            return;
            //OpenNewProjectWizardAndProcessResult();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProjectSelectionDialogAndProcessResult();
        }

        private void openSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSceneSelectionDialogAndProcessResult();
        }

        private void newSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenNewSceneWizardAndProcessResult();
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentProject();
        }

        private void saveSceneAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSceneSaveAsDialogAndProcessResult();
        }

        private void saveSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentScene();
        }

        private void projectSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProjectSettings();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMilkshakeSettings();
        }

        #endregion

        #region Tool Strip events

        private void toolStripButtonOpenProject_Click(object sender, EventArgs e)
        {
            OpenProjectSelectionDialogAndProcessResult();
        }

        private void toolStripButtonOpenScene_Click(object sender, EventArgs e)
        {
            OpenSceneSelectionDialogAndProcessResult();
        }

        private void toolStripButtonNewScene_Click(object sender, EventArgs e)
        {
            OpenNewSceneWizardAndProcessResult();
        }

        private void toolStripButtonSaveScene_Click(object sender, EventArgs e)
        {
            SaveCurrentScene();
        }

        private void toolStripButtonSaveProject_Click(object sender, EventArgs e)
        {
            SaveCurrentProject();
        }

        private void toolStripButtonCut_Click(object sender, EventArgs e)
        {
            CutSelectedItems();
        }

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            CopySelectedItems();
        }

        private void toolStripButtonPaste_Click(object sender, EventArgs e)
        {
            PasteSelectedItems();
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedItems();
        }

        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void toolStripButtonZoomOut_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void toolStripButtonNoZoom_Click(object sender, EventArgs e)
        {
            ZoomNormal();
        }

        private void toolStripButtonProjectSettings_Click(object sender, EventArgs e)
        {
            OpenProjectSettings();
        }

        private void toolStripSplitButtonGrid_ButtonClick(object sender, EventArgs e)
        {
            _preferences.ShowGrid = !_preferences.ShowGrid;
        }

        #endregion

        #region Tool and Menu Strip methods

        private void EditSceneItemComponent(IceComponent component)
        {

        }

        private void OpenMilkshakeSettings()
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.Preferences = _preferences;
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                
            }
        }

        private void OpenProjectSettings()
        {
            ProjectSettingsForm settingsForm = new ProjectSettingsForm();
            settingsForm.NativeResolution = SceneManager.GlobalDataHolder.NativeResolution;
            settingsForm.AutoSignIntoLive = SceneManager.GlobalDataHolder.AutoSignIntoLive;
            settingsForm.ContentFolderPath = SceneManager.GlobalDataHolder.ContentFolderPath;
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                SceneManager.GlobalDataHolder.NativeResolution = settingsForm.NativeResolution;
                SceneManager.GlobalDataHolder.AutoSignIntoLive = settingsForm.AutoSignIntoLive;
                this.ProjectWasModified = true;
                RefreshEditorStatus();
            }
        }

        private void SaveCurrentScene()
        {
            if (_currentProject.CurrentSceneFile != String.Empty)
            {
                try
                {
                    IceCream.Serialization.SceneSerializer.SerializeScene(SceneManager.GlobalDataHolder, 
                        Path.Combine(Path.Combine(_currentProject.Path, _currentProject.ContentFolderPath), "global.ice"));
                    IceCream.Serialization.SceneSerializer.SerializeScene(SceneManager.ActiveScene, _currentProject.CurrentSceneFile);
                    SceneWasModified = false;
                }
                catch (Exception err)
                {
                    ShowErrorMessage("Unable to save scene: " + err.Message);
                }
            }
            else
            {
                // if the new scene wasn't saved yet, use the Save As dialog instead
                OpenSceneSaveAsDialogAndProcessResult();
            }
        }

        private void RefreshEditorStatus()
        {
            RefreshMenuStripAndToolStripStatus();
            RefreshStatusStrip();
        }

        private void RefreshMenuStripAndToolStripStatus()
        {
            bool aProjectIsLoaded = (_currentProject != null);
            bool aSceneIsLoaded = (SceneManager.ActiveScene != null);

            scenesToolStripMenuItem.Enabled = aProjectIsLoaded;
            saveSceneAsToolStripMenuItem.Enabled = aSceneIsLoaded;
            splitContainerMain.Visible = aSceneIsLoaded;
            //scenePropertiesToolStripMenuItem.Enabled = aSceneIsLoaded;
            newSceneToolStripMenuItem.Enabled = toolStripButtonNewScene.Enabled = aProjectIsLoaded;
            openSceneToolStripMenuItem.Enabled = toolStripButtonOpenScene.Enabled = aProjectIsLoaded;
            toolStripButtonProjectSettings.Enabled = aProjectIsLoaded;

            toolStripButtonZoomIn.Enabled = toolStripButtonZoomOut.Enabled
                = toolStripButtonNoZoom.Enabled = aSceneIsLoaded;

            if (aSceneIsLoaded) Console.WriteLine(SceneManager.ActiveScene.ActiveCameras[0].ConvertToScreenPos(Vector2.Zero));

            saveProjectToolStripMenuItem.Enabled = toolStripButtonSaveProject.Enabled = _projectWasModified;
            saveSceneToolStripMenuItem.Enabled = toolStripButtonSaveScene.Enabled = _sceneWasModified;
            saveAllToolStripMenuItem.Enabled = toolStripButtonSaveAll.Enabled = (_sceneWasModified || _projectWasModified);

            bool enableCutCopyDelete = (sceneEditorControl.SelectedItems.Count > 0);
            
            toolStripButtonCut.Enabled = cutToolStripMenuItem.Enabled = enableCutCopyDelete;
            toolStripButtonCopy.Enabled = copyToolStripMenuItem.Enabled = enableCutCopyDelete;
            toolStripButtonDelete.Enabled = deleteToolStripMenuItem.Enabled = enableCutCopyDelete;
            toolStripButtonPaste.Enabled = pasteToolStripMenuItem.Enabled = (clipBoardSceneItems.Count > 0);

            projectToolStripMenuItem.Visible = aProjectIsLoaded;
            scenesToolStripMenuItem.Visible = aProjectIsLoaded;

            //enables the delete button if no sceneitem was selected but the selected node is a template
            if (!enableCutCopyDelete && treeViewResources.SelectedNode!=null)
                toolStripButtonDelete.Enabled = deleteToolStripMenuItem.Enabled = 
                    treeViewResources.SelectedNode.Tag is SceneItem ||
                   treeViewResources.SelectedNode.Tag is IceComponent || treeViewResources.SelectedNode.Tag is IceSceneComponent;
        }

        internal void RefreshStatusStrip()
        {
            toolStripStatusLabelSceneMousePos.Text = "";
            toolStripStatusLabelScene.Text = "";
            if (_currentProject != null)
            {
                this.Text = _currentProject.Name + " - Milkshake";
                toolStripStatusLabelProjectName.Text = "Project loaded: \"" + _currentProject.Name + "\"";
                if (SceneManager.ActiveScene != null)
                {
                    toolStripStatusLabelSceneMousePos.Text = sceneEditorControl.SceneMousePos.ToString() + " " + sceneEditorControl.RealMousePos.ToString();
                    if (_currentProject.CurrentSceneFile != String.Empty)
                    {
                        toolStripStatusLabelScene.Text = "Current Scene: \"" + Path.GetFileNameWithoutExtension(_currentProject.CurrentSceneFile) + "\"";
                    }
                    else
                    {
                        toolStripStatusLabelScene.Text = "Current Scene: New Scene (not saved)";
                    }
                }
            }
            else
            {
                this.Text = "Milkshake";
                toolStripStatusLabelProjectName.Text = "No project loaded";
            }
        }

        private void LoadProjectData(string filename)
        {
            try
            {
                LoadingForm.ChangeStatus("Loading Project", 0);
                _currentProject = IceCreamProject.OpenProjectFile(filename);

                _preferences.LastOpenedProject = filename;
                _currentProject.Path = Path.GetDirectoryName(filename);
                _currentProject.Filename = Path.GetFileName(filename);
                string binPath = Path.Combine(_currentProject.Path, _currentProject.BinaryFolderRelativePath);
                String projectPath = Path.Combine(_currentProject.Path, _currentProject.VisualStudioProjectPath);
                Microsoft.Build.Evaluation.Project _p = new Microsoft.Build.Evaluation.Project(projectPath);
                _p.Build();
                string str = _p.GetPropertyValue("AssemblyName");
                _currentProject.GameExe = str + ".exe";
                _currentProject.VSProj = _p;

                // finding Content Project
                                
                foreach (var item in _p.AllEvaluatedItems)
                {
                    if (item.ItemType == "ProjectReference")
                    {
                        ProjectMetadata previousMetadata = null;
                        foreach (var metadata in item.Metadata)
                        {
                            if (metadata.Name == "XnaReferenceType" && metadata.EvaluatedValue == "Content")
                            {
                                if (previousMetadata == null)
                                {
                                    throw new Exception("Found XnaContent Project reference but Metadata order is incorrect");
                                }
                                _currentProject.ContentFolderPath = Path.Combine(_currentProject.Path, Path.GetDirectoryName(item.UnevaluatedInclude));

                            }
                            previousMetadata = metadata;
                        }
                    }
                }

                LoadingForm.ChangeStatus("Loading Assemblies", 0);

                CopyAssembliesFromProject();

                ComponentTypeContainer.LoadAssemblyInformation(Path.GetDirectoryName(Application.ExecutablePath));
                ComponentTypeContainer.LoadAssemblyInformation(binPath);

                LoadingForm.ChangeStatus("Reading Assembly Info", 0);
                LoadAssemblyInformation();

                LoadingForm.ChangeStatus("Loading Global Project Data", 0);
                SceneManager.InitializeEmbedded(this.sceneEditorControl.Services, true);
                SceneManager.LoadGlobalData(_currentProject.ContentFolderPath);

                LoadingForm.ChangeStatus("Initializing Global Data", 0);
                SceneManager.InitializeGlobal(this.sceneEditorControl.Services, false);

                LoadingForm.ChangeStatus("Loading Global Textures", 0);
                LoadTextures(SceneManager.GlobalDataHolder);
                LoadingForm.ChangeStatus("Finished Loading Project Data", 100);
                _fileWatcher.Path = binPath;
                _fileWatcher.EnableRaisingEvents = true;
            }
            catch (Exception e)
            {
                MilkshakeForm.ShowErrorMessage(e);
                LoadingForm.HideForm();
                _preferences.LastOpenedProject = "";
                _preferences.Save();
            }
        }

        private void CopyAssembliesFromProject()
        {
            try
            {
                foreach (var file in Directory.GetFiles(Path.GetDirectoryName(Application.ExecutablePath)))
                {
                    string filename = Path.GetFileName(file);
                    if (filename.ToUpper().StartsWith("ICECREAM.") ||
                       filename.ToUpper().StartsWith("MILKSHAKE") ||                       
                        filename.ToUpper().StartsWith("XPTABLE.") ||
                        filename.ToUpper().StartsWith("TEXTWRITER"))
                        continue;

                    File.Delete(file);
                }

                string binPath = Path.Combine(_currentProject.Path, _currentProject.BinaryFolderRelativePath);
                string milkShakeBin = Path.GetDirectoryName(Application.ExecutablePath);
                foreach (var item in Directory.GetFiles(binPath))
                {
                    if (Path.GetFileNameWithoutExtension(item).ToUpper() == "ICECREAM.DLL")
                    {
                        FileInfo _fin = new FileInfo(item);
                        FileVersionInfo fin = FileVersionInfo.GetVersionInfo(item);
                        FileVersionInfo fin1 = FileVersionInfo.GetVersionInfo("IceCream.dll");

                        if (fin.FileVersion != fin1.FileVersion)
                            MessageBox.Show("Your IceCream Version is out of date. Please rebuild your project using Visual Studio");
                    }
                    if ((Path.GetExtension(item) == ".exe" ||
                        Path.GetExtension(item) == ".dll") &&
                        Path.GetFileName(item) != "IceCream.dll" &&                        
                        Path.GetFileName(item) != "MilkShake.exe")
                    {
                        File.Copy(item, Path.Combine(milkShakeBin, Path.GetFileName(item)), true);
                    }
                }
            }
            catch (Exception err)
            {
                ShowErrorMessage(err);
            }
        }

        private void LoadProjectCallBack(IAsyncResult res)
        {

            Invoke(new voidDelegate(RefreshEditorStatus));
            try
            {
                if (_preferences.AutoLoadLastOpenedScene == true
                    && File.Exists(_currentProject.LastOpenedScene))
                {
                    LoadScene(_currentProject.LastOpenedScene);
                }
                else
                    LoadingForm.HideForm();
            }
            catch (Exception e)
            {
                ShowErrorMessage(e);
            }

        }
        private void LoadProject(String filename)
        {

            //If there is a currently loaded project and scene, unload the scene and clear up first.
            if (_currentProject != null && SceneManager.ActiveScene != null)
            {
                SceneManager.UnLoadScene(SceneManager.ActiveScene);
                SceneManager.ActiveScene = null;
                SceneManager.GlobalDataHolder.ContentManager.Unload();
                SceneManager.GlobalDataHolder = new GlobalDataHolder();
                SceneManager.Scenes.Clear();
                _contentBuilder.Clear();
                toolStripSplitButtonAddSceneComponent.DropDownItems.Clear();
                menuAddSceneItemComponent.DropDownItems.Clear();                
            }

            LoadingForm.ShowForm();
            LoadingForm.ChangeHeader("Loading Project Data");
            Application.DoEvents();
            IceCream.Serialization.SceneSerializer.StatusUpdate += new IceCream.Serialization.StatusUpdateHandler(SceneSerializer_StatusUpdate);
            LoadSceneDelegate del = new LoadSceneDelegate(LoadProjectData);
            del.BeginInvoke(filename, new AsyncCallback(LoadProjectCallBack), null);

        }

        private void LoadAssemblyInformation()
        {
            List<ToolStripDropDownItem> list = new List<ToolStripDropDownItem>();

            foreach (var item in ComponentTypeContainer._types)
            {
                if (item.Key == typeof(IceComponent) || item.Key.IsSubclassOf(typeof(IceComponent)))
                {
                    if (!_currentProject.IceSceneItemComponents.ContainsKey(item.Key))
                    {
                        list.Add(AddSceneItenComponentToolStrip(item.Key));

                    }
                }
                if (item.Key == typeof(IceSceneComponent) || item.Key.IsSubclassOf(typeof(IceSceneComponent)))
                {
                    if (!_currentProject.IceSceneComponents.ContainsKey(item.Key))
                    {
                        AddSceneComponentToolStrip(item.Key);
                    }
                }

            }

            list.Sort(delegate(ToolStripDropDownItem item1,
                                ToolStripDropDownItem item2)
            {
                return Comparer<string>.Default.Compare
                    (item1.Text, item2.Text);
            });

            menuAddSceneItemComponent.DropDownItems.AddRange(list.ToArray());
            //string _rootDir = Path.Combine(_currentProject.Path,_currentProject.BinaryFolderRelativePath);
            //CheckForSceneComponents(typeof(IceSceneComponent).Assembly);
            //foreach (string fname in Directory.GetFiles(_rootDir))
            //{
            //    try
            //    {
            //        if ((fname.EndsWith(".dll") || fname.EndsWith(".exe") )&& !fname.ToUpper().EndsWith("ICECREAM.DLL"))
            //        {
            //            Assembly _assembly =Assembly.LoadFile(fname);

            //            CheckForSceneComponents(_assembly);

            //        }
            //    }
            //    catch (Exception)
            //    {
            //    }
            //}
        }
        delegate ToolStripMenuItem AddTypeToToolStrip(Type type);

        private ToolStripMenuItem AddSceneComponentToolStrip(Type type)
        {
            if (InvokeRequired)
                return (ToolStripMenuItem)Invoke(new AddTypeToToolStrip(AddSceneComponentToolStrip), type);
            else
            {
                ToolStripMenuItem button = new ToolStripMenuItem(type.Name);
                button.Tag = type;
                toolStripSplitButtonAddSceneComponent.DropDownItems.Add(button);
                //list.Add(button);
                button.Click += new EventHandler(AddSceneComponent);
                _currentProject.IceSceneComponents.Add(type, type.Assembly);
                return button;
            }
        }


        private ToolStripMenuItem AddSceneItenComponentToolStrip(Type type)
        {
            if (InvokeRequired)
                return (ToolStripMenuItem)Invoke(new AddTypeToToolStrip(AddSceneItenComponentToolStrip), type);
            else
            {
                ToolStripMenuItem button = new ToolStripMenuItem(type.Name);
                button.Tag = type;

                menuAddSceneItemComponent.DropDownItems.Add(button);
                button.Click += new EventHandler(AddSceneItemComponentButtonClick);
                _currentProject.IceSceneItemComponents.Add(type, type.Assembly);
                return button;
            }
        }

        private void CheckForSceneItemComponents(Assembly _assembly)
        {
            throw new NotImplementedException();
        }

        private void CheckForSceneComponents(Assembly assembly)
        {
            List<ToolStripDropDownItem> list = new List<ToolStripDropDownItem>();
            foreach (Type _type in assembly.GetTypes())
            {
                if (_type.IsInterface)
                    continue;
                if (_type.Name.Contains("Component"))
                    Console.Write("");
                //Check if type is a scene component

                if (_type.BaseType.Name == "IceSceneComponent")
                {
                    if (!_currentProject.IceSceneComponents.ContainsKey(_type))
                    {
                        ToolStripMenuItem button = new ToolStripMenuItem(_type.Name);
                        button.Tag = _type;
                        toolStripSplitButtonAddSceneComponent.DropDownItems.Add(button);
                        //list.Add(button);
                        button.Click += new EventHandler(AddSceneComponent);
                        _currentProject.IceSceneComponents.Add(_type, assembly);

                    }
                }
                if (HasAttribute(_type))
                {
                    //if(_type.GetCustomAttributes(typeof(IceComponentAttribute),true).Length==1)
                    //{
                    if (!_currentProject.IceSceneItemComponents.ContainsKey(_type))
                    {
                        ToolStripMenuItem button = new ToolStripMenuItem(_type.Name);
                        button.Tag = _type;
                        list.Add(button);
                        menuAddSceneItemComponent.DropDownItems.Add(button);
                        button.Click += new EventHandler(AddSceneItemComponentButtonClick);
                        _currentProject.IceSceneItemComponents.Add(_type, assembly);
                    }
                }

            }
            list.Sort(delegate(ToolStripDropDownItem item1,
                                ToolStripDropDownItem item2)
            {
                return Comparer<string>.Default.Compare
                    (item1.Text, item2.Text);
            });

            menuAddSceneItemComponent.DropDownItems.AddRange(list.ToArray());
        }

        private bool HasAttribute(Type _type)
        {
            //Wierd stuff seems to happen when using Is IceComponentAttribute while using the source code.
            object[] _attribs = _type.GetCustomAttributes(true);
            foreach (var item in _attribs)
            {
                System.Attribute att = (Attribute)item;
                if (att.ToString().Contains("IceComponentAttribute"))
                    return true;
                else
                    Console.Write("DEBUG");
            }
            return false;
        }

        void AddSceneComponent(object sender, EventArgs e)
        {
            Type type = (Type)((ToolStripMenuItem)sender).Tag;

            if (SceneContainsComponent(type))
            {
                MessageBox.Show("SceneComponent \"" + type + "\"  already exists in the Scene");
                return;
            }
            object ice1 = type.Assembly.CreateInstance(type.FullName);
            if (ice1 != null)
            {
                SceneManager.ActiveScene.AddComponent(ice1 as IceSceneComponent);
                _objecToSelect = SceneManager.ActiveScene.SceneComponents
                    [SceneManager.ActiveScene.SceneComponents.Count - 1];
                SceneWasModified = true;
            }
            RefreshSceneComponentsNode();
        }

        private bool SceneContainsComponent(Type type)
        {
            foreach (IceSceneComponent comp in SceneManager.ActiveScene.SceneComponents)
            {
                if (comp.GetType() == type)
                    return true;
            }
            return false;
        }

        void AddSceneItemComponentButtonClick(object sender, EventArgs e)
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                Type type = (Type)((ToolStripMenuItem)sender).Tag;
                object ice1 = type.Assembly.CreateInstance(type.FullName);
                if (ice1 != null)
                {
                    try
                    {
                        IceComponent comp = (IceComponent)ice1;
                        foreach (var item in comp.GetType().GetProperties())
                        {
                            try
                            {
                                if (item.GetCustomAttributes(typeof(IceComponentPropertyAttribute), true).Length == 1)
                                {
                                    IceComponentPropertyAttribute att = (IceComponentPropertyAttribute)item.GetCustomAttributes(typeof(IceComponentPropertyAttribute), true)[0];
                                    if (att.DefaultValue != null && att.DefaultValue != string.Empty)
                                        item.SetValue(comp, Convert.ChangeType(att.DefaultValue, item.PropertyType), null);
                                }
                            }
                            catch
                            {
                            }
                        }
                        _objecToSelect = comp;
                    }
                    catch
                    {

                    }
                    SceneItem sceneItem = treeViewResources.SelectedNode.Tag as SceneItem;
                    sceneItem.AddComponent(ice1 as IceComponent);
                    SceneWasModified = true;
                }
                RefreshNodeComponents(treeViewResources.SelectedNode);
                AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch (Exception err)
            {
                ShowErrorMessage(new Exception("There was an error creating the Component", err));
            }
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                foreach (var item in Directory.GetFiles(Path.Combine(_currentProject.Path, _currentProject.BinaryFolderRelativePath)))
                {
                    if (item.EndsWith(".exe") || item.EndsWith(".dll"))
                    {
                        Assembly tmp = Assembly.LoadFile(item);
                        if (tmp.FullName == args.Name)
                            return tmp;
                    }
                }
                return null;
            }
            catch (Exception err)
            {
                ShowErrorMessage(err);
                return null;
            }
        }

        private void RefreshSceneComponentsNode()
        {
            treeViewResources.Nodes[3].Nodes.Clear();
            foreach (IceSceneComponent comp in SceneManager.ActiveScene.SceneComponents)
            {
                TreeNode node = new TreeNode(comp.GetType().Name);
                node.Tag = comp;
                treeViewResources.Nodes[3].Nodes.Add(node);
                if (_objecToSelect == comp)
                {
                    treeViewResources.SelectedNode = node;
                }
            }
        }

        private void InitializeCamera()
        {
            _sceneCamera = new Camera();
            _sceneCamera.Position = Vector2.Zero;
            SceneManager.ActiveScene.ActiveCameras.Clear();
            SceneManager.ActiveScene.ActiveCameras.Add(_sceneCamera);
        }

        private void LoadScene(String filename)
        {
            if (String.IsNullOrEmpty(filename))
            {
                throw new Exception("LoadScene() was called with an empty filename path");
            }            
            LoadingForm.ShowForm();
            Application.DoEvents();
            IceCream.Serialization.SceneSerializer.StatusUpdate += new IceCream.Serialization.StatusUpdateHandler(SceneSerializer_StatusUpdate);
            LoadSceneDelegate del = new LoadSceneDelegate(LoadSceneData);
            del.BeginInvoke(filename, new AsyncCallback(LoadSceneCallBack), null);

        }

        private void SceneSerializer_StatusUpdate(string update, int progress)
        {
            LoadingForm.ChangeStatus(update, progress);
        }

        private void LoadSceneCallBack(IAsyncResult res)
        {
            Invoke(new voidDelegate(EndLoadScene));
        }

        private void EndLoadScene()
        {
            try
            {
                if (!errorLoadingScene)
                {
                    if (SceneManager.ActiveScene == null)
                    {
                        throw new Exception("Something went wrong with trying to load the scene");
                    }
                    sceneEditorControl.HighlightedItem = null;
                    sceneEditorControl.SelectedItems.Clear();
                    LoadTextures(SceneManager.ActiveScene);
                    InitializeSceneItems(SceneManager.ActiveScene);

                    InitializeCamera();
                    ZoomNormal();
                    LoadSceneTreeViewItems();
                    treeViewResources.Nodes[0].Expand();
                    treeViewResources.Nodes[1].Expand();
                    treeViewResources.Nodes[2].Expand();

                    RefreshEditorStatus();
                    LoadingForm.HideForm();
                }
                else
                {
                    
                    _currentProject.LastOpenedScene = "";
                    _currentProject.Save();

                    LoadingForm.HideForm();
                    Application.DoEvents();
                }
            }
            catch (Exception err)
            {
                ShowErrorMessage(err);
            }
        }

        private void LoadSceneData(string fname)
        {
            try
            {
                errorLoadingScene = false;
                //Load the scene without trying to load the materials.
                IceScene scene = SceneManager.LoadScene(fname, false);
                SceneManager.ActiveScene = scene;
                _currentProject.LastOpenedScene = fname;
                _currentProject.CurrentSceneFile = fname;
            }
            catch (Exception err)
            {
                ShowErrorMessage(err);
                errorLoadingScene = true;
            }

        }
        bool errorLoadingScene;
        private void InitializeSceneItems(IceScene scene)
        {
            foreach (SceneItem _item in scene.SceneItems)
            {
                _item.SceneParent = scene;
                if (_item is TileGrid)
                {
                    ((TileGrid)_item).LoadData(false);
                }
                _item.UpdateBoundingRect();
                _item.SetupLinkFuses();
            }
        }

        private void LoadTextures(SceneBase scene)
        {
            foreach (Material _material in scene.Materials)
            {
                if (String.IsNullOrEmpty(_material.Filename))
                {
                    continue;
                }
                string path = Path.Combine(_currentProject.ContentFolderPath, _material.Filename);
                if (File.Exists(path))
                {
                    _contentBuilder.Add(path, _material.Name, "TextureImporter", "TextureProcessor");                    
                }
                else
                {
                    ShowErrorMessage("File does not exist " + Environment.NewLine + path);
                }
            }

            foreach (IceFont _font in scene.Fonts)
            {
                if (String.IsNullOrEmpty(_font.Filename))
                {
                    continue;
                }
                string path = Path.Combine(_currentProject.ContentFolderPath, _font.Filename);
                if (File.Exists(path))
                {
                    if (path.EndsWith("spritefont"))
                    {
                        _contentBuilder.Add(path, _font.Name, null, "FontDescriptionProcessor");
                    }
                    else
                    {
                        _contentBuilder.Add(path, _font.Name, null, "FontTextureProcessor");
                    }
                }
                else
                {
                    ShowErrorMessage("File does not exist " + Environment.NewLine + path);
                }
            }

            foreach (IceEffect _effect in scene.Effects)
            {
                if (String.IsNullOrEmpty(_effect.Filename))
                {
                    continue;
                }
                string path = Path.Combine(_currentProject.ContentFolderPath, _effect.Filename);
                if (File.Exists(path))
                {
                    if (path.EndsWith("fx"))
                    {
                        _contentBuilder.Add(path, _effect.Name, null, "EffectProcessor");
                    }
                }
                else
                {
                    ShowErrorMessage("File does not exist " + Environment.NewLine + path);
                }
            }
            //_contentBuilder.EventRaised -= new Action<string>(_contentBuilder_EventRaised);
            //_contentBuilder.EventRaised += new Action<string>(_contentBuilder_EventRaised);
            String errorMessage = _contentBuilder.Build();
            if (String.IsNullOrEmpty(errorMessage) == false)
            {
                LoadingForm.ChangeStatus("Error while building content: " + errorMessage, 0);
                return;
            }

            LoadingForm.ChangeStatus("Initializing Content",0);
            scene.InitializeContent(sceneEditorControl.Services, _contentBuilder.OutputDirectory);

            LoadingForm.ChangeHeader("Loading Materials");
            int prog = 0;
            foreach (Material _material in scene.Materials)
            {
                
                if (_material.Texture == null)
                {
                    LoadingForm.ChangeStatus(_material.Name, (prog/scene.Materials.Count) *100);
                    _material.Texture = scene.ContentManager.Load<Texture2D>(_material.Name);
                }
                prog++;
            }

            foreach (IceFont _font in scene.Fonts)
            {
                if (_font.Font == null)
                {
                    _font.Font = scene.ContentManager.Load<SpriteFont>(_font.Name);
                }
            }
            LoadingForm.ChangeHeader("Loading Effects");
            prog = 0;
            foreach (IceEffect _effect in scene.Effects)
            {
                LoadingForm.ChangeStatus(_effect.Name, (prog / scene.Effects.Count) * 100);
                if (_effect.Effects == null)
                {
                    _effect.Load(scene.ContentManager, new string[] { _effect.Name });
                    //_effect.Effects[0] = scene.ContentManager.Load<Effect>(_effect.Name);
                }
                prog++;
            }
        }

        int contentprogress = 0;
        void _contentBuilder_EventRaised(string obj)
        {
            contentprogress+=5;
            if(contentprogress>100)
                contentprogress=0;
            LoadingForm.ChangeStatus(obj, contentprogress);
        }

        private void OpenNewSceneWizardAndProcessResult()
        {
            if (_sceneWasModified)
            {
                if (ShowWarningQuestion
                    ("The scene was modified, if you open a new scene all changes will be lost.\nDo you want to close it without saving?") == false)
                {
                    return;
                }
            }
            SceneManager.Scenes.Clear();
            SceneManager.ActiveScene = new IceScene();
            InitializeCamera();
            SceneWasModified = true;
            sceneEditorControl.HighlightedItem = null;
            sceneEditorControl.SelectedItems.Clear();
            _currentProject.CurrentSceneFile = "";
            RefreshEditorStatus();
            LoadSceneTreeViewItems();
        }

        private void CloseActiveScene()
        {
            SceneManager.ActiveScene = null;
            sceneEditorControl.HighlightedItem = null;
            SceneWasModified = false;
            sceneEditorControl.SelectedItems.Clear();
            RefreshEditorStatus();
        }

        private void OpenNewProjectWizardAndProcessResult()
        {
            if (_sceneWasModified || _projectWasModified)
            {
                if (ShowWarningQuestion("The project was modified, if you open a new project all changes will be lost.\nDo you want to close it without saving?") == false)
                {
                    return;
                }
            }
            CloseActiveScene();
            _currentProject = null;
            ProjectWasModified = true;
            sceneEditorControl.HighlightedItem = null;
            sceneEditorControl.SelectedItems.Clear();
            RefreshEditorStatus();
        }

        private void OpenProjectSelectionDialogAndProcessResult()
        {
            if (_currentProject != null)
            {
                DialogResult result = MessageBox.Show(
                    "You already have a project open in IceCream" + Environment.NewLine +
                    "Unfortunately it's currently not possible to open a project without first closing milkshake" + Environment.NewLine +
                    "Would you like to clear the last opened project setting and close Milkshake?",
                    "OH NO!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (result == DialogResult.Yes)
                {
                    _preferences.LastOpenedProject = "";
                    _preferences.Save();
                    Application.Exit();
                    //TODO: Would be nice to restart the app too. would need to spawn another process to do this though.
                }
                return;
            }
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Open IceCream Project";
                openFileDialog.Filter = "IceCream Project file (*.icproj)|*.icproj";
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName != string.Empty)
                {
                    LoadProject(openFileDialog.FileName);
                }
            }
            catch (Exception err)
            {
                ShowErrorMessage("Unable to load project file: " + err.Message);
            }
        }

        private void OpenSceneSaveAsDialogAndProcessResult()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.AddExtension = true;
                saveFileDialog.DefaultExt = ".icescene";
                if (_currentProject.CurrentSceneFile == String.Empty)
                {
                    saveFileDialog.FileName = "New Scene.icescene";
                }
                else
                {
                    saveFileDialog.FileName = _currentProject.CurrentSceneFile;
                }
                saveFileDialog.ShowDialog();
                if (saveFileDialog.FileName != string.Empty)
                {
                    try
                    {
                        IceCream.Serialization.SceneSerializer.SerializeScene(SceneManager.ActiveScene, saveFileDialog.FileName);
                        _currentProject.CurrentSceneFile = saveFileDialog.FileName;
                        _currentProject.LastOpenedScene = saveFileDialog.FileName;
                        RefreshEditorStatus();
                    }
                    catch (Exception e)
                    {
                        ShowErrorMessage("Could not save the scene to \"" + saveFileDialog.FileName + "\": " + e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                ShowErrorMessage("Unable to save : " + e.Message);
            }
        }

        private void OpenSceneSelectionDialogAndProcessResult()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Open IceCream Scene";
                openFileDialog.Filter = "IceCream Scene file (*.icescene)|*.icescene";
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName != string.Empty)
                {
                    //If the selected file is not in the content folder or a sub folder of, the user shouldnt be able to add it
                    string roothPath = Path.GetDirectoryName(openFileDialog.FileName);
                    if (!roothPath.EndsWith("\\"))
                    {
                        roothPath += "\\";
                    }
                    LoadScene(openFileDialog.FileName);
                    _projectWasModified = true;
                    RefreshEditorStatus();
                    
                }
            }
            catch (Exception err)
            {
                ShowErrorMessage("Unable to load scene file: " + err);
            }
        }


        private void ExitMilkshake()
        {
            if (ConfirmMilkshakeExit())
            {
                this.Close();
            }
        }

        /// <summary>
        /// Ask the user for confirmation to exit milkshake if needed
        /// and process housekeeping
        /// </summary>
        /// <returns>False if the user choosed to cancel closing</returns>
        private bool ConfirmMilkshakeExit()
        {
            if (_preferences.IgnoreModificationWarning == false)
            {
                if (_sceneWasModified)
                {
                    if (ShowWarningQuestion("The scene was modified, if you quit all changes will be lost.\nDo you want to quit without saving?") == false)
                    {
                        return false;
                    }
                }
                if (_projectWasModified)
                {
                    if (ShowWarningQuestion("The project was modified, if you quit all changes will be lost.\nDo you want to quit without saving?") == false)
                    {
                        return false;
                    }
                }
            }
            // Save the user preferences
            _preferences.Save();
            return true;
        }

        private void SaveCurrentProject()
        {
            try
            {
                _currentProject.Save();
                ProjectWasModified = false;
            }
            catch (Exception err)
            {
                ShowErrorMessage("Unable to save project: " + err.Message);
            }
        }

        private void SaveAll()
        {
            SaveCurrentScene();
            SaveCurrentProject();
        }

        private void toolStripButtonSaveAll_Click(object sender, EventArgs e)
        {
            SaveAll();
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAll();
        }

        #endregion

        #region Scene Treeview Code

        #region Scene Treeview Toolbar Code

        internal void AddNewSceneItemInstance(SceneItem newItem, SceneItemGroup itemGroup, bool selectItem)
        {
            newItem.Position = _sceneCamera.Position + new Vector2(100, 100);
            newItem.SceneParent = SceneManager.ActiveScene;
            if (itemGroup == SceneItemGroup.LocalTemplates)
            {
                newItem.IsTemplate = true;
                SceneManager.ActiveScene.TemplateItems.Add(newItem);
            }
            else if (itemGroup == SceneItemGroup.GlobalTemplates)
            {
                newItem.IsTemplate = true;
                SceneManager.GlobalDataHolder.TemplateItems.Add(newItem);
            }
            else
            {
                newItem.IsTemplate = false;
                SceneManager.ActiveScene.SceneItems.Add(newItem);
            }
            if (selectItem == true)
            {
                _objecToSelect = newItem;
            }
            LoadSceneTreeViewItems();
            SceneWasModified = true;
        }

        private string GetNewSpriteName()
        {
            string defaultname = "New Sprite";
            string name = GetNewName(defaultname);
            return name;
        }

        private string GetNewTextItemName()
        {
            string defaultname = "New Text";
            string name = GetNewName(defaultname);
            return name;
        }

        private string GetNewSceneItemName()
        {
            string defaultname = "New Scene Item";
            string name = GetNewName(defaultname);
            return name;
        }

        private string GetNewName(string defaultname)
        {
            int i = 1;
            string name;
            while (true)
            {
                name = defaultname + " " + String.Format("{0:00}", i);
                if (IsNameUnique(name))
                {
                    break;
                }
                i++;
            }
            return name;
        }

        private bool IsNameUnique(String name)
        {
            foreach (var item in SceneManager.GlobalDataHolder.TemplateItems)
            {
                if (item.Name == name)
                {
                    return false;
                }
            }
            foreach (var item in SceneManager.ActiveScene.TemplateItems)
            {
                if (item.Name == name)
                {
                    return false;
                }
            }
            foreach (var item in SceneManager.ActiveScene.SceneItems)
            {
                if (item.Name == name)
                {
                    return false;
                }
            }
            return true;
        }

        private SceneItemTypeStruct[] GetSceneItemTypesTable()
        {
            SceneItemTypeStruct[] items = new SceneItemTypeStruct[Enum.GetValues(typeof(SceneItemType)).Length];
            items[0] = new SceneItemTypeStruct(SceneItemType.Sprite, "Sprite", "Sprites", GetIconFromType(SceneItemType.Sprite), GetImageFromType(SceneItemType.Sprite));
            items[1] = new SceneItemTypeStruct(SceneItemType.AnimatedSprite, "Animated Sprite", "Animated Sprites", GetIconFromType(SceneItemType.AnimatedSprite),
                GetImageFromType(SceneItemType.AnimatedSprite));
            items[2] = new SceneItemTypeStruct(SceneItemType.TileGrid, "Tile Grid", "Tile Grids", GetIconFromType(SceneItemType.TileGrid),
                GetImageFromType(SceneItemType.TileGrid));
            items[3] = new SceneItemTypeStruct(SceneItemType.ParticleEffect, "Particle Effect", "Particle Effects", GetIconFromType(SceneItemType.ParticleEffect),
                GetImageFromType(SceneItemType.ParticleEffect));
            items[4] = new SceneItemTypeStruct(SceneItemType.PostProcessingAnimation, "Post Processing Animation", "Post Processing Animations",
                GetIconFromType(SceneItemType.PostProcessingAnimation),
                GetImageFromType(SceneItemType.PostProcessingAnimation));
            items[5] = new SceneItemTypeStruct(SceneItemType.TextItem, "Text Item", "Text Items", GetIconFromType(SceneItemType.TextItem),
                GetImageFromType(SceneItemType.TextItem));
            items[6] = new SceneItemTypeStruct(SceneItemType.CompositeEntity, "Composite Entity", "Composite Entities",
              GetIconFromType(SceneItemType.CompositeEntity),
            GetImageFromType(SceneItemType.CompositeEntity));
            items[7] = new SceneItemTypeStruct(SceneItemType.Default, "Scene Item", "Scene Items",
                GetIconFromType(SceneItemType.Default),
                GetImageFromType(SceneItemType.Default));
            return items;
        }

        private void InitializeNodes()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MilkshakeForm));
            // Scene instances
            treeViewResources.Nodes[2].Nodes.Clear();
            // Templates
            treeViewResources.Nodes[1].Nodes[0].Nodes.Clear();
            treeViewResources.Nodes[1].Nodes[1].Nodes.Clear();
            toolStripSplitButtonAddSceneInstance.DropDownItems.Clear();
            foreach (SceneItemTypeStruct item in _sceneItemsTypeTable)
            {
                if (item.requiresTemplate == false)
                {
                    treeViewResources.Nodes[2].Nodes.Add("SceneItemInstance" + item.pluralName, item.pluralName, item.icon, item.icon);
                }
                treeViewResources.Nodes[1].Nodes[0].Nodes.Add("SceneItemTemplateGlobal" + item.pluralName, item.pluralName, item.icon, item.icon);
                treeViewResources.Nodes[1].Nodes[1].Nodes.Add("SceneItemTemplateLocal" + item.pluralName, item.pluralName, item.icon, item.icon);

                ToolStripItem dropItem;
                if (item.requiresTemplate == false)
                {
                    dropItem = toolStripSplitButtonAddSceneInstance.DropDownItems.Add(item.name, item.image, sceneItemToolStripMenuItem_Click);
                    dropItem.Tag = item.type;
                }
                dropItem = toolStripSplitButtonAddGlobalTemplate.DropDownItems.Add(item.name, item.image, sceneItemToolStripMenuItem_Click);
                dropItem.Tag = item.type;
                dropItem = toolStripSplitButtonAddLocalTemplate.DropDownItems.Add(item.name, item.image, sceneItemToolStripMenuItem_Click);
                dropItem.Tag = item.type;
            }
        }

        private void sceneItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is ToolStripItem)
                {
                    ToolStripItem menuItem = sender as ToolStripItem;
                    SceneItemGroup itemGroup = SceneItemGroup.SceneInstances;
                    if (toolStripSplitButtonAddGlobalTemplate.DropDownItems.Contains(menuItem))
                    {
                        itemGroup = SceneItemGroup.GlobalTemplates;
                    }
                    else if (toolStripSplitButtonAddLocalTemplate.DropDownItems.Contains(menuItem))
                    {
                        itemGroup = SceneItemGroup.LocalTemplates;
                    }
                    AddNewSceneItemToTree((SceneItemType)menuItem.Tag, itemGroup);
                }
            }
            catch (Exception err)
            {
                ShowErrorMessage(err);
            }
        }

        private void AddNewSceneItemToTree(SceneItemType type, SceneItemGroup itemGroup)
        {
            SceneItem sceneItem;
            sceneItem = InstanciateNewItemOfType(type);
            sceneItem.Name = GetNewSceneItemName();
            AddNewSceneItemInstance(sceneItem, itemGroup, true);          
        }

        internal SceneItem InstanciateNewItemOfType(SceneItemType type)
        {
            SceneItem item = null;
            switch (type)
            {
                case SceneItemType.AnimatedSprite:
                    item = new AnimatedSprite();
                    AnimatedSprite animatedSprite = item as AnimatedSprite;
                    animatedSprite.Material = SceneManager.GetEmbeddedTileGridMaterial();
                    AnimationInfo newAnim = new AnimationInfo("Counting");
                    newAnim.AnimationFrames.Add(new AnimationFrame(20, "1"));
                    newAnim.AnimationFrames.Add(new AnimationFrame(20, "2"));
                    newAnim.AnimationFrames.Add(new AnimationFrame(20, "3"));
                    newAnim.AnimationFrames.Add(new AnimationFrame(20, "4"));
                    animatedSprite.AddAnimation(newAnim);
                    animatedSprite.PlayAnimation("Counting");
                    break;
                case SceneItemType.ParticleEffect:
                    item = new ParticleEffect();
                    ParticleEffect effect = item as ParticleEffect;
                    IceCream.SceneItems.ParticlesClasses.ParticleType pType = new IceCream.SceneItems.ParticlesClasses.ParticleType();
                    pType.Material = SceneManager.GetEmbeddedParticleMaterial();                    
                    effect.Emitter.ParticleTypes.Add(pType);
                    effect.Name = "New Particle Effect";
                    effect.Play();
                    break;
                case SceneItemType.PostProcessingAnimation:
                    item = new PostProcessAnimation();
                    item.Layer = 1;
                    break;
                case SceneItemType.Sprite:
                    item = new Sprite();
                    Sprite sprite = item as Sprite;
                    sprite.Name = GetNewSpriteName(); ;
                    sprite.Material = SceneManager.GetEmbeddedParticleMaterial();
                    break;
                case SceneItemType.TextItem:
                    item = new TextItem();
                    TextItem text = item as TextItem;
                    text.Name = GetNewTextItemName();
                    text.Font = SceneManager.GetEmbeddedFont("DefaultFont");
                    break;
                case SceneItemType.TileGrid:
                    item = new TileGrid();
                    TileGrid tileGrid = item as TileGrid;
                    tileGrid.Name = "New Tile Grid";
                    tileGrid.Material = SceneManager.GetEmbeddedTileGridMaterial();

                    tileGrid.TileRows = 4;
                    tileGrid.TileCols = 10;
                    tileGrid.TileSize = new Microsoft.Xna.Framework.Point(32, 32);
                    TileLayer newLayer = new TileLayer(tileGrid.TileCols, tileGrid.TileRows);
                    newLayer.Parent = tileGrid;
                    newLayer.Visible = true;
                    newLayer.Name = "Layer 1";
                    for (int tx = 0; tx < tileGrid.TileCols; tx++)
                    {
                        for (int ty = 0; ty < tileGrid.TileRows; ty++)
                        {
                            newLayer.Tiles[tx][ty].Index = 0;
                        }
                    }
                    tileGrid.TileLayers.Add(newLayer);
                    break;       
                case SceneItemType.CompositeEntity:
                    item = new CompositeEntity();
                    CompositeEntity composite = item as CompositeEntity;
                    break;
                default:
                    item = new SceneItem();
                    break;
            }
            return item;
        }


        #endregion

        #region Drag and Drop

        private void treeViewResources_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if ((e.Item as TreeNode).Tag is SceneItem)
            {
                DoDragDrop(e.Item, DragDropEffects.Copy);
            }
        }

        private void treeViewResources_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void treeViewResources_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void treeViewResources_DragOver(object sender, DragEventArgs e)
        {

        }

        private void sceneEditorControl_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            TreeNode node = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (node != null && node.Tag != null)
            {
                SceneItem item = (SceneItem)node.Tag;
                SceneItem copy = CreateNewInstaceCopyOf(item);
                AddNewSceneItemInstance(copy, SceneItemGroup.SceneInstances, true);
                System.Drawing.Point screenOrigin = sceneEditorControl.PointToScreen(new System.Drawing.Point(0, 0));
                sceneEditorControl.RealMousePos = new Vector2(e.X - screenOrigin.X, e.Y - screenOrigin.Y);
                copy.Position = sceneEditorControl.SceneMousePos;
                Console.WriteLine("Dragged object at " + (e.X - screenOrigin.X) + "," + (e.Y - screenOrigin.Y) + ": " + item);
            }
        }

        private void sceneEditorControl_DragEnter(object sender, DragEventArgs e)
        {
            TreeNode node = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (node != null && node.Tag != null)
            {
                SceneItem item = (SceneItem)node.Tag;
                if (item.IsTemplate == true)
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        #endregion

        #region Misc

        private void LoadSceneTreeViewItems()
        {
            LoadSceneItemsFromGroup(SceneItemGroup.GlobalTemplates);
            LoadSceneItemsFromGroup(SceneItemGroup.LocalTemplates);
            LoadSceneItemsFromGroup(SceneItemGroup.SceneInstances);
            RefreshSceneComponentsNode();
        }

        /// <summary>
        /// Creates all the nodes of SceneItems from a group's collection
        /// </summary>
        /// <param name="sceneItemGroup"></param>
        private void LoadSceneItemsFromGroup(SceneItemGroup sceneItemGroup)
        {
            TreeNode rootNode;
            List<SceneItem> itemList = SceneManager.ActiveScene.SceneItems;
            switch (sceneItemGroup)
            {
                case SceneItemGroup.GlobalTemplates:
                    rootNode = treeViewResources.Nodes[1].Nodes[0];
                    itemList = SceneManager.GlobalDataHolder.TemplateItems;
                    break;
                case SceneItemGroup.LocalTemplates:
                    rootNode = treeViewResources.Nodes[1].Nodes[1];
                    itemList = SceneManager.ActiveScene.TemplateItems;
                    break;
                default:
                    itemList = SceneManager.ActiveScene.SceneItems;
                    rootNode = treeViewResources.Nodes[2];
                    break;
            }
            // clear all the nodes
            foreach (TreeNode node in rootNode.Nodes)
            {
                node.Nodes.Clear();
            }
            // add the nodes

            foreach (SceneItem item in itemList)
            {
                SceneItemType itemType = GetTypeOfSceneItem(item);
                TreeNode newNode = rootNode.Nodes[(int)itemType].Nodes.Add(item.Name);
                String icon = GetIconFromType(itemType);
                newNode.SelectedImageKey = newNode.ImageKey = icon;
                newNode.Tag = item;
                // add a reference to the node and the item
                if (sceneItemGroup == SceneItemGroup.SceneInstances)
                {
                    _sceneItemNodes[item] = newNode;
                }
                if (item == _objecToSelect)
                {
                    treeViewResources.SelectedNode = newNode;
                }
                RefreshNodeComponents(newNode);
            }
        }

        internal static String GetIconFromType(SceneItemType type)
        {
            string icon = "plugin.png";
            if (type == SceneItemType.Sprite)
            {
                icon = "sport_soccer.png";
            }
            else if (type == SceneItemType.AnimatedSprite)
            {
                icon = "car.png";
            }
            else if (type == SceneItemType.ParticleEffect)
            {
                icon = "weather_sun.png";
            }
            else if (type == SceneItemType.PostProcessingAnimation)
            {
                icon = "weather_lightning.png";
            }
            else if (type == SceneItemType.TextItem)
            {
                icon = "font.png";
            }
            else if (type == SceneItemType.TileGrid)
            {
                icon = "color_swatch.png";
            }
            else if (type == SceneItemType.CompositeEntity)
            {
                icon = "user.png";
            }
            return icon;
        }

        private Bitmap GetImageFromType(SceneItemType type)
        {
            Bitmap icon = Properties.Resources.plugin;
            if (type == SceneItemType.Sprite)
            {
                icon = Properties.Resources.sport_soccer;
            }
            else if (type == SceneItemType.AnimatedSprite)
            {
                icon = Properties.Resources.car;
            }
            else if (type == SceneItemType.ParticleEffect)
            {
                icon = Properties.Resources.weather_sun;
            }
            else if (type == SceneItemType.PostProcessingAnimation)
            {
                icon = Properties.Resources.weather_lightning;
            }
            else if (type == SceneItemType.TextItem)
            {
                icon = Properties.Resources.font;
            }
            else if (type == SceneItemType.TileGrid)
            {
                icon = Properties.Resources.color_swatch;
            }
            else if (type == SceneItemType.CompositeEntity)
            {
                icon = Properties.Resources.user;
            }
            return icon;
        }

        private void RefreshNodeComponents(TreeNode node)
        {
            node.Nodes.Clear();
            SceneItem item = node.Tag as SceneItem;
            // Components code
            String componentIcon = "cog.png";
            foreach (IceComponent comp in item.Components)
            {
                IceComponentAttribute attrib = GetCustomAttribute(comp);
                if (attrib != null)
                {
                    TreeNode compNode = node.Nodes.Add("Node" + item.Name + attrib.FriendlyName,
                        attrib.FriendlyName, componentIcon, componentIcon);
                    compNode.Tag = comp;
                    if (comp == _objecToSelect)
                    {
                        treeViewResources.SelectedNode = compNode;
                    }
                }
            }
            RefreshComponentContextMenu(item);
        }

        private IceComponentAttribute GetCustomAttribute(IceComponent comp)
        {
            if (comp.GetType().IsDefined(typeof(IceComponentAttribute), true))
            {
                IceComponentAttribute attrib = (IceComponentAttribute)comp.GetType().GetCustomAttributes(typeof(IceComponentAttribute), true)[0];
                return attrib;

            }
            return null;
        }

        /// <summary>
        /// Returns the type of a SceneItem
        /// </summary>
        internal static SceneItemType GetTypeOfSceneItem(SceneItem item)
        {
            if (item is TileGrid)
            {
                return SceneItemType.TileGrid;
            }
            else if (item is AnimatedSprite)
            {
                return SceneItemType.AnimatedSprite;
            }
            else if (item is Sprite)
            {
                return SceneItemType.Sprite;
            }
            else if (item is ParticleEffect)
            {
                return SceneItemType.ParticleEffect;
            }
            else if (item is PostProcessAnimation)
            {
                return SceneItemType.PostProcessingAnimation;
            }
            else if (item is TextItem)
            {
                return SceneItemType.TextItem;
            }
            else if (item is CompositeEntity)
            {
              return SceneItemType.CompositeEntity;
            }
            else
            {
                return SceneItemType.Default;
            }
        }

        private void treeViewResources_AfterSelect(object sender, TreeViewEventArgs e)
        {
            menuAddSceneItemComponent.Visible = false;
            sceneEditorControl.PreviewedItem = null;
            if (e.Node.Tag != null)
            {
                if (e.Node.Tag is SceneItem)
                {
                    SceneItem item = e.Node.Tag as SceneItem;
                    SelectItem(item);
                    menuAddSceneItemComponent.Visible = true;
                    RefreshComponentContextMenu(item);
                }
                else if (e.Node.Tag is IceSceneComponent)
                {
                    propertyGridSceneItem.SelectedObject = e.Node.Tag;
                    genericComponentControl.SelectedComponent = null;
                    genericComponentControl.Visible = false;
                    propertyGridSceneItem.Visible = true;
                }
                else if (e.Node.Tag is IceComponent)
                {
                    SelectSceneItemComponent(e.Node.Tag as IceComponent);
                }
            }
            RefreshMenuStripAndToolStripStatus();
        }

        private void RefreshComponentContextMenu(SceneItem item)
        {
            try
            {
                foreach (ToolStripDropDownItem menu in menuAddSceneItemComponent.DropDownItems)
                {
                    if (SceneItemContainsComponent(item, menu.Tag as Type))
                    {
                        menu.Visible = false;
                    }
                    else
                    {
                        menu.Visible = true;
                    }
                }
            }
            catch (Exception err)
            {
                ShowErrorMessage(err);
            }
        }

        private bool SceneItemContainsComponent(SceneItem item, Type iceComponent)
        {

            foreach (var comp in item.Components)
            {
                if (comp.GetType().Equals(iceComponent))
                    return true;
            }
            return false;
        }

        private void treeViewResources_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is SceneItem)
            {
                EditSceneItem((SceneItem)e.Node.Tag);
            }
            else if (e.Node == treeViewResources.Nodes[0].Nodes[0])
            {
                MaterialEditor materialEditor = new MaterialEditor();
                materialEditor.StartPosition = FormStartPosition.CenterParent;
                materialEditor.ShowLocalTextures = false;
                materialEditor.ShowDialog(this);

            }
            else if (e.Node == treeViewResources.Nodes[0].Nodes[1])
            {
                MaterialEditor materialEditor = new MaterialEditor();
                materialEditor.ShowLocalTextures = true;
                materialEditor.ShowDialog();
            }
            else if (e.Node.Tag is IceComponent)
            {
                IceComponent component = e.Node.Tag as IceComponent;
                EditSceneItemComponent(component);
            }
        }

        private void treeViewResources_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyPress(e);
        }

        #endregion

        #endregion

        #region SceneItem handling

        private void DeleteItem(object item)
        {
            if (SceneManager.ActiveScene.SceneItems.Contains(item as SceneItem))
            {
                SceneManager.ActiveScene.SceneItems.Remove(item as SceneItem);
                SceneWasModified = true;
            }
            else if (SceneManager.ActiveScene.TemplateItems.Contains(item as SceneItem))
            {
                SceneManager.ActiveScene.TemplateItems.Remove(item as SceneItem);
                SceneWasModified = true;
            }
            else if (SceneManager.GlobalDataHolder.TemplateItems.Contains(item as SceneItem))
            {
                SceneManager.GlobalDataHolder.TemplateItems.Remove(item as SceneItem);
                SceneWasModified = true;
                ProjectWasModified = true;
            }
            else if (SceneManager.ActiveScene.SceneComponents.Contains(item as IceSceneComponent))
            {
                SceneManager.ActiveScene.SceneComponents.Remove(item as IceSceneComponent);
                SceneWasModified = true;
            }

        }

        #endregion

        #region Scene Input

        private void ZoomIn()
        {
            ZoomFactor += _zoomDelta;
        }

        private void ZoomOut()
        {
            ZoomFactor -= _zoomDelta;
        }

        private void ZoomNormal()
        {
            ZoomFactor = 1f;
        }

        const int WM_MOUSEWHEEL = 0x20A;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_MOUSEWHEEL)
            {
                if (SceneManager.ActiveScene != null)
                {
                    if ((int)m.WParam > 0)
                    {
                        ZoomIn();
                    }
                    else
                    {
                        ZoomOut();
                    }
                }
            }
        }

        public void SelectSceneItemComponent(IceComponent component)
        {

            SceneItem item = component.Owner as SceneItem;
            // if the item is not currently selected in the scene editor,
            // select it (after clearing the current selection)
            if (sceneEditorControl.SelectedItems.Contains(item) == false)
            {
                sceneEditorControl.SelectedItems.Clear();
                if (item.IsTemplate == false)
                {
                    sceneEditorControl.SelectedItems.Add(item);
                }
                else
                {
                    sceneEditorControl.PreviewedItem = item;
                }
            }
            menuAddSceneItemComponent.Visible = false;
            labelSceneItemProperties.Text = component.ToString();
            propertyGridSceneItem.SelectedObject = null;
            propertyGridSceneItem.Visible = false;
            genericComponentControl.SelectedComponent = component;
            genericComponentControl.Visible = true;
            _objecToSelect = component;
        }

        public void SelectItem(SceneItem item)
        {
            if (SceneManager.ActiveScene.SceneItems.Contains(item))
            {
                TreeNode treeNode = _sceneItemNodes[item];
                if (treeViewResources.SelectedNode != treeNode)
                {
                    treeViewResources.SelectedNode = treeNode;
                }
            }
            // if the item is not currently selected in the scene editor,
            // select it (after clearing the current selection)
            if (sceneEditorControl.SelectedItems.Contains(item) == false)
            {
                sceneEditorControl.SelectedItems.Clear();
                if (item.IsTemplate == false)
                {
                    sceneEditorControl.SelectedItems.Add(item);
                }
                else
                {
                    sceneEditorControl.PreviewedItem = item;
                }
            }
            labelSceneItemProperties.Text = item.Name;
            propertyGridSceneItem.SelectedObject = item;
            genericComponentControl.SelectedComponent = null;
            genericComponentControl.Visible = false;
            propertyGridSceneItem.Visible = true;

            toolStripButtonDelete.Enabled = true;
            _objecToSelect = item;
            RefreshMenuStripAndToolStripStatus();
        }

        public void EditSceneItem(SceneItem item)
        {
            if (item != null)
            {
                bool isSceneInstance = false;
                if (SceneManager.ActiveScene.SceneItems.Contains(item) 
                    || SceneManager.ActiveScene.TemplateItems.Contains(item))
                {
                    isSceneInstance = true;
                }
                if (OpenSceneItemInEditor(item, isSceneInstance) == true)
                {
                    SceneWasModified = true;
                    RefreshEditorStatus();
                }
            }
        }

        public bool OpenSceneItemInEditor(SceneItem item, bool isSceneInstance)
        {
            SceneItemType itemType = GetTypeOfSceneItem(item);
            SceneItemEditor editor = null;
            switch (itemType)
            {
                case SceneItemType.TileGrid:
                    editor = new TileGridEditor();
                    break;                
                case SceneItemType.ParticleEffect:
                    editor = new ParticleEffectEditor();
                    break;
                case SceneItemType.AnimatedSprite:
                    editor = new AnimatedSpriteEditor();
                    break;
                case SceneItemType.Sprite:
                    editor = new SpriteEditor();
                    break;
                case SceneItemType.PostProcessingAnimation:
                    editor = new PostProcessAnimationEditor();
                    break;
                case SceneItemType.CompositeEntity:
                    editor = new CompositeEntityEditor();
                    break;
                default:
                    editor = null;
                    break;
            }
            if (editor != null)
            {
                editor.SceneItem = item;
                editor.ItemIsLocal = isSceneInstance;
                editor.StartPosition = FormStartPosition.CenterParent;
                if (editor.ShowDialog(this) == DialogResult.OK)
                {
                    if (SceneManager.GlobalDataHolder.TemplateItems.Contains(item))
                    {
                        this.SceneWasModified = true;
                    }
                    else
                    {
                        this.SceneWasModified = true;
                    }
                }
            }
            return false;
        }

        public void UnselectItems()
        {
            sceneEditorControl.SelectedItems.Clear();
            treeViewResources.SelectedNode = null;
            RefreshMenuStripAndToolStripStatus();
        }

        public void UpdatePropertyGrid()
        {
            if (propertyGridSceneItem.SelectedObject != null)
            {
                propertyGridSceneItem.SelectedObject = propertyGridSceneItem.SelectedObject;
            }
        }

        private void propertyGridSceneItem_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            SceneWasModified = true;
            if (e.ChangedItem.Label == "Name")
            {
                LoadSceneTreeViewItems();
            }
        }

        public void CutSelectedItems()
        {
            CopySelectedItems();
            DeleteSelectedItems();
        }

        public void CopySelectedItems()
        {

            clipBoardSceneItems.Clear();
            if (sceneEditorControl.SelectedItems.Count == 0)
                return;
            for (int i = 0; i < sceneEditorControl.SelectedItems.Count; i++)
            {
                SceneItem copy = CreateNewInstaceCopyOf(sceneEditorControl.SelectedItems[i]);
                clipBoardSceneItems.Add(copy);
            }
            RefreshMenuStripAndToolStripStatus();
        }

        /// <summary>
        /// Create a new instance of a SceneItem using Reflection and copy the desired item value into it
        /// </summary>
        /// <param name="item">A deep copy of the item</param>
        /// <returns></returns>
        public SceneItem CreateNewInstaceCopyOf(SceneItem item)
        {
            SceneItem copy = (SceneItem)item.GetType().Assembly.CreateInstance(item.GetType().FullName, true);
            item.CopyValuesTo(copy);
            return copy;
        }

        public List<SceneItem> PasteSelectedItems()
        {
            Console.WriteLine("==== Clipboard ====");
            List<SceneItem> _tempItems = new List<SceneItem>();
            for (int i = 0; i < clipBoardSceneItems.Count; i++)
            {
                SceneItem item = clipBoardSceneItems[i];
                Console.WriteLine("Item: [" + item.IsTemplate + "] (" + item.GetType().Name + ") " + item.Name);
                if (item.IsTemplate == false)
                {
                    // create a copy of the item again, for consecutives pasting support
                    SceneItem copy = CreateNewInstaceCopyOf(item);
                    Vector2 position = copy.Position;
                    AddNewSceneItemInstance(copy, SceneItemGroup.SceneInstances, true);
                    _tempItems.Add(copy);
                    // use a small displacement for the copied object
                    copy.Position = position + new Vector2(20, 20);
                }
            }
            return _tempItems;
        }

        public void DeleteSelectedItems()
        {
            if (_preferences.ConfirmBeforeObjectDelete == true)
            {
                DialogResult res = MessageBox.Show("Are you sure that you want to delete?",
                       "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.No)
                {
                    return;
                }
            }

            object selectedObject = null;
            if (treeViewResources.SelectedNode != null & treeViewResources.SelectedNode.Tag != null)
            {
                selectedObject = treeViewResources.SelectedNode.Tag;
            }
            TreeNode _nodeToSelect = treeViewResources.SelectedNode.Parent;

            // Delete SceneItem scene instance(s)
            if (selectedObject == null || (selectedObject is SceneItem && ((SceneItem)selectedObject).IsTemplate == false))                
            {                
                for (int i = 0; i < sceneEditorControl.SelectedItems.Count; i++)
                {
                    SceneItem item = sceneEditorControl.SelectedItems[i];
                    // remove the item from the highlight if needed
                    if (sceneEditorControl.HighlightedItem == item)
                    {
                        sceneEditorControl.HighlightedItem = null;
                    }
                    DeleteItem(item);                  
                }                
                sceneEditorControl.SelectedItems.Clear();
                LoadSceneTreeViewItems();
            }
            else if (selectedObject is SceneItem) // if SceneItem template
            {
                SceneItem item = treeViewResources.SelectedNode.Tag as SceneItem;
                DeleteItem(item);
                treeViewResources.SelectedNode.Remove();
                propertyGridSceneItem.SelectedObject = null;
            }
            else if (selectedObject is IceComponent)
            {
                SceneItem item = treeViewResources.SelectedNode.Parent.Tag as SceneItem;
                item.Components.Remove(treeViewResources.SelectedNode.Tag as IceComponent);
                treeViewResources.SelectedNode.Remove();
                propertyGridSceneItem.SelectedObject = null;
            }
            else if (selectedObject is IceSceneComponent)
            {
                IceSceneComponent item = treeViewResources.SelectedNode.Tag as IceSceneComponent;
                SceneManager.ActiveScene.SceneComponents.Remove(item);
                treeViewResources.SelectedNode.Remove();
                propertyGridSceneItem.SelectedObject = null;
            }
            if (_nodeToSelect != null)
            {
                treeViewResources.SelectedNode = _nodeToSelect;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            HandleKeyPress(e);
            base.OnKeyDown(e);
        }

        public void HandleKeyPress(KeyEventArgs e)
        {
            if (SceneManager.ActiveScene == null || SceneManager.ActiveScene.ActiveCameras.Count == 0)
            {
                return;
            }
            Camera cam = SceneManager.ActiveScene.ActiveCameras[0];
            try
            {
                Console.WriteLine("key down: " + e.KeyCode);
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        DeleteSelectedItems();
                        break;
                    case Keys.M:

                        if (sceneEditorControl.SelectedItems.Count >= 2)
                        {
                            SceneItem local = sceneEditorControl.SelectedItems[1];
                            SceneItem target = sceneEditorControl.SelectedItems[0];
                            Console.WriteLine("Mounting " + target.Name + " to " + local.Name);
                            local.Mount(target, "LP", "LP");
                        }
                        else if (sceneEditorControl.SelectedItems.Count == 1)
                        {
                            SceneItem target = sceneEditorControl.SelectedItems[0];
                            if (target.IsMounted)
                            {
                                Console.WriteLine("UnMounting " + target.Name + " from " + target.MountOwner.Name);
                                target.UnMount();
                            }
                        }
                        break;
                    case Keys.Space:
                        SceneManager.ActiveScene.ActiveCameras[0].Position = new Vector2(0, 0);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception err)
            {
                ShowErrorMessage(err);
            }
        }

        #endregion


        private void windowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ComponentTypeContainer.UnloadAppDomain();
                //string msBuildPath = @"C:\Windows\Microsoft.NET\Framework\v3.5\";

                // Instantiate a new FileLogger to generate build log
                Microsoft.Build.Logging.FileLogger logger = new Microsoft.Build.Logging.FileLogger();                

                // Set the logfile parameter to indicate the log destination
                logger.Parameters = @"logfile=C:\temp\build.log";

                // Register the logger with the engine
                //engine.RegisterLogger(logger);

                String projectPath = Path.Combine(_currentProject.Path, _currentProject.VisualStudioProjectPath);
                Microsoft.Build.Evaluation.Project p = new Microsoft.Build.Evaluation.Project(projectPath);

                //BuildRequestData buildRequest = new BuildRequestData

                //bool success=p.Build("Rebuild");
                bool success = p.Build(logger); 
                if (!success)
                {
                    DialogResult _res = MessageBox.Show("There was an error during build, Would you like to see the log?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (_res == DialogResult.Yes)
                    {
                        if (File.Exists(@"c:\temp\build.log"))
                            Process.Start("notepad.exe", @"c:\temp\build.log");
                        else
                            MessageBox.Show("Log file not found");
                    }
                }
            }
            catch (Exception err)
            {
                MilkshakeForm.ShowErrorMessage(err);
            }
        }

        public void RemoveFileFromContentProject(String relativeFileName)
        {

        }

        public void AddFileToContentProject(String filename, bool shouldCompile, bool copyToOuputFolder, 
            String processor, String importer)
        {
            /*
            try
            {
                foreach (var item in _currentProject.VSProj.AllEvaluatedItems)
                {
                    if (item is Microsoft.Build.Evaluation.ProjectItem)
                    {
                        ProjectItem buildItem = (ProjectItem)item;
                        if (buildItem. == "NestedContentProject")
                        {
                            string _name = buildItem.Include;
                            _name = Path.Combine(_currentProject.Path, _name);
                            Project p = new Project();
                            p.Load(_name);
                            string _root = Path.GetDirectoryName(buildItem.Include);
                            String action = "None";
                            if (shouldCompile == true)
                            {
                                action = "Compile";
                            }
                            BuildItemGroup grp = p.GetEvaluatedItemsByName(action);
                            string include = filename.Replace(_root, "").Replace("/", "\\");
                            if (include.StartsWith("\\"))
                            {
                                include = include.Substring(1);
                            }
                            bool found = false;
                            foreach (BuildItem item2 in grp)
                            {
                                if (item2.Include == include)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (found == false)
                            {
                                BuildItem item1 = p.AddNewItem(action, include);                                
                                item1.SetMetadata("Name", Path.GetFileNameWithoutExtension(filename));
                                if (copyToOuputFolder == true)
                                {
                                    item1.SetMetadata("CopyToOutputDirectory", "PreserveNewest");
                                }
                                if (String.IsNullOrEmpty(importer) == false)
                                {
                                    item1.SetMetadata("Importer", importer);
                                }
                                if (String.IsNullOrEmpty(processor) == false)
                                {
                                    item1.SetMetadata("Processor", processor);
                                }                                
                                p.Save(_name);                               
                            }
                            p = null;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ShowErrorMessage(err);
            }
            */
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(_currentProject.Path, _currentProject.BinaryFolderRelativePath);
            path = Path.Combine(path, _currentProject.GameExe);
            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path));
            else
                MessageBox.Show("The game exe cannot be found, please make sure the build succeeded");
        } 

        private void treeViewResources_Click(object sender, EventArgs e)
        {

        }

        private void treeViewResources_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeViewResources.SelectedNode = treeViewResources.GetNodeAt(new System.Drawing.Point(e.X, e.Y));
            }
        }

        private void grid1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectGrid(1, true);
        }

        private void grid2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectGrid(2, true);
        }

        private void grid3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectGrid(3, true);
        }

        private void SelectGrid(int id, bool forceEnable)
        {
            if (_preferences.ShowGrid == false && forceEnable == true)
            {
                _preferences.ShowGrid = true;
            }
            _preferences.SelectedGrid = id;            
            grid1ToolStripMenuItem.Checked = false;
            grid1ToolStripMenuItem.Text = GetGridMenuText(1);
            grid2ToolStripMenuItem.Checked = false;
            grid2ToolStripMenuItem.Text = GetGridMenuText(2);
            grid3ToolStripMenuItem.Checked = false;
            grid3ToolStripMenuItem.Text = GetGridMenuText(3);
            switch (id)
            {
                case 1:
                    grid1ToolStripMenuItem.Checked = true;
                    break;
                case 2:
                    grid2ToolStripMenuItem.Checked = true;
                    break;
                default:
                    grid3ToolStripMenuItem.Checked = true;
                    break;
            }       
        }

        private String GetGridMenuText(int id)
        {
            String name = "Grid " + id + ": " 
                + _preferences.GridSizes[id-1].X + "x" + _preferences.GridSizes[id-1].Y;
            return name;
        }

        private void toolStripButtonToolSelect_Click(object sender, EventArgs e)
        {
            this.SceneEditorTool = MilkshakeSceneEditorTool.Select;
        }

        private void toolStripButtonToolTemplateBrush_Click(object sender, EventArgs e)
        {
            this.SceneEditorTool = MilkshakeSceneEditorTool.TemplateBrush;
        }

        private void toolStripButtonToolCamera_Click(object sender, EventArgs e)
        {
            this.SceneEditorTool = MilkshakeSceneEditorTool.Camera;
        }

        private void spriteSheetGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpriteSheetGenerator ssGenerator = new SpriteSheetGenerator();
            ssGenerator.Show();
        }
    }
}
