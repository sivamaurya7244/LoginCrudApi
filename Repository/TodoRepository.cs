
using System.Data;
using Dapper;
using LoginCrudApi.Models.Todo;
using Microsoft.Data.SqlClient;

namespace LoginCrudApi.Repository
{
    public sealed class TodoRepository : ITodoRepository
    {
        private readonly IConfiguration _configuration;

        public TodoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<TodoItem>> GetTodosAsync(int userId,string? search,string? category,string? priority,string? status)
        {
            using var connection = GetConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@Search", search);
            parameters.Add("@Category", category);
            parameters.Add("@Priority", priority);
            parameters.Add("@Status", status);
            return await connection.QueryAsync<TodoItem>("dbo.sp_Todo_GetAll",parameters,commandType: CommandType.StoredProcedure);
        }

        public async Task<TodoItem?> GetTodoByIdAsync(int userId, int id)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@Id", id);

            return await connection.QueryFirstOrDefaultAsync<TodoItem>("dbo.sp_Todo_GetById",parameters,commandType: CommandType.StoredProcedure);
        }

        

        

        public async Task<int> DeleteTodoAsync(int userId, int id)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@Id", id);

            return await connection.QuerySingleAsync<int>("dbo.sp_Todo_Delete",parameters,commandType: CommandType.StoredProcedure);
        }

        public async Task<TodoItem?> UpdateTodoStatusAsync(int userId, int id, string status)
        {
            using var con = GetConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@Id", id);
            parameters.Add("@Status", status);

            return await con.QueryFirstOrDefaultAsync<TodoItem>("dbo.sp_Todo_UpdateStatus",parameters,commandType: CommandType.StoredProcedure);
        }

        public async Task<TodoItem?> SaveTodoAsync(int userId, TodoSaveRequest model)
        {
            using var con = GetConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Id", model.Id);
            parameters.Add("@UserId", userId);
            parameters.Add("@Title", model.Title);
            parameters.Add("@Description", model.Description);
            parameters.Add("@Category", model.Category);
            parameters.Add("@Priority", model.Priority);

            return await con.QueryFirstOrDefaultAsync<TodoItem>("sp_Todo_Save",parameters,commandType: CommandType.StoredProcedure);
        }

        public async Task<TodoDashboardStats> GetDashboardStatsAsync(int userId)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            return await connection.QuerySingleAsync<TodoDashboardStats>("dbo.sp_Todo_GetDashboardStats",parameters,commandType: CommandType.StoredProcedure);
        }
    }
}
