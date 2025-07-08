namespace MyPortfolio.Models
{
    public class AboutUs
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Location { get; set; } // Optional
        public string Description { get; set; }
        public string Duration { get; set; } // e.g., "2024 – Present"
        public int Order { get; set; }
    }
}
