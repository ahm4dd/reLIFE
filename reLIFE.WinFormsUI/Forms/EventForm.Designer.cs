using reLIFE.WinFormsUI.CustomControls.RJControls;

namespace reLIFE.WinFormsUI.Forms
{
    partial class EventForm
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
            txtTitle = new MaterialSkin.Controls.MaterialTextBox2();
            txtDescription = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            materialDivider1 = new MaterialSkin.Controls.MaterialDivider();
            dtpStartDate = new RJDatePicker();
            materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            materialLabel4 = new MaterialSkin.Controls.MaterialLabel();
            dtpEndDate = new RJDatePicker();
            chkAllDay = new MaterialSkin.Controls.MaterialCheckbox();
            materialDivider2 = new MaterialSkin.Controls.MaterialDivider();
            materialLabel5 = new MaterialSkin.Controls.MaterialLabel();
            cmbCategory = new MaterialSkin.Controls.MaterialComboBox();
            lblError = new MaterialSkin.Controls.MaterialLabel();
            chkEnableReminder = new MaterialSkin.Controls.MaterialCheckbox();
            nudReminderMinutes = new NumericUpDown();
            lblMinutesBefore = new MaterialSkin.Controls.MaterialLabel();
            materialDivider4 = new MaterialSkin.Controls.MaterialDivider();
            btnSave = new MaterialSkin.Controls.MaterialButton();
            btnCancel = new MaterialSkin.Controls.MaterialButton();
            materialLabel6 = new MaterialSkin.Controls.MaterialLabel();
            dtpStartTime = new RJDatePicker();
            materialLabel7 = new MaterialSkin.Controls.MaterialLabel();
            dtpEndTime = new RJDatePicker();
            ((System.ComponentModel.ISupportInitialize)nudReminderMinutes).BeginInit();
            SuspendLayout();
            // 
            // txtTitle
            // 
            txtTitle.AnimateReadOnly = false;
            txtTitle.BackgroundImageLayout = ImageLayout.None;
            txtTitle.CharacterCasing = CharacterCasing.Normal;
            txtTitle.Depth = 0;
            txtTitle.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtTitle.HideSelection = true;
            txtTitle.Hint = "Enter event title";
            txtTitle.LeadingIcon = null;
            txtTitle.Location = new Point(17, 70);
            txtTitle.MaxLength = 32767;
            txtTitle.MouseState = MaterialSkin.MouseState.OUT;
            txtTitle.Name = "txtTitle";
            txtTitle.PasswordChar = '\0';
            txtTitle.PrefixSuffixText = null;
            txtTitle.ReadOnly = false;
            txtTitle.RightToLeft = RightToLeft.No;
            txtTitle.SelectedText = "";
            txtTitle.SelectionLength = 0;
            txtTitle.SelectionStart = 0;
            txtTitle.ShortcutsEnabled = true;
            txtTitle.Size = new Size(422, 48);
            txtTitle.TabIndex = 1;
            txtTitle.TabStop = false;
            txtTitle.TextAlign = HorizontalAlignment.Left;
            txtTitle.TrailingIcon = null;
            txtTitle.UseSystemPasswordChar = false;
            // 
            // txtDescription
            // 
            txtDescription.BackColor = Color.FromArgb(255, 255, 255);
            txtDescription.BorderStyle = BorderStyle.None;
            txtDescription.Depth = 0;
            txtDescription.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtDescription.ForeColor = Color.FromArgb(222, 0, 0, 0);
            txtDescription.Location = new Point(17, 162);
            txtDescription.MouseState = MaterialSkin.MouseState.HOVER;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(422, 81);
            txtDescription.TabIndex = 2;
            txtDescription.Text = "";
            // 
            // materialLabel1
            // 
            materialLabel1.AutoSize = true;
            materialLabel1.Depth = 0;
            materialLabel1.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel1.Location = new Point(17, 131);
            materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel1.Name = "materialLabel1";
            materialLabel1.Size = new Size(85, 19);
            materialLabel1.TabIndex = 3;
            materialLabel1.Text = "Description:";
            // 
            // materialLabel2
            // 
            materialLabel2.AutoSize = true;
            materialLabel2.Depth = 0;
            materialLabel2.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel2.Location = new Point(17, 39);
            materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel2.Name = "materialLabel2";
            materialLabel2.Size = new Size(36, 19);
            materialLabel2.TabIndex = 4;
            materialLabel2.Text = "Title:";
            // 
            // materialDivider1
            // 
            materialDivider1.BackColor = Color.FromArgb(30, 0, 0, 0);
            materialDivider1.Depth = 0;
            materialDivider1.Location = new Point(-8, 261);
            materialDivider1.MouseState = MaterialSkin.MouseState.HOVER;
            materialDivider1.Name = "materialDivider1";
            materialDivider1.Size = new Size(496, 23);
            materialDivider1.TabIndex = 5;
            materialDivider1.Text = "materialDivider1";
            // 
            // dtpStartDate
            // 
            dtpStartDate.BorderColor = Color.PaleVioletRed;
            dtpStartDate.BorderSize = 0;
            dtpStartDate.Font = new Font("Segoe UI", 9.5F);
            dtpStartDate.Location = new Point(17, 329);
            dtpStartDate.MinimumSize = new Size(4, 35);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new Size(200, 35);
            dtpStartDate.SkinColor = Color.MediumSlateBlue;
            dtpStartDate.TabIndex = 6;
            dtpStartDate.TextColor = Color.White;
            // 
            // materialLabel3
            // 
            materialLabel3.AutoSize = true;
            materialLabel3.Depth = 0;
            materialLabel3.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel3.Location = new Point(17, 298);
            materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel3.Name = "materialLabel3";
            materialLabel3.Size = new Size(74, 19);
            materialLabel3.TabIndex = 7;
            materialLabel3.Text = "Start date:";
            // 
            // materialLabel4
            // 
            materialLabel4.AutoSize = true;
            materialLabel4.Depth = 0;
            materialLabel4.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel4.Location = new Point(239, 298);
            materialLabel4.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel4.Name = "materialLabel4";
            materialLabel4.Size = new Size(67, 19);
            materialLabel4.TabIndex = 8;
            materialLabel4.Text = "End date:";
            // 
            // dtpEndDate
            // 
            dtpEndDate.BorderColor = Color.PaleVioletRed;
            dtpEndDate.BorderSize = 0;
            dtpEndDate.Font = new Font("Segoe UI", 9.5F);
            dtpEndDate.Location = new Point(238, 329);
            dtpEndDate.MinimumSize = new Size(4, 35);
            dtpEndDate.Name = "dtpEndDate";
            dtpEndDate.Size = new Size(200, 35);
            dtpEndDate.SkinColor = Color.MediumSlateBlue;
            dtpEndDate.TabIndex = 9;
            dtpEndDate.TextColor = Color.White;
            // 
            // chkAllDay
            // 
            chkAllDay.AutoSize = true;
            chkAllDay.Depth = 0;
            chkAllDay.Location = new Point(17, 438);
            chkAllDay.Margin = new Padding(0);
            chkAllDay.MouseLocation = new Point(-1, -1);
            chkAllDay.MouseState = MaterialSkin.MouseState.HOVER;
            chkAllDay.Name = "chkAllDay";
            chkAllDay.ReadOnly = false;
            chkAllDay.Ripple = true;
            chkAllDay.Size = new Size(83, 37);
            chkAllDay.TabIndex = 10;
            chkAllDay.Text = "All day";
            chkAllDay.UseVisualStyleBackColor = true;
            chkAllDay.CheckedChanged += chkAllDay_CheckedChanged;
            // 
            // materialDivider2
            // 
            materialDivider2.BackColor = Color.FromArgb(30, 0, 0, 0);
            materialDivider2.Depth = 0;
            materialDivider2.Location = new Point(-8, 489);
            materialDivider2.MouseState = MaterialSkin.MouseState.HOVER;
            materialDivider2.Name = "materialDivider2";
            materialDivider2.Size = new Size(496, 23);
            materialDivider2.TabIndex = 11;
            materialDivider2.Text = "materialDivider2";
            // 
            // materialLabel5
            // 
            materialLabel5.AutoSize = true;
            materialLabel5.Depth = 0;
            materialLabel5.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel5.Location = new Point(17, 524);
            materialLabel5.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel5.Name = "materialLabel5";
            materialLabel5.Size = new Size(68, 19);
            materialLabel5.TabIndex = 12;
            materialLabel5.Text = "Category:";
            // 
            // cmbCategory
            // 
            cmbCategory.AutoResize = false;
            cmbCategory.BackColor = Color.FromArgb(255, 255, 255);
            cmbCategory.Depth = 0;
            cmbCategory.DrawMode = DrawMode.OwnerDrawVariable;
            cmbCategory.DropDownHeight = 174;
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.DropDownWidth = 121;
            cmbCategory.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Pixel);
            cmbCategory.ForeColor = Color.FromArgb(222, 0, 0, 0);
            cmbCategory.FormattingEnabled = true;
            cmbCategory.IntegralHeight = false;
            cmbCategory.ItemHeight = 43;
            cmbCategory.Location = new Point(17, 556);
            cmbCategory.MaxDropDownItems = 4;
            cmbCategory.MouseState = MaterialSkin.MouseState.OUT;
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(200, 49);
            cmbCategory.StartIndex = 0;
            cmbCategory.TabIndex = 13;
            // 
            // lblError
            // 
            lblError.Depth = 0;
            lblError.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblError.Location = new Point(17, 676);
            lblError.MouseState = MaterialSkin.MouseState.HOVER;
            lblError.Name = "lblError";
            lblError.Size = new Size(278, 36);
            lblError.TabIndex = 14;
            // 
            // chkEnableReminder
            // 
            chkEnableReminder.AutoSize = true;
            chkEnableReminder.Depth = 0;
            chkEnableReminder.Location = new Point(239, 544);
            chkEnableReminder.Margin = new Padding(0);
            chkEnableReminder.MouseLocation = new Point(-1, -1);
            chkEnableReminder.MouseState = MaterialSkin.MouseState.HOVER;
            chkEnableReminder.Name = "chkEnableReminder";
            chkEnableReminder.ReadOnly = false;
            chkEnableReminder.Ripple = true;
            chkEnableReminder.Size = new Size(154, 37);
            chkEnableReminder.TabIndex = 16;
            chkEnableReminder.Text = "Enable Reminder";
            chkEnableReminder.UseVisualStyleBackColor = true;
            chkEnableReminder.CheckedChanged += chkEnableReminder_CheckedChanged;
            // 
            // nudReminderMinutes
            // 
            nudReminderMinutes.Location = new Point(244, 584);
            nudReminderMinutes.Name = "nudReminderMinutes";
            nudReminderMinutes.Size = new Size(74, 23);
            nudReminderMinutes.TabIndex = 17;
            // 
            // lblMinutesBefore
            // 
            lblMinutesBefore.AutoSize = true;
            lblMinutesBefore.Depth = 0;
            lblMinutesBefore.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblMinutesBefore.Location = new Point(324, 586);
            lblMinutesBefore.MouseState = MaterialSkin.MouseState.HOVER;
            lblMinutesBefore.Name = "lblMinutesBefore";
            lblMinutesBefore.Size = new Size(107, 19);
            lblMinutesBefore.TabIndex = 18;
            lblMinutesBefore.Text = "Minutes before";
            // 
            // materialDivider4
            // 
            materialDivider4.BackColor = Color.FromArgb(30, 0, 0, 0);
            materialDivider4.Depth = 0;
            materialDivider4.Location = new Point(-8, 628);
            materialDivider4.MouseState = MaterialSkin.MouseState.HOVER;
            materialDivider4.Name = "materialDivider4";
            materialDivider4.Size = new Size(496, 23);
            materialDivider4.TabIndex = 19;
            materialDivider4.Text = "materialDivider4";
            // 
            // btnSave
            // 
            btnSave.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSave.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnSave.Depth = 0;
            btnSave.HighEmphasis = true;
            btnSave.Icon = null;
            btnSave.Location = new Point(389, 676);
            btnSave.Margin = new Padding(4, 6, 4, 6);
            btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            btnSave.Name = "btnSave";
            btnSave.NoAccentTextColor = Color.Empty;
            btnSave.Size = new Size(64, 36);
            btnSave.TabIndex = 21;
            btnSave.Text = "Save";
            btnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnSave.UseAccentColor = false;
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnCancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnCancel.Depth = 0;
            btnCancel.HighEmphasis = true;
            btnCancel.Icon = null;
            btnCancel.Location = new Point(304, 676);
            btnCancel.Margin = new Padding(4, 6, 4, 6);
            btnCancel.MouseState = MaterialSkin.MouseState.HOVER;
            btnCancel.Name = "btnCancel";
            btnCancel.NoAccentTextColor = Color.Empty;
            btnCancel.Size = new Size(77, 36);
            btnCancel.TabIndex = 22;
            btnCancel.Text = "Cancel";
            btnCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnCancel.UseAccentColor = false;
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // materialLabel6
            // 
            materialLabel6.AutoSize = true;
            materialLabel6.Depth = 0;
            materialLabel6.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel6.Location = new Point(17, 370);
            materialLabel6.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel6.Name = "materialLabel6";
            materialLabel6.Size = new Size(74, 19);
            materialLabel6.TabIndex = 24;
            materialLabel6.Text = "Start time:";
            // 
            // dtpStartTime
            // 
            dtpStartTime.BorderColor = Color.PaleVioletRed;
            dtpStartTime.BorderSize = 0;
            dtpStartTime.Font = new Font("Segoe UI", 9.5F);
            dtpStartTime.Location = new Point(17, 401);
            dtpStartTime.MinimumSize = new Size(4, 35);
            dtpStartTime.Name = "dtpStartTime";
            dtpStartTime.Size = new Size(200, 35);
            dtpStartTime.SkinColor = Color.MediumSlateBlue;
            dtpStartTime.TabIndex = 23;
            dtpStartTime.TextColor = Color.White;
            // 
            // materialLabel7
            // 
            materialLabel7.AutoSize = true;
            materialLabel7.Depth = 0;
            materialLabel7.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel7.Location = new Point(239, 370);
            materialLabel7.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel7.Name = "materialLabel7";
            materialLabel7.Size = new Size(67, 19);
            materialLabel7.TabIndex = 26;
            materialLabel7.Text = "End time:";
            // 
            // dtpEndTime
            // 
            dtpEndTime.BorderColor = Color.PaleVioletRed;
            dtpEndTime.BorderSize = 0;
            dtpEndTime.Font = new Font("Segoe UI", 9.5F);
            dtpEndTime.Location = new Point(238, 401);
            dtpEndTime.MinimumSize = new Size(4, 35);
            dtpEndTime.Name = "dtpEndTime";
            dtpEndTime.Size = new Size(200, 35);
            dtpEndTime.SkinColor = Color.MediumSlateBlue;
            dtpEndTime.TabIndex = 25;
            dtpEndTime.TextColor = Color.White;
            // 
            // EventForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(458, 732);
            Controls.Add(materialLabel7);
            Controls.Add(dtpEndTime);
            Controls.Add(materialLabel6);
            Controls.Add(dtpStartTime);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(materialDivider4);
            Controls.Add(lblMinutesBefore);
            Controls.Add(nudReminderMinutes);
            Controls.Add(chkEnableReminder);
            Controls.Add(lblError);
            Controls.Add(cmbCategory);
            Controls.Add(materialLabel5);
            Controls.Add(materialDivider2);
            Controls.Add(chkAllDay);
            Controls.Add(dtpEndDate);
            Controls.Add(materialLabel4);
            Controls.Add(materialLabel3);
            Controls.Add(dtpStartDate);
            Controls.Add(materialDivider1);
            Controls.Add(materialLabel2);
            Controls.Add(materialLabel1);
            Controls.Add(txtDescription);
            Controls.Add(txtTitle);
            FormStyle = FormStyles.ActionBar_None;
            Name = "EventForm";
            Padding = new Padding(3, 24, 3, 3);
            Text = "EventForm";
            Load += EventForm_Load;
            ((System.ComponentModel.ISupportInitialize)nudReminderMinutes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MaterialSkin.Controls.MaterialTextBox2 txtTitle;
        private MaterialSkin.Controls.MaterialMultiLineTextBox txtDescription;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialDivider materialDivider1;
        private RJDatePicker dtpStartDate;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialLabel materialLabel4;
        private RJDatePicker dtpEndDate;
        private MaterialSkin.Controls.MaterialCheckbox chkAllDay;
        private MaterialSkin.Controls.MaterialDivider materialDivider2;
        private MaterialSkin.Controls.MaterialLabel materialLabel5;
        private MaterialSkin.Controls.MaterialComboBox cmbCategory;
        private MaterialSkin.Controls.MaterialLabel lblError;
        private MaterialSkin.Controls.MaterialCheckbox chkEnableReminder;
        private NumericUpDown nudReminderMinutes;
        private MaterialSkin.Controls.MaterialLabel lblMinutesBefore;
        private MaterialSkin.Controls.MaterialDivider materialDivider4;
        private MaterialSkin.Controls.MaterialDivider materialDivider5;
        private MaterialSkin.Controls.MaterialButton btnSave;
        private MaterialSkin.Controls.MaterialButton btnCancel;
        private MaterialSkin.Controls.MaterialLabel materialLabel6;
        private RJDatePicker dtpStartTime;
        private MaterialSkin.Controls.MaterialLabel materialLabel7;
        private RJDatePicker dtpEndTime;
    }
}