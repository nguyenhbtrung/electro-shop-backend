using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Return
{
    public class CreateReturnRequestDto
    {
        [Required]
        public int? OrderId { get; set; }
        [Required]
        public string? Reason { get; set; }
        public string? Detail { get; set; }
        [Required]
        public ReturnMethod ReturnMethod { get; set; }
        [Required]
        public List<ReturnItemDto> ReturnItems { get; set; } = [];
        [Required]
        public List<IFormFile> EvidenceImages { get; set; } = [];

    }

    public enum ReturnMethod
    {
        Refund,
        Exchange,
        Repair
    }
}
