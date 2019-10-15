using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RomanianVioletRollsRoyce.Crosscutting.Factories;
using RomanianVioletRollsRoyce.Crosscutting.Responses;
using RomanianVioletRollsRoyce.Data.DTO;
using RomanianVioletRollsRoyce.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RomanianVioletRollsRoyce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly IServiceFactory _serviceFactory;

        public CustomersController(ILogger<CustomersController> logger, IServiceFactory serviceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
        }

        [Route("accountdata")]
        [HttpGet]
        public async Task<IActionResult> GetCustomerAccountData()
        {
            var accountService = _serviceFactory.GetService<IAccountService>();
            var accountTransactionData = await accountService.GetCustomerAccountData();
            return Ok(new DataResponse<ICollection<CustomerAccountData>> { Data = accountTransactionData });
        }
    }
}