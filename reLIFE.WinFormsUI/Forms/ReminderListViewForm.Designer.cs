using MaterialSkin.Controls;
using System.Windows.Forms;
using System.Drawing;

namespace reLIFE.WinFormsUI.Forms
{
    partial class ReminderListViewForm
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
            // Declare Controls
            this.flpReminders = new System.Windows.Forms.FlowLayoutPanel();
            // Add declarations for any other controls you might add later (e.g., Refresh button)

            // Suspend Layout
            this.SuspendLayout();

            //
            // flpReminders (FlowLayoutPanel to hold reminder cards)
            //
            this.flpReminders.AutoScroll = true;        // Enable scrolling
            this.flpReminders.Dock = System.Windows.Forms.DockStyle.Fill; // Fill the entire form
            this.flpReminders.FlowDirection = System.Windows.Forms.FlowDirection.TopDown; // Stack cards vertically
            this.flpReminders.Location = new System.Drawing.Point(3, 0); // Respect Form's Padding if any
            this.flpReminders.Name = "flpReminders";
            this.flpReminders.Padding = new System.Windows.Forms.Padding(10); // Padding around the cards
            this.flpReminders.Size = new System.Drawing.Size(794, 447); // Will be adjusted by Dock = Fill
            this.flpReminders.TabIndex = 0;
            this.flpReminders.WrapContents = false; // Prevent wrapping

            //
            // ReminderListViewForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450); // Set a default design-time size
            this.Controls.Add(this.flpReminders); // Add the main panel
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.StatusAndActionBar_None; // Style for embedding
            this.Name = "ReminderListViewForm";
            this.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3); // Standard MaterialForm padding
            this.Text = "Active Reminders"; // Set a meaningful default title
            this.Load += new System.EventHandler(this.ReminderListViewForm_Load); // Hook load event

            // Resume Layout
            this.ResumeLayout(false);
        }

        #endregion

        // --- Declare Field ---
        private System.Windows.Forms.FlowLayoutPanel flpReminders;
        // Add fields for any other controls here
    }
}