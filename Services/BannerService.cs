using electro_shop_backend.Data;
using electro_shop_backend.Models.DTOs.Banner;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class BannerService : IBannerService
    {
        private readonly ApplicationDbContext _dbContext;

        public BannerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> CreateBanner(BannerDTO bannerDto)
        {
            try
            {
                var banner = BannerMapper.FromBannerDTOToBanner(bannerDto);
                await _dbContext.Banners.AddAsync(banner);
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult(banner);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> GetAllBanner()
        {
            var banners = await _dbContext.Banners.ToListAsync();
            return new OkObjectResult(banners);
        }
        public async Task<IActionResult> GetBanner(int id)
        {
            var banner = await _dbContext.Banners.FirstOrDefaultAsync(b => b.BannerId == id);
            if (banner == null)
            {
                return new NotFoundObjectResult("Banner not found");
            }
            return new OkObjectResult(banner);
        }
        public async Task<IActionResult> DeleteBanner(int id)
        {
            var banner = await _dbContext.Banners.FirstOrDefaultAsync(b => b.BannerId == id);
            if (banner == null)
            {
                return new NotFoundObjectResult("Banner not found");
            }
            _dbContext.Banners.Remove(banner);
            await _dbContext.SaveChangesAsync();
            return new OkObjectResult("Banner deleted");
        }
        public async Task<IActionResult> UpdateBanner(Banner bannerUpdate)
        {
            var banner = await _dbContext.Banners.FirstOrDefaultAsync(b => b.BannerId == bannerUpdate.BannerId);
            if (banner == null)
            {
                return new NotFoundObjectResult("Banner not found");
            }
            banner.Title = bannerUpdate.Title;
            banner.ImageUrl = bannerUpdate.ImageUrl;
            banner.Link = bannerUpdate.Link;
            banner.Position = bannerUpdate.Position;
            await _dbContext.SaveChangesAsync();
            return new OkObjectResult(banner);
        }
    }
}
