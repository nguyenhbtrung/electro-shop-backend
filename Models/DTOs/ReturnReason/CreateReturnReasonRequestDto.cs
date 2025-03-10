using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.ReturnReason
{
    public class CreateReturnReasonRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
