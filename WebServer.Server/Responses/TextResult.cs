namespace WebServer.Server.Responses
{
    using HTTP;

    public class TextResult : ContentResult
    {
        public TextResult(Response response, string content) 
            : base(response ,content, ContentType.PlainText)
        {
        }
    }
}
