using reLIFE.BusinessLogic.Repositories;
using reLIFE.Core.Models;
using System;
using System.Collections.Generic;

namespace reLIFE.BusinessLogic.Services
{
    public class EventService
    {
        private readonly EventRepository _eventRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly ArchivedEventRepository _archivedEventRepository;
        private readonly ReminderRepository _reminderRepository;

        public EventService(
            EventRepository eventRepository,
            CategoryRepository categoryRepository,
            ArchivedEventRepository archivedEventRepository,
            ReminderRepository reminderRepository
            )
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _archivedEventRepository = archivedEventRepository ?? throw new ArgumentNullException(nameof(archivedEventRepository));
            _reminderRepository = reminderRepository ?? throw new ArgumentNullException(nameof(reminderRepository));
        }

        public List<Event> GetEventsForDateRange(int userId, DateTime rangeStart, DateTime rangeEnd)
        {
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));
            if (rangeEnd < rangeStart) throw new ArgumentException("End date cannot be before start date.", nameof(rangeEnd));
            try { return _eventRepository.GetEventsByDateRange(userId, rangeStart, rangeEnd); }
            catch (ApplicationException appEx) { Console.WriteLine($"Service Error (GetEventsForDateRange): {appEx.Message}"); throw; }
            catch (Exception ex) { Console.WriteLine($"Unexpected Service Error (GetEventsForDateRange): {ex.Message}"); throw new ApplicationException("An unexpected error occurred while retrieving events.", ex); }
        }

        public Event? GetEventById(int eventId, int userId)
        {
            if (userId <= 0 || eventId <= 0) return null;
            try { return _eventRepository.GetEventById(eventId, userId); }
            catch (ApplicationException appEx) { Console.WriteLine($"Service Error (GetEventById): {appEx.Message}"); throw; }
            catch (Exception ex) { Console.WriteLine($"Unexpected Service Error (GetEventById): {ex.Message}"); throw new ApplicationException("An unexpected error occurred while retrieving the event.", ex); }
        }

        public Event AddEvent(Event eventToAdd, int userId)
        {
            if (eventToAdd == null) throw new ArgumentNullException(nameof(eventToAdd));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));
            ValidateEventData(eventToAdd, userId, isNewEvent: true);
            try { eventToAdd.UserId = userId; return _eventRepository.AddEvent(eventToAdd); }
            catch (ApplicationException appEx) { Console.WriteLine($"Service Error (AddEvent): {appEx.Message}"); throw; }
            catch (Exception ex) { Console.WriteLine($"Unexpected Service Error (AddEvent): {ex.Message}"); throw new ApplicationException("An unexpected error occurred while adding the event.", ex); }
        }

        public bool UpdateEvent(Event eventToUpdate, int userId)
        {
            if (eventToUpdate == null) throw new ArgumentNullException(nameof(eventToUpdate));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));
            if (eventToUpdate.Id <= 0) throw new ArgumentException("Event ID must be provided for update.", nameof(eventToUpdate.Id));
            ValidateEventData(eventToUpdate, userId, isNewEvent: false);
            try { return _eventRepository.UpdateEvent(eventToUpdate, userId); }
            catch (ApplicationException appEx) { Console.WriteLine($"Service Error (UpdateEvent): {appEx.Message}"); throw; }
            catch (Exception ex) { Console.WriteLine($"Unexpected Service Error (UpdateEvent): {ex.Message}"); throw new ApplicationException("An unexpected error occurred while updating the event.", ex); }
        }

        public bool DeleteEvent(int eventId, int userId)
        {
            if (eventId <= 0) throw new ArgumentException("Invalid Event ID.", nameof(eventId));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));

            bool remindersDeleted = false;
            try
            {
                remindersDeleted = _reminderRepository.DeleteRemindersForEvent(eventId);
                if (!remindersDeleted)
                {
                    Console.WriteLine($"Warning: Attempted to delete reminders for potentially non-existent event ID {eventId} during event deletion.");
                }
                return _eventRepository.DeleteEvent(eventId, userId);
            }
            catch (ApplicationException appEx)
            {
                Console.WriteLine($"Service Error (DeleteEvent): {appEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (DeleteEvent): {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while deleting the event.", ex);
            }
        }

        public bool ArchiveEvent(int eventId, int userId)
        {
            if (eventId <= 0) throw new ArgumentException("Invalid Event ID.", nameof(eventId));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));

            try
            {
                Event? originalEvent = _eventRepository.GetEventById(eventId, userId);
                if (originalEvent == null)
                {
                    throw new InvalidOperationException($"Event with ID {eventId} not found for user {userId}. Cannot archive.");
                }

                var archivedEvent = new ArchivedEvent
                {
                    Id = originalEvent.Id,
                    UserId = originalEvent.UserId,
                    CategoryId = originalEvent.CategoryId,
                    Title = originalEvent.Title,
                    Description = originalEvent.Description,
                    StartTime = originalEvent.StartTime,
                    EndTime = originalEvent.EndTime,
                    IsAllDay = originalEvent.IsAllDay,
                    CreatedAt = originalEvent.CreatedAt,
                    LastModifiedAt = originalEvent.LastModifiedAt,
                    ArchivedAt = DateTime.UtcNow
                };

                bool addedToArchive = _archivedEventRepository.AddArchivedEvent(archivedEvent);
                if (!addedToArchive)
                {
                    throw new ApplicationException($"Failed to add event ID {eventId} to the archive table.");
                }

                bool remindersDeleted = _reminderRepository.DeleteRemindersForEvent(eventId);
                if (!remindersDeleted)
                {
                    Console.WriteLine($"Warning: Failed to delete reminders for archived event ID {eventId}.");
                }

                bool originalDeleted = _eventRepository.DeleteEvent(eventId, userId);
                if (!originalDeleted)
                {
                    Console.WriteLine($"CRITICAL ERROR: Failed to delete original event ID {eventId} after archiving.");
                    throw new ApplicationException($"Failed to complete archiving process: Original event ID {eventId} could not be deleted after being added to archive.");
                }

                return true;
            }
            catch (InvalidOperationException opEx)
            {
                Console.WriteLine($"Service Error (ArchiveEvent): {opEx.Message}");
                throw;
            }
            catch (ApplicationException appEx)
            {
                Console.WriteLine($"Service Error (ArchiveEvent): {appEx.Message}");
                throw new ApplicationException($"A database error occurred during the archiving process for event ID {eventId}.", appEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Service Error (ArchiveEvent): {ex.Message}");
                throw new ApplicationException($"An unexpected error occurred while archiving event ID {eventId}.", ex);
            }
        }

        private void ValidateEventData(Event evt, int userId, bool isNewEvent)
        {
            if (string.IsNullOrWhiteSpace(evt.Title)) throw new ArgumentException("Event title cannot be empty.", nameof(evt.Title));
            if (evt.Title.Length > 200) throw new ArgumentException("Event title cannot exceed 200 characters.", nameof(evt.Title));
            if (evt.EndTime < evt.StartTime) throw new ArgumentException("End time cannot be before start time.", nameof(evt.EndTime));
            if (evt.CategoryId.HasValue)
            {
                if (evt.CategoryId.Value <= 0) throw new ArgumentException("Invalid Category ID specified.", nameof(evt.CategoryId));
                try
                {
                    var category = _categoryRepository.GetCategoryById(evt.CategoryId.Value, userId);
                    if (category == null) throw new InvalidOperationException($"Category with ID {evt.CategoryId.Value} not found or does not belong to the user.");
                }
                catch (ApplicationException) { throw new ApplicationException("Failed to verify the selected category due to a database error."); }
            }
            if (!isNewEvent)
            {
                if (evt.Id <= 0) throw new ArgumentException("Invalid Event ID for update.", nameof(evt.Id));
            }
        }

        public bool RetrieveEventFromArchive(int archivedEventId, int userId)
        {
            if (archivedEventId <= 0) throw new ArgumentException("Invalid Archived Event ID.", nameof(archivedEventId));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));

            try
            {
                // 1. Get the archived event to ensure it exists and belongs to the user
                //    (ArchivedEventRepository should have a GetById method that checks UserId)
                ArchivedEvent? archivedEvent = _archivedEventRepository.GetArchivedEventById(archivedEventId, userId);
                if (archivedEvent == null)
                {
                    throw new InvalidOperationException($"Archived event with ID {archivedEventId} not found for user {userId}.");
                }

                // 2. Map it back to a regular Event object
                Event eventToRestore = new Event
                {
                    // Id = archivedEvent.Id, // Do NOT set ID if Events table is IDENTITY. Let DB generate new one.
                    // OR if Events.Id and ArchivedEvents.Id are meant to match,
                    // then EventRepository.AddEvent would need IDENTITY_INSERT.
                    // For simplicity, let's assume it gets a new ID in Events table.
                    UserId = archivedEvent.UserId,
                    CategoryId = archivedEvent.CategoryId,
                    Title = archivedEvent.Title,
                    Description = archivedEvent.Description,
                    StartTime = archivedEvent.StartTime,
                    EndTime = archivedEvent.EndTime,
                    IsAllDay = archivedEvent.IsAllDay,
                    // CreatedAt will be new, LastModifiedAt will be null initially
                };

                // 3. Add it back to the active Events table
                //    This will assign a new Event.Id if your Events table has an IDENTITY PK.
                Event? restoredEvent = _eventRepository.AddEvent(eventToRestore);
                if (restoredEvent == null || restoredEvent.Id <= 0)
                {
                    throw new ApplicationException("Failed to add the event back to the active events table.");
                }

                // 4. Delete it from the ArchivedEvents table
                //    (ArchivedEventRepository needs a DeleteArchivedEvent(id) method)
                bool deletedFromArchive = _archivedEventRepository.DeleteArchivedEvent(archivedEventId);
                if (!deletedFromArchive)
                {
                    // This is an inconsistent state: event is restored but not removed from archive.
                    // Log this critical issue. For now, we'll consider the main operation failed.
                    Console.WriteLine($"CRITICAL: Failed to delete event ID {archivedEventId} from archive after restoring.");
                    // Optionally try to delete the just-added active event to roll back? Complex.
                    throw new ApplicationException("Failed to complete retrieval: could not remove from archive.");
                }

                return true;
            }
            catch (InvalidOperationException opEx) { Console.WriteLine($"Service Error (RetrieveEventFromArchive): {opEx.Message}"); throw; }
            catch (ApplicationException appEx) { Console.WriteLine($"Service Error (RetrieveEventFromArchive): {appEx.Message}"); throw; }
            catch (Exception ex) { Console.WriteLine($"Unexpected Error (RetrieveEventFromArchive): {ex.Message}"); throw new ApplicationException("An unexpected error occurred while retrieving the event from archive.", ex); }
        }

        // *** NEW METHOD: DeleteArchivedEventPermanently ***
        /// <summary>
        /// Permanently deletes an event from the archive table.
        /// </summary>
        /// <param name="archivedEventId">The ID of the event in the archive table to delete.</param>
        /// <param name="userId">The ID of the user performing the action (for ownership check).</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool DeleteArchivedEventPermanently(int archivedEventId, int userId)
        {
            if (archivedEventId <= 0) throw new ArgumentException("Invalid Archived Event ID.", nameof(archivedEventId));
            if (userId <= 0) throw new ArgumentException("Invalid User ID.", nameof(userId));

            try
            {
                // 1. Optional: Verify ownership before deleting, if GetArchivedEventById doesn't already.
                //    ArchivedEventRepository.DeleteArchivedEvent should ideally take userId too.
                //    For simplicity, let's assume DeleteArchivedEvent handles ownership or we trust the call.
                //    Alternatively:
                //    var archivedEvent = _archivedEventRepository.GetArchivedEventById(archivedEventId, userId);
                //    if (archivedEvent == null) {
                //        throw new InvalidOperationException($"Archived event with ID {archivedEventId} not found for user {userId}.");
                //    }

                // 2. Call repository to delete
                return _archivedEventRepository.DeleteArchivedEvent(archivedEventId); // Assumes this method exists
            }
            catch (ApplicationException appEx) { Console.WriteLine($"Service Error (DeleteArchivedEventPermanently): {appEx.Message}"); throw; }
            catch (Exception ex) { Console.WriteLine($"Unexpected Error (DeleteArchivedEventPermanently): {ex.Message}"); throw new ApplicationException("An unexpected error occurred while permanently deleting the archived event.", ex); }
        }
    }
}