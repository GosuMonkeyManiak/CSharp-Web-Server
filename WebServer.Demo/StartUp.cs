﻿namespace WebServer.Demo
{
    using Server;
    using Server.HTTP;
    using Server.Responses;

    public class StartUp
    {
        private const string HtmlForm = @"<form action='/HTML' method='POST'>
               Name: <input type='text' name='Name'/>
               Age: <input type='number' name ='Age'/>
               <input type='submit' value ='Save' />
            </form>";


        public static void Main(string[] args)
            => new HttpServer(routes => routes
                .MapGet("/", new TextResponse("Hello from the server."))
                .MapGet("/Redirect", new RedirectResponse("https://softuni.bg"))
                .MapGet("/HTML", new HtmlResponse(StartUp.HtmlForm))
                .MapPost("/HTML", new TextResponse("", StartUp.AddFormDataAction)))
            .Start();

        private static void AddFormDataAction(Request request, Response response)
        {
            response.Body = "";

            foreach (var (key, value) in request.Form)
            {
                response.Body = $"{key} - {value}";
                response.Body = Environment.NewLine;
            }
        }
    }
}