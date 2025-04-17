using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace reLIFE.BusinessLogic.Validators
{
    public static class Validation
    {
        // Define constraints
        private const int MinUsernameLength = 3;
        private const int MaxUsernameLength = 100; // Should match DB
        private const int MinPasswordLength = 8; // Example minimum length
        private const int MaxEmailLength = 255;  // Should match DB

        // --- Regular Expressions ---
        // Username: Starts and ends with alphanumeric, contains only alphanumeric.
        private const string UsernameRegexPattern = @"^[a-zA-Z0-9]+$";

        // Email: Reasonably strict pattern balancing RFC adherence and practicality.
        // - Local part: Allows alphanumeric and common special chars like .!#$%&'*+-/=?^_`{|}~
        // - Domain part: Allows alphanumeric and hyphen, requires at least one dot.
        // - TLD: Requires at least two letters.
        // NOTE: Perfect email regex is complex; this covers most valid cases without excessive strictness.
        private const string EmailRegexPattern = @"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$";
        // Alternate stricter (but more complex pattern - consider if needed):
        // @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)+)(?<!\.)@([a-z0-9][\w-]*\.)+[a-z0-9]{2,}$"

        /// <summary>
        /// Validates a username (alphanumeric only).
        /// </summary>
        /// <param name="username">The username string to validate.</param>
        /// <returns>A tuple containing a boolean indicating validity and an error message if invalid.</returns>
        public static (bool IsValid, string? ErrorMessage) ValidateUsername(string? username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return (false, "Username cannot be empty.");
            }
            if (username.Length < MinUsernameLength)
            {
                return (false, $"Username must be at least {MinUsernameLength} characters long.");
            }
            if (username.Length > MaxUsernameLength)
            {
                return (false, $"Username cannot exceed {MaxUsernameLength} characters.");
            }

            // *** Stricter Character Validation ***
            if (!Regex.IsMatch(username, UsernameRegexPattern))
            {
                return (false, "Username can only contain letters (a-z, A-Z) and numbers (0-9).");
            }

            return (true, null); // Valid
        }

        /// <summary>
        /// Validates a password (basic checks).
        /// </summary>
        /// <param name="password">The password string to validate.</param>
        /// <returns>A tuple containing a boolean indicating validity and an error message if invalid.</returns>
        public static (bool IsValid, string? ErrorMessage) ValidatePassword(string? password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return (false, "Password cannot be empty.");
            }
            if (password.Length < MinPasswordLength)
            {
                return (false, $"Password must be at least {MinPasswordLength} characters long.");
            }
            // Add more complexity rules here if desired
            return (true, null); // Valid
        }

        /// <summary>
        /// Validates an email address format using a Regular Expression.
        /// </summary>
        /// <param name="email">The email string to validate.</param>
        /// <returns>A tuple containing a boolean indicating validity and an error message if invalid.</returns>
        public static (bool IsValid, string? ErrorMessage) ValidateEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, "Email cannot be empty.");
            }

            if (email.Length > MaxEmailLength)
            {
                return (false, $"Email cannot exceed {MaxEmailLength} characters.");
            }

            // *** Use Regex for Email Validation ***
            // Using RegexOptions.IgnoreCase for case-insensitive matching (standard for emails)
            if (!Regex.IsMatch(email, EmailRegexPattern, RegexOptions.IgnoreCase))
            {
                return (false, "Please enter a valid email address format (e.g., user@example.com).");
            }

            // Note: Regex validation doesn't guarantee the email address *exists*, only
            // that its format appears valid according to the pattern.

            return (true, null); // Valid
        }
    }
}