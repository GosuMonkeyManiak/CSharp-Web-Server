namespace WebServer.Server.Responses
{
    using HTTP;

    public class BadRequestResult : ActionResult
    {
        public BadRequestResult(Response response)
            : base(response)
            => this.StatusCode = StatusCode.BadRequest;
    }
}
