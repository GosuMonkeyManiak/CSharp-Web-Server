namespace WebServer.Server.Responses
{
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

            this.Body = viewContent;
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
                const string openingBrackets = "{{";
                const string closingBrackets = "}}";

                viewContent = viewContent.Replace(
                    $"{openingBrackets}{entry.Name}{closingBrackets}",
                    entry.Value.ToString());
            }

            return viewContent;
        }
    }
}
