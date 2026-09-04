-- Creating Database RaceDay
IF DB_ID('RaceDay') IS NULL
BEGIN
    CREATE DATABASE RaceDay;
END;
GO

--Using RaceDay Database
USE RaceDay;
GO

-- Drop Existing Tables
IF OBJECT_ID('dbo.Result', 'U') IS NOT NULL
    DROP TABLE dbo.Result;
IF OBJECT_ID('dbo.Enrolment', 'U') IS NOT NULL
    DROP TABLE dbo.Enrolment;
IF OBJECT_ID('dbo.Category', 'U') IS NOT NULL
    DROP TABLE dbo.Category;
IF OBJECT_ID('dbo.Event', 'U') IS NOT NULL
    DROP TABLE dbo.Event;
IF OBJECT_ID('dbo.Participant', 'U') IS NOT NULL
    DROP TABLE dbo.Participant;
IF OBJECT_ID('dbo.[User]', 'U') IS NOT NULL
    DROP TABLE dbo.[User];
GO

-- Creating User Table
CREATE TABLE dbo.[User]
(
    UserID INT IDENTITY(1,1) NOT NULL,
    FirstName VARCHAR(50) NOT NULL,
    Surname VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Role VARCHAR(20) NOT NULL,
    ProfilePictureURL VARCHAR(500) NULL,
    CONSTRAINT PK_User
        PRIMARY KEY (UserID),
    CONSTRAINT UQ_User_Email
        UNIQUE (Email),
    CONSTRAINT CK_User_Role
        CHECK (Role IN ('Organiser', 'Participant'))
);
GO

-- Creating Participant TABLE
CREATE TABLE dbo.Participant
(
    ParticipantID INT IDENTITY(1,1) NOT NULL,
    UserID INT NOT NULL,
    CONSTRAINT PK_Participant
        PRIMARY KEY (ParticipantID),
    CONSTRAINT UQ_Participant_User
        UNIQUE (UserID),
    CONSTRAINT FK_Participant_User
        FOREIGN KEY (UserID)
        REFERENCES dbo.[User](UserID)
);
GO

-- Creating Event Table
CREATE TABLE dbo.Event
(
    EventID INT IDENTITY(1,1) NOT NULL,
    OrganiserID INT NOT NULL,
    EventName VARCHAR(100) NOT NULL,
    Description VARCHAR(500) NOT NULL,
    EventType VARCHAR(20) NOT NULL,
    Distance DECIMAL(6,2) NOT NULL,
    Location VARCHAR(150) NOT NULL,
    EventDate DATE NOT NULL,
    BannerImageURL VARCHAR(500) NULL,
    CONSTRAINT PK_Event
        PRIMARY KEY (EventID),
    CONSTRAINT FK_Event_Organiser
        FOREIGN KEY (OrganiserID)
        REFERENCES dbo.[User](UserID),
    CONSTRAINT CK_Event_Type
        CHECK (EventType IN ('Run', 'Walk', 'Cycle')),
    CONSTRAINT CK_Event_Distance
        CHECK (Distance > 0)
);
GO

-- Creating Category Table
CREATE TABLE dbo.Category
(
    CategoryID INT IDENTITY(1,1) NOT NULL,
    EventID INT NOT NULL,
    CategoryName VARCHAR(50) NOT NULL,
    CONSTRAINT PK_Category
        PRIMARY KEY (CategoryID),
    CONSTRAINT FK_Category_Event
        FOREIGN KEY (EventID)
        REFERENCES dbo.Event(EventID),
    CONSTRAINT UQ_Category_Event_Name
        UNIQUE (EventID, CategoryName)
);
GO


-- Creating Enrolment Table
CREATE TABLE dbo.Enrolment
(
    EnrolmentID INT IDENTITY(1,1) NOT NULL,
    ParticipantID INT NOT NULL,
    EventID INT NOT NULL,
    CategoryID INT NOT NULL,
    EnrolmentDate DATETIME2 NOT NULL
        CONSTRAINT DF_Enrolment_Date
        DEFAULT SYSDATETIME(),
    Status VARCHAR(20) NOT NULL
        CONSTRAINT DF_Enrolment_Status
        DEFAULT 'Pending',
    CONSTRAINT PK_Enrolment
        PRIMARY KEY (EnrolmentID),
    CONSTRAINT FK_Enrolment_Participant
        FOREIGN KEY (ParticipantID)
        REFERENCES dbo.Participant(ParticipantID),
    CONSTRAINT FK_Enrolment_Event
        FOREIGN KEY (EventID)
        REFERENCES dbo.Event(EventID),
    CONSTRAINT FK_Enrolment_Category
        FOREIGN KEY (CategoryID)
        REFERENCES dbo.Category(CategoryID),
    CONSTRAINT UQ_Enrolment_Participant_Event
        UNIQUE (ParticipantID, EventID),
    CONSTRAINT CK_Enrolment_Status
        CHECK (Status IN ('Pending', 'Confirmed', 'Cancelled'))
);
GO

