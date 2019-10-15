using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RomanianVioletRollsRoyce.Crosscutting.Factories;
using RomanianVioletRollsRoyce.Crosscutting.Responses;
using RomanianVioletRollsRoyce.Domain.Entities;
using RomanianVioletRollsRoyce.Domain.Requests;
using RomanianVioletRollsRoyce.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace RomanianVioletRollsRoyce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IServiceFactory _serviceFactory;

        public AccountsController(ILogger<AccountsController> logger, IServiceFactory serviceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
            var createdAccount = await _serviceFactory.GetService<IAccountService>().CreateAccount(request);
            return Ok(new DataResponse<Account> { Data = createdAccount });
        }

        [Route("{accountId}/transactions")]
        [HttpPost]
        public async Task<IActionResult> PerformTransaction([FromBody] CreateTransactionRequest request, int accountId)
        {
            var createdTransaction = await _serviceFactory.GetService<ITransactionService>().CreateTransaction(accountId, request.Amount);
            return Ok(new DataResponse<Transaction> { Data = createdTransaction });
        }
    }
}