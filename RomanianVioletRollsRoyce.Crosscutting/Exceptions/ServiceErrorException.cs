using Microsoft.AspNetCore.Http;

namespace RomanianVioletRollsRoyce.Crosscutting.Exceptions
{
    public class ServiceErrorException : CustomException
    {
        public ServiceErrorException(string message) : base(message) { }

        public override int HttpStatusCode => StatusCodes.Status503ServiceUnavailable;
    }
}
