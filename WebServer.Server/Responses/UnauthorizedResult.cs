namespace WebServer.Server.Responses
{
    using HTTP;

    public class UnauthorizedResult : ActionResult
    {
        public UnauthorizedResult(Response response)
            : base(response)
            => this.StatusCode = StatusCode.Unauthorized;
    }
}
