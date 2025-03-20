using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportMessageController : ControllerBase
    {
        private readonly ISupportMessageService _supportMessageService;

        public SupportMessageController(ISupportMessageService supportMessageService)
        {
            _supportMessageService = supportMessageService;
        }

        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> CreateMessage([FromBody])
        //{

        //}
    }
}
