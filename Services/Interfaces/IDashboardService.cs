namespace electro_shop_backend.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetTotalImportFeeAsync();
        Task<int> CountNewActiveUsersThisMonthAsync();
    }
}
