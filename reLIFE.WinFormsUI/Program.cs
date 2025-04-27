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

namespace reLIFE.WinFormsUI
{
    internal static class Program
    {
        /// <summary>
        /// Stores the currently logged-in user after successful authentication.
        /// </summary>
        public static User? CurrentUser { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Declare services needed throughout the application lifecycle
            AuthService? authService = null;
            EventService? eventService = null;
            CategoryService? categoryService = null;
            ReminderService? reminderService = null;

            try
            {
                // --- Step 1: Get Connection String ---
                string connectionString = DbHelper.GetConnectionString();

                // --- Step 2: Manually Create ALL Dependencies ---
                // Security
                var passwordHasher = new PasswordHasher();

                // Repositories (All need connection string)
                var userRepository = new UserRepository(connectionString);
                var categoryRepository = new CategoryRepository(connectionString);
                var eventRepository = new EventRepository(connectionString);
                var archivedEventRepository = new ArchivedEventRepository(connectionString); // New
                var reminderRepository = new ReminderRepository(connectionString);         // New

                // Services (Inject repositories)
                authService = new AuthService(userRepository, passwordHasher);
                categoryService = new CategoryService(categoryRepository);
                reminderService = new ReminderService(reminderRepository, eventRepository); // New (needs EventRepo too)
                eventService = new EventService(                                         // Modified constructor
                    eventRepository,
                    categoryRepository,
                    archivedEventRepository,
                    reminderRepository // Inject ReminderRepository directly
                    );

                // --- Step 3: Show Login Form Modally ---
                bool loggedInSuccessfully = false;
                using (var loginForm = new LoginForm(authService)) // Only needs AuthService
                {
                    DialogResult loginResult = loginForm.ShowDialog();

                    if (loginResult == DialogResult.OK)
                    {
                        CurrentUser = loginForm.LoggedInUser; // Get user from form property
                        if (CurrentUser != null)
                        {
                            loggedInSuccessfully = true;
                        }
                        else
                        {
                            MessageBox.Show("Login succeeded but user data could not be retrieved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    // else: User cancelled login, loggedInSuccessfully remains false
                }

                // --- Step 4: Run Main Form Only If Login Succeeded ---
                if (loggedInSuccessfully && CurrentUser != null)
                {
                    // Ensure all required services were created successfully before running main form
                    if (eventService == null || categoryService == null || reminderService == null)
                    {
                        throw new InvalidOperationException("One or more required services failed to initialize.");
                    }

                    // Run the main application form, passing the user and necessary services
                    // MainForm constructor needs updating to accept these
                    Application.Run(new MainForm(CurrentUser, eventService, categoryService, reminderService));
                }
                // else: If login failed or was cancelled, the application exits here.

            }
            catch (InvalidOperationException configEx) // Catch critical config errors from DbHelper
            {
                Console.WriteLine($"CONFIGURATION ERROR: {configEx}");
                MessageBox.Show($"Fatal Configuration Error:\n{configEx.Message}\n\nApplication cannot start. Please check 'appsettings.json'.",
                                "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ApplicationException appEx) // Catch potential connection/DB errors during startup/service creation
            {
                Console.WriteLine($"STARTUP APPLICATION ERROR: {appEx}");
                MessageBox.Show($"A critical application error occurred during startup:\n{appEx.Message}\n\nPlease contact support.",
                                "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex) // Catch any other unexpected startup errors
            {
                Console.WriteLine($"UNEXPECTED STARTUP ERROR: {ex}");
                MessageBox.Show($"An unexpected error occurred during application startup:\n{ex.Message}",
                                "Unexpected Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}