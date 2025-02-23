namespace electro_shop_backend.Models.DTOs.User
{
    public class ViewUserForAdminDTO
    {

        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarImg { get; set; }
        public string UserStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
