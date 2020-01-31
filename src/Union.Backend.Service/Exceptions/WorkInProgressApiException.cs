namespace Union.Backend.Service.Exceptions
{
    public class WorkInProgressApiException : HttpResponseException
    {
        public override int Status => 202;

        public WorkInProgressApiException(string message = "Still in development") 
            : base(message)
        { }
    }
}
