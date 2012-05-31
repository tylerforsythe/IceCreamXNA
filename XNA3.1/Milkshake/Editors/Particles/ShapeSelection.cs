using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using IceCream;
using IceCream.Drawing;
using IceCream.SceneItems;
using IceCream.SceneItems.ParticlesClasses;

namespace Milkshake.Editors.Particles
{
    public partial class ShapeSelection : Form
    {
        private EmitterShapeType shapeType;
        private int shapeWidth;
        private int shapeHeight;
        private int xOffset;
        private int yOffset;
        private String texturePath;
        private bool useFilled;
        private bool useLeft;
        private bool useRight;
        private bool useTop;
        private bool useBottom;

        public bool UseFilled
        {
            get { return useFilled; }
        }
        public bool UseLeft
        {
            get { return useLeft; }
        }
        public bool UseRight
        {
            get { return useRight; }
        }
        public bool UseTop
        {
            get { return useTop; }
        }
        public bool UseBottom
        {
            get { return useBottom; }
        }
        public EmitterShapeType ShapeType
        {
            get { return shapeType; }
            set 
            { 
                shapeType = value;
                LoadProperties();
            }
        }
        public Vector2 ShapeSize
        {
            get { return new Vector2(shapeWidth, shapeHeight); }
            set
            {
                shapeWidth = (int)value.X;
                shapeHeight = (int)value.Y;
                LoadProperties();
            }
        }
        public Vector2 ShapeOffset
        {
            get { return new Vector2(xOffset, yOffset); }
            set
            {
                xOffset = (int)value.X;
                yOffset = (int)value.Y;
                LoadProperties();
            }
        }
        public String TexturePath
        {
            get { return texturePath; }
            set 
            { 
                texturePath = value;
                LoadProperties();
            }
        }

        public ShapeSelection()
        {
            InitializeComponent();
            shapeType = EmitterShapeType.Point;
            shapeWidth = 100;
            shapeHeight = 100;
            for (int i = 0; i < Enum.GetValues(typeof(EmitterShapeType)).Length; i++)
            {
                comboBoxShapes.Items.Add(((EmitterShapeType)i).ToString());
            }
        }

        private void LoadProperties()
        {
            comboBoxShapes.SelectedIndex = (int)shapeType;
            numericUpDownWidth.Value = (decimal)shapeWidth;
            numericUpDownHeight.Value = (decimal)shapeHeight;
            if (shapeType == EmitterShapeType.TextureMask)
            {
                panelMask.Visible = true;
                panelSize.Visible = false;
                if (texturePath == null || texturePath == "")
                {
                    buttonOk.Enabled = false;
                }
                else
                {
                    buttonOk.Enabled = true;
                }
                textBoxImagePath.Text = texturePath;
            }
            else if (shapeType == EmitterShapeType.Point)
            {
                panelMask.Visible = false;
                panelSize.Visible = false;
                buttonOk.Enabled = true;
            }
            else
            {
                panelMask.Visible = false;
                panelSize.Visible = true;
                buttonOk.Enabled = true;
                numericUpDownWidth.Value = (decimal)shapeWidth;
                numericUpDownHeight.Value = (decimal)shapeHeight;
            }
        }

        private void ShapeSelection_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Loading Shape Dialog");            
            LoadProperties();
        }

        private void comboBoxShapes_SelectedIndexChanged(object sender, EventArgs e)
        {
            shapeType = (EmitterShapeType)comboBoxShapes.SelectedIndex;
            LoadProperties();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Load image";
            fileDialog.Filter = "PNG Files (*.png)|*.png|" +
                                "TGA Files (*.tga)|*.tga|" +
                                "BMP Files (*.bmp)|*.bmp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxImagePath.Text = fileDialog.FileName;                
                buttonOk.Enabled = true;
            }                    
        }

        private void numericUpDownHeight_ValueChanged(object sender, EventArgs e)
        {
            shapeHeight = (int)numericUpDownHeight.Value;
        }

        private void numericUpDownWidth_ValueChanged(object sender, EventArgs e)
        {
            shapeWidth = (int)numericUpDownWidth.Value;
        }

        private void textBoxImagePath_TextChanged(object sender, EventArgs e)
        {
            texturePath = textBoxImagePath.Text;
            if (texturePath == null || texturePath == "")
            {
                buttonOk.Enabled = false;
            }
            else
            {
                buttonOk.Enabled = true;
            }
        }

        private void radioButtonFilled_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxLeft.Enabled = false;
            checkBoxRight.Enabled = false;
            checkBoxTop.Enabled = false;
            checkBoxBottom.Enabled = false;
        }

        private void radioButtonOutlined_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxLeft.Enabled = true;
            checkBoxRight.Enabled = true;
            checkBoxTop.Enabled = true;
            checkBoxBottom.Enabled = true;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            useFilled = radioButtonFilled.Checked;
            useLeft = checkBoxLeft.Checked;
            useRight = checkBoxRight.Checked;
            useTop = checkBoxTop.Checked;
            useBottom = checkBoxBottom.Checked;
        }
    }
}