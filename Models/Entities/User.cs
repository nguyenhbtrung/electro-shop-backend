using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.Entities
{
    public class User : IdentityUser // nho doi ca trong applicationdbcontext tu dbcontext thanh identitydbcontext
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string AvatarImg { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string userStatus { get; set; }
        public DateTime createdAt { get; set; }
    }
}
