using MaterialSkin;
using MaterialSkin.Controls;
using reLIFE.BusinessLogic.Services;
using reLIFE.BusinessLogic.Validators;
using reLIFE.Core.Models;
using reLIFE.WinFormsUI.Helpers; // ThemeHelper namespace
using System;
using System.Drawing; // Needed for Color
using System.Windows.Forms;

namespace reLIFE.WinFormsUI.Forms
{
    /// <summary>
    /// Form for managing user account settings (Email, Password, Theme),
    /// designed to be embedded within another container like MainForm.
    /// </summary>
    public partial class AccountSettingsForm : MaterialForm
    {
        private readonly User _currentUser;
        private readonly UserService _userService;
        // AuthService might still be needed if ChangePassword uses it directly
        // private readonly AuthService _authService;

        // Helper flags for theme combo box events
        private bool _isThemeLoading = false;

        public AccountSettingsForm(User currentUser, UserService userService/*, AuthService authService*/)
        {
            InitializeComponent();


            // Store dependencies
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));

            // Password fields init
            txtCurrentPassword.PasswordChar = '*';
            txtNewPassword.PasswordChar = '*';
            txtConfirmPassword.PasswordChar = '*';

            lblError.AutoSize = false;
            //lblError.Size = new Size(323, 72);
            lblError.FontType = MaterialSkinManager.fontType.Body2;
            lblError.HighEmphasis = true;
            lblError.UseAccent = true;
            lblError.Enabled = true;
        }

        private void AccountSettingsForm_Load(object sender, EventArgs e)
        {
            // Note: Controls like lblUsernameValue, txtEmail etc. are assumed
            // to be children of flpAccountContent now.
            this.Cursor = Cursors.WaitCursor;
            try
            {
                PopulateUserInfo();
                PopulateThemeComboBoxes();
                HookEventHandlers(); // Ensure this references controls correctly (e.g., this.btnUpdateEmail)
            }
            catch (Exception ex) { ShowError($"Error loading settings: {ex.Message}"); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void HookEventHandlers()
        {
            btnUpdateEmail.Click += BtnUpdateEmail_Click;
            btnChangePassword.Click += BtnChangePassword_Click;
            boxShowPasswords.CheckedChanged += BoxShowPasswords_CheckedChanged;
            cmbTheme.SelectedIndexChanged += ThemeOrSchemeChanged;
            cmbColorScheme.SelectedIndexChanged += ThemeOrSchemeChanged;
            // No Close button needed
        }

        private void PopulateUserInfo()
        {
            lblUsernameValue.Text = _currentUser.Username;
            txtEmail.Text = _currentUser.Email;
        }

        private void PopulateThemeComboBoxes()
        {
            _isThemeLoading = true; // Prevent events firing during load

            cmbTheme.Items.Clear();
            cmbTheme.Items.Add("Light");
            cmbTheme.Items.Add("Dark");
            cmbTheme.SelectedItem = MaterialSkinManager.Instance.Theme == MaterialSkinManager.Themes.DARK ? "Dark" : "Light";

            cmbColorScheme.Items.Clear();
            cmbColorScheme.Items.Add("Blue");
            cmbColorScheme.Items.Add("Indigo");
            cmbColorScheme.Items.Add("BlueGrey");
            cmbColorScheme.Items.Add("Green");
            cmbColorScheme.SelectedItem = ThemeHelper.GetCurrentSchemeName(); // Use helper to get current name

            _isThemeLoading = false;
        }

        // --- Event Handlers ---

        private void BtnUpdateEmail_Click(object? sender, EventArgs e)
        {
            string newEmail = txtEmail.Text.Trim();
            ShowError(null);
            var emailValidation = Validation.ValidateEmail(newEmail);
            if (!emailValidation.IsValid) { ShowError(emailValidation.ErrorMessage); SetActiveControl(txtEmail); return; }
            if (newEmail.Equals(_currentUser.Email, StringComparison.OrdinalIgnoreCase)) { ShowError("Email address is the same as the current one."); return; }

            SetProcessingState(true);
            try
            {
                if (_userService.UpdateUserEmail(_currentUser.Id, newEmail))
                {
                    _currentUser.Email = newEmail; // Update local copy
                    MessageBox.Show("Email address updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else { ShowError("Failed to update email."); }
            }
            catch (Exception ex) { HandleActionError("updating email", ex); } // Use common handler
            finally { SetProcessingState(false); }
        }

        private void BtnChangePassword_Click(object? sender, EventArgs e)
        {
            string currentPassword = txtCurrentPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            ShowError(null);

            if (string.IsNullOrEmpty(currentPassword)) { ShowError("Current password needed."); SetActiveControl(txtCurrentPassword); return; }
            if (string.IsNullOrEmpty(newPassword)) { ShowError("New password needed."); SetActiveControl(txtNewPassword); return; }
            if (newPassword != confirmPassword) { ShowError("New passwords do not match."); SetActiveControl(txtConfirmPassword); return; }
            var passwordValidation = Validation.ValidatePassword(newPassword);
            if (!passwordValidation.IsValid) { ShowError(passwordValidation.ErrorMessage); SetActiveControl(txtNewPassword); return; }
            if (currentPassword == newPassword) { ShowError("New password matches current."); SetActiveControl(txtNewPassword); return; }

            SetProcessingState(true);
            try
            {
                if (_userService.ChangePassword(_currentUser.Id, currentPassword, newPassword))
                {
                    MessageBox.Show("Password changed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCurrentPassword.Clear(); txtNewPassword.Clear(); txtConfirmPassword.Clear();
                    boxShowPasswords.Checked = false;
                }
                else { ShowError("Incorrect current password."); SetActiveControl(txtCurrentPassword); }
            }
            catch (Exception ex) { HandleActionError("changing password", ex); } // Use common handler
            finally { SetProcessingState(false); }
        }

        private void BoxShowPasswords_CheckedChanged(object? sender, EventArgs e)
        {
            bool show = boxShowPasswords.Checked;
            txtCurrentPassword.PasswordChar = '\0';
            txtNewPassword.PasswordChar = '\0';
            txtConfirmPassword.PasswordChar = '\0';
        }

        private void ThemeOrSchemeChanged(object? sender, EventArgs e)
        {
            if (_isThemeLoading) return; // Don't apply during initial population
            ApplySelectedTheme();
        }

        // Removed BtnClose_Click - Form is embedded

        // --- Helper Methods ---

        private void ApplySelectedTheme()
        {
            try
            {
                if (cmbTheme.SelectedItem == null || cmbColorScheme.SelectedItem == null) return; // Not ready yet

                MaterialSkinManager.Themes selectedTheme = cmbTheme.SelectedItem.ToString() == "Dark"
                                                           ? MaterialSkinManager.Themes.DARK
                                                           : MaterialSkinManager.Themes.LIGHT;
                string schemeName = cmbColorScheme.SelectedItem.ToString();
                ColorScheme selectedScheme = ThemeHelper.GetSchemeByName(schemeName);

                ThemeHelper.ApplyTheme(selectedTheme, selectedScheme, schemeName);

                // Optional: Save persistence (requires Settings)
                // Properties.Settings.Default.ThemeName = cmbTheme.SelectedItem.ToString();
                // Properties.Settings.Default.SchemeName = schemeName;
                // Properties.Settings.Default.Save();
            }
            catch (Exception ex) { MessageBox.Show($"Error applying theme: {ex.Message}", "Theme Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void ShowError(string? message)
        {
            if (lblError != null) { lblError.Text = message ?? ""; lblError.Visible = !string.IsNullOrEmpty(message); }
            else if (!string.IsNullOrEmpty(message)) { MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void SetProcessingState(bool processing)
        {
            // Simplify - disable the whole form content area maybe?
            // Or disable specific controls
            foreach (Control c in this.Controls) // Iterate through top-level controls
            {
                if (c is MaterialCard card) // Disable cards if using them
                {
                    foreach (Control child in card.Controls) child.Enabled = !processing;
                }
                else
                {
                    c.Enabled = !processing;
                }
            }
            // Ensure error label is always enabled
            if (lblError != null) lblError.Enabled = true;

            this.Cursor = processing ? Cursors.WaitCursor : Cursors.Default;
            if (processing) Application.DoEvents();
        }

        private void SetActiveControl(Control control)
        {
            if (control != null && control.CanFocus)
            {
                control.Focus(); // Use Focus for MaterialControls
                if (control is MaterialTextBox mtb) { mtb.SelectAll(); }
                else if (control is MaterialTextBox2 mtb2) { mtb2.SelectAll(); }
            }
        }

        /// <summary>
        /// Centralized handling for errors from button actions.
        /// </summary>
        private void HandleActionError(string actionName, Exception ex)
        {
            Console.WriteLine($"ERROR {actionName}: {ex}"); // Log full exception
            string userMessage = $"An error occurred while {actionName}.";
            // Provide more specific user messages based on known exception types
            if (ex is InvalidOperationException || ex is ArgumentException)
            {
                userMessage = $"Error: {ex.Message}"; // Show specific validation/operation errors
            }
            else if (ex is ApplicationException)
            {
                userMessage = $"Database Error: Please try again later."; // More generic for DB issues
            }
            else
            {
                userMessage = $"An unexpected error occurred. Please try again."; // Generic for others
            }
            ShowError(userMessage);
        }
    }
}