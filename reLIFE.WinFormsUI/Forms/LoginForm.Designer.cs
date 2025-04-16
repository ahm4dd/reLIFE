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
            txtUsername = new TextBox();
            Username = new Label();
            Password = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            lnkRegister = new Button();
            lblError = new Label();
            SuspendLayout();
            // 
            // GetStarted
            // 
            GetStarted.AutoSize = true;
            GetStarted.BackColor = Color.Transparent;
            GetStarted.FlatStyle = FlatStyle.Flat;
            GetStarted.Font = new Font("Lucida Sans", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            GetStarted.ForeColor = Color.FromArgb(192, 255, 255);
            GetStarted.Location = new Point(91, 39);
            GetStarted.Name = "GetStarted";
            GetStarted.Size = new Size(143, 27);
            GetStarted.TabIndex = 0;
            GetStarted.Text = "Get Started";
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.FromArgb(192, 255, 255);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsername.ForeColor = Color.FromArgb(0, 0, 64);
            txtUsername.Location = new Point(55, 137);
            txtUsername.MaxLength = 100;
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Enter username (E.g. ahm4dd)";
            txtUsername.Size = new Size(220, 23);
            txtUsername.TabIndex = 1;
            // 
            // Username
            // 
            Username.AutoSize = true;
            Username.BackColor = Color.Transparent;
            Username.FlatStyle = FlatStyle.Flat;
            Username.Font = new Font("Lucida Sans", 12.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Username.ForeColor = Color.FromArgb(192, 255, 255);
            Username.Location = new Point(52, 115);
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
            Password.ForeColor = Color.FromArgb(192, 255, 255);
            Password.Location = new Point(52, 188);
            Password.Name = "Password";
            Password.Size = new Size(95, 19);
            Password.TabIndex = 4;
            Password.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.FromArgb(192, 255, 255);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.ForeColor = Color.FromArgb(0, 0, 64);
            txtPassword.Location = new Point(55, 210);
            txtPassword.MaxLength = 999;
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.PlaceholderText = "Enter password (E.g. securePass1)";
            txtPassword.Size = new Size(220, 23);
            txtPassword.TabIndex = 3;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(192, 255, 255);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = Color.FromArgb(0, 0, 64);
            btnLogin.Location = new Point(238, 301);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(75, 25);
            btnLogin.TabIndex = 5;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // lnkRegister
            // 
            lnkRegister.BackColor = Color.FromArgb(0, 0, 64);
            lnkRegister.FlatStyle = FlatStyle.Popup;
            lnkRegister.ForeColor = Color.FromArgb(192, 255, 255);
            lnkRegister.Location = new Point(42, 239);
            lnkRegister.Name = "lnkRegister";
            lnkRegister.Size = new Size(170, 23);
            lnkRegister.TabIndex = 6;
            lnkRegister.Text = "No Account? Register here";
            lnkRegister.UseVisualStyleBackColor = false;
            lnkRegister.Click += lnkRegister_Click;
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.BackColor = Color.FromArgb(0, 0, 64);
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(55, 86);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 15);
            lblError.TabIndex = 7;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 0, 64);
            ClientSize = new Size(325, 338);
            Controls.Add(lblError);
            Controls.Add(lnkRegister);
            Controls.Add(btnLogin);
            Controls.Add(Password);
            Controls.Add(txtPassword);
            Controls.Add(Username);
            Controls.Add(txtUsername);
            Controls.Add(GetStarted);
            Name = "LoginForm";
            Text = "Form1";
            Load += LoginForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label GetStarted;
        private TextBox txtUsername;
        private Label Username;
        private Label Password;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button lnkRegister;
        private Label lblError;
    }
}
