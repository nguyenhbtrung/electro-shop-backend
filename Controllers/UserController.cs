using electro_shop_backend.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController
    {
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return new List<User>
            {
                new User { Id = "1", UserName = "John Doe" },
                new User { Id = "2", UserName = "Jane Doe" },
            };
        }
    }
}
