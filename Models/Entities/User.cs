using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.Entities
{
    public class User : IdentityUser
    {
        public int user_id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string avatar_img { get; set; }
        [Required]
        public string role { get; set; }
        [Required]
        public string user_status { get; set; }
        public DateTime created_at { get; set; }
    }
}
