namespace reLIFE.WinFormsUI.Forms
{
    partial class CalendarViewForm
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
            flpEvents = new FlowLayoutPanel();
            lblSelectedDateInfo = new MaterialSkin.Controls.MaterialLabel();
            clbCategoryFilter = new MaterialSkin.Controls.MaterialCheckedListBox();
            btnApplyFilters = new MaterialSkin.Controls.MaterialButton();
            btnAddEvent = new MaterialSkin.Controls.MaterialButton();
            dtpSelectedDate = new CustomControls.RJControls.RJDatePicker();
            clbCategoryFilter.SuspendLayout();
            SuspendLayout();
            // 
            // flpEvents
            // 
            flpEvents.AutoScroll = true;
            flpEvents.Location = new Point(377, 75);
            flpEvents.Name = "flpEvents";
            flpEvents.Size = new Size(393, 388);
            flpEvents.TabIndex = 1;
            flpEvents.Click += btnApplyFilters_Click;
            // 
            // lblSelectedDateInfo
            // 
            lblSelectedDateInfo.AutoSize = true;
            lblSelectedDateInfo.Depth = 0;
            lblSelectedDateInfo.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblSelectedDateInfo.Location = new Point(222, 28);
            lblSelectedDateInfo.MouseState = MaterialSkin.MouseState.HOVER;
            lblSelectedDateInfo.Name = "lblSelectedDateInfo";
            lblSelectedDateInfo.Size = new Size(89, 19);
            lblSelectedDateInfo.TabIndex = 2;
            lblSelectedDateInfo.Text = "rnstsrndceta";
            lblSelectedDateInfo.Click += lblSelectedDateInfo_Click;
            // 
            // clbCategoryFilter
            // 
            clbCategoryFilter.AutoScroll = true;
            clbCategoryFilter.BackColor = SystemColors.Control;
            clbCategoryFilter.Controls.Add(btnApplyFilters);
            clbCategoryFilter.Depth = 0;
            clbCategoryFilter.Location = new Point(16, 75);
            clbCategoryFilter.MouseState = MaterialSkin.MouseState.HOVER;
            clbCategoryFilter.Name = "clbCategoryFilter";
            clbCategoryFilter.Size = new Size(355, 388);
            clbCategoryFilter.Striped = false;
            clbCategoryFilter.StripeDarkColor = Color.Empty;
            clbCategoryFilter.TabIndex = 3;
            // 
            // btnApplyFilters
            // 
            btnApplyFilters.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnApplyFilters.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnApplyFilters.Depth = 0;
            btnApplyFilters.HighEmphasis = true;
            btnApplyFilters.Icon = null;
            btnApplyFilters.Location = new Point(225, 346);
            btnApplyFilters.Margin = new Padding(4, 6, 4, 6);
            btnApplyFilters.MouseState = MaterialSkin.MouseState.HOVER;
            btnApplyFilters.Name = "btnApplyFilters";
            btnApplyFilters.NoAccentTextColor = Color.Empty;
            btnApplyFilters.Size = new Size(126, 36);
            btnApplyFilters.TabIndex = 4;
            btnApplyFilters.Text = "Apply Filters";
            btnApplyFilters.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnApplyFilters.UseAccentColor = false;
            btnApplyFilters.UseVisualStyleBackColor = true;
            btnApplyFilters.Click += btnApplyFilters_Click;
            // 
            // btnAddEvent
            // 
            btnAddEvent.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnAddEvent.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnAddEvent.Depth = 0;
            btnAddEvent.HighEmphasis = true;
            btnAddEvent.Icon = null;
            btnAddEvent.Location = new Point(670, 18);
            btnAddEvent.Margin = new Padding(4, 6, 4, 6);
            btnAddEvent.MouseState = MaterialSkin.MouseState.HOVER;
            btnAddEvent.Name = "btnAddEvent";
            btnAddEvent.NoAccentTextColor = Color.Empty;
            btnAddEvent.Size = new Size(100, 36);
            btnAddEvent.TabIndex = 0;
            btnAddEvent.Text = "Add Event";
            btnAddEvent.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnAddEvent.UseAccentColor = false;
            btnAddEvent.UseVisualStyleBackColor = true;
            btnAddEvent.Click += btnAddEvent_Click;
            // 
            // dtpSelectedDate
            // 
            dtpSelectedDate.BorderColor = Color.PaleVioletRed;
            dtpSelectedDate.BorderSize = 0;
            dtpSelectedDate.Font = new Font("Segoe UI", 9.5F);
            dtpSelectedDate.Location = new Point(16, 18);
            dtpSelectedDate.MinimumSize = new Size(0, 35);
            dtpSelectedDate.Name = "dtpSelectedDate";
            dtpSelectedDate.Size = new Size(200, 35);
            dtpSelectedDate.SkinColor = Color.MediumSlateBlue;
            dtpSelectedDate.TabIndex = 4;
            dtpSelectedDate.TextColor = Color.White;
            dtpSelectedDate.ValueChanged += dtpSelectedDate_ValueChanged;
            // 
            // CalendarViewForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(800, 469);
            Controls.Add(dtpSelectedDate);
            Controls.Add(clbCategoryFilter);
            Controls.Add(btnAddEvent);
            Controls.Add(lblSelectedDateInfo);
            Controls.Add(flpEvents);
            FormStyle = FormStyles.StatusAndActionBar_None;
            Name = "CalendarViewForm";
            Padding = new Padding(3, 0, 3, 3);
            Sizable = false;
            Text = "CalendarViewForm";
            WindowState = FormWindowState.Maximized;
            Load += CalendarViewForm_Load;
            clbCategoryFilter.ResumeLayout(false);
            clbCategoryFilter.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dtpSelectedDate2;
        private FlowLayoutPanel flpEvents;
        private MaterialSkin.Controls.MaterialLabel lblSelectedDateInfo;
        private MaterialSkin.Controls.MaterialCheckedListBox clbCategoryFilter;
        private MaterialSkin.Controls.MaterialButton btnAddEvent;
        private MaterialSkin.Controls.MaterialButton btnApplyFilters;
        private CustomControls.RJControls.RJDatePicker dtpSelectedDate;
    }
}