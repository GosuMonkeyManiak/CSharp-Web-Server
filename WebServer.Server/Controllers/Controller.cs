namespace WebServer.Server.Controllers
{
    using HTTP;
    using Responses;
    using System.Runtime.CompilerServices;

    public abstract class Controller
    {
        protected Controller(Request request)
        {
            this.Request = request;
            this.Response = new Response(StatusCode.OK);
        }

        protected Request Request { get; private init; }

        protected Response Response { get; private init; }

        protected Response Text(string text) 
            => new TextResult(this.Response, text);

        protected Response Html(string html)
            => new HtmlResult(this.Response, html);

        protected Response BadRequest() 
            => new BadRequestResult(this.Response);

        protected Response Unauthorized() 
            => new UnauthorizedResult(this.Response);

        protected Response NotFound() 
            => new NotFoundResult(this.Response);

        protected Response Redirect(string location) 
            => new RedirectResult(this.Response, location);

        protected Response File(string fileName) 
            => new TextFileResult(this.Response, fileName);

        protected Response View([CallerMemberName] string viewName = "")
            => new ViewResult(this.Response, viewName, this.GetControllerName());

        protected Response View(object model, [CallerMemberName] string viewName = "")
            => new ViewResult(this.Response, viewName, this.GetControllerName(), model);

        private string GetControllerName()
            => this.GetType().Name
                .Replace(nameof(Controller), "");
    }
}
