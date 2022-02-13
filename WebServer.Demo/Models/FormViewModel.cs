namespace WebServer.Demo.Models
{
    using System.ComponentModel.DataAnnotations;
    using Server.Controllers;

    public class FormViewModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(10, MinimumLength = 9, ErrorMessage = "Name has to be between {2} and {1} characters!")]
        public string Name { get; init; }

        [Range(5, 10, ErrorMessage = "Age has to be between {1} and {2}!")]
        public int Age { get; init; }

        [NeverBind]
        public string Never { get; set; }
    }
}
