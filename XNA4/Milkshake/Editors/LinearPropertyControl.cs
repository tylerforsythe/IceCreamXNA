using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Rectangle = System.Drawing.Rectangle;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using IceCream;
using IceCream.Drawing;
using IceCream.SceneItems;
using IceCream.SceneItems.ParticlesClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Milkshake.Editors
{
    class LinearPropertyControl : Control
    {
        private LinearProperty selectedLinearProperty;
        private int zoneWidth = 200;
        private int zoneHeigth = 200;
        private int selectedPoint = -1;
        private float leftOffset = 40;
        private float topOffset = 24;
        private float minDistanceBetweenPoints = 0.025f;
        private System.Windows.Forms.TextBox textBoxMax;
        private System.Windows.Forms.TextBox textBoxMin;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override Size DefaultSize
        {
            get { return new Size(256, 256); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal LinearProperty SelectedLinearProperty
        {
            get { return selectedLinearProperty; }
            set 
            { 
                selectedLinearProperty = value;
                textBoxMax.Text = selectedLinearProperty.UpperBound
                        .ToString(System.Globalization.CultureInfo.InvariantCulture);
                textBoxMin.Text = selectedLinearProperty.LowerBound
                        .ToString(System.Globalization.CultureInfo.InvariantCulture);
                ApplyNewRangeOfValues();
            }
        }

        public LinearPropertyControl()
        {
            // Activates double buffering
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true); 
            selectedLinearProperty = new LinearProperty(5, "Properties", 0, 10);
            selectedLinearProperty.Values.Add(new Vector2(0.5f, 8));
            selectedLinearProperty.Values.Add(new Vector2(0.7f, 9));
            selectedLinearProperty.Values.Add(new Vector2(1f, 0));

            this.textBoxMax = new TextBox();           
            this.textBoxMax.Location = new System.Drawing.Point(4, 23);
            this.textBoxMax.Name = "textBoxMax";
            this.textBoxMax.Size = new System.Drawing.Size(32, 20);
            this.textBoxMax.TabIndex = 1;
            this.textBoxMax.Text = selectedLinearProperty.UpperBound.ToString();
            this.textBoxMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;            
            this.textBoxMax.Validated += new System.EventHandler(this.textBoxMax_Validated);
            this.textBoxMax.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            this.Controls.Add(textBoxMax);

            this.textBoxMin = new TextBox();
            this.textBoxMin.Location = new System.Drawing.Point(4, 205);
            this.textBoxMin.Name = "textBoxMin";
            this.textBoxMin.Size = new System.Drawing.Size(32, 20);
            this.textBoxMin.TabIndex = 2;
            this.textBoxMin.Text = selectedLinearProperty.LowerBound.ToString();
            this.textBoxMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxMin.Validated += new System.EventHandler(this.textBoxMin_Validated);
            this.textBoxMin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            this.Controls.Add(textBoxMin);
        }

        private void textBoxMax_Validated(object sender, EventArgs e)
        {
            try
            {
                int value = int.Parse(textBoxMax.Text, System.Globalization.CultureInfo.InvariantCulture);
                if (value <= selectedLinearProperty.LowerBound)
                {
                    throw new Exception("The upper bound must be higher than the lower bound");
                }
                SelectedLinearProperty.UpperBound = value;
                ApplyNewRangeOfValues();
            }
            catch (Exception ex)
            {
                textBoxMax.Text = selectedLinearProperty.UpperBound
                        .ToString(System.Globalization.CultureInfo.InvariantCulture);
                MilkshakeForm.ShowErrorMessage(ex.Message);
            }
        }

        private void textBoxMin_Validated(object sender, EventArgs e)
        {
            try
            {
                int value = int.Parse(textBoxMin.Text, System.Globalization.CultureInfo.InvariantCulture);
                if (value >= selectedLinearProperty.UpperBound)
                {
                    throw new Exception("The lower bound must be lower than the upper bound");
                }
                SelectedLinearProperty.LowerBound = value;
                ApplyNewRangeOfValues();
            }
            catch (Exception ex)
            {
                textBoxMin.Text = selectedLinearProperty.LowerBound
                        .ToString(System.Globalization.CultureInfo.InvariantCulture);
                MilkshakeForm.ShowErrorMessage(ex.Message);
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {                
                this.Focus();
            }
        }

        private void ApplyNewRangeOfValues()
        {
            for (int i = 0; i < selectedLinearProperty.Values.Count; i++)
            {
                float value = selectedLinearProperty.Values[i].Y;
                // clamp the value to the new boundaries
                selectedLinearProperty.Values[i] = new Vector2(selectedLinearProperty.Values[i].X,
                    MathHelper.Clamp(value,
                    (float)selectedLinearProperty.LowerBound,
                    (float)selectedLinearProperty.UpperBound));
            }
            this.Invalidate();
        }

        private Vector2 convertPixelCoordinatesToLinearValues(PointF coordinates)
        {
            Vector2 value = new Vector2(coordinates.X - leftOffset, coordinates.Y - topOffset);
            float pointFactor = zoneHeigth / (float)(selectedLinearProperty.UpperBound - selectedLinearProperty.LowerBound);
            return new Vector2(value.X / zoneWidth,
                (zoneHeigth - value.Y) / pointFactor + selectedLinearProperty.LowerBound);           
        }

        private PointF convertLinearValuesToPixelCoordinates(Vector2 values)
        {
            float value = values.Y;
            float time = MathHelper.Clamp(values.X, 0, 1);
            float pointFactor = 1 - ((value - selectedLinearProperty.LowerBound) / (selectedLinearProperty.UpperBound - selectedLinearProperty.LowerBound));
            PointF point = new PointF(zoneWidth * time, zoneHeigth * pointFactor);            
            return new PointF(leftOffset + point.X, topOffset + point.Y);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            this.Focus();
            PointF mousePos = e.Location;
            // if it's a click inside the zone
            if (mousePos.X >= leftOffset && mousePos.Y >= topOffset &&
                mousePos.X <= leftOffset + zoneWidth && mousePos.Y <= topOffset + zoneHeigth)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (selectedPoint > 0)
                    {
                        selectedLinearProperty.Values.RemoveAt(selectedPoint);
                        this.Invalidate();
                    }
                }
                else if (e.Button == MouseButtons.Left && selectedPoint == -1)
                {
                    int newIndex = selectedLinearProperty.Values.Count;
                    Vector2 newValue = convertPixelCoordinatesToLinearValues(mousePos);
                    if (selectedLinearProperty.Values.Count > 1)
                    {
                        for (int i = selectedLinearProperty.Values.Count - 1; i >= 0; i--)
                        {
                            if (selectedLinearProperty.Values[i].X <= newValue.X)
                            {
                                newIndex = i + 1;
                                break;
                            }
                        }
                    }
                    float leftBound = 0;
                    float rightBound = 1;

                    if (newIndex > 0)
                    {
                        leftBound = selectedLinearProperty.Values[newIndex - 1].X;
                    }
                    if (newIndex < selectedLinearProperty.Values.Count - 1)
                    {
                        rightBound = selectedLinearProperty.Values[newIndex].X;
                    }
                    if (newValue.X - leftBound >= minDistanceBetweenPoints && rightBound - newValue.X >= minDistanceBetweenPoints)
                    {                        
                        selectedLinearProperty.Values.Insert(newIndex, newValue);
                        this.Invalidate();
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int oldSelectedPoint = selectedPoint;
            PointF mousePos = e.Location;
            // if the mouse left button is pushed
            if (e.Button == MouseButtons.Left && selectedPoint != -1)
            {
                Vector2 newValue = convertPixelCoordinatesToLinearValues(mousePos);
                // the first point is always at time 0
                if (selectedPoint == 0)
                {
                    newValue.X = 0;
                }
                // clamp the value horizontally
                float leftBound = 0;
                float rightBound = 1;
                if (selectedPoint >= 1)
                {
                    leftBound = selectedLinearProperty.Values[selectedPoint - 1].X + minDistanceBetweenPoints;
                }
                if (selectedPoint < selectedLinearProperty.Values.Count - 1)
                {
                    rightBound = selectedLinearProperty.Values[selectedPoint + 1].X - minDistanceBetweenPoints;
                }
                newValue.X = MathHelper.Clamp(newValue.X, leftBound, rightBound);
                // clamp the value vertically
                newValue.Y = MathHelper.Clamp(newValue.Y, selectedLinearProperty.LowerBound, selectedLinearProperty.UpperBound);
                selectedLinearProperty.Values[selectedPoint] = newValue;
                this.Invalidate();
            }
            else
            {
                selectedPoint = -1;
                // check for an highlighted point
                PointF[] drawPoints = new PointF[selectedLinearProperty.Values.Count];
                for (int i = 0; i < selectedLinearProperty.Values.Count; i++)
                {
                    drawPoints[i] = convertLinearValuesToPixelCoordinates(selectedLinearProperty.Values[i]);
                    if (e.X >= drawPoints[i].X - 3 && e.X <= drawPoints[i].X + 3
                        && e.Y >= drawPoints[i].Y - 3 && e.Y <= drawPoints[i].Y + 3)
                    {
                        selectedPoint = i;
                    }
                }
                // Redraw the control if needed
                if (selectedPoint != oldSelectedPoint)
                {
                    this.Invalidate();
                }
            }            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            Rectangle backgroundRect = new Rectangle(0, 0, this.Width, this.Height);
            Rectangle drawingZoneRect = new Rectangle(2, 2, this.Width - 4, this.Height - 4);
            Rectangle chartZoneRect = new Rectangle((int)leftOffset, (int)topOffset, zoneWidth, zoneHeigth);
            // fill each rectangle with a solid color
            e.Graphics.FillRectangle(Brushes.Black, backgroundRect);
            e.Graphics.FillRectangle(Brushes.White, drawingZoneRect);
            e.Graphics.FillRectangle(Brushes.LightGray, chartZoneRect);


            Font drawFontBold = new Font("Tahoma", 9, FontStyle.Bold);
            Font drawFont = new Font("Tahoma", 8);
            StringFormat drawFormatTitle = new StringFormat();
            drawFormatTitle.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(selectedLinearProperty.Description, drawFontBold, Brushes.Black,
                new RectangleF(0, 5, 256, 20), drawFormatTitle);
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            e.Graphics.DrawString("time [0 to 1]", drawFont, Brushes.Black, new RectangleF(leftOffset, topOffset + 204, zoneWidth, 20), drawFormat);
            for (int i = 0; i <= 10; i++)
            {
                int x = (int)(leftOffset + i * (zoneWidth / 10));
                int y = (int)(topOffset + i * (zoneHeigth / 10));
                e.Graphics.DrawLine(Pens.Gray, new PointF(x, topOffset), new PointF(x, topOffset + zoneHeigth - 1));
                e.Graphics.DrawLine(Pens.Gray, new PointF(leftOffset, y), new PointF(leftOffset + zoneWidth - 1, y));
            }            

            PointF[] drawPoints = new PointF[selectedLinearProperty.Values.Count];
            for (int i = 0; i < selectedLinearProperty.Values.Count; i++)
            {
                float value = selectedLinearProperty.Values[i].Y;
                drawPoints[i] = convertLinearValuesToPixelCoordinates(selectedLinearProperty.Values[i]);
            }
            if (drawPoints.Length > 1)
            {
                System.Drawing.Drawing2D.SmoothingMode oldMode = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawLines(Pens.Blue, drawPoints);
                e.Graphics.SmoothingMode = oldMode;
            }
            for (int i = 0; i < selectedLinearProperty.Values.Count; i++)
            {
                Brush paintBrush = Brushes.Red;
                if (i == selectedPoint)
                {
                    paintBrush = Brushes.Aqua;
                    // tool tip
                    String tipText = Math.Round(selectedLinearProperty.Values[selectedPoint].X, 2) + " - "
                        + Math.Round(selectedLinearProperty.Values[selectedPoint].Y, 2);
                    int tipWidth = (int)e.Graphics.MeasureString(tipText, drawFont).Width + 6;
                    int tipPos = 10;
                    if (drawPoints[i].X + tipPos + tipWidth > 256)
                    {
                        tipPos = 256 - (int)drawPoints[i].X - tipWidth - tipPos;
                    }
                    Rectangle tipRect = new Rectangle((int)drawPoints[i].X + tipPos, (int)drawPoints[i].Y - 20, tipWidth, 16);
                    SolidBrush tipBrush = new SolidBrush(System.Drawing.Color.FromArgb(160, System.Drawing.Color.White));
                    e.Graphics.FillRectangle(tipBrush, tipRect);
                    e.Graphics.DrawRectangle(Pens.Black, tipRect);
                    StringFormat drawFormatTip = new StringFormat();
                    drawFormatTip.Alignment = StringAlignment.Center;
                    drawFormatTip.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(tipText, drawFont, Brushes.Black,
                        tipRect, drawFormatTip);
                    tipBrush.Dispose();

                }
                e.Graphics.FillRectangle(paintBrush, drawPoints[i].X - 3, drawPoints[i].Y - 3, 6, 6);
                e.Graphics.DrawRectangle(Pens.Black, drawPoints[i].X - 3, drawPoints[i].Y - 3, 6, 6);
            }
        }
    }
}
