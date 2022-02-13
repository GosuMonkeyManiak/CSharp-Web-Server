namespace WebServer.Demo
{
    using Data;
    using Server;
    using Server.Controllers;
    using Server.Controllers.Contracts;
    using Server.Results.ViewEngine;

    public class StartUp
    {
        public static async Task Main(string[] args)
            => await HttpServer
                .WithRoutes(routes => routes
                    .MapStaticFiles()
                    .MapControllers())
                .WithServices(services => services
                    .Add<IViewEngine, ViewEngine>()
                    .Add<IModelState, ModelState>()
                    .Add<IData, DbContextData>())
                .Start();
    }
}