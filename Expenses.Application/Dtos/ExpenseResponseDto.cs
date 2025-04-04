using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Application.Dtos
{
    public record ExpenseResponseDto
    {
        public long IdExpense { get; set; }  
        public DateTime ExpenseDate { get; set; } 
        public decimal Amount { get; set; } 
        public string Comment { get; set; }
        public string Type { get; set; }  
        public string Currency { get; set; } 
        public string UserFullName { get; set; } 

        public ExpenseResponseDto() { }
        public ExpenseResponseDto(long idExpense, DateTime expenseDate, decimal amount, string comment,
                                  string type, string currency, string firstName, string lastName)
        {
            IdExpense = idExpense;
            ExpenseDate = expenseDate;
            Amount = amount;
            Comment = comment;
            Type = type;
            Currency = currency;
            UserFullName = $"{firstName} {lastName}";
        }
    }
}
