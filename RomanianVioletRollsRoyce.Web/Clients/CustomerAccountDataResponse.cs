using RomanianVioletRollsRoyce.Data.DTO;
using System.Collections.Generic;

namespace RomanianVioletRollsRoyce.Web.Clients
{
    public class CustomerAccountDataResponse
    {
        public bool Success { get; set; }

        public ICollection<CustomerAccountData> Data { get; set; } = new List<CustomerAccountData>();

        public string ErrorMessage { get; set; }
    }
}
