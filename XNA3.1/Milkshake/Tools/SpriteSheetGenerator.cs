using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Windows.Forms;
using IceCream.SceneItems;
using Microsoft.Xna.Framework;
using GdiColor = System.Drawing.Color;
using XnaColor = Microsoft.Xna.Framework.Graphics.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using IceCream.Components;
using IceCream.Attributes;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Milkshake.Tools
{
    public partial class SpriteSheetGenerator : Form
    {
        public const int MAX_SAVED_ENTRIES = 50;
        public List<String> LastOutputDir { get; set; }
        public List<String> LastInputDir { get; set; }
        public List<String> LastNames { get; set; }

        public List<SpriteInfo> Sprites
        {
            get;
            set;
        }

        public GdiColor BaseTextureColor
        {
            get;
            set;
        }

        public SpriteSheetGenerator()
        {
            InitializeComponent();
            this.Sprites = new List<SpriteInfo>();
        }

        private List<String> ConvertStringToList(String input)
        {
            if (input == null)
            {
                return new List<String>();
            }
            else
            {
                return new List<String>(input.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        private String ConvertListToString(List<String> input)
        {
            String output = "";
            for (int i = 0; i < input.Count; i++)
            {
                if (i > 0)
                {
                    output += ",";
                }
                output += input[i];
            }
            return output;
        }

        private void LoadCombobox(ComboBox combo, List<String> list)
        {
            combo.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                combo.Items.Add(list[i]);
            }
            if (combo.Items.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
        }

        private void CheckForNewEntry(String entry, List<String> list)
        {
            if (list.Contains(entry) == true)
            {
                list.Remove(entry);
            }
            list.Insert(0, entry);
            if (list.Count > MAX_SAVED_ENTRIES)
            {
                list.RemoveRange(MAX_SAVED_ENTRIES - 1, list.Count - 1);
            }
        }
        
        private void SpriteSheetGenerator_Load(object sender, EventArgs e)
        {
            Array fileFormats = Enum.GetValues(typeof(ImageFileFormat));
            foreach (object format in fileFormats)
            {
                comboBoxFileFormat.Items.Add(format.ToString());
            }
            comboBoxFileFormat.SelectedItem = "Png";            
            MilkshakePreferences prefs = MilkshakeForm.Instance.Preferences;            
            this.LastInputDir = ConvertStringToList(prefs.ToolSpritesheetLastInputFolder);
            this.LastOutputDir = ConvertStringToList(prefs.ToolSpritesheetLastOutputFolder);
            this.LastNames = ConvertStringToList(prefs.ToolSpritesheetLastOutputName);
            LoadCombobox(comboBoxInputDir, this.LastInputDir);
            LoadCombobox(comboBoxOutputDir, this.LastOutputDir);
            LoadCombobox(comboBoxName, this.LastNames);            
            numericUpDownSafeBorderSize.Value = (decimal)prefs.ToolSpritesheetLastSafeBorderSize;
            checkBoxAlphaCorrection.Checked = prefs.ToolSpritesheetLastCorrectTransparencyColor;
            checkBoxOverrideBaseColor.Checked = prefs.ToolSpritesheetLastReplaceBaseColor;
            checkBoxPowerOf2.Checked = prefs.ToolSpritesheetLastPadTexturePowerOfTwo;
            XnaColor backColor = prefs.ToolSpritesheetLastBaseColor;
            this.BaseTextureColor = GdiColor.FromArgb(backColor.R, backColor.G, backColor.B);
            pictureBoxTint.BackColor = this.BaseTextureColor;
            CheckValidation();
        }

        private void CheckValidation()
        {
            bool enableState = false;
            bool d = Directory.Exists(comboBoxInputDir.Text);
            if (Directory.Exists(comboBoxInputDir.Text) && Directory.Exists(comboBoxOutputDir.Text)
                && String.IsNullOrEmpty(comboBoxName.Text) == false)
            {
                enableState = true;
            }
            buttonGenerate.Enabled = enableState;
        }

        #region Generation

        public void StartSpriteSheetGeneration()
        {
            MilkshakePreferences prefs = MilkshakeForm.Instance.Preferences;
            CheckForNewEntry(comboBoxInputDir.Text, this.LastInputDir);
            CheckForNewEntry(comboBoxOutputDir.Text, this.LastOutputDir);
            CheckForNewEntry(comboBoxName.Text, this.LastNames);
            prefs.ToolSpritesheetLastInputFolder = ConvertListToString(this.LastInputDir);
            prefs.ToolSpritesheetLastOutputFolder = ConvertListToString(this.LastOutputDir);
            prefs.ToolSpritesheetLastOutputName = ConvertListToString(this.LastNames);
            prefs.ToolSpritesheetLastSafeBorderSize = (int)numericUpDownSafeBorderSize.Value;
            prefs.ToolSpritesheetLastCorrectTransparencyColor = checkBoxAlphaCorrection.Checked;
            prefs.ToolSpritesheetLastReplaceBaseColor = checkBoxOverrideBaseColor.Checked;
            prefs.ToolSpritesheetLastPadTexturePowerOfTwo = checkBoxPowerOf2.Checked;
            prefs.ToolSpritesheetLastBaseColor = new XnaColor(this.BaseTextureColor.R,
                    this.BaseTextureColor.G, this.BaseTextureColor.B);

            this.Sprites.Clear();
            String path = comboBoxInputDir.Text;
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] rgFiles = di.GetFiles("*.*");
            foreach (FileInfo fi in rgFiles)
            {
                String fileExt = fi.Extension.ToLower();
                if (fileExt == ".png" || fileExt == ".jpg"
                    || fileExt == ".dds" || fileExt == ".bmp" || fileExt == ".tga")
                {
                    Console.WriteLine(Sprites.Count + "] Packing sprite " + fi.Name);
                    SpriteInfo newInfo = new SpriteInfo(Path.GetFileNameWithoutExtension(fi.Name));
                    newInfo.Texture2D = Texture2D.FromFile(IceCream.Drawing.DrawingManager.GraphicsDevice, fi.FullName);
                    newInfo.PixelData = new uint[newInfo.Texture2D.Width * newInfo.Texture2D.Height];
                    newInfo.Texture2D.GetData<uint>(newInfo.PixelData);
                    Sprites.Add(newInfo);
                }
            }
            PackSprites((int)numericUpDownSafeBorderSize.Value, comboBoxOutputDir.Text, comboBoxName.Text);
        }


        /// <summary>
        /// Comparison function for sorting sprites by size.
        /// </summary>
        public int CompareSpriteSizes(SpriteInfo a, SpriteInfo b)
        {
            int aSize = a.Height * 1024 + a.Width;
            int bSize = b.Height * 1024 + b.Width;
            return bSize.CompareTo(aSize);
        }

        /// <summary>
        /// Comparison function for sorting sprites by their original indices.
        /// </summary>
        static int CompareSpriteIndices(SpriteInfo a, SpriteInfo b)
        {
            return a.Index.CompareTo(b.Index);
        }

        private void PackSprites(int safeBorderSize, String outputPath, String sheetName)
        {
            for (int i = 0; i < this.Sprites.Count; i++)
            {
                SpriteInfo sprite = this.Sprites[i];
                sprite.Width = sprite.Texture2D.Width + safeBorderSize * 2;
                sprite.Height = sprite.Texture2D.Height + safeBorderSize * 2;
                sprite.Index = i;
            }
            this.Sprites.Sort(CompareSpriteSizes);

            // Work out how big the output bitmap should be.
            int outputWidth = GuessOutputWidth(this.Sprites);
            int outputHeight = 0;
            int totalSpriteSize = 0;
            // Choose positions for each sprite, one at a time.
            for (int i = 0; i < this.Sprites.Count; i++)
            {
                PositionSprite(this.Sprites, i, outputWidth);

                outputHeight = Math.Max(outputHeight, this.Sprites[i].Y + this.Sprites[i].Height);

                totalSpriteSize += this.Sprites[i].Width * this.Sprites[i].Height;
            }

            // Sort the sprites back into index order.
            this.Sprites.Sort(CompareSpriteIndices);

            Console.WriteLine("Packed {0} sprites into a {1}x{2} sheet, {3}% efficiency",
                this.Sprites.Count, outputWidth, outputHeight, totalSpriteSize * 100 / outputWidth / outputHeight);

            GenerateSpriteSheet(outputWidth, outputHeight, safeBorderSize, outputPath, sheetName);
        }

        public int GetNextPowerOf2(int value)
        {
            int[] powers = { 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 };
            for (int i = 0; i < powers.Length; i++)
            {
                if (i == powers.Length - 1 || powers[i] >= value)
                {
                    return powers[i];
                }
            }
            return powers[powers.Length - 1];
        }

        public void GenerateSpriteSheet(int width, int height, int safeBorderSize, String outputPath, String sheetName)
        {            
            if (checkBoxPowerOf2.Checked == true)
            {
                // if not a power of 2 already
                if ((width & (width - 1)) != 0)
                {
                    width = GetNextPowerOf2(width);
                }
                if ((height & (height - 1)) != 0)
                {
                    height = GetNextPowerOf2(height);
                }
            }
            bool overrideTransColor = checkBoxOverrideBaseColor.Checked;
            XnaColor baseColor = XnaColor.Black;
            if (overrideTransColor == true)
            {
                baseColor = new XnaColor(this.BaseTextureColor.R,
                    this.BaseTextureColor.G, this.BaseTextureColor.B, 0);
            }
            uint[] destinationData = new uint[width * height];
            for (int i = 0; i < destinationData.Length; i++)
            {
                destinationData[i] = baseColor.PackedValue;
            }            
            Texture2D spriteSheet = new Texture2D(IceCream.Drawing.DrawingManager.GraphicsDevice,
                width, height);
            foreach (SpriteInfo sprite in this.Sprites)
            {
                Texture2D source = sprite.Texture2D;
                int x = sprite.X;
                int y = sprite.Y;

                int w = source.Width;
                int h = source.Height;

                int b = safeBorderSize;

                int sourceSize = sprite.Texture2D.Width;
                int destSize = width;                
                
                // Copy the main sprite data to the output sheet.
                sprite.CopyPixels(sprite.PixelData, sourceSize, new Rectangle(0, 0, w, h),
                    ref destinationData, destSize, new Point(x + b, y + b),
                    baseColor.PackedValue, overrideTransColor);
                
                // Copy a border strip from each edge of the sprite, creating
                // a padding area to avoid filtering problems if the
                // sprite is scaled or rotated.
                for (int i = 0; i < safeBorderSize; i++)
                {
                    sprite.CopyPixels(sprite.PixelData, sourceSize, new Rectangle(0, 0, 1, h),
                                       ref destinationData, destSize, new Point(x + i, y + b),
                                       baseColor.PackedValue, overrideTransColor);

                    sprite.CopyPixels(sprite.PixelData, sourceSize, new Rectangle(w - 1, 0, 1, h),
                                       ref destinationData, destSize,
                                       new Point(x + w + i + b, y + b), baseColor.PackedValue, overrideTransColor);

                    sprite.CopyPixels(sprite.PixelData, sourceSize, new Rectangle(0, 0, w, 1),
                                       ref destinationData, destSize,
                                       new Point(x + b, y + i), baseColor.PackedValue, overrideTransColor);

                    sprite.CopyPixels(sprite.PixelData, sourceSize, new Rectangle(0, h - 1, w, 1),
                                       ref destinationData, destSize,
                                       new Point(x + b, y + h + i + b), baseColor.PackedValue, overrideTransColor);
                    
                    // Copy a single pixel from each corner of the sprite,
                    // filling in the corners of the padding area.                    
                    for (int j = 0; j < b; j++)
                    {
                        sprite.CopyPixels(sprite.PixelData, sourceSize, new Rectangle(0, 0, 1, 1),
                                           ref destinationData, destSize, new Point(x + j, y + i), baseColor.PackedValue, 
                                           overrideTransColor);
                        sprite.CopyPixels(sprite.PixelData, sourceSize, new Rectangle(0, 0, 1, 1),
                                           ref destinationData, destSize, new Point(x + i, y + j), baseColor.PackedValue,
                                           overrideTransColor);

                        sprite.CopyPixels(sprite.PixelData, sourceSize, new Rectangle(w - 1, 0, 1, 1),
                                       ref destinationData, destSize, new Point(x + w + b + i, y + j),
                                       baseColor.PackedValue, overrideTransColor);
                        sprite.CopyPixels(sprite.PixelData, sourceSize, new Rectangle(w - 1, 0, 1, 1),
                                       ref destinationData, destSize, new Point(x + w + b + j, y + i), baseColor.PackedValue,
                                       overrideTransColor);

                        sprite.CopyPixels(sprite.PixelData, sourceSize, new Rectangle(0, h - 1, 1, 1),
                                       ref destinationData, destSize, new Point(x + i, y + h + b + j), baseColor.PackedValue, 
                                       overrideTransColor);
                        sprite.CopyPixels(sprite.PixelData, sourceSize, new Rectangle(0, h - 1, 1, 1),
                                       ref destinationData, destSize, new Point(x + j, y + h + b + i), baseColor.PackedValue,
                                       overrideTransColor);

                        sprite.CopyPixels(sprite.PixelData, sourceSize, new Rectangle(w - 1, h - 1, 1, 1),
                                           ref destinationData, destSize, new Point(x + w + b + i, y + h + b + j),
                                           baseColor.PackedValue, overrideTransColor);
                        sprite.CopyPixels(sprite.PixelData, sourceSize, new Rectangle(w - 1, h - 1, 1, 1),
                                           ref destinationData, destSize, new Point(x + w + b + j, y + h + b + i), 
                                           baseColor.PackedValue, overrideTransColor);
                    }                 
                }
                sprite.Area = new Rectangle(x + safeBorderSize, y + safeBorderSize, 
                    sprite.Texture2D.Width, sprite.Texture2D.Height);                 
                 
            }
            if (checkBoxAlphaCorrection.Checked == true)
            {
                CorrectAlphaBorders(destinationData, width, height, 2);
            }
            spriteSheet.SetData<uint>(destinationData);
            ImageFileFormat format = (ImageFileFormat)comboBoxFileFormat.SelectedIndex;
            String outputFilename = Path.Combine(outputPath, sheetName + "." + format.ToString().ToLowerInvariant());
            spriteSheet.Save(outputFilename, format);
            ExportXML(Path.Combine(outputPath, sheetName + ".xml"));
        }

        public void ExportXML(String filename)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode _rootNode = doc.CreateElement("Areas");
            doc.AppendChild(_rootNode);
            foreach (SpriteInfo sprite in this.Sprites)
            {
                XmlNode _rectNode = doc.CreateElement("Area");
                _rectNode.Attributes.Append(doc.CreateAttribute("Key")).InnerText = sprite.Name;
                _rectNode.Attributes.Append(doc.CreateAttribute("X")).InnerText = sprite.Area.X.ToString(CultureInfo.InvariantCulture);
                _rectNode.Attributes.Append(doc.CreateAttribute("Y")).InnerText = sprite.Area.Y.ToString(CultureInfo.InvariantCulture);
                _rectNode.Attributes.Append(doc.CreateAttribute("W")).InnerText = sprite.Area.Width.ToString(CultureInfo.InvariantCulture);
                _rectNode.Attributes.Append(doc.CreateAttribute("H")).InnerText = sprite.Area.Height.ToString(CultureInfo.InvariantCulture);
                _rootNode.AppendChild(_rectNode);
            }
            doc.Save(filename);
            doc = null;
        }


        /// <summary>
        /// Works out where to position a single sprite.
        /// </summary>
        public void PositionSprite(List<SpriteInfo> sprites,
                                   int index, int outputWidth)
        {
            int x = 0;
            int y = 0;

            while (true)
            {
                // Is this position free for us to use?
                int intersects = FindIntersectingSprite(sprites, index, x, y);

                if (intersects < 0)
                {
                    sprites[index].X = x;
                    sprites[index].Y = y;

                    return;
                }

                // Skip past the existing sprite that we collided with.
                x = sprites[intersects].X + sprites[intersects].Width;

                // If we ran out of room to move to the right,
                // try the next line down instead.
                if (x + sprites[index].Width > outputWidth)
                {
                    x = 0;
                    y++;
                }
            }
        }

        /// <summary>
        /// Checks if a proposed sprite position collides with anything
        /// that we already arranged.
        /// </summary>
        public int FindIntersectingSprite(List<SpriteInfo> sprites,
                                          int index, int x, int y)
        {
            int w = sprites[index].Width;
            int h = sprites[index].Height;
            for (int i = 0; i < index; i++)
            {
                if (sprites[i].X >= x + w)
                    continue;

                if (sprites[i].X + sprites[i].Width <= x)
                    continue;

                if (sprites[i].Y >= y + h)
                    continue;

                if (sprites[i].Y + sprites[i].Height <= y)
                    continue;

                return i;
            }
            return -1;
        }


        /// <summary>
        /// Heuristic guesses what might be a good output width for a list of sprites.
        /// </summary>
        public int GuessOutputWidth(List<SpriteInfo> sprites)
        {
            // Gather the widths of all our sprites into a temporary list.
            List<int> widths = new List<int>();

            foreach (SpriteInfo sprite in sprites)
            {
                widths.Add(sprite.Width);
            }

            // Sort the widths into ascending order.
            widths.Sort();

            // Extract the maximum and median widths.
            int maxWidth = widths[widths.Count - 1];
            int medianWidth = widths[widths.Count / 2];

            // Heuristic assumes an NxN grid of median sized sprites.
            int width = medianWidth * (int)Math.Round(Math.Sqrt(sprites.Count));

            // Make sure we never choose anything smaller than our largest sprite.
            return Math.Max(width, maxWidth);
        }

        private void CheckAroundPixel(uint[] pixels, bool[] mask, uint color, Point pixRef, int width, int height, int step, int maxStep)
        {
            if (step <= maxStep)
            {
                step++;
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x != 0 || y != 0)
                        {
                            Point pix = new Point(pixRef.X + x, pixRef.Y + y);
                            if (pix.X >= 0 && pix.Y >= 0 && pix.X < width && pix.Y < height)
                            {
                                uint pos = (uint)(pix.Y * width + pix.X);
                                if (mask[pos] == false)
                                {                                    
                                    pixels[pos] = color;
                                    CheckAroundPixel(pixels, mask, color, pix, width, height, step, maxStep);
                                }                                
                            }
                        }
                    }
                }
            }
        }

        public void CorrectAlphaBorders(uint[] pixels, int width, int height, int maxSteps)
        {
            bool[] pixelsMask = new bool[pixels.Length];
            for (int i = 0; i < pixels.Length; i++)
            {
                // if Alpha > 0
                if (pixels[i] >= 16777216)
                {
                    pixelsMask[i] = true;
                }
            }
            for (int i = 0; i < pixelsMask.Length; i++)
            {
                // if alpha, check each borders to fill it with a copy
                if (pixelsMask[i] == true)
                {
                    double widthSize = (double)width;
                    Point pixRef = new Point((int)(i % widthSize), (int)Math.Floor(i / widthSize));
                    uint pixRefPos = (uint)(pixRef.Y * width + pixRef.X);
                    uint colorRef = pixels[pixRefPos];
                    XnaColor referenceColor = XnaColor.Black;
                    referenceColor.PackedValue = colorRef;
                    // remove alpha value
                    referenceColor.A = 0;
                    colorRef = referenceColor.PackedValue;
                    CheckAroundPixel(pixels, pixelsMask, colorRef, pixRef, width, height, 1, maxSteps);                               
                }
            }
        }

        #endregion

        #region Events

        private void OpenTintSelectionDialog()
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.FullOpen = true;
            colorDialog.Color = this.BaseTextureColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBoxTint.BackColor = colorDialog.Color;
                this.BaseTextureColor = colorDialog.Color;                
            }
        }

        private void buttonBrowseInput_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                comboBoxInputDir.Text = folderDialog.SelectedPath;
            }
        }

        private void buttonBrowseOutput_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                comboBoxOutputDir.Text = folderDialog.SelectedPath;
            }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                StartSpriteSheetGeneration();
                MilkshakeForm.ShowInfoMessage("Spritesheet \"" + comboBoxName.Text + "\" generated successfully");
            }
            catch (Exception ex)
            {
                MilkshakeForm.ShowErrorMessage("Could not generate spritesheet: " + ex.Message);
            }
        }

        #endregion

        private void textBoxInputDir_TextChanged(object sender, EventArgs e)
        {
            CheckValidation();
        }

        private void textBoxOutputDir_TextChanged(object sender, EventArgs e)
        {
            CheckValidation();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            CheckValidation();
        }

        private void pictureBoxTint_Click(object sender, EventArgs e)
        {
            OpenTintSelectionDialog();
        }

        private void comboBoxInputDir_SelectedValueChanged(object sender, EventArgs e)
        {
            CheckValidation();
        }
    }
}
