namespace WebServer.Server.HTTP.Collections.Contracts
{
    public interface IQueryCollection
    {
        string this[string key] { get; }

        void Add(string key, string value);
    }
}
