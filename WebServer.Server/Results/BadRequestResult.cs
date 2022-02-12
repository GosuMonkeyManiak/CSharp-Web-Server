namespace WebServer.Server.Results
{
    using HTTP;

    public class BadRequestResult : ActionResult
    {
        public BadRequestResult(Response response)
            : base(response)
            => this.StatusCode = StatusCode.BadRequest;
    }
}
