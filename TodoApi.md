USE [LoginCrudDb]
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateUser]    Script Date: 24-10-2026 21:50:02 ******/
DROP PROCEDURE [dbo].[sp_UpdateUser]
GO
/****** Object:  StoredProcedure [dbo].[sp_Todo_UpdateStatus]    Script Date: 24-10-2026 21:50:02 ******/
DROP PROCEDURE [dbo].[sp_Todo_UpdateStatus]
GO
/****** Object:  StoredProcedure [dbo].[sp_Todo_Save]    Script Date: 24-10-2026 21:50:02 ******/
DROP PROCEDURE [dbo].[sp_Todo_Save]
GO
/****** Object:  StoredProcedure [dbo].[sp_Todo_GetDashboardStats]    Script Date: 24-10-2026 21:50:02 ******/
DROP PROCEDURE [dbo].[sp_Todo_GetDashboardStats]
GO
/****** Object:  StoredProcedure [dbo].[sp_Todo_GetById]    Script Date: 24-10-2026 21:50:02 ******/
DROP PROCEDURE [dbo].[sp_Todo_GetById]
GO
/****** Object:  StoredProcedure [dbo].[sp_Todo_GetAll]    Script Date: 24-10-2026 21:50:02 ******/
DROP PROCEDURE [dbo].[sp_Todo_GetAll]
GO
/****** Object:  StoredProcedure [dbo].[sp_Todo_Delete]    Script Date: 24-10-2026 21:50:02 ******/
DROP PROCEDURE [dbo].[sp_Todo_Delete]
GO
/****** Object:  StoredProcedure [dbo].[sp_Register]    Script Date: 24-10-2026 21:50:02 ******/
DROP PROCEDURE [dbo].[sp_Register]
GO
/****** Object:  StoredProcedure [dbo].[sp_Login]    Script Date: 24-10-2026 21:50:02 ******/
DROP PROCEDURE [dbo].[sp_Login]
GO
ALTER TABLE [dbo].[Todos] DROP CONSTRAINT [CK_Todos_Status]
GO
ALTER TABLE [dbo].[Todos] DROP CONSTRAINT [CK_Todos_Priority]
GO
ALTER TABLE [dbo].[Todos] DROP CONSTRAINT [CK_Todos_Category]
GO
ALTER TABLE [dbo].[Todos] DROP CONSTRAINT [FK_Todos_UsersLogin]
GO
ALTER TABLE [dbo].[Todos] DROP CONSTRAINT [DF_Todos_CreatedAt]
GO
ALTER TABLE [dbo].[Todos] DROP CONSTRAINT [DF_Todos_Status]
GO
/****** Object:  Table [dbo].[UsersLogin]    Script Date: 24-10-2026 21:50:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UsersLogin]') AND type in (N'U'))
DROP TABLE [dbo].[UsersLogin]
GO
/****** Object:  Table [dbo].[Todos]    Script Date: 24-10-2026 21:50:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Todos]') AND type in (N'U'))
DROP TABLE [dbo].[Todos]
GO
/****** Object:  Table [dbo].[Todos]    Script Date: 24-10-2026 21:50:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Todos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[Category] [nvarchar](20) NOT NULL,
	[Priority] [nvarchar](10) NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[CreatedAt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_Todos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersLogin]    Script Date: 24-10-2026 21:50:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersLogin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NULL,
	[Password] [varchar](100) NULL,
	[FaceDescriptor] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Todos] ADD  CONSTRAINT [DF_Todos_Status]  DEFAULT (N'Pending') FOR [Status]
GO
ALTER TABLE [dbo].[Todos] ADD  CONSTRAINT [DF_Todos_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Todos]  WITH CHECK ADD  CONSTRAINT [FK_Todos_UsersLogin] FOREIGN KEY([UserId])
REFERENCES [dbo].[UsersLogin] ([Id])
GO
ALTER TABLE [dbo].[Todos] CHECK CONSTRAINT [FK_Todos_UsersLogin]
GO
ALTER TABLE [dbo].[Todos]  WITH CHECK ADD  CONSTRAINT [CK_Todos_Category] CHECK  (([Category]=N'Personal' OR [Category]=N'Work'))
GO
ALTER TABLE [dbo].[Todos] CHECK CONSTRAINT [CK_Todos_Category]
GO
ALTER TABLE [dbo].[Todos]  WITH CHECK ADD  CONSTRAINT [CK_Todos_Priority] CHECK  (([Priority]=N'Low' OR [Priority]=N'Medium' OR [Priority]=N'High'))
GO
ALTER TABLE [dbo].[Todos] CHECK CONSTRAINT [CK_Todos_Priority]
GO
ALTER TABLE [dbo].[Todos]  WITH CHECK ADD  CONSTRAINT [CK_Todos_Status] CHECK  (([Status]=N'Completed' OR [Status]=N'Pending'))
GO
ALTER TABLE [dbo].[Todos] CHECK CONSTRAINT [CK_Todos_Status]
GO
/****** Object:  StoredProcedure [dbo].[sp_Login]    Script Date: 24-10-2026 21:50:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ================= LOGIN ================= */
Create Proc [dbo].[sp_Login]
    @Username Varchar(50),
    @Password Varchar(100)
