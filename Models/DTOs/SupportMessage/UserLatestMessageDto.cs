namespace electro_shop_backend.Models.DTOs.SupportMessage
{
    public class UserLatestMessageDto
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public bool? IsFromAdmin { get; set; }
        public string? SenderName {  get; set; }
        public string? Message { get; set; }
        public string? AdminId { get; set; }
    }
}
