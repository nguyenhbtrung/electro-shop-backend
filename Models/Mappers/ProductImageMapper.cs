using electro_shop_backend.Models.DTOs.ProductImage;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class ProductImageMapper
    {
        public static ProductImageDto ToProductImageDto(this ProductImage imageProduct)
        {
            return new ProductImageDto
            {
                ProductImageId = imageProduct.ProductImageId,
                ImageUrl = imageProduct.ImageUrl,
            };
        }
    }
}