-- Creating Result Table
CREATE TABLE dbo.Result
(
    ResultID INT IDENTITY(1,1) NOT NULL,
    EnrolmentID INT NOT NULL,
    FinishTime TIME NOT NULL,
    Position INT NOT NULL,
    CONSTRAINT PK_Result
        PRIMARY KEY (ResultID),
    CONSTRAINT UQ_Result_Enrolment
        UNIQUE (EnrolmentID),
    CONSTRAINT FK_Result_Enrolment
        FOREIGN KEY (EnrolmentID)
        REFERENCES dbo.Enrolment(EnrolmentID),
    CONSTRAINT CK_Result_Position
        CHECK (Position > 0)
);
GO

--Seed Data For User
INSERT INTO dbo.[User]
(
    FirstName,
    Surname,
    Email,
    PasswordHash,
    Role,
    ProfilePictureURL
)
VALUES
(
    'James',
    'Mokoena',
    'james.mokoena@raceday.co.za',
    CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'RaceDay@123'), 2),
    'Organiser',
    NULL
),
(
    'Lerato',
    'Naidoo',
    'lerato.naidoo@raceday.co.za',
    CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'RaceDay@123'), 2),
    'Organiser',
    NULL
),
(
    'Thabo',
    'Dlamini',
    'thabo.dlamini@example.com',
    CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'RaceDay@123'), 2),
    'Participant',
    NULL
),
(
    'Ayesha',
    'Pillay',
    'ayesha.pillay@example.com',
    CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'RaceDay@123'), 2),
    'Participant',
    NULL
);
GO

--Seed Data For Participant
INSERT INTO dbo.Participant
(
    UserID
)
VALUES
(
    3
),
(
    4
);
GO


-- Seed Data For Event
INSERT INTO dbo.Event
(
    OrganiserID,
    EventName,
    Description,
    EventType,
    Distance,
    Location,
    EventDate,
    BannerImageURL
)
VALUES
(
    1,
    'Johannesburg City Run',
    'A competitive road running event through central Johannesburg.',
    'Run',
    10.00,
    'Johannesburg, Gauteng',
    '2026-10-10',
    NULL
),
(
    1,
    'Soweto Community Walk',
    'A community-focused walking event promoting health and active living.',
    'Walk',
    5.00,
    'Soweto, Gauteng',
    '2026-11-07',
    NULL
),
(
    2,
    'Cradle Cycle Challenge',
    'A cycling event through the scenic Cradle of Humankind region.',
    'Cycle',
    21.00,
    'Cradle of Humankind, Gauteng',
    '2027-01-23',
    NULL
);
GO

-- Seed Category Data
INSERT INTO dbo.Category
(
    EventID,
    CategoryName
)
VALUES
-- Johannesburg City Run
(
    1,
    'Under 20'
),
(
    1,
    'Senior'
),
(
    1,
    'Veteran'
),

-- Soweto Community Walk
(
    2,
    'Under 20'
),
(
    2,
    'Senior'
),
(
    2,
    'Family'
),

-- Cradle Cycle Challenge
(
    3,
    'Junior'
),
(
    3,
    'Senior'
),
(
    3,
    'Veteran'
);
GO

-- Seed Data For Enrolment
INSERT INTO dbo.Enrolment
(
    ParticipantID,
    EventID,
    CategoryID,
    EnrolmentDate,
    Status
)
VALUES
(
    1,
    1,
    2,
    '2026-09-01 09:30:00',
    'Confirmed'
),
(
    2,
    1,
    2,
    '2026-09-02 10:15:00',
    'Confirmed'
),
(
    1,
    2,
    5,
    '2026-09-03 11:00:00',
    'Pending'
),
(
    2,
    3,
    8,
    '2026-09-03 14:20:00',
    'Confirmed'
);
GO

-- Verifying Database Content
SELECT * FROM dbo.[User];
SELECT * FROM dbo.Participant;
SELECT * FROM dbo.Event;
SELECT * FROM dbo.Category;
SELECT * FROM dbo.Enrolment;
SELECT * FROM dbo.Result;
GO