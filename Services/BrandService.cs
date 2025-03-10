using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Brand;

using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class BrandService : IBrandService
    {
        private readonly ApplicationDbContext _context;
        public BrandService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<BrandDto>> GetAllBrandAsync()
        {
            var brands = await _context.Brands.AsNoTracking().ToListAsync();
            var brandDtos = brands.Select( BrandMapper.ToBrandDto).ToList();
            return brandDtos;
        }
        public async Task<BrandDto> CreateBrandAsync(CreateBrandRequestDto requestDto)
        {
            try
            {
                var brand = requestDto.ToBrandFromCreateDto();
                await _context.Brands.AddAsync(brand);
                await _context.SaveChangesAsync();
                return brand.ToBrandDto();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BrandDto> UpdateBrandAsync(int BrandId, UpdateBrandRequestDto requestDto)
        {
            var brand = await _context.Brands.FindAsync(BrandId);
            if (brand == null)
            {
                throw new NotFoundException("Không tìm thấy hãng sản phẩm.");
            }
            brand.UpdateBrandDto(requestDto);
            await _context.SaveChangesAsync();
            return brand.ToBrandDto();
        }
        public async Task<bool> DeleteBrandAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand== null)
            {
                throw new NotFoundException("Không tìm thấy hãng sản phẩm.");
            }
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
