using PRTracker.Entities;
using System.ComponentModel.DataAnnotations;

namespace PRTracker.Models
{
    public class UpdateExerciseViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Instructions { get; set; }
        public List<string>? Images { get; set; }
    }
}
