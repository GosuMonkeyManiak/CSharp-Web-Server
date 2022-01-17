namespace WebServer.Server.Responses
{
    using HTTP;

    public class NotFoundResponse : Response
    {
        public NotFoundResponse() 
            : base(StatusCode.NotFound)
        {
        }
    }
}
