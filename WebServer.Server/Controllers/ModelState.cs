namespace WebServer.Server.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using Contracts;

    public class ModelState : IModelState
    {
        public ModelState()
            => this.Errors = new List<string>();

        public IList<string> Errors { get; init; }

        private object[] ActionParameters { get; init; }

        public bool IsValid => IsValidModel();

        private bool IsValidModel()
        {
            var realParameters = this.ActionParameters
                .Where(p => p.GetType().IsClass)
                .ToList();

            foreach (var realParameter in realParameters)
            {
                var properties = realParameter
                    .GetType()
                    .GetProperties();

                ValidateProperties(properties, realParameter);
            }

            return this.Errors.Count() > 0 ? false : true;
        }

        private void ValidateProperties(PropertyInfo[] properties, object realParameter)
        {
            foreach (var property in properties)
            {
                var propertyValue = property
                    .GetValue(realParameter);

                var propertyAttributes = property
                    .GetCustomAttributes()
                    .Where(a => a.GetType().IsAssignableTo(typeof(ValidationAttribute)))
                    .Cast<ValidationAttribute>()
                    .ToList();

                foreach (var attribute in propertyAttributes)
                {
                    ValidatePropertyWithAttribute(attribute, propertyValue);
                }
            }
        }

        private void ValidatePropertyWithAttribute(ValidationAttribute attribute, object propertyValue)
        {
            if (!attribute.IsValid(propertyValue))
            {
                this.Errors.Add(attribute.FormatErrorMessage(attribute.ErrorMessageResourceName));
            }
        }
    }
}
