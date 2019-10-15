using System;

namespace RomanianVioletRollsRoyce.Crosscutting.Exceptions
{
    public abstract class CustomException : Exception
    {
        protected CustomException(string message) : base(message) { }

        public abstract int HttpStatusCode { get; }
    }
}
