using System.ComponentModel.DataAnnotations;

namespace Projact__ItI.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Supplier name is required.")]
        [StringLength(120, ErrorMessage = "Name cannot be longer than 120 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [StringLength(30)]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(80)]
        public string? City { get; set; }

        [StringLength(200)]
        public string? Supplies { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
