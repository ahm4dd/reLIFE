using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using reLIFE.BusinessLogic.Repositories;
using reLIFE.Core.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace reLIFE.BusinessLogic.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        // Potentially add EventRepository if needed for complex delete scenarios, but
        // ON DELETE SET NULL in DB handles basic case.

        // Constructor for dependency injection
        public CategoryService(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        /// <summary>
        /// Retrieves all categories for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of categories, ordered by name.</returns>
        /// <exception cref="ArgumentException">Thrown if userId is invalid.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public List<Category> GetCategoriesByUser(int userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));

            try
            {
                return _categoryRepository.GetCategoriesByUser(userId);
            }
            catch (ApplicationException appEx) // Catch repository errors
            {
                Console.WriteLine($"Service Error (GetCategoriesByUser): {appEx.Message}");
                // Log appEx
                throw;
            }
            catch (Exception ex) // Catch unexpected errors
            {
                Console.WriteLine($"Unexpected Service Error (GetCategoriesByUser): {ex.Message}");
                // Log ex
                throw new ApplicationException("An unexpected error occurred while retrieving categories.", ex);
            }
        }

        /// <summary>
        /// Retrieves a specific category by its ID for a given user.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The category if found and belongs to the user; otherwise, null.</returns>
        /// <exception cref="ArgumentException">Thrown if categoryId or userId is invalid.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public Category? GetCategoryById(int categoryId, int userId)
        {
            if (categoryId <= 0) throw new ArgumentException("Invalid Category ID.", nameof(categoryId));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));

            try
            {
                return _categoryRepository.GetCategoryById(categoryId, userId);
            }
            catch (ApplicationException appEx)
            {
                Console.WriteLine($"Service Error (GetCategoryById): {appEx.Message}");
                // Log appEx
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (GetCategoryById): {ex.Message}");
                // Log ex
                throw new ApplicationException("An unexpected error occurred while retrieving the category.", ex);
            }
        }

        /// <summary>
        /// Adds a new category after validation.
        /// </summary>
        /// <param name="categoryToAdd">The category data provided by the user.</param>
        /// <param name="userId">The ID of the user adding the category.</param>
        /// <returns>The newly created category with its assigned ID.</returns>
        /// <exception cref="ArgumentNullException">Thrown if categoryToAdd is null.</exception>
        /// <exception cref="ArgumentException">Thrown if validation fails (e.g., invalid name or color format).</exception>
        /// <exception cref="InvalidOperationException">Thrown if a category with the same name already exists for the user.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public Category AddCategory(Category categoryToAdd, int userId)
        {
            if (categoryToAdd == null) throw new ArgumentNullException(nameof(categoryToAdd));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));

            // --- Validation ---
            ValidateCategoryData(categoryToAdd, userId, isNewCategory: true);

            // Optionally check for duplicate name *before* hitting DB, although DB constraint is the final guarantee
            // var existing = _categoryRepository.GetCategoryByName(categoryToAdd.Name, userId); // Requires adding GetCategoryByName to Repo
            // if (existing != null) { throw new InvalidOperationException(...); }

            try
            {
                // Set the user ID
                categoryToAdd.UserId = userId;

                // Delegate to repository
                // Repository AddCategory already handles the OUTPUT clause
                // and should throw InvalidOperationException if name unique constraint violated
                return _categoryRepository.AddCategory(categoryToAdd);
            }
            catch (InvalidOperationException opEx) // Catch unique constraint violation from repo
            {
                Console.WriteLine($"Service Validation (AddCategory): {opEx.Message}");
                // Log opEx
                throw; // Re-throw specific exception
            }
            catch (ApplicationException appEx)
            {
                Console.WriteLine($"Service Error (AddCategory): {appEx.Message}");
                // Log appEx
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (AddCategory): {ex.Message}");
                // Log ex
                throw new ApplicationException("An unexpected error occurred while adding the category.", ex);
            }
        }

        /// <summary>
        /// Updates an existing category after validation.
        /// </summary>
        /// <param name="categoryToUpdate">The category data with modifications.</param>
        /// <param name="userId">The ID of the user performing the update.</param>
        /// <returns>True if update was successful, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if categoryToUpdate is null.</exception>
        /// <exception cref="ArgumentException">Thrown if validation fails (e.g., invalid ID, name, or color format).</exception>
        /// <exception cref="InvalidOperationException">Thrown if a category with the new name already exists for the user.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public bool UpdateCategory(Category categoryToUpdate, int userId)
        {
            if (categoryToUpdate == null) throw new ArgumentNullException(nameof(categoryToUpdate));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));
            if (categoryToUpdate.Id <= 0) throw new ArgumentException("Category ID must be provided for update.", nameof(categoryToUpdate.Id));

            // --- Validation ---
            ValidateCategoryData(categoryToUpdate, userId, isNewCategory: false);

            try
            {
                // Delegate to repository (which handles the WHERE Id=@Id AND UserId=@UserId check)
                // Repository UpdateCategory handles name unique constraint check and returns bool
                return _categoryRepository.UpdateCategory(categoryToUpdate, userId);
            }
            catch (InvalidOperationException opEx) // Catch unique constraint violation from repo
            {
                Console.WriteLine($"Service Validation (UpdateCategory): {opEx.Message}");
                // Log opEx
                throw; // Re-throw specific exception
            }
            catch (ApplicationException appEx)
            {
                Console.WriteLine($"Service Error (UpdateCategory): {appEx.Message}");
                // Log appEx
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (UpdateCategory): {ex.Message}");
                // Log ex
                throw new ApplicationException("An unexpected error occurred while updating the category.", ex);
            }
        }

        /// <summary>
        /// Deletes a category. Events using this category will have their CategoryId set to NULL by the database.
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete.</param>
        /// <param name="userId">The ID of the user performing the deletion.</param>
        /// <returns>True if deletion was successful, false otherwise (e.g., category not found for user).</returns>
        /// <exception cref="ArgumentException">Thrown if categoryId or userId is invalid.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public bool DeleteCategory(int categoryId, int userId)
        {
            if (categoryId <= 0) throw new ArgumentException("Invalid Category ID.", nameof(categoryId));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));

            try
            {
                // Delegate to repository (which handles the WHERE Id=@Id AND UserId=@UserId check)
                return _categoryRepository.DeleteCategory(categoryId, userId);
            }
            catch (ApplicationException appEx)
            {
                Console.WriteLine($"Service Error (DeleteCategory): {appEx.Message}");
                // Log appEx
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (DeleteCategory): {ex.Message}");
                // Log ex
                throw new ApplicationException("An unexpected error occurred while deleting the category.", ex);
            }
        }

        // --- Private Helper Methods ---

        /// <summary>
        /// Performs common validation checks for category data.
        /// </summary>
        /// <param name="category">The category object to validate.</param>
        /// <param name="userId">The user ID for context (currently unused here but good practice).</param>
        /// <param name="isNewCategory">Flag indicating if this is for Add (true) or Update (false).</param>
        private void ValidateCategoryData(Category category, int userId, bool isNewCategory)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("Category name cannot be empty.", nameof(category.Name));
            }
            if (category.Name.Length > 100) // Assuming max length from table
            {
                throw new ArgumentException("Category name cannot exceed 100 characters.", nameof(category.Name));
            }

            if (string.IsNullOrWhiteSpace(category.ColorHex))
            {
                throw new ArgumentException("Category color cannot be empty.", nameof(category.ColorHex));
            }

            // Basic Hex Color validation (allows #RRGGBB or #RGB)
            if (!Regex.IsMatch(category.ColorHex, @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$"))
            {
                throw new ArgumentException("Invalid color format. Use #RRGGBB (e.g., #FF0000).", nameof(category.ColorHex));
            }
            // Ensure consistent format (optional, store as 6 hex digits?)
            // category.ColorHex = StandardizeColorHex(category.ColorHex);


            if (!isNewCategory)
            {
                if (category.Id <= 0) throw new ArgumentException("Invalid Category ID for update.", nameof(category.Id));
            }
        }

        // Example of standardization (optional)
        // private string StandardizeColorHex(string input)
        // {
        //     input = input.Trim().ToUpper();
        //     if (input.Length == 4 && input.StartsWith("#")) // #RGB -> #RRGGBB
        //     {
        //         char r = input[1];
        //         char g = input[2];
        //         char b = input[3];
        //         return $"#{r}{r}{g}{g}{b}{b}";
        //     }
        //     return input; // Assuming #RRGGBB or already invalid
        // }
    }
}