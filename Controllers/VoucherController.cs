using electro_shop_backend.Data;
using electro_shop_backend.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using electro_shop_backend.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class VoucherController : ControllerBase
{
    private MyDbContext _context;

    public VoucherController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var dsVoucher = _context.Vouchers.ToList();
        return Ok(dsVoucher);
    }

    [HttpGet("{VoucherName}")]
    public IActionResult GetVoucherByName(string VoucherName)
    {
        if (string.IsNullOrWhiteSpace(VoucherName))
            return BadRequest("Voucher name is required");

        var voucher = _context.Vouchers.FirstOrDefault(item => item.VoucherName.ToLower() == VoucherName.ToLower());

        if (voucher == null)
            return NotFound("Voucher not found");

        return Ok(voucher);
    }

    [HttpPut("{VoucherId}")]
    public IActionResult UpdateVoucherById(int VoucherId, VoucherModel model)
    {
        var voucher = _context.Vouchers.SingleOrDefault(item => item.VoucherId == VoucherId);
        if (VoucherId != null)
        {
            voucher.VoucherName = model.VoucherName;
            _context.SaveChanges();
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    public IActionResult CreateNewVoucher(VoucherModel model)
    {
        try
        {
            var voucher = new VoucherDBContext
            {
                VoucherCode = model.VoucherCode,
                VoucherName = model.VoucherName,
                VoucherType = model.VoucherType,
                DiscountValue = model.DiscountValue,
                MinOrderValue = model.MinOrderValue,
                MaxDiscount = model.MaxDiscount,
                UsageLimit = model.UsageLimit,
                UsageCount = model.UsageCount,
                VoucherStatus = model.VoucherStatus,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };
            _context.Add(voucher);
            _context.SaveChanges();
            return Ok(voucher);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
    }
}