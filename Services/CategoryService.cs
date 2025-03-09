using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Category;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<AllCategoryDto>> GetAllCategoriesIdsAndNamesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .Select(p => new AllCategoryDto
                {
                    CategoryId = p.CategoryId,
                    Name = p.Name,
                    Description = p.Description,
                    ParentCategoryId = p.ParentCategoryId,
                    ImageUrl = p.ImageUrl,
                })
                .ToListAsync();
        }
        public async Task<CategoryDto?> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new CategoryDto
                {
                    CategoryId = p.CategoryId,
                    Name = p.Name,
                    Description = p.Description,
                    ParentCategoryId = p.ParentCategoryId,
                    ImageUrl = p.ImageUrl,
                })
                .FirstOrDefaultAsync();
        }
        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto requestDto)
        {
            try
            {
                var category = requestDto.ToCategoryFromCreate();
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return category.ToCategoryDto();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<CategoryDto> UpdateCategoryAsync(int categoryId, UpdateCategoryRequestDto requestDto)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                throw new NotFoundException("Không tìm thấy danh mục sản phẩm.");
            }
            category.UpdateCategoryFromDto(requestDto);
            await _context.SaveChangesAsync();
            return category.ToCategoryDto();
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new NotFoundException("Không tìm thấy danh mục sản phẩm.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CategoryTreeDto>> GetCategoryTreeAsync()
        {
            var parentCategories = await _context.Categories
                .Where(c => c.ParentCategoryId == null)
                .Include(c => c.InverseParentCategory)
                .ToListAsync();

            var result = parentCategories.Select(c => new CategoryTreeDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Childs = c.InverseParentCategory.Select(child => new CategoryTreeDto
                {
                    CategoryId = child.CategoryId,
                    Name = child.Name
                }).ToList()
            }).ToList();

            return result;

        }
        public async Task<List<ProductCardDto>> GetAllProductsByCategoryIdAsync(int categoryId)
        {
            var now = DateTime.Now;
            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductImages)
                .Include(p => p.Categories)
                .Include(p => p.Ratings)
                .Include(p => p.ProductDiscounts)
                    .ThenInclude(pd => pd.Discount)
                .Where(p => p.Categories.Any(c => c.CategoryId == categoryId))
                .ToListAsync();

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

