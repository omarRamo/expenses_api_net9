using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expenses.Domain.Entities;
using Expenses.Domain.Repositories;
using Expenses.Domain.Services;
using Expenses.Domain.Exceptions;
using Xunit;

namespace Expenses.UnitTest.Domain.Services
{
    public class ExpenseServiceTests
    {
        private readonly Mock<IExpenseRepository> _mockExpenseRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ICurrencyRepository> _mockCurrencyRepository;
        private readonly Mock<IExpenseTypeRepository> _mockExpenseTypeRepository;
        private readonly ExpenseService _expenseService;

        public ExpenseServiceTests()
        {
            _mockExpenseRepository = new Mock<IExpenseRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockCurrencyRepository = new Mock<ICurrencyRepository>();
            _mockExpenseTypeRepository = new Mock<IExpenseTypeRepository>();

            _expenseService = new ExpenseService(
                _mockExpenseRepository.Object,
                _mockUserRepository.Object,
                _mockCurrencyRepository.Object,
                _mockExpenseTypeRepository.Object
            );
        }

        [Fact]
        public async Task GetExpenseByIdAsync_ShouldThrowEntityNotFoundException_WhenExpenseNotFound()
        {
            // Arrange
            var expenseId = 1;
            _mockExpenseRepository.Setup(repo => repo.GetExpenseByIdAsync(expenseId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Expense)null); // Simulate not found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _expenseService.GetExpenseByIdAsync(expenseId, CancellationToken.None));

            Assert.Equal("The expense does not exist.", exception.Message);
        }

        [Fact]
        public async Task CreateExpenseAsync_ShouldThrowValidationException_WhenUserDoesNotExist()
        {
            // Arrange
            var expenseRequest = new Expense
            {
                UserId = 999, // Simulate user not found
                ExpenseDate = DateTime.Now,
                Amount = 100,
                Comment = "Test expense"
            };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(expenseRequest.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null); // Simulate user not found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                _expenseService.CreateExpenseAsync(expenseRequest, "USD", "Restaurant", CancellationToken.None));

            Assert.Equal("The given user does not exist!", exception.Message);
        }

        [Fact]
        public async Task CreateExpenseAsync_ShouldThrowValidationException_WhenExpenseDateIsInFuture()
        {
            // Arrange
            var expenseRequest = new Expense
            {
                UserId = 1,
                ExpenseDate = DateTime.Now.AddDays(1), // Future date
                Amount = 100,
                Comment = "Test expense"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                _expenseService.CreateExpenseAsync(expenseRequest, "USD", "Restaurant", CancellationToken.None));

            Assert.Equal("The Date could not be in the future", exception.Message);
        }

        [Fact]
        public async Task CreateExpenseAsync_ShouldThrowValidationException_WhenExpenseAlreadyExists()
        {
            // Arrange
            var expenseRequest = new Expense
            {
                UserId = 1,
                ExpenseDate = DateTime.Now,
                Amount = 100,
                Comment = "Test expense"
            };

            // Simulate existing expense with the same date and amount
            _mockExpenseRepository.Setup(repo => repo.GetExpenseByDateAndAmountAsync(expenseRequest.UserId, expenseRequest.ExpenseDate, expenseRequest.Amount))
                .ReturnsAsync(new Expense()); // Simulate existing expense

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                _expenseService.CreateExpenseAsync(expenseRequest, "USD", "Restaurant", CancellationToken.None));

            Assert.Equal("This expense already exists, we cannot create it twice.", exception.Message);
        }

        [Fact]
        public async Task CreateExpenseAsync_ShouldReturnExpenseId_WhenExpenseCreatedSuccessfully()
        {
            // Arrange
            var expenseRequest = new Expense
            {
                UserId = 1,
                ExpenseDate = DateTime.Now,
                Amount = 100,
                Comment = "Test expense"
            };

            var user = new User { IdUser = 1, Currency = new Currency { CurrencyCode = "USD" } };
            var currency = new Currency { CurrencyCode = "USD" };
            var expenseType = new ExpenseType { Label = "Restaurant" };

            // Mock repository setups
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(expenseRequest.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _mockCurrencyRepository.Setup(repo => repo.GetCurrencyByName("USD"))
                .ReturnsAsync(currency);
            _mockExpenseTypeRepository.Setup(repo => repo.GetExpenseTypeByLabelAsync("Restaurant", It.IsAny<CancellationToken>()))
                .ReturnsAsync(expenseType);
            _mockExpenseRepository.Setup(repo => repo.GetExpenseByDateAndAmountAsync(expenseRequest.UserId, expenseRequest.ExpenseDate, expenseRequest.Amount))
                .ReturnsAsync((Expense)null); // No existing expense

            _mockExpenseRepository.Setup(repo => repo.AddAsync(It.IsAny<Expense>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask); // Simulate expense creation
            _mockExpenseRepository.Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); // Simulate successful save

            // Act
            var result = await _expenseService.CreateExpenseAsync(expenseRequest, "USD", "Restaurant", CancellationToken.None);

            // Assert
            Assert.Equal(expenseRequest.IdExpense, result); // Assert the returned expense ID
        }
    }
}
