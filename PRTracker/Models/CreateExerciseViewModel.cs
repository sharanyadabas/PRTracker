using PRTracker.Entities;
using System.ComponentModel.DataAnnotations;

namespace PRTracker.Models
{
    public class CreateExerciseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name of the exercise is required")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public List<string> Images { get; set; }
    }
}
