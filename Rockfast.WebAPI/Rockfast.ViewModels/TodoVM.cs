using System;
using System.ComponentModel.DataAnnotations;

namespace Rockfast.ViewModels
{
    public class TodoVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        public DateTime? DateCreated { get; set; }
        public bool Complete { get; set; }
        public DateTime? DateCompleted { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
    }
}
