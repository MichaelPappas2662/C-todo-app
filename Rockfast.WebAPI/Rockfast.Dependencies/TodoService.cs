using Rockfast.ApiDatabase;
using Rockfast.ApiDatabase.DomainModels;
using Rockfast.ServiceInterfaces;
using Rockfast.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rockfast.Dependencies
{
    public class TodoService : ITodoService
    {
        private readonly ApiDbContext _database;
        private readonly ILogger<TodoService> _logger;

        public TodoService(ApiDbContext db, ILogger<TodoService> logger)
        {
            _database = db;
            _logger = logger;
        }

        public async Task<IEnumerable<TodoVM>> GetTodosAsync(int userId)
        {
            var todos = await _database.Todos
                .Where(t => t.UserId == userId)
                .ToListAsync();

            return todos.Select(todo => new TodoVM
            {
                Id = todo.Id,
                Name = todo.Name,
                DateCreated = todo.DateCreated,
                Complete = todo.Complete,
                DateCompleted = todo.DateCompleted,
                UserId = todo.UserId
            });
        }

        public async Task<TodoVM?> CreateTodoAsync(TodoVM todoVM)
        {

            var userExists = await _database.Users.AnyAsync(u => u.Id == todoVM.UserId);
            if (!userExists)
            {
                _logger.LogWarning($"User with ID {todoVM.UserId} not found.");
                return null;
            }

            var todo = new Todo
            {
                Name = todoVM.Name,
                DateCreated = DateTime.Now,
                Complete = todoVM.Complete,
                DateCompleted = todoVM.Complete ? DateTime.Now : null,
                UserId = todoVM.UserId
            };

            _database.Todos.Add(todo);
            await _database.SaveChangesAsync();

            todoVM.Id = todo.Id;
            todoVM.DateCreated = todo.DateCreated;
            todoVM.DateCompleted = todo.DateCompleted;

            return todoVM;
        }

        public async Task<TodoVM?> UpdateTodoAsync(TodoVM todoVM)
        {
            var todo = await _database.Todos.FindAsync(todoVM.Id);
            if (todo == null)
            {
                _logger.LogWarning($"Todo with ID {todoVM.Id} not found.");
                return null;
            }

            if (todo.UserId != todoVM.UserId)
            {
                _logger.LogWarning($"User ID mismatch for Todo ID {todoVM.Id}.");
                return null;
            }

            todo.Name = todoVM.Name;
            todo.Complete = todoVM.Complete;
            if (todo.Complete && todo.DateCompleted == null)
            {
                todo.DateCompleted = DateTime.Now;
            }
            else if (!todo.Complete)
            {
                todo.DateCompleted = null;
            }

            _database.Todos.Update(todo);
            await _database.SaveChangesAsync();

            todoVM.DateCreated = todo.DateCreated;
            todoVM.DateCompleted = todo.DateCompleted;

            return todoVM;
        }

        public async Task DeleteTodoAsync(int id, int userId)
        {
            var todo = await _database.Todos.FindAsync(id);
            if (todo == null)
            {
                _logger.LogWarning($"Todo with ID {id} not found.");
                return;
            }

            if (todo.UserId != userId)
            {
                _logger.LogWarning($"User ID mismatch for Todo ID {id}.");
                return;
            }

            _database.Todos.Remove(todo);
            await _database.SaveChangesAsync();
        }
    }
}
