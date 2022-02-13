namespace WebServer.Server.Results.ViewEngine
{
    public interface IView
    {
        string ExecuteTemplate(object viewModel, object user);
    }
}