using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models
{
    public class ContactInfo
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string LinkedIn { get; set; }

        public string GitHub { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? Whatsapp { get; set; }


    }
}

