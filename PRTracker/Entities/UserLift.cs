namespace PRTracker.Entities
{
    public class UserLift
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExerciseId { get; set; }
        public DateTime Date { get; set; }
        public int Sets {  get; set; }
        public int Reps { get; set; }
        public float Weight { get; set; }
        public string Notes { get; set; }
    }
}
