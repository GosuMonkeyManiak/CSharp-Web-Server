namespace WebServer.Server.Controllers.Contracts
{
    public interface IModelState
    {
        IList<string> Errors { get; }

        bool IsValid { get; }
    }
}