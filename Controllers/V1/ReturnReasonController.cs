using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.ReturnReason;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnReasonController : ControllerBase
    {
        private readonly IReturnReasonService _returnReasonService;

        public ReturnReasonController(IReturnReasonService returnReasonService)
        {
            _returnReasonService = returnReasonService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllReturnReason()
        {
            var returnReasons = await _returnReasonService.GetAllReturnReasonAsync();
            return Ok(returnReasons);
        }
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateReturnReason([FromBody] CreateReturnReasonRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _returnReasonService.CreateReturnReasonAsync(requestDto);
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
        [HttpPut("{ReasonId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateReturnReason(int ReasonId, [FromBody] UpdateReturnReasonDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _returnReasonService.UpdateReturnReasonAsync(ReasonId, requestDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [HttpDelete("{ReasonId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteReturnReason(int ReasonId)
        {
            try
            {
                await _returnReasonService.DeleteReturnReasonAsync(ReasonId);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
