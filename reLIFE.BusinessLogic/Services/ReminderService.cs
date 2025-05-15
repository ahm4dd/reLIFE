using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using reLIFE.BusinessLogic.Repositories;
using reLIFE.Core.Models;

namespace reLIFE.BusinessLogic.Services
{
    public class ReminderService
    {
        private readonly ReminderRepository _reminderRepository;
        private readonly EventRepository _eventRepository;

        public ReminderService(ReminderRepository reminderRepository, EventRepository eventRepository)
        {
            _reminderRepository = reminderRepository ?? throw new ArgumentNullException(nameof(reminderRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }

        /// <summary>
        /// Gets all reminders associated with a specific event, verifying user ownership of the event first.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <param name="userId">The ID of the user requesting the reminders.</param>
        /// <returns>A list of reminders for the event, or an empty list if the event doesn't exist or belong to the user.</returns>
        /// <exception cref="ArgumentException">Thrown if userId is invalid.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors during checks or retrieval.</exception>
        public List<Reminder> GetRemindersForEvent(int eventId, int userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));
            if (eventId <= 0) return new List<Reminder>(); // Event ID must be valid

            try
            {
                // 1. Verify the user owns the event first
                var parentEvent = _eventRepository.GetEventById(eventId, userId);
                if (parentEvent == null)
                {
                    // Event not found or doesn't belong to the user - return empty list
                    Console.WriteLine($"GetRemindersForEvent: Event {eventId} not found for user {userId}.");
                    return new List<Reminder>();
                }

                // 2. If event ownership is confirmed, get the reminders
                return _reminderRepository.GetRemindersForEvent(eventId);
            }
            catch (ApplicationException appEx)
            {
                Console.WriteLine($"Service Error (GetRemindersForEvent): {appEx.Message}");
                throw; // Re-throw DB errors
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (GetRemindersForEvent): {ex.Message}");
                throw new ApplicationException($"An unexpected error occurred while retrieving reminders for event ID {eventId}.", ex);
            }
        }

        /// <summary>
        /// Saves (adds or updates) a reminder for an event, verifying event ownership.
        /// </summary>
        /// <param name="reminder">The reminder object to save. If Id > 0, it's an update; otherwise, an add.</param>
        /// <param name="userId">The ID of the user performing the action.</param>
        /// <returns>The saved Reminder object (with ID populated if added).</returns>
        /// <exception cref="ArgumentNullException">Thrown if reminder is null.</exception>
        /// <exception cref="ArgumentException">Thrown if validation fails (e.g., invalid IDs, negative MinutesBefore).</exception>
        /// <exception cref="InvalidOperationException">Thrown if the associated Event does not exist or belong to the user.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public Reminder SaveReminder(Reminder reminder, int userId)
        {
            if (reminder == null) throw new ArgumentNullException(nameof(reminder));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));
            if (reminder.EventId <= 0) throw new ArgumentException("Invalid Event ID in reminder.", nameof(reminder.EventId));
            if (reminder.MinutesBefore < 0) throw new ArgumentException("MinutesBefore cannot be negative.", nameof(reminder.MinutesBefore));

