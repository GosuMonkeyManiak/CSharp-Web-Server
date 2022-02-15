namespace WebServer.Server.Results
{
    using System.Text;
    using HTTP;
    using Identity;
    using ViewEngine;

    public class ViewResult : ActionResult
    {
        private const char PathSeparator = '/';
        private const string RenderBody = "@RenderBody";
        private const string ReplaceHelper = "___View___Is___Here___";

        private readonly IViewEngine viewEngine;
        private readonly IUserIdentity user;

        public ViewResult(
            Response response,
            IViewEngine viewEngine,
            IUserIdentity user,
            string viewName,
            string controllerName,
            object viewModel = null)
            : base(response)
        {
            this.viewEngine = viewEngine;
            this.user = user;

            byte[] content = GetHtml(viewName, controllerName, viewModel);

            this.SetContent(content, ContentType.Html);
        }

        private byte[] GetHtml(
            string viewName, 
            string controllerName,
            object viewModel = null)
        {
            if (!viewName.Contains(PathSeparator))
            {
                viewName = controllerName + PathSeparator + viewName;
            }

            var layoutPath = Path.GetFullPath("./Views/_Layout.cshtml");

            var viewPath = Path.GetFullPath(
                $"./Views/" +
                viewName.TrimStart(PathSeparator)
                + ".cshtml");

            var layoutTemplate = File.ReadAllText(layoutPath)
                .Replace(RenderBody, ReplaceHelper);

            var viewTemplate = File.ReadAllText(viewPath);

            var layoutContent = this.viewEngine.RenderHtml(layoutTemplate, viewModel, this.user);

            var viewContent = this.viewEngine.RenderHtml(viewTemplate, viewModel, this.user);

            layoutContent = layoutContent
                .Replace(ReplaceHelper, viewContent);

            return Encoding.UTF8.GetBytes(layoutContent);
        }
    }
}
