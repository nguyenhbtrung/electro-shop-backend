using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Return
{
    public class ReturnHistoryDto
    {
        public string? Status { get; set; }
        public DateTime? ChangedAt { get; set; }
    }
}
