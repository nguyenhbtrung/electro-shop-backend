using Azure.Core;
using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Discount;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly ApplicationDbContext _context;

        public DiscountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> ApplyDiscountToProductsAsync(ApplyDiscountDto request)
        {
            var discount = await _context.Discounts.FindAsync(request.DiscountId) ?? 
                throw new ArgumentException("Discount không tồn tại.");
            var existingAssociations = await _context.Set<ProductDiscount>()
                .Where(pd => pd.DiscountId == request.DiscountId)
                .ToListAsync();

            if (existingAssociations.Count != 0)
            {
                _context.Set<ProductDiscount>().RemoveRange(existingAssociations);
            }

            foreach (var productId in request.ProductIds)
            {
                var newAssociation = new ProductDiscount
                {
                    DiscountId = request.DiscountId,
                    ProductId = productId
                };
                await _context.Set<ProductDiscount>().AddAsync(newAssociation);
            }

            await _context.SaveChangesAsync();
            return request.ProductIds.Count;
        }

        public async Task<Discount> CreateDiscountAsync(CreateDiscountRequestDto requestDto)
        {
            try
            {
                var discount = requestDto.ToDiscountFromCreate();
                await _context.Discounts.AddAsync(discount);
                await _context.SaveChangesAsync();
                return discount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteDiscountAsync(int discountId)
        {
            try
            {
                var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == discountId);
                if (discount == null)
                {
                    throw new NotFoundException("Không tìm thấy discount");
                }
                _context.Discounts.Remove(discount);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DiscountDto> GetDiscountByIdAsync(int discountId)
        {
            try
            {
                var discount = await _context.Discounts.AsNoTracking().FirstOrDefaultAsync(d => d.DiscountId == discountId);
                if (discount == null)
                {
                    throw new NotFoundException("Không tìm thấy discount");
                }
                return discount.ToDiscountDto();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DiscountProductsResponseDto> GetDiscountedProductsAsync(int discountId)
        {
            
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductImages)
                .ToListAsync();

            var selectedProductIds = await _context.Set<ProductDiscount>()
                .Where(pd => pd.DiscountId == discountId)
                .Select(pd => pd.ProductId)
                .ToListAsync();

            var productDtos = products.Select(p => new DiscountedProductDto
            {
                Id = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                Brand = p.Brand?.BrandName ?? string.Empty,
                Image = p.ProductImages.FirstOrDefault()?.ImageUrl ?? string.Empty
            }).ToList();

            return new DiscountProductsResponseDto
            {
                Products = productDtos,
                SelectedProductIds = selectedProductIds
            };
        }

        public async Task<ICollection<DiscountDto>> GetDiscountsAsync(DiscountQuery discountQuery)
        {
            try
            {
                int skipNumber = (discountQuery.PageNumber - 1) * discountQuery.PageSize;
                var discounts = await _context.Discounts
                    .AsNoTracking()
                    .Select(d => new DiscountDto
                    {
                        DiscountId = d.DiscountId,
                        Name = d.Name,
                        DiscountType = d.DiscountType,
                        DiscountValue = d.DiscountValue,
                        StartDate = d.StartDate,
                        EndDate = d.EndDate,
                        ProductCount = d.ProductDiscounts.Count()
                    })
                    .OrderByDescending(d => d.StartDate)
                    .Skip(skipNumber)
                    .Take(discountQuery.PageSize)
                    .ToListAsync();

                return discounts;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DiscountDto> UpdateDiscountAsync(int discountId, CreateDiscountRequestDto requestDto)
        {
            try
            {
                var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == discountId);
                if (discount == null)
                {
                    throw new NotFoundException("Không tìm thấy discount");
                }
                discount.Name = requestDto.Name;
                discount.DiscountType = requestDto.DiscountType;
                discount.DiscountValue = requestDto.DiscountValue;
                discount.StartDate = requestDto.StartDate;
                discount.EndDate = requestDto.EndDate;
                await _context.SaveChangesAsync();
                return discount.ToDiscountDto();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
