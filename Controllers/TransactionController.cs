
using System.Security.Claims;
using GestaoFinanca.Data;
using GestaoFinanca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanca.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    [Authorize]
    public class TransactionController : ControllerBase
    {

        private readonly AppDbContext _appDbContext;
        public TransactionController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private bool TryGetAuthenticatedUserId(out int userId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim, out userId);
        }

        // Income Endpoints --------------------------------

        [HttpPost("income")]
        public async Task<ActionResult> CreateTransactionIncome ([FromBody] TransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryGetAuthenticatedUserId(out var userId))
            {
                return Unauthorized();
            }

            var transaction = new Transaction
            {
                Description = request.Description,
                Amount = request.Amount,
                Type = TransactionType.Income,
                Date = request.Date ?? DateTime.UtcNow,
                UserId = userId
            };

            _appDbContext.Transactions.Add(transaction);
            await _appDbContext.SaveChangesAsync();

            var response = new TransactionResponse
            {
                Id = transaction.Id,
                Description = transaction.Description,
                Amount = transaction.Amount,
                Type = transaction.Type,
                Date = transaction.Date
            };

            return Ok(response);

        }

        [HttpGet("income")]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetTransactionsInCome()
        {
            if (!TryGetAuthenticatedUserId(out var userId))
            {
                return Unauthorized();
            }

            var transactions = await _appDbContext.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Income)
            .Select(t => new TransactionResponse
            {
                Id = t.Id,
                Description = t.Description,
                Amount = t.Amount,
                Type = t.Type,
                Date = t.Date
            }).ToListAsync();
            return Ok(transactions);
        }
        

        [HttpGet("income/{id}")]
        public async Task<ActionResult<TransactionResponse>> GetTransactionIncome(int id)
        {
            if (!TryGetAuthenticatedUserId(out var userId))
            {
                return Unauthorized();
            }

            var transactions = await _appDbContext.Transactions
            .Where(t => t.Id == id && t.UserId == userId && t.Type == TransactionType.Income)
            .Select(t => new TransactionResponse
            {
                Id = t.Id,
                Description = t.Description,
                Amount = t.Amount,
                Type = t.Type,
                Date = t.Date
            }).FirstOrDefaultAsync();

            if (transactions == null)
            {
                return NotFound("Transaction not found!");
            }

            return Ok(transactions);

        }

        [HttpPut("income/{id}")]
        public async Task<ActionResult> UpdateTransactionIncome(int id, [FromBody] TransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryGetAuthenticatedUserId(out var userId))
            {
                return Unauthorized();
            }

            var transaction = await _appDbContext.Transactions.
            FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId && t.Type == TransactionType.Income);

            if (transaction == null)
            {
                return NotFound("Transaction not found!");
            }

            transaction.Description = request.Description;
            transaction.Amount = request.Amount;
            transaction.Date = request.Date ?? transaction.Date;

            await _appDbContext.SaveChangesAsync();

            var response = new TransactionResponse
            {
                Id = transaction.Id,
                Description = transaction.Description,
                Amount = transaction.Amount,
                Type = transaction.Type,
                Date = transaction.Date
            };

            return Ok(response);
        }

        [HttpDelete("income/{id}")]
        public async Task<ActionResult> DeleteTransactionIncome(int id)
        {
            if (!TryGetAuthenticatedUserId(out var userId))
            {
                return Unauthorized();
            }

            var transaction = await _appDbContext.Transactions.
            FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId && t.Type == TransactionType.Income);

            if (transaction == null)
            {
                return NotFound("Transaction not found!");
            }

            _appDbContext.Transactions.Remove(transaction);
            await _appDbContext.SaveChangesAsync();

            return Ok("Transaction deleted successfully!");
        }


        // Expense Endpoints --------------------------------

        [HttpPost("expense")]
        public async Task<ActionResult> CreateTransactionExpense ([FromBody] TransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryGetAuthenticatedUserId(out var userId))
            {
                return Unauthorized();
            }

            var transaction = new Transaction
            {
                Description = request.Description,
                Amount = request.Amount,
                Type = TransactionType.Expense,
                Date = request.Date ?? DateTime.UtcNow,
                UserId = userId
            };

            _appDbContext.Transactions.Add(transaction);
            await _appDbContext.SaveChangesAsync();

            var response = new TransactionResponse
            {
                Id = transaction.Id,
                Description = transaction.Description,
                Amount = transaction.Amount,
                Type = transaction.Type,
                Date = transaction.Date
            };

            return Ok(response);

        }

        [HttpGet("expense")]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetTransactionsExpense()
        {
            if (!TryGetAuthenticatedUserId(out var userId))
            {
                return Unauthorized();
            }

            var transactions = await _appDbContext.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Expense)
            .Select(t => new TransactionResponse
            {
                Id = t.Id,
                Description = t.Description,
                Amount = t.Amount,
                Type = t.Type,
                Date = t.Date
            }).ToListAsync();
            return Ok(transactions);
        }

        [HttpGet("expense/{id}")]
        public async Task<ActionResult<TransactionResponse>> GetTransactionExpense(int id)
        {
            if (!TryGetAuthenticatedUserId(out var userId))
            {
                return Unauthorized();
            }

            var transactions = await _appDbContext.Transactions
            .Where(t => t.Id == id && t.UserId == userId && t.Type == TransactionType.Expense)
            .Select(t => new TransactionResponse
            {
                Id = t.Id,
                Description = t.Description,
                Amount = t.Amount,
                Type = t.Type,
                Date = t.Date
            }).FirstOrDefaultAsync();

            if (transactions == null)
            {
                return NotFound("Transaction not found!");
            }

            return Ok(transactions);

        }

        [HttpPut("expense/{id}")]
        public async Task<ActionResult> UpdateTransactionExpense(int id, [FromBody] TransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryGetAuthenticatedUserId(out var userId))
            {
                return Unauthorized();
            }

            var transaction = await _appDbContext.Transactions.
            FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId && t.Type == TransactionType.Expense);

            if (transaction == null)
            {
                return NotFound("Transaction not found!");
            }

            transaction.Description = request.Description;
            transaction.Amount = request.Amount;
            transaction.Date = request.Date ?? transaction.Date;

            await _appDbContext.SaveChangesAsync();

            var response = new TransactionResponse
            {
                Id = transaction.Id,
                Description = transaction.Description,
                Amount = transaction.Amount,
                Type = transaction.Type,
                Date = transaction.Date
            };

            return Ok(response);
        }

        [HttpDelete("expense/{id}")]
        public async Task<ActionResult> DeleteTransactionExpense(int id)
        {
            if (!TryGetAuthenticatedUserId(out var userId))
            {
                return Unauthorized();
            }

            var transaction = await _appDbContext.Transactions.
            FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId && t.Type == TransactionType.Expense);

            if (transaction == null)
            {
                return NotFound("Transaction not found!");
            }

            _appDbContext.Transactions.Remove(transaction);
            await _appDbContext.SaveChangesAsync();

            return Ok("Transaction deleted successfully!");
        }
    }
}