namespace reLIFE.WinFormsUI.Forms
{
    partial class AccountSettingsForm
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
            materialCard1 = new MaterialSkin.Controls.MaterialCard();
            btnClose = new MaterialSkin.Controls.MaterialButton();
            lblUsernameValue = new MaterialSkin.Controls.MaterialLabel();
            lblUsernameHdr = new MaterialSkin.Controls.MaterialLabel();
            materialCard3 = new MaterialSkin.Controls.MaterialCard();
            btnUpdateEmail = new MaterialSkin.Controls.MaterialButton();
            txtEmail = new MaterialSkin.Controls.MaterialTextBox2();
            lblEmailHdr = new MaterialSkin.Controls.MaterialLabel();
            materialCard2 = new MaterialSkin.Controls.MaterialCard();
            lblError = new MaterialSkin.Controls.MaterialLabel();
            btnApplyTheme = new MaterialSkin.Controls.MaterialButton();
            cmbColorScheme = new MaterialSkin.Controls.MaterialComboBox();
            cmbTheme = new MaterialSkin.Controls.MaterialComboBox();
            lblThemeHdr = new MaterialSkin.Controls.MaterialLabel();
            btnChangePassword = new MaterialSkin.Controls.MaterialButton();
            boxShowPasswords = new MaterialSkin.Controls.MaterialCheckbox();
            txtConfirmPassword = new MaterialSkin.Controls.MaterialTextBox2();
            txtNewPassword = new MaterialSkin.Controls.MaterialTextBox2();
            txtCurrentPassword = new MaterialSkin.Controls.MaterialTextBox2();
            materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            lblPasswordHdr = new MaterialSkin.Controls.MaterialLabel();
            flpAccountContent = new FlowLayoutPanel();
            materialCard1.SuspendLayout();
            materialCard3.SuspendLayout();
            materialCard2.SuspendLayout();
            flpAccountContent.SuspendLayout();
            SuspendLayout();
            // 
            // materialCard1
            // 
            materialCard1.BackColor = Color.FromArgb(255, 255, 255);
            materialCard1.Controls.Add(btnClose);
            materialCard1.Controls.Add(lblUsernameValue);
            materialCard1.Controls.Add(lblUsernameHdr);
            materialCard1.Depth = 0;
            materialCard1.ForeColor = Color.FromArgb(222, 0, 0, 0);
            materialCard1.Location = new Point(14, 14);
            materialCard1.Margin = new Padding(14);
            materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            materialCard1.Name = "materialCard1";
            materialCard1.Padding = new Padding(14);
            materialCard1.Size = new Size(736, 100);
            materialCard1.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnClose.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnClose.Depth = 0;
            btnClose.Enabled = false;
            btnClose.HighEmphasis = true;
            btnClose.Icon = null;
            btnClose.Location = new Point(652, 14);
            btnClose.Margin = new Padding(4, 6, 4, 6);
            btnClose.MouseState = MaterialSkin.MouseState.HOVER;
            btnClose.Name = "btnClose";
            btnClose.NoAccentTextColor = Color.Empty;
            btnClose.Size = new Size(66, 36);
            btnClose.TabIndex = 9;
            btnClose.Text = "Close";
            btnClose.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnClose.UseAccentColor = false;
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Visible = false;
            // 
            // lblUsernameValue
            // 
            lblUsernameValue.AutoSize = true;
            lblUsernameValue.Depth = 0;
            lblUsernameValue.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblUsernameValue.Location = new Point(17, 54);
            lblUsernameValue.MouseState = MaterialSkin.MouseState.HOVER;
            lblUsernameValue.Name = "lblUsernameValue";
            lblUsernameValue.Size = new Size(1, 0);
            lblUsernameValue.TabIndex = 1;
            // 
            // lblUsernameHdr
            // 
            lblUsernameHdr.AutoSize = true;
            lblUsernameHdr.Depth = 0;
            lblUsernameHdr.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblUsernameHdr.Location = new Point(17, 14);
            lblUsernameHdr.MouseState = MaterialSkin.MouseState.HOVER;
            lblUsernameHdr.Name = "lblUsernameHdr";
            lblUsernameHdr.Size = new Size(76, 19);
            lblUsernameHdr.TabIndex = 0;
            lblUsernameHdr.Text = "Username:";
            // 
            // materialCard3
            // 
            materialCard3.BackColor = Color.FromArgb(255, 255, 255);
            materialCard3.Controls.Add(btnUpdateEmail);
            materialCard3.Controls.Add(txtEmail);
            materialCard3.Controls.Add(lblEmailHdr);
            materialCard3.Depth = 0;
            materialCard3.ForeColor = Color.FromArgb(222, 0, 0, 0);
            materialCard3.Location = new Point(14, 142);
            materialCard3.Margin = new Padding(14);
            materialCard3.MouseState = MaterialSkin.MouseState.HOVER;
            materialCard3.Name = "materialCard3";
            materialCard3.Padding = new Padding(14);
            materialCard3.Size = new Size(736, 156);
            materialCard3.TabIndex = 3;
            // 
            // btnUpdateEmail
            // 
            btnUpdateEmail.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnUpdateEmail.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnUpdateEmail.Depth = 0;
            btnUpdateEmail.HighEmphasis = true;
            btnUpdateEmail.Icon = null;
            btnUpdateEmail.Location = new Point(281, 106);
            btnUpdateEmail.Margin = new Padding(4, 6, 4, 6);
            btnUpdateEmail.MouseState = MaterialSkin.MouseState.HOVER;
            btnUpdateEmail.Name = "btnUpdateEmail";
            btnUpdateEmail.NoAccentTextColor = Color.Empty;
            btnUpdateEmail.Size = new Size(124, 36);
            btnUpdateEmail.TabIndex = 2;
            btnUpdateEmail.Text = "Update Email";
            btnUpdateEmail.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnUpdateEmail.UseAccentColor = false;
            btnUpdateEmail.UseVisualStyleBackColor = true;
            btnUpdateEmail.Click += BtnUpdateEmail_Click;
            // 
            // txtEmail
            // 
            txtEmail.AnimateReadOnly = false;
            txtEmail.BackgroundImageLayout = ImageLayout.None;
            txtEmail.CharacterCasing = CharacterCasing.Normal;
            txtEmail.Depth = 0;
            txtEmail.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtEmail.HideSelection = true;
            txtEmail.Hint = "Change your email";
            txtEmail.LeadingIcon = null;
            txtEmail.Location = new Point(17, 49);
            txtEmail.MaxLength = 32767;
            txtEmail.MouseState = MaterialSkin.MouseState.OUT;
            txtEmail.Name = "txtEmail";
            txtEmail.PasswordChar = '\0';
            txtEmail.PrefixSuffixText = null;
            txtEmail.ReadOnly = false;
            txtEmail.RightToLeft = RightToLeft.No;
            txtEmail.SelectedText = "";
            txtEmail.SelectionLength = 0;
            txtEmail.SelectionStart = 0;
            txtEmail.ShortcutsEnabled = true;
            txtEmail.Size = new Size(388, 48);
            txtEmail.TabIndex = 1;
            txtEmail.TabStop = false;
            txtEmail.TextAlign = HorizontalAlignment.Left;
            txtEmail.TrailingIcon = null;
            txtEmail.UseSystemPasswordChar = false;
            // 
            // lblEmailHdr
            // 
            lblEmailHdr.AutoSize = true;
            lblEmailHdr.Depth = 0;
            lblEmailHdr.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblEmailHdr.Location = new Point(17, 14);
            lblEmailHdr.MouseState = MaterialSkin.MouseState.HOVER;
            lblEmailHdr.Name = "lblEmailHdr";
            lblEmailHdr.Size = new Size(106, 19);
            lblEmailHdr.TabIndex = 0;
            lblEmailHdr.Text = "Email Address:";
            // 
            // materialCard2
            // 
            materialCard2.BackColor = Color.FromArgb(255, 255, 255);
            materialCard2.Controls.Add(lblError);
            materialCard2.Controls.Add(btnApplyTheme);
            materialCard2.Controls.Add(cmbColorScheme);
            materialCard2.Controls.Add(cmbTheme);
            materialCard2.Controls.Add(lblThemeHdr);
            materialCard2.Controls.Add(btnChangePassword);
            materialCard2.Controls.Add(boxShowPasswords);
            materialCard2.Controls.Add(txtConfirmPassword);
            materialCard2.Controls.Add(txtNewPassword);
            materialCard2.Controls.Add(txtCurrentPassword);
            materialCard2.Controls.Add(materialLabel1);
            materialCard2.Controls.Add(lblPasswordHdr);
            materialCard2.Depth = 0;
            materialCard2.ForeColor = Color.FromArgb(222, 0, 0, 0);
            materialCard2.Location = new Point(14, 326);
            materialCard2.Margin = new Padding(14);
            materialCard2.MouseState = MaterialSkin.MouseState.HOVER;
            materialCard2.Name = "materialCard2";
            materialCard2.Padding = new Padding(14);
            materialCard2.Size = new Size(736, 313);
            materialCard2.TabIndex = 4;
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.Depth = 0;
            lblError.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblError.Location = new Point(469, 213);
            lblError.MouseState = MaterialSkin.MouseState.HOVER;
            lblError.Name = "lblError";
            lblError.Size = new Size(1, 0);
            lblError.TabIndex = 10;
            // 
            // btnApplyTheme
            // 
            btnApplyTheme.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnApplyTheme.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnApplyTheme.Depth = 0;
            btnApplyTheme.HighEmphasis = true;
            btnApplyTheme.Icon = null;
            btnApplyTheme.Location = new Point(553, 167);
            btnApplyTheme.Margin = new Padding(4, 6, 4, 6);
            btnApplyTheme.MouseState = MaterialSkin.MouseState.HOVER;
            btnApplyTheme.Name = "btnApplyTheme";
            btnApplyTheme.NoAccentTextColor = Color.Empty;
            btnApplyTheme.Size = new Size(120, 36);
            btnApplyTheme.TabIndex = 8;
            btnApplyTheme.Text = "Apply Theme";
            btnApplyTheme.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnApplyTheme.UseAccentColor = false;
            btnApplyTheme.UseVisualStyleBackColor = true;
            btnApplyTheme.Click += ThemeOrSchemeChanged;
            // 
            // cmbColorScheme
            // 
            cmbColorScheme.AutoResize = false;
            cmbColorScheme.BackColor = Color.FromArgb(255, 255, 255);
            cmbColorScheme.Depth = 0;
            cmbColorScheme.DrawMode = DrawMode.OwnerDrawVariable;
            cmbColorScheme.DropDownHeight = 174;
            cmbColorScheme.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbColorScheme.DropDownWidth = 121;
            cmbColorScheme.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Pixel);
            cmbColorScheme.ForeColor = Color.FromArgb(222, 0, 0, 0);
            cmbColorScheme.FormattingEnabled = true;
            cmbColorScheme.Hint = "Scheme";
            cmbColorScheme.IntegralHeight = false;
            cmbColorScheme.ItemHeight = 43;
            cmbColorScheme.Location = new Point(469, 109);
            cmbColorScheme.MaxDropDownItems = 4;
            cmbColorScheme.MouseState = MaterialSkin.MouseState.OUT;
            cmbColorScheme.Name = "cmbColorScheme";
            cmbColorScheme.Size = new Size(204, 49);
            cmbColorScheme.StartIndex = 0;
            cmbColorScheme.TabIndex = 7;
            // 
            // cmbTheme
            // 
            cmbTheme.AutoResize = false;
            cmbTheme.BackColor = Color.FromArgb(255, 255, 255);
            cmbTheme.Depth = 0;
            cmbTheme.DrawMode = DrawMode.OwnerDrawVariable;
            cmbTheme.DropDownHeight = 174;
            cmbTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTheme.DropDownWidth = 121;
            cmbTheme.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Pixel);
            cmbTheme.ForeColor = Color.FromArgb(222, 0, 0, 0);
            cmbTheme.FormattingEnabled = true;
            cmbTheme.Hint = "Theme";
            cmbTheme.IntegralHeight = false;
            cmbTheme.ItemHeight = 43;
            cmbTheme.Location = new Point(469, 54);
            cmbTheme.MaxDropDownItems = 4;
            cmbTheme.MouseState = MaterialSkin.MouseState.OUT;
            cmbTheme.Name = "cmbTheme";
            cmbTheme.Size = new Size(204, 49);
            cmbTheme.StartIndex = 0;
            cmbTheme.TabIndex = 6;
            // 
            // lblThemeHdr
            // 
            lblThemeHdr.AutoSize = true;
            lblThemeHdr.Depth = 0;
            lblThemeHdr.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblThemeHdr.Location = new Point(469, 14);
            lblThemeHdr.MouseState = MaterialSkin.MouseState.HOVER;
            lblThemeHdr.Name = "lblThemeHdr";
            lblThemeHdr.Size = new Size(89, 19);
            lblThemeHdr.TabIndex = 5;
            lblThemeHdr.Text = "Appearance:";
            // 
            // btnChangePassword
            // 
            btnChangePassword.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnChangePassword.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnChangePassword.Depth = 0;
            btnChangePassword.HighEmphasis = true;
            btnChangePassword.Icon = null;
            btnChangePassword.Location = new Point(242, 249);
            btnChangePassword.Margin = new Padding(4, 6, 4, 6);
            btnChangePassword.MouseState = MaterialSkin.MouseState.HOVER;
            btnChangePassword.Name = "btnChangePassword";
            btnChangePassword.NoAccentTextColor = Color.Empty;
            btnChangePassword.Size = new Size(163, 36);
            btnChangePassword.TabIndex = 6;
            btnChangePassword.Text = "Change Password";
            btnChangePassword.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnChangePassword.UseAccentColor = false;
            btnChangePassword.UseVisualStyleBackColor = true;
            btnChangePassword.Click += BtnChangePassword_Click;
            // 
            // boxShowPasswords
            // 
            boxShowPasswords.AutoSize = true;
            boxShowPasswords.Depth = 0;
            boxShowPasswords.Location = new Point(17, 248);
            boxShowPasswords.Margin = new Padding(0);
            boxShowPasswords.MouseLocation = new Point(-1, -1);
            boxShowPasswords.MouseState = MaterialSkin.MouseState.HOVER;
            boxShowPasswords.Name = "boxShowPasswords";
            boxShowPasswords.ReadOnly = false;
            boxShowPasswords.Ripple = true;
            boxShowPasswords.Size = new Size(157, 37);
            boxShowPasswords.TabIndex = 5;
            boxShowPasswords.Text = "Show Passwords";
            boxShowPasswords.UseVisualStyleBackColor = true;
            boxShowPasswords.CheckedChanged += BoxShowPasswords_CheckedChanged;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.AnimateReadOnly = false;
            txtConfirmPassword.BackgroundImageLayout = ImageLayout.None;
            txtConfirmPassword.CharacterCasing = CharacterCasing.Normal;
            txtConfirmPassword.Depth = 0;
            txtConfirmPassword.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtConfirmPassword.HideSelection = true;
            txtConfirmPassword.Hint = "Confirm New Password";
            txtConfirmPassword.LeadingIcon = null;
            txtConfirmPassword.Location = new Point(17, 188);
            txtConfirmPassword.MaxLength = 32767;
            txtConfirmPassword.MouseState = MaterialSkin.MouseState.OUT;
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '\0';
            txtConfirmPassword.PrefixSuffixText = null;
            txtConfirmPassword.ReadOnly = false;
            txtConfirmPassword.RightToLeft = RightToLeft.No;
            txtConfirmPassword.SelectedText = "";
            txtConfirmPassword.SelectionLength = 0;
            txtConfirmPassword.SelectionStart = 0;
            txtConfirmPassword.ShortcutsEnabled = true;
            txtConfirmPassword.Size = new Size(388, 48);
            txtConfirmPassword.TabIndex = 4;
            txtConfirmPassword.TabStop = false;
            txtConfirmPassword.TextAlign = HorizontalAlignment.Left;
            txtConfirmPassword.TrailingIcon = null;
            txtConfirmPassword.UseSystemPasswordChar = false;
            // 
            // txtNewPassword
            // 
            txtNewPassword.AnimateReadOnly = false;
            txtNewPassword.BackgroundImageLayout = ImageLayout.None;
            txtNewPassword.CharacterCasing = CharacterCasing.Normal;
            txtNewPassword.Depth = 0;
            txtNewPassword.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtNewPassword.HideSelection = true;
            txtNewPassword.Hint = "New Password";
            txtNewPassword.LeadingIcon = null;
            txtNewPassword.Location = new Point(17, 134);
            txtNewPassword.MaxLength = 32767;
            txtNewPassword.MouseState = MaterialSkin.MouseState.OUT;
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PasswordChar = '\0';
            txtNewPassword.PrefixSuffixText = null;
            txtNewPassword.ReadOnly = false;
            txtNewPassword.RightToLeft = RightToLeft.No;
            txtNewPassword.SelectedText = "";
            txtNewPassword.SelectionLength = 0;
            txtNewPassword.SelectionStart = 0;
            txtNewPassword.ShortcutsEnabled = true;
            txtNewPassword.Size = new Size(388, 48);
            txtNewPassword.TabIndex = 3;
            txtNewPassword.TabStop = false;
            txtNewPassword.TextAlign = HorizontalAlignment.Left;
            txtNewPassword.TrailingIcon = null;
            txtNewPassword.UseSystemPasswordChar = false;
            // 
            // txtCurrentPassword
            // 
            txtCurrentPassword.AnimateReadOnly = false;
            txtCurrentPassword.BackgroundImageLayout = ImageLayout.None;
            txtCurrentPassword.CharacterCasing = CharacterCasing.Normal;
            txtCurrentPassword.Depth = 0;
            txtCurrentPassword.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtCurrentPassword.HideSelection = true;
            txtCurrentPassword.Hint = "Current Password";
            txtCurrentPassword.LeadingIcon = null;
            txtCurrentPassword.Location = new Point(17, 54);
            txtCurrentPassword.MaxLength = 32767;
            txtCurrentPassword.MouseState = MaterialSkin.MouseState.OUT;
            txtCurrentPassword.Name = "txtCurrentPassword";
            txtCurrentPassword.PasswordChar = '\0';
            txtCurrentPassword.PrefixSuffixText = null;
            txtCurrentPassword.ReadOnly = false;
            txtCurrentPassword.RightToLeft = RightToLeft.No;
            txtCurrentPassword.SelectedText = "";
            txtCurrentPassword.SelectionLength = 0;
            txtCurrentPassword.SelectionStart = 0;
            txtCurrentPassword.ShortcutsEnabled = true;
            txtCurrentPassword.Size = new Size(388, 48);
            txtCurrentPassword.TabIndex = 2;
            txtCurrentPassword.TabStop = false;
            txtCurrentPassword.TextAlign = HorizontalAlignment.Left;
            txtCurrentPassword.TrailingIcon = null;
            txtCurrentPassword.UseSystemPasswordChar = false;
            // 
            // materialLabel1
            // 
            materialLabel1.AutoSize = true;
            materialLabel1.Depth = 0;
            materialLabel1.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel1.Location = new Point(17, 54);
            materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel1.Name = "materialLabel1";
            materialLabel1.Size = new Size(1, 0);
            materialLabel1.TabIndex = 1;
            // 
            // lblPasswordHdr
            // 
            lblPasswordHdr.AutoSize = true;
            lblPasswordHdr.Depth = 0;
            lblPasswordHdr.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblPasswordHdr.Location = new Point(17, 14);
            lblPasswordHdr.MouseState = MaterialSkin.MouseState.HOVER;
            lblPasswordHdr.Name = "lblPasswordHdr";
            lblPasswordHdr.Size = new Size(133, 19);
            lblPasswordHdr.TabIndex = 0;
            lblPasswordHdr.Text = "Change Password:";
            // 
            // flpAccountContent
            // 
            flpAccountContent.Controls.Add(materialCard1);
            flpAccountContent.Controls.Add(materialCard3);
            flpAccountContent.Controls.Add(materialCard2);
            flpAccountContent.Location = new Point(6, 16);
            flpAccountContent.Name = "flpAccountContent";
            flpAccountContent.Size = new Size(760, 653);
            flpAccountContent.TabIndex = 10;
            // 
            // AccountSettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(788, 675);
            Controls.Add(flpAccountContent);
            FormStyle = FormStyles.StatusAndActionBar_None;
            Name = "AccountSettingsForm";
            Padding = new Padding(3, 0, 3, 3);
            Text = "AccountSettingsForm";
            Load += AccountSettingsForm_Load;
            materialCard1.ResumeLayout(false);
            materialCard1.PerformLayout();
            materialCard3.ResumeLayout(false);
            materialCard3.PerformLayout();
            materialCard2.ResumeLayout(false);
            materialCard2.PerformLayout();
            flpAccountContent.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialLabel lblUsernameValue;
        private MaterialSkin.Controls.MaterialLabel lblUsernameHdr;
        private MaterialSkin.Controls.MaterialCard materialCard3;
        private MaterialSkin.Controls.MaterialButton btnUpdateEmail;
        private MaterialSkin.Controls.MaterialTextBox2 txtEmail;
        private MaterialSkin.Controls.MaterialLabel lblEmailHdr;
        private MaterialSkin.Controls.MaterialCard materialCard2;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel lblPasswordHdr;
        private MaterialSkin.Controls.MaterialButton btnChangePassword;
        private MaterialSkin.Controls.MaterialCheckbox boxShowPasswords;
        private MaterialSkin.Controls.MaterialTextBox2 txtConfirmPassword;
        private MaterialSkin.Controls.MaterialTextBox2 txtNewPassword;
        private MaterialSkin.Controls.MaterialTextBox2 txtCurrentPassword;
        private MaterialSkin.Controls.MaterialButton btnApplyTheme;
        private MaterialSkin.Controls.MaterialComboBox cmbColorScheme;
        private MaterialSkin.Controls.MaterialComboBox cmbTheme;
        private MaterialSkin.Controls.MaterialLabel lblThemeHdr;
        private MaterialSkin.Controls.MaterialButton btnClose;
        private MaterialSkin.Controls.MaterialLabel lblError;
        private FlowLayoutPanel flpAccountContent;
    }
}