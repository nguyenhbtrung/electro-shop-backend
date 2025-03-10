using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.ReturnReason
{
    public class UpdateReturnReasonDto
    {
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
    }
}
