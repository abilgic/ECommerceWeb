using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core
{
    public partial class AccountController
    {
        public class RegisterRequestModel
        {
            [Required]
            [StringLength(25)]
            public string Username { get; set; }
            [Required]
            [StringLength(16, MinimumLength =6)]
            public string Password { get; set; }
            [Compare(nameof(Password))]
            public string RePassword { get; set; }
        }
    }
}
