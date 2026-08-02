using Dapper;
using LoginCrudApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LoginCrudApi.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly IConfiguration _config;

        public HomeRepository(IConfiguration config)
        {
            _config = config;
        }

        private IDbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        public async Task<int> RegisterAsync(LoginModel model)
        {
            using var con = GetConnection();

            return await con.ExecuteAsync(
                "sp_Register",
                new
                {
                    model.Username,
                    model.Password,
                    model.FaceDescriptor
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<LoginModel?> LoginAsync(LoginModel model)
        {
            using var con = GetConnection();

            return await con.QueryFirstOrDefaultAsync<LoginModel>(
                "sp_Login",
                new
                {
                    model.Username,
                    model.Password
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var con = GetConnection();

            return await con.QueryAsync<User>(
                "sp_GetAllUsers",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<User>> GetAllAsync(UserInputModel inputModel)
        {
            using var con = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@PageNumber", inputModel.pageNumber, DbType.Int32);
            parameters.Add("@PageSize", inputModel.pageSize, DbType.Int32);

            return await con.QueryAsync<User>(
                "sp_GetAllUsersWithpagination",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var con = GetConnection();

            return await con.QueryFirstOrDefaultAsync<User>(
                "sp_GetUserById",
                new
                {
                    Id = id
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> CreateAsync(User user)
        {
            using var con = GetConnection();

            return await con.ExecuteAsync(
                "sp_CreateUser",
                new
                {
                    user.Name,
                    user.Email,
                    user.Salary
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(User user)
        {
            using var con = GetConnection();

            return await con.ExecuteAsync(
                "sp_UpdateUser",
                new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Salary
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var con = GetConnection();

            return await con.ExecuteAsync(
                "sp_DeleteUser",
                new
                {
                    Id = id
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}