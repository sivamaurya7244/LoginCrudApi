using LoginCrudApi.Models;
using LoginCrudApi.Models.Todo;
using LoginCrudApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginCrudApi.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repository;

        public TodoController(ITodoRepository repository)
        {
            _repository = repository;
        }

        private const int userId = 2;

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll(string? search,string? category,string? priority,string? status)
        {
            var result = await _repository.GetTodosAsync(userId, search, category, priority, status);

            return Ok(new ApiResponse
            {
                Data = result,
                Success = true,
                HttpStatus = 200
            });
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repository.GetTodoByIdAsync(userId, id);

            if (result == null)
            {
                return NotFound(new ApiResponse
                {
                    Data = "Todo not found",
                    Success = false,
                    HttpStatus = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = result,
                Success = true,
                HttpStatus = 200
            });
        }

        [HttpPost("create-or-update")]
        public async Task<IActionResult> CreateOrUpdate(TodoSaveRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                return BadRequest(new ApiResponse
                {
                    Data = "Title is required",
                    Success = false,
                    HttpStatus = 400
                });
            }

            var result = await _repository.SaveTodoAsync(userId, model);

            if (result == null)
            {
                return NotFound(new ApiResponse
                {
                    Data = "Todo not found",
                    Success = false,
                    HttpStatus = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = result,
                Success = true,
                HttpStatus = 200
            });
        }

        [HttpPatch("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateTodoStatusRequest model)
        {
            if (model.Status != "Pending" && model.Status != "Completed")
            {
                return BadRequest(new ApiResponse
                {
                    Data = "Invalid status",
                    Success = false,
                    HttpStatus = 400
                });
            }

            var result = await _repository.UpdateTodoStatusAsync(userId, id, model.Status);

            if (result == null)
            {
                return NotFound(new ApiResponse
                {
                    Data = "Todo not found",
                    Success = false,
                    HttpStatus = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = result,
                Success = true,
                HttpStatus = 200
            });
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteTodoAsync(userId, id);

            if (result == 0)
            {
                return NotFound(new ApiResponse
                {
                    Data = "Todo not found",
                    Success = false,
                    HttpStatus = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = "Todo deleted successfully",
                Success = true,
                HttpStatus = 200
            });
        }

        [HttpGet("dashboard-stats")]
        public async Task<IActionResult> DashboardStats()
        {
            var result = await _repository.GetDashboardStatsAsync(userId);

            return Ok(new ApiResponse
            {
                Data = result,
                Success = true,
                HttpStatus = 200
            });
        }
    }
}