namespace WebServer.Server.Responses
{
    using HTTP;

    public class RedirectResult : ActionResult
    {
        public RedirectResult(Response response, string location) 
            : base(response)
        {
            this.StatusCode = StatusCode.Found;
            this.Headers.Add(Header.Location, location);
        }
    }
}
