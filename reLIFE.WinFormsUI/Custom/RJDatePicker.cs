using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
// REMOVED: using reLIFE.WinFormsUI.Properties; // Not using Resources anymore
using System.IO; // Needed for Path

namespace reLIFE.WinFormsUI.CustomControls.RJControls
{
    public class RJDatePicker : DateTimePicker
    {
        // --- Static Fields for Images (Load Once) ---
        private static readonly Image? _calendarIconWhite; // Nullable initially
        private static readonly Image? _calendarIconDark;

        // --- Static Constructor (Load Images from Files) ---
        static RJDatePicker()
        {
            try
            {
                string assetsPath = Path.Combine(Application.StartupPath, "Assets"); // Path to Assets in output dir
                string whiteIconPath = Path.Combine(assetsPath, "calendarWhite.png"); // Adjust filename/extension if needed
                string darkIconPath = Path.Combine(assetsPath, "calendarDark.png");   // Adjust filename/extension if needed

                if (File.Exists(whiteIconPath))
                {
                    _calendarIconWhite = Image.FromFile(whiteIconPath);
                }
                else
                {
                    Console.WriteLine($"ERROR: RJDatePicker - Could not find file: {whiteIconPath}");
                    // Optionally load a default system icon or leave null
                }

                if (File.Exists(darkIconPath))
                {
                    _calendarIconDark = Image.FromFile(darkIconPath);
                }
                else
                {
                    Console.WriteLine($"ERROR: RJDatePicker - Could not find file: {darkIconPath}");
                    // Optionally load a default system icon or leave null
                }
            }
            catch (Exception ex)
            {
                // Log the error loading images
                Console.WriteLine($"ERROR: RJDatePicker - Failed to load calendar icons from files: {ex.Message}");
                // Icons will remain null, OnPaint needs to handle this
            }
        }


        // --- Instance Fields ---
        private Color skinColor = Color.MediumSlateBlue;
        private Color textColor = Color.White;
        private Color borderColor = Color.PaleVioletRed;
        private int borderSize = 0;
        private bool droppedDown = false;
        private Image? _currentCalendarIcon; // Instance field to hold the icon to use
        private RectangleF iconButtonArea;
        private const int calendarIconWidth = 34; // Assumes image width allows this space
        private const int arrowIconWidth = 17;

        // Properties
        public Color SkinColor
        {
            get { return skinColor; }
            set
            {
                skinColor = value;
                // Select icon based on brightness AND if icon loaded successfully
                if (skinColor.GetBrightness() >= 0.6F)
                    _currentCalendarIcon = _calendarIconDark; // Use dark icon if available
                else
                    _currentCalendarIcon = _calendarIconWhite; // Use white icon if available

                this.Invalidate(); // Redraw
            }
        }

        public Color TextColor
        { /* ... getter/setter with Invalidate ... */
            get { return textColor; }
            set { textColor = value; this.Invalidate(); }
        }
        public Color BorderColor
        { /* ... getter/setter with Invalidate ... */
            get { return borderColor; }
            set { borderColor = value; this.Invalidate(); }
        }
        public int BorderSize
        { /* ... getter/setter with Invalidate ... */
            get { return borderSize; }
            set { borderSize = value; this.Invalidate(); }
        }


        // Constructor
        public RJDatePicker()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true); // Added ResizeRedraw & DoubleBuffer
            this.MinimumSize = new Size(0, 35);
            this.Font = new Font(this.Font.Name, 9.5F);

