

**reLIFE Scheduler - Application Documentation**

**Version:** 1.0
**Date:** 2025/05/25
**Author(s):** Ahmed Emad

**Table of Contents**

**Part I: User Documentation**

1.  **Introduction to reLIFE Scheduler**
    1.1. Welcome
    1.2. What is reLIFE Scheduler?
    1.3. System Requirements
    1.4. Installation (If Applicable - for desktop, usually just running the .exe)
2.  **Getting Started**
    2.1. Launching the Application
    2.2. User Registration
        2.2.1. Creating a New Account
        2.2.2. Password Requirements
    2.3. Logging In
    2.4. Main Dashboard Overview
        2.4.1. Navigation Panel
        2.4.2. Content Area
3.  **Core Features & Usage**
    3.1. Calendar View (`CalendarViewForm`)
        3.1.1. Selecting a Date
        3.1.2. Adding a New Event (Launching `EventForm`)
        3.1.3. Viewing Event Details (Event Cards)
        3.1.4. Filtering Events by Category
        3.1.5. Editing an Existing Event
        3.1.6. Deleting an Active Event
        3.1.7. Archiving an Active Event
    3.2. Managing Categories (`CategoryManagerForm`)
        3.2.1. Viewing Existing Categories
        3.2.2. Adding a New Category (Name and Color)
        3.2.3. Editing an Existing Category
        3.2.4. Deleting a Category (and its impact on events)
    3.3. Event Form (`EventForm` - Add/Edit)
        3.3.1. Entering Event Title and Description
        3.3.2. Setting Start and End Dates & Times
        3.3.3. Marking an Event as "All Day"
        3.3.4. Assigning a Category
        3.3.5. Setting Event Reminders
        3.3.6. Saving or Cancelling Changes
    3.4. Managing Reminders (`ReminderListViewForm`)
        3.4.1. Viewing Active/Upcoming Reminders
        3.4.2. Navigating to the Associated Event
        3.4.3. Deleting a Reminder
    3.5. Managing Archived Events (`ArchiveViewForm`)
        3.5.1. Viewing Archived Events
        3.5.2. Filtering Archived Events (by Date/Category)
        3.5.3. Viewing Details of an Archived Event
        3.5.4. Retrieving (Unarchiving) an Event
        3.5.5. Permanently Deleting an Archived Event
    3.6. Account Settings (`AccountSettingsForm`)
        3.6.1. Viewing Username
        3.6.2. Updating Email Address
        3.6.3. Changing Password
        3.6.4. Customizing Application Theme & Color Scheme
    3.7. Logging Out
4.  **Troubleshooting (Optional Basic Section)**
    4.1. Login Issues
    4.2. Data Not Displaying

**Part II: Technical Documentation**

6.  **Project Overview**
    6.1. Purpose and Objectives (Recap)
    6.2. Technology Stack
7.  **System Architecture**
    7.1. Layered Design Philosophy
    7.2. Presentation Layer (`reLIFE.WinFormsUI`)
        7.2.1. Key Forms and their Roles (Dashboard, Modals, Embedded Views)
        7.2.2. UI Theming (MaterialSkin 2 Integration, `ThemeHelper`)
        7.2.3. Configuration (`appsettings.json`)
    7.3. Business Logic Layer (`reLIFE.BusinessLogic`)
        7.3.1. Services (Responsibilities and Interactions)
        7.3.2. Repositories (Data Access Abstraction)
        7.3.3. Utility Classes (`DbHelper`, `PasswordHasher`, `Validation`)
    7.4. Core Layer (`reLIFE.Core`)
        7.4.1. Data Models (POCOs)
    7.5. Database Layer (SQL Server)
        7.5.1. Rationale for SQL Server
        7.5.2. Data Access Strategy (ADO.NET without ORM)
