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
            txtUsernameReg = new MaterialSkin.Controls.MaterialTextBox2();
            txtPasswordReg = new MaterialSkin.Controls.MaterialTextBox2();
            btnRegister = new MaterialSkin.Controls.MaterialButton();
            btnCancelReg = new MaterialSkin.Controls.MaterialButton();
            txtConfirmPasswordReg = new MaterialSkin.Controls.MaterialTextBox2();
            txtEmailReg = new MaterialSkin.Controls.MaterialTextBox2();
            lblErrorReg = new MaterialSkin.Controls.MaterialLabel();
            materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            materialLabel4 = new MaterialSkin.Controls.MaterialLabel();
            SuspendLayout();
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
            txtUsernameReg.Location = new Point(27, 108);
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
            txtPasswordReg.Location = new Point(27, 262);
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
            // btnRegister
            // 
            btnRegister.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnRegister.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnRegister.Depth = 0;
            btnRegister.HighEmphasis = true;
            btnRegister.Icon = null;
            btnRegister.Location = new Point(217, 474);
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
            btnCancelReg.Location = new Point(27, 474);
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
            txtConfirmPasswordReg.Location = new Point(27, 345);
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
            txtEmailReg.Location = new Point(27, 185);
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
            // lblErrorReg
            // 
            lblErrorReg.AutoSize = true;
            lblErrorReg.Depth = 0;
            lblErrorReg.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblErrorReg.Location = new Point(28, 396);
            lblErrorReg.MouseState = MaterialSkin.MouseState.HOVER;
            lblErrorReg.Name = "lblErrorReg";
            lblErrorReg.Size = new Size(1, 0);
            lblErrorReg.TabIndex = 23;
            // 
            // materialLabel1
            // 
            materialLabel1.AutoSize = true;
            materialLabel1.Depth = 0;
            materialLabel1.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel1.Location = new Point(28, 81);
            materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel1.Name = "materialLabel1";
            materialLabel1.Size = new Size(76, 19);
            materialLabel1.TabIndex = 24;
            materialLabel1.Text = "Username:";
            // 
            // materialLabel2
            // 
            materialLabel2.AutoSize = true;
            materialLabel2.Depth = 0;
            materialLabel2.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel2.Location = new Point(27, 160);
            materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel2.Name = "materialLabel2";
            materialLabel2.Size = new Size(45, 19);
            materialLabel2.TabIndex = 25;
            materialLabel2.Text = "Email:";
            // 
            // materialLabel3
            // 
            materialLabel3.AutoSize = true;
            materialLabel3.Depth = 0;
            materialLabel3.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel3.Location = new Point(27, 236);
            materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel3.Name = "materialLabel3";
            materialLabel3.Size = new Size(75, 19);
            materialLabel3.TabIndex = 26;
            materialLabel3.Text = "Password:";
            // 
            // materialLabel4
            // 
            materialLabel4.AutoSize = true;
            materialLabel4.Depth = 0;
            materialLabel4.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel4.Location = new Point(27, 315);
            materialLabel4.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel4.Name = "materialLabel4";
            materialLabel4.Size = new Size(136, 19);
            materialLabel4.TabIndex = 27;
            materialLabel4.Text = "Confirm Password:";
            // 
            // RegistrationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(334, 536);
            Controls.Add(materialLabel4);
            Controls.Add(materialLabel3);
            Controls.Add(materialLabel2);
            Controls.Add(materialLabel1);
            Controls.Add(lblErrorReg);
            Controls.Add(txtEmailReg);
            Controls.Add(txtConfirmPasswordReg);
            Controls.Add(btnCancelReg);
            Controls.Add(btnRegister);
            Controls.Add(txtPasswordReg);
            Controls.Add(txtUsernameReg);
            MaximizeBox = false;
            Name = "RegistrationForm";
            Sizable = false;
            Text = "RegistrationForm";
            Load += RegistrationForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MaterialSkin.Controls.MaterialTextBox2 txtUsernameReg;
        private MaterialSkin.Controls.MaterialTextBox2 txtPasswordReg;
        private MaterialSkin.Controls.MaterialButton btnRegister;
        private MaterialSkin.Controls.MaterialButton btnCancelReg;
        private MaterialSkin.Controls.MaterialTextBox2 txtConfirmPasswordReg;
        private MaterialSkin.Controls.MaterialTextBox2 txtEmailReg;
        private MaterialSkin.Controls.MaterialLabel lblErrorReg;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialLabel materialLabel4;
    }
}