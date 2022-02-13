namespace WebServer.Server.Identity
{
    public interface IUserIdentity
    {
        string Id { get; init; }

        bool IsAuthenticated { get; }
    }
}
