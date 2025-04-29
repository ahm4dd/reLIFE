using reLIFE.BusinessLogic.Repositories;
using reLIFE.BusinessLogic.Validators;
using reLIFE.BusinessLogic.Security; // *** ADDED for PasswordHasher ***
using reLIFE.Core.Models;
using System;

namespace reLIFE.BusinessLogic.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher; // *** ADDED Dependency ***

        // *** MODIFIED Constructor ***
        public UserService(UserRepository userRepository, PasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher)); // Store hasher
        }

        // ... (Existing method: UpdateUserEmail) ...
        public bool UpdateUserEmail(int userId, string newEmail)
        {
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));
            if (string.IsNullOrWhiteSpace(newEmail)) throw new ArgumentException("New email cannot be empty.", nameof(newEmail));
            var emailValidation = Validation.ValidateEmail(newEmail);
            if (!emailValidation.IsValid) throw new ArgumentException(emailValidation.ErrorMessage ?? "Invalid email format.", nameof(newEmail));
            string cleanedEmail = newEmail.Trim().ToLowerInvariant();
            try
            {
                User? existingUser = _userRepository.GetUserByEmail(cleanedEmail);
                if (existingUser != null && existingUser.Id != userId) throw new InvalidOperationException($"The email address '{cleanedEmail}' is already registered to another account.");
                return _userRepository.UpdateUserEmail(userId, cleanedEmail);
            }
            catch (InvalidOperationException opEx) { Console.WriteLine($"Service Error (UpdateUserEmail - uniqueness): {opEx.Message}"); throw; }
            catch (ApplicationException appEx) { Console.WriteLine($"Service Error (UpdateUserEmail - repo): {appEx.Message}"); throw; }
            catch (Exception ex) { Console.WriteLine($"Unexpected Service Error (UpdateUserEmail): {ex.Message}"); throw new ApplicationException("An unexpected error occurred while updating the email address.", ex); }
        }


        // --- NEW Method: ChangePassword ---
        /// <summary>
        /// Changes the password for a specified user after verifying the current password.
        /// </summary>
        /// <param name="userId">The ID of the user changing their password.</param>
        /// <param name="currentPassword">The user's current password.</param>
        /// <param name="newPassword">The desired new password.</param>
        /// <returns>True if the password was successfully changed, false otherwise (e.g., incorrect current password).</returns>
        /// <exception cref="ArgumentException">Thrown if the new password format is invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user cannot be found.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));
            if (string.IsNullOrEmpty(currentPassword)) throw new ArgumentException("Current password cannot be empty.", nameof(currentPassword));
            if (string.IsNullOrEmpty(newPassword)) throw new ArgumentException("New password cannot be empty.", nameof(newPassword));

            // 1. Validate New Password Format/Complexity
            var passwordValidation = Validation.ValidatePassword(newPassword); // Use your validator
            if (!passwordValidation.IsValid)
            {
                throw new ArgumentException(passwordValidation.ErrorMessage ?? "New password is invalid.", nameof(newPassword));
            }

            // Prevent setting password same as current (optional but good practice)
            if (currentPassword == newPassword)
            {
                throw new ArgumentException("New password cannot be the same as the current password.", nameof(newPassword));
            }

            try
            {
                // 2. Get User's Current Hash and Salt
                // Note: GetUserById is needed. If UserRepository doesn't have it, add it.
                // For now, assume GetUserByUsername or GetUserByEmail can work if username/email is available,
                // but GetUserById is better. Let's assume it exists or needs adding to UserRepository.
                // We'll use a placeholder - you'll need to implement GetUserById if missing.
                User? user = _userRepository.GetUserById(userId); // <<< NEEDS IMPLEMENTATION IN REPO if not present
                if (user == null)
                {
                    throw new InvalidOperationException($"User with ID {userId} not found.");
                }

                // 3. Verify Current Password
                if (!_passwordHasher.VerifyPassword(currentPassword, user.PasswordHash, user.PasswordSalt))
                {
                    // Current password does not match
                    return false; // Indicate failure specifically due to wrong current password
                }

                // 4. Hash New Password
                string newSalt;
                string newHash = _passwordHasher.HashPassword(newPassword, out newSalt);

                // 5. Update Password in Repository
                bool success = _userRepository.UpdatePassword(userId, newHash, newSalt);
                if (!success)
                {
                    // This could happen if the user was deleted between Get and Update
                    throw new ApplicationException($"Failed to update password for user ID {userId}. User might no longer exist.");
                }

                return true; // Password changed successfully
            }
            catch (InvalidOperationException opEx) // Catch User not found
            {
                Console.WriteLine($"Service Error (ChangePassword - user lookup): {opEx.Message}");
                throw;
            }
            catch (ApplicationException appEx) // Catch DB errors from repo
            {
                Console.WriteLine($"Service Error (ChangePassword - repo): {appEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (ChangePassword): {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while changing the password.", ex);
            }
        }
        // Add GetUserById to UserRepository if needed:
        /*
        public User? GetUserById(int userId) {
            const string sql = "SELECT ... FROM Users WHERE Id = @UserId";
            // ... implementation similar to GetUserByUsername ...
        }
        */
    }
}