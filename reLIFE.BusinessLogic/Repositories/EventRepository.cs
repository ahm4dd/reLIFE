using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using reLIFE.Core.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace reLIFE.BusinessLogic.Repositories
{
    public class EventRepository
    {
        private readonly string _connectionString;

        // Constructor to inject the connection string
        public EventRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Database connection string cannot be null or empty.");
            }
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a new event to the database for a specific user.
        /// </summary>
        /// <param name="eventToAdd">The event object containing details (UserId should be set by the caller or service).</param>
        /// <returns>The Event object updated with the generated ID and CreatedAt timestamp.</returns>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public Event AddEvent(Event eventToAdd)
        {
            // Ensure UserId is set before attempting to insert
            if (eventToAdd.UserId <= 0)
            {
                throw new InvalidOperationException("UserId must be set on the Event object before adding.");
            }

            const string sql = @"
                INSERT INTO Events (UserId, CategoryId, Title, Description, StartTime, EndTime, IsAllDay)
                OUTPUT INSERTED.Id, INSERTED.CreatedAt
                VALUES (@UserId, @CategoryId, @Title, @Description, @StartTime, @EndTime, @IsAllDay);";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@UserId", eventToAdd.UserId);
                    // Handle nullable CategoryId
                    command.Parameters.AddWithValue("@CategoryId", (object?)eventToAdd.CategoryId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Title", eventToAdd.Title);
                    // Handle nullable Description
                    command.Parameters.AddWithValue("@Description", (object?)eventToAdd.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StartTime", eventToAdd.StartTime);
                    command.Parameters.AddWithValue("@EndTime", eventToAdd.EndTime);
                    command.Parameters.AddWithValue("@IsAllDay", eventToAdd.IsAllDay);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Read the OUTPUT row
                        {
                            eventToAdd.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            eventToAdd.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                            return eventToAdd;
                        }
                        else
                        {
                            throw new InvalidOperationException("Failed to retrieve inserted event data after insert.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error adding event: {ex.Message}");
                throw new ApplicationException("An error occurred while adding the event.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error adding event: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while adding the event.", ex);
            }
        }

        /// <summary>
        /// Retrieves events for a specific user that overlap with a given date range.
        /// Note: An event overlaps the range if (Event.StartTime < rangeEnd AND Event.EndTime > rangeStart).
        /// </summary>
        /// <param name="userId">The ID of the user whose events to retrieve.</param>
        /// <param name="rangeStart">The start of the date range (exclusive for end times).</param>
        /// <param name="rangeEnd">The end of the date range (exclusive for start times).</param>
        /// <returns>A list of events within the specified range.</returns>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public List<Event> GetEventsByDateRange(int userId, DateTime rangeStart, DateTime rangeEnd)
        {
            var events = new List<Event>();
            // Ensure EndTime > StartTime to avoid infinite loops or weirdness in logic
            const string sql = @"
                SELECT Id, UserId, CategoryId, Title, Description, StartTime, EndTime, IsAllDay, CreatedAt, LastModifiedAt
                FROM Events
                WHERE UserId = @UserId
                  AND StartTime < @RangeEnd
                  AND EndTime > @RangeStart
                ORDER BY StartTime;"; // Order results for better display

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@RangeStart", rangeStart);
                    command.Parameters.AddWithValue("@RangeEnd", rangeEnd);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) // Loop through all matching events
                        {
                            events.Add(MapReaderToEvent(reader));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error getting events by date range: {ex.Message}");
                throw new ApplicationException("An error occurred while retrieving events.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error getting events by date range: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while retrieving events.", ex);
            }

            return events;
        }

        /// <summary>
        /// Retrieves a specific event by its ID, ensuring it belongs to the specified user.
        /// </summary>
        /// <param name="eventId">The ID of the event to retrieve.</param>
        /// <param name="userId">The ID of the user who should own the event.</param>
        /// <returns>The Event object if found and belongs to the user; otherwise, null.</returns>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public Event? GetEventById(int eventId, int userId)
        {
            const string sql = @"
                SELECT Id, UserId, CategoryId, Title, Description, StartTime, EndTime, IsAllDay, CreatedAt, LastModifiedAt
                FROM Events
                WHERE Id = @EventId AND UserId = @UserId;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EventId", eventId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Check if an event was found
                        {
                            return MapReaderToEvent(reader);
                        }
                        else
                        {
                            return null; // Not found or doesn't belong to user
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error getting event by ID: {ex.Message}");
                throw new ApplicationException("An error occurred while retrieving the event.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error getting event by ID: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while retrieving the event.", ex);
            }
        }


        /// <summary>
        /// Updates an existing event in the database.
        /// Verifies that the event belongs to the specified user before updating.
        /// </summary>
        /// <param name="eventToUpdate">The event object with updated details.</param>
        /// <param name="userId">The ID of the user attempting the update.</param>
        /// <returns>True if the event was successfully updated; otherwise, false (likely not found or user mismatch).</returns>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public bool UpdateEvent(Event eventToUpdate, int userId)
        {
            const string sql = @"
                UPDATE Events
                SET CategoryId = @CategoryId,
                    Title = @Title,
                    Description = @Description,
                    StartTime = @StartTime,
                    EndTime = @EndTime,
                    IsAllDay = @IsAllDay,
                    LastModifiedAt = GETDATE()
                WHERE Id = @EventId AND UserId = @UserId;"; // IMPORTANT: Ensure UserId match!

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@EventId", eventToUpdate.Id);
                    command.Parameters.AddWithValue("@UserId", userId); // Use the parameter for the WHERE clause
                    command.Parameters.AddWithValue("@CategoryId", (object?)eventToUpdate.CategoryId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Title", eventToUpdate.Title);
                    command.Parameters.AddWithValue("@Description", (object?)eventToUpdate.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StartTime", eventToUpdate.StartTime);
                    command.Parameters.AddWithValue("@EndTime", eventToUpdate.EndTime);
                    command.Parameters.AddWithValue("@IsAllDay", eventToUpdate.IsAllDay);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // Return true if one row was updated
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error updating event: {ex.Message}");
                throw new ApplicationException("An error occurred while updating the event.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error updating event: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while updating the event.", ex);
            }
        }

        /// <summary>
        /// Deletes an event from the database.
        /// Verifies that the event belongs to the specified user before deleting.
        /// </summary>
        /// <param name="eventId">The ID of the event to delete.</param>
        /// <param name="userId">The ID of the user attempting the deletion.</param>
        /// <returns>True if the event was successfully deleted; otherwise, false (likely not found or user mismatch).</returns>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public bool DeleteEvent(int eventId, int userId)
        {
            const string sql = @"
                DELETE FROM Events
                WHERE Id = @EventId AND UserId = @UserId;"; // IMPORTANT: Ensure UserId match!

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EventId", eventId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // Return true if one row was deleted
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error deleting event: {ex.Message}");
                throw new ApplicationException("An error occurred while deleting the event.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error deleting event: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while deleting the event.", ex);
            }
        }

        /// <summary>
        /// Helper method to map a SqlDataReader row to an Event object.
        /// </summary>
        /// <param name="reader">The SqlDataReader, positioned at the correct row.</param>
        /// <returns>An Event object.</returns>
        private Event MapReaderToEvent(SqlDataReader reader)
        {
            return new Event
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                // Handle nullable CategoryId
                CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                // Handle nullable Description
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                StartTime = reader.GetDateTime(reader.GetOrdinal("StartTime")),
                EndTime = reader.GetDateTime(reader.GetOrdinal("EndTime")),
                IsAllDay = reader.GetBoolean(reader.GetOrdinal("IsAllDay")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                // Handle nullable LastModifiedAt
                LastModifiedAt = reader.IsDBNull(reader.GetOrdinal("LastModifiedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LastModifiedAt"))
            };
        }
    }
}