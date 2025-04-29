using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using reLIFE.Core.Models;
using System.Data;


namespace reLIFE.BusinessLogic.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        // Constructor receives the connection string.
        // This should be read from App.config in the UI layer and passed down.
        public UserRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Database connection string cannot be null or empty.");
            }
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user object containing username, email, hash, and salt.</param>
        /// <returns>The User object updated with the generated ID and CreatedAt timestamp.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the username or email already exists.</exception>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public User AddUser(User user)
        {
            const string sql = @"
                INSERT INTO Users (Username, Email, PasswordHash, PasswordSalt)
                OUTPUT INSERTED.Id, INSERTED.CreatedAt
                VALUES (@Username, @Email, @PasswordHash, @PasswordSalt);";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    // Add parameters securely to prevent SQL Injection
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Read the row returned by OUTPUT
                        {
                            user.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            user.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                            return user;
                        }
                        else
                        {
                            // Should not happen if insert was successful and OUTPUT clause is correct
                            throw new InvalidOperationException("Failed to retrieve inserted user data after insert.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Check for specific SQL Server error number for UNIQUE constraint violation
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    // Determine if it was Username or Email based on the message (less reliable)
                    // Or simply throw a generic "already exists" error
                    throw new InvalidOperationException($"Username '{user.Username}' or Email '{user.Email}' already exists.", ex);
                }
                // Log ex details (implement proper logging later)
                Console.WriteLine($"SQL Error adding user: {ex.Message} (Number: {ex.Number})");
                throw new ApplicationException("An error occurred while adding the user to the database.", ex);
            }
            catch (Exception ex) // Catch any other unexpected exceptions
            {
                Console.WriteLine($"General Error adding user: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while adding the user.", ex);
            }
        }

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The User object if found; otherwise, null.</returns>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public User? GetUserByUsername(string username)
        {
            const string sql = @"
                SELECT Id, Username, Email, PasswordHash, PasswordSalt, CreatedAt
                FROM Users
                WHERE Username = @Username;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Check if a row was returned
                        {
                            return MapReaderToUser(reader);
                        }
                        else
                        {
                            return null; // User not found
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error getting user by username: {ex.Message}");
                throw new ApplicationException("An error occurred while retrieving user data.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error getting user by username: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while retrieving user data.", ex);
            }
        }

        /// <summary>
        /// Retrieves a user by their email address. Used for registration checks.
        /// </summary>
        /// <param name="email">The email to search for.</param>
        /// <returns>The User object if found; otherwise, null.</returns>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public User? GetUserByEmail(string email)
        {
            const string sql = @"
                SELECT Id, Username, Email, PasswordHash, PasswordSalt, CreatedAt
                FROM Users
                WHERE Email = @Email;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToUser(reader);
                        }
                        else
                        {
                            return null; // User not found
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error getting user by email: {ex.Message}");
                throw new ApplicationException("An error occurred while retrieving user data.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error getting user by email: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while retrieving user data.", ex);
            }
        }

        /// <summary>
        /// Updates the email address for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="newEmail">The new email address.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the new email violates the unique constraint.</exception>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public bool UpdateUserEmail(int userId, string newEmail)
        {
            const string sql = @"
        UPDATE Users
        SET Email = @NewEmail
        WHERE Id = @UserId;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NewEmail", newEmail);
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                // Check for unique constraint violation on Email
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new InvalidOperationException($"The email address '{newEmail}' is already in use.", ex);
                }
                Console.WriteLine($"SQL Error updating user email: {ex.Message}");
                throw new ApplicationException($"A database error occurred while updating email for user ID {userId}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error updating user email: {ex.Message}");
                throw new ApplicationException($"An unexpected error occurred while updating email for user ID {userId}.", ex);
            }
        }

        /// <summary>
        /// Updates the password hash and salt for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose password to update.</param>
        /// <param name="newHash">The new password hash (hex string).</param>
        /// <param name="newSalt">The new password salt (hex string).</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public bool UpdatePassword(int userId, string newHash, string newSalt)
        {
            // Validate inputs (basic length check based on DB schema)
            if (string.IsNullOrEmpty(newHash) || newHash.Length > 64) // Max length from schema
                throw new ArgumentException("Invalid new password hash provided.", nameof(newHash));
            if (string.IsNullOrEmpty(newSalt) || newSalt.Length > 32) // Max length from schema
                throw new ArgumentException("Invalid new password salt provided.", nameof(newSalt));

            const string sql = @"
        UPDATE Users
        SET PasswordHash = @NewHash,
            PasswordSalt = @NewSalt
        WHERE Id = @UserId;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NewHash", newHash);
                    command.Parameters.AddWithValue("@NewSalt", newSalt);
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // Return true if the user was found and updated
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error updating password for user {userId}: {ex.Message}");
                throw new ApplicationException($"A database error occurred while updating the password for user ID {userId}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error updating password for user {userId}: {ex.Message}");
                throw new ApplicationException($"An unexpected error occurred while updating the password for user ID {userId}.", ex);
            }
        }

        /// <summary>
        /// Retrieves a user by their unique ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The User object if found; otherwise, null.</returns>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public User? GetUserById(int userId)
        {
            if (userId <= 0) return null; // Invalid ID

            const string sql = @"
        SELECT Id, Username, Email, PasswordHash, PasswordSalt, CreatedAt
        FROM Users
        WHERE Id = @UserId;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Check if a row was returned
                        {
                            // Use existing helper to map data
                            return MapReaderToUser(reader);
                        }
                        else
                        {
                            return null; // User not found
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error getting user by ID {userId}: {ex.Message}");
                throw new ApplicationException($"An error occurred while retrieving user data for ID {userId}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error getting user by ID {userId}: {ex.Message}");
                throw new ApplicationException($"An unexpected error occurred while retrieving user data for ID {userId}.", ex);
            }
        }

        /// <summary>
        /// Helper method to map a SqlDataReader row to a User object.
        /// </summary>
        /// <param name="reader">The SqlDataReader, positioned at the correct row.</param>
        /// <returns>A User object.</returns>
        private User MapReaderToUser(SqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                PasswordSalt = reader.GetString(reader.GetOrdinal("PasswordSalt")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            };
        }
    }
}