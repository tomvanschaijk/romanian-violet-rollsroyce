using System.Collections.Generic;

namespace RomanianVioletRollsRoyce.Crosscutting.Responses
{
    public class DataResponse<TData> : Response
    {
        public TData Data { get; set; }
        public ICollection<Message> Warnings { get; set; } = new List<Message>();
        public IDictionary<string, IDictionary<string, object>> Dictionaries { get; set; } = new Dictionary<string, IDictionary<string, object>>();
    }
}
