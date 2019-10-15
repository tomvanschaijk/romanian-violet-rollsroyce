using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RomanianVioletRollsRoyce.Data.DTO;

namespace RomanianVioletRollsRoyce.Web.Clients
{
    public interface ICustomersClient
    {
        Task<CustomerAccountDataResponse> GetCustomerAccountData();
    }
}
