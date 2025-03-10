using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Brand
{
    public class BrandDto
    {
        public int BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? Country { get; set; }
        public string? ImageUrl { get; set; }
        public string? Info { get; set; }
    }
}
