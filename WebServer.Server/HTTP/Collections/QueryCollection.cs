namespace WebServer.Server.HTTP.Collections
{
    using Common;
    using Contracts;

    public class QueryCollection : IQueryCollection
    {
        private readonly Dictionary<string, string> query;

        public QueryCollection()
            => this.query = new Dictionary<string, string>();

        public string this[string key]
            => this.query[key];

        public void Add(string key, string value)
        {
            Guard.AgainstNull(key, nameof(key));
            Guard.AgainstNull(value, nameof(value));

            this.query[key] = value;
        }

    }
}