As
Begin
    Select * 
    From UsersLogin
    Where Username=@Username And Password=@Password
End
GO
/****** Object:  StoredProcedure [dbo].[sp_Register]    Script Date: 24-10-2026 21:50:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
  
/* ================= REGISTER ================= */  
CREATE Proc [dbo].[sp_Register]  
    @Username Varchar(50),  
    @Password Varchar(100)
	--@FaceDescriptor VARCHAR(MAX) = NULL
As  
Begin  
    Insert Into UsersLogin(Username,Password)  
    Values(@Username,@Password)  
End  
GO
/****** Object:  StoredProcedure [dbo].[sp_Todo_Delete]    Script Date: 24-10-2026 21:50:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Todo_Delete]
    @UserId INT,
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE dbo.Todos WHERE Id = @Id AND UserId = @UserId;
    SELECT CONVERT(INT, @@ROWCOUNT);
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Todo_GetAll]    Script Date: 24-10-2026 21:50:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Todo_GetAll]
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
/****** Object:  StoredProcedure [dbo].[sp_Todo_GetById]    Script Date: 24-10-2026 21:50:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Todo_GetById]
    @UserId INT,
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, UserId, Title, Description, Category, Priority, Status, CreatedAt
    FROM dbo.Todos WHERE Id = @Id AND UserId = @UserId;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_Todo_GetDashboardStats]    Script Date: 24-10-2026 21:50:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Todo_GetDashboardStats]
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
/****** Object:  StoredProcedure [dbo].[sp_Todo_Save]    Script Date: 24-10-2026 21:50:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE   PROCEDURE [dbo].[sp_Todo_Save]
    @Id INT = 0,
    @UserId INT,
    @Title NVARCHAR(200),
    @Description NVARCHAR(2000) = NULL,
    @Category NVARCHAR(20),
    @Priority NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    IF ISNULL(@Id, 0) <= 0
    BEGIN
        INSERT INTO dbo.Todos
            (UserId, Title, Description, Category, Priority)
        VALUES
            (@UserId, @Title, @Description, @Category, @Priority);

        SET @Id = CONVERT(INT, SCOPE_IDENTITY());
    END
    ELSE
    BEGIN
        UPDATE dbo.Todos
        SET Title = @Title,
            Description = @Description,
            Category = @Category,
            Priority = @Priority
        WHERE Id = @Id AND UserId = @UserId;

        IF @@ROWCOUNT = 0
            RETURN;
    END;

    SELECT Id, UserId, Title, Description, Category,
           Priority, Status, CreatedAt
    FROM dbo.Todos
    WHERE Id = @Id AND UserId = @UserId;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Todo_UpdateStatus]    Script Date: 24-10-2026 21:50:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Todo_UpdateStatus]
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
/****** Object:  StoredProcedure [dbo].[sp_UpdateUser]    Script Date: 24-10-2026 21:50:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ================= UPDATE USER ================= */
Create Proc [dbo].[sp_UpdateUser]
    @Id Int,
    @Name Varchar(100),
    @Email Varchar(100),
    @Salary Decimal(10,2)
As
Begin
    Update Users
    Set Name=@Name,
        Email=@Email,
        Salary=@Salary
    Where Id=@Id
End
GO
