using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projact__ItI.Models
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        [Required(ErrorMessage = "Please select a product.")]
        public int ProductId { get; set; }

        public Product? Product { get; set; }

        [StringLength(100, ErrorMessage = "Customer name cannot be longer than 100 characters.")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.Now;

        [NotMapped]
        public decimal Total => UnitPrice * Quantity;
    }
}
