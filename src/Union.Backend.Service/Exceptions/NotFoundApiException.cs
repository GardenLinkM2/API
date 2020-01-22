using System;

namespace Union.Backend.Service.Exceptions
{
    public class NotFoundApiException : Exception
    {
        public NotFoundApiException(string message = "No entity found for the request")
            : base(message)
        { }
    }
}
