namespace WebServer.Demo.Data
{
    public class DbContextData : IData
    {
        public IEnumerable<string> GetNames()
            => new List<string>()
            {
                "Ivan",
                "Dimitar",
                "Pesho"
            };
    }
}
