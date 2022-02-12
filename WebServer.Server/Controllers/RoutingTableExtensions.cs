namespace WebServer.Server.Controllers
{
    using System.Reflection;
    using HTTP;
    using Routing;

    public static class RoutingTableExtensions
    {
        private static Type stringType = typeof(string);

        public static IRoutingTable MapGet<TController>(
            this IRoutingTable routingTable,
            string path,
            Func<TController, Response> controllerFunction)
            where TController : Controller
            => routingTable.MapGet(path, request => controllerFunction(CreateController<TController>(request)));

        public static IRoutingTable MapPost<TController>(
            this IRoutingTable routingTable,
            string path,
            Func<TController, Response> controllerFunction)
            where TController : Controller
            => routingTable.MapPost(path, request => controllerFunction(CreateController<TController>(request)));

        private static TController CreateController<TController>(Request request)
            where TController : Controller
            => (TController)CreateController(typeof(TController), request);

        private static Controller CreateController(Type controllerType, Request request)
        {
            var controller = Activator.CreateInstance(controllerType);

            controllerType
                .BaseType
                .GetProperty("Request", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(controller, request);

            return (Controller) controller;
        }

        public static IRoutingTable MapControllers(this IRoutingTable routingTable)
        {
            var actions = GetControllerActions();

            foreach (var action in actions)
            {
                var controllerType = action.DeclaringType;

                var actionName = action.Name;
                var controllerName = controllerType.GetControllerName();

                var urlPath = $"/{controllerName}/{actionName}";

                var actionMethod = GetActionMethod(action);

                Func<Request, Response> responseFunction = GetResponseFunction(action, controllerType);

                routingTable.Map(actionMethod, urlPath, responseFunction);

                MapDefaultRoutes(
                    routingTable,
                    actionName,
                    controllerName,
                    actionMethod,
                    responseFunction);
            }

            return routingTable;
        }

        private static IEnumerable<MethodInfo> GetControllerActions()
            => Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract &&
                            t.IsPublic &&
                            t.IsClass &&
                            t.IsAssignableTo(typeof(Controller)) &&
                            t.Name.EndsWith(nameof(Controller)))
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                .Where(m => m.ReturnType.IsAssignableTo(typeof(Response)))
                .ToList();

        private static Method GetActionMethod(MethodInfo action)
        {
            var defaultMethod = Method.Get;

            var actionMethodAttribute = action
                .GetCustomAttribute<HttpMethodAttribute>();

            if (actionMethodAttribute != null)
            {
                defaultMethod = actionMethodAttribute.Method;
            }

            return defaultMethod;
        }

        private static Func<Request, Response> GetResponseFunction(MethodInfo action, Type controllerType)
            => request =>
            {
                var actionAuthorizeAttribute = action
                    .GetCustomAttribute<AuthorizeAttribute>();

                if (actionAuthorizeAttribute != null &&
                    !request.Session.ContainsKey(Session.SessionUserKey))
                {
                    return new Response(StatusCode.Unauthorized);
                }

                var controller = CreateController(controllerType, request);

                var parameters = GetParametersValues(action, request);

                return (Response) action.Invoke(controller, parameters);
            };

        private static void MapDefaultRoutes(
            IRoutingTable routingTable,
            string actionName,
            string controllerName,
            Method actionMethod,
            Func<Request, Response> responseFunction)
        {
            const string defaultActionName = "Index";
            const string defaultControllerName = "Home";

            if (actionName == defaultActionName)
            {
                routingTable.Map(actionMethod, $"/{controllerName}", responseFunction);

                if (controllerName == defaultControllerName)
                {
                    routingTable.Map(actionMethod, "/", responseFunction);
                }
            }
        }

        private static object[] GetParametersValues(MethodInfo action, Request request)
        {
            var actionParameters = action
                .GetParameters()
                .Select(p => new
                {
                    p.Name,
                    Type = p.ParameterType
                })
                .ToList();

            var parametersValues = new List<object>(actionParameters.Count);

            foreach (var parameterInfo in actionParameters)
            {
                var parameterName = parameterInfo.Name;
                var parameterType = parameterInfo.Type;

                if (parameterType.IsPrimitive || parameterType == stringType)
                {
                    var requestValue = request.GetValue(parameterName);

                    var parameterValue = Convert.ChangeType(requestValue, parameterType);
                    parametersValues.Add(parameterValue);
                }
                else if (parameterType.IsClass)
                {
                    var classProperties = parameterType
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .Where(p => p.GetCustomAttribute<NeverBindAttribute>() == null)
                        .ToList();

                    var parameterClass = Activator.CreateInstance(parameterType);

                    foreach (var propertyInfo in classProperties)
                    {
                        var propertyName = propertyInfo.Name;
                        var propertyType = propertyInfo.PropertyType;

                        var requestValue = request.GetValue(propertyName);

                        var propertyValue = Convert.ChangeType(requestValue, propertyType);

                        propertyInfo
                            .SetValue(parameterClass, propertyValue);
                    }

                    parametersValues.Add(parameterClass);
                }

            }

            return parametersValues.ToArray();
        }

        private static string GetValue(this Request request, string key)
            => request.Query.GetValueOrDefault(key) ??
               request.Form.GetValueOrDefault(key);
    }
}
