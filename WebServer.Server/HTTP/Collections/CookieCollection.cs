namespace WebServer.Server.HTTP.Collections
{
    using System.Collections;

    public class CookieCollection : IEnumerable<Cookie>
    {
        private readonly Dictionary<string, Cookie> cookies;

        public CookieCollection()
            => this.cookies = new();

        public string this[string name]
            => this.cookies[name].Value;

        public int Count => this.cookies.Count;

        public void Add(string name, string value)
            => this.cookies[name] = new Cookie(name, value);

        public bool Contains(string name)
            => this.cookies.ContainsKey(name);

        public IEnumerator<Cookie> GetEnumerator()
            => this.cookies.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
