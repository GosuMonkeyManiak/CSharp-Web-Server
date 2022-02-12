namespace WebServer.Demo.Controllers
{
    using Models;
    using Server.Controllers;
    using Server.HTTP;
    using System.Text;
    using System.Web;

    public class HomeController : Controller
    {
        public Response Index() 
            => View();

        public Response Redirect() 
            => Redirect("https://softuni.bg");

        public Response Html() 
            => View();

        [HttpPost]
        public Response Html(FormViewModel formModel) 
            => View(formModel, "HtmlFormPost");

        public Response Content() 
            => View();

        [HttpPost]
        public Response DownloadContent() 
            => File("test.pdf", Header.InlineFile);

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

            return View();
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

        public Response PowerPoint()
            => File("testPowerPoint.pptx");

        public Response Word()
            => File("TestWord.docx");

        public Response Excel()
            => File("TestExcel.xlsx");
    }
}
