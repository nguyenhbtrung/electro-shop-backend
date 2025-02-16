using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Discount
{
    public class CreateDiscountRequestDto
    {
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public required string DiscountType { get; set; }

        [Required]
        public decimal DiscountValue { get; set; }

        public DateTime? StartDate { get; set; } = DateTime.Now;

        public DateTime? EndDate { get; set; } = DateTime.Now;

    }
}
