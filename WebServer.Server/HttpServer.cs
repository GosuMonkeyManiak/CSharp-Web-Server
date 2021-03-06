namespace WebServer.Server
{
    using HTTP;
    using Routing;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using Controllers;
    using Controllers.Contracts;
    using Results.ViewEngine;
    using Services;

    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener serverListener;

        private readonly RoutingTable routingTable;
        private readonly ServiceCollection servicesCollection;

        private HttpServer(string ipAddress, int port, IRoutingTable routingTable)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;

            this.serverListener = new TcpListener(this.ipAddress, this.port);

            this.routingTable = (RoutingTable) routingTable;

            this.servicesCollection = new ServiceCollection();

            this.servicesCollection.Add<IModelState, ModelState>();
            this.servicesCollection.Add<IViewEngine, ViewEngine>();
        }

        private HttpServer(int port, IRoutingTable routingTable)
            : this("127.0.0.1", port, routingTable)
        {
        }

        private HttpServer(IRoutingTable routingTable)
            : this(8080, routingTable)
        {
        }

        public static HttpServer WithRoutes(Action<IRoutingTable> routingTableConfiguration)
        {
            var routingTable = new RoutingTable();

            routingTableConfiguration(routingTable);

            return new HttpServer(routingTable);
        }

        public HttpServer WithServices(Action<IServiceCollection> serviceConfiguration)
        {
            serviceConfiguration(this.servicesCollection);

            return this;
        }

        public async Task Start()
        {
            this.serverListener.Start();

            Console.WriteLine($"Server started on port {this.port}.");
            Console.WriteLine("Listening for requests...");

            while (true)
            {
                var connection = await serverListener.AcceptTcpClientAsync();

                _ = Task.Run(async () =>
                {
                    var networkStream = connection.GetStream();

                    string requestText = await ReadRequest(networkStream);

                    try
                    {
                        Request request = Request.Parse(requestText, this.servicesCollection);

                        Response response = this.routingTable.ExecuteRequest(request);

                        AddSession(request, response);

                        await WriteResponse(networkStream, response);

                        this.LogPipeLine(request, response);
                    }
                    catch (Exception exception)
                    {
                        await HandleError(networkStream, exception);
                    }

                    connection.Close();
                });
            }
        }

        private void AddSession(Request request, Response response)
        {
            if (request.Session.IsNew)
            {
                response.Cookies.Add(Session.SessionCookieName, request.Session.Id);
                request.Session.IsNew = false;
            }
        }

        private async Task<string> ReadRequest(NetworkStream networkStream)
        {
            var bufferLength = 1024;
            var buffer = new byte[bufferLength];

            var totalBytes = 0;

            StringBuilder requestBuilder = new StringBuilder();

            do
            {
                var bytesRead = await networkStream.ReadAsync(buffer, 0, bufferLength);

                totalBytes += bytesRead;

                if (totalBytes > 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large.");
                }

                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

            } while (networkStream.DataAvailable); //May not run correctly over the Internet

            return requestBuilder.ToString();
        }

        private async Task HandleError(NetworkStream networkStream, Exception exception)
        {
            var errorMessage = $"{exception.Message}{Environment.NewLine}{exception.StackTrace}";

            var errorResponse = Response.ForError(errorMessage);

            await WriteResponse(networkStream, errorResponse);
        }

        private void LogPipeLine(Request request, Response response)
        {
            string messageSeparator = new string('-', 50);

            var logMessage = new StringBuilder();

            logMessage.AppendLine(messageSeparator);
            logMessage.AppendLine();

            logMessage.AppendLine("Request:");
            logMessage.AppendLine(request.ToString());
            
            logMessage.AppendLine("Response:");
            logMessage.AppendLine(response.ToString());

            Console.WriteLine(logMessage);
        }

        private async Task WriteResponse(NetworkStream networkStream, Response response)
        {
            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());

            await networkStream.WriteAsync(responseBytes);

            if (response.Body != null)
            {
                await networkStream.WriteAsync(response.Body);
            }
        }
    }
}
