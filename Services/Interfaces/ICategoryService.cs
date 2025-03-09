using electro_shop_backend.Models.DTOs.Category;
using electro_shop_backend.Models.DTOs.Product;

namespace electro_shop_backend.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<AllCategoryDto>> GetAllCategoriesIdsAndNamesAsync();
        Task<List<CategoryTreeDto>> GetCategoryTreeAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int categoryId);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto requestDto);
        Task<CategoryDto> UpdateCategoryAsync(int categoryId, UpdateCategoryRequestDto requestDto);
        Task<bool> DeleteCategoryAsync(int id);
        Task<List<ProductCardDto>> GetAllProductsByCategoryIdAsync(int categoryId);
    }
}
