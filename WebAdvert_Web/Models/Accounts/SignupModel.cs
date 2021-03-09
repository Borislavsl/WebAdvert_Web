using System.ComponentModel.DataAnnotations;

namespace WebAdvert_Web.Models.Accounts
{
    public class SignupModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password should be at least six characters long!")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and its confirmation do not match!")]
        public string ConfirmPassword { get; set; }
    }
}
