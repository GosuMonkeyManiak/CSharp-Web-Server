﻿namespace WebServer.Server.HTTP
{
    public abstract class ContentResult : ActionResult
    {
        protected ContentResult(Response response, string content, string contentType)
            : base(response)
            => this.SetContent(content, contentType);
    }
}
