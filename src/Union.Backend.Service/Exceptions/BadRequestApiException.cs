namespace Union.Backend.Service.Exceptions
{
    public class BadRequestApiException : HttpResponseException
    {
        public BadRequestApiException(string message = "Your request cannot be evaluate correctly") 
            : base(message)
        {
        }

        public override int Status => 400;
    }
}
