using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            try
            {
                var result = await _dashboardService.GetTotalRevenueAsync();
                return Ok(new { TotalRevenue = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("importfee")]
        public async Task<IActionResult> GetTotalImportFee()
        {
            try
            {
                var result = await _dashboardService.GetTotalImportFeeAsync();
                return Ok(new { TotalImportFee = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("new_user")]
        public async Task<IActionResult> GetNewActiveUsersCount()
        {
            try
            {
                var result = await _dashboardService.CountNewActiveUsersThisMonthAsync();
                return Ok(new { NewActiveUserCount = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
