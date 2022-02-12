namespace WebServer.Demo
{
    using Server;
    using Server.Controllers;

    public class StartUp
    {
        public static async Task Main(string[] args)
            => await new HttpServer(routes => routes
                    .MapStaticFiles()
                    .MapControllers())
                .Start();
    }
}