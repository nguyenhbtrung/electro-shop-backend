using electro_shop_backend.Data;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Orders
                .Where(o => o.Status != null && o.Status.ToLower() == "successed")
                .SumAsync(o => o.Total);
        }

        public async Task<decimal> GetTotalImportFeeAsync()
        {
            return await _context.StockImports
                .Where(si => si.StockImportStatus != null && si.StockImportStatus.ToLower() == "completed")
                .SumAsync(si => si.TotalPrice);
        }

        public async Task<int> CountNewActiveUsersThisMonthAsync()
        {
            var now = DateTime.Now;

            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);

            return await _context.Users
                .Where(u => u.UserStatus != null &&
                            u.UserStatus.ToLower() == "active" &&
                            u.CreatedAt >= startOfMonth &&
                            u.CreatedAt < startOfNextMonth)
                .CountAsync();
        }
    }
}
