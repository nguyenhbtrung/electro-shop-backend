namespace electro_shop_backend.Models.DTOs.User
{
    public class RegisterResponseDTO
    {
        public NewUserDTO NewUser { get; set; }
        public string EmailConfirmationMessage { get; set; }
    }
}
