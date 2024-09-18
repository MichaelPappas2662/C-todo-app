using Rockfast.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rockfast.ServiceInterfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoVM>> GetTodosAsync(int userId);
        Task<TodoVM?> CreateTodoAsync(TodoVM todo);
        Task<TodoVM?> UpdateTodoAsync(TodoVM todo);
        Task DeleteTodoAsync(int id, int userId);
    }
}
