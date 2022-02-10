namespace WebServer.Demo.Controllers
{
    using Models;
    using Server.Controllers;
    using Server.HTTP;
    using Server.HTTP.Collections;

    public class UsersController : Controller
    {
        private const string Username = "user";
        private const string Password = "user123";

        public UsersController(Request request) 
            : base(request)
        {
        }

        public Response Login()
        {
            if (this.Request.Session.ContainsKey(Session.SessionUserKey))
            {
                return Redirect($"/UserProfile");
            }

            return View();
        }

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

        public Response Logout()
        {
            if (!this.Request.Session.ContainsKey(Session.SessionUserKey))
            {
                return Redirect($"/{nameof(Login)}");
            }

            this.Request.Session.Remove(Session.SessionUserKey);

            return View();
        }

        public Response GetUserData()
        {
            if (this.Request.Session.ContainsKey(Session.SessionUserKey))
            {
                var userProfileViewModel = new UserProfileViewModel()
                {
                    UserName = Username,
                    DateTime = this.Request.Session[Session.SessionCurrentDateKey]
                };

                return View(userProfileViewModel);
            }

            return Redirect($"/{nameof(Login)}");
        }
    }
}
