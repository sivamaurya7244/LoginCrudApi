using Dapper;
using LoginCrudApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LoginCrudApi.Repository
{
    public class LoginRepository : IloginRepository
    {
        private readonly IConfiguration _config;

        public LoginRepository(IConfiguration config)
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

        
    }
}