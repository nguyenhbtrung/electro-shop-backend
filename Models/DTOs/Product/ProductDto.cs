﻿using electro_shop_backend.Models.DTOs.ProductImage;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.DTOs.Category;
using electro_shop_backend.Models.DTOs.Brand;
namespace electro_shop_backend.Models.DTOs.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string? Info { get; set; }
        public decimal OriginalPrice { get; set; }

        public decimal DiscountedPrice { get; set; }
        public string? DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public double AverageRating { get; set; }
        public bool IsFavorite { get; set; } = false;

        public int Stock { get; set; }
        public int UnitsSold { get; set; }

        public List<ProductImageDto> ProductImages { get; set; } = new();
        public List<CategoryIdDto> Categories { get; set; } = new();
        public List<ProductAttributeDetailDto> ProductAttributeDetail { get; set; } = new List<ProductAttributeDetailDto>();
        public BrandDto? Brand { get; set; }
    }
}
