using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using reLIFE.BusinessLogic.Repositories;
using reLIFE.Core.Models;
using System.Collections.Generic;

namespace reLIFE.BusinessLogic.Services
{
    public class EventService
    {
        private readonly EventRepository _eventRepository;
        private readonly CategoryRepository _categoryRepository; // Optional, for validation

        // Constructor for dependency injection
        public EventService(EventRepository eventRepository, CategoryRepository categoryRepository)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        /// <summary>
        /// Retrieves events for a specific user within a given date range.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="rangeStart">The start of the date range.</param>
        /// <param name="rangeEnd">The end of the date range.</param>
        /// <returns>A list of events.</returns>
        /// <exception cref="ArgumentException">Thrown if rangeEnd is before rangeStart.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public List<Event> GetEventsForDateRange(int userId, DateTime rangeStart, DateTime rangeEnd)
        {
            if (userId <= 0)
            {
                // In a real app with authentication context, this check might be implicit
                throw new ArgumentException("Invalid User ID.", nameof(userId));
            }
            if (rangeEnd < rangeStart)
            {
                throw new ArgumentException("End date cannot be before start date.", nameof(rangeEnd));
            }

            try
            {
                // Delegate directly to the repository
                return _eventRepository.GetEventsByDateRange(userId, rangeStart, rangeEnd);
            }
            catch (ApplicationException appEx) // Catch repository exceptions
            {
                Console.WriteLine($"Service Error (GetEventsForDateRange): {appEx.Message}");
                // Log appEx properly
                throw; // Re-throw to allow UI layer to handle DB errors
            }
            catch (Exception ex) // Catch unexpected errors
            {
                Console.WriteLine($"Unexpected Service Error (GetEventsForDateRange): {ex.Message}");
                // Log ex properly
                throw new ApplicationException("An unexpected error occurred while retrieving events.", ex);
            }
        }

        /// <summary>
        /// Retrieves a specific event by its ID for a given user.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The event if found and belongs to the user; otherwise, null.</returns>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public Event? GetEventById(int eventId, int userId)
        {
            if (userId <= 0 || eventId <= 0)
            {
                // Could throw ArgumentException if preferred, but null fits the "not found" semantic
                return null;
            }

            try
            {
                return _eventRepository.GetEventById(eventId, userId);
            }
            catch (ApplicationException appEx) // Catch repository exceptions
            {
                Console.WriteLine($"Service Error (GetEventById): {appEx.Message}");
                // Log appEx
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (GetEventById): {ex.Message}");
                // Log ex
                throw new ApplicationException("An unexpected error occurred while retrieving the event.", ex);
            }
        }

        /// <summary>
        /// Adds a new event after validation.
        /// </summary>
        /// <param name="eventToAdd">The event data provided by the user.</param>
        /// <param name="userId">The ID of the user adding the event.</param>
        /// <returns>The newly created event with its assigned ID.</returns>
        /// <exception cref="ArgumentNullException">Thrown if eventToAdd is null.</exception>
        /// <exception cref="ArgumentException">Thrown if validation fails (e.g., empty title, invalid dates).</exception>
        /// <exception cref="InvalidOperationException">Thrown if the specified CategoryId does not exist for the user.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public Event AddEvent(Event eventToAdd, int userId)
        {
            if (eventToAdd == null) throw new ArgumentNullException(nameof(eventToAdd));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));

            // --- Validation ---
            ValidateEventData(eventToAdd, userId, isNewEvent: true); // Re-use validation logic

