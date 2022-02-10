namespace WebServer.Server.Responses
{
    using HTTP;

    public class HtmlResult : ContentResult
    {
        public HtmlResult(Response response, string content) 
            : base(response, content, ContentType.Html)
        {
        }
    }
}
