namespace reLIFE.WinFormsUI.Forms
{
    partial class RegistrationForm
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
            getStarted = new Label();
            lblUsername = new Label();
            txtUsernameReg = new MaterialSkin.Controls.MaterialTextBox2();
            txtPasswordReg = new MaterialSkin.Controls.MaterialTextBox2();
            lblPassword = new Label();
            btnRegister = new MaterialSkin.Controls.MaterialButton();
            btnCancelReg = new MaterialSkin.Controls.MaterialButton();
            txtConfirmPasswordReg = new MaterialSkin.Controls.MaterialTextBox2();
            lblConfirmPassword = new Label();
            txtEmailReg = new MaterialSkin.Controls.MaterialTextBox2();
            lblEmail = new Label();
            lblErrorReg = new Label();
            SuspendLayout();
            // 
            // getStarted
            // 
            getStarted.AutoSize = true;
            getStarted.Font = new Font("Lucida Sans", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            getStarted.Location = new Point(95, 83);
            getStarted.Name = "getStarted";
            getStarted.Size = new Size(143, 27);
            getStarted.TabIndex = 0;
            getStarted.Text = "Get Started";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Lucida Sans", 12.75F);
            lblUsername.Location = new Point(30, 140);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(98, 19);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Username:";
            // 
            // txtUsernameReg
            // 
            txtUsernameReg.AnimateReadOnly = false;
            txtUsernameReg.BackgroundImageLayout = ImageLayout.None;
            txtUsernameReg.CharacterCasing = CharacterCasing.Normal;
            txtUsernameReg.Depth = 0;
            txtUsernameReg.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtUsernameReg.HideSelection = true;
            txtUsernameReg.Hint = "Enter username (E.g. ahm4dd)";
            txtUsernameReg.LeadingIcon = null;
            txtUsernameReg.Location = new Point(30, 162);
            txtUsernameReg.MaxLength = 32767;
            txtUsernameReg.MouseState = MaterialSkin.MouseState.OUT;
            txtUsernameReg.Name = "txtUsernameReg";
            txtUsernameReg.PasswordChar = '\0';
            txtUsernameReg.PrefixSuffixText = null;
            txtUsernameReg.ReadOnly = false;
            txtUsernameReg.RightToLeft = RightToLeft.No;
            txtUsernameReg.SelectedText = "";
            txtUsernameReg.SelectionLength = 0;
            txtUsernameReg.SelectionStart = 0;
            txtUsernameReg.ShortcutsEnabled = true;
            txtUsernameReg.Size = new Size(279, 48);
            txtUsernameReg.TabIndex = 2;
            txtUsernameReg.TabStop = false;
            txtUsernameReg.TextAlign = HorizontalAlignment.Left;
            txtUsernameReg.TrailingIcon = null;
            txtUsernameReg.UseSystemPasswordChar = false;
            // 
            // txtPasswordReg
            // 
            txtPasswordReg.AnimateReadOnly = false;
            txtPasswordReg.BackgroundImageLayout = ImageLayout.None;
            txtPasswordReg.CharacterCasing = CharacterCasing.Normal;
            txtPasswordReg.Depth = 0;
            txtPasswordReg.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtPasswordReg.HideSelection = true;
            txtPasswordReg.Hint = "Enter password (E.g. securePass1)";
            txtPasswordReg.LeadingIcon = null;
            txtPasswordReg.Location = new Point(30, 308);
            txtPasswordReg.MaxLength = 32767;
            txtPasswordReg.MouseState = MaterialSkin.MouseState.OUT;
            txtPasswordReg.Name = "txtPasswordReg";
            txtPasswordReg.PasswordChar = '*';
            txtPasswordReg.PrefixSuffixText = null;
            txtPasswordReg.ReadOnly = false;
            txtPasswordReg.RightToLeft = RightToLeft.No;
            txtPasswordReg.SelectedText = "";
            txtPasswordReg.SelectionLength = 0;
            txtPasswordReg.SelectionStart = 0;
            txtPasswordReg.ShortcutsEnabled = true;
            txtPasswordReg.Size = new Size(279, 48);
            txtPasswordReg.TabIndex = 4;
            txtPasswordReg.TabStop = false;
            txtPasswordReg.TextAlign = HorizontalAlignment.Left;
            txtPasswordReg.TrailingIcon = null;
            txtPasswordReg.UseSystemPasswordChar = false;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Lucida Sans", 12.75F);
            lblPassword.Location = new Point(30, 286);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(95, 19);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Password:";
            // 
            // btnRegister
            // 
            btnRegister.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnRegister.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnRegister.Depth = 0;
            btnRegister.HighEmphasis = true;
            btnRegister.Icon = null;
            btnRegister.Location = new Point(220, 498);
            btnRegister.Margin = new Padding(4, 6, 4, 6);
            btnRegister.MouseState = MaterialSkin.MouseState.HOVER;
            btnRegister.Name = "btnRegister";
            btnRegister.NoAccentTextColor = Color.Empty;
            btnRegister.Size = new Size(89, 36);
            btnRegister.TabIndex = 14;
            btnRegister.Text = "Register";
            btnRegister.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnRegister.UseAccentColor = false;
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // btnCancelReg
            // 
            btnCancelReg.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnCancelReg.BackColor = Color.SpringGreen;
            btnCancelReg.BackgroundImageLayout = ImageLayout.None;
            btnCancelReg.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnCancelReg.Depth = 0;
            btnCancelReg.HighEmphasis = true;
            btnCancelReg.Icon = null;
            btnCancelReg.Location = new Point(30, 498);
            btnCancelReg.Margin = new Padding(4, 6, 4, 6);
            btnCancelReg.MouseState = MaterialSkin.MouseState.HOVER;
            btnCancelReg.Name = "btnCancelReg";
            btnCancelReg.NoAccentTextColor = Color.Empty;
            btnCancelReg.Size = new Size(77, 36);
            btnCancelReg.TabIndex = 15;
            btnCancelReg.Text = "Cancel";
            btnCancelReg.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnCancelReg.UseAccentColor = false;
            btnCancelReg.UseVisualStyleBackColor = false;
            btnCancelReg.Click += btnCancelReg_Click;
            // 
            // txtConfirmPasswordReg
            // 
            txtConfirmPasswordReg.AnimateReadOnly = false;
            txtConfirmPasswordReg.BackgroundImageLayout = ImageLayout.None;
            txtConfirmPasswordReg.CharacterCasing = CharacterCasing.Normal;
            txtConfirmPasswordReg.Depth = 0;
            txtConfirmPasswordReg.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtConfirmPasswordReg.HideSelection = true;
            txtConfirmPasswordReg.Hint = "Re-enter your password";
            txtConfirmPasswordReg.LeadingIcon = null;
            txtConfirmPasswordReg.Location = new Point(30, 391);
            txtConfirmPasswordReg.MaxLength = 32767;
            txtConfirmPasswordReg.MouseState = MaterialSkin.MouseState.OUT;
            txtConfirmPasswordReg.Name = "txtConfirmPasswordReg";
            txtConfirmPasswordReg.PasswordChar = '*';
            txtConfirmPasswordReg.PrefixSuffixText = null;
            txtConfirmPasswordReg.ReadOnly = false;
            txtConfirmPasswordReg.RightToLeft = RightToLeft.No;
            txtConfirmPasswordReg.SelectedText = "";
            txtConfirmPasswordReg.SelectionLength = 0;
            txtConfirmPasswordReg.SelectionStart = 0;
            txtConfirmPasswordReg.ShortcutsEnabled = true;
            txtConfirmPasswordReg.Size = new Size(279, 48);
            txtConfirmPasswordReg.TabIndex = 17;
            txtConfirmPasswordReg.TabStop = false;
            txtConfirmPasswordReg.TextAlign = HorizontalAlignment.Left;
            txtConfirmPasswordReg.TrailingIcon = null;
            txtConfirmPasswordReg.UseSystemPasswordChar = false;
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Font = new Font("Lucida Sans", 12.75F);
            lblConfirmPassword.Location = new Point(30, 369);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(169, 19);
            lblConfirmPassword.TabIndex = 16;
            lblConfirmPassword.Text = "Confirm Password:";
            // 
            // txtEmailReg
            // 
            txtEmailReg.AnimateReadOnly = false;
            txtEmailReg.BackgroundImageLayout = ImageLayout.None;
            txtEmailReg.CharacterCasing = CharacterCasing.Normal;
            txtEmailReg.Depth = 0;
            txtEmailReg.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtEmailReg.HideSelection = true;
            txtEmailReg.Hint = "Enter email (E.g. AJ@gmail.com)";
            txtEmailReg.LeadingIcon = null;
            txtEmailReg.Location = new Point(30, 235);
            txtEmailReg.MaxLength = 32767;
            txtEmailReg.MouseState = MaterialSkin.MouseState.OUT;
            txtEmailReg.Name = "txtEmailReg";
            txtEmailReg.PasswordChar = '\0';
            txtEmailReg.PrefixSuffixText = null;
            txtEmailReg.ReadOnly = false;
            txtEmailReg.RightToLeft = RightToLeft.No;
            txtEmailReg.SelectedText = "";
            txtEmailReg.SelectionLength = 0;
            txtEmailReg.SelectionStart = 0;
            txtEmailReg.ShortcutsEnabled = true;
            txtEmailReg.Size = new Size(279, 48);
            txtEmailReg.TabIndex = 19;
            txtEmailReg.TabStop = false;
            txtEmailReg.TextAlign = HorizontalAlignment.Left;
            txtEmailReg.TrailingIcon = null;
            txtEmailReg.UseSystemPasswordChar = false;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Lucida Sans", 12.75F);
            lblEmail.Location = new Point(30, 213);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(61, 19);
            lblEmail.TabIndex = 18;
            lblEmail.Text = "Email:";
            // 
            // lblErrorReg
            // 
            lblErrorReg.BackColor = Color.Transparent;
            lblErrorReg.FlatStyle = FlatStyle.Flat;
            lblErrorReg.Font = new Font("Segoe UI Semibold", 7.75F, FontStyle.Bold);
            lblErrorReg.ForeColor = Color.IndianRed;
            lblErrorReg.Location = new Point(30, 442);
            lblErrorReg.Name = "lblErrorReg";
            lblErrorReg.Size = new Size(279, 50);
            lblErrorReg.TabIndex = 22;
            // 
            // RegistrationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(334, 549);
            Controls.Add(lblErrorReg);
            Controls.Add(txtEmailReg);
            Controls.Add(lblEmail);
            Controls.Add(txtConfirmPasswordReg);
            Controls.Add(lblConfirmPassword);
            Controls.Add(btnCancelReg);
            Controls.Add(btnRegister);
            Controls.Add(txtPasswordReg);
            Controls.Add(lblPassword);
            Controls.Add(txtUsernameReg);
            Controls.Add(lblUsername);
            Controls.Add(getStarted);
            MaximizeBox = false;
            Name = "RegistrationForm";
            Sizable = false;
            Text = "RegistrationForm";
            Load += RegistrationForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label getStarted;
        private Label lblUsername;
        private MaterialSkin.Controls.MaterialTextBox2 txtUsernameReg;
        private MaterialSkin.Controls.MaterialTextBox2 txtPasswordReg;
        private Label lblPassword;
        private MaterialSkin.Controls.MaterialButton btnRegister;
        private MaterialSkin.Controls.MaterialButton btnCancelReg;
        private MaterialSkin.Controls.MaterialTextBox2 txtConfirmPasswordReg;
        private Label lblConfirmPassword;
        private MaterialSkin.Controls.MaterialTextBox2 txtEmailReg;
        private Label lblEmail;
        private Label lblErrorReg;
    }
}