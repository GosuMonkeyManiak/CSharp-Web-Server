namespace WebServer.Demo.Controllers
{
    using Server.Controllers;
    using Server.HTTP;

    public class HomeController : Controller
    {
        public HomeController(Request request) 
            : base(request)
        {
        }

        public Response Index() => Text("Hello from the server!");
    }
}
