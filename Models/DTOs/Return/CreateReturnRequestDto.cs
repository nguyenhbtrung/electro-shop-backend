using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Return
{
    public class CreateReturnRequestDto
    {
        [Required]
        public int? OrderId { get; set; }
        [Required]
        public string? Reason { get; set; }
        [Required]
        public string? Detail { get; set; }
        [Required]
        public string? Status { get; set; }
        [Required]
        public string? ReturnMethod { get; set; }
        [Required]
        public string? Address { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
