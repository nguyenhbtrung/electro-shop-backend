using electro_shop_backend.Exceptions;
using electro_shop_backend.Extensions;
using electro_shop_backend.Models.DTOs.Rating;
using electro_shop_backend.Models.DTOs.Return;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnController : ControllerBase
    {
        private readonly IReturnService _returnService;
        private readonly UserManager<User> _userManager;

        public ReturnController(IReturnService returnService, UserManager<User> userManager)
        {
            _returnService = returnService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllReturn()
        {
            var returns = await _returnService.GetAllReturnAsync();
            return Ok(returns);
        }

        [HttpGet("{returnId}")]
        [Authorize]
        public async Task<IActionResult> GetReturnById([FromRoute] int returnId)
        {
            //var username = User.GetUsername();
            //var user = await _userManager.FindByNameAsync(username);
            //if (user == null)
            //{
            //    return NotFound();
            //}
            try
            {
                var result = await _returnService.GetReturnByIdAsync(returnId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReturn([FromForm] CreateReturnRequestDto requestDto)
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
                var result = await _returnService.CreateReturnAsync(user.Id, requestDto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{ReturnId}")]
        public async Task<IActionResult> UpdateReturn(int ReturnId, [FromBody] UpdateReturnDto requestDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _returnService.UpdateReturnAsync(ReturnId, requestDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{ReturnId}")]
        public async Task<IActionResult> DeleteReturn(int ReturnId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _returnService.DeleteReturnAsync(ReturnId);
            return Ok("Rating deleted successfully");
        }
    }
}
