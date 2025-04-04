using AutoMapper;
using Expenses.Domain.Entities;
using Expenses.Domain.Exceptions;
using Expenses.Domain.Repositories;
using Expenses.Utils.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Domain.Services
{
    public class ExpenseService(IExpenseRepository expenseRepository, IUserRepository userRepository
        ,ICurrencyRepository currencyRespository, IExpenseTypeRepository expenseTypeRepository)
    : IExpenseService
    {

        public async Task<Expense> GetExpenseByIdAsync(int id, CancellationToken cancellationToken)
        {
            var expense = await expenseRepository.GetExpenseByIdAsync(id, cancellationToken);

            if (expense == null)
            {
                throw new EntityNotFoundException("The expense does not exist.");
            }

            return expense;

        }
        public async Task<int> CreateExpenseAsync(Expense expenseRequest, string currencyRequest, string typeRequest, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByIdAsync(expenseRequest.UserId, cancellationToken);

            var currency = await currencyRespository.GetCurrencyByName(currencyRequest);
            var expenseType = await expenseTypeRepository.GetExpenseTypeByLabelAsync(typeRequest, cancellationToken);

            ValidateExpenseForCreation(expenseRequest, currencyRequest, user, currency, expenseType, typeRequest);

            expenseRequest.CreatedDate = DateTime.Now;
            expenseRequest.UpdatedDate = DateTime.Now;
            expenseRequest.Currency = currency;
            expenseRequest.ExpenseType = expenseType;

            await expenseRepository.AddAsync(expenseRequest, cancellationToken);

            await expenseRepository.SaveChangesAsync(cancellationToken);

            return expenseRequest.IdExpense;
        }

        public async Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(int id, CancellationToken cancellationToken)
        {
            var expenses = await expenseRepository.GetExpensesByUserIdAsync(id);

            return expenses;
        }

        public async Task<IEnumerable<Expense>> GetExpensesSortedAsync(int? userId, SortBy sortBy, SortOrder sortOrder, CancellationToken cancellationToken)
        {
            var expenses = await expenseRepository.GetExpensesByUserIdSortedAsync(userId, sortBy, sortOrder, cancellationToken);

            return expenses;
        }

        private async void ValidateExpenseForCreation(Expense expenseRequest, string currencyRequest, User user, Currency currency, Entities.ExpenseType? expenseType, string typeRequest)
        {
            if (user == null)
            {
                throw new ValidationException("The given user does not exist!");
            }

            if (expenseRequest.ExpenseDate > DateTime.Now)
            {
                throw new ValidationException("The Date could not be in the future");
            }

            if (expenseRequest.ExpenseDate < DateTime.Now.AddMonths(-3))
            {
                throw new ValidationException("An expense cannot be dated more than 3 months ago");
            }

            if (string.IsNullOrEmpty(expenseRequest.Comment))
            {
                throw new ValidationException("The comment is mandatory.");
            }

            var existingExpense = await expenseRepository.GetExpenseByDateAndAmountAsync(expenseRequest.UserId, expenseRequest.ExpenseDate, expenseRequest.Amount);
            if (existingExpense != null)
            {
                throw new ValidationException("This expense already exists, we cannot create it twice.");
            }

            if (user.Currency.CurrencyCode != currencyRequest)
            {
                throw new ValidationException("The expense currency must match the user's currency.");
            }

            if (currency == null)
            {
                throw new ValidationException($"The currency '{expenseRequest.Currency.Name}' does not exist.");
            }

            if (expenseType == null)
            {
                throw new ValidationException($"The expense type '{typeRequest}' does not exist.");
            }
        }
    }

}
