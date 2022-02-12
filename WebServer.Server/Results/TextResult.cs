namespace WebServer.Server.Results
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