            try
            {
                // Set the user ID before sending to repository
                eventToAdd.UserId = userId;

                // Delegate to repository
                return _eventRepository.AddEvent(eventToAdd);
            }
            catch (ApplicationException appEx) // Catch repository exceptions
            {
                Console.WriteLine($"Service Error (AddEvent): {appEx.Message}");
                // Log appEx
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (AddEvent): {ex.Message}");
                // Log ex
                throw new ApplicationException("An unexpected error occurred while adding the event.", ex);
            }
        }


        /// <summary>
        /// Updates an existing event after validation.
        /// </summary>
        /// <param name="eventToUpdate">The event data with modifications.</param>
        /// <param name="userId">The ID of the user performing the update.</param>
        /// <returns>True if update was successful, false otherwise (e.g., event not found for user).</returns>
        /// <exception cref="ArgumentNullException">Thrown if eventToUpdate is null.</exception>
        /// <exception cref="ArgumentException">Thrown if validation fails (e.g., empty title, invalid ID, invalid dates).</exception>
        /// <exception cref="InvalidOperationException">Thrown if the specified CategoryId does not exist for the user.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public bool UpdateEvent(Event eventToUpdate, int userId)
        {
            if (eventToUpdate == null) throw new ArgumentNullException(nameof(eventToUpdate));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));
            if (eventToUpdate.Id <= 0) throw new ArgumentException("Event ID must be provided for update.", nameof(eventToUpdate.Id));

            // --- Validation ---
            ValidateEventData(eventToUpdate, userId, isNewEvent: false);

            try
            {
                // Delegate to repository (which handles the WHERE Id = @Id AND UserId = @UserId check)
                return _eventRepository.UpdateEvent(eventToUpdate, userId);
            }
            catch (ApplicationException appEx) // Catch repository exceptions
            {
                Console.WriteLine($"Service Error (UpdateEvent): {appEx.Message}");
                // Log appEx
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (UpdateEvent): {ex.Message}");
                // Log ex
                throw new ApplicationException("An unexpected error occurred while updating the event.", ex);
            }
        }

        /// <summary>
        /// Deletes an event.
        /// </summary>
        /// <param name="eventId">The ID of the event to delete.</param>
        /// <param name="userId">The ID of the user performing the deletion.</param>
        /// <returns>True if deletion was successful, false otherwise (e.g., event not found for user).</returns>
        /// <exception cref="ArgumentException">Thrown if eventId or userId is invalid.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public bool DeleteEvent(int eventId, int userId)
        {
            if (eventId <= 0) throw new ArgumentException("Invalid Event ID.", nameof(eventId));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));

            try
            {
                // Delegate to repository (which handles the WHERE Id = @Id AND UserId = @UserId check)
                return _eventRepository.DeleteEvent(eventId, userId);
            }
            catch (ApplicationException appEx) // Catch repository exceptions
            {
                Console.WriteLine($"Service Error (DeleteEvent): {appEx.Message}");
                // Log appEx
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (DeleteEvent): {ex.Message}");
                // Log ex
                throw new ApplicationException("An unexpected error occurred while deleting the event.", ex);
            }
        }


        // --- Private Helper Methods ---

        /// <summary>
        /// Performs common validation checks for event data.
        /// </summary>
        /// <param name="evt">The event object to validate.</param>
        /// <param name="userId">The user performing the action.</param>
        /// <param name="isNewEvent">Flag indicating if this is for Add (true) or Update (false).</param>
        private void ValidateEventData(Event evt, int userId, bool isNewEvent)
        {
            if (string.IsNullOrWhiteSpace(evt.Title))
            {
                throw new ArgumentException("Event title cannot be empty.", nameof(evt.Title));
            }
            if (evt.Title.Length > 200) // Assuming max length from table
            {
                throw new ArgumentException("Event title cannot exceed 200 characters.", nameof(evt.Title));
            }

            if (evt.EndTime < evt.StartTime)
            {
                throw new ArgumentException("End time cannot be before start time.", nameof(evt.EndTime));
            }

            // Validate CategoryId if provided
            if (evt.CategoryId.HasValue)
            {
                if (evt.CategoryId.Value <= 0)
                {
                    throw new ArgumentException("Invalid Category ID specified.", nameof(evt.CategoryId));
                }
                try
                {
                    // Check if the category exists AND belongs to the user
                    var category = _categoryRepository.GetCategoryById(evt.CategoryId.Value, userId);
                    if (category == null)
                    {
                        throw new InvalidOperationException($"Category with ID {evt.CategoryId.Value} not found or does not belong to the user.");
                    }
                }
                catch (ApplicationException)
                {
                    // If checking the category causes a DB error, we should probably fail validation too
                    throw new ApplicationException("Failed to verify the selected category due to a database error.");
                }
            }

            // Specific checks for update
            if (!isNewEvent)
            {
                if (evt.Id <= 0) throw new ArgumentException("Invalid Event ID for update.", nameof(evt.Id));
                // Potentially check if LastModifiedAt matches expectation (optimistic concurrency) - more advanced topic
            }
        }
    }
}