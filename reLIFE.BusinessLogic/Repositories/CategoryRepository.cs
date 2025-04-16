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
    public class CategoryRepository
    {
        private readonly string _connectionString;

        // Constructor injection for the connection string
        public CategoryRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Database connection string cannot be null or empty.");
            }
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a new category for a specific user.
        /// </summary>
        /// <param name="categoryToAdd">The category object containing UserId, Name, and ColorHex.</param>
        /// <returns>The Category object updated with the generated ID.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a category with the same name already exists for the user (based on DB constraint).</exception>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public Category AddCategory(Category categoryToAdd)
        {
            // Ensure UserId is set
            if (categoryToAdd.UserId <= 0)
            {
                throw new InvalidOperationException("UserId must be set on the Category object before adding.");
            }

            // Note: CreatedAt is handled by DB default
            const string sql = @"
                INSERT INTO Categories (UserId, Name, ColorHex)
                OUTPUT INSERTED.Id
                VALUES (@UserId, @Name, @ColorHex);";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", categoryToAdd.UserId);
                    command.Parameters.AddWithValue("@Name", categoryToAdd.Name);
                    command.Parameters.AddWithValue("@ColorHex", categoryToAdd.ColorHex);

                    connection.Open();

                    // ExecuteScalar is suitable here as we only expect the single ID back from OUTPUT
                    var insertedId = command.ExecuteScalar();

                    if (insertedId != null && insertedId != DBNull.Value)
                    {
                        categoryToAdd.Id = Convert.ToInt32(insertedId);
                        return categoryToAdd;
                    }
                    else
                    {
                        throw new InvalidOperationException("Failed to retrieve inserted category ID after insert.");
                    }
                }
            }
            catch (SqlException ex)
            {
                // Check for specific SQL Server error number for UNIQUE constraint violation
                // Assuming a UNIQUE constraint exists on (UserId, Name)
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new InvalidOperationException($"A category named '{categoryToAdd.Name}' already exists for this user.", ex);
                }
                Console.WriteLine($"SQL Error adding category: {ex.Message}");
                throw new ApplicationException("An error occurred while adding the category.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error adding category: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while adding the category.", ex);
            }
        }

        /// <summary>
        /// Retrieves all categories belonging to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose categories to retrieve.</param>
        /// <returns>A list of Category objects.</returns>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public List<Category> GetCategoriesByUser(int userId)
        {
            var categories = new List<Category>();
            const string sql = @"
                SELECT Id, UserId, Name, ColorHex
                FROM Categories
                WHERE UserId = @UserId
                ORDER BY Name;"; // Order alphabetically for display

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) // Loop through all categories for the user
                        {
                            categories.Add(MapReaderToCategory(reader));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error getting categories by user: {ex.Message}");
                throw new ApplicationException("An error occurred while retrieving categories.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error getting categories by user: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while retrieving categories.", ex);
            }
            return categories;
        }

        /// <summary>
        /// Retrieves a specific category by its ID, ensuring it belongs to the specified user.
        /// </summary>
        /// <param name="categoryId">The ID of the category to retrieve.</param>
        /// <param name="userId">The ID of the user who should own the category.</param>
        /// <returns>The Category object if found and belongs to the user; otherwise, null.</returns>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public Category? GetCategoryById(int categoryId, int userId)
        {
            const string sql = @"
                SELECT Id, UserId, Name, ColorHex
                FROM Categories
                WHERE Id = @CategoryId AND UserId = @UserId;";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CategoryId", categoryId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Check if a category was found
                        {
                            return MapReaderToCategory(reader);
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
                Console.WriteLine($"SQL Error getting category by ID: {ex.Message}");
                throw new ApplicationException("An error occurred while retrieving the category.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error getting category by ID: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while retrieving the category.", ex);
            }
        }


        /// <summary>
        /// Updates an existing category's name and color.
        /// Verifies that the category belongs to the specified user before updating.
        /// </summary>
        /// <param name="categoryToUpdate">The category object with updated details.</param>
        /// <param name="userId">The ID of the user attempting the update.</param>
        /// <returns>True if the category was successfully updated; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a category with the new name already exists for the user (based on DB constraint).</exception>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public bool UpdateCategory(Category categoryToUpdate, int userId)
        {
            const string sql = @"
                UPDATE Categories
                SET Name = @Name,
                    ColorHex = @ColorHex
                WHERE Id = @CategoryId AND UserId = @UserId;"; // IMPORTANT: Ensure UserId match!

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", categoryToUpdate.Name);
                    command.Parameters.AddWithValue("@ColorHex", categoryToUpdate.ColorHex);
                    command.Parameters.AddWithValue("@CategoryId", categoryToUpdate.Id);
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // Return true if one row was updated
                }
            }
            catch (SqlException ex)
            {
                // Check for specific SQL Server error number for UNIQUE constraint violation
                // Assuming a UNIQUE constraint exists on (UserId, Name)
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new InvalidOperationException($"A category named '{categoryToUpdate.Name}' already exists for this user.", ex);
                }
                Console.WriteLine($"SQL Error updating category: {ex.Message}");
                throw new ApplicationException("An error occurred while updating the category.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error updating category: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while updating the category.", ex);
            }
        }

        /// <summary>
        /// Deletes a category from the database.
        /// Verifies that the category belongs to the specified user before deleting.
        /// Note: Associated events will have their CategoryId set to NULL due to FK constraint.
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete.</param>
        /// <param name="userId">The ID of the user attempting the deletion.</param>
        /// <returns>True if the category was successfully deleted; otherwise, false.</returns>
        /// <exception cref="ApplicationException">Thrown for general database errors.</exception>
        public bool DeleteCategory(int categoryId, int userId)
        {
            const string sql = @"
                DELETE FROM Categories
                WHERE Id = @CategoryId AND UserId = @UserId;"; // IMPORTANT: Ensure UserId match!

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CategoryId", categoryId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // Return true if one row was deleted
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error deleting category: {ex.Message}");
                // Consider specific checks, e.g., if FK constraints prevented delete (though ours is ON DELETE SET NULL)
                throw new ApplicationException("An error occurred while deleting the category.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error deleting category: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while deleting the category.", ex);
            }
        }

        /// <summary>
        /// Helper method to map a SqlDataReader row to a Category object.
        /// </summary>
        /// <param name="reader">The SqlDataReader, positioned at the correct row.</param>
        /// <returns>A Category object.</returns>
        private Category MapReaderToCategory(SqlDataReader reader)
        {
            return new Category
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")), // Included for completeness, though often known by caller
                Name = reader.GetString(reader.GetOrdinal("Name")),
                ColorHex = reader.GetString(reader.GetOrdinal("ColorHex"))
            };
        }
    }
}