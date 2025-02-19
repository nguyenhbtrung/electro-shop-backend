using electro_shop_backend.Models.DTOs.Category;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToCategoryDto(this Category category) //shows
        {
            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                ImageUrl = category.ImageUrl,
            };
        }
        public static Category ToCategoryFromCreate(this CreateCategoryRequestDto requestDto)//tạo
        {
            return new Category
            {
                Name = requestDto.Name,
                Description = requestDto.Description,
                ParentCategoryId = requestDto.ParentCategoryId,
                ImageUrl = requestDto.ImageUrl,
            };
        }
        public static void UpdateCategoryFromDto(this Category category, UpdateCategoryRequestDto requestDto) // Cập nhật
        {
            category.Name = requestDto.Name;
            category.Description = requestDto.Description;
            category.ParentCategoryId = requestDto.ParentCategoryId;
            category.ImageUrl = requestDto.ImageUrl;
        }
    }
}
