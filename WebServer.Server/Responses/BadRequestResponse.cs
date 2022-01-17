namespace WebServer.Server.Responses
{
    using HTTP;

    public class BadRequestResponse : Response
    {
        public BadRequestResponse() 
            : base(StatusCode.BadRequest)
        {
        }
    }
}
