using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.Data.SqlClient;
using reLIFE.BusinessLogic.Services;
using reLIFE.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;



namespace reLIFE.WinFormsUI.Forms
{
    public partial class CalendarViewForm : MaterialForm
    {
        // --- Dependencies ---
        private readonly User _currentUser;
        private readonly EventService _eventService;
        private readonly CategoryService _categoryService;
        private readonly ReminderService _reminderService;

        // --- UI State & Data ---
        private List<Category> _userCategories = new List<Category>();
        private List<Event> _allEventsForRange = new List<Event>();
        private bool _blockFilterEvent = false; // Flag to prevent event recursion

        public CalendarViewForm(
            User currentUser,
            EventService eventService,
            CategoryService categoryService,
            ReminderService reminderService)
        {
            InitializeComponent();

            // Form settings (as set by LoadControl, but good practice)
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;     // Form fills its container (pnlContent)
            this.AutoScroll = false;        // Form does NOT scroll
            this.AutoSize = false;         // Form does NOT autosize

            // --- REMOVED MaterialSkinManager.AddFormToManage(this); ---
            // --- REMOVED local Theme/Scheme setting ---

            // Store dependencies
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _reminderService = reminderService ?? throw new ArgumentNullException(nameof(reminderService));

            // Initial Setup
            dtpSelectedDate.Value = DateTime.Today;
            UpdateSelectedDateLabel();

            // Hook Apply button if it exists
            if (this.btnApplyFilters != null) this.btnApplyFilters.Click += btnApplyFilters_Click;
        }

