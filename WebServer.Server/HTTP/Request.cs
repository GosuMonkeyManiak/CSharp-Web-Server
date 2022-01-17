﻿namespace WebServer.Server.HTTP
{
    using System.Web;

    public class Request
    {
        public Method Method { get; private set; }

        public string Url { get; private set; }

        public HeaderCollection Headers { get; private set; }

        public string Body { get; private set; }

        public IReadOnlyDictionary<string, string> Form { get; private set; }

        public static Request Parse(string request)
        {
            string separator = "\r\n";

            string[] lines = request.Split(separator);

            var startLine = lines.First().Split(" ");

            Method method = ParseMethod(startLine[0]);
            string url = startLine[1];

            HeaderCollection headers = ParseHeaders(lines.Skip(1));

            string[] bodyLines = lines.Skip(headers.Count + 2).ToArray();

            string body = string.Join(separator, bodyLines);

            var form = ParseForm(headers, body);

            return new Request()
            {
                Method = method,
                Url =  url,
                Headers = headers,
                Body = body,
                Form = form
            };
        }

        private static IReadOnlyDictionary<string, string> ParseForm(HeaderCollection headers, string body)
        {
            var formCollection = new Dictionary<string, string>();

            if (headers.Contains(Header.ContentType)
                && headers[Header.ContentType] == ContentType.FormUrlEncoded)
            {
                var parsedResult = ParseFormData(body);

                foreach (KeyValuePair<string, string> keyValuePair in parsedResult)
                {
                    formCollection.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            return formCollection;
        }

        private static Dictionary<string, string> ParseFormData(string bodyLines)
            => HttpUtility.UrlDecode(bodyLines)
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

        private static Method ParseMethod(string method)
        {
            try
            {
                return Enum.Parse<Method>(method);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Method '{method}' is not supported.");
            }
        }
    }
}
