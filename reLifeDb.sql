CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(64) NOT NULL, -- SHA256 produces a 64-character hex string
    PasswordSalt NVARCHAR(32) NOT NULL, -- Store salt as hex or Base64 (adjust size accordingly)
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    ColorHex NVARCHAR(7) NOT NULL, -- e.g., "#RRGGBB"
    CONSTRAINT FK_Categories_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    -- Add Unique constraint for UserId, Name ?
    -- CONSTRAINT UQ_Categories_User_Name UNIQUE (UserId, Name)
);
CREATE INDEX IX_Categories_UserId ON Categories(UserId);

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
    CONSTRAINT FK_Events_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Events_Category FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE NO ACTION ON UPDATE CASCADE
);
CREATE INDEX IX_Events_UserId_StartTime ON Events(UserId, StartTime); -- Useful for fetching events by date


-- TODO:










-- Add a flag to indicate if the event is active
ALTER TABLE Events
ADD IsArchived BIT NOT NULL DEFAULT 0;

CREATE TABLE ArchivedEvents (
    Id INT PRIMARY KEY,                     -- Keep original ID? Or new IDENTITY? (Keeping original is simpler for lookup)
    OriginalEventId INT NULL,             -- (Optional) Store the original event's Id if Primary Key is new IDENTITY
    UserId INT NOT NULL,
    CategoryId INT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    StartTime DATETIME2 NOT NULL,
    EndTime DATETIME2 NOT NULL,
    IsAllDay BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL,       -- Original creation date
    LastModifiedAt DATETIME2 NULL,      -- Original last modified date
    ArchivedAt DATETIME2 NOT NULL DEFAULT GETDATE(), -- When it was moved to archive

    -- Foreign keys usually not necessary in archive unless linking to
    -- things that are also archived consistently (less common).
    -- We are explicitly NOT using ON DELETE SET NULL or CASCADE here
    -- as the data is meant to be static history.
    -- CONSTRAINT FK_ArchivedEvents_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE NO ACTION -- Could add but not common in archives
    -- CONSTRAINT FK_ArchivedEvents_Category FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE NO ACTION -- Could add
);

-- Add indexes for lookup or user filtering
CREATE INDEX IX_ArchivedEvents_UserId ON ArchivedEvents(UserId);
CREATE INDEX IX_ArchivedEvents_ArchivedAt ON ArchivedEvents(ArchivedAt); -- Useful for clearing old archives

CREATE TABLE Reminders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EventId INT NOT NULL,                   -- Which event this reminder is for
    MinutesBefore INT NOT NULL,             -- How many minutes before Event.StartTime to trigger
    IsEnabled BIT NOT NULL DEFAULT 1,       -- Allows temporarily disabling a reminder
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Reminders_Event FOREIGN KEY (EventId) REFERENCES Events(Id) ON DELETE CASCADE -- Delete reminder if event is deleted
);

-- Index to efficiently find reminders for a specific event
CREATE INDEX IX_Reminders_EventId ON Reminders(EventId);
