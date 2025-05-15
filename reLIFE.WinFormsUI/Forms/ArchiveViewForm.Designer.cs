using MaterialSkin.Controls; // For MaterialSkin controls
using System.Windows.Forms;  // For standard controls like Panel, FlowLayoutPanel
using System.Drawing;      // For Size, Point, Color, etc.

namespace reLIFE.WinFormsUI.Forms
{
    partial class ArchiveViewForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlTopFilterBar = new Panel();
            lblArchiveFrom = new MaterialLabel();
            dtpArchiveFrom = new CustomControls.RJControls.RJDatePicker();
            lblArchiveTo = new MaterialLabel();
            dtpArchiveTo = new CustomControls.RJControls.RJDatePicker();
            btnApplyDateFilters = new MaterialButton();
            pnlCategoryFilterArea = new Panel();
            clbArchiveCategoryFilter = new MaterialCheckedListBox();
            btnApplyArchiveCategoryFilter = new MaterialButton();
            lblArchiveCategoryHeader = new MaterialLabel();
            flpArchivedEvents = new FlowLayoutPanel();
            pnlTopFilterBar.SuspendLayout();
            pnlCategoryFilterArea.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTopFilterBar
            // 
            pnlTopFilterBar.Controls.Add(lblArchiveFrom);
            pnlTopFilterBar.Controls.Add(dtpArchiveFrom);
            pnlTopFilterBar.Controls.Add(lblArchiveTo);
            pnlTopFilterBar.Controls.Add(dtpArchiveTo);
            pnlTopFilterBar.Controls.Add(btnApplyDateFilters);
            pnlTopFilterBar.Dock = DockStyle.Top;
            pnlTopFilterBar.Location = new Point(3, 0);
            pnlTopFilterBar.Name = "pnlTopFilterBar";
            pnlTopFilterBar.Padding = new Padding(10, 10, 10, 5);
            pnlTopFilterBar.Size = new Size(794, 55);
            pnlTopFilterBar.TabIndex = 0;
            // 
            // lblArchiveFrom
            // 
            lblArchiveFrom.AutoSize = true;
            lblArchiveFrom.Depth = 0;
            lblArchiveFrom.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblArchiveFrom.Location = new Point(13, 18);
            lblArchiveFrom.MouseState = MaterialSkin.MouseState.HOVER;
            lblArchiveFrom.Name = "lblArchiveFrom";
            lblArchiveFrom.Size = new Size(107, 19);
            lblArchiveFrom.TabIndex = 0;
            lblArchiveFrom.Text = "Archived From:";
            // 
            // dtpArchiveFrom
            // 
            dtpArchiveFrom.BorderColor = Color.PaleVioletRed;
            dtpArchiveFrom.BorderSize = 0;
            dtpArchiveFrom.Font = new Font("Segoe UI", 9.5F);
            dtpArchiveFrom.Location = new Point(125, 10);
            dtpArchiveFrom.MinimumSize = new Size(0, 35);
            dtpArchiveFrom.Name = "dtpArchiveFrom";
            dtpArchiveFrom.Size = new Size(180, 35);
            dtpArchiveFrom.SkinColor = Color.MediumSlateBlue;
            dtpArchiveFrom.TabIndex = 1;
            dtpArchiveFrom.TextColor = Color.White;
            // 
            // lblArchiveTo
            // 
            lblArchiveTo.AutoSize = true;
            lblArchiveTo.Depth = 0;
            lblArchiveTo.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblArchiveTo.Location = new Point(315, 18);
            lblArchiveTo.MouseState = MaterialSkin.MouseState.HOVER;
            lblArchiveTo.Name = "lblArchiveTo";
            lblArchiveTo.Size = new Size(89, 19);
            lblArchiveTo.TabIndex = 2;
            lblArchiveTo.Text = "Archived To:";
            // 
            // dtpArchiveTo
            // 
            dtpArchiveTo.BorderColor = Color.PaleVioletRed;
            dtpArchiveTo.BorderSize = 0;
            dtpArchiveTo.Font = new Font("Segoe UI", 9.5F);
            dtpArchiveTo.Location = new Point(408, 10);
            dtpArchiveTo.MinimumSize = new Size(0, 35);
            dtpArchiveTo.Name = "dtpArchiveTo";
            dtpArchiveTo.Size = new Size(180, 35);
            dtpArchiveTo.SkinColor = Color.MediumSlateBlue;
            dtpArchiveTo.TabIndex = 3;
            dtpArchiveTo.TextColor = Color.White;
            // 
            // btnApplyDateFilters
            // 
            btnApplyDateFilters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnApplyDateFilters.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnApplyDateFilters.Density = MaterialButton.MaterialButtonDensity.Default;
            btnApplyDateFilters.Depth = 0;
            btnApplyDateFilters.HighEmphasis = true;
            btnApplyDateFilters.Icon = null;
            btnApplyDateFilters.Location = new Point(650, 10);
            btnApplyDateFilters.Margin = new Padding(4, 6, 4, 6);
            btnApplyDateFilters.MouseState = MaterialSkin.MouseState.HOVER;
            btnApplyDateFilters.Name = "btnApplyDateFilters";
            btnApplyDateFilters.NoAccentTextColor = Color.Empty;
            btnApplyDateFilters.Size = new Size(130, 36);
            btnApplyDateFilters.TabIndex = 4;
            btnApplyDateFilters.Text = "Filter by Date";
            btnApplyDateFilters.Type = MaterialButton.MaterialButtonType.Contained;
            btnApplyDateFilters.UseAccentColor = false;
            btnApplyDateFilters.UseVisualStyleBackColor = true;
            btnApplyDateFilters.Click += btnApplyArchiveFilters_Click;
            // 
            // pnlCategoryFilterArea
            // 
            pnlCategoryFilterArea.Controls.Add(clbArchiveCategoryFilter);
            pnlCategoryFilterArea.Controls.Add(btnApplyArchiveCategoryFilter);
            pnlCategoryFilterArea.Controls.Add(lblArchiveCategoryHeader);
            pnlCategoryFilterArea.Dock = DockStyle.Left;
            pnlCategoryFilterArea.Location = new Point(3, 55);
            pnlCategoryFilterArea.Name = "pnlCategoryFilterArea";
            pnlCategoryFilterArea.Padding = new Padding(10, 5, 5, 10);
            pnlCategoryFilterArea.Size = new Size(250, 392);
            pnlCategoryFilterArea.TabIndex = 1;
            // 
            // clbArchiveCategoryFilter
            // 
            clbArchiveCategoryFilter.AutoScroll = true;
            clbArchiveCategoryFilter.BackColor = SystemColors.Control;
            clbArchiveCategoryFilter.Depth = 0;
            clbArchiveCategoryFilter.Dock = DockStyle.Fill;
            clbArchiveCategoryFilter.Location = new Point(10, 24);
            clbArchiveCategoryFilter.MouseState = MaterialSkin.MouseState.HOVER;
            clbArchiveCategoryFilter.Name = "clbArchiveCategoryFilter";
            clbArchiveCategoryFilter.Size = new Size(235, 322);
            clbArchiveCategoryFilter.Striped = false;
            clbArchiveCategoryFilter.StripeDarkColor = Color.Empty;
            clbArchiveCategoryFilter.TabIndex = 1;
            // 
            // btnApplyArchiveCategoryFilter
            // 
            btnApplyArchiveCategoryFilter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnApplyArchiveCategoryFilter.Density = MaterialButton.MaterialButtonDensity.Default;
            btnApplyArchiveCategoryFilter.Depth = 0;
            btnApplyArchiveCategoryFilter.Dock = DockStyle.Bottom;
            btnApplyArchiveCategoryFilter.HighEmphasis = true;
            btnApplyArchiveCategoryFilter.Icon = null;
            btnApplyArchiveCategoryFilter.Location = new Point(10, 346);
            btnApplyArchiveCategoryFilter.Margin = new Padding(4, 6, 4, 6);
            btnApplyArchiveCategoryFilter.MouseState = MaterialSkin.MouseState.HOVER;
            btnApplyArchiveCategoryFilter.Name = "btnApplyArchiveCategoryFilter";
            btnApplyArchiveCategoryFilter.NoAccentTextColor = Color.Empty;
            btnApplyArchiveCategoryFilter.Size = new Size(235, 36);
            btnApplyArchiveCategoryFilter.TabIndex = 2;
            btnApplyArchiveCategoryFilter.Text = "Apply Category Filter";
            btnApplyArchiveCategoryFilter.Type = MaterialButton.MaterialButtonType.Contained;
            btnApplyArchiveCategoryFilter.UseAccentColor = false;
            btnApplyArchiveCategoryFilter.UseVisualStyleBackColor = true;
            btnApplyArchiveCategoryFilter.Click += btnApplyArchiveCategoryFilter_Click;
            // 
            // lblArchiveCategoryHeader
            // 
            lblArchiveCategoryHeader.AutoSize = true;
            lblArchiveCategoryHeader.Depth = 0;
            lblArchiveCategoryHeader.Dock = DockStyle.Top;
            lblArchiveCategoryHeader.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblArchiveCategoryHeader.Location = new Point(10, 5);
            lblArchiveCategoryHeader.MouseState = MaterialSkin.MouseState.HOVER;
            lblArchiveCategoryHeader.Name = "lblArchiveCategoryHeader";
            lblArchiveCategoryHeader.Padding = new Padding(0, 0, 0, 5);
            lblArchiveCategoryHeader.Size = new Size(128, 19);
            lblArchiveCategoryHeader.TabIndex = 0;
            lblArchiveCategoryHeader.Text = "Filter by Category:";
            // 
            // flpArchivedEvents
            // 
            flpArchivedEvents.AutoScroll = true;
            flpArchivedEvents.Dock = DockStyle.Fill;
            flpArchivedEvents.FlowDirection = FlowDirection.TopDown;
            flpArchivedEvents.Location = new Point(253, 55);
            flpArchivedEvents.Name = "flpArchivedEvents";
            flpArchivedEvents.Padding = new Padding(10);
            flpArchivedEvents.Size = new Size(544, 392);
            flpArchivedEvents.TabIndex = 2;
            flpArchivedEvents.WrapContents = false;
            // 
            // ArchiveViewForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(flpArchivedEvents);
            Controls.Add(pnlCategoryFilterArea);
            Controls.Add(pnlTopFilterBar);
            FormStyle = FormStyles.StatusAndActionBar_None;
            Name = "ArchiveViewForm";
            Padding = new Padding(3, 0, 3, 3);
            Text = "Archived Events";
            Load += ArchiveViewForm_Load;
            pnlTopFilterBar.ResumeLayout(false);
            pnlTopFilterBar.PerformLayout();
            pnlCategoryFilterArea.ResumeLayout(false);
            pnlCategoryFilterArea.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        // --- Field Declarations ---
        private System.Windows.Forms.Panel pnlTopFilterBar;
        private MaterialSkin.Controls.MaterialLabel lblArchiveFrom;
        private CustomControls.RJControls.RJDatePicker dtpArchiveFrom;
        private MaterialSkin.Controls.MaterialLabel lblArchiveTo;
        private CustomControls.RJControls.RJDatePicker dtpArchiveTo;
        private MaterialSkin.Controls.MaterialButton btnApplyDateFilters;
        private System.Windows.Forms.Panel pnlCategoryFilterArea;
        private MaterialSkin.Controls.MaterialLabel lblArchiveCategoryHeader;
        private MaterialSkin.Controls.MaterialCheckedListBox clbArchiveCategoryFilter;
        private MaterialSkin.Controls.MaterialButton btnApplyArchiveCategoryFilter;
        private System.Windows.Forms.FlowLayoutPanel flpArchivedEvents;
    }
}