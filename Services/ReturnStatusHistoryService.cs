using electro_shop_backend.Data;
using electro_shop_backend.Services.Interfaces;

namespace electro_shop_backend.Services
{
    public class ReturnStatusHistoryService :IReturnStatusHistoryService
    {
        private readonly ApplicationDbContext _context;

        public ReturnStatusHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }


    }
}
