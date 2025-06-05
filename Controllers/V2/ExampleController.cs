using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { Message = "Test API Version 2"} );
    }
}
