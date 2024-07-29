using PRTracker.Entities;
using System.ComponentModel.DataAnnotations;

namespace PRTracker.Models
{
    public class UpdateExerciseViewModel : CreateExerciseViewModel
    {
        public new string? Name { get; set; }
    }
}
