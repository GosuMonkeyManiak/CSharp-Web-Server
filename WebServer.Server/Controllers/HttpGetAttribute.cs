namespace WebServer.Server.Controllers
{
    using HTTP;

    public class HttpGetAttribute : HttpMethodAttribute
    {
        public HttpGetAttribute() 
            : base(Method.Get)
        {
        }
    }
}
