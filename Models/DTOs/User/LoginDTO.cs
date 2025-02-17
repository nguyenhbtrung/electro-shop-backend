using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.User
{
    public class LoginDTO
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
