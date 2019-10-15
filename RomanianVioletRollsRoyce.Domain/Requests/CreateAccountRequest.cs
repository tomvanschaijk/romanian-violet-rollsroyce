namespace RomanianVioletRollsRoyce.Domain.Requests
{
    public class CreateAccountRequest
    {
        public int CustomerId { get; set; }

        public decimal InitialCredit { get; set; }
    }
}
