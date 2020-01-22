using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Service.Exceptions
{
    public class NotFoundApiException : HttpResponseException
    {
        public override int Status => 404;

        public NotFoundApiException(string message = "No entity found for the request")
            : base(message)
        { }
    }
}
