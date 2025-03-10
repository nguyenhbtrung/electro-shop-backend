using electro_shop_backend.Models.DTOs.Brand;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IBrandService
    {
        Task<List<BrandDto>> GetAllBrandAsync();
        Task<BrandDto> CreateBrandAsync(CreateBrandRequestDto requestDto);
        Task<BrandDto> UpdateBrandAsync(int BrandId, UpdateBrandRequestDto requestDto);
        Task<bool> DeleteBrandAsync(int id);
    }
}
