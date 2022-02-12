namespace WebServer.Server.Controllers
{
    using System;
    using HTTP;

    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HttpMethodAttribute : Attribute
    {
        protected HttpMethodAttribute(Method method)
            => this.Method = method;

        public Method Method { get; init; }
    }
}
