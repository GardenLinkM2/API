namespace Union.Backend.Service.Exceptions
{
    public class WorkInProgressApiException : HttpResponseException
    {
        public WorkInProgressApiException(string message = "Still in development") 
            : base(message)
        { }

        public override int Status => 202;
    }
}
