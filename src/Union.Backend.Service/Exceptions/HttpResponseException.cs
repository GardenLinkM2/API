using System;

namespace Union.Backend.Service.Exceptions
{
    public abstract class HttpResponseException : Exception
    {
        public HttpResponseException(string message) : base(message) { }
        public abstract int Status { get; }
    }
}
