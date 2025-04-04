using Moq;
using Xunit;
using Expenses.Application.Dtos;
using Expenses.Application.Facades;
using Expenses.Domain.Entities;
using Expenses.Domain.Services;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Expenses.Utils.Enum;

namespace Expenses.UnitTest.Application.Facades
{
    public class ExpenseFacadeTests
    {
        private readonly Mock<IExpenseService> _mockExpenseService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ExpenseFacade _expenseFacade;

        public ExpenseFacadeTests()
        {
            _mockExpenseService = new Mock<IExpenseService>();
            _mockMapper = new Mock<IMapper>();
            _expenseFacade = new ExpenseFacade(_mockExpenseService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateExpenseAsync_ShouldReturnId()
        {
            // Arrange
            var expenseRequestDto = new ExpenseRequestDto
            {
                UserId = 1,
                ExpenseDate = DateTime.Now,
                Amount = 100.0m,
                Comment = "Test expense",
                Type = "Misc",
                Currency = "USD"
            };

            var expense = new Expense
            {
                IdExpense = 1,
                ExpenseDate = expenseRequestDto.ExpenseDate,
                Amount = expenseRequestDto.Amount,
                Comment = expenseRequestDto.Comment
            };

            _mockMapper.Setup(m => m.Map<Expense>(It.IsAny<ExpenseRequestDto>())).Returns(expense);
            _mockExpenseService.Setup(s => s.CreateExpenseAsync(It.IsAny<Expense>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _expenseFacade.CreateExpenseAsync(expenseRequestDto, CancellationToken.None);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetExpenseByIdAsync_ShouldReturnExpenseResponseDto()
        {
            // Arrange
            var expense = new Expense
            {
                IdExpense = 1,
                ExpenseDate = DateTime.Now,
                Amount = 100.0m,
                Comment = "Test expense"
            };

            var expenseResponseDto = new ExpenseResponseDto(
                1, expense.ExpenseDate, expense.Amount, expense.Comment, "Misc", "USD", "John", "Doe");

            _mockExpenseService.Setup(s => s.GetExpenseByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expense);
            _mockMapper.Setup(m => m.Map<ExpenseResponseDto>(It.IsAny<Expense>())).Returns(expenseResponseDto);

            // Act
            var result = await _expenseFacade.GetExpenseByIdAsync(1, CancellationToken.None);

            // Assert
            Assert.Equal(expenseResponseDto, result);
        }

        [Fact]
        public async Task GetExpensesByUserIdAsync_ShouldReturnListOfExpenseResponseDto()
        {
            // Arrange
            var expenses = new List<Expense>
            {
                new Expense
                {
                    IdExpense = 1,
                    ExpenseDate = DateTime.Now,
                    Amount = 100.0m,
                    Comment = "Test expense"
                },
                new Expense
                {
                    IdExpense = 2,
                    ExpenseDate = DateTime.Now,
                    Amount = 200.0m,
                    Comment = "Test expense 2"
                }
            };

            var expenseResponseDtos = expenses.Select(expense => new ExpenseResponseDto(
                expense.IdExpense, expense.ExpenseDate, expense.Amount, expense.Comment, "Misc", "USD", "John", "Doe"
            )).ToList();

            _mockExpenseService.Setup(s => s.GetExpensesByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expenses);
            _mockMapper.Setup(m => m.Map<IList<ExpenseResponseDto>>(It.IsAny<IEnumerable<Expense>>()))
                .Returns(expenseResponseDtos);

            // Act
            var result = await _expenseFacade.GetExpensesByUserIdAsync(1, CancellationToken.None);

            // Assert
            Assert.Equal(expenseResponseDtos, result);
        }

        [Fact]
        public async Task GetExpensesSortedAsync_ShouldReturnSortedExpenses()
        {
            // Arrange
            var expenses = new List<Expense>
            {
                new Expense
                {
                    IdExpense = 1,
                    ExpenseDate = DateTime.Now.AddDays(-1),
                    Amount = 100.0m,
                    Comment = "Test expense"
                },
                new Expense
                {
                    IdExpense = 2,
                    ExpenseDate = DateTime.Now.AddDays(1),
                    Amount = 200.0m,
                    Comment = "Test expense 2"
                }
            };

            var expenseResponseDtos = expenses.Select(expense => new ExpenseResponseDto(
                expense.IdExpense, expense.ExpenseDate, expense.Amount, expense.Comment, "Misc", "USD", "John", "Doe"
            )).ToList();

            _mockExpenseService.Setup(s => s.GetExpensesSortedAsync(It.IsAny<int?>(), It.IsAny<SortBy>(), It.IsAny<SortOrder>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expenses);
            _mockMapper.Setup(m => m.Map<IList<ExpenseResponseDto>>(It.IsAny<IEnumerable<Expense>>()))
                .Returns(expenseResponseDtos);

            // Act
            var result = await _expenseFacade.GetExpensesSortedAsync(null, SortBy.amount, SortOrder.Ascending, CancellationToken.None);

            // Assert
            Assert.Equal(expenseResponseDtos, result);
        }
    }
}
