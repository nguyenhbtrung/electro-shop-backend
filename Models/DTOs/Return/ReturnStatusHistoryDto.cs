using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using electro_shop_backend.Services;

namespace electro_shop_backend.Models.DTOs.Return
{
    public class ReturnStatusHistoryDto
    {
        public int ReturnHistoryId { get; set; }
        public int ReturnId { get; set; }
        public string? Status { get; set; }
        public DateTime? ChangedAt { get; set; }
    }
}
