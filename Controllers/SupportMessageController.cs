using electro_shop_backend.Exceptions;
using electro_shop_backend.Extensions;
using electro_shop_backend.Models.DTOs.SupportMessage;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportMessageController : ControllerBase
    {
        private readonly ISupportMessageService _supportMessageService;
        private readonly UserManager<User> _userManager;

        public SupportMessageController(ISupportMessageService supportMessageService, UserManager<User> userManager)
        {
            _supportMessageService = supportMessageService;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageRequestDto requestDto)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _supportMessageService.CreateMessageAsync(user.Id, requestDto);
                return Ok(result);
            }
            catch (BadRequestException ex)
            {
                return BadRequest( ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("all-users")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllUserLatestMessages()
        {
            try
            {
                var result = await _supportMessageService.GetAllUserLatestMessagesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
