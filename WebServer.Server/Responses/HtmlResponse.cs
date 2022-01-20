namespace WebServer.Server.Responses
{
    using HTTP;

    public class HtmlResponse : ContentResponse
    {
        public HtmlResponse(string content, Action<Request, Response> preRenderAction = null) 
            : base(content, ContentType.Html, preRenderAction)
        {
        }
    }
}
