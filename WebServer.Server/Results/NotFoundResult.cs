namespace WebServer.Server.Results
{
    using HTTP;

    public class NotFoundResult : ActionResult
    {
        public NotFoundResult(Response response)
            : base(response)
            => this.StatusCode = StatusCode.Found;
    }
}
