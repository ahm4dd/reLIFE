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

        private readonly User _currentUser;             // Holds the logged-in user details
        private readonly EventService _eventService;     // Service for handling event operations
        private readonly CategoryService _categoryService; // Service for handling category operations

        // --- Fields for UI State and Data ---

        private List<Category> _userCategories = new List<Category>(); // Cache categories for dropdowns/filtering
        private List<Event> _currentEvents = new List<Event>();       // Cache events currently displayed


        // Example: Could add fields for current view type (Day, Week, Month), selected date, etc.
        // private ViewType _currentView = ViewType.Month; // Assuming ViewType enum exists
        private DateTime _selectedDate = DateTime.Today;

        // --- Constructor ---

        /// <summary>
        /// Initializes a new instance of the MainForm.
        /// </summary>
        /// <param name="currentUser">The authenticated user who logged in.</param>
        /// <param name="eventService">The service for event management.</param>
        /// <param name="categoryService">The service for category management.</param>
        public MainForm(User currentUser, EventService eventService, CategoryService categoryService)
        {
            InitializeComponent();

            // --- Store Dependencies ---
            // Perform null checks to ensure dependencies are provided
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));

            // Add other constructor initialization if needed (e.g., setting initial UI state variables)
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
