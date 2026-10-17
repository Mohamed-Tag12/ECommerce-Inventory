using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projact__ItI.Models
{
    
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [Required(ErrorMessage = "Please select a supplier.")]
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        [Required(ErrorMessage = "Please select a product.")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } = 1;

        [Range(0.01, 10000000, ErrorMessage = "Unit cost must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitCost { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        [NotMapped]
        public decimal Total => UnitCost * Quantity;
    }
}
