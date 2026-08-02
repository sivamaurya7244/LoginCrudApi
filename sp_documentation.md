# Stored Procedures Summary

| Stored Procedure Name | Purpose | Required Parameters |
| :--- | :--- | :--- |
| **`sp_Register`** | Registers a new user. | `@Username`, `@Password` |
| **`sp_Login`** | Validates login credentials. | `@Username`, `@Password` |
| **`sp_CreateUser`** | Inserts a new user record. | `@Name`, `@Email`, `@Salary` |
| **`sp_GetAllUsers`** | Fetches all users. | *None* |
| **`sp_GetUserById`** | Fetches a single user's details. | `@Id` |
| **`sp_UpdateUser`** | Updates user details. | `@Id`, `@Name`, `@Email`, `@Salary` |
| **`sp_DeleteUser`** | Deletes a user. | `@Id` |
| **`sp_GetAllUsersWithpagination`** | Fetches users page by page. | `@PageNumber`, `@PageSize` |
