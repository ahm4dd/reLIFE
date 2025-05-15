namespace reLIFE.WinFormsUI.Forms
{
    partial class MainForm
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
            dashboardPanel = new Panel();
            btnManageCategories = new MaterialSkin.Controls.MaterialButton();
            materialDivider1 = new MaterialSkin.Controls.MaterialDivider();
            btnLogout = new MaterialSkin.Controls.MaterialButton();
            btnAccountSettings = new MaterialSkin.Controls.MaterialButton();
            btnViewArchive = new MaterialSkin.Controls.MaterialButton();
            btnViewReminders = new MaterialSkin.Controls.MaterialButton();
            btnCalendarView = new MaterialSkin.Controls.MaterialButton();
            pictureBox1 = new PictureBox();
            pnlContent = new Panel();
            dashboardPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dashboardPanel
            // 
            dashboardPanel.BackColor = Color.FromArgb(64, 64, 64);
            dashboardPanel.Controls.Add(btnManageCategories);
            dashboardPanel.Controls.Add(materialDivider1);
            dashboardPanel.Controls.Add(btnLogout);
            dashboardPanel.Controls.Add(btnAccountSettings);
            dashboardPanel.Controls.Add(btnViewArchive);
            dashboardPanel.Controls.Add(btnViewReminders);
            dashboardPanel.Controls.Add(btnCalendarView);
            dashboardPanel.Controls.Add(pictureBox1);
            dashboardPanel.Dock = DockStyle.Left;
            dashboardPanel.ForeColor = SystemColors.ControlText;
            dashboardPanel.Location = new Point(3, 24);
            dashboardPanel.Name = "dashboardPanel";
            dashboardPanel.Size = new Size(200, 548);
            dashboardPanel.TabIndex = 0;
            // 
            // btnManageCategories
            // 
            btnManageCategories.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnManageCategories.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnManageCategories.Depth = 0;
            btnManageCategories.FlatStyle = FlatStyle.Flat;
            btnManageCategories.HighEmphasis = true;
            btnManageCategories.Icon = null;
            btnManageCategories.Location = new Point(22, 270);
            btnManageCategories.Margin = new Padding(4, 6, 4, 6);
            btnManageCategories.MouseState = MaterialSkin.MouseState.HOVER;
            btnManageCategories.Name = "btnManageCategories";
            btnManageCategories.NoAccentTextColor = Color.Empty;
            btnManageCategories.Size = new Size(109, 36);
            btnManageCategories.TabIndex = 6;
            btnManageCategories.Text = "Categories";
            btnManageCategories.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnManageCategories.UseAccentColor = false;
            btnManageCategories.UseVisualStyleBackColor = true;
            btnManageCategories.Click += btnManageCategories_Click;
            // 
            // materialDivider1
            // 
            materialDivider1.BackColor = Color.FromArgb(30, 0, 0, 0);
            materialDivider1.Depth = 0;
            materialDivider1.Location = new Point(200, 0);
            materialDivider1.MouseState = MaterialSkin.MouseState.HOVER;
            materialDivider1.Name = "materialDivider1";
            materialDivider1.Size = new Size(10, 548);
            materialDivider1.TabIndex = 0;
            materialDivider1.Text = "materialDivider1";
            // 
            // btnLogout
            // 
            btnLogout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnLogout.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnLogout.Depth = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.HighEmphasis = true;
            btnLogout.Icon = null;
            btnLogout.Location = new Point(22, 495);
            btnLogout.Margin = new Padding(4, 6, 4, 6);
            btnLogout.MouseState = MaterialSkin.MouseState.HOVER;
            btnLogout.Name = "btnLogout";
            btnLogout.NoAccentTextColor = Color.Empty;
            btnLogout.Size = new Size(78, 36);
            btnLogout.TabIndex = 5;
            btnLogout.Text = "LOGOUT";
            btnLogout.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnLogout.UseAccentColor = false;
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // btnAccountSettings
            // 
            btnAccountSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnAccountSettings.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnAccountSettings.Depth = 0;
            btnAccountSettings.FlatStyle = FlatStyle.Flat;
            btnAccountSettings.HighEmphasis = true;
            btnAccountSettings.Icon = null;
            btnAccountSettings.Location = new Point(22, 414);
            btnAccountSettings.Margin = new Padding(4, 6, 4, 6);
            btnAccountSettings.MouseState = MaterialSkin.MouseState.HOVER;
            btnAccountSettings.Name = "btnAccountSettings";
            btnAccountSettings.NoAccentTextColor = Color.Empty;
            btnAccountSettings.Size = new Size(90, 36);
            btnAccountSettings.TabIndex = 4;
            btnAccountSettings.Text = "Account";
            btnAccountSettings.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnAccountSettings.UseAccentColor = false;
            btnAccountSettings.UseVisualStyleBackColor = true;
            btnAccountSettings.Click += btnAccountSettings_Click;
            // 
            // btnViewArchive
            // 
            btnViewArchive.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnViewArchive.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnViewArchive.Depth = 0;
            btnViewArchive.FlatStyle = FlatStyle.Flat;
            btnViewArchive.HighEmphasis = true;
            btnViewArchive.Icon = null;
            btnViewArchive.Location = new Point(22, 366);
            btnViewArchive.Margin = new Padding(4, 6, 4, 6);
            btnViewArchive.MouseState = MaterialSkin.MouseState.HOVER;
            btnViewArchive.Name = "btnViewArchive";
            btnViewArchive.NoAccentTextColor = Color.Empty;
            btnViewArchive.Size = new Size(82, 36);
            btnViewArchive.TabIndex = 3;
            btnViewArchive.Text = "Archive";
            btnViewArchive.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnViewArchive.UseAccentColor = false;
            btnViewArchive.UseVisualStyleBackColor = true;
            btnViewArchive.Click += btnViewArchive_Click;
            // 
            // btnViewReminders
            // 
            btnViewReminders.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnViewReminders.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnViewReminders.Depth = 0;
            btnViewReminders.FlatStyle = FlatStyle.Flat;
            btnViewReminders.HighEmphasis = true;
            btnViewReminders.Icon = null;
            btnViewReminders.Location = new Point(22, 318);
            btnViewReminders.Margin = new Padding(4, 6, 4, 6);
            btnViewReminders.MouseState = MaterialSkin.MouseState.HOVER;
            btnViewReminders.Name = "btnViewReminders";
            btnViewReminders.NoAccentTextColor = Color.Empty;
            btnViewReminders.Size = new Size(103, 36);
            btnViewReminders.TabIndex = 2;
            btnViewReminders.Text = "Reminders";
            btnViewReminders.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnViewReminders.UseAccentColor = false;
            btnViewReminders.UseVisualStyleBackColor = true;
            btnViewReminders.Click += btnViewReminders_Click;
            // 
            // btnCalendarView
            // 
            btnCalendarView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnCalendarView.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnCalendarView.Depth = 0;
            btnCalendarView.FlatStyle = FlatStyle.Flat;
            btnCalendarView.HighEmphasis = true;
            btnCalendarView.Icon = null;
            btnCalendarView.Location = new Point(22, 222);
            btnCalendarView.Margin = new Padding(4, 6, 4, 6);
            btnCalendarView.MouseState = MaterialSkin.MouseState.HOVER;
            btnCalendarView.Name = "btnCalendarView";
            btnCalendarView.NoAccentTextColor = Color.Empty;
            btnCalendarView.Size = new Size(96, 36);
            btnCalendarView.TabIndex = 1;
            btnCalendarView.Text = "Calendar";
            btnCalendarView.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnCalendarView.UseAccentColor = false;
            btnCalendarView.UseVisualStyleBackColor = true;
            btnCalendarView.Click += btnCalendarView_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.reLIFEicon;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(200, 186);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pnlContent
            // 
            pnlContent.AutoScroll = true;
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(203, 24);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(891, 548);
            pnlContent.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1097, 575);
            Controls.Add(pnlContent);
            Controls.Add(dashboardPanel);
            FormStyle = FormStyles.ActionBar_None;
            MaximizeBox = false;
            Name = "MainForm";
            Padding = new Padding(3, 24, 3, 3);
            Sizable = false;
            Text = "MainForm";
            Load += MainForm_Load;
            dashboardPanel.ResumeLayout(false);
            dashboardPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel dashboardPanel;
        private PictureBox pictureBox1;
        private MaterialSkin.Controls.MaterialButton btnCalendarView;
        private MaterialSkin.Controls.MaterialButton btnViewArchive;
        private MaterialSkin.Controls.MaterialButton btnViewReminders;
        private MaterialSkin.Controls.MaterialButton btnLogout;
        private MaterialSkin.Controls.MaterialButton btnAccountSettings;
        private Panel pnlContent;
        private MaterialSkin.Controls.MaterialDivider materialDivider1;
        private MaterialSkin.Controls.MaterialButton btnManageCategories;
    }
}