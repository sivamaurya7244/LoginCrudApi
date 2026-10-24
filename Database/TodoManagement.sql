-- TodoPro module for LoginCrudDb.
-- Run after the existing UsersLogin table is created. UsersLogin.Id is INT IDENTITY.

IF OBJECT_ID(N'dbo.Todos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Todos
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Todos PRIMARY KEY,
        UserId INT NOT NULL,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(2000) NULL,
        Category NVARCHAR(20) NOT NULL,
        Priority NVARCHAR(10) NOT NULL,
        Status NVARCHAR(20) NOT NULL CONSTRAINT DF_Todos_Status DEFAULT N'Pending',
        CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_Todos_CreatedAt DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_Todos_UsersLogin FOREIGN KEY (UserId) REFERENCES dbo.UsersLogin(Id),
        CONSTRAINT CK_Todos_Category CHECK (Category IN (N'Work', N'Personal')),
        CONSTRAINT CK_Todos_Priority CHECK (Priority IN (N'High', N'Medium', N'Low')),
        CONSTRAINT CK_Todos_Status CHECK (Status IN (N'Pending', N'Completed'))
    );
    CREATE INDEX IX_Todos_UserId_Filters ON dbo.Todos(UserId, Category, Priority, Status, CreatedAt DESC);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Todo_GetAll
    @UserId INT,
    @Search NVARCHAR(2000) = NULL,
    @Category NVARCHAR(20) = NULL,
    @Priority NVARCHAR(10) = NULL,
    @Status NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, UserId, Title, Description, Category, Priority, Status, CreatedAt
    FROM dbo.Todos
    WHERE UserId = @UserId
      AND (NULLIF(LTRIM(RTRIM(@Search)), N'') IS NULL
           OR Title LIKE N'%' + @Search + N'%'
           OR Description LIKE N'%' + @Search + N'%')
      AND (@Category IS NULL OR Category = @Category)
      AND (@Priority IS NULL OR Priority = @Priority)
      AND (@Status IS NULL OR Status = @Status)
    ORDER BY CreatedAt DESC, Id DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Todo_GetById
    @UserId INT,
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, UserId, Title, Description, Category, Priority, Status, CreatedAt
    FROM dbo.Todos WHERE Id = @Id AND UserId = @UserId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Todo_Create
    @UserId INT,
    @Title NVARCHAR(200),
    @Description NVARCHAR(2000) = NULL,
    @Category NVARCHAR(20),
    @Priority NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.Todos (UserId, Title, Description, Category, Priority)
    VALUES (@UserId, @Title, @Description, @Category, @Priority);

    SELECT Id, UserId, Title, Description, Category, Priority, Status, CreatedAt
    FROM dbo.Todos WHERE Id = CONVERT(INT, SCOPE_IDENTITY());
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Todo_Update
    @UserId INT,
    @Id INT,
    @Title NVARCHAR(200),
    @Description NVARCHAR(2000) = NULL,
    @Category NVARCHAR(20),
    @Priority NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Todos
    SET Title = @Title, Description = @Description, Category = @Category, Priority = @Priority
    WHERE Id = @Id AND UserId = @UserId;

    IF @@ROWCOUNT = 1
        SELECT Id, UserId, Title, Description, Category, Priority, Status, CreatedAt
        FROM dbo.Todos WHERE Id = @Id AND UserId = @UserId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Todo_Delete
    @UserId INT,
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE dbo.Todos WHERE Id = @Id AND UserId = @UserId;
    SELECT CONVERT(INT, @@ROWCOUNT);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Todo_UpdateStatus
    @UserId INT,
    @Id INT,
    @Status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Todos SET Status = @Status WHERE Id = @Id AND UserId = @UserId;

    IF @@ROWCOUNT = 1
        SELECT Id, UserId, Title, Description, Category, Priority, Status, CreatedAt
        FROM dbo.Todos WHERE Id = @Id AND UserId = @UserId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Todo_GetDashboardStats
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        COUNT(1) AS TotalTodos,
        COALESCE(SUM(CASE WHEN Status = N'Completed' THEN 1 ELSE 0 END), 0) AS Completed,
        COALESCE(SUM(CASE WHEN Status = N'Pending' THEN 1 ELSE 0 END), 0) AS Pending,
        COALESCE(SUM(CASE WHEN Status = N'Pending' AND Priority = N'High' THEN 1 ELSE 0 END), 0) AS HighPriority
    FROM dbo.Todos
    WHERE UserId = @UserId;
END;
GO
