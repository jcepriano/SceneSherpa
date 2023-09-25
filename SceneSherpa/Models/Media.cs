using System.Runtime.InteropServices;

namespace SceneSherpa.Models
{
    public class Media
    {
        public int Id { get; set; }
        public int ViewCount { get; set; }
        public string Title { get; set; }
        public string ProductionCompany { get; set; }
        public string Type { get; set; }
        public string Description { get; set; } = "No Description";
        public string AgeRating { get; set; } = "NOT RATED";
        public string ImageURL { get; set; } = "https://placehold.co/200x300";
        public List<Review> Reviews { get; set; }

    }
}
