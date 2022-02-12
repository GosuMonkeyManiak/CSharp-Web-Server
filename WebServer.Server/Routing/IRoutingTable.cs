namespace WebServer.Server.Routing
{
    using Common;
    using HTTP;

    public interface IRoutingTable
    {
        IRoutingTable MapStaticFiles(string folder = Settings.StaticFilesRootFolder);

        IRoutingTable Map(
            Method method,
            string path,
            Func<Request, Response> responseFunction);

        IRoutingTable MapGet(string path, Func<Request, Response> responseFunction);

        IRoutingTable MapPost(string path, Func<Request, Response> responseFunction);
    }
}
