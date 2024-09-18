using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rockfast.ViewModels
{
    public class UserVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;
        public ICollection<TodoVM> Todos { get; set; } = new List<TodoVM>();
    }
}
