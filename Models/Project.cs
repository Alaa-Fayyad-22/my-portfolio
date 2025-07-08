using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }  // Make nullable
        public string? ImageUrl { get; set; }     // Make nullable
        public string? ProjectURL { get; set; }   // Make nullable

    }
}

