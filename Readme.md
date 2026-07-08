CREATE DATABASE Siva_InterView

USE [Siva_InterView]
GO
CREATE Proc [dbo].[sp_Login]
    @Username Varchar(50),
    @Password Varchar(100)
As
Begin
    Select * 
    From UsersLogin
    Where Username=@Username And Password=@Password
End

/* ================= REGISTER ================= */
CREATE Proc [dbo].[sp_Register]
    @Username Varchar(50),
    @Password Varchar(100)
As
Begin
    Insert Into UsersLogin(Username,Password)
    Values(@Username,@Password)
End

/* ================= CREATE USER ================= */
CREATE Proc [dbo].[sp_CreateUser]
    @Name Varchar(100),
    @Email Varchar(100),
    @Salary Decimal(10,2)
As
Begin
    Insert Into Users(Name,Email,Salary)
    Values(@Name,@Email,@Salary)
End

/* ================= UPDATE USER ================= */
CREATE Proc [dbo].[sp_UpdateUser]
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

CREATE Proc [dbo].[sp_DeleteUser]
    @Id Int
As
Begin
    Delete From Users
    Where Id=@Id
End

CREATE Proc [dbo].[sp_GetAllUsers]
As
Begin
    Select * From Users
End


/* ================= GET USER BY ID ================= */
CREATE Proc [dbo].[sp_GetUserById]
    @Id Int
As
Begin
    Select * From Users
    Where Id=@Id
End

create PROC sp_GetAllUsersWithpagination
    @PageNumber INT,
    @PageSize INT
AS
BEGIN

    --SELECT COUNT(*) AS TotalRecords
    --FROM Users;

    SELECT *
    FROM Users
    ORDER BY Id
    OFFSET (@PageNumber-1)*@PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

END