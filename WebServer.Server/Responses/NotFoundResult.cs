namespace WebServer.Server.Responses
{
    using HTTP;

    public class NotFoundResult : ActionResult
    {
        public NotFoundResult(Response response)
            : base(response)
            => this.StatusCode = StatusCode.Found;
    }
}
