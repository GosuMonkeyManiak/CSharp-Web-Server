namespace WebServer.Server.Routing
{
    using HTTP;
    using System;
    using Common;
    using Responses;

    public class RoutingTable : IRoutingTable
    {
        private readonly Dictionary<Method,Dictionary<string, Func<Request, Response>>> routes;

        public RoutingTable() => this.routes = new()
        {
            [Method.Get] = new(StringComparer.InvariantCultureIgnoreCase),
            [Method.Post] = new(StringComparer.InvariantCultureIgnoreCase),
            [Method.Put] = new(StringComparer.InvariantCultureIgnoreCase),
            [Method.Delete] = new(StringComparer.InvariantCultureIgnoreCase)
        };

        public IRoutingTable Map(
            Method method,
            string path,
            Func<Request, Response> responseFunction)
        {
            Guard.AgainstNull(path, nameof(path));
            Guard.AgainstNull(responseFunction, nameof(responseFunction));

            this.routes[method][path] = responseFunction;

            return this;
        }

        public IRoutingTable MapGet(string path, Func<Request, Response> responseFunction)
            => Map(Method.Get, path, responseFunction);

        public IRoutingTable MapPost(string path, Func<Request, Response> responseFunction)
            => Map(Method.Post, path, responseFunction);


        public Response ExecuteRequest(Request request)
        {
            Method requestMethod = request.Method;
            string requestUrl = request.Url;

            if (!this.routes.ContainsKey(requestMethod) 
                || !this.routes[requestMethod].ContainsKey(requestUrl))
            {
                return new Response(StatusCode.NotFound);
            }

            return this.routes[requestMethod][requestUrl](request);
        }
    }
}
