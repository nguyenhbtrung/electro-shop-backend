using electro_shop_backend.Models.DTOs.Banner;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateBanner(BannerDTO bannerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _bannerService.CreateBanner(bannerDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBanner([FromRoute] int id)
        {
            return await _bannerService.GetBanner(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBanner()
        {
            return await _bannerService.GetAllBanner();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteBanner([FromRoute] int id)
        {
            return await _bannerService.DeleteBanner(id);
        }
        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateBanner(Banner banner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _bannerService.UpdateBanner(banner);
        }
    }
}
