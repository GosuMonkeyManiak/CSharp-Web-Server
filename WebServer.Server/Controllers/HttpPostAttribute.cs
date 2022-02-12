namespace WebServer.Server.Controllers
{
    using HTTP;

    public class HttpPostAttribute : HttpMethodAttribute
    {
        public HttpPostAttribute() 
            : base(Method.Post)
        {
        }
    }
}
