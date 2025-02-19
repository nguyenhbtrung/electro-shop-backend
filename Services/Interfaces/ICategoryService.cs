using electro_shop_backend.Models.DTOs.Category;

namespace electro_shop_backend.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<AllCategoryDto>> GetAllCategoriesIdsAndNamesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int categoryId);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto requestDto);
        Task<CategoryDto> UpdateCategoryAsync(int categoryId, UpdateCategoryRequestDto requestDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
