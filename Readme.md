# LoginCrudApi - ASP.NET Core Web API

A simple, secure, and clean ASP.NET Core Web API project that demonstrates User Authentication using JWT (JSON Web Tokens) and full CRUD (Create, Read, Update, Delete) operations with pagination, using **Dapper ORM** and **SQL Server Stored Procedures**.

---

## 🛠️ Tech Stack

* **Framework:** [ASP.NET Core Web API (.NET 8.0)](https://learn.microsoft.com/en-us/aspnet/core/)
* **Micro-ORM:** [Dapper (v2.1.72)](https://github.com/DapperLib/Dapper)
* **Database Driver:** [Microsoft.Data.SqlClient (v7.0.0)](https://learn.microsoft.com/en-us/sql/connect/ado-net/introduction-microsoft-data-sqlclient-namespace)
* **Database:** SQL Server
* **Authentication:** [JWT Bearer Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/limitingidentitybyscheme)
* **API Documentation:** [Swagger / OpenAPI (Swashbuckle.AspNetCore v6.6.2)](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

---

## 📁 Project Structure

* **[Controllers/](file:///d:/WebHooks/LoginCrudApi/Controllers/)**: Contains the `HomeController` which defines all API routes.
* **[Repository/](file:///d:/WebHooks/LoginCrudApi/Repository/)**: Contains database interaction logic implemented with Dapper.
* **[Models/](file:///d:/WebHooks/LoginCrudApi/Models/)**: Defines data models for User and Login models.
* **[Program.cs](file:///d:/WebHooks/LoginCrudApi/Program.cs)**: Main entry point where services (JWT, DI, Swagger) are registered and middleware configured.
* **[appsettings.json](file:///d:/WebHooks/LoginCrudApi/appsettings.json)**: Application configuration containing database ConnectionString and JWT configuration.

---

## 🚀 Getting Started

### 1. Database Setup
Ensure you have SQL Server running. Run the following SQL Script to create the database, tables, and required Stored Procedures.

```sql
CREATE DATABASE Siva_InterView;
GO

USE [Siva_InterView];
GO

-- 1. Create Tables
CREATE TABLE UsersLogin (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL
);

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Salary DECIMAL(10,2) NOT NULL
);
GO

-- 2. Stored Procedures

/* ================= LOGIN ================= */
CREATE Proc [dbo].[sp_Login]
    @Username Varchar(50),
    @Password Varchar(100)
As
Begin
    Select * 
    From UsersLogin
    Where Username=@Username And Password=@Password
End
GO

/* ================= REGISTER ================= */
CREATE Proc [dbo].[sp_Register]
    @Username Varchar(50),
    @Password Varchar(100)
As
Begin
    Insert Into UsersLogin(Username,Password)
    Values(@Username,@Password)
End
GO

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
GO

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
GO

/* ================= DELETE USER ================= */
CREATE Proc [dbo].[sp_DeleteUser]
    @Id Int
As
Begin
    Delete From Users
    Where Id=@Id
End
GO

/* ================= GET ALL USERS ================= */
CREATE Proc [dbo].[sp_GetAllUsers]
As
Begin
    Select * From Users
End
GO

/* ================= GET USER BY ID ================= */
CREATE Proc [dbo].[sp_GetUserById]
    @Id Int
As
Begin
    Select * From Users
    Where Id=@Id
End
GO

/* ================= GET USERS WITH PAGINATION ================= */
CREATE PROC sp_GetAllUsersWithpagination
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SELECT *
    FROM Users
    ORDER BY Id
    OFFSET (@PageNumber-1)*@PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
```

### 2. Configure the Connection String
Open **[appsettings.json](file:///d:/WebHooks/LoginCrudApi/appsettings.json)** and update your connection string under `ConnectionStrings.DefaultConnection`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=Siva_InterView;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Run the Application
Run the project via Visual Studio or using dotnet CLI:
```bash
dotnet restore
dotnet run
```
Once started, Swagger documentation will be available at `http://localhost:<port>/swagger` (under Development mode).

---

## 📡 API Reference & Documentation

*All User endpoints (except Register and Login) require a Bearer token in the `Authorization` header:*
`Authorization: Bearer <Your_JWT_Token>`

### 1. Authentication Endpoints

#### Register a new Account
* **URL:** `/api/home/register`
* **Method:** `POST`
* **Request Body (JSON):**
  ```json
  {
    "username": "john_doe",
    "password": "Password123"
  }
  ```
* **Responses:**
  * `200 OK`: `"Registered Successfully"`
  * `400 Bad Request`: `"Registration Failed"`

#### User Login (Generate Token)
* **URL:** `/api/home/login`
* **Method:** `POST`
* **Request Body (JSON):**
  ```json
  {
    "username": "john_doe",
    "password": "Password123"
  }
  ```
* **Responses:**
  * `200 OK`: Returns the token.
    ```json
    {
      "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    }
    ```
  * `411 Unauthorized`: `"Invalid Username or Password"`

---

### 2. User CRUD Endpoints (Protected by JWT Authorization)

#### Get All Users
* **URL:** `/api/home/getall`
* **Method:** `GET`
* **Headers:** `Authorization: Bearer <token>`
* **Responses:**
  * `200 OK`: Returns array of user records.
    ```json
    [
      {
        "id": 1,
        "name": "Alice Johnson",
        "email": "alice@example.com",
        "salary": 75000.00
      }
    ]
    ```

#### Get Users with Pagination
* **URL:** `/api/home/getall-byPagination`
* **Method:** `GET`
* **Headers:** `Authorization: Bearer <token>`
* **Query Parameters:**
  * `pageNumber` (int) - Default: `1`
  * `pageSize` (int) - Default: `10`
* **Responses:**
  * `200 OK`: Returns matching page of user records.

#### Get User by ID
* **URL:** `/api/home/getbyid/{id}`
* **Method:** `GET`
* **Headers:** `Authorization: Bearer <token>`
* **Route Parameter:** `id` (int)
* **Responses:**
  * `200 OK`: Returns user details.
  * `404 Not Found`: `"User Not Found"`

#### Create a User
* **URL:** `/api/home/create`
* **Method:** `POST`
* **Headers:** `Authorization: Bearer <token>`
* **Request Body (JSON):**
  ```json
  {
    "name": "Bob Smith",
    "email": "bob.smith@example.com",
    "salary": 65000.00
  }
  ```
* **Responses:**
  * `200 OK`: `"User Created Successfully"`
  * `400 Bad Request`: `"Insert Failed"`

#### Update a User
* **URL:** `/api/home/update`
* **Method:** `PUT`
* **Headers:** `Authorization: Bearer <token>`
* **Request Body (JSON):**
  ```json
  {
    "id": 1,
    "name": "Bob Updated",
    "email": "bob.updated@example.com",
    "salary": 70000.00
  }
  ```
* **Responses:**
  * `200 OK`: `"Updated Successfully"`
  * `400 Bad Request`: `"Update Failed"`

#### Delete a User
* **URL:** `/api/home/delete/{id}`
* **Method:** `DELETE`
* **Headers:** `Authorization: Bearer <token>`
* **Route Parameter:** `id` (int)
* **Responses:**
  * `200 OK`: `"Deleted Successfully"`
  * `400 Bad Request`: `"Delete Failed"`