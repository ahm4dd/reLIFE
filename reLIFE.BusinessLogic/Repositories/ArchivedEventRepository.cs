using Microsoft.Data.SqlClient;
using reLIFE.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace reLIFE.BusinessLogic.Repositories
{
    /// <summary>
    /// Repository for managing archived event data in the ArchivedEvents table.
    /// </summary>
    public class ArchivedEventRepository
    {
        private readonly string _connectionString;

        public ArchivedEventRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Database connection string cannot be null or empty.");
            }
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a previously active event to the archive table.
        /// Assumes the primary key 'Id' is the original event's ID.
        /// </summary>
        /// <param name="archiveToAdd">The ArchivedEvent object containing data copied from the original event.</param>
        /// <returns>True if the event was successfully added to the archive; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the event ID already exists in the archive.</exception>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public bool AddArchivedEvent(ArchivedEvent archiveToAdd)
        {
            // *** REMOVED SET IDENTITY_INSERT logic ***
            const string insertSql = @"
                INSERT INTO ArchivedEvents (Id, UserId, CategoryId, Title, Description, StartTime, EndTime, IsAllDay, CreatedAt, LastModifiedAt, ArchivedAt)
                VALUES (@Id, @UserId, @CategoryId, @Title, @Description, @StartTime, @EndTime, @IsAllDay, @CreatedAt, @LastModifiedAt, @ArchivedAt);";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(insertSql, connection)) // Just use the INSERT command
                {
                    // Add parameters (Same as before)
                    command.Parameters.AddWithValue("@Id", archiveToAdd.Id); // Provide the original ID
                    command.Parameters.AddWithValue("@UserId", archiveToAdd.UserId);
                    command.Parameters.AddWithValue("@CategoryId", (object?)archiveToAdd.CategoryId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Title", archiveToAdd.Title);
                    command.Parameters.AddWithValue("@Description", (object?)archiveToAdd.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StartTime", archiveToAdd.StartTime);
                    command.Parameters.AddWithValue("@EndTime", archiveToAdd.EndTime);
                    command.Parameters.AddWithValue("@IsAllDay", archiveToAdd.IsAllDay);
                    command.Parameters.AddWithValue("@CreatedAt", archiveToAdd.CreatedAt);
                    command.Parameters.AddWithValue("@LastModifiedAt", (object?)archiveToAdd.LastModifiedAt ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ArchivedAt", archiveToAdd.ArchivedAt);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery(); // Execute the insert
                    return rowsAffected > 0; // Return true if one row was inserted
                }
                // *** No transaction needed just for a single INSERT ***
                // *** No SET IDENTITY_INSERT OFF needed ***
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error adding archived event: {ex.Message}");
                // Check for specific errors like PK violation (event already archived)
                if (ex.Number == 2627 || ex.Number == 2601) // Primary Key violation codes
                {
                    throw new InvalidOperationException($"Event with ID {archiveToAdd.Id} has already been archived.", ex);
                }
                throw new ApplicationException($"A database error occurred while archiving the event (ID: {archiveToAdd.Id}).", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error adding archived event: {ex.Message}");
                throw new ApplicationException($"An unexpected error occurred while archiving the event (ID: {archiveToAdd.Id}).", ex);
            }
            // No finally needed just for closing connection with 'using'
        }

        /// <summary>
        /// Retrieves all archived events for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose archived events to retrieve.</param>
        /// <param name="limit">Optional limit on the number of records to return.</param>
        /// <returns>A list of ArchivedEvent objects.</returns>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public List<ArchivedEvent> GetArchivedEventsByUser(int userId, int? limit = null)
        {
            var archivedEvents = new List<ArchivedEvent>();
            string sql = $@"
                SELECT {(limit.HasValue ? $"TOP {limit.Value}" : "")}
                       Id, UserId, CategoryId, Title, Description, StartTime, EndTime, IsAllDay, CreatedAt, LastModifiedAt, ArchivedAt
                FROM ArchivedEvents
                WHERE UserId = @UserId
                ORDER BY ArchivedAt DESC;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            archivedEvents.Add(MapReaderToArchivedEvent(reader));
                        }
                    }
                }
            }
            catch (SqlException ex) { Console.WriteLine($"SQL Error retrieving archived events: {ex.Message}"); throw new ApplicationException("Error retrieving archived events.", ex); }
            catch (Exception ex) { Console.WriteLine($"General Error retrieving archived events: {ex.Message}"); throw new ApplicationException("Unexpected error retrieving archived events.", ex); }

            return archivedEvents;
        }

        // --- Add other methods as needed (GetById, Delete, Purge, Restore etc.) ---


        /// <summary>
        /// Helper method to map a SqlDataReader row to an ArchivedEvent object.
        /// </summary>
        private ArchivedEvent MapReaderToArchivedEvent(SqlDataReader reader)
        {
            return new ArchivedEvent
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                StartTime = reader.GetDateTime(reader.GetOrdinal("StartTime")),
                EndTime = reader.GetDateTime(reader.GetOrdinal("EndTime")),
                IsAllDay = reader.GetBoolean(reader.GetOrdinal("IsAllDay")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                LastModifiedAt = reader.IsDBNull(reader.GetOrdinal("LastModifiedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LastModifiedAt")),
                ArchivedAt = reader.GetDateTime(reader.GetOrdinal("ArchivedAt"))
            };
        }

        public ArchivedEvent? GetArchivedEventById(int archivedEventId, int userId)
        {
            if (archivedEventId <= 0 || userId <= 0) return null;

            const string sql = @"
                SELECT Id, UserId, CategoryId, Title, Description, StartTime, EndTime, IsAllDay, CreatedAt, LastModifiedAt, ArchivedAt
                FROM ArchivedEvents
                WHERE Id = @ArchivedEventId AND UserId = @UserId;";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ArchivedEventId", archivedEventId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToArchivedEvent(reader);
                        }
                        return null; // Not found or doesn't belong to user
                    }
                }
            }
            catch (SqlException ex) { /* ... error handling ... */ throw new ApplicationException("Error retrieving archived event by ID.", ex); }
            catch (Exception ex) { /* ... error handling ... */ throw new ApplicationException("Unexpected error retrieving archived event by ID.", ex); }
        }


        // *** NEW METHOD: DeleteArchivedEvent ***
        /// <summary>
        /// Permanently deletes an archived event by its ID.
        /// Consider adding userId parameter for ownership check if needed by service layer strategy.
        /// </summary>
        public bool DeleteArchivedEvent(int archivedEventId)
        {
            if (archivedEventId <= 0) return false;

            const string sql = "DELETE FROM ArchivedEvents WHERE Id = @ArchivedEventId;";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ArchivedEventId", archivedEventId);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex) { /* ... error handling ... */ throw new ApplicationException($"Error deleting archived event ID {archivedEventId}.", ex); }
            catch (Exception ex) { /* ... error handling ... */ throw new ApplicationException($"Unexpected error deleting archived event ID {archivedEventId}.", ex); }
        }
    }
}