namespace WebServer.Server.Services
{
    public class ServiceCollection : IServiceCollection
    {
        private readonly Dictionary<Type, Type> services;

        public ServiceCollection()
            => this.services = new();

        public IServiceCollection Add<TService, TImplementation>() 
            where TService : class 
            where TImplementation : TService
        {
            this.services[typeof(TService)] = typeof(TImplementation);

            return this;
        }

        public TService GetService<TService>()
            where TService : class
        {
            var serviceType = typeof(TService);

            if (!this.services.ContainsKey(serviceType))
            {
                return null;
            }

            var implementationType = this.services[serviceType];

            return (TService) this.CreateInstance(implementationType);
        }

        public object CreateInstance(Type type)
        {
            if (this.services.ContainsKey(type))
            {
                type = this.services[type];
            }
            else if (type.IsInterface)
            {
                throw new InvalidOperationException($"Service '{type.FullName}' is not registered");
            }

            var constructors = type
                .GetConstructors()
                .ToList();

            if (constructors.Count > 1)
            {
                throw new InvalidOperationException(
                    $"Service resolver doesn't support instance with multiple constructors");
            }

            var constructor = constructors.First();

            var parameters = constructor
                .GetParameters()
                .ToList();

            var parameterValues = new List<object>(parameters.Count);

            foreach (var parameter in parameters)
            {
                var parameterType = parameter.ParameterType;

                var parameterValue = this.CreateInstance(parameterType);

                parameterValues.Add(parameterValue);
            }

            return constructor.Invoke(parameterValues.ToArray());
        }
    }
}
