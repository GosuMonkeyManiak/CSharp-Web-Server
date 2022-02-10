namespace WebServer.Server.HTTP
{
    using System.Text;
    using System.Web;
    using Collections;
    using Collections.Contracts;

    public class Request
    {
        private static Dictionary<string, Session> Sessions = new();

        public Method Method { get; init; }

        public string Url { get; init; }

        public QueryCollection Query { get; init; }

        public HeaderCollection Headers { get; init; }

        public CookieCollection Cookies { get; init; }

        public string Body { get; init; }

        public Session Session { get; init; }

        public FormCollection Form { get; init; }

        public static Request Parse(string request)
        {
            string separator = "\r\n";

            string[] lines = request.Split(separator);

            var startLine = lines.First().Split(" ");

            Method method = ParseMethod(startLine[0]);

            var (url, query) = ParseUrl(startLine[1]);

            HeaderCollection headers = ParseHeaders(lines.Skip(1));

            CookieCollection cookies = ParseCookies(headers);

            Session session = GetSession(cookies);

            string[] bodyLines = lines.Skip(headers.Count + 2).ToArray();

            string body = string.Join(separator, bodyLines);

            var form = ParseForm(headers, body);

            return new Request()
            {
                Method = method,
                Url =  url,
                Query = query,
                Headers = headers,
                Cookies = cookies,
                Body = body,
                Session = session,
                Form = form
            };
        }
        private static Method ParseMethod(string method)
        {
            try
            {
                return Enum.Parse<Method>(method, true);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Method '{method}' is not supported.");
            }
        }

        private static (string, QueryCollection) ParseUrl(string fullUrl)
        {
            var urlParts = fullUrl.Split('?', 2);

            var url = urlParts[0];

            var query = urlParts.Length == 2
                ? ParseQuery<QueryCollection>(urlParts[1])
                : new QueryCollection();

            return (url, query);
        }

        private static TCollection ParseQuery<TCollection>(string queryString)
            where TCollection : IQueryCollection
        {
            var keyValues = HttpUtility
                .UrlDecode(queryString)
                .Split('&')
                .Select(part => part.Split('='))
                .Where(part => part.Length == 2)
                .Select(part => new KeyValuePair<string, string>(part[0], part[1]));

            var queryCollection = Activator.CreateInstance<TCollection>();

            foreach (var (key, value) in keyValues)
            {
                queryCollection.Add(key, value);
            }

            return queryCollection;
        }

        private static HeaderCollection ParseHeaders(IEnumerable<string> HeaderLines)
        {
            var headerCollection = new HeaderCollection();

            foreach (string headerLine in HeaderLines)
            {
                if (headerLine == string.Empty)
                {
                    break;
                }

                var headerParts = headerLine.Split(":", 2);

                if (headerParts.Length != 2)
                {
                    throw new InvalidOperationException("Request is no valid.");
                }

                string headerName = headerParts[0];
                string headerValue = headerParts[1].Trim();

                headerCollection.Add(headerName, headerValue);
            }

            return headerCollection;
        }

        private static CookieCollection ParseCookies(HeaderCollection headers)
        {
            var cookieCollection = new CookieCollection();

            if (headers.Contains(Header.Cookie))
            {
                var cookieHeader = headers[Header.Cookie];

                var allCookies = cookieHeader.Split(';');

                foreach (var cookieText in allCookies)
                {
                    var cookieParts = cookieText.Split('=', 2);

                    var cookieName = cookieParts[0].Trim();
                    var cookieValue = cookieParts[1].Trim();

                    cookieCollection.Add(cookieName, cookieValue);
                }
            }

            return cookieCollection;
        }

        private static Session GetSession(CookieCollection cookies)
        {
            var sessionId = cookies.Contains(Session.SessionCookieName)
                ? cookies[Session.SessionCookieName]
                : Guid.NewGuid().ToString();

            if (!Sessions.ContainsKey(sessionId))
            {
                Sessions[sessionId] = new Session(sessionId);
            }

            return Sessions[sessionId];
        }

        private static FormCollection ParseForm(HeaderCollection headers, string body)
        {
            if (headers.Contains(Header.ContentType)
                && headers[Header.ContentType] == ContentType.FormUrlEncoded)
            {
                return ParseQuery<FormCollection>(body);
            }

            return new FormCollection();
        }

        public override string ToString()
        {
            var request = new StringBuilder();

            request.AppendLine($"{this.Method} {this.Url} HTTP/1.1");

            foreach (var header in this.Headers)
            {
                request.AppendLine(header.ToString());
            }

            if (this.Body != null)
            {
                request.AppendLine();

                request.Append(this.Body);
            }

            return request.ToString();
        }
    }
}
