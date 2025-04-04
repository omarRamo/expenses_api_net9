using Expenses.Domain.Entities;
using Expenses.Domain.Exceptions;
using Expenses.Infrastructure.DbContext;
using Expenses.Infrastructure.Repositories;
using Expenses.Utils.Enum;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Expenses.UnitTest.Infrastructure.Repositories
{
    public class ExpenseRepositoryTests
    {
        private readonly ExpensesDbContext _dbContext;
        private readonly ExpenseRepository _expenseRepository;

        public ExpenseRepositoryTests()
        {
            // Create an in-memory database for testing
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())  // Use a unique name for each test
                .Options;

            _dbContext = new ExpensesDbContext(options);
            _expenseRepository = new ExpenseRepository(_dbContext);
        }

        [Fact]
        public async Task GetExpenseByDateAndAmountAsync_ShouldReturnExpense_WhenFound()
        {
            // Arrange
            var expenseId = 1;
            var expenseDate = DateTime.Now;
            var amount = 100m;

            var mockExpense = new Expense
            {
                IdExpense = expenseId,
                ExpenseDate = expenseDate,
                Amount = amount,
                UserId = expenseId
            };

            // Add the mock expense to the in-memory database
            _dbContext.Expenses.Add(mockExpense);
            await _dbContext.SaveChangesAsync();

            // Act
            var expense = await _expenseRepository.GetExpenseByDateAndAmountAsync(expenseId, expenseDate, amount);

            // Assert
            Assert.NotNull(expense);
            Assert.Equal(expenseId, expense.IdExpense);
        }

        [Fact]
        public async Task GetExpenseByIdAsync_ShouldThrowException_WhenNotFound()
        {
            // Arrange
            var expenseId = 1;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _expenseRepository.GetExpenseByIdAsync(expenseId, CancellationToken.None));
            Assert.Equal("user with ID 1 not found.", exception.Message);
        }

        [Fact]
        public async Task GetExpensesByUserIdAsync_ShouldReturnListOfExpenses()
        {
            // Arrange
            var userId = 1;
            var mockExpenses = new List<Expense>
            {
                new Expense { IdExpense = 1, UserId = userId, Amount = 200 },
                new Expense { IdExpense = 2, UserId = userId, Amount = 100 }
            };

            // Add the mock expenses to the in-memory database
            _dbContext.Expenses.AddRange(mockExpenses);
            await _dbContext.SaveChangesAsync();

            // Log the data inserted
            var allExpenses = await _dbContext.Expenses.ToListAsync();
            Console.WriteLine("Inserted Expenses:");
            foreach (var expense in allExpenses)
            {
                Console.WriteLine($"Expense Id: {expense.IdExpense}, UserId: {expense.UserId}, Amount: {expense.Amount}");
            }

            // Act
            var expenses = await _expenseRepository.GetExpensesByUserIdAsync(userId);

            // Log the retrieved expenses
            Console.WriteLine("Retrieved Expenses:");
            foreach (var expense in expenses)
            {
                Console.WriteLine($"Expense Id: {expense.IdExpense}, UserId: {expense.UserId}, Amount: {expense.Amount}");
            }

            // Assert
            Assert.Equal(0, expenses.Count());
            Assert.All(expenses, expense => Assert.Equal(userId, expense.UserId));
        }

        [Fact]
        public async Task GetExpensesByUserIdSortedAsync_ShouldReturnSortedExpenses_Ascending()
        {
            // Arrange
            var userId = 1;
            var mockExpenses = new List<Expense>
            {
                new Expense { IdExpense = 1, UserId = userId, Amount = 200 },
                new Expense { IdExpense = 2, UserId = userId, Amount = 100 }
            };

            _dbContext.Expenses.AddRange(mockExpenses);
            await _dbContext.SaveChangesAsync();

            // Act
            var expenses = await _expenseRepository.GetExpensesByUserIdSortedAsync(userId, SortBy.amount, SortOrder.Ascending, CancellationToken.None);

            // Assert
            Assert.Equal(2, expenses.Count());
            Assert.Equal(100, expenses.First().Amount);  // First element should have the lowest amount (ascending)
        }

        [Fact]
        public async Task GetExpensesByUserIdSortedAsync_ShouldReturnSortedExpenses_Descending()
        {
            // Arrange
            var userId = 1;
            var mockExpenses = new List<Expense>
            {
                new Expense { IdExpense = 1, UserId = userId, Amount = 100 },
                new Expense { IdExpense = 2, UserId = userId, Amount = 200 }
            };

            _dbContext.Expenses.AddRange(mockExpenses);
            await _dbContext.SaveChangesAsync();

            // Act
            var expenses = await _expenseRepository.GetExpensesByUserIdSortedAsync(userId, SortBy.amount, SortOrder.Ascending, CancellationToken.None);

            // Assert
            Assert.Equal(2, expenses.Count());
            Assert.Equal(200, expenses.First().Amount);  // First element should have the highest amount (descending)
        }
    }
}
