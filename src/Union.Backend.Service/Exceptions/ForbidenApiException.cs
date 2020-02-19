namespace Union.Backend.Service.Exceptions
{
    public class ForbidenException : HttpResponseException
    {
        public ForbidenException(string message = "Forbiden request")
            : base(message)
        {
        }

        public override int Status => 403;
    }
}
