namespace WebServer.Server.Results
{
    using HTTP;

    public class UnauthorizedResult : ActionResult
    {
        public UnauthorizedResult(Response response)
            : base(response)
            => this.StatusCode = StatusCode.Unauthorized;
    }
}
