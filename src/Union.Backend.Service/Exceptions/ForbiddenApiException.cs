namespace Union.Backend.Service.Exceptions
{
    public class ForbiddenApiException : HttpResponseException
    {
        public ForbiddenApiException(string message = "Forbidden request")
            : base(message)
        {
        }

        public override int Status => 403;
    }
}
