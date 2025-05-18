
---
**Project Report: reLIFE Scheduler Database Design**

**Course:** Database II
**Student Name:** Ahmed Emad & Natheer Nihad & Bavin Kawa
**Date:** 2025/05/18

**1. Introduction & Project Goal**

**1.1 Project Goal:**
The primary goal of the "reLIFE Scheduler" project is to develop a functional and user-friendly desktop application for personal event management. The application aims to provide users with the tools to organize their schedules, categorize their events, set reminders for important appointments, and maintain a history of past activities through an archiving feature. A key technical objective was to implement direct database interaction using ADO.NET with SQL Server, eschewing high-level Object-Relational Mappers (ORMs) to gain a deeper understanding of database operations and SQL query construction.

**1.2 Problem Statement:**
In an increasingly busy world, individuals often struggle to manage a multitude of personal and professional commitments effectively. Relying on memory or scattered notes can lead to missed appointments, forgotten tasks, and a general sense of disorganization. While numerous web-based and mobile calendar solutions exist, some users prefer or require a dedicated, local desktop application that offers simplicity, privacy, and focused functionality without the distractions or potential complexities of cloud-based services.

The reLIFE Scheduler addresses this by providing a secure, standalone application where users can:
*   Confidently store and manage their personal schedule.
*   Visually distinguish between different types of activities through categorization.
*   Receive timely alerts for upcoming events.
*   Maintain a clean active calendar by archiving past events while still retaining access to them.
*   Manage their account details, including password security and interface personalization.

The database component is critical to this, as it is responsible for the persistent storage, integrity, and efficient retrieval of all user-specific data.

**2. Database Schema Design**

The database for reLIFE Scheduler is designed using Microsoft SQL Server and consists of five core tables, structured to support the application's features efficiently and maintain data integrity.

**2.1 Entity-Relationship Diagram (ERD) - Conceptual**

*(If you can, include a simple ERD diagram here. If not, describe the relationships clearly.)*

*   **Users:** Central entity, owning Categories, Events, ArchivedEvents, and indirectly Reminders.
*   **Categories:** Belong to one User. One User can have many Categories.
*   **Events:** Belong to one User and can optionally belong to one Category. One User can have many Events.
*   **ArchivedEvents:** Stores historical data from Events. Belongs to one User and can optionally reference a Category.
*   **Reminders:** Belong to one Event. One Event can have many Reminders.

**2.2 Table Definitions:**

**2.2.1 `Users` Table**
*   **Purpose:** Stores user account information, including credentials for authentication.
*   **Schema:**
    ```sql
    CREATE TABLE Users (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Username NVARCHAR(100) NOT NULL UNIQUE,
        Email NVARCHAR(255) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(64) NOT NULL, -- SHA256 Hex
        PasswordSalt NVARCHAR(32) NOT NULL, -- Salt Hex
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
    );
    ```
*   **Design Rationale:**
    *   `Id`: Auto-incrementing primary key for unique user identification.
    *   `Username`, `Email`: Enforced as `UNIQUE` for distinct user registration. `NVARCHAR` for international character support.
    *   `PasswordHash`, `PasswordSalt`: Stores securely hashed passwords (SHA256) and their corresponding unique salts, preventing plain-text password storage. Lengths accommodate hex string representation.
    *   `CreatedAt`: Tracks account creation time.

**2.2.2 `Categories` Table**
*   **Purpose:** Allows users to define custom categories for their events, aiding in organization and filtering.
*   **Schema:**
    ```sql
    CREATE TABLE Categories (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserId INT NOT NULL,
        Name NVARCHAR(100) NOT NULL,
        ColorHex NVARCHAR(7) NOT NULL, -- e.g., "#RRGGBB"
        CONSTRAINT FK_Categories_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
        CONSTRAINT UQ_Categories_User_Name UNIQUE (UserId, Name) -- Recommended
    );
    CREATE INDEX IX_Categories_UserId ON Categories(UserId);
    ```
*   **Design Rationale:**
    *   `UserId`: Foreign key linking to the `Users` table. `ON DELETE CASCADE` ensures that if a user is deleted, their associated categories are also removed.
    *   `Name`: The user-defined name for the category.
    *   `ColorHex`: Stores the hex code for the category's color for UI display.
    *   `UQ_Categories_User_Name`: (Recommended unique constraint) Ensures a user cannot have two categories with the same name.
    *   `IX_Categories_UserId`: Index to speed up queries fetching categories for a specific user.

**2.2.3 `Events` Table**
*   **Purpose:** Stores details of active (non-archived) events scheduled by users.
*   **Schema:**
    ```sql
    CREATE TABLE Events (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserId INT NOT NULL,
        CategoryId INT NULL,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        StartTime DATETIME2 NOT NULL,
        EndTime DATETIME2 NOT NULL,
        IsAllDay BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        LastModifiedAt DATETIME2 NULL,
        IsArchived BIT NOT NULL DEFAULT 0, -- Flag for soft delete/archiving
        CONSTRAINT FK_Events_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Events_Category FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE NO ACTION ON UPDATE CASCADE
    );
    CREATE INDEX IX_Events_UserId_StartTime ON Events(UserId, StartTime);
    ```
