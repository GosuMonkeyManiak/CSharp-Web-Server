namespace WebServer.Server.Results
{
    using Common;
    using HTTP;
    using HTTP.Collections;

    public abstract class ActionResult : Response
    {
        protected ActionResult(Response response) 
            : base(response.StatusCode)
        {
            Guard.AgainstNull(response, nameof(response));

            this.PrepareHeaders(response.Headers);
            this.PrepareCookies(response.Cookies);
        }

        private void PrepareHeaders(HeaderCollection headers)
        {
            foreach (var header in headers)
            {
                if (!this.Headers.Contains(header.Name))
                {
                    this.Headers.Add(header.Name, header.Value);
                }
            }
        }

        private void PrepareCookies(CookieCollection cookies)
        {
            foreach (var cookie in cookies)
            {
                if (!this.Cookies.Contains(cookie.Name))
                {
                    this.Cookies.Add(cookie.Name, cookie.Value);
                }
            }
        }
    }
}
