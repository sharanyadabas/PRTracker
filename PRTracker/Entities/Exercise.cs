namespace PRTracker.Entities
{
    public class Exercise
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public List<string> Images {  get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

    }
}
