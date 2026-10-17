using System.ComponentModel.DataAnnotations;

namespace Projact__ItI.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(30)]
        public string? Phone { get; set; }

        [StringLength(80)]
        public string? City { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.Now;
    }
}
