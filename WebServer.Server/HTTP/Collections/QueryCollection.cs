namespace WebServer.Server.HTTP.Collections
{
    using Common;
    using Contracts;

    public class QueryCollection : IQueryCollection
    {
        private readonly Dictionary<string, string> query;

        public QueryCollection()
            => this.query = new(StringComparer.InvariantCultureIgnoreCase);

        public string this[string key]
            => this.query[key];

        public void Add(string key, string value)
        {
            Guard.AgainstNull(key, nameof(key));
            Guard.AgainstNull(value, nameof(value));

            this.query[key] = value;
        }

        public bool Contains(string key)
            => this.query.ContainsKey(key);

        public string GetValueOrDefault(string key)
            => this.query.GetValueOrDefault(key);
    }
}
