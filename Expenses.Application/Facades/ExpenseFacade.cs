using AutoMapper;
using Expenses.Application.Dtos;
using Expenses.Application.Facades.Interfaces;
using Expenses.Utils.Enum;
using Expenses.Domain.Entities;
using Expenses.Domain.Services;


namespace Expenses.Application.Facades;

public class ExpenseFacade(IExpenseService expenseService, IMapper mapper) : IExpenseFacade
{
    public async Task<int> CreateExpenseAsync(ExpenseRequestDto expenseRequest, CancellationToken cancellationToken)
    {
        var expense = mapper.Map<Expense>(expenseRequest);

        var id = await expenseService.CreateExpenseAsync(expense,expenseRequest.Currency,expenseRequest.Type, cancellationToken);

        return id;
    }

    public async Task<ExpenseResponseDto> GetExpenseByIdAsync(int id, CancellationToken cancellationToken)
    {
        var expense = await expenseService.GetExpenseByIdAsync(id, cancellationToken);

        var expenseResponse = mapper.Map<ExpenseResponseDto>(expense);
        return expenseResponse;
    }

    public async Task<IEnumerable<ExpenseResponseDto>> GetExpensesByUserIdAsync(int id, CancellationToken cancellationToken)
    {
        var expenses = await expenseService.GetExpensesByUserIdAsync(id, cancellationToken);
        var expenseResponse = mapper.Map<IList<ExpenseResponseDto>>(expenses);
        return expenseResponse;
    }

    public async Task<IEnumerable<ExpenseResponseDto>> GetExpensesSortedAsync(int? userId, SortBy sortBy, SortOrder sortOrder, CancellationToken cancellationToken)
    {
        IEnumerable<Expense> expenses = await expenseService.GetExpensesSortedAsync(userId, sortBy, sortOrder, cancellationToken);
        var expensesResponse = mapper.Map<IList<ExpenseResponseDto>>(expenses);
        return expensesResponse;
    }
}
