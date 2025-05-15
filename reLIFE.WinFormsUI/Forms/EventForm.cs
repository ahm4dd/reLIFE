using MaterialSkin;
using MaterialSkin.Controls;
using reLIFE.BusinessLogic.Services;
using reLIFE.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace reLIFE.WinFormsUI.Forms
{
    /// <summary>
    /// Modal form for adding or editing event details, including reminders.
    /// Uses MaterialSkin controls where possible and standard DateTimePickers for time.
    /// </summary>
    public partial class EventForm : MaterialForm
    {
        // --- Dependencies ---
        private readonly User _currentUser;
        private readonly EventService _eventService;
        private readonly CategoryService _categoryService;
        private readonly ReminderService _reminderService;

        // --- State ---
        private readonly Event? _eventToEdit; // Null if adding, holds event if editing
        private bool _isEditMode => _eventToEdit != null;
        private List<Category> _userCategories = new List<Category>();
        private Reminder? _existingReminder = null; // Holds existing reminder for the event being edited

        // --- Constructor ---
        public EventForm(
        User currentUser,
        EventService eventService,
        CategoryService categoryService,
        ReminderService reminderService,
        Event? eventToEdit = null
        )
        {
            InitializeComponent();

            var skinManager = MaterialSkin.MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);

            // Store dependencies
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _reminderService = reminderService ?? throw new ArgumentNullException(nameof(reminderService));
            _eventToEdit = eventToEdit;

            // ... rest of constructor (SetupCategoryComboBox, ToggleReminderControls) ...
            SetupCategoryComboBox();
            ToggleReminderControls(false);
            lblError.AutoSize = false;
            //lblError.Size = new Size(323, 72);
            lblError.FontType = MaterialSkinManager.fontType.Body2;
            lblError.HighEmphasis = true;
            lblError.UseAccent = true;
            lblError.Enabled = true;
        }

        // --- Form Load ---
        private void EventForm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                LoadCategories();       // Load categories into the combo box
                PopulateFormFields();   // Populate fields based on Add or Edit mode
            }
            catch (Exception ex)
            {
                ShowError($"Error loading event data: {ex.Message}");
                btnSave.Enabled = false; // Disable save if loading fails critically
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        // --- UI Event Handlers ---

        private void chkAllDay_CheckedChanged(object sender, EventArgs e)
        {
            // Disable time pickers if All Day is checked
            bool timeEnabled = !chkAllDay.Checked;
            dtpStartTime.Enabled = timeEnabled; // Standard DateTimePicker for Time
            dtpEndTime.Enabled = timeEnabled;   // Standard DateTimePicker for Time
        }

        private void chkEnableReminder_CheckedChanged(object sender, EventArgs e)
        {
            // Enable/disable the minutes input based on checkbox
            ToggleReminderControls(chkEnableReminder.Checked);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return; // Stop if basic validation fails
            }

            SetProcessingState(true); // Disable UI, show wait cursor
            bool overallSuccess = false;

            try
            {
                // 1. Prepare Event Data
                Event eventData = PrepareEventData();

                // 2. Save Event (Add or Update)
                Event savedEvent; // Will hold the result (with ID)
                if (_isEditMode)
                {
                    bool updated = _eventService.UpdateEvent(eventData, _currentUser.Id);
                    if (!updated) throw new InvalidOperationException("Failed to update the event. It might have been modified or deleted.");
                    savedEvent = eventData; // Use the updated object (Id is already correct)
                }
                else
                {
                    savedEvent = _eventService.AddEvent(eventData, _currentUser.Id);
                    if (savedEvent == null || savedEvent.Id <= 0) throw new ApplicationException("Failed to retrieve saved event data after adding.");
                }

                // 3. Handle Reminder
                // This method now returns bool indicating success/failure of reminder part
                bool reminderSuccess = HandleReminderSave(savedEvent.Id);

                // 4. Close Form if all parts were successful
                if (reminderSuccess) // Check if reminder step was okay
                {
                    overallSuccess = true; // Mark overall operation as successful
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                // If reminderSuccess is false, HandleReminderSave already showed an error

            }
            catch (InvalidOperationException opEx) { ShowError($"Validation Error: {opEx.Message}"); }
            catch (ArgumentException argEx) { ShowError($"Invalid Input: {argEx.Message}"); }
            catch (ApplicationException appEx) { ShowError($"Database Error: {appEx.Message}"); }
            catch (Exception ex) { ShowError($"An unexpected error occurred: {ex.Message}"); Console.WriteLine($"Save Event Error: {ex}"); }
            finally
            {
                // Re-enable UI only if staying on the form (i.e., if save wasn't fully successful)
                if (!overallSuccess)
                {
                    SetProcessingState(false);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // --- Data Loading and Population ---

        private void LoadCategories()
        {
            _userCategories = _categoryService.GetCategoriesByUser(_currentUser.Id);
            PopulateCategoryComboBox();
        }

        private void SetupCategoryComboBox()
        {
            cmbCategory.Items.Clear();
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Id";
            cmbCategory.Items.Add(new Category { Id = -1, Name = "(None)" }); // Use -1 to indicate null/none
        }

        private void PopulateCategoryComboBox()
        {
            var noneOption = cmbCategory.Items.Count > 0 ? cmbCategory.Items[0] : null; // Preserve if exists
            cmbCategory.Items.Clear();
            if (noneOption != null) cmbCategory.Items.Add(noneOption);
            else cmbCategory.Items.Add(new Category { Id = -1, Name = "(None)" });


            foreach (var category in _userCategories.OrderBy(c => c.Name))
            {
                cmbCategory.Items.Add(category);
            }

            cmbCategory.SelectedIndex = 0; // Default to "(None)"
        }

        private void PopulateFormFields()
        {
            if (_isEditMode && _eventToEdit != null)
            {
                this.Text = "Edit Event";
                txtTitle.Text = _eventToEdit.Title;
                txtDescription.Text = _eventToEdit.Description;
                chkAllDay.Checked = _eventToEdit.IsAllDay;

                // Set Date pickers first
                dtpStartDate.Value = _eventToEdit.StartTime.Date;
                dtpEndDate.Value = _eventToEdit.EndTime.Date; // Set end date picker

                // Set Time pickers using a valid base date (e.g., the selected start date or today)
                // Ensure the date part is valid before adding TimeOfDay
                DateTime baseDateForStartTime = dtpStartDate.Value.Date; // Use the selected start date
                if (baseDateForStartTime < dtpStartTime.MinDate) baseDateForStartTime = dtpStartTime.MinDate;
                if (baseDateForStartTime > dtpStartTime.MaxDate) baseDateForStartTime = dtpStartTime.MaxDate;
                dtpStartTime.Value = baseDateForStartTime.Add(_eventToEdit.StartTime.TimeOfDay);

                DateTime baseDateForEndTime = dtpEndDate.Value.Date; // Use the selected end date
                if (baseDateForEndTime < dtpEndTime.MinDate) baseDateForEndTime = dtpEndTime.MinDate;
                if (baseDateForEndTime > dtpEndTime.MaxDate) baseDateForEndTime = dtpEndTime.MaxDate;
                dtpEndTime.Value = baseDateForEndTime.Add(_eventToEdit.IsAllDay ? TimeSpan.Zero : _eventToEdit.EndTime.TimeOfDay);


                if (_eventToEdit.CategoryId.HasValue)
                {
                    foreach (var item in cmbCategory.Items) { if (item is Category cat && cat.Id == _eventToEdit.CategoryId.Value) { cmbCategory.SelectedItem = item; break; } }
                }
                else { cmbCategory.SelectedIndex = 0; }

                LoadAndPopulateReminder(_eventToEdit.Id);
            }
            else
            {
                // --- ADD MODE ---
                this.Text = "Add New Event";
                DateTime now = DateTime.Now;
                // Default start time to the next hour, on the current date from dtpStartDate
                DateTime defaultStartDate = dtpStartDate.Value.Date; // Get current date from picker
                TimeSpan defaultStartTimeSpan = new TimeSpan(now.Hour + 1, 0, 0);
                if (defaultStartTimeSpan.TotalHours >= 24)
                { // Handle rolling over midnight
                    defaultStartTimeSpan = new TimeSpan(0, 0, 0); // Start at midnight
                    defaultStartDate = defaultStartDate.AddDays(1);
                }
                DateTime start = defaultStartDate.Add(defaultStartTimeSpan);
                DateTime end = start.AddHours(1); // Default end time one hour after start

                // Set Date pickers
                dtpStartDate.Value = start.Date;
                dtpEndDate.Value = end.Date;

                // Set Time pickers using a valid base date (from the date pickers themselves)
                dtpStartTime.Value = dtpStartDate.Value.Date.Add(start.TimeOfDay);
                dtpEndTime.Value = dtpEndDate.Value.Date.Add(end.TimeOfDay);

                chkAllDay.Checked = false;
                cmbCategory.SelectedIndex = 0;
                chkEnableReminder.Checked = false;
                ToggleReminderControls(false);
            }
            chkAllDay_CheckedChanged(chkAllDay, EventArgs.Empty);
        }

        private void LoadAndPopulateReminder(int eventId)
        {
            try
            {
                var reminders = _reminderService.GetRemindersForEvent(eventId, _currentUser.Id);
                _existingReminder = reminders.FirstOrDefault();

                if (_existingReminder != null)
                {
                    chkEnableReminder.Checked = _existingReminder.IsEnabled;
                    nudReminderMinutes.Value = Math.Max(nudReminderMinutes.Minimum, Math.Min(nudReminderMinutes.Maximum, _existingReminder.MinutesBefore));
                    ToggleReminderControls(_existingReminder.IsEnabled);
                }
                else
                {
                    chkEnableReminder.Checked = false;
                    nudReminderMinutes.Value = nudReminderMinutes.Minimum > 0 ? nudReminderMinutes.Minimum : 5; // Default minutes if needed
                    ToggleReminderControls(false);
                }
            }
            catch (Exception ex) { ShowError($"Failed to load reminder settings: {ex.Message}"); ToggleReminderControls(false); }
        }

        // --- Data Preparation and Validation ---

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text)) { ShowError("Event title cannot be empty."); SetActiveControl(txtTitle); return false; }

            DateTime startDateTime = GetStartDateTime();
            DateTime endDateTime = GetEndDateTime();

            if (endDateTime < startDateTime && !chkAllDay.Checked) { ShowError("End time cannot be before start time."); SetActiveControl(dtpEndTime); return false; }
            if (endDateTime <= startDateTime && chkAllDay.Checked && dtpEndDate.Value.Date < dtpStartDate.Value.Date) { ShowError("End date cannot be before start date for an all-day event."); SetActiveControl(dtpEndDate); return false; } // All day specific check

            if (chkEnableReminder.Checked && nudReminderMinutes.Value <= 0) { ShowError("Reminder time must be greater than 0 minutes."); SetActiveControl(nudReminderMinutes); return false; }

            ShowError(null);
            return true;
        }

        private Event PrepareEventData()
        {
            Event eventData = _isEditMode ? _eventToEdit! : new Event();

            eventData.Title = txtTitle.Text.Trim();
            eventData.Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim();
            eventData.IsAllDay = chkAllDay.Checked;
            eventData.UserId = _currentUser.Id;

            if (cmbCategory.SelectedItem is Category selectedCategory && selectedCategory.Id > 0) { eventData.CategoryId = selectedCategory.Id; }
            else { eventData.CategoryId = null; }

            eventData.StartTime = GetStartDateTime();
            eventData.EndTime = GetEndDateTime();

            return eventData;
        }

        private DateTime GetStartDateTime()
        {
            DateTime datePart = dtpStartDate.Value.Date; // Use the DATE picker for the date part
            TimeSpan timePart = chkAllDay.Checked ? TimeSpan.Zero : dtpStartTime.Value.TimeOfDay; // Use the TIME picker for time
            return datePart.Add(timePart);
        }

        private DateTime GetEndDateTime()
        {
            DateTime datePart = dtpEndDate.Value.Date; // Use the DATE picker for the date part
            if (chkAllDay.Checked)
            {
                // For all-day events, end time is start of next day (exclusive)
                // Ensure the date part is taken from the *start date* if end date can be before start for all-day
                // Or, if all-day always means the whole of dtpStartDate:
                return dtpStartDate.Value.Date.AddDays(1);
                // If all-day means the whole of dtpEndDate (as selected by user):
                // return dtpEndDate.Value.Date.AddDays(1);
            }
            else
            {
                TimeSpan timePart = dtpEndTime.Value.TimeOfDay; // Use the TIME picker for time
                return datePart.Add(timePart);
            }
        }

        // --- Reminder Handling ---
        private bool HandleReminderSave(int eventId)
        {
            try
            {
                bool reminderEnabledUI = chkEnableReminder.Checked;
                int minutesBeforeUI = (int)nudReminderMinutes.Value;

                if (reminderEnabledUI)
                {
                    Reminder reminderToSave = _existingReminder ?? new Reminder { EventId = eventId };
                    reminderToSave.MinutesBefore = minutesBeforeUI;
                    reminderToSave.IsEnabled = true;
                    _reminderService.SaveReminder(reminderToSave, _currentUser.Id);
                }
                else if (_existingReminder != null) // Reminder disabled in UI AND one existed before
                {
                    _reminderService.DeleteReminder(_existingReminder.Id, _currentUser.Id);
                }
                return true; // Reminder handled successfully
            }
            catch (Exception ex)
            {
                ShowError($"Failed to save reminder settings: {ex.Message}");
                Console.WriteLine($"Reminder Save Error: {ex}");
                return false; // Indicate reminder save failed
            }
        }

        // --- UI Helper Methods ---
        private void ToggleReminderControls(bool enabled)
        {
            nudReminderMinutes.Enabled = enabled;
            lblMinutesBefore.Enabled = enabled; // Assuming you have this label
        }

        private void SetProcessingState(bool processing)
        {
            txtTitle.Enabled = !processing;
            txtDescription.Enabled = !processing;
            dtpStartDate.Enabled = !processing;
            dtpStartTime.Enabled = !processing && !chkAllDay.Checked;
            dtpEndDate.Enabled = !processing;
            dtpEndTime.Enabled = !processing && !chkAllDay.Checked;
            chkAllDay.Enabled = !processing;
            cmbCategory.Enabled = !processing;
            chkEnableReminder.Enabled = !processing;
            nudReminderMinutes.Enabled = !processing && chkEnableReminder.Checked;
            btnSave.Enabled = !processing;
            btnCancel.Enabled = !processing;
            this.Cursor = processing ? Cursors.WaitCursor : Cursors.Default;
            if (processing) Application.DoEvents(); // Force UI update mainly for cursor
        }

        private void ShowError(string? message)
        {
            if (lblError != null) { lblError.Text = message ?? ""; lblError.Visible = !string.IsNullOrEmpty(message); }
            else if (!string.IsNullOrEmpty(message)) { MessageBox.Show(message, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void SetActiveControl(Control control)
        {
            if (control != null && control.CanFocus)
            {
                this.ActiveControl = control;
                if (control is TextBoxBase || control is MaterialTextBox2 || control is MaterialMultiLineTextBox2) { control.Focus(); }
                else if (control is NumericUpDown nud) { nud.Select(0, nud.Text.Length); }
            }
        }
    }
}