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
using reLIFE.Core.Models;


namespace reLIFE.WinFormsUI.Forms
{
    public partial class MainForm : MaterialForm
    {
        // --- Dependencies (Needs ALL services to pass down) ---
        private readonly User _currentUser;
        private readonly EventService _eventService;
        private readonly CategoryService _categoryService;
        private readonly ReminderService _reminderService;
        private readonly AuthService _authService;     // Needed for logout, password change form
        private readonly UserService _userService;     // *** NEW dependency for Account Settings ***
        // ArchivedEventRepository might be needed directly for ArchiveViewControl if EventService doesn't expose needed methods
        private readonly reLIFE.BusinessLogic.Repositories.ArchivedEventRepository _archivedEventRepository;


        // --- Constructor ---
        public MainForm(
            User currentUser,
            EventService eventService,
            CategoryService categoryService,
            ReminderService reminderService,
            AuthService authService,         // Add AuthService
            UserService userService,         // Add UserService
            reLIFE.BusinessLogic.Repositories.ArchivedEventRepository archivedEventRepository // Add Archive Repo
            )
        {
            InitializeComponent();

            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _reminderService = reminderService ?? throw new ArgumentNullException(nameof(reminderService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _archivedEventRepository = archivedEventRepository ?? throw new ArgumentNullException(nameof(archivedEventRepository));

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500,
                Accent.Red200, TextShade.WHITE);
            //UpdateWindowTitle();
        }

        // --- Form Load ---
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load the default view (Calendar) initially

            //LoadCalendarView();
        }

        // --- Navigation Button Handlers ---

        //private void btnCalendarView_Click(object sender, EventArgs e)
        //{
        //    LoadCalendarView();
        //}

        //private void btnAccountSettings_Click(object sender, EventArgs e)
        //{
        //    // Pass needed services to the UserControl constructor
        //    LoadControl(new AccountSettingsControl(_currentUser, _userService, _authService));
        //}

        //private void btnViewArchive_Click(object sender, EventArgs e)
        //{
        //    // Pass needed services/repositories
        //    LoadControl(new ArchiveViewControl(_currentUser, _archivedEventRepository)); // Pass repo directly for simplicity
        //}

        //private void btnViewReminders_Click(object sender, EventArgs e)
        //{
        //    // Pass needed services
        //    LoadControl(new ReminderListControl(_currentUser, _reminderService, _eventService)); // Needs EventService for context?
        //}

        //private void btnLogout_Click(object sender, EventArgs e)
        //{
        //    var confirm = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //    if (confirm == DialogResult.Yes)
        //    {
        //        // Signal Program.cs to restart the login process (or simply close)
        //        this.DialogResult = DialogResult.Abort; // Use Abort to signal logout
        //        this.Close();
        //    }
        //}

        //// --- Helper Methods ---

        ///// <summary>
        ///// Loads the primary Calendar/Event view user control.
        ///// </summary>
        //private void LoadCalendarView()
        //{
        //    // Pass needed services to the UserControl constructor
        //    LoadControl(new CalendarViewControl(_currentUser, _eventService, _categoryService, _reminderService));
        //}

        ///// <summary>
        ///// Clears the content panel and loads the specified user control.
        ///// </summary>
        ///// <param name="control">The UserControl instance to load.</param>
        //private void LoadControl(UserControl control)
        //{
        //    pnlContent.Controls.Clear(); // Remove previous control

        //    if (control != null)
        //    {
        //        control.Dock = DockStyle.Fill; // Make control fill the panel
        //        pnlContent.Controls.Add(control); // Add the new control
        //        control.BringToFront();
        //    }
        //}

        //private void UpdateWindowTitle()
        //{
        //    this.Text = $"reLIFE Dashboard - [{_currentUser.Username}]";
        //}

        // --- Optional: Methods called by UserControls (less common, prefer events) ---
        // Example: If CalendarViewControl needs to trigger opening EventForm globally
        // public void RequestOpenEventForm(Event eventToEdit = null) { /* ... */ }
    }
}