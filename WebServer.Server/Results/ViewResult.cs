namespace WebServer.Server.Results
{
    using System.Text;
    using HTTP;

    public class ViewResult : ContentResult
    {
        private const char PathSeparator = '/';

        public ViewResult(
            Response response, 
            string viewName, 
            string controllerName, 
            object model = null)
            : base(response, string.Empty, ContentType.Html) 
            => this.Body = GetHtml(viewName, controllerName, model);

        private byte[] GetHtml(
            string viewName, 
            string controllerName, 
            object model = null)
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

            if (model != null)
            {
                viewContent = this.PopulateModel(viewContent, model);
            }
            
            var layoutPath = Path.GetFullPath("./Views/_Layout.cshtml");

            var layoutContent = File.ReadAllText(layoutPath);

            layoutContent = layoutContent.Replace("@RenderBody", viewContent);

            return Encoding.UTF8.GetBytes(layoutContent);
        }

        private string PopulateModel(string viewContent, object model)
        {
            var data = model
                .GetType()
                .GetProperties()
                .Select(pr => new
                {
                    pr.Name,
                    Value = pr.GetValue(model)
                });

            foreach (var entry in data)
            {
                viewContent = viewContent.Replace(
                    $"@Model.{entry.Name}",
                    entry.Value.ToString());
            }

            return viewContent;
        }
    }
}
