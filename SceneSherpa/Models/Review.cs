namespace SceneSherpa.Models
{
    public class Review
    {
        public int Id { get; set; }
        public float Rating { get; set; }
        public Media Media { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } //make nullable
        public string? Content { get; set; }
    }
}