            try
            {
                // 1. Verify user owns the target event
                var parentEvent = _eventRepository.GetEventById(reminder.EventId, userId);
                if (parentEvent == null)
                {
                    throw new InvalidOperationException($"Cannot save reminder: Event with ID {reminder.EventId} not found or does not belong to user {userId}.");
                }

                // 2. Perform Add or Update based on Id
                if (reminder.Id > 0)
                {
                    // --- Update ---
                    bool updated = _reminderRepository.UpdateReminder(reminder);
                    if (!updated)
                    {
                        // This could happen if the reminder was deleted between load and save
                        throw new ApplicationException($"Failed to update reminder (ID: {reminder.Id}). It might no longer exist.");
                    }
                    return reminder; // Return the updated object passed in
                }
                else
                {
                    // --- Add ---
                    return _reminderRepository.AddReminder(reminder); // Returns the object with new ID
                }
            }
            catch (InvalidOperationException opEx) // Catch event ownership error
            {
                Console.WriteLine($"Service Validation (SaveReminder): {opEx.Message}");
                throw; // Re-throw specific validation error
            }
            catch (ApplicationException appEx) // Catch repo errors
            {
                Console.WriteLine($"Service Error (SaveReminder): {appEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (SaveReminder): {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while saving the reminder.", ex);
            }
        }

        /// <summary>
        /// Deletes a specific reminder by its ID, verifying ownership via the associated event.
        /// </summary>
        /// <param name="reminderId">The ID of the reminder to delete.</param>
        /// <param name="userId">The ID of the user performing the deletion.</param>
        /// <returns>True if deletion was successful, false otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown if IDs are invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the reminder or its associated event cannot be verified for the user.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public bool DeleteReminder(int reminderId, int userId)
        {
            if (reminderId <= 0) throw new ArgumentException("Invalid Reminder ID.", nameof(reminderId));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));

            try
            {
                // 1. Get the reminder to find its EventId
                var reminder = _reminderRepository.GetReminderById(reminderId);
                if (reminder == null)
                {
                    // Reminder doesn't exist, consider deletion successful? Or return false?
                    // Let's return false to indicate it wasn't found.
                    return false;
                }

                // 2. Verify user owns the associated event
                var parentEvent = _eventRepository.GetEventById(reminder.EventId, userId);
                if (parentEvent == null)
                {
                    // User doesn't own the event associated with this reminder
                    throw new InvalidOperationException($"Cannot delete reminder: Access denied or parent event (ID: {reminder.EventId}) not found for user {userId}.");
                }

                // 3. If ownership verified, delete the reminder
                return _reminderRepository.DeleteReminder(reminderId);
            }
            catch (InvalidOperationException opEx) // Catch ownership error
            {
                Console.WriteLine($"Service Validation (DeleteReminder): {opEx.Message}");
                throw;
            }
            catch (ApplicationException appEx) // Catch repo errors
            {
                Console.WriteLine($"Service Error (DeleteReminder): {appEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (DeleteReminder): {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while deleting the reminder.", ex);
            }
        }

        /// <summary>
        /// Deletes all reminders for a specific event. Intended for internal use by EventService
        /// when archiving/deleting events, but exposed here for potential direct use if needed.
        /// Performs event ownership check.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <param name="userId">The ID of the user owning the event.</param>
        /// <returns>True if the operation completed successfully (even if no reminders existed).</returns>
        /// <exception cref="ArgumentException">Thrown if IDs are invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the event does not exist or belong to the user.</exception>
        /// <exception cref="ApplicationException">Thrown for underlying repository/database errors.</exception>
        public bool DeleteRemindersForEvent(int eventId, int userId)
        {
            if (eventId <= 0) throw new ArgumentException("Invalid Event ID.", nameof(eventId));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));

            try
            {
                // 1. Verify ownership of the event
                var parentEvent = _eventRepository.GetEventById(eventId, userId);
                if (parentEvent == null)
                {
                    throw new InvalidOperationException($"Cannot delete reminders: Event with ID {eventId} not found or does not belong to user {userId}.");
                }

                // 2. Call repository to delete all reminders for this event
                return _reminderRepository.DeleteRemindersForEvent(eventId);
            }
            catch (InvalidOperationException opEx) // Catch ownership error
            {
                Console.WriteLine($"Service Validation (DeleteRemindersForEvent): {opEx.Message}");
                throw;
            }
            catch (ApplicationException appEx) // Catch repo errors
            {
                Console.WriteLine($"Service Error (DeleteRemindersForEvent): {appEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (DeleteRemindersForEvent): {ex.Message}");
                throw new ApplicationException($"An unexpected error occurred while deleting reminders for event ID {eventId}.", ex);
            }
        }


        public List<Reminder> GetActiveUserReminders(int userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));

            try
            {
                // 1. Get all future events for the user
                //    We need a range slightly beyond now to catch reminders set far in advance
                //    Let's fetch events starting from now (or slightly before).
                DateTime rangeStart = DateTime.Now.AddHours(-1); // Look back slightly
                // No practical upper end date limit for this approach
                List<Event> futureEvents = _eventRepository.GetEventsByDateRange(userId, rangeStart, DateTime.MaxValue);

                if (!futureEvents.Any())
                {
                    return new List<Reminder>(); // No future events, so no relevant reminders
                }

                // 2. Get reminders ONLY for those future events
                var futureEventIds = futureEvents.Select(e => e.Id).ToList();
                var allReminders = new List<Reminder>();

                // Fetch reminders for each event (less efficient than one JOIN, but works with current repo)
                // Consider adding a GetRemindersForEventList to the repo later for optimization
                foreach (int eventId in futureEventIds)
                {
                    allReminders.AddRange(_reminderRepository.GetRemindersForEvent(eventId));
                }

                // 3. Filter for enabled reminders
                //    (Could also filter by ReminderTime < upcomingLimit here if needed)
                var activeReminders = allReminders
                                        .Where(r => r.IsEnabled)
                                        .ToList();

                // 4. Optional: Order them by calculated reminder time (requires joining back to event start time)
                //    This sorting is better done in the Repository JOIN method,
                //    but we can do an approximate sort here.
                var sortedReminders = activeReminders
                    .Select(r => new
                    {
                        Reminder = r,
                        EventStartTime = futureEvents.FirstOrDefault(e => e.Id == r.EventId)?.StartTime ?? DateTime.MaxValue
                    })
                    .OrderBy(x => x.EventStartTime.AddMinutes(-x.Reminder.MinutesBefore))
                    .Select(x => x.Reminder)
                    .ToList();

                return sortedReminders;

            }
            catch (ApplicationException) { throw; }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (GetActiveUserReminders): {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while retrieving reminders.", ex);
            }
        }
        // --- Optional: Method for checking upcoming reminders ---
        // This would be more complex, potentially involving joining Events and Reminders
        // and calculating trigger times. Omitted for now for simplicity.
        // public List<UpcomingReminderInfo> GetUpcomingReminders(int userId, DateTime checkUntil) { ... }
    }
}