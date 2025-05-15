using MaterialSkin;
using MaterialSkin.Controls;
using reLIFE.BusinessLogic.Repositories; // Needed for passing Archive Repo
using reLIFE.BusinessLogic.Services;
using reLIFE.Core.Models;
using System;
using System.Windows.Forms;

namespace reLIFE.WinFormsUI.Forms
{
    // Assuming you are using standard WinForms Form
    // If using MaterialSkin, change inheritance to MaterialForm
    public partial class MainForm : MaterialForm
    {
        // --- Dependencies (Received via Constructor) ---
        private readonly User _currentUser;
        private readonly EventService _eventService;
        private readonly CategoryService _categoryService;
        private readonly ReminderService _reminderService;
        private readonly AuthService _authService;
        private readonly UserService _userService;
        private readonly ArchivedEventRepository _archivedEventRepository; // Passed directly to Archive view

        // --- Constructor ---
        public MainForm(
            User currentUser,
            EventService eventService,
            CategoryService categoryService,
            ReminderService reminderService,
            AuthService authService,
            UserService userService,
            ArchivedEventRepository archivedEventRepository
            )
        {
            InitializeComponent();

            // *** ADD FORM TO MANAGER HERE (Only Once) ***
            MaterialSkinManager.Instance.AddFormToManage(this);

            // *** Parent Panel DOES NOT SCROLL ***
            pnlContent.AutoScroll = false;

            // Store dependencies
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            // ... (store other dependencies) ...
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _reminderService = reminderService ?? throw new ArgumentNullException(nameof(reminderService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _archivedEventRepository = archivedEventRepository ?? throw new ArgumentNullException(nameof(archivedEventRepository));


            // --- Remove button AutoSize overrides if not needed ---
            dashboardPanel.BackColor = Color.FromArgb(64, 64, 64);
            pictureBox1.BackColor = Color.Transparent;
            btnAccountSettings.AutoSize = false;
            btnCalendarView.AutoSize = false;
            btnLogout.AutoSize = false;
            btnViewArchive.AutoSize = false;
            btnViewReminders.AutoSize = false;
            btnManageCategories.AutoSize = false;
            btnAccountSettings.Size = new Size(158, 36);
            btnCalendarView.Size = new Size(158, 36);
            btnLogout.Size = new Size(158, 36);
            btnViewArchive.Size = new Size(158, 36);
            btnViewReminders.Size = new Size(158, 36);
            btnManageCategories.Size = new Size(158, 36);
            // btnAccountSettings.AutoSize = false; // etc. - remove unless specific sizing needed

            UpdateWindowTitle();
        }

        // --- Form Load ---
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load the default view (Calendar) when the form loads
            LoadCalendarView();
            this.ActiveControl = pnlContent;
        }

        // --- Navigation Button Event Handlers ---

        private void btnCalendarView_Click(object sender, EventArgs e)
        {
            // Load the main scheduling view
            LoadCalendarView();
        }

        private void btnAccountSettings_Click(object sender, EventArgs e)
        {
            // Instead of loading a UserControl, create and show the modal Form
            // Pass the current user and the UserService
            LoadControl(new AccountSettingsForm(_currentUser, _userService));
        }


        private void btnViewArchive_Click(object sender, EventArgs e)
        {
            // Load the archive viewing control
            LoadControl(new ArchiveViewForm(_currentUser, _archivedEventRepository, _eventService, _categoryService, _reminderService));
        }

        //private void btnViewReminders_Click(object sender, EventArgs e)
        //{
        //    // Load the reminder viewing control
        //    LoadControl(new ReminderListControl(_currentUser, _reminderService, _eventService));
        //}

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MaterialMessageBox.Show("Are you sure you want to logout?\nIf you click yes, you'll be brought back to the login form.", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                // Use DialogResult.Abort as the signal to Program.cs to handle logout/restart
                this.DialogResult = DialogResult.Abort;
                this.Close(); // Close this MainForm
            }
        }

        // --- Helper Methods ---

        /// <summary>
        /// Loads the primary Calendar/Event view user control into the content panel.
        /// </summary>
        private void LoadCalendarView()
        {
            // Instantiate CalendarViewForm, passing all services it needs
            LoadControl(new CalendarViewForm(_currentUser, _eventService, _categoryService, _reminderService));
        }

        /// <summary>
        /// Clears the main content panel and loads the specified user control into it.
        /// Handles disposing of the previous control if necessary.
        /// </summary>
        /// <param name="controlToLoad">The UserControl instance to load.</param>
        // Inside MainForm.cs
        // Inside MainForm.cs
        private void LoadControl(Form formToLoad)
        {
            if (pnlContent == null) return;
            this.Cursor = Cursors.WaitCursor;
            pnlContent.SuspendLayout(); // Suspend panel layout

            // Dispose previous
            if (pnlContent.Controls.Count > 0) { var cTR = pnlContent.Controls.OfType<Control>().ToList(); foreach (var c in cTR) { pnlContent.Controls.Remove(c); c.Dispose(); } }
            pnlContent.Controls.Clear();

            // Reset panel scroll properties (belt and braces)
            pnlContent.AutoScroll = false;
            pnlContent.AutoScrollMinSize = Size.Empty;
            pnlContent.AutoScrollPosition = Point.Empty;


            if (formToLoad != null)
            {
                formToLoad.TopLevel = false;
                formToLoad.FormBorderStyle = FormBorderStyle.None;
                formToLoad.Dock = DockStyle.Fill; // *** FORM FILLS PANEL ***
                formToLoad.AutoScroll = false;    // *** FORM DOES NOT SCROLL ***
                formToLoad.AutoSize = false;     // *** FORM DOES NOT AUTOSIZE ***

                var skinManager = MaterialSkinManager.Instance;
                skinManager.AddFormToManage((MaterialForm)formToLoad); // Manage theme

                pnlContent.Controls.Add(formToLoad); // Add form to panel
                formToLoad.Show();                   // Show the embedded form
                formToLoad.BringToFront();
            }

            pnlContent.ResumeLayout(true); // Resume panel layout
            pnlContent.PerformLayout();
            this.Cursor = Cursors.Default;
        }


        /// <summary>
        /// Updates the main form's title bar.
        /// </summary>
        private void UpdateWindowTitle()
        {
            this.Text = $"reLIFE Dashboard - [{_currentUser.Username}]";
        }

        private void btnManageCategories_Click(object sender, EventArgs e)
        {
            LoadControl(new CategoryManagerForm(_currentUser, _categoryService));
        }

        private void btnViewReminders_Click(object sender, EventArgs e)
        {
            LoadControl(new ReminderListViewForm(_currentUser, _reminderService, _eventService, _categoryService));
        }


        // --- Form Closing ---
        // Ensure that if the user closes this form via the 'X' button,
        // the application exits cleanly, similar to cancelling the login form.
        // The DialogResult might be 'Cancel' by default if closed via 'X'.
        // Program.cs already handles DialogResult != OK && != Retry by exiting.
        // If DialogResult.Abort is set (by logout), Program.cs handles restart.
        // No specific FormClosing code needed here unless more complex cleanup is required.
    }
}