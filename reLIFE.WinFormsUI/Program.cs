// Core .NET & WinForms
using System;
using System.Windows.Forms;

// Project References
using reLIFE.BusinessLogic.Data;           // For DbHelper
using reLIFE.BusinessLogic.Repositories;   // For Repositories
using reLIFE.BusinessLogic.Security;       // For PasswordHasher
using reLIFE.BusinessLogic.Services;       // For Services
using reLIFE.Core.Models;           // For User model
using reLIFE.WinFormsUI.Forms;      // For your form classes
using Microsoft.Data.SqlClient;           // Needed for connection test

namespace reLIFE.WinFormsUI
{
    internal static class Program
    {
        public static User? CurrentUser { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Declare services needed - initialize within try block
            AuthService? authService = null;
            EventService? eventService = null;
            CategoryService? categoryService = null;
            ReminderService? reminderService = null;
            UserService? userService = null;
            string connectionString = string.Empty;

            try
            {
                // --- Step 1: Get Connection String ---
                connectionString = DbHelper.GetConnectionString();
                Console.WriteLine($"Loaded Connection String: [Length={connectionString.Length}]");

                // --- Optional Step 1.5: Test Connection ---
#if DEBUG
                try
                {
                    Console.WriteLine("Attempting database connection test...");
                    using (var testConnection = new SqlConnection(connectionString)) { testConnection.Open(); }
                    Console.WriteLine("Database connection test successful.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"!!! Database Connection Test FAILED: {ex.Message}");
                    MessageBox.Show($"Database Connection Failed:\n{ex.Message}\n\nPlease check connection string in appsettings.json and ensure SQL Server is running and accessible.",
                                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
#endif
                // --- End Connection Test ---

                // --- Step 2: Manually Create ALL Dependencies ---
                var passwordHasher = new PasswordHasher(); // Create once

                // Repositories
                var userRepository = new UserRepository(connectionString);
                var categoryRepository = new CategoryRepository(connectionString);
                var eventRepository = new EventRepository(connectionString);
                var archivedEventRepository = new ArchivedEventRepository(connectionString);
                var reminderRepository = new ReminderRepository(connectionString);

                // Services (Inject dependencies)
                authService = new AuthService(userRepository, passwordHasher);
                userService = new UserService(userRepository, passwordHasher); // Pass hasher here too
                categoryService = new CategoryService(categoryRepository);
                reminderService = new ReminderService(reminderRepository, eventRepository);
                eventService = new EventService(eventRepository, categoryRepository, archivedEventRepository, reminderRepository);


                // --- Step 3: Application Loop (Login/Register/Main) ---
                while (true)
                {
                    CurrentUser = null;
                    DialogResult formResult = DialogResult.None;

                    // Show Login Form
                    using (var loginForm = new LoginForm(authService)) // Pass AuthService
                    {
                        formResult = loginForm.ShowDialog();

                        if (formResult == DialogResult.OK)
                        {
                            CurrentUser = loginForm.LoggedInUser;
                            if (CurrentUser != null) { break; } // Exit loop on success
                            else { MessageBox.Show("Login error: User data unavailable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                        }
                        else if (formResult == DialogResult.Retry) // Signal to Register
                        {
                            using (var registrationForm = new RegistrationForm(authService)) { registrationForm.ShowDialog(); }
                            // Loop continues back to Login
                        }
                        else // User cancelled/closed Login
                        {
                            return; // Exit application
                        }
                    }
                } // End while loop

                // --- Step 4: Run Main Dashboard (only if login succeeded) ---
                if (CurrentUser != null)
                {
                    // Ensure all services were created (defensive check)
                    if (eventService == null || categoryService == null || reminderService == null || userService == null || authService == null)
                    {
                        throw new InvalidOperationException("One or more required services failed to initialize before launching main form.");
                    }

                    // Create and run the MainForm, passing all needed services
                    MainForm mainDashboard = new MainForm(
                        CurrentUser,
                        eventService,
                        categoryService,
                        reminderService,
                        authService,    // Needed for logout
                        userService,    // Needed for account settings
                        archivedEventRepository // Passed directly for archive view
                    );

                    Application.Run(mainDashboard); // Blocks until closed

                    // --- Step 5: Handle Logout ---
                    if (mainDashboard.DialogResult == DialogResult.Abort) // Logout signal
                    {
                        Application.Restart();
                        Environment.Exit(0);
                    }
                }
            }
            catch (InvalidOperationException configEx) // Catch critical config/init errors
            {
                Console.WriteLine($"CONFIGURATION/INIT ERROR: {configEx.InnerException?.Message ?? configEx.Message}"); // Show inner exception if available
                MessageBox.Show($"Fatal Application Error:\n{configEx.Message}\n\nApplication cannot start.",
                                "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ApplicationException appEx) // Catch startup DB errors etc.
            {
                Console.WriteLine($"STARTUP APPLICATION ERROR: {appEx.InnerException?.Message ?? appEx.Message}");
                MessageBox.Show($"A critical application error occurred during startup:\n{appEx.Message}",
                                "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex) // Catch all other unexpected startup errors
            {
                Console.WriteLine($"UNEXPECTED STARTUP ERROR: {ex}");
                MessageBox.Show($"An unexpected error occurred during application startup:\n{ex.Message}",
                                "Unexpected Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}