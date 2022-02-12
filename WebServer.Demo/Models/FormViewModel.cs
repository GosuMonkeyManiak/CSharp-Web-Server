namespace WebServer.Demo.Models
{
    using Server.Controllers;

    public class FormViewModel
    {
        public string Name { get; init; }

        public int Age { get; init; }

        [NeverBind]
        public string Never { get; set; }
    }
}
