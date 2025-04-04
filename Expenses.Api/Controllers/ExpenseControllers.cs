using CorrelationId.Abstractions;
using Expenses.Application.Dtos;
using Expenses.Application.Facades.Interfaces;
using Expenses.Utils.Enum;
using Expenses.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Expenses.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly ICorrelationContextAccessor _correlationContext;
        private readonly ILogger<ExpensesController> _logger;
        private readonly IExpenseFacade _expenseFacade;

        public ExpensesController(
            ICorrelationContextAccessor correlationContext,
            ILogger<ExpensesController> logger,
            IExpenseFacade expenseFacade)
        {
            _correlationContext = correlationContext;
            _logger = logger;
            _expenseFacade = expenseFacade;
        }

        /// <summary>
        /// Récupère une dépense par son Id.
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid id.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Expense not found.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Expense successfully returned.")]
        public async Task<IActionResult> GetExpenseById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var expense = await _expenseFacade.GetExpenseByIdAsync(id, cancellationToken);

                if (expense == null)
                {
                    return NotFound();
                }

                return Ok(expense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Exception Details. CorrelationId: {correlationId}",
                    _correlationContext.CorrelationContext.CorrelationId);

                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new { message = ex.Message });
            }
        }

        /// <summary>
        /// Récupère la liste des dépenses, avec possibilité de filtrer et de trier.
        /// </summary>
        /// <param name="userId">Id de l'utilisateur (optionnel).</param>
        /// <param name="sortBy">0 = expenseDate, 1 = amount</param>
        /// <param name="sortOrder">0 = Ascending, 1 = Descending</param>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, "Expenses successfully returned.")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "No expenses found.")]
        public async Task<IActionResult> GetExpenses(
            [FromQuery, SwaggerParameter("Id de l'utilisateur (optionnel).")] int? userId,
            [FromQuery, SwaggerParameter("Tri : 0 = expenseDate, 1 = amount")] SortBy sortBy = SortBy.expenseDate,
            [FromQuery, SwaggerParameter("Ordre de tri : 0 = Ascending, 1 = Descending")] SortOrder sortOrder = SortOrder.Ascending,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var expenses = await _expenseFacade
                    .GetExpensesSortedAsync(userId, sortBy, sortOrder, cancellationToken);

                if (expenses == null || !expenses.Any())
                {
                    return NoContent(); // 204 No Content
                }

                return Ok(expenses); // 200 OK
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Exception Details. CorrelationId: {correlationId}",
                    _correlationContext.CorrelationContext.CorrelationId);

                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crée une nouvelle dépense.
        /// </summary>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid Request.")]
        [SwaggerResponse((int)HttpStatusCode.Created, "Expense has been created successfully.")]
        public async Task<IActionResult> CreateExpense(
            [FromBody] ExpenseRequestDto expenseRequestDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var id = await _expenseFacade.CreateExpenseAsync(expenseRequestDto, cancellationToken);

                // Retourne l'URL de la ressource créée dans l'en-tête "Location"
                return CreatedAtAction(nameof(GetExpenseById), new { id }, new { id });
            }
            catch (ValidationException e)
            {
                _logger.LogError(e,
                    "Validation Exception Details. CorrelationId: {correlationId}",
                    _correlationContext.CorrelationContext.CorrelationId);
                return BadRequest(new { message = e.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Exception Details. CorrelationId: {correlationId}",
                    _correlationContext.CorrelationContext.CorrelationId);

                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new { message = ex.Message });
            }
        }
    }
}
