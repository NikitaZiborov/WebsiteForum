using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteForum.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string SurName { get; set; }
        [Required(ErrorMessage = "NickName is required.")]
        public string NickName { get; set; }
        [Required(ErrorMessage = "BirthDate is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:Day/Month/Year}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> BirthDate { get; set; }
        [Required(ErrorMessage = "Email Name is required.")]
        [RegularExpression(@"[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z0-9]+", ErrorMessage = "Please enter valid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please confirms your password.")]
        [DataType(DataType.Password)]
        public string ConfirmationPassword { get; set; }
    }
}
