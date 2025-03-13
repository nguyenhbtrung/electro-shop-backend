using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Discount
{
    public class ApplyDiscountDto
    {
        [Required]
        public int DiscountId { get; set; }

        [Required]
        public List<int> ProductIds { get; set; } = [];
    }

}
