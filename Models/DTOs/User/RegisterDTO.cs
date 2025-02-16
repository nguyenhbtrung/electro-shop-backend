using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.User
{
    public class RegisterDTO
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
