using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> createToken(User user);
    }
}
