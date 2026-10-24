# Todo API

The Todo module uses the existing JWT bearer authentication and common `ApiResponse` envelope. Send the token on every request:

```http
Authorization: Bearer <token>
Content-Type: application/json
```

The backend build targets .NET 8 and uses the existing Dapper / SQL Server stored procedure pattern. `LoginCrudDb` was checked: `dbo.UsersLogin.Id` is `INT`, and no Todo table or procedures existed. [Database/TodoManagement.sql](Database/TodoManagement.sql) creates only the new Todo table and Todo procedures.

Success envelope:

```json
{ "data": {}, "success": true, "httpStatus": 200 }
```

Errors use the same envelope with `success: false` and a message in `data`. Each Todo object contains `id`, `userId`, `title`, `description`, `category`, `priority`, `status`, and `createdAt`.

## Routes and examples

### List and filter: `GET /api/todo`

Optional query parameters can be combined: `search`, `category` (`Work|Personal`), `priority` (`High|Medium|Low`), `status` (`Pending|Completed`). Empty or missing search text is ignored. The list is restricted to the current user.

Example: `GET /api/todo?search=project&category=Work&priority=High&status=Pending`

```json
{ "data": [{ "id": 12, "userId": 4, "title": "Finish project documentation", "description": "Complete the technical documentation.", "category": "Work", "priority": "High", "status": "Pending", "createdAt": "2026-10-24T14:00:00" }], "success": true, "httpStatus": 200 }
```

### Get one: `GET /api/todo/{id}`

Example: `GET /api/todo/12`

```json
{ "data": { "id": 12, "userId": 4, "title": "Finish project documentation", "description": "Complete the technical documentation.", "category": "Work", "priority": "High", "status": "Pending", "createdAt": "2026-10-24T14:00:00" }, "success": true, "httpStatus": 200 }
```

Returns 404 if the todo does not exist or belongs to another user.

### Create: `POST /api/todo`

Request:

```json
{ "title": "Finish project documentation", "description": "Complete the technical documentation.", "category": "Work", "priority": "High" }
```

Response: HTTP 201. New todos start Pending.

```json
{ "data": { "id": 12, "userId": 4, "title": "Finish project documentation", "description": "Complete the technical documentation.", "category": "Work", "priority": "High", "status": "Pending", "createdAt": "2026-10-24T14:00:00" }, "success": true, "httpStatus": 201 }
```

### Update: `PUT /api/todo/{id}`

Request:

```json
{ "title": "Finish final documentation", "description": "Ready for review.", "category": "Work", "priority": "Medium" }
```

Response: HTTP 200 with the changed todo in `data`.

```json
{ "data": { "id": 12, "userId": 4, "title": "Finish final documentation", "description": "Ready for review.", "category": "Work", "priority": "Medium", "status": "Pending", "createdAt": "2026-10-24T14:00:00" }, "success": true, "httpStatus": 200 }
```

Returns 404 for a missing todo or one owned by another user.

### Change checkbox status: `PATCH /api/todo/{id}/status`

Request:

```json
{ "status": "Completed" }
```

Response:

```json
{ "data": { "id": 12, "userId": 4, "title": "Finish final documentation", "description": "Ready for review.", "category": "Work", "priority": "Medium", "status": "Completed", "createdAt": "2026-10-24T14:00:00" }, "success": true, "httpStatus": 200 }
```

### Delete: `DELETE /api/todo/{id}`

Response:

```json
{ "data": "Todo deleted.", "success": true, "httpStatus": 200 }
```

Returns 404 when the item is unavailable to the current user; success is reported only when a row was deleted.

### Dashboard: `GET /api/todo/dashboard-stats`

This counts the user's full collection, regardless of list filters. High Priority means Pending todos with High priority.

```json
{ "data": { "totalTodos": 12, "completed": 6, "pending": 6, "highPriority": 3 }, "success": true, "httpStatus": 200 }
```

## Existing UI integration

Keep the TodoPro page and its components. Use `GET /api/todo` for the table and search/category/priority/status filters; `GET /api/todo/dashboard-stats` for the four cards; `POST` for New Todo; `PUT` for edit; `PATCH /{id}/status` for the checkbox; and `DELETE` for the trash action.

The API allows browser origins `http://localhost:5173` and `http://localhost:4200`. Example:

```ts
const response = await fetch("http://localhost:5163/api/todo?category=Work", {
  headers: { Authorization: `Bearer ${token}` }
});
const result = await response.json();
const todos = result.data;
```

The screenshot's TodoPro source was not present in the available project folders, so I left its frontend components untouched. Sign in again to get a token with the new user ID claim.
