using electro_shop_backend.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Discount
{
    public class DiscountDto
    {
        public int DiscountId { get; set; }

        public string Name { get; set; } = null!;

        public string? DiscountType { get; set; }

        public decimal? DiscountValue { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public int ProductCount { get; set; }
    }
}
