using PRTracker.Entities;
using System.ComponentModel.DataAnnotations;

namespace PRTracker.Models
{
    public class CreateUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string PasswordHash { get; set; }
    }
}
