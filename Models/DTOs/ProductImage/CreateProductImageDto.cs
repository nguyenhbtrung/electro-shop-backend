using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace electro_shop_backend.Models.DTOs.ProductImage
{
    public class CreateProductImageDto
    {
        [Required]
        public IFormFile[] Images { get; set; }
    }
}
