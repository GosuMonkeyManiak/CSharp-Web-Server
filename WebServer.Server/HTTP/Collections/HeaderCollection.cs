namespace WebServer.Server.HTTP.Collections
{
    using System.Collections;

    public class HeaderCollection : IEnumerable<Header>
    {
        private readonly Dictionary<string, Header> headers;

        public HeaderCollection() 
            => this.headers = new();

        public string this[string name]
            => this.headers[name].Value;

        public int Count => this.headers.Count;

        public void Add(string name, string value)
            => this.headers[name] = new Header(name, value);

        public bool Contains(string name)
            => this.headers.ContainsKey(name);

        public IEnumerator<Header> GetEnumerator() => this.headers.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
