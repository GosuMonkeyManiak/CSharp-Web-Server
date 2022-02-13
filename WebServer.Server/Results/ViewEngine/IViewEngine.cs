namespace WebServer.Server.Results.ViewEngine
{
    using Identity;

    public interface IViewEngine
    {
        string RenderHtml(string content, object viewModel, IUserIdentity user);
    }
}
