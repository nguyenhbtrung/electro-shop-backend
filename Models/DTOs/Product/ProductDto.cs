﻿using electro_shop_backend.Models.DTOs.ProductImage;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.DTOs.Category;
namespace electro_shop_backend.Models.DTOs.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string? Info { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int? RatingCount { get; set; }
        public double? AverageRating { get; set; }
        public List<ProductImageDto> ProductImages { get; set; } = new();
        public List<CategoryIdDto> Categories { get; set; } = new();
    }
}
