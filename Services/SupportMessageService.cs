using electro_shop_backend.Data;
using electro_shop_backend.Services.Interfaces;

namespace electro_shop_backend.Services
{
    public class SupportMessageService : ISupportMessageService
    {
        private readonly ApplicationDbContext _context;

        public SupportMessageService(ApplicationDbContext context)
        {
            _context = context;
        }


    }
}