            // Initialize _currentCalendarIcon based on initial SkinColor
            this.SkinColor = skinColor; // Call setter to set initial icon
        }

        // Overridden methods
        protected override void OnDropDown(EventArgs eventargs) { base.OnDropDown(eventargs); droppedDown = true; this.Invalidate(); } // Invalidate on state change
        protected override void OnCloseUp(EventArgs eventargs) { base.OnCloseUp(eventargs); droppedDown = false; this.Invalidate(); } // Invalidate on state change
        protected override void OnKeyPress(KeyPressEventArgs e) { base.OnKeyPress(e); e.Handled = true; } // Prevent manual text editing

        protected override void OnPaint(PaintEventArgs e)
        {
            // Use provided Graphics object from PaintEventArgs for better drawing
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias; // Optional: Smoother drawing

            // Use ClientRectangle for bounds available for drawing
            Rectangle clientRect = this.ClientRectangle;
            // Adjust bounds slightly for border if needed (prevents drawing over border)
            RectangleF drawingArea = new RectangleF(clientRect.X, clientRect.Y, clientRect.Width - 0.5f, clientRect.Height - 0.5f);
            if (borderSize > 0)
            {
                drawingArea.Inflate(-borderSize / 2.0f, -borderSize / 2.0f);
            }

            RectangleF iconArea = new RectangleF(drawingArea.Width + drawingArea.X - calendarIconWidth, drawingArea.Y, calendarIconWidth, drawingArea.Height);

            // Pens and Brushes should be disposed
            using (Pen penBorder = new Pen(borderColor, borderSize))
            using (SolidBrush skinBrush = new SolidBrush(skinColor))
            using (SolidBrush openIconBrush = new SolidBrush(Color.FromArgb(50, 64, 64, 64))) // Semi-transparent highlight
            using (SolidBrush textBrush = new SolidBrush(textColor))
            using (StringFormat textFormat = new StringFormat())
            {
                penBorder.Alignment = PenAlignment.Center; // Center aligns better with adjusted drawing area
                textFormat.LineAlignment = StringAlignment.Center;
                textFormat.Alignment = StringAlignment.Near; // Align text left

                // Draw Surface
                graphics.FillRectangle(skinBrush, clientRect); // Fill the whole client area

                // Draw Text (add padding manually)
                // Adjust text drawing area slightly to avoid overlap with icon/border
                RectangleF textArea = drawingArea;
                textArea.Width -= calendarIconWidth; // Reduce width for icon space
                textArea.X += 4; // Add left padding
                graphics.DrawString(this.Text, this.Font, textBrush, textArea, textFormat);

                // Draw open calendar icon highlight
                if (droppedDown) graphics.FillRectangle(openIconBrush, iconArea);

                // Draw Icon if it loaded correctly
                if (_currentCalendarIcon != null)
                {
                    graphics.DrawImage(_currentCalendarIcon,
                        (int)(iconArea.X + (iconArea.Width - _currentCalendarIcon.Width) / 2), // Center icon horizontally within icon area
                        (int)(iconArea.Y + (iconArea.Height - _currentCalendarIcon.Height) / 2)); // Center icon vertically
                }

                // Draw Border (draw last, on top of fill)
                if (borderSize >= 1)
                {
                    // Draw rectangle inset by half border size to keep it within client rect
                    graphics.DrawRectangle(penBorder, clientRect.X + borderSize / 2.0f, clientRect.Y + borderSize / 2.0f, clientRect.Width - borderSize, clientRect.Height - borderSize);
                }
            }
        }

        // --- Force redraw on size change ---
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateIconArea(); // Recalculate icon area when size changes
            this.Invalidate(); // Force redraw
        }


        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateIconArea(); // Calculate initial icon area
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (iconButtonArea.Contains(e.Location)) this.Cursor = Cursors.Hand;
            else this.Cursor = Cursors.Default;
        }

        // Private methods
        private void UpdateIconArea() // Helper to recalculate icon area
        {
            int iconWidth = GetIconButtonWidth();
            iconButtonArea = new RectangleF(this.ClientRectangle.Width - iconWidth, 0, iconWidth, this.ClientRectangle.Height);
        }

        private int GetIconButtonWidth()
        {
            // This logic might need adjustment based on how text is actually drawn
            // Using a fixed width for the icon area might be simpler
            return calendarIconWidth; // Keep it simple for now
            // int textWidth = TextRenderer.MeasureText(this.Text, this.Font).Width;
            // if (textWidth <= this.Width - (calendarIconWidth + 20))
            //     return calendarIconWidth;
            // else return arrowIconWidth;
        }

        // --- Dispose Images ---
        // Need to dispose the static images when the application exits if loaded from file
        // This is tricky with static. A better approach is an instance-based image load
        // or ensuring your app properly cleans up static resources on exit.
        // For now, we omit explicit static disposal, relying on process termination.
    }
}