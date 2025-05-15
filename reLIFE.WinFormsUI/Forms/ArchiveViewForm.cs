using MaterialSkin;
using MaterialSkin.Controls;
using reLIFE.BusinessLogic.Repositories; // For ArchivedEventRepository
using reLIFE.BusinessLogic.Services;    // For EventService (if restoring/deleting active events) & CategoryService
using reLIFE.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace reLIFE.WinFormsUI.Forms
{
    public partial class ArchiveViewForm : MaterialForm
    {
        // --- Dependencies ---
        private readonly User _currentUser;
        private readonly ArchivedEventRepository _archivedEventRepository;
        private readonly EventService _eventService; // Needed for Retrieve (unarchive) and Permanent Delete
        private readonly CategoryService _categoryService; // For displaying category names
        private readonly ReminderService _reminderService; // Add this field

        // --- UI State & Data ---
        private List<Category> _userCategories = new List<Category>();
        private List<ArchivedEvent> _allArchivedEvents = new List<ArchivedEvent>();

        public ArchiveViewForm(
    User currentUser,
    ArchivedEventRepository archivedEventRepository,
    EventService eventService,
    CategoryService categoryService,
    ReminderService reminderService) // Add ReminderService here
        {
            InitializeComponent();

            // Configure Form for Embedding
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            this.AutoScroll = false; // Form itself doesn't scroll, FlowLayoutPanel does

            // Store dependencies
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _archivedEventRepository = archivedEventRepository ?? throw new ArgumentNullException(nameof(archivedEventRepository));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _reminderService = reminderService ?? throw new ArgumentNullException(nameof(reminderService)); // Store it

            // Initialize MaterialSkin
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);

            // Initial Setup for optional date filters
            // dtpArchiveFrom.Value = DateTime.Today.AddMonths(-1); // Example: Default to last month
            // dtpArchiveTo.Value = DateTime.Today;
        }

        // --- Form Load ---
        private void ArchiveViewForm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                LoadCategories(); // Load for display on cards
                PopulateArchiveCategoryFilters(); // Populate category filter
                LoadAndDisplayArchivedEvents(); // Initial load
            }
            catch (Exception ex) { HandleLoadError("loading archived events", ex); }
            finally { this.Cursor = Cursors.Default; }
        }

        // --- Event Handlers ---
        private void btnApplyArchiveFilters_Click(object sender, EventArgs e) // If using date/other general filters
        {
            LoadAndDisplayArchivedEvents();
        }
        private void btnApplyArchiveCategoryFilter_Click(object sender, EventArgs e) // Specific to category
        {
            FilterAndDisplayArchivedEvents(); // Only re-filters and displays
        }


        // --- Data Loading and Display ---
        private void LoadAndDisplayArchivedEvents()
        {
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            try
            {
                LoadArchivedEventsFromRepo();    // Fetches into _allArchivedEvents
                FilterAndDisplayArchivedEvents(); // Filters and populates UI
            }
            catch (Exception ex) { HandleLoadError("refreshing archived events", ex); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void LoadCategories()
        {
            _userCategories = _categoryService.GetCategoriesByUser(_currentUser.Id);
        }

        private void PopulateArchiveCategoryFilters(bool checkAll = true)
        {
            clbArchiveCategoryFilter.Items.Clear();
            foreach (var category in _userCategories.OrderBy(c => c.Name))
            {
                MaterialCheckbox cb = new MaterialCheckbox { Text = category.Name, Checked = checkAll, Tag = category.Id, AutoSize = true };
                clbArchiveCategoryFilter.Items.Add(cb);
            }
            // Optional: Add uncategorized item
            // MaterialCheckbox cbUncat = new MaterialCheckbox { Text = "(Uncategorized)", Checked = checkAll, Tag = null, AutoSize = true };
            // clbArchiveCategoryFilter.Items.Add(cbUncat);
        }

        private void LoadArchivedEventsFromRepo()
        {
            // TODO: Implement date range filtering if dtpArchiveFrom/To are used
            // For now, loads all for user, or a recent limit
            _allArchivedEvents = _archivedEventRepository.GetArchivedEventsByUser(_currentUser.Id, limit: 100); // Example limit
        }

        private void FilterAndDisplayArchivedEvents()
        {
            var selectedCategoryIds = new HashSet<int?>();
            bool includeUncategorized = false;
            foreach (var itemObject in clbArchiveCategoryFilter.Items)
            {
                if (itemObject is MaterialCheckbox cbItem && cbItem.Checked)
                {
                    if (cbItem.Tag is int categoryId) { selectedCategoryIds.Add(categoryId); }
                    else if (cbItem.Tag == null) { includeUncategorized = true; }
                }
            }
            bool showAllCategories = selectedCategoryIds.Count == 0 && !includeUncategorized && clbArchiveCategoryFilter.Items.Count > 0;

            var filteredArchivedEvents = _allArchivedEvents
                .Where(evt => showAllCategories ||
                              (evt.CategoryId == null && includeUncategorized) ||
                              (evt.CategoryId != null && selectedCategoryIds.Contains(evt.CategoryId.Value)))
                .OrderByDescending(evt => evt.ArchivedAt) // Show most recently archived first
                .ToList();

            flpArchivedEvents.SuspendLayout();
            while (flpArchivedEvents.Controls.Count > 0) { flpArchivedEvents.Controls[0].Dispose(); }

            if (filteredArchivedEvents.Any())
            {
                foreach (var arcEvt in filteredArchivedEvents) { MaterialCard card = CreateArchivedEventCard(arcEvt); flpArchivedEvents.Controls.Add(card); }
            }
            else
            { MaterialLabel noEventsLabel = new MaterialLabel { Text = "No archived events match the current filters.", HighEmphasis = false, Depth = 0, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, Margin = new Padding(20) }; flpArchivedEvents.Controls.Add(noEventsLabel); }
            flpArchivedEvents.ResumeLayout();
        }

        // --- Archived Event Card Creation ---
        private MaterialCard CreateArchivedEventCard(ArchivedEvent arcEvt)
        {
            MaterialCard card = new MaterialCard { Width = flpArchivedEvents.Width - 30, Margin = new Padding(10), Padding = new Padding(5), Tag = arcEvt, AutoSize = true };
            TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, AutoSize = true, ColumnStyles = { new ColumnStyle(SizeType.Absolute, 8), new ColumnStyle(SizeType.Percent, 100F), new ColumnStyle(SizeType.Absolute, 95) } }; // Wider button column
            card.Controls.Add(layout);

            Panel colorStripe = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) };
            Category? category = _userCategories.FirstOrDefault(c => c.Id == arcEvt.CategoryId);
            try { colorStripe.BackColor = (category != null && !string.IsNullOrEmpty(category.ColorHex)) ? ColorTranslator.FromHtml(category.ColorHex) : Color.Transparent; } catch { colorStripe.BackColor = Color.Gray; }
            layout.Controls.Add(colorStripe, 0, 0);

            FlowLayoutPanel mainContentPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = false, AutoSize = true, Padding = new Padding(5, 0, 0, 0) };
            layout.Controls.Add(mainContentPanel, 1, 0);

            MaterialLabel lblTitle = new MaterialLabel { Text = arcEvt.Title, FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle1, HighEmphasis = true, AutoSize = true, MaximumSize = new Size(card.Width - 130, 0), Margin = new Padding(0, 0, 0, 3) }; mainContentPanel.Controls.Add(lblTitle);
            lblTitle.HighEmphasis = true;
            lblTitle.UseAccent = true;
            lblTitle.Enabled = true;
            MaterialLabel lblOriginalTime = new MaterialLabel { Text = arcEvt.IsAllDay ? $"Originally: {arcEvt.StartTime:d} (All Day)" : $"Originally: {arcEvt.StartTime:g} - {arcEvt.EndTime:g}", FontType = MaterialSkin.MaterialSkinManager.fontType.Caption, Depth = 1, HighEmphasis = false, AutoSize = true, Margin = new Padding(0, 0, 0, 3) }; mainContentPanel.Controls.Add(lblOriginalTime);
            MaterialLabel lblArchivedAt = new MaterialLabel { Text = $"Archived: {arcEvt.ArchivedAt:g}", FontType = MaterialSkin.MaterialSkinManager.fontType.Caption, Depth = 1, HighEmphasis = false, AutoSize = true, Margin = new Padding(0, 0, 0, 5) }; mainContentPanel.Controls.Add(lblArchivedAt);
            MaterialLabel lblCategory = new MaterialLabel { Text = category?.Name ?? "Uncategorized", FontType = MaterialSkin.MaterialSkinManager.fontType.Caption, Depth = 1, HighEmphasis = false, AutoSize = true, MaximumSize = new Size(mainContentPanel.Width - 10, 0), Margin = new Padding(0, 0, 0, 3) }; if (category != null) lblCategory.ForeColor = colorStripe.BackColor; else lblCategory.ForeColor = Color.Gray; mainContentPanel.Controls.Add(lblCategory);
            if (!string.IsNullOrWhiteSpace(arcEvt.Description)) { MaterialLabel lblDesc = new MaterialLabel { Text = arcEvt.Description.Length > 50 ? arcEvt.Description.Substring(0, 50) + "..." : arcEvt.Description, FontType = MaterialSkin.MaterialSkinManager.fontType.Body2, Depth = 2, HighEmphasis = false, AutoSize = true, MaximumSize = new Size(mainContentPanel.Width - 10, 0) }; mainContentPanel.Controls.Add(lblDesc); }


            FlowLayoutPanel buttonPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(5, 0, 0, 0), Margin = new Padding(0), WrapContents = false, AutoSize = true };
            layout.Controls.Add(buttonPanel, 2, 0);

            // "View/Edit" - For archived, it might just be "View Details" as editing might not make sense.
            // Or, edit means "edit before restore". For now, let's make it "View" and it opens EventForm read-only.
            MaterialButton btnView = new MaterialButton { Text = "View", Tag = arcEvt, Type = MaterialButton.MaterialButtonType.Text, UseAccentColor = true, Margin = new Padding(1), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Height = 28 /* Keep fixed height if desired */ };
            btnView.Click += CardButton_Click; buttonPanel.Controls.Add(btnView);

            MaterialButton btnRetrieve = new MaterialButton { Text = "Retrieve", Tag = arcEvt, Type = MaterialButton.MaterialButtonType.Text, Margin = new Padding(1), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Height = 28 /* Keep fixed height if desired */ };
            btnRetrieve.Click += CardButton_Click; buttonPanel.Controls.Add(btnRetrieve);

            MaterialButton btnDeletePerm = new MaterialButton { Text = "Delete", Tag = arcEvt, Type = MaterialButton.MaterialButtonType.Text, Margin = new Padding(1), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Height = 28 /* Keep fixed height if desired */ };
            btnDeletePerm.Click += CardButton_Click; buttonPanel.Controls.Add(btnDeletePerm);
            return card;
        }

        // --- Common Card Button Click Handler ---
        private void CardButton_Click(object? sender, EventArgs e)
        {
            if (sender is MaterialButton button && button.Tag is ArchivedEvent targetArchivedEvent)
            {
                if (button.Text == "View") { OpenArchivedEventFormForViewing(targetArchivedEvent); }
                else if (button.Text == "Retrieve") { RetrieveArchivedEvent(targetArchivedEvent); }
                else if (button.Text == "Delete Perm.") { DeleteArchivedEventPermanently(targetArchivedEvent); }
            }
        }

        // --- Action Methods ---
        private void OpenArchivedEventFormForViewing(ArchivedEvent arcEvt)
        {
            // Map ArchivedEvent back to Event for EventForm (or create a read-only view)
            Event eventToView = new Event
            {
                Id = arcEvt.Id,
                UserId = arcEvt.UserId,
                CategoryId = arcEvt.CategoryId,
                Title = arcEvt.Title,
                Description = arcEvt.Description,
                StartTime = arcEvt.StartTime,
                EndTime = arcEvt.EndTime,
                IsAllDay = arcEvt.IsAllDay,
                CreatedAt = arcEvt.CreatedAt,
                LastModifiedAt = arcEvt.LastModifiedAt
            };
            // TODO: Modify EventForm to have a "read-only" mode if called with an archived event.
            // For now, it will open in edit mode, but saving should be disabled or handled differently.
            using (var eventForm = new EventForm(_currentUser, _eventService, _categoryService, _reminderService, eventToView))
            {
                // eventForm.SetReadOnlyMode(); // Imaginary method
                eventForm.ShowDialog(this);
                // No refresh needed as we are just viewing.
            }
        }
        private void RetrieveArchivedEvent(ArchivedEvent eventToRetrieve)
        {
            var confirmResult = MessageBox.Show($"Retrieve '{eventToRetrieve.Title}' back to active events?", "Confirm Retrieve", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                PerformActionWithRefresh(
                   () => _eventService.RetrieveEventFromArchive(eventToRetrieve.Id, _currentUser.Id), // Needs implementation in EventService
                   "retrieving event",
                   $"Event '{eventToRetrieve.Title}' retrieved successfully.");
            }
        }
        private void DeleteArchivedEventPermanently(ArchivedEvent eventToDelete)
        {
            var confirmResult = MessageBox.Show($"PERMANENTLY DELETE '{eventToDelete.Title}' from archive?\nThis action cannot be undone.", "Confirm Permanent Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (confirmResult == DialogResult.Yes)
            {
                PerformActionWithRefresh(
                   () => _eventService.DeleteArchivedEventPermanently(eventToDelete.Id, _currentUser.Id), // Needs implementation in EventService & Repo
                   "deleting archived event",
                   $"Archived event '{eventToDelete.Title}' permanently deleted.");
            }
        }

        // --- UI Helper Methods ---
        private void UpdateSelectedDateLabel() { /* Not used in this form */ }
        private void HandleLoadError(string actionDescription, Exception ex) { Console.WriteLine($"ERROR {actionDescription}: {ex}"); MessageBox.Show($"An error occurred while {actionDescription}.\nDetails: {ex.Message}", "Data Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        private void PerformActionWithRefresh(Func<bool> action, string actionName, string successMessage) { this.Cursor = Cursors.WaitCursor; Application.DoEvents(); try { bool success = action(); if (success) { LoadAndDisplayArchivedEvents(); MessageBox.Show(successMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); } else { MessageBox.Show($"Failed to perform action: {actionName}.", "Action Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning); LoadAndDisplayArchivedEvents(); } } catch (InvalidOperationException opEx) { MessageBox.Show($"Error {actionName}: {opEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); } catch (ApplicationException appEx) { MessageBox.Show($"An error occurred while {actionName}: {appEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } catch (Exception ex) { MessageBox.Show($"An unexpected error occurred while {actionName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } finally { this.Cursor = Cursors.Default; } }
    }
}