namespace WebServer.Demo.Controllers
{
    using Models;
    using Server.Controllers;
    using Server.HTTP;
    using System.Text;
    using System.Web;

    public class HomeController : Controller
    {
        private const string FileName = "content.txt";

        public HomeController(Request request) 
            : base(request)
        {
        }

        private static async Task DownloadSitesAsTextFile(string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();

            foreach (var url in urls)
            {
                downloads.Add(DownloadWebSiteContentAsync(url));
            }

            var responses = await Task.WhenAll(downloads);

            var responsesString = string.Join(Environment.NewLine + new string('-', 100), responses);

            await System.IO.File.WriteAllTextAsync(fileName, responsesString);
        }

        private static async Task<string> DownloadWebSiteContentAsync(string url)
        {
            var httpClient = new HttpClient();

            using (httpClient)
            {
                var response = await httpClient.GetAsync(url);

                var html = await response.Content.ReadAsStringAsync();

                return html.Substring(0, 2000);
            }
        }
        
        public Response Index() => View();

        public Response Redirect() => Redirect("https://softuni.bg");

        public Response Html() => View();

        public Response HtmlFormPost()
        {
            var name = this.Request.Form["Name"];
            var age = this.Request.Form["Age"];

            var model = new FormViewModel()
            {
                Name = name,
                Age = int.Parse(age)
            };

            return View(model);
        }

        public Response Content() => View();

        public Response DownloadContent()
        {
           DownloadSitesAsTextFile(HomeController.FileName,
                new[] { "https://softuni.bg/", "https://judge.softuni.org/" })
               .Wait();

           return File(HomeController.FileName);
        }

        public Response Cookies()
        {
            if (this.Request.Cookies.Any(c => c.Name != Server.HTTP.Session.SessionCookieName))
            {
                var cookieText = new StringBuilder();
                cookieText.AppendLine("<h1>Cookies</h1>");

                cookieText.AppendLine("<table border=\"1\"><tr><th>Name</th><th>Value</th></tr>");

                foreach (var cookie in Request.Cookies)
                {
                    cookieText.Append("<tr>");
                    cookieText
                        .Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                    cookieText
                        .Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                    cookieText.Append("</tr>");
                }

                cookieText.Append("</table>");

                return Html(cookieText.ToString());
            }
            
            this.Response.Cookies.Add("My-Cookie", "My-Cookie");
            this.Response.Cookies.Add("My-Second-Cookie", "My-Second-Value");

            return Html("<h1>Cookies set!</h1>");
        }

        public Response Session()
        {
            var sessionExists = this.Request.Session.ContainsKey(Server.HTTP.Session.SessionCurrentDateKey);

            if (sessionExists)
            {
                var currentDate = this.Request.Session[Server.HTTP.Session.SessionCurrentDateKey];
                return Text($"Stored date: {currentDate}");
            }

            this.Request.Session[Server.HTTP.Session.SessionCurrentDateKey] = DateTime.UtcNow.ToString();

            return Text("Current date stored!");
        }

        public Response Error() => throw new InvalidOperationException("Invalid");
    }
}
