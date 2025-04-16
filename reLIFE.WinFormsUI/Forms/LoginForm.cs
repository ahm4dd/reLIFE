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
    public partial class LoginForm : Form
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

            ShowError(null);

            // --- Use Validator ---
            var usernameValidation = Validation.ValidateUsername(username);
            if (!usernameValidation.IsValid) { ShowError(usernameValidation.ErrorMessage); txtUsername.Focus(); txtUsername.SelectAll(); return; }
            var passwordValidation = Validation.ValidatePassword(password);
            if (!passwordValidation.IsValid) { ShowError(passwordValidation.ErrorMessage); txtPassword.Focus(); txtPassword.SelectAll(); return; }

            // --- Attempt Login ---
            SetProcessingState(true);

            User? loggedInUser = null; // Variable to hold the user if successful

            try
            {
                loggedInUser = _authService.Login(username, password);

                if (loggedInUser != null)
                {
                    // --- Login Successful - Launch MainForm ---
                    LaunchMainApplication(loggedInUser); // Call helper to load next stage
                    this.Hide(); // Hide the login form after successfully launching main
                }
                else
                {
                    // --- Login Failed ---
                    ShowError("Invalid username or password.");
                    txtPassword.Focus();
                    txtPassword.SelectAll();
                    SetProcessingState(false); // Reset UI only on failure
                }
            }
            catch (ApplicationException appEx)
            {
                Console.WriteLine($"Login Application Exception: {appEx}");
                ShowError($"Login Error: {appEx.Message}");
                SetProcessingState(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Unexpected Exception: {ex}");
                ShowError("An unexpected error occurred. Please try again.");
                SetProcessingState(false);
            }
        }

        private void lnkRegister_Click(object sender, EventArgs e)
        {
            // This part is tricky now. The register form still needs the AuthService,
            // but maybe shouldn't be modal *blocking* the login form which is the
            // main app form for now. Showing it non-modally might be better.
            var registrationForm = new RegistrationForm(_authService);
            registrationForm.Show(this); // Show non-modally, owned by Login Form
            // Or use ShowDialog if blocking behaviour is ok.
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (!btnLogin.Enabled) return;
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; btnLogin.PerformClick(); }
        }

        private void LaunchMainApplication(User loggedInUser)
        {
            // We need the connection string again here to create other repos/services
            try
            {
                string connectionString = DbHelper.GetConnectionString(); // Get it again

                // Create Remaining Dependencies
                var categoryRepository = new CategoryRepository(connectionString);
                var eventRepository = new EventRepository(connectionString);
                var categoryService = new CategoryService(categoryRepository);
                var eventService = new EventService(eventRepository, categoryRepository);

                // Create and Show MainForm
                var mainForm = new MainForm(loggedInUser, eventService, categoryService);

                // IMPORTANT: Handle closing - When MainForm closes, exit the whole app.
                mainForm.FormClosed += MainForm_FormClosed;

                mainForm.Show();
                // Don't call this.Hide() here - do it AFTER calling Show()
            }
            catch (Exception ex)
            {
                // Log details
                Console.WriteLine($"ERROR launching main application: {ex}");
                MessageBox.Show($"Failed to load the main application.\nError: {ex.Message}\n\nThe application might need to close.",
                                "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetProcessingState(false); // Re-enable login form UI maybe?
                                           // Or Application.Exit(); might be safer here
                Application.Exit();
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
            lnkRegister.Enabled = !processing;
            this.Cursor = processing ? Cursors.WaitCursor : Cursors.Default;
            Application.DoEvents(); // Try to force UI update
        }

        private void ShowError(string? message)
        {
            lblError.Text = message ?? "";
            lblError.Visible = !string.IsNullOrEmpty(message);
        }
    }
}
