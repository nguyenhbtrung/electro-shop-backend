using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Exceptions.CustomExceptions;
using electro_shop_backend.Helpers;
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
                    if (errorMessage.Contains("fk__category__parent__3a81b327"))
                    {
                        throw new Exception("Không được xóa danh mục cha khi đang tồn tại 1 danh mục con");
                    }
                    else if (errorMessage.Contains("fk__product_c__categ__4222d4ef"))
                    {
                        throw new Exception("Không được xóa danh mục khi tồn tại 1 sản phẩm thuộc danh mục");
                    }
                }
                throw;
            }
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
                var selectedAttributeDetailIds = new List<int>();
                var (originalPrice, discountedPrice, discountType, discountValue) = ProductCalculationValue.CalculateDiscount(product, selectedAttributeDetailIds);
                double avgRating = ProductCalculationValue.CalculateAverageRating(product);
                return new ProductCardDto
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Images = product.ProductImages?
                                .Where(pi => !string.IsNullOrWhiteSpace(pi.ImageUrl))
                                .Select(pi => pi.ImageUrl)
                                .ToList() ?? new List<string>(),
                    OriginalPrice = originalPrice,
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

