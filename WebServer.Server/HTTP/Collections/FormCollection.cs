namespace WebServer.Server.HTTP.Collections
{
    using Common;
    using Contracts;

    public class FormCollection : IQueryCollection
    {
        private readonly Dictionary<string, string> form;

        public FormCollection()
            => this.form = new(StringComparer.InvariantCultureIgnoreCase);

        public string this[string key]
            => this.form[key];

        public void Add(string key, string value)
        {
            Guard.AgainstNull(key, nameof(key));
            Guard.AgainstNull(value, nameof(value));

            this.form[key] = value;
        }

        public bool Contains(string key)
            => this.form.ContainsKey(key);

        public string GetValueOrDefault(string key)
            => this.form.GetValueOrDefault(key);
    }
}