        // --- Control Load ---
        private void CalendarViewForm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                LoadCategories();
                PopulateCategoryFilters(checkAll: true); // Populate and check all initially
                LoadAndDisplayEvents(); // Initial load
            }
            catch (Exception ex) { HandleLoadError("loading calendar data", ex); }
            finally { this.Cursor = Cursors.Default; }
        }

        // --- Event Handlers ---
        private void dtpSelectedDate_ValueChanged(object sender, EventArgs e)
        {
            UpdateSelectedDateLabel();
            LoadAndDisplayEvents(); // Reload and redisplay for new date
        }

        // Event handler for the Apply Filters button
        private void btnApplyFilters_Click(object sender, EventArgs e)
        {
            FilterAndDisplayEvents(); // Trigger filtering based on current checkbox states
        }

        // Optional: Handler if using ItemCheck for immediate filtering (can be complex)
        // private void clbCategoryFilter_ItemCheck(object sender, ItemCheckEventArgs e)
        // {
        //     if (_blockFilterEvent) return; // Prevent recursion if needed
        //     // Filter *after* the check state has visually updated
        //     this.BeginInvoke((MethodInvoker)FilterAndDisplayEvents);
        // }

        private void btnAddEvent_Click(object sender, EventArgs e)
        {
            OpenEventForm(null);
        }

        // --- Data Loading and Display ---

        private void LoadAndDisplayEvents()
        {
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            try
            {
                LoadEventsForView();
                FilterAndDisplayEvents(); // Filter and display based on current filter state
            }
            catch (Exception ex) { HandleLoadError("refreshing events", ex); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void LoadCategories()
        {
            _userCategories = _categoryService.GetCategoriesByUser(_currentUser.Id);
        }

        // *** MODIFIED: Added 'checkAll' parameter ***
        private void PopulateCategoryFilters(bool checkAll = false)
        {
            _blockFilterEvent = true; // Block filter events during population
            clbCategoryFilter.Items.Clear();
            foreach (var category in _userCategories.OrderBy(c => c.Name))
            {
                MaterialCheckbox cb = new MaterialCheckbox { Text = category.Name, Checked = checkAll, Tag = category.Id, AutoSize = true };
                clbCategoryFilter.Items.Add(cb);
            }
            // Add uncategorized checkbox if needed
            // MaterialCheckbox cbUncat = new MaterialCheckbox { Text = "(Uncategorized)", Checked = checkAll, Tag = null, AutoSize = true };
            // clbCategoryFilter.Items.Add(cbUncat);
            _blockFilterEvent = false;
        }

        private void LoadEventsForView()
        {
            DateTime selectedDate = dtpSelectedDate.Value.Date;
            DateTime rangeStart = selectedDate;
            DateTime rangeEnd = selectedDate.AddDays(1);
            _allEventsForRange = _eventService.GetEventsForDateRange(_currentUser.Id, rangeStart, rangeEnd);
        }

        // Filter and Display Events (No change needed here from previous fix)
        private void FilterAndDisplayEvents()
        {
            // 1. Get Selected Category Filter Criteria
            var selectedCategoryIds = new HashSet<int?>();
            bool includeUncategorized = false; // For events with CategoryId = null

            foreach (var itemObject in clbCategoryFilter.Items)
            {
                if (itemObject is MaterialCheckbox cbItem && cbItem.Checked)
                {
                    if (cbItem.Tag is int categoryId)
                    {
                        selectedCategoryIds.Add(categoryId);
                        Console.WriteLine($"Filter: Added Category ID {categoryId} ({cbItem.Text})");
                    }
                    else if (cbItem.Tag == null) // Assuming Tag == null marks your "(Uncategorized)" item
                    {
                        includeUncategorized = true;
                        Console.WriteLine($"Filter: Including Uncategorized");
                    }
                }
            }

            // Determine if any specific filter is active.
            bool anyCheckboxIsChecked = selectedCategoryIds.Count > 0 || includeUncategorized;
            Console.WriteLine($"Filter: Any Checkbox Is Checked = {anyCheckboxIsChecked}");

            // 2. Filter Events
            var filteredEvents = new List<Event>();
            Console.WriteLine($"Filter: Processing {_allEventsForRange.Count} events from range.");

            foreach (Event evt in _allEventsForRange)
            {
                bool includeThisEvent = false;

                // Condition 1: If there are category filters available BUT NONE are checked by the user,
                // then show ALL events (including those with categories and those without).
                if (clbCategoryFilter.Items.Count > 0 && !anyCheckboxIsChecked)
                {
                    includeThisEvent = true; // SHOW ALL when no filters are actively selected
                    Console.WriteLine($"Filter: Including Event ID {evt.Id} (Title: '{evt.Title}') because NO filters are checked.");
                }
                // Condition 2: If there are NO category filters defined AT ALL, show all events.
                else if (clbCategoryFilter.Items.Count == 0)
                {
                    includeThisEvent = true;
                    Console.WriteLine($"Filter: Including Event ID {evt.Id} (Title: '{evt.Title}') because NO category filters exist.");
                }
                // Condition 3: Specific filters ARE active, so apply them.
                else
                {
                    if (evt.CategoryId != null && selectedCategoryIds.Contains(evt.CategoryId.Value))
                    {
                        includeThisEvent = true;
                        Console.WriteLine($"Filter: Including Event ID {evt.Id} (Title: '{evt.Title}') due to Category ID {evt.CategoryId.Value}");
                    }
                    else if (evt.CategoryId == null && includeUncategorized)
                    {
                        includeThisEvent = true;
                        Console.WriteLine($"Filter: Including Event ID {evt.Id} (Title: '{evt.Title}') because it's Uncategorized and filter is set.");
                    }
                }

                if (includeThisEvent)
                {
                    filteredEvents.Add(evt);
                }
                else
                {
                    Console.WriteLine($"Filter: EXCLUDING Event ID {evt.Id} (Title: '{evt.Title}', CategoryId: {evt.CategoryId?.ToString() ?? "NULL"})");
                }
            }

            // Order the results
            filteredEvents = filteredEvents.OrderBy(e => e.StartTime).ToList();
            Console.WriteLine($"Filter: Found {filteredEvents.Count} events after filtering.");

            // 3. Populate FlowLayoutPanel (Same as before)
            flpEvents.SuspendLayout();
            while (flpEvents.Controls.Count > 0) { flpEvents.Controls[0].Dispose(); }

            if (filteredEvents.Any())
            {
                foreach (var evt in filteredEvents) { MaterialCard card = CreateEventMaterialCard(evt); flpEvents.Controls.Add(card); }
            }
            else
            {
                MaterialLabel noEventsLabel = new MaterialLabel { Text = "No events match the current filters for the selected date.", HighEmphasis = false, Depth = 0, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, Margin = new Padding(20) };
                flpEvents.Controls.Add(noEventsLabel);
            }
            flpEvents.ResumeLayout();
        }
        // --- Event Card Creation (No change needed here) ---
        private MaterialCard CreateEventMaterialCard(Event evt)
        {
            MaterialCard card = new MaterialCard
            {
                Width = flpEvents.Width - 30, // Adjust padding if needed
                                              // UseAccentColor = false, // REMOVE - Card background comes from theme
                Margin = new Padding(10),
                Padding = new Padding(5),
                Tag = evt,
                AutoSize = true
            };
            // ... (TableLayoutPanel, colorStripe setup remains the same) ...
            TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, AutoSize = true, ColumnStyles = { new ColumnStyle(SizeType.Absolute, 8), new ColumnStyle(SizeType.Percent, 100F), new ColumnStyle(SizeType.Absolute, 95) } }; card.Controls.Add(layout); // Wider button col
            Panel colorStripe = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) }; Category? category = _userCategories.FirstOrDefault(c => c.Id == evt.CategoryId); try { colorStripe.BackColor = (category != null && !string.IsNullOrEmpty(category.ColorHex)) ? ColorTranslator.FromHtml(category.ColorHex) : Color.Transparent; } catch { colorStripe.BackColor = Color.Gray; }
            layout.Controls.Add(colorStripe, 0, 0);
            FlowLayoutPanel mainContentPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = false, AutoSize = true, Padding = new Padding(5, 0, 0, 0) }; layout.Controls.Add(mainContentPanel, 1, 0);


            MaterialLabel lblTitle = new MaterialLabel
            {
                Text = evt.Title,
                FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle1,
                HighEmphasis = true, // Good for titles
                AutoSize = true,
                MaximumSize = new Size(card.Width - 130, 0), // Adjust based on button column width
                UseAccent = true,
                Margin = new Padding(0, 0, 0, 3),
                // UseAccent = true, // ONLY if you specifically want title in accent color
            };
            mainContentPanel.Controls.Add(lblTitle);

            // ... (lblTime, lblCategory, lblDesc remain the same, ensure fontType is lowercase) ...
            MaterialLabel lblTime = new MaterialLabel { Text = evt.IsAllDay ? "All Day" : $"{evt.StartTime:HH:mm} - {evt.EndTime:HH:mm}", FontType = MaterialSkin.MaterialSkinManager.fontType.Caption, Depth = 1, HighEmphasis = false, AutoSize = true, Margin = new Padding(0, 0, 0, 3) }; mainContentPanel.Controls.Add(lblTime);
            MaterialLabel lblCategoryText = new MaterialLabel { Text = category?.Name ?? "Uncategorized", FontType = MaterialSkin.MaterialSkinManager.fontType.Caption, Depth = 1, HighEmphasis = false, AutoSize = true, Margin = new Padding(0, 0, 0, 5) }; if (category != null) lblCategoryText.ForeColor = colorStripe.BackColor; else lblCategoryText.ForeColor = Color.Gray; mainContentPanel.Controls.Add(lblCategoryText); // Renamed to avoid conflict
            if (!string.IsNullOrWhiteSpace(evt.Description)) { MaterialLabel lblDesc = new MaterialLabel { Text = evt.Description.Length > 60 ? evt.Description.Substring(0, 60) + "..." : evt.Description, FontType = MaterialSkin.MaterialSkinManager.fontType.Body2, Depth = 2, HighEmphasis = false, AutoSize = true, MaximumSize = new Size(mainContentPanel.Width - 10, 0), Margin = new Padding(0, 0, 0, 3) }; mainContentPanel.Controls.Add(lblDesc); }


            FlowLayoutPanel buttonPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(5, 0, 0, 0), Margin = new Padding(0), WrapContents = false, AutoSize = true };
            layout.Controls.Add(buttonPanel, 2, 0);

            // Button styling: Rely on Type and global ColorScheme.
            // Type = Text buttons often use accent color by default or can be styled via scheme.
            MaterialButton btnEdit = new MaterialButton { Text = "Edit", Tag = evt, Type = MaterialButton.MaterialButtonType.Text, /* UseAccentColor=true is often default for Text type */ Margin = new Padding(1), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, MinimumSize = new Size(80, 28) };
            btnEdit.UseAccentColor = true;
            btnEdit.Click += CardButton_Click; buttonPanel.Controls.Add(btnEdit);

            MaterialButton btnArchive = new MaterialButton { Text = "Archive", Tag = evt, Type = MaterialButton.MaterialButtonType.Text, Margin = new Padding(1), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, MinimumSize = new Size(80, 28) };
            btnArchive.Click += CardButton_Click; buttonPanel.Controls.Add(btnArchive);

            MaterialButton btnDelete = new MaterialButton { Text = "Delete", Tag = evt, Type = MaterialButton.MaterialButtonType.Text, Margin = new Padding(1), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, MinimumSize = new Size(80, 28) };
            // For delete, you might want a more distinct (but still theme-aware) color if not using icons
            // E.g., if your Accent is red: btnDelete.UseAccentColor = true;
            // Or if you have a "Warning" color in your scheme (custom scheme):
            // btnDelete.ForeColor = skinManager.ColorScheme.WarningColor; // Imaginary property
            btnDelete.Click += CardButton_Click; buttonPanel.Controls.Add(btnDelete);

            return card;
        }

        // --- Common Card Button Click Handler (Remains the Same) ---
        private void CardButton_Click(object? sender, EventArgs e) { if (sender is MaterialButton button && button.Tag is Event targetEvent) { if (button.Text == "Edit") { OpenEventForm(targetEvent); } else if (button.Text == "Archive") { ArchiveEvent(targetEvent); } else if (button.Text == "Delete") { DeleteEvent(targetEvent); } } }

        // --- Refactored Action Helpers (Remain the Same) ---
        private void DeleteEvent(Event eventToDelete) { var confirmResult = MessageBox.Show($"Are you sure you want to permanently delete the event '{eventToDelete.Title}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); if (confirmResult == DialogResult.Yes) { PerformActionWithRefresh(() => _eventService.DeleteEvent(eventToDelete.Id, _currentUser.Id), "deleting event", $"Event '{eventToDelete.Title}' deleted successfully."); } }
        private void ArchiveEvent(Event eventToArchive) { var confirmResult = MessageBox.Show($"Are you sure you want to archive the event '{eventToArchive.Title}'?", "Confirm Archive", MessageBoxButtons.YesNo, MessageBoxIcon.Question); if (confirmResult == DialogResult.Yes) { PerformActionWithRefresh(() => _eventService.ArchiveEvent(eventToArchive.Id, _currentUser.Id), "archiving event", $"Event '{eventToArchive.Title}' archived successfully."); } }

        // *** MODIFIED: OpenEventForm - Resets filters before refresh ***
        private void OpenEventForm(Event? eventToEdit)
        {
            // Get the global instance
            var skinManager = MaterialSkinManager.Instance;

            using (var eventForm = new EventForm(_currentUser, _eventService, _categoryService, _reminderService, eventToEdit))
            {
                skinManager.AddFormToManage(eventForm); // *** ADD Before ShowDialog ***
                DialogResult result = eventForm.ShowDialog(); // Show modally
                skinManager.RemoveFormToManage(eventForm); // *** REMOVE After Close ***

                if (result == DialogResult.OK)
                {
                    CheckAllCategoryFilters(true);
                    LoadAndDisplayEvents();
                }
            }
        }


        // --- UI Helper Methods ---
        private void UpdateSelectedDateLabel() { if (lblSelectedDateInfo != null) { lblSelectedDateInfo.Text = dtpSelectedDate.Value.ToString("D"); } }
        private void HandleLoadError(string actionDescription, Exception ex) { Console.WriteLine($"ERROR {actionDescription}: {ex}"); MessageBox.Show($"An error occurred while {actionDescription}.\nDetails: {ex.Message}", "Data Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        private void PerformActionWithRefresh(Func<bool> action, string actionName, string successMessage)
        {
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            try
            {
                bool success = action();
                if (success)
                {
                    // *** FIX: Ensure filters allow the remaining events to be seen ***
                    // Or simply reload with the current filters if preferred:
                    // LoadAndDisplayEvents();
                    // For consistency after Add/Edit, let's re-check all and refresh
                    CheckAllCategoryFilters(true);
                    LoadAndDisplayEvents();
                    // MessageBox.Show(successMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); // Optional success message
                }
                else
                {
                    MessageBox.Show($"Failed to perform action: {actionName}.", "Action Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadAndDisplayEvents(); // Refresh anyway
                }
            }
            catch (InvalidOperationException opEx) { MessageBox.Show($"Error {actionName}: {opEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            catch (ApplicationException appEx) { MessageBox.Show($"An error occurred while {actionName}: {appEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            catch (Exception ex) { MessageBox.Show($"An unexpected error occurred while {actionName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { this.Cursor = Cursors.Default; }
        }

        // *** NEW HELPER: To check/uncheck all category filter checkboxes ***
        private void CheckAllCategoryFilters(bool checkState)
        {
            _blockFilterEvent = true; // Prevent triggering filter updates during this change
            foreach (var itemObject in clbCategoryFilter.Items)
            {
                if (itemObject is MaterialCheckbox cbItem)
                {
                    cbItem.Checked = checkState;
                }
            }
            _blockFilterEvent = false;
        }

        private void lblSelectedDateInfo_Click(object sender, EventArgs e)
        {

        }
    }
}