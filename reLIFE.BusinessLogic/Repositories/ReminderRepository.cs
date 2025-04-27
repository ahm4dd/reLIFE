using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using reLIFE.Core.Models;

namespace reLIFE.BusinessLogic.Repositories
{
    public class ReminderRepository
    {
        private readonly string _connectionString;

        public ReminderRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Database connection string cannot be null or empty.");
            }
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a new reminder to the database.
        /// </summary>
        /// <param name="reminderToAdd">The reminder object to add.</param>
        /// <returns>The added Reminder object with its generated ID and CreatedAt timestamp.</returns>
        /// <exception cref="ApplicationException">Thrown for database errors.</exception>
        public Reminder AddReminder(Reminder reminderToAdd)
        {
            const string sql = @"
                INSERT INTO Reminders (EventId, MinutesBefore, IsEnabled)
                OUTPUT INSERTED.Id, INSERTED.CreatedAt
                VALUES (@EventId, @MinutesBefore, @IsEnabled);";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EventId", reminderToAdd.EventId);
                    command.Parameters.AddWithValue("@MinutesBefore", reminderToAdd.MinutesBefore);
                    command.Parameters.AddWithValue("@IsEnabled", reminderToAdd.IsEnabled);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            reminderToAdd.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            reminderToAdd.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                            return reminderToAdd;
                        }
                        else
                        {
                            throw new InvalidOperationException("Failed to retrieve inserted reminder data.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Check for FK violation? (EventId doesn't exist) Error 547
                if (ex.Number == 547)
                {
                    throw new InvalidOperationException($"Cannot add reminder: Event with ID {reminderToAdd.EventId} does not exist.", ex);
                }
                Console.WriteLine($"SQL Error adding reminder: {ex.Message}");
                throw new ApplicationException("A database error occurred while adding the reminder.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error adding reminder: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while adding the reminder.", ex);
            }
        }

        /// <summary>
        /// Updates an existing reminder.
        /// </summary>
        /// <param name="reminderToUpdate">The reminder object with updated values.</param>
        /// <returns>True if the reminder was updated, false otherwise.</returns>
        /// <exception cref="ApplicationException">Thrown for database errors.</exception>
        public bool UpdateReminder(Reminder reminderToUpdate)
        {
            const string sql = @"
                UPDATE Reminders
                SET MinutesBefore = @MinutesBefore,
                    IsEnabled = @IsEnabled
                WHERE Id = @Id;"; // Assuming EventId doesn't change for a given reminder ID

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MinutesBefore", reminderToUpdate.MinutesBefore);
                    command.Parameters.AddWithValue("@IsEnabled", reminderToUpdate.IsEnabled);
                    command.Parameters.AddWithValue("@Id", reminderToUpdate.Id);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex) { /* ... standard error handling ... */ throw new ApplicationException("Error updating reminder.", ex); }
            catch (Exception ex) { /* ... standard error handling ... */ throw new ApplicationException("Unexpected error updating reminder.", ex); }
        }

        /// <summary>
        /// Deletes a specific reminder by its ID.
        /// </summary>
        /// <param name="reminderId">The ID of the reminder to delete.</param>
        /// <returns>True if the reminder was deleted, false otherwise.</returns>
        /// <exception cref="ApplicationException">Thrown for database errors.</exception>
        public bool DeleteReminder(int reminderId)
        {
            const string sql = "DELETE FROM Reminders WHERE Id = @Id;";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", reminderId);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex) { /* ... standard error handling ... */ throw new ApplicationException("Error deleting reminder.", ex); }
            catch (Exception ex) { /* ... standard error handling ... */ throw new ApplicationException("Unexpected error deleting reminder.", ex); }
        }

        /// <summary>
        /// Deletes all reminders associated with a specific event.
        /// Used when an event is deleted or archived.
        /// </summary>
        /// <param name="eventId">The ID of the event whose reminders should be deleted.</param>
        /// <returns>True if deletion completed (even if no reminders existed), false on error.</returns>
        /// <exception cref="ApplicationException">Thrown for database errors.</exception>
        public bool DeleteRemindersForEvent(int eventId)
        {
            const string sql = "DELETE FROM Reminders WHERE EventId = @EventId;";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EventId", eventId);
                    connection.Open();
                    command.ExecuteNonQuery(); // Execute even if 0 rows affected
                    return true; // Return true indicating the operation completed
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error deleting reminders for event {eventId}: {ex.Message}");
                throw new ApplicationException($"Error deleting reminders for event ID {eventId}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error deleting reminders for event {eventId}: {ex.Message}");
                throw new ApplicationException($"Unexpected error deleting reminders for event ID {eventId}.", ex);
            }
        }

        /// <summary>
        /// Retrieves all reminders for a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <returns>A list of Reminder objects, possibly empty.</returns>
        /// <exception cref="ApplicationException">Thrown for database errors.</exception>
        public List<Reminder> GetRemindersForEvent(int eventId)
        {
            var reminders = new List<Reminder>();
            const string sql = @"
                SELECT Id, EventId, MinutesBefore, IsEnabled, CreatedAt
                FROM Reminders
                WHERE EventId = @EventId
                ORDER BY MinutesBefore ASC;"; // Order logically

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EventId", eventId);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reminders.Add(MapReaderToReminder(reader));
                        }
                    }
                }
            }
            catch (SqlException ex) { /* ... standard error handling ... */ throw new ApplicationException("Error retrieving reminders.", ex); }
            catch (Exception ex) { /* ... standard error handling ... */ throw new ApplicationException("Unexpected error retrieving reminders.", ex); }

            return reminders;
        }

        // --- Optional: GetReminderById ---
        public Reminder? GetReminderById(int reminderId)
        {
            const string sql = @"
                SELECT Id, EventId, MinutesBefore, IsEnabled, CreatedAt
                FROM Reminders
                WHERE Id = @Id";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", reminderId);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToReminder(reader);
                        }
                        return null; // Not found
                    }
                }
            }
            catch (SqlException ex) { /* ... standard error handling ... */ throw new ApplicationException("Error retrieving reminder by ID.", ex); }
            catch (Exception ex) { /* ... standard error handling ... */ throw new ApplicationException("Unexpected error retrieving reminder by ID.", ex); }
        }


        /// <summary>
        /// Helper method to map a SqlDataReader row to a Reminder object.
        /// </summary>
        private Reminder MapReaderToReminder(SqlDataReader reader)
        {
            return new Reminder
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                EventId = reader.GetInt32(reader.GetOrdinal("EventId")),
                MinutesBefore = reader.GetInt32(reader.GetOrdinal("MinutesBefore")),
                IsEnabled = reader.GetBoolean(reader.GetOrdinal("IsEnabled")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            };
        }
    }
}