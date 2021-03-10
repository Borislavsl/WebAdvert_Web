using System.ComponentModel.DataAnnotations;

namespace WebAdvert_Web.Models.Accounts
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
