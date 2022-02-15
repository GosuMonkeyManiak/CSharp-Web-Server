namespace WebServer.Server.HTTP
{
    using Collections;
    using System.Text;
    using Common;

    public class Response
    {
        public Response(StatusCode statusCode)
        {
            this.StatusCode = statusCode;

            this.Headers = new HeaderCollection();
            this.Cookies = new CookieCollection();

            this.Headers.Add(Header.Server, "My Web Server");
            this.Headers.Add(Header.Date, $"{DateTime.UtcNow:r}");
        }

        public StatusCode StatusCode { get; protected set; }

        public HeaderCollection Headers { get; init; }

        public CookieCollection Cookies { get; init; }

        public byte[] Body { get; protected set; }

        public static Response ForError(string message)
            => new Response(StatusCode.InternalServerError)
                .SetContent(message, ContentType.PlainText);

        public Response SetContent(string content, string contentType)
        {
            var contentBytes = Encoding.UTF8.GetBytes(content);
            return this.SetContent(contentBytes, contentType);
        }

        public Response SetContent(byte[] content, string contentType)
        {
            Guard.AgainstNull(content, nameof(content));
            Guard.AgainstNull(contentType, nameof(contentType));

            var contentLength = content.Length;

            this.Headers.Add(Header.ContentType, contentType);
            this.Headers.Add(Header.ContentLength, contentLength.ToString());

            this.Body = content;

            return this;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine($"HTTP/1.1 {(int)this.StatusCode} {this.StatusCode}");

            foreach (var header in this.Headers)
            {
                result.AppendLine(header.ToString());
            }

            foreach (var cookie in Cookies)
            {
                result.AppendLine($"{Header.SetCookie}: {cookie}");
            }

            if (this.Body != null)
            {
                result.AppendLine();
            }

            return result.ToString();
        }
    }
}
