using MaterialSkin.Controls;

namespace reLIFE.WinFormsUI.Forms
{
    partial class CategoryManagerForm
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>

        // --- End Field Declarations ---


        private void InitializeComponent()
        {
            pnlListArea = new Panel();
            lstCategories = new MaterialListView();
            colName = new ColumnHeader();
            colColor = new ColumnHeader();
            btnDeleteCategory = new MaterialButton();
            cardEditArea = new MaterialCard();
            lblError = new MaterialLabel();
            btnClearSelection = new MaterialButton();
            pnlColorPreview = new Panel();
            txtColorHex = new MaterialTextBox2();
            lblEditHeader = new MaterialLabel();
            txtName = new MaterialTextBox2();
            btnSaveCategory = new MaterialButton();
            pnlListArea.SuspendLayout();
            cardEditArea.SuspendLayout();
            SuspendLayout();
            // 
            // pnlListArea
            // 
            pnlListArea.Controls.Add(lstCategories);
            pnlListArea.Controls.Add(btnDeleteCategory);
            pnlListArea.Location = new Point(13, 10);
            pnlListArea.Margin = new Padding(10);
            pnlListArea.Name = "pnlListArea";
            pnlListArea.Size = new Size(801, 231);
            pnlListArea.TabIndex = 1;
            // 
            // lstCategories
            // 
            lstCategories.AutoSizeTable = false;
            lstCategories.BackColor = Color.FromArgb(255, 255, 255);
            lstCategories.BorderStyle = BorderStyle.None;
            lstCategories.Columns.AddRange(new ColumnHeader[] { colName, colColor });
            lstCategories.Depth = 0;
            lstCategories.Dock = DockStyle.Fill;
            lstCategories.Font = new Font("Microsoft Sans Serif", 12F);
            lstCategories.ForeColor = Color.FromArgb(222, 0, 0, 0);
            lstCategories.FullRowSelect = true;
            lstCategories.Location = new Point(0, 0);
            lstCategories.MinimumSize = new Size(200, 100);
            lstCategories.MouseLocation = new Point(-1, -1);
            lstCategories.MouseState = MaterialSkin.MouseState.OUT;
            lstCategories.MultiSelect = false;
            lstCategories.Name = "lstCategories";
            lstCategories.OwnerDraw = true;
            lstCategories.Size = new Size(801, 195);
            lstCategories.TabIndex = 0;
            lstCategories.UseCompatibleStateImageBehavior = false;
            lstCategories.View = View.Details;
            lstCategories.SelectedIndexChanged += LstCategories_SelectedIndexChanged;
            lstCategories.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.None);
            lstCategories.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.None);
            // 
            // colName
            // 
            colName.Text = "Name";
            colName.Width = 720;
            // 
            // colColor
            // 
            colColor.Text = "Color";
            colColor.Width = 90;
            // 
            // btnDeleteCategory
            // 
            btnDeleteCategory.AutoSize = false;
            btnDeleteCategory.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnDeleteCategory.Density = MaterialButton.MaterialButtonDensity.Default;
            btnDeleteCategory.Depth = 0;
            btnDeleteCategory.Dock = DockStyle.Bottom;
            btnDeleteCategory.Enabled = false;
            btnDeleteCategory.HighEmphasis = false;
            btnDeleteCategory.Icon = null;
            btnDeleteCategory.Location = new Point(0, 195);
            btnDeleteCategory.Margin = new Padding(4, 6, 4, 6);
            btnDeleteCategory.MouseState = MaterialSkin.MouseState.HOVER;
            btnDeleteCategory.Name = "btnDeleteCategory";
            btnDeleteCategory.NoAccentTextColor = Color.Empty;
            btnDeleteCategory.Size = new Size(801, 36);
            btnDeleteCategory.TabIndex = 1;
            btnDeleteCategory.Text = "DELETE SELECTED";
            btnDeleteCategory.Type = MaterialButton.MaterialButtonType.Text;
            btnDeleteCategory.UseAccentColor = false;
            btnDeleteCategory.UseVisualStyleBackColor = true;
            btnDeleteCategory.Click += BtnDeleteCategory_Click;
            // 
            // cardEditArea
            // 
            cardEditArea.BackColor = Color.FromArgb(255, 255, 255);
            cardEditArea.Controls.Add(lblError);
            cardEditArea.Controls.Add(btnClearSelection);
            cardEditArea.Controls.Add(pnlColorPreview);
            cardEditArea.Controls.Add(txtColorHex);
            cardEditArea.Controls.Add(lblEditHeader);
            cardEditArea.Controls.Add(txtName);
            cardEditArea.Controls.Add(btnSaveCategory);
            cardEditArea.Depth = 0;
            cardEditArea.ForeColor = Color.FromArgb(222, 0, 0, 0);
            cardEditArea.Location = new Point(13, 241);
            cardEditArea.Margin = new Padding(10);
            cardEditArea.MouseState = MaterialSkin.MouseState.HOVER;
            cardEditArea.Name = "cardEditArea";
            cardEditArea.Padding = new Padding(14);
            cardEditArea.Size = new Size(801, 278);
            cardEditArea.TabIndex = 0;
            // 
            // lblError
            // 
            lblError.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblError.Depth = 0;
            lblError.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblError.ForeColor = Color.FromArgb(180, 0, 0);
            lblError.HighEmphasis = true;
            lblError.Location = new Point(17, 212);
            lblError.MouseState = MaterialSkin.MouseState.HOVER;
            lblError.Name = "lblError";
            lblError.Size = new Size(767, 55);
            lblError.TabIndex = 6;
            lblError.Visible = false;
            // 
            // btnClearSelection
            // 
            btnClearSelection.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClearSelection.AutoSize = false;
            btnClearSelection.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnClearSelection.Density = MaterialButton.MaterialButtonDensity.Default;
            btnClearSelection.Depth = 0;
            btnClearSelection.HighEmphasis = false;
            btnClearSelection.Icon = null;
            btnClearSelection.Location = new Point(462, 170);
            btnClearSelection.Margin = new Padding(4, 6, 4, 6);
            btnClearSelection.MouseState = MaterialSkin.MouseState.HOVER;
            btnClearSelection.Name = "btnClearSelection";
            btnClearSelection.NoAccentTextColor = Color.Empty;
            btnClearSelection.Size = new Size(122, 36);
            btnClearSelection.TabIndex = 5;
            btnClearSelection.Text = "Clear / New";
            btnClearSelection.Type = MaterialButton.MaterialButtonType.Outlined;
            btnClearSelection.UseAccentColor = false;
            btnClearSelection.UseVisualStyleBackColor = true;
            btnClearSelection.Click += BtnClearSelection_Click;
            // 
            // pnlColorPreview
            // 
            pnlColorPreview.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlColorPreview.BorderStyle = BorderStyle.FixedSingle;
            pnlColorPreview.Location = new Point(693, 105);
            pnlColorPreview.Name = "pnlColorPreview";
            pnlColorPreview.Size = new Size(91, 48);
            pnlColorPreview.TabIndex = 3;
            // 
            // txtColorHex
            // 
            txtColorHex.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtColorHex.AnimateReadOnly = false;
            txtColorHex.BackgroundImageLayout = ImageLayout.None;
            txtColorHex.CharacterCasing = CharacterCasing.Upper;
            txtColorHex.Depth = 0;
            txtColorHex.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtColorHex.HideSelection = true;
            txtColorHex.Hint = "#RRGGBB";
            txtColorHex.LeadingIcon = null;
            txtColorHex.Location = new Point(17, 105);
            txtColorHex.MaxLength = 7;
            txtColorHex.MouseState = MaterialSkin.MouseState.OUT;
            txtColorHex.Name = "txtColorHex";
            txtColorHex.PasswordChar = '\0';
            txtColorHex.PrefixSuffixText = null;
            txtColorHex.ReadOnly = false;
            txtColorHex.RightToLeft = RightToLeft.No;
            txtColorHex.SelectedText = "";
            txtColorHex.SelectionLength = 0;
            txtColorHex.SelectionStart = 0;
            txtColorHex.ShortcutsEnabled = true;
            txtColorHex.Size = new Size(670, 48);
            txtColorHex.TabIndex = 2;
            txtColorHex.TabStop = false;
            txtColorHex.TextAlign = HorizontalAlignment.Left;
            txtColorHex.TrailingIcon = null;
            txtColorHex.UseSystemPasswordChar = false;
            txtColorHex.TextChanged += TxtColorHex_TextChanged;
            // 
            // lblEditHeader
            // 
            lblEditHeader.AutoSize = true;
            lblEditHeader.Depth = 0;
            lblEditHeader.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblEditHeader.Location = new Point(17, 14);
            lblEditHeader.MouseState = MaterialSkin.MouseState.HOVER;
            lblEditHeader.Name = "lblEditHeader";
            lblEditHeader.Size = new Size(138, 19);
            lblEditHeader.TabIndex = 0;
            lblEditHeader.Text = "Add / Edit Category";
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtName.AnimateReadOnly = false;
            txtName.BackgroundImageLayout = ImageLayout.None;
            txtName.CharacterCasing = CharacterCasing.Normal;
            txtName.Depth = 0;
            txtName.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtName.HideSelection = true;
            txtName.Hint = "Category Name";
            txtName.LeadingIcon = null;
            txtName.Location = new Point(17, 46);
            txtName.MaxLength = 100;
            txtName.MouseState = MaterialSkin.MouseState.OUT;
            txtName.Name = "txtName";
            txtName.PasswordChar = '\0';
            txtName.PrefixSuffixText = null;
            txtName.ReadOnly = false;
            txtName.RightToLeft = RightToLeft.No;
            txtName.SelectedText = "";
            txtName.SelectionLength = 0;
            txtName.SelectionStart = 0;
            txtName.ShortcutsEnabled = true;
            txtName.Size = new Size(767, 48);
            txtName.TabIndex = 1;
            txtName.TabStop = false;
            txtName.TextAlign = HorizontalAlignment.Left;
            txtName.TrailingIcon = null;
            txtName.UseSystemPasswordChar = false;
            // 
            // btnSaveCategory
            // 
            btnSaveCategory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveCategory.AutoSize = false;
            btnSaveCategory.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSaveCategory.Density = MaterialButton.MaterialButtonDensity.Default;
            btnSaveCategory.Depth = 0;
            btnSaveCategory.HighEmphasis = true;
            btnSaveCategory.Icon = null;
            btnSaveCategory.Location = new Point(592, 170);
            btnSaveCategory.Margin = new Padding(4, 6, 4, 6);
            btnSaveCategory.MouseState = MaterialSkin.MouseState.HOVER;
            btnSaveCategory.Name = "btnSaveCategory";
            btnSaveCategory.NoAccentTextColor = Color.Empty;
            btnSaveCategory.Size = new Size(192, 36);
            btnSaveCategory.TabIndex = 4;
            btnSaveCategory.Text = "Add Category";
            btnSaveCategory.Type = MaterialButton.MaterialButtonType.Contained;
            btnSaveCategory.UseAccentColor = true;
            btnSaveCategory.UseVisualStyleBackColor = true;
            btnSaveCategory.Click += BtnSaveCategory_Click;
            // 
            // CategoryManagerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(827, 532);
            Controls.Add(cardEditArea);
            Controls.Add(pnlListArea);
            FormStyle = FormStyles.StatusAndActionBar_None;
            Name = "CategoryManagerForm";
            Padding = new Padding(3, 0, 3, 3);
            Sizable = false;
            WindowState = FormWindowState.Maximized;
            Load += CategoryManagerForm_Load;
            pnlListArea.ResumeLayout(false);
            cardEditArea.ResumeLayout(false);
            cardEditArea.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        // --- Field declarations (ensure these match controls used) ---
        private Panel pnlListArea;
        private MaterialSkin.Controls.MaterialListView lstCategories;
        private ColumnHeader colName;
        private ColumnHeader colColor;
        private MaterialSkin.Controls.MaterialButton btnDeleteCategory;
        private MaterialSkin.Controls.MaterialCard cardEditArea;
        private MaterialSkin.Controls.MaterialLabel lblError;
        private MaterialSkin.Controls.MaterialButton btnClearSelection;
        private Panel pnlColorPreview;
        private MaterialSkin.Controls.MaterialTextBox2 txtColorHex;
        private MaterialSkin.Controls.MaterialLabel lblEditHeader;
        private MaterialSkin.Controls.MaterialTextBox2 txtName;
        private MaterialSkin.Controls.MaterialButton btnSaveCategory;

    }
}