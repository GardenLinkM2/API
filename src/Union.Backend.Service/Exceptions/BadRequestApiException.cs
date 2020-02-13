namespace Union.Backend.Service.Exceptions
{
    public class BadRequestApiException : HttpResponseException
    {
        public override int Status => 400;

        public BadRequestApiException(string message = "Your request cannot be evaluate correctly") 
            : base(message)
        {
        }
    }
}
