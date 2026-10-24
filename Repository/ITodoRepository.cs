using LoginCrudApi.Models.Todo;

namespace LoginCrudApi.Repository
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetTodosAsync(int userId, string? search, string? category, string? priority, string? status);

        Task<TodoItem?> GetTodoByIdAsync(int userId, int id);        
        Task<int> DeleteTodoAsync(int userId, int id);
        Task<TodoItem?> UpdateTodoStatusAsync(int userId, int id, string status);
        Task<TodoDashboardStats> GetDashboardStatsAsync(int userId);
        Task<TodoItem?> SaveTodoAsync(int userId, TodoSaveRequest model);
    }
}