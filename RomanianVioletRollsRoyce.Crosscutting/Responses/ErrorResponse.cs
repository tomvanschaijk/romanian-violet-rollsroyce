using System.Collections.Generic;

namespace RomanianVioletRollsRoyce.Crosscutting.Responses
{
    public class ErrorResponse : Response
    {
        public ICollection<Message> Errors { get; } = new List<Message>();

        public ErrorResponse(int statusCode, string message)
        {
            Errors.Add(new Message
            {
                StatusCode = statusCode,
                Title = message
            });
        }
    }
}
