using LoginCrudApi.Models;
using LoginCrudApi.Repository;
using LoginCrudApi.SingalRHub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeRepository _repository;
        private readonly IConfiguration _config;
        private readonly IHubContext<DashboardHub> _hubContext;

        public HomeController(IHomeRepository repository,
                              IConfiguration config,
                              IHubContext<DashboardHub>  hubContext
        )
        {
            _repository = repository;
            _config = config;
            _hubContext = hubContext;
        }
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
                new Claim(ClaimTypes.Name, model.Username)
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

       // [Authorize]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repository.GetAllAsync();

            return Ok(users);
        }

        //[Authorize]
        [HttpGet("getall-byPagination")]
        public async Task<IActionResult> GetAll(UserInputModel inputModel)
        {
            var users = await _repository.GetAllAsync(inputModel);

            return Ok(users);
        }

        [Authorize]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                return NotFound("User Not Found");

            return Ok(user);
        }

        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(User user)
        {
            var result = await _repository.CreateAsync(user);

            if (result > 0)
            {
                await _hubContext.Clients.All.SendAsync("DashboardUpdated");
                return Ok("User Created Successfully");
            }

            return BadRequest("Insert Failed");
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update(User user)
        {
            var result = await _repository.UpdateAsync(user);

            if (result > 0)
            {
                await _hubContext.Clients.All.SendAsync("DashboardUpdated");
                return Ok("Updated Successfully");
            }

            return BadRequest("Update Failed");
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);

            if (result > 0)
            {
                await _hubContext.Clients.All.SendAsync("DashboardUpdated");
                return Ok("Deleted Successfully");
            }

            return BadRequest("Delete Failed");
        }
    }
}