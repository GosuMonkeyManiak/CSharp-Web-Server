namespace WebServer.Server.Identity
{
    public class UserIdentity : IUserIdentity
    {
        public string Id { get; init;}

        public bool IsAuthenticated => this.Id != null;
    }
}
