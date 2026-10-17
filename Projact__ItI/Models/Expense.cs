using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projact__ItI.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(120, ErrorMessage = "Title cannot be longer than 120 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please choose a category.")]
        [StringLength(50)]
        public string Category { get; set; } = "Other";

        [Range(0.01, 100000000, ErrorMessage = "Amount must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime ExpenseDate { get; set; } = DateTime.Today;

        [StringLength(300)]
        public string? Note { get; set; }
    }

    public static class ExpenseCategories
    {
        public static readonly string[] All =
        {
            "Rent", "Salaries", "Utilities", "Marketing", "Shipping", "Maintenance", "Other"
        };
    }
}
