namespace Union.Backend.Service.Exceptions
{
    public class ForbidenApiException : HttpResponseException
    {
        public ForbidenApiException(string message = "Forbiden request")
            : base(message)
        {
        }

        public override int Status => 403;
    }
}