8.  **Database Design**
    8.1. Entity-Relationship Diagram (ERD)
        *[Insert EER Diagram Image Here]*
    8.2. Table Schemas (DDL for each table)
        8.2.1. `Users`
        8.2.2. `Categories`
        8.2.3. `Events`
        8.2.4. `ArchivedEvents`
        8.2.5. `Reminders`
    8.3. Relationships, Constraints, and Integrity Rules
        8.3.1. Foreign Key Definitions and Cascade Actions
        8.3.2. Unique Constraints
        8.3.3. Default Values
    8.4. Indexing Strategy and Rationale
    8.5. Conceptual Relationships for `ArchivedEvents`
9.  **Detailed Workflow Examples with Code References**
    9.1. User Registration and Password Hashing
        *   Flow: `RegistrationForm` -> `AuthService.Register` -> `PasswordHasher.HashPassword` -> `UserRepository.AddUser` -> SQL `INSERT`.
        *   Key Code Snippet: `PasswordHasher.HashPassword` method.
    9.2. Login and Password Verification
        *   Flow: `LoginForm` -> `AuthService.Login` -> `UserRepository.GetUserByUsername` -> `PasswordHasher.VerifyPassword`.
        *   Key Code Snippet: `PasswordHasher.VerifyPassword` method.
    9.3. Fetching and Displaying Events with Category Filtering
        *   Flow: `CalendarViewForm.LoadAndDisplayEvents` -> `EventService.GetEventsForDateRange` -> `EventRepository.GetEventsByDateRange` (SQL `SELECT` with `WHERE` and `IsArchived=0`) -> UI filtering in `CalendarViewForm.FilterAndDisplayEvents` -> Dynamic card creation.
        *   Key Code Snippet: `EventRepository.GetEventsByDateRange` SQL query.
        *   Key Code Snippet: `CalendarViewForm.FilterAndDisplayEvents` filtering loop.
    9.4. Archiving an Event (Multi-Step Process)
        *   Flow: `CalendarViewForm.ArchiveEvent` (via card button) -> `EventService.ArchiveEvent` -> `EventRepo.GetEventById` -> Map `Event` to `ArchivedEvent` -> `ArchivedEventRepo.AddArchivedEvent` -> `ReminderRepo.DeleteRemindersForEvent` -> `EventRepo.DeleteEvent`.
        *   Key Code Snippet: `EventService.ArchiveEvent` method logic.
        *   Key Code Snippet: `ArchivedEventRepository.AddArchivedEvent` (showing direct ID insert).
    9.5. Retrieving Active Reminders (using SQL JOIN)
        *   Flow: `ReminderListViewForm.LoadAndDisplayReminders` -> `ReminderService.GetActiveUpcomingReminderInfos` -> `ReminderRepository.GetActiveUpcomingReminderInfosForUser`.
        *   Key Code Snippet: `ReminderRepository.GetActiveUpcomingReminderInfosForUser` SQL query with `JOIN`.
    9.6. Theme Application and Propagation
        *   Flow: `Program.cs` sets initial theme via `ThemeHelper`. `AccountSettingsForm` allows changes, calling `ThemeHelper.ApplyTheme`. `ThemeHelper` updates `MaterialSkinManager.Instance` and iterates through open `MaterialForm`s to refresh them. `MainForm.LoadControl` re-adds embedded forms to the manager.
        *   Key Code Snippet: `ThemeHelper.ApplyTheme` method.
10. **Security Considerations**
    10.1. Password Storage (SHA256 + Salting)
    10.2. SQL Injection Prevention (Parameterized Queries)
    10.3. Data Access Control (User-Specific Data Retrieval)
11. **Deployment (If Applicable)**
    11.1. Prerequisites (.NET Runtime)
    11.2. Database Setup (SQL Server instance, running the schema script)
    11.3. Application Files (Executable and `appsettings.json`)
12. **Future Development and Enhancements**
    *(List from previous detailed report: Advanced Recurring Events, Pop-up Notifications, Time Zone Support, Search, Settings Persistence, Import/Export, etc.)*
13. **Conclusion**
14. **Appendix**
    14.1. Full Database Schema SQL Script
    14.2. Key Configuration File (`appsettings.json` structure)
    14.3. Additional Code Snippets (as deemed necessary)
