namespace WebServer.Server.Responses
{
    using HTTP;

    public class TextResponse : ContentResponse
    {
        public TextResponse(string content) 
            : base(content, ContentType.PlainText)
        {
        }
    }
}
