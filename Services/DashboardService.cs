using electro_shop_backend.Data;
using electro_shop_backend.Models.DTOs.Dashboard;
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

        public async Task<List<RevenueStatistic>> GetLast12MonthsRevenueAsync()
        {
            var now = DateTime.Now;
            // Tháng bắt đầu là tháng hiện tại trừ đi 11 tháng để có 12 tháng
            var startMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-11);

            // Truy vấn các đơn hàng có trạng thái "successed" và TimeStamp từ startMonth trở đi
            var revenueData = await _context.Orders
                .Where(o => o.Status != null &&
                            o.Status.ToLower() == "successed" &&
                            o.TimeStamp.HasValue &&
                            o.TimeStamp.Value >= startMonth)
                .GroupBy(o => new { Year = o.TimeStamp.Value.Year, Month = o.TimeStamp.Value.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalRevenue = g.Sum(o => o.Total)
                })
                .ToListAsync();

            // Dùng Dictionary để tra cứu nhanh theo key dạng "MM-yyyy"
            var revenueDict = revenueData.ToDictionary(
                key => $"{key.Month:D2}-{key.Year}",
                value => value.TotalRevenue);

            var result = new List<RevenueStatistic>();

            // Lặp qua 12 tháng từ startMonth đến tháng hiện tại
            for (int i = 0; i < 12; i++)
            {
                var monthDate = startMonth.AddMonths(i);
                var key = $"{monthDate.Month:D2}-{monthDate.Year}";
                revenueDict.TryGetValue(key, out decimal totalRevenue);

                result.Add(new RevenueStatistic
                {
                    X = key,
                    Y = totalRevenue
                });
            }

            return result;
        }

        public async Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(int topCount = 5)
        {
            // Truy vấn dữ liệu từ OrderItems (bảng Order_Item)
            // Lọc những record có ProductId khác null
            // Nhóm theo ProductId và ProductName, sau đó tính tổng số lượng bán được
            var query = await _context.OrderItems
                .Where(oi => oi.ProductId != null
                             && oi.Order != null
                             && oi.Order.Status != null
                             && oi.Order.Status.ToLower() == "successed")
                .GroupBy(oi => new { oi.ProductId, oi.ProductName })
                .Select(g => new TopSellingProductDto
                {
                    Id = g.Key.ProductId.Value,
                    Name = g.Key.ProductName,
                    Sold = g.Sum(x => x.Quantity),
                    Price = g.Max(x => x.Price)
                })
                .OrderByDescending(x => x.Sold)
                .Take(topCount)
                .ToListAsync();

            return query;
        }
    }
}
