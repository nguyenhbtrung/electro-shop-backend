using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Return
{
    public class ReturnDto
    {
        [Required]
        public int ReturnId { get; set; }
        public int? OrderId { get; set; }
        public string? Reason { get; set; }
        public string? Detail { get; set; }
        public string? Status { get; set; }
        public string? ReturnMethod { get; set; }
        public string? Address { get; set; }
        public string? AdminComment { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}