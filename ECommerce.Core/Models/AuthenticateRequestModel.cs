using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core
{
    public class AuthenticateRequestModel
    {
        [Required]
        [StringLength(25)]
        public string Username { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
