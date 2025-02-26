using electro_shop_backend.Models.DTOs.Banner;
using electro_shop_backend.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IBannerService
    {
        Task<IActionResult> CreateBanner(BannerDTO bannerDto);
        Task<IActionResult> GetAllBanner();
        Task<IActionResult> GetBanner(int id);
        Task<IActionResult> DeleteBanner(int id);
        Task<IActionResult> UpdateBanner(Banner banner);
    }
}
