using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Voucher;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;
        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet("all_voucher")]
        public async Task<IActionResult> GetAllVouchersAsyncs()
        {
            var listVoucher = await _voucherService.GetAllVouchersAsyncs();
            return Ok(listVoucher);
        }

        [HttpGet("available_voucher")]
        public async Task<IActionResult> GetAvailableVouchersAsync()
        {
            var listVoucher = await _voucherService.GetVoucherAvailableAsync();
            return Ok(listVoucher);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucherByIdAsync(int id)
        {
            var voucher = await _voucherService.GetVoucherByIdAsyncs(id);
            if (voucher == null) return NotFound("Không tìm thấy voucher");
            return Ok(voucher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucherAsync(int id, UpdateVoucherRequestDto requestDto)
        {
            try
            {
                var newvoucher = await _voucherService.UpdateVoucherAsync(id, requestDto);
                return Ok(newvoucher);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateVoucherAsync(CreateVoucherRequestDto requestDto)
        {
            try
            {
                var voucher = await _voucherService.CreateVoucherAsync(requestDto);
                return Ok(voucher);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucherAsync(int id)
        {
            try
            {
                var result = await _voucherService.DeleteVoucherAsync(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
