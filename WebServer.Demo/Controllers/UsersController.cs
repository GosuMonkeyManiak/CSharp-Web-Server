﻿namespace WebServer.Demo.Controllers
{
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

        public Response Login() => View();

        public Response LogInUser()
        {
            this.Request.Session.Clear();

            var usernameMatches = this.Request.Form["Username"] == UsersController.Username;
            var passwordMatches = this.Request.Form["Password"] == UsersController.Password;

            if (usernameMatches && passwordMatches)
            {
                if (!this.Request.Session.ContainsKey(Session.SessionUserKey))
                {
                    this.Request.Session[Session.SessionUserKey] = "MyUserId";

                    var cookieCollection = new CookieCollection();
                    cookieCollection.Add(Session.SessionCookieName, this.Request.Session.Id);

                    return Html("<h3>Logged successfully!</h3>", cookieCollection);
                }

                return Html("<h3>Logged successfully!</h3>");
            }

            return Redirect("/Login");
        }

        public Response Logout()
        {
            this.Request.Session.Clear();

            return Html("<h3>Logged out successfully!</h3>");
        }

        public Response GetUserData()
        {
            if (this.Request.Session.ContainsKey(Session.SessionUserKey))
            {
                return Html(
                    $"<h3>Current logged-in user is with username '{UsersController.Username}' and {this.Request.Session[Session.SessionUserKey]}</h3>");
            }

            return Redirect("/Login");
        }
    }
}
