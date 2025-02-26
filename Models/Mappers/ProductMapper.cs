using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class ProductMapper
    {
        public static Product ToProductFromCreate(this CreateProductRequestDto requestDto)//tạo
        {
            return new Product
            {
                Name = requestDto.Name,
                Info = requestDto.Info,
                Price = requestDto.Price,
                Stock = requestDto.Stock,
            };
        }

        public static ProductDto ToProductDto(this Product product)//show
        {
            return new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Info = product.Info,
                Price = product.Price,
                Stock = product.Stock,
                RatingCount = product.RatingCount,
                AverageRating = product.AverageRating,
                ProductImages = new()
            };
        }
        public static void UpdateProductFromDto(this Product product, UpdateProductRequestDto requestDto) // Cập nhật
        {
            product.Name = requestDto.Name;
            product.Info = requestDto.Info;
            product.Price = requestDto.Price;
            product.Stock = requestDto.Stock;
        }
    }
}
