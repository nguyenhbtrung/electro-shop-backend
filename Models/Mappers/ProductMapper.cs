using electro_shop_backend.Models.DTOs.Brand;
using electro_shop_backend.Models.DTOs.Category;
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
                BrandId = requestDto.BrandId,
                Categories = new List<Category>()
            };
        }

        public static ProductDto ToProductDto(this Product product)//show
        {
            return new ProductDto
            {   
                ProductId = product.ProductId,
                Name = product.Name,
                Info = product.Info,
                OriginalPrice = product.Price ,
                Stock = product.Stock,
                UnitsSold=product.UnitsSold,
                ProductImages = new(),
                Categories = product.Categories?.Select(c => new CategoryIdDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                }).ToList() ?? new List<CategoryIdDto>(),
                Brand = product.Brand != null ? new BrandDto
                {
                    BrandId = product.Brand.BrandId,
                    BrandName = product.Brand.BrandName
                } : null
            };
        }
        public static void UpdateProductFromDto(this Product product, UpdateProductRequestDto requestDto) // Cập nhật
        {
            product.Name = requestDto.Name;
            product.Info = requestDto.Info;
            product.Price = requestDto.Price;
            product.Stock = requestDto.Stock;
            product.BrandId = requestDto.BrandId;
        }
    }
}
