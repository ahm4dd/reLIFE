using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using reLIFE.BusinessLogic.Services;
using reLIFE.Core.Models;


namespace reLIFE.WinFormsUI.Forms
{
    public partial class MainForm : Form
    {
        // --- Private Fields for Dependencies ---
        // These services and the user are provided by the constructor (passed from login process)

        private readonly User _currentUser;
        private readonly EventService _eventService;
        private readonly CategoryService _categoryService;
        private readonly ReminderService _reminderService; // *** ADDED Dependency ***

        // --- UI State & Data ---
        private List<Category> _userCategories = new List<Category>();
        private List<Event> _currentEvents = new List<Event>();
        private DateTime _selectedDate = DateTime.Today;
        // Add other state fields like _currentView, _selectedFilterCategories etc.

        // --- MODIFIED Constructor ---
        public MainForm(
            User currentUser,
            EventService eventService,
            CategoryService categoryService,
            ReminderService reminderService // *** ADDED Parameter ***
            )
        {
            InitializeComponent();

            // --- Store Dependencies ---
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _reminderService = reminderService ?? throw new ArgumentNullException(nameof(reminderService)); // *** Store ReminderService ***

            // --- Initial UI Setup ---
            // Setup ListView columns, Calendar settings etc. here if not done in Designer
            //SetupListViewColumns(); // Example call
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
