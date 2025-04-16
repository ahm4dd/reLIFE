// Core .NET & WinForms
using System;
using System.Windows.Forms;

// Project References
using reLIFE.BusinessLogic.Data;           // For DbHelper
using reLIFE.BusinessLogic.Repositories;   // For Repositories
using reLIFE.BusinessLogic.Security;       // For PasswordHasher
using reLIFE.BusinessLogic.Services;       // For Services
using reLIFE.Core.Models;           // For User model
using reLIFE.WinFormsUI;              // For your form classes


namespace reLIFE.WinFormsUI
{
    internal static class Program
    {
        // NOTE: CurrentUser is not strictly needed in Program.cs in this pattern,
        //       as the LoginForm will handle passing it to MainForm.
        // public static User? CurrentUser { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AuthService? authService = null;

            try
            {
                // --- Step 1: Get Connection String & Create Auth Service ---
                string connectionString = DbHelper.GetConnectionString();
                var passwordHasher = new PasswordHasher();
                var userRepository = new UserRepository(connectionString);
                authService = new AuthService(userRepository, passwordHasher);

                // --- Step 2: Run the Login Form as the Primary Form ---
                // The login form itself will handle launching the MainForm on success
                Application.Run(new LoginForm(authService)); //LoginForm IS the main application form initially

            }
            catch (InvalidOperationException configEx)
            {
                Console.WriteLine($"CONFIGURATION ERROR: {configEx}");
                MessageBox.Show($"Fatal Configuration Error:\n{configEx.Message}\n\nApplication cannot start. Please check 'appsettings.json'.",
                                "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ApplicationException appEx)
            {
                Console.WriteLine($"STARTUP APPLICATION ERROR: {appEx}");
                MessageBox.Show($"A critical application error occurred during startup:\n{appEx.Message}\n\nPlease contact support.",
                                "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UNEXPECTED STARTUP ERROR: {ex}");
                MessageBox.Show($"An unexpected error occurred during application startup:\n{ex.Message}",
                                "Unexpected Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}