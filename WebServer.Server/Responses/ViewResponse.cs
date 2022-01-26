namespace WebServer.Server.Responses
{
    using HTTP;

    public class ViewResponse : ContentResponse
    {
        private const char PathSeparator = '/';

        public ViewResponse(string viewName, string controllerName)
            : base(string.Empty, ContentType.Html)
        {
            if (!viewName.Contains(PathSeparator))
            {
                viewName = controllerName + PathSeparator + viewName;
            }

            var viewPath = Path.GetFullPath(
                $"./Views/" +
                viewName.TrimStart(PathSeparator)
                + ".cshtml");

            var viewContent = File.ReadAllText(viewPath);

            this.Body = viewContent;
        }
    }
}
