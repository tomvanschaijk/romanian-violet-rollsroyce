using Microsoft.AspNetCore.Http;

namespace RomanianVioletRollsRoyce.Crosscutting.Exceptions
{
    public class DataNotFoundException : CustomException
    {
        public DataNotFoundException(string message) : base(message) { }

        public override int HttpStatusCode => StatusCodes.Status404NotFound;
    }
}
