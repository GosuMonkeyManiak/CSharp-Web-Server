namespace WebServer.Server.Controllers
{
    using HTTP;
    using Results;
    using System.Runtime.CompilerServices;
    using Contracts;
    using Identity;
    using Results.ViewEngine;

    public abstract class Controller
    {
        private UserIdentity user;
        private IViewEngine viewEngine;

        protected Controller() 
            => this.Response = new Response(StatusCode.OK);

        protected Request Request { get; private init; }

        protected Response Response { get; private init; }

        protected UserIdentity User
        {
            get
            {
                if (this.user == null)
                {
                    this.user = this.Request.Session.ContainsKey(Session.SessionUserKey)
                        ? this.user = new() { Id = this.Request.Session[Session.SessionUserKey] }
                        : this.user = new();
                }

                return this.user;
            }
            private set => this.user = value;
        }

        protected IViewEngine ViewEngine
        {
            get
            {
                if (this.viewEngine == null)
                {
                    this.viewEngine = this.Request.Services.GetService<IViewEngine>();
                }

                return this.viewEngine;
            }
        }

        protected IModelState ModelState { get; private init; }

        protected void SignIn(string userId)
        {
            this.Request.Session[Session.SessionUserKey] = userId;
            this.User = new()
            {
                Id = userId
            };
        }

        protected void SignOut()
        {
            this.Request.Session.Remove(Session.SessionUserKey);
            this.User = new();
        }

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

        protected Response File(string fileName, string disposition = "") 
            => new TextFileResult(this.Response, fileName, disposition);

        protected Response View([CallerMemberName] string viewName = "")
            => new ViewResult(
                this.Response, 
                this.ViewEngine, 
                this.User, 
                viewName, 
                this.GetType().GetControllerName());

        protected Response View(object model, [CallerMemberName] string viewName = "")
            => new ViewResult(
                this.Response, 
                this.ViewEngine, 
                this.User, 
                viewName, 
                this.GetType().GetControllerName(), 
                model);
    }
}
