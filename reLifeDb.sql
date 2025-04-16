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
    CONSTRAINT FK_Events_Category FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE NO ACTION
);
CREATE INDEX IX_Events_UserId_StartTime ON Events(UserId, StartTime); -- Useful for fetching events by date