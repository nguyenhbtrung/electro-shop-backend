using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Voucher;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class VoucherService: IVoucherService
    {
        private readonly ApplicationDbContext _context;

        public VoucherService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AllVoucherDto>> GetAllVouchersAsyncs()
        {
            return await _context.Vouchers
                .AsNoTracking()
                .Select(item => new AllVoucherDto
                {
                    VoucherId = item.VoucherId,
                    VoucherName = item.VoucherName,
                    VoucherCode = item.VoucherCode,
                    VoucherStatus = item.VoucherStatus
                })
                .ToListAsync();
        }

        public async Task<VoucherDto?> GetVoucherByIdAsyncs(int voucherId)
        {
            if (voucherId <= 0)
                throw new ArgumentException("Voucher ID is required");

            var voucher = await _context.Vouchers
                .AsNoTracking()
                .Where(item => item.VoucherId == voucherId)
                .Select(item => new VoucherDto
                {
                    VoucherId = item.VoucherId,
                    VoucherName = item.VoucherName,
                    VoucherCode = item.VoucherCode,
                    DiscountValue = item.DiscountValue,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    VoucherStatus = item.VoucherStatus
                })
                .FirstOrDefaultAsync();

            if (voucher == null)
            {
                throw new NotFoundException("Voucher not found");
            }

            return voucher;
        }

        public async Task<VoucherDto> CreateVoucherAsync(CreateVoucherRequestDto requestDto)
        {
            try
            {
                var voucher = requestDto.ToVoucherFromCreate();
                await _context.Vouchers.AddAsync(voucher);
                await _context.SaveChangesAsync();
                return voucher.ToVoucherDto();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<VoucherDto> UpdateVoucherAsync(int voucherId, UpdateVoucherRequestDto requestDto)
        {
            var voucher = await _context.Vouchers.FindAsync(voucherId);
            if (voucher == null)
            {
                throw new NotFoundException("Voucher not found");
            }
            voucher.UpdateVoucherFromUpdateDto(requestDto);
            await _context.SaveChangesAsync();
            return voucher.ToVoucherDto();
        }

        public async Task<bool> DeleteVoucherAsync(int voucherId)
        {
            var voucher = await _context.Vouchers.FindAsync(voucherId);
            if (voucher == null)
            {
                throw new NotFoundException("Voucher not found");
            }
            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