*   **Design Rationale:**
    *   `UserId`: Foreign key to `Users`. `ON DELETE CASCADE` removes a user's events if their account is deleted.
    *   `CategoryId`: Nullable foreign key to `Categories`. `ON DELETE SET NULL` means if a category is deleted, associated events become uncategorized rather than being deleted or preventing category deletion.
    *   `StartTime`, `EndTime`: Define the event's duration. `DATETIME2` for precision.
    *   `IsAllDay`: Boolean flag.
    *   `IsArchived`: Flag to indicate if an event has been moved to the archive (soft delete from the active view). This is queried with `WHERE IsArchived = 0` for the main calendar.
    *   `IX_Events_UserId_StartTime`: Crucial composite index for efficient retrieval of events for a specific user within a date range.

**2.2.4 `ArchivedEvents` Table**
*   **Purpose:** Stores historical event data that has been explicitly archived by the user, keeping the main `Events` table lean.
*   **Schema:**
    ```sql
    CREATE TABLE ArchivedEvents (
        Id INT PRIMARY KEY,             -- Original Event ID, NOT an IDENTITY column
        UserId INT NOT NULL,
        CategoryId INT NULL,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        StartTime DATETIME2 NOT NULL,
        EndTime DATETIME2 NOT NULL,
        IsAllDay BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL,   -- Original creation date of the event
        LastModifiedAt DATETIME2 NULL,  -- Original last modified date
        ArchivedAt DATETIME2 NOT NULL DEFAULT GETDATE() -- Timestamp of when it was archived
    );
    CREATE INDEX IX_ArchivedEvents_UserId ON ArchivedEvents(UserId);
    CREATE INDEX IX_ArchivedEvents_ArchivedAt ON ArchivedEvents(ArchivedAt);
    ```
*   **Design Rationale:**
    *   `Id`: Stores the *original* `Id` from the `Events` table. This makes it easier to potentially restore or reference the event's history. It is the Primary Key but **not an IDENTITY column**. Data is inserted with this ID explicitly.
    *   The table largely mirrors the `Events` table structure to preserve all relevant details.
    *   `ArchivedAt`: Timestamp for when the event was moved to the archive.
    *   Foreign keys to `Users` or `Categories` are generally omitted here as the archived data is primarily for historical reference, and the parent user/category might eventually be deleted from the active tables. If strict referential integrity for archived data is required even after original deletion, these could be added with `ON DELETE NO ACTION`.

**2.2.5 `Reminders` Table**
*   **Purpose:** Stores reminder settings for specific events.
*   **Schema:**
    ```sql
    CREATE TABLE Reminders (
        Id INT PRIMARY KEY IDENTITY(1,1),
        EventId INT NOT NULL,
        MinutesBefore INT NOT NULL,
        IsEnabled BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Reminders_Event FOREIGN KEY (EventId) REFERENCES Events(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_Reminders_EventId ON Reminders(EventId);
    ```
*   **Design Rationale:**
    *   `EventId`: Foreign key linking to the `Events` table. `ON DELETE CASCADE` ensures that if an active event is deleted, its associated reminders are also automatically removed. This is crucial for data consistency.
    *   `MinutesBefore`: Defines when the reminder should trigger relative to the event's `StartTime`.
    *   `IsEnabled`: Allows users to temporarily disable a reminder without deleting it.
    *   `IX_Reminders_EventId`: Index for quick lookup of reminders for a given event.

**3. Data Integrity and Relationships**

*   **Referential Integrity:** Enforced through foreign key constraints, ensuring valid relationships between tables (e.g., an event must belong to an existing user).
*   **Cascade Actions:**
    *   `ON DELETE CASCADE` is used for `Users` to `Categories`/`Events`, and for `Events` to `Reminders`. This means deleting a parent record automatically deletes related child records, simplifying data management and preventing orphaned rows.
    *   `ON DELETE SET NULL` is used for `Categories` to `Events`. Deleting a category makes associated events "uncategorized."
*   **Unique Constraints:** Used on `Users.Username`, `Users.Email`, and (recommended) `Categories.Name` (scoped by `UserId`) to prevent duplicate entries.
*   **Data Types:** Chosen to appropriately store the information (e.g., `NVARCHAR` for text, `DATETIME2` for dates/times, `BIT` for booleans).
*   **Indexing:** Primary keys are automatically indexed. Additional non-clustered indexes (`IX_Categories_UserId`, `IX_Events_UserId_StartTime`, etc.) are created on frequently queried columns or columns used in `WHERE` clauses and `JOIN` conditions to optimize query performance.

**4. Database Interaction (from Application)**

*   The application uses ADO.NET (`Microsoft.Data.SqlClient`) for all database interactions.
*   Repositories in the Business Logic Layer are responsible for constructing and executing raw SQL queries (SELECT, INSERT, UPDATE, DELETE).
*   Parameterized queries are used exclusively to prevent SQL injection vulnerabilities.
*   The `DbHelper` class centralizes the retrieval of the database connection string from `appsettings.json`.

**5. Conclusion**

The database schema for reLIFE Scheduler is designed to be relational, maintain data integrity, and efficiently support the application's core functionalities. By using appropriate data types, constraints, indexes, and well-defined relationships, the database provides a solid backend for storing and managing user scheduling information. The decision to use direct ADO.NET allows for precise control over database interactions, which was a key learning objective of this project. Future enhancements to the application might necessitate schema modifications, such as adding tables or columns for more advanced recurring event patterns or shared calendar features, but the current design serves as a robust foundation.

---
