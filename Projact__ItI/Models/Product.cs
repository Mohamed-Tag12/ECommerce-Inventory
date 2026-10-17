using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projact__ItI.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product title is required.")]
        [StringLength(150, ErrorMessage = "Title cannot be longer than 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }

        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
