using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using reLIFE.Core.Models;

namespace reLIFE.BusinessLogic.Repositories
{
    public class ArchivedEventRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the ArchivedEventRepository.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
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
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public bool AddArchivedEvent(ArchivedEvent archiveToAdd)
        {
            // Note: ArchivedAt is typically set by the service layer before calling this.
            // We use SET IDENTITY_INSERT ON because we are specifying the Id value (original event Id)
            // instead of letting the (non-existent) IDENTITY column generate one.
            // This requires specific DB permissions. Alternatively, make ArchivedEvents.Id an IDENTITY
            // column and store OriginalEventId separately. The current approach is simpler if permissions allow.

            // Use separate commands for IDENTITY_INSERT control and the actual INSERT
            const string enableIdentityInsertSql = "SET IDENTITY_INSERT dbo.ArchivedEvents ON;";
            const string insertSql = @"
                INSERT INTO ArchivedEvents (Id, UserId, CategoryId, Title, Description, StartTime, EndTime, IsAllDay, CreatedAt, LastModifiedAt, ArchivedAt)
                VALUES (@Id, @UserId, @CategoryId, @Title, @Description, @StartTime, @EndTime, @IsAllDay, @CreatedAt, @LastModifiedAt, @ArchivedAt);";
            const string disableIdentityInsertSql = "SET IDENTITY_INSERT dbo.ArchivedEvents OFF;";

            SqlConnection? connection = null; // Declare outside using for broader scope if needed
            SqlTransaction? transaction = null; // Use a transaction for safety

            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();
                transaction = connection.BeginTransaction(); // Start transaction

                // 1. Enable Identity Insert
                using (var cmdEnable = new SqlCommand(enableIdentityInsertSql, connection, transaction))
                {
                    cmdEnable.ExecuteNonQuery();
                }

                // 2. Perform the Insert
                using (var cmdInsert = new SqlCommand(insertSql, connection, transaction))
                {
                    // Add parameters
                    cmdInsert.Parameters.AddWithValue("@Id", archiveToAdd.Id); // Provide the original ID
                    cmdInsert.Parameters.AddWithValue("@UserId", archiveToAdd.UserId);
                    cmdInsert.Parameters.AddWithValue("@CategoryId", (object?)archiveToAdd.CategoryId ?? DBNull.Value);
                    cmdInsert.Parameters.AddWithValue("@Title", archiveToAdd.Title);
                    cmdInsert.Parameters.AddWithValue("@Description", (object?)archiveToAdd.Description ?? DBNull.Value);
                    cmdInsert.Parameters.AddWithValue("@StartTime", archiveToAdd.StartTime);
                    cmdInsert.Parameters.AddWithValue("@EndTime", archiveToAdd.EndTime);
                    cmdInsert.Parameters.AddWithValue("@IsAllDay", archiveToAdd.IsAllDay);
                    cmdInsert.Parameters.AddWithValue("@CreatedAt", archiveToAdd.CreatedAt);
                    cmdInsert.Parameters.AddWithValue("@LastModifiedAt", (object?)archiveToAdd.LastModifiedAt ?? DBNull.Value);
                    cmdInsert.Parameters.AddWithValue("@ArchivedAt", archiveToAdd.ArchivedAt); // Should be set before calling

                    cmdInsert.ExecuteNonQuery(); // Execute the main insert
                }

                // 3. Disable Identity Insert
                using (var cmdDisable = new SqlCommand(disableIdentityInsertSql, connection, transaction))
                {
                    cmdDisable.ExecuteNonQuery();
                }

                transaction.Commit(); // Commit transaction if all steps succeed
                return true; // Indicate success

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error adding archived event: {ex.Message}");
                try { transaction?.Rollback(); } catch { /* Ignore rollback error */ } // Attempt to rollback on error
                                                                                       // Check for specific errors like PK violation if event was already archived? Error 2627/2601
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new InvalidOperationException($"Event with ID {archiveToAdd.Id} might already be archived.", ex);
                }
                throw new ApplicationException($"A database error occurred while archiving the event (ID: {archiveToAdd.Id}).", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error adding archived event: {ex.Message}");
                try { transaction?.Rollback(); } catch { /* Ignore rollback error */ }
                throw new ApplicationException($"An unexpected error occurred while archiving the event (ID: {archiveToAdd.Id}).", ex);
            }
            finally // Ensure connection is closed even if transaction handling has issues
            {
                connection?.Close(); // Close connection in finally block
            }
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
            // Add TOP clause if limit is specified
            string sql = $@"
                SELECT {(limit.HasValue ? $"TOP {limit.Value}" : "")}
                       Id, UserId, CategoryId, Title, Description, StartTime, EndTime, IsAllDay, CreatedAt, LastModifiedAt, ArchivedAt
                FROM ArchivedEvents
                WHERE UserId = @UserId
                ORDER BY ArchivedAt DESC;"; // Order by most recently archived

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
            catch (SqlException ex) { /* ... standard error handling ... */ throw new ApplicationException("Error retrieving archived events.", ex); }
            catch (Exception ex) { /* ... standard error handling ... */ throw new ApplicationException("Unexpected error retrieving archived events.", ex); }

            return archivedEvents;
        }

        // --- Add other methods as needed ---
        // - GetArchivedEventById(int archiveId, int userId)
        // - DeleteArchivedEvent(int archiveId) (Potentially dangerous, use with care)
        // - DeleteArchivedEventsBefore(DateTime cutoffDate) (For purging)


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
    }
}