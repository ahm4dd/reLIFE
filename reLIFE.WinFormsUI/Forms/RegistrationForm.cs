using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using reLIFE.BusinessLogic.Services;
using reLIFE.BusinessLogic.Validators;
using reLIFE.Core.Models;

namespace reLIFE.WinFormsUI.Forms
{
    public partial class RegistrationForm : MaterialForm
    {
        private readonly AuthService _authService;
        public RegistrationForm(AuthService authService)
        {
            InitializeComponent();
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(
    Primary.Indigo800,
    Primary.Indigo900,
    Primary.Indigo500,
    Accent.Red200, // Light Blue accent provides nice contrast
    TextShade.WHITE
);

            materialLabel1.AutoSize = false;
            materialLabel1.Size = new Size(127, 25);
            materialLabel1.FontType = MaterialSkinManager.fontType.H6;
            materialLabel2.AutoSize = false;
            materialLabel2.Size = new Size(127, 25);
            materialLabel2.FontType = MaterialSkinManager.fontType.H6;
            materialLabel3.AutoSize = false;
            materialLabel3.Size = new Size(127, 25);
            materialLabel3.FontType = MaterialSkinManager.fontType.H6;
            materialLabel4.AutoSize = false;
            materialLabel4.Size = new Size(300, 25);
            materialLabel4.FontType = MaterialSkinManager.fontType.H6;
            
            lblErrorReg.AutoSize = false;
            lblErrorReg.Size = new Size(323, 72);
            lblErrorReg.FontType = MaterialSkinManager.fontType.Body2;
            lblErrorReg.HighEmphasis = true;
            lblErrorReg.UseAccent = true;
            lblErrorReg.Enabled = true;
        }

        // --- Event Handlers ---

        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtUsernameReg; // Set initial focus
        }

        // Register Button Click
        private void btnRegister_Click(object sender, EventArgs e)
        {
            // 1. Get Input
            string username = txtUsernameReg.Text.Trim();
            string email = txtEmailReg.Text.Trim();
            string password = txtPasswordReg.Text; // Don't trim password
            string confirmPassword = txtConfirmPasswordReg.Text;

            // 2. Clear Errors
            ShowError(null); // Hide/clear error label

            // 3. Client-Side Validation using Validation class
            var usernameValidation = Validation.ValidateUsername(username);
            if (!usernameValidation.IsValid)
            {
                ShowError(usernameValidation.ErrorMessage);
                SetActiveControl(txtUsernameReg);
                return;
            }

            var emailValidation = Validation.ValidateEmail(email);
            if (!emailValidation.IsValid)
            {
                ShowError(emailValidation.ErrorMessage);
                SetActiveControl(txtEmailReg);
                return;
            }

            var passwordValidation = Validation.ValidatePassword(password);
            if (!passwordValidation.IsValid)
            {
                ShowError(passwordValidation.ErrorMessage);
                SetActiveControl(txtPasswordReg);
                return;
            }

            // 4. Specific Registration Checks
            if (password != confirmPassword)
            {
                ShowError("Passwords do not match.");
                SetActiveControl(txtConfirmPasswordReg);
                txtConfirmPasswordReg.SelectAll();
                return;
            }

            // 5. Attempt Registration via Service
            SetProcessingState(true); // Disable UI, show wait cursor

            try
            {
                // Call the synchronous registration method
                User newUser = _authService.Register(username, email, password);

                // --- Registration Successful ---
                MessageBox.Show("Registration successful! You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK; // Indicate success (optional
                this.Close();                      // Close the registration form

            }
            catch (InvalidOperationException opEx) // Specific errors from AuthService (e.g., User/Email exists)
            {
                Console.WriteLine($"Registration Validation Failed: {opEx.Message}");
                ShowError(opEx.Message); // Show specific message to user
                SetProcessingState(false); // Re-enable UI
                // Decide which control to focus based on error message? Less reliable. Maybe username.
                SetActiveControl(txtUsernameReg);
            }
            catch (ArgumentException argEx) // Should be caught by local validation, but catch defensively
            {
                Console.WriteLine($"Registration Argument Exception: {argEx}");
                ShowError($"Invalid input: {argEx.Message}");
                SetProcessingState(false);
            }
            catch (ApplicationException appEx) // Catch errors from repository/database access
            {
                Console.WriteLine($"Registration Application Exception: {appEx}");
                ShowError($"An error occurred during registration: {appEx.Message}"); // Might be too technical, consider generic msg
                SetProcessingState(false);

            }
            catch (Exception ex)
            {  // Catch unexpected errors
                Console.WriteLine($"Registration Unexpected Exception: {ex}");
                ShowError("An unexpected error occurred during registration. Please try again later.");
                SetProcessingState(false);
            }
            // Note: UI state reset only happens if an error occurs.
            // If successful, the form closes before reaching here.
        }

        // Cancel Button Click
        private void btnCancelReg_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // Set result
            this.Close();                          // Close the form
        }

        // --- Helper Methods ---

        /// <summary>
        /// Displays or hides the error message label.
        /// </summary>
        /// <param name="message">The error message to display, or null/empty to hide.</param>
        private void ShowError(string? message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                lblErrorReg.Text = message;
                lblErrorReg.Visible = true;
            }
            else
            {
                lblErrorReg.Text = "";
                lblErrorReg.Visible = false;
            }
        }

        /// <summary>
        /// Sets the visual state during processing.
        /// </summary>
        private void SetProcessingState(bool processing)
        {
            txtUsernameReg.Enabled = !processing;
            txtEmailReg.Enabled = !processing;
            txtPasswordReg.Enabled = !processing;
            txtConfirmPasswordReg.Enabled = !processing;
            btnRegister.Enabled = !processing;
            btnCancelReg.Enabled = !processing; // Might want to leave cancel enabled? User choice.

            this.Cursor = processing ? Cursors.WaitCursor : Cursors.Default;
            Application.DoEvents(); // Force UI update
        }

        /// <summary>
        /// Safely sets the active control and selects its text.
        /// </summary>
        private void SetActiveControl(Control control)
        {
            this.ActiveControl = control;
            if (control is TextBoxBase textBox) // TextBoxBase covers TextBox
            {
                textBox.SelectAll();
            }
        }
    }
}