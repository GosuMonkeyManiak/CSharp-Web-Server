namespace WebServer.Demo.Controllers
{
    using Models;
    using Server.Controllers;
    using Server.HTTP;

    public class UsersController : Controller
    {
        private const string Username = "user";
        private const string Password = "user123";

        public Response Index()
            => View();

        public Response Login()
        {
            if (this.Request.Session.ContainsKey(Session.SessionUserKey))
            {
                return Redirect($"/UserProfile");
            }

            return View();
        }

        [HttpPost]
        public Response LogInUser()
        {
            var userName = this.Request.Form["Username"];
            var password = this.Request.Form["Password"];

            var usernameMatches = userName == UsersController.Username;
            var passwordMatches = password == UsersController.Password;

            if (usernameMatches && passwordMatches)
            {
                this.Request.Session[Session.SessionUserKey] = "MyUserId";
                this.Request.Session[Session.SessionCurrentDateKey] = DateTime.UtcNow.ToString();

                return View();
            }

            return Redirect($"/{nameof(Login)}");
        }

        [Authorize]
        public Response Logout()
        { 
            this.Request.Session.Remove(Session.SessionUserKey);

            return View();
        }

        [Authorize]
        public Response GetUserData()
        {
            var userProfileViewModel = new UserProfileViewModel()
            {
                UserName = Username,
                DateTime = this.Request.Session[Session.SessionCurrentDateKey]
            };

            return View(userProfileViewModel);
        }

        [Authorize]
        public Response AuthorizeCheck()
        {
            return View();
        }

    }
}
