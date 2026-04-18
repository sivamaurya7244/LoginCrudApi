using Dapper;
using LoginCrudApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        [HttpPost("register")]
        public IActionResult Register(LoginModel model)
        {
            using var con = GetConnection();

            con.Execute("sp_Register",new{ model.Username,model.Password },commandType: CommandType.StoredProcedure);

            return Ok("Registered Successfully");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            using var con = GetConnection();

            var user = con.QueryFirstOrDefault("sp_Login",new{model.Username,model.Password},commandType: CommandType.StoredProcedure);

            if (user == null)
                return Unauthorized("Invalid Username or Password");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.Username)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [Authorize]
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            using var con = GetConnection();

            var data = con.Query<User>("sp_GetAllUsers",commandType: CommandType.StoredProcedure);

            return Ok(data);
        }

        [Authorize]
        [HttpGet("getbyid/{id}")]
        public IActionResult GetById(int id)
        {
            using var con = GetConnection();

            var data = con.QueryFirstOrDefault<User>("sp_GetUserById",new { Id = id },commandType: CommandType.StoredProcedure);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult Create(User user)
        {
            using var con = GetConnection();

            con.Execute("sp_CreateUser",
                new
                {
                    user.Name,
                    user.Email,
                    user.Salary
                },
                commandType: CommandType.StoredProcedure
            );

            return Ok("Inserted Successfully");
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult Update(User user)
        {
            using var con = GetConnection();

            con.Execute("sp_UpdateUser",
                new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Salary
                },
                commandType: CommandType.StoredProcedure
            );

            return Ok("Updated Successfully");
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            using var con = GetConnection();

            con.Execute("sp_DeleteUser",new { Id = id },commandType: CommandType.StoredProcedure);
            return Ok("Deleted Successfully");
        }
    }

    

   
}