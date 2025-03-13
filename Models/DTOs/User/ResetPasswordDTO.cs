using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.User
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token {get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
