﻿namespace WebServer.Demo
{
    using Data;
    using Server;
    using Server.Controllers;
    using Server.Results.ViewEngine;

    public class StartUp
    {
        public static async Task Main(string[] args)
            => await HttpServer
                .WithRoutes(routes => routes
                    .MapStaticFiles()
                    .MapControllers())
                .WithServices(services => services
                    .Add<IData, DbContextData>()
                    .Add<IViewEngine, ViewEngine>())
                .Start();
    }
}