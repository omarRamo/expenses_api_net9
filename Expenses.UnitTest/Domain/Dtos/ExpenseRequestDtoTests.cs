using System;
using System.ComponentModel.DataAnnotations;
using Expenses.Application.Dtos;
using Xunit;

namespace Expenses.UnitTest.Domain.Dtos
{
    public class ExpenseRequestDtoTests
    {
        [Fact]
        public void ExpenseRequestDto_ShouldHaveRequiredAttributes()
        {
            // Arrange
            var expenseRequestDto = new ExpenseRequestDto
            {
                UserId = 1,
                ExpenseDate = DateTime.Now,
                Amount = 100.0m,
                Comment = "Test comment",
                Type = "Food",
                Currency = "USD"
            };

            // Act
            var validationResults = ValidateModel(expenseRequestDto);

            // Assert
            Assert.Empty(validationResults); // Should not have any validation errors
        }

        [Fact]
        public void ExpenseRequestDto_ShouldFailValidation_WhenExpenseDateIsInThePast()
        {
            // Arrange
            var expenseRequestDto = new ExpenseRequestDto
            {
                UserId = 1,
                ExpenseDate = DateTime.Now.AddMonths(-6),  // Date older than 3 months
                Amount = 100.0m,
                Comment = "Test comment",
                Type = "Food",
                Currency = "USD"
            };

            // Act
            var validationResults = ValidateModel(expenseRequestDto);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("ExpenseDate"));
            Assert.Equal("The expense date cannot be older than 3 months.", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void ExpenseRequestDto_ShouldFailValidation_WhenAmountIsZero()
        {
            // Arrange
            var expenseRequestDto = new ExpenseRequestDto
            {
                UserId = 1,
                ExpenseDate = DateTime.Now,
                Amount = 0.0m, // Invalid amount (must be greater than zero)
                Comment = "Test comment",
                Type = "Restaurant",
                Currency = "USD"
            };

            // Act
            var validationResults = ValidateModel(expenseRequestDto);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Amount"));
            Assert.Equal("The amount must be greater than zero.", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void ExpenseRequestDto_ShouldFailValidation_WhenCommentIsTooLong()
        {
            // Arrange
            var expenseRequestDto = new ExpenseRequestDto
            {
                UserId = 1,
                ExpenseDate = DateTime.Now,
                Amount = 100.0m,
                Comment = new string('a', 501), // Invalid comment (should be <= 500 characters)
                Type = "Restaurant",
                Currency = "USD"
            };

            // Act
            var validationResults = ValidateModel(expenseRequestDto);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Comment"));
            Assert.Equal("The Comment field must not exceed 500 characters.", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void ExpenseRequestDto_ShouldFailValidation_WhenCurrencyIsIncorrectLength()
        {
            // Arrange
            var expenseRequestDto = new ExpenseRequestDto
            {
                UserId = 1,
                ExpenseDate = DateTime.Now,
                Amount = 100.0m,
                Comment = "Test comment",
                Type = "Restaurant",
                Currency = "US"  // Invalid currency (should be 3 characters)
            };

            // Act
            var validationResults = ValidateModel(expenseRequestDto);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Currency"));
            Assert.Equal("The Currency field must be 3 characters long.", validationResults[0].ErrorMessage);
        }

        private static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
