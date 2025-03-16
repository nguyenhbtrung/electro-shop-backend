using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Brand;
using electro_shop_backend.Models.DTOs.Product;
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
            if (brand == null)
            {
                throw new NotFoundException("Không tìm thấy hãng sản phẩm.");
            }

            _context.Brands.Remove(brand);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    string errorMessage = ex.InnerException.Message.ToLower();
                    if (errorMessage.Contains("fk__product__brandid__46e78a0c"))
                    {
                        throw new Exception("Không được xóa nhãn hàng khi còn sản phẩm thuộc nhãn hàng.");
                    }
                }
                throw;
            }
        }

        public async Task<List<ProductCardDto>> GetAllProdcutByBrandIdAsync(int brandid)
        {
            var now = DateTime.Now;
            var products = await _context.Products
            .AsNoTracking()
            .Include(p=>p.ProductImages)
            .Include(p => p.Ratings)
            .Include(p => p.ProductDiscounts)
            .ThenInclude(pd => pd.Discount)
            .Where (c=>c.BrandId == brandid)
            .ToListAsync();
            ;
            var productDtos = products.Select(product =>
            {
                double avgRating = 0;
                if (product.Ratings != null && product.Ratings.Any())
                {
                    avgRating = product.Ratings.Average(r => r.RatingScore);
                }


                var effectiveDiscount = product.ProductDiscounts
                    .Where(pd => pd.Discount != null &&
                                 pd.Discount.StartDate <= now &&
                                 pd.Discount.EndDate >= now)
                    .Select(pd => pd.Discount)
                    .FirstOrDefault();

                string discountType = string.Empty;
                decimal discountValue = 0;
                decimal discountedPrice = product.Price;

                if (effectiveDiscount != null)
                {
                    discountType = effectiveDiscount.DiscountType;
                    discountValue = effectiveDiscount.DiscountValue ?? 0;

                    // Nếu discount theo phần trăm
                    if (string.Equals(discountType, "Percentage", StringComparison.OrdinalIgnoreCase))
                    {
                        discountedPrice = product.Price * (1 - discountValue / 100);
                    }
                    // Nếu discount theo số tiền cố định
                    else if (string.Equals(discountType, "Flat Amount", StringComparison.OrdinalIgnoreCase))
                    {
                        discountedPrice = product.Price - discountValue;
                    }

                    // Đảm bảo giá không âm
                    if (discountedPrice < 0)
                    {
                        discountedPrice = 0;
                    }
                }
                return new ProductCardDto
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Images = product.ProductImages?
                                .Where(pi => !string.IsNullOrWhiteSpace(pi.ImageUrl))
                                .Select(pi => pi.ImageUrl)
                                .ToList() ?? new List<string>(),
                    OriginalPrice = product.Price,
                    DiscountedPrice = discountedPrice,
                    DiscountType = discountType,
                    DiscountValue = discountValue,
                    AverageRating = avgRating
                };
            }).ToList();
            return productDtos;
        }
    }
}
