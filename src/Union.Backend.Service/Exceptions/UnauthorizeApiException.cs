namespace Union.Backend.Service.Exceptions
{
    public class UnauthorizeApiException : HttpResponseException
    {
        public UnauthorizeApiException(string message = "You are not allowed to do this") : base(message)
        {
        }

        public override int Status => 401;
    }
}
