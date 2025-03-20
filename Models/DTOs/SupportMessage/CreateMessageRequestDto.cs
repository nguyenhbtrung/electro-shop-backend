namespace electro_shop_backend.Models.DTOs.SupportMessage
{
    public class CreateMessageRequestDto
    {
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string? Message { get; set; }
    }
}
