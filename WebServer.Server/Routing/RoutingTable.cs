namespace WebServer.Server.Routing
{
    using HTTP;
    using System;
    using Common;

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
        public IRoutingTable MapStaticFiles(string folder = Settings.StaticFilesRootFolder)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var staticFileFolder = Path.Combine(currentDirectory, folder);

            if (!Directory.Exists(staticFileFolder))
            {
                return this;
            }

            var staticFiles = Directory
                .GetFiles(staticFileFolder, "*.*", SearchOption.AllDirectories);

            foreach (var file in staticFiles)
            {
                var relativePath = Path.GetRelativePath(staticFileFolder, file);

                var urlPath = "/" + relativePath.Replace("\\", "/");

                this.MapGet(urlPath, request =>
                {
                    var fileExtension = Path.GetExtension(urlPath).TrimStart('.');

                    var content = File.ReadAllBytes(file);
                    var contentType = ContentType.GetTypeByFileExtension(fileExtension);

                    return new Response(StatusCode.OK)
                        .SetContent(content, contentType);
                });
            }

            return this;
        }
    }
}
