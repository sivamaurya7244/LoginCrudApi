using LoginCrudApi.Models;
using LoginCrudApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IloginRepository _repository;
        private readonly IConfiguration _config;

        public LoginController(IloginRepository repository,IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginModel model)
        {
            var result = await _repository.RegisterAsync(model);

            ApiResponse obj = new ApiResponse();

            if (result > 0)
            {
                obj.HttpStatus = StatusCodes.Status200OK;
                obj.Data = "Registered Successfully";
                obj.Success = true;

                return Ok(obj);
            }

            obj.HttpStatus = StatusCodes.Status400BadRequest;
            obj.Data = "Registration Failed";
            obj.Success = false;

            return BadRequest(obj);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _repository.LoginAsync(model);

            if (user == null)
                return Unauthorized("Invalid Username or Password");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

       
    }
}

