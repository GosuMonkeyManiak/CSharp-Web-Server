namespace WebServer.Server.Responses
{
    using HTTP;

    public class UnauthorizedResponse : Response
    {
        public UnauthorizedResponse()
            : base(StatusCode.Unauthorized)
        {
            
        }
    }
}
