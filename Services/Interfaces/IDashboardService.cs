using electro_shop_backend.Models.DTOs.Dashboard;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetTotalImportFeeAsync();
        Task<int> CountNewActiveUsersThisMonthAsync();
        Task<List<RevenueStatistic>> GetLast12MonthsRevenueAsync();
        Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(int topCount = 5);
    }
}
