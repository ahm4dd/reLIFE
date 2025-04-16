using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using reLIFE.BusinessLogic.Repositories;
using reLIFE.BusinessLogic.Security;
using reLIFE.Core.Models;

namespace reLIFE.BusinessLogic.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;

        // Constructor injecting dependencies
        public AuthService(UserRepository userRepository, PasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        /// <summary>
        /// Attempts to log in a user with the provided credentials.
        /// </summary>
        /// <param name="username">The username entered by the user.</param>
        /// <param name="password">The password entered by the user.</param>
        /// <returns>The User object if login is successful; otherwise, null.</returns>
        public User? Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                // Or throw ArgumentException, depending on desired behavior
                return null;
            }

            try
            {
                // Find the user by username
                var user = _userRepository.GetUserByUsername(username);

                // Check if user exists and if the password is correct
                if (user != null && _passwordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                {
                    // Login successful
                    // Optional: Update LastLoginAt (requires an UpdateUser method in UserRepository)
                    // _userRepository.UpdateLastLogin(user.Id);
                    return user;
                }
                else
                {
                    // Login failed (user not found or incorrect password)
                    return null;
                }
            }
            catch (ApplicationException appEx) // Catch exceptions propagated from repository
            {
                Console.WriteLine($"Authentication error (repository): {appEx.Message}");
                // Log the full exception appEx somewhere
                return null; // Treat repository errors as login failure
            }
            catch (Exception ex) // Catch unexpected errors
            {
                Console.WriteLine($"Unexpected error during login: {ex.Message}");
                // Log the full exception ex somewhere
                return null; // Treat unexpected errors as login failure
            }
        }

        /// <summary>
        /// Attempts to register a new user account.
        /// </summary>
        /// <param name="username">The desired username.</param>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's chosen password.</param>
        /// <returns>The newly created User object.</returns>
        /// <exception cref="ArgumentException">Thrown if input parameters are invalid (e.g., empty).</exception>
        /// <exception cref="InvalidOperationException">Thrown if the username or email is already taken.</exception>
        /// <exception cref="ApplicationException">Thrown if a database error occurs during registration.</exception>
        public User Register(string username, string email, string password)
        {
            // --- Basic Input Validation ---
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.", nameof(username));
            if (string.IsNullOrWhiteSpace(email)) // Add more robust email validation if needed
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            if (string.IsNullOrWhiteSpace(password)) // Add password complexity rules if needed
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            try
            {
                // --- Check for Duplicates ---
                // Consider performance: One DB call might be better using OR, but two calls is clearer logic
                if (_userRepository.GetUserByUsername(username) != null)
                {
                    throw new InvalidOperationException($"Username '{username}' is already taken.");
                }
                if (_userRepository.GetUserByEmail(email) != null)
                {
                    throw new InvalidOperationException($"Email '{email}' is already registered.");
                }

                // --- Hash Password ---
                string salt;
                string hash = _passwordHasher.HashPassword(password, out salt);

                // --- Create User Object ---
                var newUser = new User
                {
                    Username = username.Trim(), // Trim whitespace
                    Email = email.Trim().ToLowerInvariant(), // Store email consistently (lowercase)
                    PasswordHash = hash,
                    PasswordSalt = salt
                    // CreatedAt is set by the database default
                };

                // --- Add User to Database ---
                // AddUser might throw InvalidOperationException for duplicates (race condition)
                // or ApplicationException for DB errors. Let these propagate up.
                var createdUser = _userRepository.AddUser(newUser);

                return createdUser;
            }
            catch (InvalidOperationException opEx) // Catch username/email exists errors (from checks or AddUser race condition)
            {
                // Log opEx?
                Console.WriteLine($"Registration validation failed: {opEx.Message}");
                // Re-throw the specific exception so UI can handle it distinctly
                throw;
            }
            catch (ApplicationException appEx) // Catch database errors propagated from AddUser
            {
                Console.WriteLine($"Registration error (repository): {appEx.Message}");
                // Log the full exception appEx somewhere
                throw new ApplicationException("A database error occurred during registration. Please try again later.", appEx);
            }
            catch (Exception ex) // Catch unexpected errors
            {
                Console.WriteLine($"Unexpected error during registration: {ex.Message}");
                // Log the full exception ex somewhere
                throw new ApplicationException("An unexpected error occurred during registration.", ex);
            }
        }
    }
}
