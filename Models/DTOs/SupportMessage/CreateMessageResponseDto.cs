using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.SupportMessage
{
    public class CreateMessageResponseDto
    {
        public int Id { get; set; }
        public string? SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? ReceiverId { get; set; }
        public string? Message { get; set; }
        public bool IsFromAdmin { get; set; }
        public DateTime SentAt { get; set; }
    }
}
