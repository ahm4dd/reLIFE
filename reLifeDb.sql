CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(64) NOT NULL,
    PasswordSalt NVARCHAR(32) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    ColorHex NVARCHAR(7) NOT NULL, -- e.g., "#RRGGBB"
    CONSTRAINT FK_Categories_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
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
CREATE INDEX IX_Events_UserId_StartTime ON Events(UserId, StartTime);

-- Add a flag to indicate if the event is active
ALTER TABLE Events
ADD IsArchived BIT NOT NULL DEFAULT 0;

CREATE TABLE ArchivedEvents (
    Id INT PRIMARY KEY,
    OriginalEventId INT NULL, -- (Optional) Store the original event's Id if Primary Key is new IDENTITY
    UserId INT NOT NULL,
    CategoryId INT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    StartTime DATETIME2 NOT NULL,
    EndTime DATETIME2 NOT NULL,
    IsAllDay BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL,
    LastModifiedAt DATETIME2 NULL,
    ArchivedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
);

CREATE INDEX IX_ArchivedEvents_UserId ON ArchivedEvents(UserId);
CREATE INDEX IX_ArchivedEvents_ArchivedAt ON ArchivedEvents(ArchivedAt); -- Useful for clearing old archives

CREATE TABLE Reminders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EventId INT NOT NULL,
    MinutesBefore INT NOT NULL,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Reminders_Event FOREIGN KEY (EventId) REFERENCES Events(Id) ON DELETE CASCADE
);
CREATE INDEX IX_Reminders_EventId ON Reminders(EventId);

