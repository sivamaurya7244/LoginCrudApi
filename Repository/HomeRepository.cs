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

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Username", model.Username);
            parameters.Add("@Password", model.Password);
            var res = await con.ExecuteAsync("sp_Register", parameters, commandType: CommandType.StoredProcedure);
            return res;
        }

        public async Task<LoginModel?> LoginAsync(LoginModel model)
        {
            using var con = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Username", model.Username);
            parameters.Add("@Password", model.Password);

            var res = await con.QueryFirstOrDefaultAsync<LoginModel>("sp_Login", parameters, commandType: CommandType.StoredProcedure);
             return res;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var con = GetConnection();
            return await con.QueryAsync<User>("sp_GetAllUsers",commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<User>> GetAllAsync(UserInputModel inputModel)
        {
            using var con = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@PageNumber", inputModel.pageNumber, DbType.Int32);
            parameters.Add("@PageSize", inputModel.pageSize, DbType.Int32);
            return await con.QueryAsync<User>("sp_GetAllUsersWithpagination",parameters,commandType: CommandType.StoredProcedure);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var con = GetConnection();

            return await con.QueryFirstOrDefaultAsync<User>("sp_GetUserById",
                new{Id = id},commandType: CommandType.StoredProcedure);
        }

        public async Task<int> CreateAsync(User user)
        {
            using var con = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Name", user.Name);
            parameters.Add("@Email", user.Email);
            parameters.Add("@Salary", user.Salary);
            var data = await con.ExecuteAsync("sp_CreateUser",parameters,commandType: CommandType.StoredProcedure);
            return data;
        }

        public async Task<int> UpdateAsync(User user)
        {
            using var con = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", user.Id);
            parameters.Add("@Name", user.Name);
            parameters.Add("@Email", user.Email);
            parameters.Add("@Salary", user.Salary);


            var data = await con.ExecuteAsync("sp_UpdateUser",parameters,commandType: CommandType.StoredProcedure);
            return data;
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var con = GetConnection();

            var data = await con.ExecuteAsync("sp_DeleteUser",new{Id = id},commandType: CommandType.StoredProcedure);
            return data;
        }
    }
}