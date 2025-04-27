namespace reLIFE.WinFormsUI
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            GetStarted = new Label();
            Username = new Label();
            Password = new Label();
            lblError = new Label();
            btnLogin = new MaterialSkin.Controls.MaterialButton();
            txtUsername = new MaterialSkin.Controls.MaterialTextBox2();
            lnkRegister = new MaterialSkin.Controls.MaterialButton();
            txtPassword = new MaterialSkin.Controls.MaterialTextBox2();
            boxShow = new MaterialSkin.Controls.MaterialCheckbox();
            SuspendLayout();
            // 
            // GetStarted
            // 
            GetStarted.AutoSize = true;
            GetStarted.BackColor = Color.Transparent;
            GetStarted.FlatStyle = FlatStyle.Flat;
            GetStarted.Font = new Font("Lucida Sans", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            GetStarted.ForeColor = Color.Black;
            GetStarted.Location = new Point(102, 79);
            GetStarted.Name = "GetStarted";
            GetStarted.Size = new Size(143, 27);
            GetStarted.TabIndex = 0;
            GetStarted.Text = "Get Started";
            // 
            // Username
            // 
            Username.AutoSize = true;
            Username.BackColor = Color.Transparent;
            Username.FlatStyle = FlatStyle.Flat;
            Username.Font = new Font("Lucida Sans", 12.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Username.ForeColor = Color.Black;
            Username.Location = new Point(30, 135);
            Username.Name = "Username";
            Username.Size = new Size(98, 19);
            Username.TabIndex = 2;
            Username.Text = "Username:";
            // 
            // Password
            // 
            Password.AutoSize = true;
            Password.BackColor = Color.Transparent;
            Password.FlatStyle = FlatStyle.Flat;
            Password.Font = new Font("Lucida Sans", 12.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Password.ForeColor = Color.Black;
            Password.Location = new Point(30, 222);
            Password.Name = "Password";
            Password.Size = new Size(95, 19);
            Password.TabIndex = 4;
            Password.Text = "Password:";
            // 
            // lblError
            // 
            lblError.BackColor = Color.Transparent;
            lblError.FlatStyle = FlatStyle.Flat;
            lblError.Font = new Font("Segoe UI Semibold", 7.75F, FontStyle.Bold);
            lblError.ForeColor = Color.IndianRed;
            lblError.Location = new Point(30, 337);
            lblError.Name = "lblError";
            lblError.Size = new Size(283, 54);
            lblError.TabIndex = 7;
            // 
            // btnLogin
            // 
            btnLogin.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnLogin.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnLogin.Depth = 0;
            btnLogin.HighEmphasis = true;
            btnLogin.Icon = null;
            btnLogin.Location = new Point(249, 397);
            btnLogin.Margin = new Padding(4, 6, 4, 6);
            btnLogin.MouseState = MaterialSkin.MouseState.HOVER;
            btnLogin.Name = "btnLogin";
            btnLogin.NoAccentTextColor = Color.Empty;
            btnLogin.Size = new Size(64, 36);
            btnLogin.TabIndex = 8;
            btnLogin.Text = "Login";
            btnLogin.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnLogin.UseAccentColor = false;
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // txtUsername
            // 
            txtUsername.AnimateReadOnly = false;
            txtUsername.BackgroundImageLayout = ImageLayout.None;
            txtUsername.CharacterCasing = CharacterCasing.Normal;
            txtUsername.Depth = 0;
            txtUsername.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtUsername.HideSelection = true;
            txtUsername.Hint = "Enter username (E.g. ahm4dd)";
            txtUsername.LeadingIcon = null;
            txtUsername.Location = new Point(30, 157);
            txtUsername.MaxLength = 32767;
            txtUsername.MouseState = MaterialSkin.MouseState.OUT;
            txtUsername.Name = "txtUsername";
            txtUsername.PasswordChar = '\0';
            txtUsername.PrefixSuffixText = null;
            txtUsername.ReadOnly = false;
            txtUsername.RightToLeft = RightToLeft.No;
            txtUsername.SelectedText = "";
            txtUsername.SelectionLength = 0;
            txtUsername.SelectionStart = 0;
            txtUsername.ShortcutsEnabled = true;
            txtUsername.Size = new Size(283, 48);
            txtUsername.TabIndex = 9;
            txtUsername.TabStop = false;
            txtUsername.TextAlign = HorizontalAlignment.Left;
            txtUsername.TrailingIcon = null;
            txtUsername.UseSystemPasswordChar = false;
            // 
            // lnkRegister
            // 
            lnkRegister.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            lnkRegister.BackColor = Color.SpringGreen;
            lnkRegister.BackgroundImageLayout = ImageLayout.None;
            lnkRegister.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            lnkRegister.Depth = 0;
            lnkRegister.HighEmphasis = true;
            lnkRegister.Icon = null;
            lnkRegister.Location = new Point(30, 397);
            lnkRegister.Margin = new Padding(4, 6, 4, 6);
            lnkRegister.MouseState = MaterialSkin.MouseState.HOVER;
            lnkRegister.Name = "lnkRegister";
            lnkRegister.NoAccentTextColor = Color.Empty;
            lnkRegister.Size = new Size(129, 36);
            lnkRegister.TabIndex = 10;
            lnkRegister.Text = "Register Here";
            lnkRegister.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            lnkRegister.UseAccentColor = false;
            lnkRegister.UseVisualStyleBackColor = false;
            lnkRegister.Click += lnkRegister_Click;
            // 
            // txtPassword
            // 
            txtPassword.AnimateReadOnly = false;
            txtPassword.BackgroundImageLayout = ImageLayout.None;
            txtPassword.CharacterCasing = CharacterCasing.Normal;
            txtPassword.Depth = 0;
            txtPassword.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtPassword.HideSelection = true;
            txtPassword.Hint = "Enter password (E.g. securePass1)";
            txtPassword.LeadingIcon = null;
            txtPassword.Location = new Point(30, 249);
            txtPassword.MaxLength = 32767;
            txtPassword.MouseState = MaterialSkin.MouseState.OUT;
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.PrefixSuffixText = null;
            txtPassword.ReadOnly = false;
            txtPassword.RightToLeft = RightToLeft.No;
            txtPassword.SelectedText = "";
            txtPassword.SelectionLength = 0;
            txtPassword.SelectionStart = 0;
            txtPassword.ShortcutsEnabled = true;
            txtPassword.Size = new Size(283, 48);
            txtPassword.TabIndex = 11;
            txtPassword.TabStop = false;
            txtPassword.TextAlign = HorizontalAlignment.Left;
            txtPassword.TrailingIcon = null;
            txtPassword.UseSystemPasswordChar = false;
            txtPassword.KeyDown += txtPassword_KeyDown;
            // 
            // boxShow
            // 
            boxShow.AutoSize = true;
            boxShow.BackColor = Color.Transparent;
            boxShow.Depth = 0;
            boxShow.Location = new Point(30, 300);
            boxShow.Margin = new Padding(0);
            boxShow.MouseLocation = new Point(-1, -1);
            boxShow.MouseState = MaterialSkin.MouseState.HOVER;
            boxShow.Name = "boxShow";
            boxShow.ReadOnly = false;
            boxShow.Ripple = true;
            boxShow.Size = new Size(149, 37);
            boxShow.TabIndex = 12;
            boxShow.Text = "Show Password";
            boxShow.UseVisualStyleBackColor = false;
            boxShow.CheckedChanged += boxShow_CheckedChanged;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Menu;
            ClientSize = new Size(336, 454);
            Controls.Add(boxShow);
            Controls.Add(txtPassword);
            Controls.Add(lnkRegister);
            Controls.Add(txtUsername);
            Controls.Add(btnLogin);
            Controls.Add(lblError);
            Controls.Add(Password);
            Controls.Add(Username);
            Controls.Add(GetStarted);
            MaximizeBox = false;
            Name = "LoginForm";
            Sizable = false;
            Text = "LoginForm";
            Load += LoginForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label GetStarted;
        private Label Username;
        private Label Password;
        private Label lblError;
        private MaterialSkin.Controls.MaterialButton btnLogin;
        private MaterialSkin.Controls.MaterialTextBox2 txtUsername;
        private MaterialSkin.Controls.MaterialButton lnkRegister;
        private MaterialSkin.Controls.MaterialTextBox2 txtPassword;
        private MaterialSkin.Controls.MaterialCheckbox boxShow;
    }
}
