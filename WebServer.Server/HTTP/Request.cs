namespace WebServer.Server.HTTP
{
    using System.Web;

    public class Request
    {
        private static Dictionary<string, Session> Sessions = new();

        public Method Method { get; init; }

        public string Url { get; init; }

        public IReadOnlyDictionary<string, string> Query { get; init; }

        public HeaderCollection Headers { get; init; }

        public CookieCollection Cookies { get; init; }

        public string Body { get; init; }

        public Session Session { get; init; }

        public IReadOnlyDictionary<string, string> Form { get; init; }

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

        private static (string, Dictionary<string, string>) ParseUrl(string fullUrl)
        {
            var urlParts = fullUrl.Split('?', 2);

            var url = urlParts[0];

            var query = urlParts.Length == 2
                ? ParseQuery(urlParts[1])
                : new Dictionary<string, string>();

            return (url, query);
        }

        private static Dictionary<string, string> ParseQuery(string queryString)
            => HttpUtility.UrlDecode(queryString)
                .Split('&')
                .Select(part => part.Split('='))
                .Where(part => part.Length == 2)
                .ToDictionary(
                    part => part[0],
                    part => part[1],
                    StringComparer.InvariantCultureIgnoreCase);

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

        private static IReadOnlyDictionary<string, string> ParseForm(HeaderCollection headers, string body)
        {
            var formCollection = new Dictionary<string, string>();

            if (headers.Contains(Header.ContentType)
                && headers[Header.ContentType] == ContentType.FormUrlEncoded)
            {
                var parsedResult = ParseQuery(body);

                foreach (KeyValuePair<string, string> keyValuePair in parsedResult)
                {
                    formCollection.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            return formCollection;
        }
    }
}
