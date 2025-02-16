using electro_shop_backend.Data;
using electro_shop_backend.Services.Interfaces;

namespace electro_shop_backend.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly ApplicationDbContext _context;

        public DiscountService(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
