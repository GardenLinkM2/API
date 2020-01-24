using System;

namespace Union.Backend.Service.Exceptions
{
    public abstract class HttpResponseException : Exception
    {
        public abstract int Status { get; }
        public HttpResponseException(string message) : base(message) { }
    }
}
