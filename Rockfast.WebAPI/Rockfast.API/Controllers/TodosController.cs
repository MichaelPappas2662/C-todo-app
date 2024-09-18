using Microsoft.AspNetCore.Mvc;
using Rockfast.ServiceInterfaces;
using Rockfast.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rockfast.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ILogger<TodosController> _logger;

        public TodosController(ITodoService todoService, ILogger<TodosController> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoVM>>> Get([FromQuery] int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("User ID is required and must be greater than zero.");
                }

                var todos = await _todoService.GetTodosAsync(userId);
                return Ok(todos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting todos.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TodoVM>> Post([FromBody] TodoVM model)
        {
            try
            {
                if (model == null)
                {
                    _logger.LogWarning("Received null TodoVM.");
                    return BadRequest("Todo data is null.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdTodo = await _todoService.CreateTodoAsync(model);
                if (createdTodo == null)
                {
                    return NotFound($"User with ID {model.UserId} not found.");
                }

                return CreatedAtAction(nameof(Get), new { id = createdTodo.Id }, createdTodo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a todo.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoVM>> Put(int id, [FromBody] TodoVM model)
        {
            try
            {
                if (model == null || id != model.Id)
                {
                    _logger.LogWarning("Invalid TodoVM data.");
                    return BadRequest("Invalid data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedTodo = await _todoService.UpdateTodoAsync(model);
                if (updatedTodo == null)
                {
                    return NotFound($"Todo with ID {id} not found or user mismatch.");
                }

                return Ok(updatedTodo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating todo with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromQuery] int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("User ID is required and must be greater than zero.");
                }

                await _todoService.DeleteTodoAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting todo with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
