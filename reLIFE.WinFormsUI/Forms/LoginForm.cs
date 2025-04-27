using MaterialSkin.Controls;
using Microsoft.Data.SqlClient;
using reLIFE.BusinessLogic.Data;
using reLIFE.BusinessLogic.Repositories;
using reLIFE.BusinessLogic.Services;
using reLIFE.BusinessLogic.Validators;
using reLIFE.Core.Models;
using reLIFE.WinFormsUI.Forms;
using System;
using System.Windows.Forms;

namespace reLIFE.WinFormsUI
{
    public partial class LoginForm : MaterialForm
    {
        // Service dependency (provided via constructor)
        private readonly AuthService _authService;

        // Property to hold the logged-in user
        public User? LoggedInUser { get; private set; }
        public LoginForm(AuthService authService)
        {
            InitializeComponent();
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtUsername;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            ShowError(null); // Clear previous errors

            // --- Use Validator ---
            var usernameValidation = Validation.ValidateUsername(username);
            if (!usernameValidation.IsValid)
            {
                ShowError(usernameValidation.ErrorMessage);
                SetActiveControl(txtUsername);
                return;
            }

            var passwordValidation = Validation.ValidatePassword(password);
            if (!passwordValidation.IsValid)
            {
                ShowError(passwordValidation.ErrorMessage);
                SetActiveControl(txtPassword);
                return;
            }

            // --- Attempt Login ---
            SetProcessingState(true); // Disable UI, show wait cursor

            try
            {
                // Call the synchronous Login method
                User? user = _authService.Login(username, password);

                if (user != null)
                {
                    // --- Login Successful ---
                    this.LoggedInUser = user;           // Store the user in the public property
                    this.DialogResult = DialogResult.OK; // Signal success to Program.cs
                    // The form will close automatically because DialogResult is set
                }
                else
                {
                    // --- Login Failed ---
                    ShowError("Invalid username or password.");
                    SetActiveControl(txtPassword);
                    SetProcessingState(false); // Re-enable UI on failure
                }
            }
            catch (ApplicationException appEx) // Catch errors from service/repo/dbhelper
            {
                Console.WriteLine($"Login Application Exception: {appEx}");
                ShowError($"Login Error: {appEx.Message}");
                SetProcessingState(false); // Re-enable UI
            }
            catch (Exception ex) // Catch unexpected errors
            {
                Console.WriteLine($"Login Unexpected Exception: {ex}");
                ShowError("An unexpected error occurred. Please try again.");
                SetProcessingState(false); // Re-enable UI
            }
            // If successful, form closes via DialogResult before reaching here.
            // If failed, SetProcessingState(false) was called in the relevant block.
        }

        private void lnkRegister_Click(object sender, EventArgs e)
        {
            // This part is tricky now. The register form still needs the AuthService,
            // but maybe shouldn't be modal *blocking* the login form which is the
            // main app form for now. Showing it non-modally might be better.
            var registrationForm = new RegistrationForm(_authService);
            registrationForm.Show(this); // Show non-modally, owned by Login 
            this.Hide();
            // Or use ShowDialog if blocking behaviour is ok.
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            // Only process if login button is enabled (i.e., not already processing)
            if (!btnLogin.Enabled) return;

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the 'ding' sound on Enter
                btnLogin.PerformClick();   // Simulate clicking the login button
            }
        }

        /// <summary>
        /// Handles the FormClosed event of the MainForm to exit the application.
        /// </summary>
        private void MainForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            // When the main form (launched from login) closes, exit the application
            // This prevents the hidden LoginForm from reappearing or hanging the process.
            Application.Exit();
        }

        private void SetProcessingState(bool processing)
        {
            txtUsername.Enabled = !processing;
            txtPassword.Enabled = !processing;
            btnLogin.Enabled = !processing;
            boxShow.Enabled = !processing; // Assuming 'boxShow' is the checkbox name
            lnkRegister.Enabled = !processing;
            this.Cursor = processing ? Cursors.WaitCursor : Cursors.Default;
            Application.DoEvents(); // Try to force UI update
        }

        private void ShowError(string? message)
        {
            // Assuming lblError is the name of your error Label control
            if (lblError != null) // Check if label exists
            {
                lblError.Text = message ?? "";
                lblError.Visible = !string.IsNullOrEmpty(message);
            }
            else // Fallback or alternative: MessageBox
            {
                if (!string.IsNullOrEmpty(message))
                {
                    MessageBox.Show(message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void boxShow_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = boxShow.Checked ? '\0' : '*';
        }

        private void SetActiveControl(Control control)
        {
            this.ActiveControl = control;
            if (control is TextBoxBase textBox)
            {
                textBox.SelectAll();
            }
        }
    }
}
