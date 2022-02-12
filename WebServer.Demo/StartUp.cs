namespace WebServer.Demo
{
    using Controllers;
    using Server;
    using Server.Controllers;

    public class StartUp
    {
        public static async Task Main(string[] args)
            => await new HttpServer(routes => routes
                    .MapStaticFiles()
                    .MapControllers())
                    //.MapGet<HomeController>("/", c => c.Index())
                    //.MapGet<HomeController>("/Redirect", c => c.Redirect())
                    //.MapGet<HomeController>("/PowerPoint", c => c.PowerPoint())
                    //.MapGet<HomeController>("/Word", c => c.Word())
                    //.MapGet<HomeController>("/Excel", c => c.Excel())
                    //.MapGet<HomeController>("/HTML", c => c.Html())
                    //.MapPost<HomeController>("/HTML", c => c.HtmlFormPost())
                    //.MapGet<HomeController>("/Content", c => c.Content())
                    //.MapPost<HomeController>("/Content", c => c.DownloadContent())
                    //.MapGet<HomeController>("/Cookies", c => c.Cookies())
                    //.MapGet<HomeController>("/Session", c => c.Session())
                    //.MapGet<HomeController>("/Error", c => c.Error())
                    //.MapGet<UsersController>("/Login", c => c.Login())
                    //.MapPost<UsersController>("/Login", c => c.LogInUser())
                    //.MapGet<UsersController>("/Logout", c => c.Logout())
                    //.MapGet<UsersController>("/UserProfile", c => c.GetUserData()))
                .Start();
    }
}