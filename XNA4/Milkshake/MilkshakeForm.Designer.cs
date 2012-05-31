
namespace Milkshake
{
    partial class MilkshakeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MilkshakeForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Project Materials (Global)");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Scene Materials (Local)");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Materials", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Project Templates (Global)");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Scene Templates (Local)");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Templates", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Scene Instances");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Scene Components");
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentProjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scenesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.openSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentScenesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.saveSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSceneAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.scenePropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildwindowsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spriteSheetGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOpenProject = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveProject = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonProjectSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonNewScene = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOpenScene = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveScene = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonCut = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCopy = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNoZoom = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonToolSelect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonToolTemplateBrush = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonToolCamera = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButtonGrid = new System.Windows.Forms.ToolStripSplitButton();
            this.grid1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grid2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grid3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelProjectName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelScene = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSceneMousePos = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerSceneEdition = new System.Windows.Forms.SplitContainer();
            this.tabControlSceneEditionTabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.toolStripResources = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButtonAddSceneInstance = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSplitButtonAddLocalTemplate = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSplitButtonAddGlobalTemplate = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSplitButtonAddSceneComponent = new System.Windows.Forms.ToolStripDropDownButton();
            this.treeViewResources = new System.Windows.Forms.TreeView();
            this.contextmenuTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuAddSceneItemComponent = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListTreeview = new System.Windows.Forms.ImageList(this.components);
            this.genericComponentControl = new Milkshake.Editors.Components.GenericComponentControl();
            this.labelSceneItemProperties = new System.Windows.Forms.Label();
            this.propertyGridSceneItem = new System.Windows.Forms.PropertyGrid();
            this.sceneEditorControl = new Milkshake.GraphicsDeviceControls.SceneEditorControl();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.splitContainerSceneEdition.Panel1.SuspendLayout();
            this.splitContainerSceneEdition.Panel2.SuspendLayout();
            this.splitContainerSceneEdition.SuspendLayout();
            this.tabControlSceneEditionTabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.toolStripResources.SuspendLayout();
            this.contextmenuTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.projectToolStripMenuItem,
            this.scenesToolStripMenuItem,
            this.buildToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(836, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.openToolStripMenuItem,
            this.recentProjectsToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveProjectToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newProjectToolStripMenuItem.Image")));
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.newProjectToolStripMenuItem.Text = "New Project";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.openToolStripMenuItem.Text = "Open Project...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // recentProjectsToolStripMenuItem
            // 
            this.recentProjectsToolStripMenuItem.Enabled = false;
            this.recentProjectsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("recentProjectsToolStripMenuItem.Image")));
            this.recentProjectsToolStripMenuItem.Name = "recentProjectsToolStripMenuItem";
            this.recentProjectsToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.recentProjectsToolStripMenuItem.Text = "Recent Projects";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(152, 6);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveProjectToolStripMenuItem.Image")));
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.saveProjectToolStripMenuItem.Text = "Save Project";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAllToolStripMenuItem.Image")));
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.saveAllToolStripMenuItem.Text = "Save All";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(152, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exitToolStripMenuItem.Image")));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectSettingsToolStripMenuItem});
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.projectToolStripMenuItem.Text = "Project";
            this.projectToolStripMenuItem.Visible = false;
            // 
            // projectSettingsToolStripMenuItem
            // 
            this.projectSettingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("projectSettingsToolStripMenuItem.Image")));
            this.projectSettingsToolStripMenuItem.Name = "projectSettingsToolStripMenuItem";
            this.projectSettingsToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.projectSettingsToolStripMenuItem.Text = "Project Settings...";
            this.projectSettingsToolStripMenuItem.Click += new System.EventHandler(this.projectSettingsToolStripMenuItem_Click);
            // 
            // scenesToolStripMenuItem
            // 
            this.scenesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSceneToolStripMenuItem,
            this.toolStripSeparator4,
            this.openSceneToolStripMenuItem,
            this.recentScenesToolStripMenuItem,
            this.toolStripSeparator5,
            this.saveSceneToolStripMenuItem,
            this.saveSceneAsToolStripMenuItem,
            this.toolStripSeparator7,
            this.scenePropertiesToolStripMenuItem});
            this.scenesToolStripMenuItem.Name = "scenesToolStripMenuItem";
            this.scenesToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.scenesToolStripMenuItem.Text = "Scenes";
            this.scenesToolStripMenuItem.Visible = false;
            // 
            // newSceneToolStripMenuItem
            // 
            this.newSceneToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newSceneToolStripMenuItem.Image")));
            this.newSceneToolStripMenuItem.Name = "newSceneToolStripMenuItem";
            this.newSceneToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.newSceneToolStripMenuItem.Text = "New Scene...";
            this.newSceneToolStripMenuItem.Click += new System.EventHandler(this.newSceneToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(167, 6);
            // 
            // openSceneToolStripMenuItem
            // 
            this.openSceneToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openSceneToolStripMenuItem.Image")));
            this.openSceneToolStripMenuItem.Name = "openSceneToolStripMenuItem";
            this.openSceneToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.openSceneToolStripMenuItem.Text = "Open Scene...";
            this.openSceneToolStripMenuItem.Click += new System.EventHandler(this.openSceneToolStripMenuItem_Click);
            // 
            // recentScenesToolStripMenuItem
            // 
            this.recentScenesToolStripMenuItem.Enabled = false;
            this.recentScenesToolStripMenuItem.Name = "recentScenesToolStripMenuItem";
            this.recentScenesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.recentScenesToolStripMenuItem.Text = "Recent Scenes";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(167, 6);
            // 
            // saveSceneToolStripMenuItem
            // 
            this.saveSceneToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveSceneToolStripMenuItem.Image")));
            this.saveSceneToolStripMenuItem.Name = "saveSceneToolStripMenuItem";
            this.saveSceneToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.saveSceneToolStripMenuItem.Text = "Save Scene";
            this.saveSceneToolStripMenuItem.Click += new System.EventHandler(this.saveSceneToolStripMenuItem_Click);
            // 
            // saveSceneAsToolStripMenuItem
            // 
            this.saveSceneAsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveSceneAsToolStripMenuItem.Image")));
            this.saveSceneAsToolStripMenuItem.Name = "saveSceneAsToolStripMenuItem";
            this.saveSceneAsToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.saveSceneAsToolStripMenuItem.Text = "Save Scene As...";
            this.saveSceneAsToolStripMenuItem.Click += new System.EventHandler(this.saveSceneAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(167, 6);
            // 
            // scenePropertiesToolStripMenuItem
            // 
            this.scenePropertiesToolStripMenuItem.Enabled = false;
            this.scenePropertiesToolStripMenuItem.Name = "scenePropertiesToolStripMenuItem";
            this.scenePropertiesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.scenePropertiesToolStripMenuItem.Text = "Scene Properties...";
            // 
            // buildToolStripMenuItem
            // 
            this.buildToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buildwindowsMenuItem,
            this.runMenuItem});
            this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
            this.buildToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.buildToolStripMenuItem.Text = "Build";
            this.buildToolStripMenuItem.Visible = false;
            // 
            // buildwindowsMenuItem
            // 
            this.buildwindowsMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("buildwindowsMenuItem.Image")));
            this.buildwindowsMenuItem.Name = "buildwindowsMenuItem";
            this.buildwindowsMenuItem.Size = new System.Drawing.Size(123, 22);
            this.buildwindowsMenuItem.Text = "Windows";
            this.buildwindowsMenuItem.Click += new System.EventHandler(this.windowsToolStripMenuItem_Click);
            // 
            // runMenuItem
            // 
            this.runMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("runMenuItem.Image")));
            this.runMenuItem.Name = "runMenuItem";
            this.runMenuItem.Size = new System.Drawing.Size(123, 22);
            this.runMenuItem.Text = "Run";
            this.runMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.spriteSheetGeneratorToolStripMenuItem,
            this.toolStripSeparator3,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // spriteSheetGeneratorToolStripMenuItem
            // 
            this.spriteSheetGeneratorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("spriteSheetGeneratorToolStripMenuItem.Image")));
            this.spriteSheetGeneratorToolStripMenuItem.Name = "spriteSheetGeneratorToolStripMenuItem";
            this.spriteSheetGeneratorToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.spriteSheetGeneratorToolStripMenuItem.Text = "SpriteSheet Generator";
            this.spriteSheetGeneratorToolStripMenuItem.Click += new System.EventHandler(this.spriteSheetGeneratorToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("optionsToolStripMenuItem.Image")));
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutToolStripMenuItem.Image")));
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripMain
            // 
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpenProject,
            this.toolStripButtonSaveProject,
            this.toolStripButtonSaveAll,
            this.toolStripSeparator13,
            this.toolStripButtonProjectSettings,
            this.toolStripSeparator10,
            this.toolStripButtonNewScene,
            this.toolStripButtonOpenScene,
            this.toolStripButtonSaveScene,
            this.toolStripSeparator9,
            this.toolStripButtonCut,
            this.toolStripButtonCopy,
            this.toolStripButtonPaste,
            this.toolStripButtonDelete,
            this.toolStripSeparator8,
            this.toolStripButtonZoomOut,
            this.toolStripButtonNoZoom,
            this.toolStripButtonZoomIn,
            this.toolStripSeparator11,
            this.toolStripButtonToolSelect,
            this.toolStripButtonToolTemplateBrush,
            this.toolStripButtonToolCamera,
            this.toolStripSeparator12,
            this.toolStripSplitButtonGrid});
            this.toolStripMain.Location = new System.Drawing.Point(0, 24);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(836, 25);
            this.toolStripMain.TabIndex = 1;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripButtonOpenProject
            // 
            this.toolStripButtonOpenProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpenProject.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpenProject.Image")));
            this.toolStripButtonOpenProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenProject.Name = "toolStripButtonOpenProject";
            this.toolStripButtonOpenProject.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonOpenProject.Text = "toolStripButton1";
            this.toolStripButtonOpenProject.ToolTipText = "Open Project";
            this.toolStripButtonOpenProject.Click += new System.EventHandler(this.toolStripButtonOpenProject_Click);
            // 
            // toolStripButtonSaveProject
            // 
            this.toolStripButtonSaveProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveProject.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveProject.Image")));
            this.toolStripButtonSaveProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveProject.Name = "toolStripButtonSaveProject";
            this.toolStripButtonSaveProject.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSaveProject.Text = "toolStripButton3";
            this.toolStripButtonSaveProject.ToolTipText = "Save Project";
            this.toolStripButtonSaveProject.Click += new System.EventHandler(this.toolStripButtonSaveProject_Click);
            // 
            // toolStripButtonSaveAll
            // 
            this.toolStripButtonSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveAll.Image")));
            this.toolStripButtonSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveAll.Name = "toolStripButtonSaveAll";
            this.toolStripButtonSaveAll.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSaveAll.Text = "toolStripButton3";
            this.toolStripButtonSaveAll.ToolTipText = "Save All";
            this.toolStripButtonSaveAll.Click += new System.EventHandler(this.toolStripButtonSaveAll_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonProjectSettings
            // 
            this.toolStripButtonProjectSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectSettings.Enabled = false;
            this.toolStripButtonProjectSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonProjectSettings.Image")));
            this.toolStripButtonProjectSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectSettings.Name = "toolStripButtonProjectSettings";
            this.toolStripButtonProjectSettings.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonProjectSettings.Text = "toolStripButton1";
            this.toolStripButtonProjectSettings.ToolTipText = "Project Settings";
            this.toolStripButtonProjectSettings.Click += new System.EventHandler(this.toolStripButtonProjectSettings_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonNewScene
            // 
            this.toolStripButtonNewScene.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNewScene.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNewScene.Image")));
            this.toolStripButtonNewScene.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNewScene.Name = "toolStripButtonNewScene";
            this.toolStripButtonNewScene.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNewScene.Text = "toolStripButton1";
            this.toolStripButtonNewScene.ToolTipText = "New Scene";
            this.toolStripButtonNewScene.Click += new System.EventHandler(this.toolStripButtonNewScene_Click);
            // 
            // toolStripButtonOpenScene
            // 
            this.toolStripButtonOpenScene.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpenScene.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpenScene.Image")));
            this.toolStripButtonOpenScene.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenScene.Name = "toolStripButtonOpenScene";
            this.toolStripButtonOpenScene.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonOpenScene.Text = "toolStripButton2";
            this.toolStripButtonOpenScene.ToolTipText = "Open Scene";
            this.toolStripButtonOpenScene.Click += new System.EventHandler(this.toolStripButtonOpenScene_Click);
            // 
            // toolStripButtonSaveScene
            // 
            this.toolStripButtonSaveScene.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveScene.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveScene.Image")));
            this.toolStripButtonSaveScene.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveScene.Name = "toolStripButtonSaveScene";
            this.toolStripButtonSaveScene.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSaveScene.Text = "toolStripButton3";
            this.toolStripButtonSaveScene.ToolTipText = "Save Scene";
            this.toolStripButtonSaveScene.Click += new System.EventHandler(this.toolStripButtonSaveScene_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonCut
            // 
            this.toolStripButtonCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCut.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCut.Image")));
            this.toolStripButtonCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCut.Name = "toolStripButtonCut";
            this.toolStripButtonCut.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonCut.Text = "toolStripButton1";
            this.toolStripButtonCut.ToolTipText = "Cut";
            this.toolStripButtonCut.Click += new System.EventHandler(this.toolStripButtonCut_Click);
            // 
            // toolStripButtonCopy
            // 
            this.toolStripButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCopy.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCopy.Image")));
            this.toolStripButtonCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCopy.Name = "toolStripButtonCopy";
            this.toolStripButtonCopy.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonCopy.Text = "toolStripButton2";
            this.toolStripButtonCopy.ToolTipText = "Copy";
            this.toolStripButtonCopy.Click += new System.EventHandler(this.toolStripButtonCopy_Click);
            // 
            // toolStripButtonPaste
            // 
            this.toolStripButtonPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPaste.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPaste.Image")));
            this.toolStripButtonPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPaste.Name = "toolStripButtonPaste";
            this.toolStripButtonPaste.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPaste.Text = "toolStripButton3";
            this.toolStripButtonPaste.ToolTipText = "Paste";
            this.toolStripButtonPaste.Click += new System.EventHandler(this.toolStripButtonPaste_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelete.Image")));
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDelete.Text = "toolStripButton1";
            this.toolStripButtonDelete.ToolTipText = "Delete";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonZoomOut
            // 
            this.toolStripButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomOut.Image")));
            this.toolStripButtonZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomOut.Name = "toolStripButtonZoomOut";
            this.toolStripButtonZoomOut.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonZoomOut.Text = "Zoom Out";
            this.toolStripButtonZoomOut.Click += new System.EventHandler(this.toolStripButtonZoomOut_Click);
            // 
            // toolStripButtonNoZoom
            // 
            this.toolStripButtonNoZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNoZoom.Enabled = false;
            this.toolStripButtonNoZoom.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNoZoom.Image")));
            this.toolStripButtonNoZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNoZoom.Name = "toolStripButtonNoZoom";
            this.toolStripButtonNoZoom.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNoZoom.Text = "Normal Zoom 1:1";
            this.toolStripButtonNoZoom.Click += new System.EventHandler(this.toolStripButtonNoZoom_Click);
            // 
            // toolStripButtonZoomIn
            // 
            this.toolStripButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomIn.Image")));
            this.toolStripButtonZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomIn.Name = "toolStripButtonZoomIn";
            this.toolStripButtonZoomIn.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonZoomIn.Text = "Zoom In";
            this.toolStripButtonZoomIn.Click += new System.EventHandler(this.toolStripButtonZoomIn_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonToolSelect
            // 
            this.toolStripButtonToolSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonToolSelect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonToolSelect.Image")));
            this.toolStripButtonToolSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonToolSelect.Name = "toolStripButtonToolSelect";
            this.toolStripButtonToolSelect.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonToolSelect.Text = "Select Tool";
            this.toolStripButtonToolSelect.Click += new System.EventHandler(this.toolStripButtonToolSelect_Click);
            // 
            // toolStripButtonToolTemplateBrush
            // 
            this.toolStripButtonToolTemplateBrush.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonToolTemplateBrush.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonToolTemplateBrush.Image")));
            this.toolStripButtonToolTemplateBrush.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonToolTemplateBrush.Name = "toolStripButtonToolTemplateBrush";
            this.toolStripButtonToolTemplateBrush.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonToolTemplateBrush.Text = "toolStripButton2";
            this.toolStripButtonToolTemplateBrush.ToolTipText = "Template Brush Tool";
            this.toolStripButtonToolTemplateBrush.Click += new System.EventHandler(this.toolStripButtonToolTemplateBrush_Click);
            // 
            // toolStripButtonToolCamera
            // 
            this.toolStripButtonToolCamera.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonToolCamera.Enabled = false;
            this.toolStripButtonToolCamera.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonToolCamera.Image")));
            this.toolStripButtonToolCamera.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonToolCamera.Name = "toolStripButtonToolCamera";
            this.toolStripButtonToolCamera.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonToolCamera.Text = "toolStripButton1";
            this.toolStripButtonToolCamera.ToolTipText = "Camera Tool";
            this.toolStripButtonToolCamera.Click += new System.EventHandler(this.toolStripButtonToolCamera_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitButtonGrid
            // 
            this.toolStripSplitButtonGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonGrid.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.grid1ToolStripMenuItem,
            this.grid2ToolStripMenuItem,
            this.grid3ToolStripMenuItem});
            this.toolStripSplitButtonGrid.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonGrid.Image")));
            this.toolStripSplitButtonGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonGrid.Name = "toolStripSplitButtonGrid";
            this.toolStripSplitButtonGrid.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButtonGrid.Text = "toolStripSplitButton1";
            this.toolStripSplitButtonGrid.ToolTipText = "Grid (click to toggle)";
            this.toolStripSplitButtonGrid.ButtonClick += new System.EventHandler(this.toolStripSplitButtonGrid_ButtonClick);
            // 
            // grid1ToolStripMenuItem
            // 
            this.grid1ToolStripMenuItem.Name = "grid1ToolStripMenuItem";
            this.grid1ToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.grid1ToolStripMenuItem.Text = "grid1";
            this.grid1ToolStripMenuItem.Click += new System.EventHandler(this.grid1ToolStripMenuItem_Click);
            // 
            // grid2ToolStripMenuItem
            // 
            this.grid2ToolStripMenuItem.Name = "grid2ToolStripMenuItem";
            this.grid2ToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.grid2ToolStripMenuItem.Text = "grid2";
            this.grid2ToolStripMenuItem.Click += new System.EventHandler(this.grid2ToolStripMenuItem_Click);
            // 
            // grid3ToolStripMenuItem
            // 
            this.grid3ToolStripMenuItem.Name = "grid3ToolStripMenuItem";
            this.grid3ToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.grid3ToolStripMenuItem.Text = "grid3";
            this.grid3ToolStripMenuItem.Click += new System.EventHandler(this.grid3ToolStripMenuItem_Click);
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelProjectName,
            this.toolStripStatusLabelScene,
            this.toolStripStatusLabelSceneMousePos});
            this.statusStripMain.Location = new System.Drawing.Point(0, 555);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(836, 22);
            this.statusStripMain.TabIndex = 2;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabelProjectName
            // 
            this.toolStripStatusLabelProjectName.Name = "toolStripStatusLabelProjectName";
            this.toolStripStatusLabelProjectName.Size = new System.Drawing.Size(87, 17);
            this.toolStripStatusLabelProjectName.Text = "Current Project";
            // 
            // toolStripStatusLabelScene
            // 
            this.toolStripStatusLabelScene.Name = "toolStripStatusLabelScene";
            this.toolStripStatusLabelScene.Size = new System.Drawing.Size(81, 17);
            this.toolStripStatusLabelScene.Text = "Current Scene";
            // 
            // toolStripStatusLabelSceneMousePos
            // 
            this.toolStripStatusLabelSceneMousePos.Name = "toolStripStatusLabelSceneMousePos";
            this.toolStripStatusLabelSceneMousePos.Size = new System.Drawing.Size(92, 17);
            this.toolStripStatusLabelSceneMousePos.Text = "sceneMousePos";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 49);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerSceneEdition);
            this.splitContainerMain.Panel1MinSize = 125;
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.sceneEditorControl);
            this.splitContainerMain.Size = new System.Drawing.Size(836, 506);
            this.splitContainerMain.SplitterDistance = 242;
            this.splitContainerMain.TabIndex = 3;
            // 
            // splitContainerSceneEdition
            // 
            this.splitContainerSceneEdition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSceneEdition.Location = new System.Drawing.Point(0, 0);
            this.splitContainerSceneEdition.Name = "splitContainerSceneEdition";
            this.splitContainerSceneEdition.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerSceneEdition.Panel1
            // 
            this.splitContainerSceneEdition.Panel1.Controls.Add(this.tabControlSceneEditionTabs);
            // 
            // splitContainerSceneEdition.Panel2
            // 
            this.splitContainerSceneEdition.Panel2.Controls.Add(this.genericComponentControl);
            this.splitContainerSceneEdition.Panel2.Controls.Add(this.labelSceneItemProperties);
            this.splitContainerSceneEdition.Panel2.Controls.Add(this.propertyGridSceneItem);
            this.splitContainerSceneEdition.Size = new System.Drawing.Size(242, 506);
            this.splitContainerSceneEdition.SplitterDistance = 253;
            this.splitContainerSceneEdition.TabIndex = 2;
            // 
            // tabControlSceneEditionTabs
            // 
            this.tabControlSceneEditionTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlSceneEditionTabs.Controls.Add(this.tabPage1);
            this.tabControlSceneEditionTabs.ImageList = this.imageListTreeview;
            this.tabControlSceneEditionTabs.Location = new System.Drawing.Point(7, 3);
            this.tabControlSceneEditionTabs.Name = "tabControlSceneEditionTabs";
            this.tabControlSceneEditionTabs.SelectedIndex = 0;
            this.tabControlSceneEditionTabs.Size = new System.Drawing.Size(229, 247);
            this.tabControlSceneEditionTabs.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.toolStripResources);
            this.tabPage1.Controls.Add(this.treeViewResources);
            this.tabPage1.ImageKey = "image_edit.png";
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(221, 220);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Scene";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // toolStripResources
            // 
            this.toolStripResources.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripResources.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButtonAddSceneInstance,
            this.toolStripSplitButtonAddLocalTemplate,
            this.toolStripSplitButtonAddGlobalTemplate,
            this.toolStripSplitButtonAddSceneComponent});
            this.toolStripResources.Location = new System.Drawing.Point(3, 3);
            this.toolStripResources.Name = "toolStripResources";
            this.toolStripResources.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripResources.Size = new System.Drawing.Size(215, 25);
            this.toolStripResources.TabIndex = 1;
            this.toolStripResources.Text = "toolStrip1";
            // 
            // toolStripSplitButtonAddSceneInstance
            // 
            this.toolStripSplitButtonAddSceneInstance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonAddSceneInstance.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonAddSceneInstance.Image")));
            this.toolStripSplitButtonAddSceneInstance.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonAddSceneInstance.Name = "toolStripSplitButtonAddSceneInstance";
            this.toolStripSplitButtonAddSceneInstance.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButtonAddSceneInstance.Text = "toolStripSplitButton1";
            this.toolStripSplitButtonAddSceneInstance.ToolTipText = "Add New SceneItem Instance";
            // 
            // toolStripSplitButtonAddLocalTemplate
            // 
            this.toolStripSplitButtonAddLocalTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonAddLocalTemplate.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonAddLocalTemplate.Image")));
            this.toolStripSplitButtonAddLocalTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonAddLocalTemplate.Name = "toolStripSplitButtonAddLocalTemplate";
            this.toolStripSplitButtonAddLocalTemplate.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButtonAddLocalTemplate.Text = "toolStripSplitButton1";
            this.toolStripSplitButtonAddLocalTemplate.ToolTipText = "Add New Local Template";
            // 
            // toolStripSplitButtonAddGlobalTemplate
            // 
            this.toolStripSplitButtonAddGlobalTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonAddGlobalTemplate.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonAddGlobalTemplate.Image")));
            this.toolStripSplitButtonAddGlobalTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonAddGlobalTemplate.Name = "toolStripSplitButtonAddGlobalTemplate";
            this.toolStripSplitButtonAddGlobalTemplate.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButtonAddGlobalTemplate.Text = "toolStripSplitButton1";
            this.toolStripSplitButtonAddGlobalTemplate.ToolTipText = "Add New Global Template";
            // 
            // toolStripSplitButtonAddSceneComponent
            // 
            this.toolStripSplitButtonAddSceneComponent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonAddSceneComponent.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonAddSceneComponent.Image")));
            this.toolStripSplitButtonAddSceneComponent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonAddSceneComponent.Name = "toolStripSplitButtonAddSceneComponent";
            this.toolStripSplitButtonAddSceneComponent.Size = new System.Drawing.Size(29, 22);
            this.toolStripSplitButtonAddSceneComponent.Text = "toolStripDropDownButton1";
            this.toolStripSplitButtonAddSceneComponent.ToolTipText = "Add Scene Component";
            // 
            // treeViewResources
            // 
            this.treeViewResources.AllowDrop = true;
            this.treeViewResources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewResources.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewResources.ContextMenuStrip = this.contextmenuTree;
            this.treeViewResources.HideSelection = false;
            this.treeViewResources.ImageIndex = 0;
            this.treeViewResources.ImageList = this.imageListTreeview;
            this.treeViewResources.Location = new System.Drawing.Point(6, 31);
            this.treeViewResources.Name = "treeViewResources";
            treeNode1.ImageKey = "image.png";
            treeNode1.Name = "Node4";
            treeNode1.SelectedImageKey = "image.png";
            treeNode1.Text = "Project Materials (Global)";
            treeNode2.ImageKey = "image.png";
            treeNode2.Name = "Node5";
            treeNode2.SelectedImageKey = "image.png";
            treeNode2.Text = "Scene Materials (Local)";
            treeNode3.ImageKey = "image.png";
            treeNode3.Name = "NodeMaterials";
            treeNode3.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode3.SelectedImageKey = "image.png";
            treeNode3.Text = "Materials";
            treeNode4.ImageKey = "book_red.png";
            treeNode4.Name = "Node2";
            treeNode4.SelectedImageKey = "book_red.png";
            treeNode4.Text = "Project Templates (Global)";
            treeNode5.ImageKey = "book.png";
            treeNode5.Name = "Node3";
            treeNode5.SelectedImageKey = "book.png";
            treeNode5.Text = "Scene Templates (Local)";
            treeNode6.ImageKey = "book.png";
            treeNode6.Name = "NodeTemplates";
            treeNode6.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode6.SelectedImageKey = "book.png";
            treeNode6.Text = "Templates";
            treeNode7.ImageKey = "plugin_blue.png";
            treeNode7.Name = "NodeSceneItems";
            treeNode7.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode7.SelectedImageKey = "plugin_blue.png";
            treeNode7.Text = "Scene Instances";
            treeNode8.ImageKey = "cog.png";
            treeNode8.Name = "NodeSceneComponents";
            treeNode8.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            treeNode8.SelectedImageKey = "cog.png";
            treeNode8.Text = "Scene Components";
            this.treeViewResources.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode6,
            treeNode7,
            treeNode8});
            this.treeViewResources.SelectedImageIndex = 0;
            this.treeViewResources.Size = new System.Drawing.Size(212, 189);
            this.treeViewResources.TabIndex = 0;
            this.treeViewResources.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewResources_NodeMouseDoubleClick);
            this.treeViewResources.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewResources_MouseClick);
            this.treeViewResources.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewResources_DragDrop);
            this.treeViewResources.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewResources_AfterSelect);
            this.treeViewResources.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewResources_DragEnter);
            this.treeViewResources.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewResources_KeyDown);
            this.treeViewResources.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewResources_ItemDrag);
            this.treeViewResources.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewResources_DragOver);
            this.treeViewResources.Click += new System.EventHandler(this.treeViewResources_Click);
            // 
            // contextmenuTree
            // 
            this.contextmenuTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAddSceneItemComponent});
            this.contextmenuTree.Name = "contextmenuTree";
            this.contextmenuTree.Size = new System.Drawing.Size(164, 26);
            // 
            // menuAddSceneItemComponent
            // 
            this.menuAddSceneItemComponent.Image = ((System.Drawing.Image)(resources.GetObject("menuAddSceneItemComponent.Image")));
            this.menuAddSceneItemComponent.Name = "menuAddSceneItemComponent";
            this.menuAddSceneItemComponent.Size = new System.Drawing.Size(163, 22);
            this.menuAddSceneItemComponent.Text = "Add Component";
            // 
            // imageListTreeview
            // 
            this.imageListTreeview.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeview.ImageStream")));
            this.imageListTreeview.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeview.Images.SetKeyName(0, "plugin.png");
            this.imageListTreeview.Images.SetKeyName(1, "weather_lightning.png");
            this.imageListTreeview.Images.SetKeyName(2, "weather_sun.png");
            this.imageListTreeview.Images.SetKeyName(3, "color_swatch.png");
            this.imageListTreeview.Images.SetKeyName(4, "sport_soccer.png");
            this.imageListTreeview.Images.SetKeyName(5, "car.png");
            this.imageListTreeview.Images.SetKeyName(6, "lock.png");
            this.imageListTreeview.Images.SetKeyName(7, "lock_red.png");
            this.imageListTreeview.Images.SetKeyName(8, "book.png");
            this.imageListTreeview.Images.SetKeyName(9, "image_edit.png");
            this.imageListTreeview.Images.SetKeyName(10, "image.png");
            this.imageListTreeview.Images.SetKeyName(11, "plugin_edit.png");
            this.imageListTreeview.Images.SetKeyName(12, "cog_edit.png");
            this.imageListTreeview.Images.SetKeyName(13, "font.png");
            this.imageListTreeview.Images.SetKeyName(14, "cog.png");
            this.imageListTreeview.Images.SetKeyName(15, "book_red.png");
            this.imageListTreeview.Images.SetKeyName(16, "user.png");
            // 
            // genericComponentControl
            // 
            this.genericComponentControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.genericComponentControl.AutoScroll = true;
            this.genericComponentControl.Location = new System.Drawing.Point(7, 18);
            this.genericComponentControl.Name = "genericComponentControl";
            this.genericComponentControl.Size = new System.Drawing.Size(229, 223);
            this.genericComponentControl.TabIndex = 2;
            this.genericComponentControl.Text = "genericComponentControl";
            // 
            // labelSceneItemProperties
            // 
            this.labelSceneItemProperties.AutoSize = true;
            this.labelSceneItemProperties.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSceneItemProperties.Location = new System.Drawing.Point(4, 2);
            this.labelSceneItemProperties.Name = "labelSceneItemProperties";
            this.labelSceneItemProperties.Size = new System.Drawing.Size(64, 13);
            this.labelSceneItemProperties.TabIndex = 1;
            this.labelSceneItemProperties.Text = "Properties";
            // 
            // propertyGridSceneItem
            // 
            this.propertyGridSceneItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridSceneItem.Location = new System.Drawing.Point(7, 18);
            this.propertyGridSceneItem.Name = "propertyGridSceneItem";
            this.propertyGridSceneItem.Size = new System.Drawing.Size(229, 223);
            this.propertyGridSceneItem.TabIndex = 1;
            this.propertyGridSceneItem.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridSceneItem_PropertyValueChanged);
            // 
            // sceneEditorControl
            // 
            this.sceneEditorControl.AllowDrop = true;
            this.sceneEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sceneEditorControl.HighlightedItem = null;
            this.sceneEditorControl.Location = new System.Drawing.Point(0, 0);
            this.sceneEditorControl.Name = "sceneEditorControl";
            this.sceneEditorControl.Preferences = null;
            this.sceneEditorControl.RealMousePos = new Microsoft.Xna.Framework.Vector2(0F, 0F);
            this.sceneEditorControl.SceneMousePos = new Microsoft.Xna.Framework.Vector2(0F, 0F);
            this.sceneEditorControl.Size = new System.Drawing.Size(590, 506);
            this.sceneEditorControl.TabIndex = 0;
            this.sceneEditorControl.Text = "sceneEditorControl1";
            this.sceneEditorControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.sceneEditorControl_DragDrop);
            this.sceneEditorControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.sceneEditorControl_DragEnter);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(185, 6);
            // 
            // MilkshakeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 577);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "MilkshakeForm";
            this.Text = "Milkshake - IceCream Editor";
            this.Load += new System.EventHandler(this.MilkshakeForm_Load);
            this.SizeChanged += new System.EventHandler(this.MilkshakeForm_SizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MilkshakeForm_FormClosing);
            this.LocationChanged += new System.EventHandler(this.MilkshakeForm_LocationChanged);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerSceneEdition.Panel1.ResumeLayout(false);
            this.splitContainerSceneEdition.Panel2.ResumeLayout(false);
            this.splitContainerSceneEdition.Panel2.PerformLayout();
            this.splitContainerSceneEdition.ResumeLayout(false);
            this.tabControlSceneEditionTabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.toolStripResources.ResumeLayout(false);
            this.toolStripResources.PerformLayout();
            this.contextmenuTree.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scenesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenProject;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveAll;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentProjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem recentScenesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem scenePropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonNewScene;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenScene;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveProject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton toolStripButtonCut;
        private System.Windows.Forms.ToolStripButton toolStripButtonCopy;
        private System.Windows.Forms.ToolStripButton toolStripButtonPaste;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveScene;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelProjectName;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private Milkshake.GraphicsDeviceControls.SceneEditorControl sceneEditorControl;
        private System.Windows.Forms.ToolStripMenuItem saveSceneAsToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControlSceneEditionTabs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainerSceneEdition;
        private System.Windows.Forms.PropertyGrid propertyGridSceneItem;
        private System.Windows.Forms.Label labelSceneItemProperties;
        private System.Windows.Forms.ToolStrip toolStripResources;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonAddSceneInstance;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomOut;
        private System.Windows.Forms.ToolStripButton toolStripButtonNoZoom;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomIn;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSceneMousePos;
        private System.Windows.Forms.ToolStripDropDownButton toolStripSplitButtonAddSceneComponent;
        private System.Windows.Forms.ContextMenuStrip contextmenuTree;
        private System.Windows.Forms.ToolStripMenuItem menuAddSceneItemComponent;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelScene;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private Milkshake.Editors.Components.GenericComponentControl genericComponentControl;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonAddLocalTemplate;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonAddGlobalTemplate;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        public System.Windows.Forms.ImageList imageListTreeview;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildwindowsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectSettings;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonGrid;
        private System.Windows.Forms.ToolStripMenuItem grid1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grid2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grid3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonToolSelect;
        private System.Windows.Forms.ToolStripButton toolStripButtonToolTemplateBrush;
        private System.Windows.Forms.ToolStripButton toolStripButtonToolCamera;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        internal System.Windows.Forms.TreeView treeViewResources;
        private System.Windows.Forms.ToolStripMenuItem spriteSheetGeneratorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}

