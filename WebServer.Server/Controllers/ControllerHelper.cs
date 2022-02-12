namespace WebServer.Server.Controllers
{
    public static class ControllerHelper
    {
        public static string GetControllerName(this Type type)
            => type.Name.Replace(nameof(Controller), "");
    }
}
