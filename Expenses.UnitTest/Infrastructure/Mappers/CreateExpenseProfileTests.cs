using AutoMapper;
using Expenses.Application.Dtos;
using Expenses.Domain.Entities;
using Xunit;
using System;
using Expenses.Infrastructure.Mappers;

namespace Expenses.UnitTest.Infrastructure.Mappers
{
    public class CreateExpenseProfileTests
    {
        private readonly IMapper _mapper;

        public CreateExpenseProfileTests()
        {
            // Initialize AutoMapper with the CreateExpenseProfile
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CreateExpenseProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Mapping_ExpenseRequestDto_To_Expense_Should_Succeed()
        {
            // Arrange
            var expenseRequestDto = new ExpenseRequestDto
            {
                UserId = 1,
                ExpenseDate = DateTime.Now,
                Amount = 100.50m,
                Comment = "Test expense",
                Type = "Food",
                Currency = "USD"
            };

            // Act
            var expense = _mapper.Map<Expense>(expenseRequestDto);

            // Assert
            Assert.Equal(expenseRequestDto.UserId, expense.UserId);
            Assert.Equal(expenseRequestDto.ExpenseDate, expense.ExpenseDate);
            Assert.Equal(expenseRequestDto.Amount, expense.Amount);
            Assert.Equal(expenseRequestDto.Comment, expense.Comment);
            Assert.Null(expense.User);  // Since user is ignored in mapping
            Assert.Null(expense.Currency);  // Since currency is ignored in mapping
            Assert.NotEqual(default(DateTime), expense.CreatedDate);  // CreatedDate should be mapped to current DateTime
            Assert.NotEqual(default(DateTime), expense.UpdatedDate);  // UpdatedDate should be mapped to current DateTime
        }

        [Fact]
        public void Mapping_Expense_To_ExpenseResponseDto_Should_Succeed()
        {
            // Arrange
            var expense = new Expense
            {
                IdExpense = 1,
                ExpenseDate = DateTime.Now,
                Amount = 100.50m,
                Comment = "Test expense",
                User = new User { FirstName = "John", LastName = "Doe" },
                ExpenseType = new ExpenseType { Label = "Food" },
                Currency = new Currency { CurrencyCode = "USD" }
            };

            // Act
            var expenseResponseDto = _mapper.Map<ExpenseResponseDto>(expense);

            // Assert
            Assert.Equal(expense.IdExpense, expenseResponseDto.IdExpense);
            Assert.Equal(expense.ExpenseDate, expenseResponseDto.ExpenseDate);
            Assert.Equal(expense.Amount, expenseResponseDto.Amount);
            Assert.Equal(expense.Comment, expenseResponseDto.Comment);
            Assert.Equal("John Doe", expenseResponseDto.UserFullName);  // Full name should be correctly mapped
            Assert.Equal("Food", expenseResponseDto.Type);  // Expense type label should be correctly mapped
            Assert.Equal("USD", expenseResponseDto.Currency);  // Currency code should be correctly mapped
        }
    }
}
