﻿namespace electro_shop_backend.Models.DTOs.Category
{
    public class AllCategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int? ParentCategoryId { get; set; }
        public string? ImageUrl { get; set; }
    }
}
