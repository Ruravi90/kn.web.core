using System.ComponentModel.DataAnnotations;

namespace kn.web.core.JWTAuthentication.Models.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "CorporateName is required")]
        public string CorporateName { get; set; }

        [Required(ErrorMessage = "User LastName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "IsAdmin is required")]
        public int IsAdmin { get; set; }
    }
}