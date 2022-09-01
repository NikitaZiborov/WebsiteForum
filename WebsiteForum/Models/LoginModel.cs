using System.ComponentModel.DataAnnotations;

namespace WebsiteForum.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z0-9]+", ErrorMessage = "Please enter valid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
