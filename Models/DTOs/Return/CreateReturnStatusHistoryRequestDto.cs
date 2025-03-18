using electro_shop_backend.Services;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Return
{
    public class CreateReturnStatusHistoryRequestDto
    {
        [Required]
        public int ReturnId { get; set; }
        [Required]
        public ReturnStatus Status { get; set; }
        public DateTime? ChangedAt { get; set; }
    }
}
