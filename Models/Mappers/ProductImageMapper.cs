using electro_shop_backend.Models.DTOs.ProductImage;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class ProductImageMapper
    {
        public static ProductImageDto ToProductImageDto(this ProductImage imageproduct)//show
        {
            return new ProductImageDto
            {
                ProductImageId = imageproduct.ProductImageId,
                ImageUrl = imageproduct.ImageUrl,
            };
        }
        public static ProductImage ToProductImageFromCreate(this CreateProductImageDto requestDto)//tạo
        {
            return new ProductImage
            {
                ImageUrl = requestDto.ImageUrl,
            };
        }
        public static void ToProductImageFromUpdate(this CreateProductImageDto requestDto, ProductImage productImage)
        {
            productImage.ImageUrl = requestDto.ImageUrl;
        }


    }
}
