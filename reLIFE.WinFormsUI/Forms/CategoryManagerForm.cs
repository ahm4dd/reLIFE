using MaterialSkin;
using MaterialSkin.Controls;
using reLIFE.BusinessLogic.Services;
using reLIFE.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace reLIFE.WinFormsUI.Forms
{
    public partial class CategoryManagerForm : MaterialForm
    {
        // ... (Fields and Constructor remain the same) ...
        private readonly User _currentUser;
        private readonly CategoryService _categoryService;
        private List<Category> _userCategories = new List<Category>();
        private Category? _selectedCategory = null;


        public CategoryManagerForm(User currentUser, CategoryService categoryService)
        {
            InitializeComponent();

            // Configure Form for Embedding
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            this.AutoScroll = false; // Form itself shouldn't scroll

            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));

            // Initial UI State
            ClearEditFields();
        }

        private void CategoryManagerForm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                SetupListViewColumnsAndResize(); // Setup columns first
                LoadAndDisplayCategories();
                HookEventHandlers();

                // *** ADD THIS LINE ***
                // Try setting background AFTER potential theme application
                // Use a color that matches your dark theme's surface color
                lstCategories.BackColor = Color.FromArgb(50, 50, 50); // Example dark grey - Adjust this color!

                this.BeginInvoke((MethodInvoker)delegate { SetActiveControl(txtName); });
            }
            catch (Exception ex) { HandleActionError("loading categories", ex); } // Changed from HandleLoadError if using common handler
            finally { this.Cursor = Cursors.Default; }
        }

        private void HookEventHandlers()
        {
            lstCategories.SelectedIndexChanged += LstCategories_SelectedIndexChanged;
            txtColorHex.TextChanged += TxtColorHex_TextChanged;
            btnSaveCategory.Click += BtnSaveCategory_Click;
            btnClearSelection.Click += BtnClearSelection_Click;
            btnDeleteCategory.Click += BtnDeleteCategory_Click;
            // Handle SizeChanged event to recalculate column width if form resizes
            lstCategories.SizeChanged += LstCategories_SizeChanged;
        }

        // *** MODIFIED/NEW: Setup Columns AND Handle Initial Resize ***
        private void SetupListViewColumnsAndResize()
        {
            lstCategories.Columns.Clear(); // Start fresh

            // Ensure colName and colColor are initialized (usually done in Designer.cs)
            // If not, initialize them here:
            if (this.colName == null) this.colName = new ColumnHeader();
            if (this.colColor == null) this.colColor = new ColumnHeader();

            this.colName.Text = "Name"; // Set text if not set in designer
            this.colColor.Text = "Color"; // Set text if not set in designer

            lstCategories.Columns.Add(this.colName);
            lstCategories.Columns.Add(this.colColor);

            if (this.colColor != null) this.colColor.Width = 90; // Fixed width for color

            ResizeNameColumn(); // Initial sizing

            lstCategories.View = View.Details;
            lstCategories.FullRowSelect = true;
            lstCategories.MultiSelect = false;
            lstCategories.OwnerDraw = true; // For custom color cell drawing
            lstCategories.AutoSizeTable = false; // Crucial for manual column sizing
            lstCategories.HeaderStyle = ColumnHeaderStyle.None; // Hide the header
        }

        // *** NEW: Handler for ListView Size Changed ***
        private void LstCategories_SizeChanged(object sender, EventArgs e)
        {
            // Recalculate Name column width when the ListView size changes
            ResizeNameColumn();
        }

        // *** NEW: Helper to Calculate and Set Name Column Width ***
        private void ResizeNameColumn()
        {
            if (lstCategories == null || lstCategories.Columns.Count < 2 || colName == null || colColor == null)
                return; // Exit if controls not ready or columns not set

            try
            {
                int listViewClientWidth = lstCategories.ClientSize.Width;
                int colorColumnWidth = colColor.Width;
                int scrollBarWidth = 0;

                // Determine if a vertical scrollbar is likely taking up space
                // This check is more reliable: if total item height > client height
                if (lstCategories.Items.Count > 0)
                {
                    int totalItemHeight = 0;
                    // Calculate total height of all items (approximate)
                    // For OwnerDraw=true, GetItemRect(0).Height is better if items have uniform height
                    if (lstCategories.Items.Count > 0 && lstCategories.View == View.Details)
                    {
                        // Summing heights or checking last item's bottom
                        // If items are uniform height:
                        // totalItemHeight = lstCategories.Items.Count * lstCategories.GetItemRect(0).Height;
                        // Or, more generally, check if the last item's bottom is beyond the client height
                        // This also needs to account for the header if visible.
                        // Since header is NONE, we just check the last item.
                        if (lstCategories.Items[lstCategories.Items.Count - 1].Bounds.Bottom > lstCategories.ClientSize.Height)
                        {
                            scrollBarWidth = SystemInformation.VerticalScrollBarWidth;
                        }
                    }
                }


                // Calculate available width for the Name column
                int availableWidth = listViewClientWidth - colorColumnWidth - scrollBarWidth;

                // Subtract a small margin for borders/padding within the ListView itself
                availableWidth -= 6; // Adjust this value as needed (e.g., 4 to 10)

                // Ensure a minimum width for the Name column
                colName.Width = Math.Max(60, availableWidth);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resizing category list columns: {ex.Message}");
                // Fallback width in case of error during calculation
                if (colName != null) colName.Width = 250;
            }
        }


        // --- Data Loading ---
        private void LoadAndDisplayCategories()
        {
            this.Cursor = Cursors.WaitCursor;
            int? selectedId = null;
            if (lstCategories.SelectedItems.Count > 0 && lstCategories.SelectedItems[0].Tag is Category c)
            {
                selectedId = c.Id; // Preserve selection ID
            }

            try
            {
                _userCategories = _categoryService.GetCategoriesByUser(_currentUser.Id);
                PopulateListView(selectedId); // Pass selectedId to re-select
                ResizeNameColumn(); // Ensure columns are sized after populating
            }
            catch (Exception ex) { HandleActionError("loading categories", ex); }
            finally { this.Cursor = Cursors.Default; }
        }

        // *** MODIFIED: Takes optional ID to re-select ***
        private void PopulateListView(int? selectedCategoryId = null)
        {
            lstCategories.Items.Clear();
            ListViewItem? itemToSelect = null;

            foreach (var cat in _userCategories.OrderBy(c => c.Name))
            {
                var item = new ListViewItem(cat.Name);
                item.SubItems.Add(""); // Placeholder for color cell
                item.Tag = cat;

                try { if (!string.IsNullOrEmpty(cat.ColorHex)) { item.UseItemStyleForSubItems = false; item.SubItems[1].BackColor = ColorTranslator.FromHtml(cat.ColorHex); if (IsColorDark(item.SubItems[1].BackColor)) item.SubItems[1].ForeColor = Color.White; else item.SubItems[1].ForeColor = item.SubItems[1].BackColor; } } catch { /* Ignore invalid */ }

                lstCategories.Items.Add(item);

                // Check if this is the item to re-select
                if (selectedCategoryId.HasValue && cat.Id == selectedCategoryId.Value)
                {
                    itemToSelect = item;
                }
            }

            // Re-select the item after populating
            if (itemToSelect != null)
            {
                itemToSelect.Selected = true;
                lstCategories.EnsureVisible(itemToSelect.Index);
                lstCategories.Focus(); // Give focus back to list
            }
            else
            {
                ClearEditFields(); // Clear edit fields if previous selection is gone
            }
        }


        // --- Event Handlers ---

        private void LstCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCategories.SelectedItems.Count == 1)
            {
                _selectedCategory = lstCategories.SelectedItems[0].Tag as Category;
                if (_selectedCategory != null)
                {
                    txtName.Text = _selectedCategory.Name;
                    txtColorHex.Text = _selectedCategory.ColorHex;
                    UpdateColorPreview();
                    btnSaveCategory.Text = "Update Category"; // Change button text
                    btnDeleteCategory.Enabled = true;
                }
                else { ClearEditFields(); }
            }
            else { ClearEditFields(); }
        }

        private void TxtColorHex_TextChanged(object sender, EventArgs e) { UpdateColorPreview(); }

        private void BtnSaveCategory_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string colorHex = txtColorHex.Text.Trim().ToUpper(); // Standardize case

            if (string.IsNullOrWhiteSpace(name)) { ShowError("Category name cannot be empty."); txtName.Focus(); return; }
            if (!IsValidHexColor(colorHex)) { ShowError("Invalid Color Hex format. Use #RRGGBB (e.g., #FF00AA)."); txtColorHex.Focus(); return; }

            SetProcessingState(true);
            try
            {
                bool success = false;
                string actionVerb = _selectedCategory != null ? "updating" : "adding"; // For error message

                if (_selectedCategory != null) // Update
                {
                    _selectedCategory.Name = name;
                    _selectedCategory.ColorHex = colorHex;
                    success = _categoryService.UpdateCategory(_selectedCategory, _currentUser.Id);
                    if (success) MessageBox.Show("Category updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else // Add
                {
                    var newCategory = new Category { UserId = _currentUser.Id, Name = name, ColorHex = colorHex };
                    var addedCategory = _categoryService.AddCategory(newCategory, _currentUser.Id);
                    if (addedCategory != null && addedCategory.Id > 0) success = true;
                    if (success) MessageBox.Show("Category added.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (success)
                {
                    LoadAndDisplayCategories(); // Refresh list (will clear selection)
                    ClearEditFields(); // Explicitly clear fields
                }
                else { ShowError($"Failed to {actionVerb} category."); }
            }
            catch (Exception ex) { HandleActionError($"category", ex); }
            finally { SetProcessingState(false); }
        }

        private void BtnClearSelection_Click(object sender, EventArgs e) { ClearEditFields(); }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (_selectedCategory == null) return;
            var confirm = MessageBox.Show($"Delete '{_selectedCategory.Name}'?\nEvents using it will become uncategorized.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                PerformActionWithRefresh(() => _categoryService.DeleteCategory(_selectedCategory.Id, _currentUser.Id), "deleting category", "Category deleted.");
            }
        }

        // --- Helper Methods ---

        private void ClearEditFields()
        {
            _selectedCategory = null;
            lstCategories.SelectedItems.Clear();
            txtName.Clear();
            txtColorHex.Clear();
            UpdateColorPreview(); // Reset preview
            btnSaveCategory.Text = "Add Category"; // Reset button text
            btnDeleteCategory.Enabled = false;
            txtName.Focus();
            ShowError(null);
        }

        private void UpdateColorPreview() { if (pnlColorPreview != null) { if (IsValidHexColor(txtColorHex.Text)) { try { pnlColorPreview.BackColor = ColorTranslator.FromHtml(txtColorHex.Text); } catch { pnlColorPreview.BackColor = SystemColors.ControlDark; } } else { pnlColorPreview.BackColor = SystemColors.Control; } } }
        private bool IsValidHexColor(string hex) { return Regex.IsMatch(hex ?? "", @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$"); }
        private bool IsColorDark(Color color) { return (color.R * 0.299 + color.G * 0.587 + color.B * 0.114) < 140; }
        private void ShowError(string? message) { if (lblError != null) { lblError.Text = message ?? ""; lblError.Visible = !string.IsNullOrEmpty(message); } else if (!string.IsNullOrEmpty(message)) { MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); } }
        private void SetProcessingState(bool processing) { pnlListArea.Enabled = !processing; cardEditArea.Enabled = !processing; this.Cursor = processing ? Cursors.WaitCursor : Cursors.Default; if (processing) Application.DoEvents(); }
        private void HandleActionError(string actionName, Exception ex) { Console.WriteLine($"ERROR {actionName}: {ex}"); string userMessage = $"An error occurred while {actionName}."; if (ex is InvalidOperationException || ex is ArgumentException) { userMessage = $"Error: {ex.Message}"; } else if (ex is ApplicationException) { userMessage = $"Database Error: Please try again later."; } else { userMessage = $"An unexpected error occurred."; } ShowError(userMessage); }
        private void SetActiveControl(Control control) { if (control != null && control.CanFocus) { control.Focus(); if (control is MaterialTextBox2 mtb2) { mtb2.SelectAll(); } } }

        // Make sure PerformActionWithRefresh exists and calls LoadAndDisplayCategories
        private void PerformActionWithRefresh(Func<bool> action, string actionName, string successMessage)
        {
            SetProcessingState(true);
            try
            {
                bool success = action();
                if (success)
                {
                    LoadAndDisplayCategories(); // Refresh category list
                    ClearEditFields(); // Clear edit fields after success
                    MessageBox.Show(successMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ShowError($"Failed to perform action: {actionName}.");
                }
            }
            catch (Exception ex) { HandleActionError(actionName, ex); }
            finally { SetProcessingState(false); }
        }

    }
}