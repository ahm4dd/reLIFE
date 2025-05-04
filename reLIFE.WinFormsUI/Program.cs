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
using reLIFE.WinFormsUI.Helpers;    // For ThemeHelper
using MaterialSkin;                     // For MaterialSkinManager
using Microsoft.Data.SqlClient;           // Needed for connection test

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

            // *** Initialize MaterialSkinManager GLOBALLY ***
            var skinManager = MaterialSkinManager.Instance;
            // Set initial theme/scheme (e.g., load from settings or default)
            // In a real app, load saved settings here:
            // string savedTheme = Properties.Settings.Default.ThemeName ?? "Light";
            // string savedScheme = Properties.Settings.Default.SchemeName ?? "Blue";
            // ThemeHelper.ApplyTheme(
            //     savedTheme == "Dark" ? MaterialSkinManager.Themes.DARK : MaterialSkinManager.Themes.LIGHT,
            //     ThemeHelper.GetSchemeByName(savedScheme),
            //     savedScheme
            // );
            // --- OR --- Set a hardcoded default for now:
            ThemeHelper.ApplyTheme(MaterialSkinManager.Themes.LIGHT, ThemeHelper.BlueScheme, "Blue");


            // Declare services needed - initialize within try block
            AuthService? authService = null;
            EventService? eventService = null;
            CategoryService? categoryService = null;
            ReminderService? reminderService = null;
            UserService? userService = null;
            string connectionString = string.Empty;
            ArchivedEventRepository? archivedEventRepository = null; // Declare repo needed by MainForm


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
                     return; // Exit if DB connection fails critically on startup
                }
#endif
                // --- End Connection Test ---

                // --- Step 2: Manually Create ALL Dependencies ---
                var passwordHasher = new PasswordHasher();

                // Repositories
                var userRepository = new UserRepository(connectionString);
                var categoryRepository = new CategoryRepository(connectionString);
                var eventRepository = new EventRepository(connectionString);
                archivedEventRepository = new ArchivedEventRepository(connectionString); // Assign to declared variable
                var reminderRepository = new ReminderRepository(connectionString);

                // Services
                authService = new AuthService(userRepository, passwordHasher);
                userService = new UserService(userRepository, passwordHasher); // Pass hasher here too
                categoryService = new CategoryService(categoryRepository);
                reminderService = new ReminderService(reminderRepository, eventRepository);
                eventService = new EventService(eventRepository, categoryRepository, archivedEventRepository, reminderRepository);


                // --- Step 3: Application Loop (Login/Register/Main) ---
                while (true) // Loop until Application.Exit or fatal error
                {
                    CurrentUser = null; // Reset user for this login attempt
                    DialogResult formResult = DialogResult.None;

                    // Show Login Form
                    using (var loginForm = new LoginForm(authService))
                    {
                        // Add Login Form to Manager *before* showing
                        // Note: Individual forms should NOT call AddFormToManage in their constructors anymore
                        skinManager.AddFormToManage(loginForm); // Manage the login form appearance
                        formResult = loginForm.ShowDialog();
                        skinManager.RemoveFormToManage(loginForm); // Clean up manager reference after close

                        if (formResult == DialogResult.OK)
                        {
                            CurrentUser = loginForm.LoggedInUser;
                            if (CurrentUser != null) { break; } // Exit Login loop, proceed to Main Form
                            else { MessageBox.Show("Login error: User data unavailable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                        }
                        else if (formResult == DialogResult.Retry) // Signal from LoginForm to Register
                        {
                            using (var registrationForm = new RegistrationForm(authService))
                            {
                                skinManager.AddFormToManage(registrationForm); // Manage registration form appearance
                                registrationForm.ShowDialog();
                                skinManager.RemoveFormToManage(registrationForm); // Clean up
                                // Loop continues back to Login Form after registration closes
                            }
                        }
                        else // User cancelled or closed Login Form
                        {
                            return; // Exit application
                        }
                    }
                } // End while loop (exits only on successful login)


                // --- Step 4: Run Main Dashboard (only if login succeeded) ---
                if (CurrentUser != null)
                {
                    // Ensure all services needed by MainForm were created
                    if (eventService == null || categoryService == null || reminderService == null || userService == null || authService == null || archivedEventRepository == null)
                    {
                        throw new InvalidOperationException("One or more required services or repositories failed to initialize before launching main form.");
                    }

                    // Create the MainForm instance
                    MainForm mainDashboard = new MainForm(
                        CurrentUser,
                        eventService,
                        categoryService,
                        reminderService,
                        authService,    // Pass for logout/password change
                        userService,    // Pass for account settings
                        archivedEventRepository // Pass repo directly for archive view simplicity
                    );

                    // Add MainForm to manager *before* running
                    skinManager.AddFormToManage(mainDashboard);

                    Application.Run(mainDashboard); // Blocks here until mainDashboard closes

                    // Clean up manager reference (optional but good practice)
                    skinManager.RemoveFormToManage(mainDashboard);

                    // --- Step 5: Handle Logout ---
                    if (mainDashboard.DialogResult == DialogResult.Abort) // Our signal for logout
                    {
                        Application.Restart();
                        Environment.Exit(0); // Ensure current process exits cleanly after signalling restart
                    }
                    // Otherwise (if closed normally), the application exits naturally after Application.Run finishes.
                }
            }
            catch (InvalidOperationException configEx) // Catch critical config/init errors
            {
                Console.WriteLine($"CONFIGURATION/INIT ERROR: {configEx.InnerException?.Message ?? configEx.Message}");
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