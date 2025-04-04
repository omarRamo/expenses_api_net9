using Expenses.Utils.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace Expenses.Application.Dtos
{
    public record ExpenseRequestDto
    {
        [Required(ErrorMessage = "The UserId field is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The Expense Date field is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [ExpenseDateValidation(ErrorMessage = "The expense date cannot be older than 3 months.")]
        public DateTime ExpenseDate { get; set; }

        [Required(ErrorMessage = "The Amount field is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "The Comment field is required.")]
        [StringLength(500, ErrorMessage = "The Comment field must not exceed 500 characters.")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "The Expense Type field is required.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "The Currency field is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "The Currency field must be 3 characters long.")]
        public string Currency { get; set; }
    }
}
