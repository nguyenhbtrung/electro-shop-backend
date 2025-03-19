using electro_shop_backend.Services;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Return
{
    public class UpdateReturnStatusRequestDto
    {
        [Required]
        public ReturnStatus ReturnStatus { get; set; }
        public string? AdminComment { get; set; }
    }
}
