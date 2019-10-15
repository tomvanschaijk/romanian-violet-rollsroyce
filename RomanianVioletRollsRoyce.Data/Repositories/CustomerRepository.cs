using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RomanianVioletRollsRoyce.Crosscutting.Exceptions;
using RomanianVioletRollsRoyce.Data.Interfaces;
using RomanianVioletRollsRoyce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RomanianVioletRollsRoyce.Data.Repositories
{
    public class CustomerRepository : DataRepositoryBase, ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(ILogger<CustomerRepository> logger, IMemoryCache cache) : base(cache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Customer> GetCustomer(int customerId)
        {
            var customer = _cache.Get<Customer>($"C-{customerId}");
            if (customer == null)
            {
                _logger.LogError($"Customer with id '{customerId}' not found");
                throw new DataNotFoundException("Requested account not found!");
            }

            return customer;
        }

        public async Task<ICollection<Customer>> GetAllCustomers() => await GetAll<Customer>('C');
    }
}
