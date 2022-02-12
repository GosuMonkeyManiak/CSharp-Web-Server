namespace WebServer.Server.Controllers
{

    [AttributeUsage(AttributeTargets.Property)]
    public class NeverBindAttribute : Attribute
    {
    }
}
