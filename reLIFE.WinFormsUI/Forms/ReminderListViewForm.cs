using MaterialSkin;
using MaterialSkin.Controls;
using reLIFE.BusinessLogic.Services;
using reLIFE.Core.Models; // Uses Reminder and Event models
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace reLIFE.WinFormsUI.Forms
{
    public partial class ReminderListViewForm : MaterialForm
    {
        // --- Dependencies ---
        private readonly User _currentUser;
        private readonly ReminderService _reminderService;
        private readonly EventService _eventService;
        private readonly CategoryService _categoryService; // Still needed for EventForm


        // --- Data ---
        private List<ReminderInfo> _activeReminders = new List<ReminderInfo>();
        // --- NEW: Cache for event details to avoid repeated lookups ---
        private Dictionary<int, Event> _eventDetailCache = new Dictionary<int, Event>();


        public ReminderListViewForm(
            User currentUser,
            ReminderService reminderService,
            EventService eventService,
            CategoryService categoryService)
        {
            InitializeComponent();

            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _reminderService = reminderService ?? throw new ArgumentNullException(nameof(reminderService));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));

            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
        }

        // --- Form Load ---
        private void ReminderListViewForm_Load(object sender, EventArgs e)
        {
            LoadAndDisplayReminders();
        }

        // --- Data Loading & Display ---
        private void LoadAndDisplayReminders()
        {
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            try
            {
                // Use the service method that returns List<ReminderInfo>
                // Example: Get reminders due in the next 7 days
                // _activeReminders = _reminderService.GetActiveUpcomingReminderInfos(_currentUser.Id, DateTime.Now.AddDays(7));
                // Or get all future active ones:
                _activeReminders = _reminderService.GetActiveUpcomingReminderInfos(_currentUser.Id);
                DisplayReminders();
            }
            catch (Exception ex) { HandleLoadError("loading reminders", ex); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void DisplayReminders()
        {
            flpReminders.SuspendLayout();
            while (flpReminders.Controls.Count > 0) { flpReminders.Controls[0].Dispose(); }

            if (_activeReminders.Any()) // Already ordered by the service/repo
            {
                foreach (var reminderInfo in _activeReminders)
                {
                    MaterialCard card = CreateReminderCard(reminderInfo);
                    flpReminders.Controls.Add(card);
                }
            }
            else
            {
                MaterialLabel noRemindersLabel = new MaterialLabel { Text = "No active upcoming reminders found.", HighEmphasis = false, Depth = 0, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, Margin = new Padding(20) };
                flpReminders.Controls.Add(noRemindersLabel);
            }
            flpReminders.ResumeLayout();
        }

        // --- Helper to Get Event Details (with Basic Caching) ---
        private Event? GetEventDetails(int eventId)
        {
            if (_eventDetailCache.TryGetValue(eventId, out Event? cachedEvent))
            {
                return cachedEvent;
            }
            try
            {
                Event? fetchedEvent = _eventService.GetEventById(eventId, _currentUser.Id);
                if (fetchedEvent != null)
                {
                    _eventDetailCache[eventId] = fetchedEvent; // Add to cache
                }
                return fetchedEvent;
            }
            catch (Exception ex)
            {
                HandleLoadError($"fetching details for event ID {eventId}", ex);
                return null; // Failed to get details
            }
        }




        // --- Reminder Card Creation (Uses Reminder and Event) ---
        private MaterialCard CreateReminderCard(ReminderInfo reminderInfo)
        {
            // ReminderInfo already has EventTitle and EventStartTime
            MaterialCard card = new MaterialCard { Width = flpReminders.Width - 30, Margin = new Padding(10), Padding = new Padding(10), Tag = reminderInfo, AutoSize = true }; // Tag is ReminderInfo
            TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, AutoSize = true, ColumnStyles = { new ColumnStyle(SizeType.Percent, 100F), new ColumnStyle(SizeType.Absolute, 95) } };
            card.Controls.Add(layout);

            FlowLayoutPanel mainContentPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = false, AutoSize = true, Padding = new Padding(5, 0, 0, 0) };
            layout.Controls.Add(mainContentPanel, 0, 0);

            MaterialLabel lblEventTitle = new MaterialLabel { Text = reminderInfo.EventTitle, UseAccent = true, FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle1, HighEmphasis = true, AutoSize = true, MaximumSize = new Size(card.Width - 130, 0), Margin = new Padding(0, 0, 0, 3) };
            mainContentPanel.Controls.Add(lblEventTitle);

            MaterialLabel lblReminderTime = new MaterialLabel { Text = $"Reminder due: {reminderInfo.ReminderTime:g}", FontType = MaterialSkin.MaterialSkinManager.fontType.Body1, UseAccent = true, AutoSize = true, Margin = new Padding(0, 0, 0, 3) };
            mainContentPanel.Controls.Add(lblReminderTime);

            MaterialLabel lblEventTime = new MaterialLabel { Text = $"Event starts: {reminderInfo.EventStartTime:g}", FontType = MaterialSkin.MaterialSkinManager.fontType.Caption, Depth = 1, HighEmphasis = false, AutoSize = true, Margin = new Padding(0, 0, 0, 3) };
            mainContentPanel.Controls.Add(lblEventTime);

            MaterialLabel lblSetting = new MaterialLabel { Text = $"({reminderInfo.MinutesBefore} minutes before)", FontType = MaterialSkin.MaterialSkinManager.fontType.Caption, Depth = 1, HighEmphasis = false, AutoSize = true, Margin = new Padding(0, 0, 0, 3) };
            mainContentPanel.Controls.Add(lblSetting);

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(5, 0, 0, 0), Margin = new Padding(0), WrapContents = false, AutoSize = true };
            layout.Controls.Add(buttonPanel, 1, 0);

            MaterialButton btnViewEvent = new MaterialButton { Text = "View", UseAccentColor = true, Tag = reminderInfo, Type = MaterialButton.MaterialButtonType.Text, Margin = new Padding(1), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, MinimumSize = new Size(90, 28) };
            btnViewEvent.Click += CardButton_Click;
            buttonPanel.Controls.Add(btnViewEvent);

            MaterialButton btnDeleteReminder = new MaterialButton { Text = "Delete", Tag = reminderInfo, Type = MaterialButton.MaterialButtonType.Text, Margin = new Padding(1), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, MinimumSize = new Size(90, 28) };
            btnDeleteReminder.Click += CardButton_Click;
            buttonPanel.Controls.Add(btnDeleteReminder);

            return card;
        }

        // --- Card Button Click Handler (Uses Reminder object) ---
        private void CardButton_Click(object? sender, EventArgs e)
        {
            if (sender is MaterialButton button && button.Tag is ReminderInfo targetReminderInfo)
            {
                if (button.Text == "View")
                {
                    ViewEvent(targetReminderInfo.EventId);
                }
                else if (button.Text == "Delete")
                {
                    DeleteReminder(targetReminderInfo); // Pass ReminderInfo
                }
            }
        }

        // --- Action Methods ---
        private void ViewEvent(int eventId)
        {
            SetProcessingState(true);
            try
            {
                Event? eventToView = _eventService.GetEventById(eventId, _currentUser.Id); // Fetch full event
                if (eventToView != null)
                {
                    using (var eventForm = new EventForm(_currentUser, _eventService, _categoryService, _reminderService, eventToView))
                    {
                        var skinManager = MaterialSkinManager.Instance;
                        skinManager.AddFormToManage(eventForm);
                        eventForm.ShowDialog(this.FindForm());
                        skinManager.RemoveFormToManage(eventForm);
                    }
                    LoadAndDisplayReminders(); // Refresh list after EventForm closes
                }
                else { MessageBox.Show("Could not find the associated event.", "Event Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning); LoadAndDisplayReminders(); }
            }
            catch (Exception ex) { HandleActionError("viewing event", ex); }
            finally { SetProcessingState(false); }
        }

        private void DeleteReminder(ReminderInfo reminderToDelete) // Takes ReminderInfo
        {
            var confirm = MessageBox.Show($"Are you sure you want to delete the reminder for '{reminderToDelete.EventTitle}'?", "Confirm Delete Reminder", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                PerformActionWithRefresh(
                    () => _reminderService.DeleteReminder(reminderToDelete.ReminderId, _currentUser.Id), // Use ReminderId
                    "deleting reminder",
                    "Reminder deleted.");
            }
        }

        // --- UI Helper Methods ---
        private void HandleLoadError(string actionDescription, Exception ex) { Console.WriteLine($"ERROR {actionDescription}: {ex}"); MessageBox.Show($"An error occurred while {actionDescription}.\nDetails: {ex.Message}", "Data Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        private void SetProcessingState(bool processing) { this.Cursor = processing ? Cursors.WaitCursor : Cursors.Default; if (processing) Application.DoEvents(); /* Maybe disable buttons? */ }
        private void HandleActionError(string actionName, Exception ex) { Console.WriteLine($"ERROR {actionName}: {ex}"); string userMessage = $"An error occurred while {actionName}."; if (ex is InvalidOperationException || ex is ArgumentException) { userMessage = $"Error: {ex.Message}"; } else if (ex is ApplicationException) { userMessage = $"Database Error."; } else { userMessage = $"An unexpected error occurred."; } MessageBox.Show(userMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        private void PerformActionWithRefresh(Func<bool> action, string actionName, string successMessage) { SetProcessingState(true); try { bool success = action(); if (success) { LoadAndDisplayReminders(); MessageBox.Show(successMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); } else { MessageBox.Show($"Failed to perform action: {actionName}.", "Action Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning); LoadAndDisplayReminders(); } } catch (Exception ex) { HandleActionError(actionName, ex); } finally { SetProcessingState(false); } }

    }
}