using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Discount;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace electro_shop_backend.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductImages)
                .Include(p => p.Categories)
                .Include(p => p.Ratings)
                .Include(p => p.ProductDiscounts)
                    .ThenInclude(pd => pd.Discount)
                .Include(p=>p.Brand)
                .ToListAsync();
            var productDtos = products.Select(p =>
            {
                var selectedAttributeDetailIds = new List<int>();
                var productDto = ProductMapper.ToProductDto(p);
                productDto.ProductImages = p.ProductImages
                    .Select(ProductImageMapper.ToProductImageDto)
                    .ToList();
                productDto.Categories = p.Categories
                    .Select(CategoryMapper.ToCategoryIdDto)
                    .ToList();
                productDto.Brand = p.Brand != null ? BrandMapper.ToBrandDto(p.Brand) : null;
                var (discountedPrice, discountType, discountValue) = ProductCalculationValue.CalculateDiscount(p, selectedAttributeDetailIds);
                productDto.DiscountValue = discountValue;
                productDto.DiscountType = discountType;
                productDto.DiscountedPrice = discountedPrice;
                productDto.AverageRating= ProductCalculationValue.CalculateAverageRating(p);
                return productDto;
            }).ToList();

            return productDtos;
        }
        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products
            .AsNoTracking()
            .Include(p => p.ProductImages)
            .Include(p => p.Categories)
            .Include(p => p.Brand)
            .Include(p=>p.Ratings)
            .Include(p => p.ProductDiscounts)
                .ThenInclude(pd => pd.Discount)
            .FirstOrDefaultAsync(p => p.ProductId == productId); 

            if (product == null) return null;
            var selectedAttributeDetailIds = new List<int>();
            var productDto = ProductMapper.ToProductDto(product);
            productDto.ProductImages = product.ProductImages
                .Select(ProductImageMapper.ToProductImageDto)
                .ToList();
            productDto.Categories = product.Categories
                .Select(CategoryMapper.ToCategoryIdDto)
                .ToList();
            var (discountedPrice, discountType, discountValue) = ProductCalculationValue.CalculateDiscount(product, selectedAttributeDetailIds);
            productDto.DiscountValue = discountValue;
            productDto.DiscountType = discountType;
            productDto.DiscountedPrice = discountedPrice;
            productDto.AverageRating = ProductCalculationValue.CalculateAverageRating(product);
            productDto.Brand =product.Brand!=null?BrandMapper.ToBrandDto(product.Brand) : null;
            return productDto;
        }
        public async Task<ProductDto> CreateProductAsync(CreateProductRequestDto requestDto)
        {
            try
            {
                var product = requestDto.ToProductFromCreate();
                if (requestDto.CategoryIds != null && requestDto.CategoryIds.Any())
                {
                    var categories = await _context.Categories
                        .Where(c => requestDto.CategoryIds.Contains(c.CategoryId))
                        .ToListAsync();
                    product.Categories = categories;
                }
                if (requestDto.BrandId > 0)
                {
                    var brand = await _context.Brands
                        .FirstOrDefaultAsync(c => c.BrandId == requestDto.BrandId);

                    product.Brand = brand;
                }
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                if (!string.IsNullOrWhiteSpace(requestDto.ImageUrl))
                {
                    var productImage = new ProductImage 
                    {
                        ProductId = product.ProductId,
                        ImageUrl = requestDto.ImageUrl,
                    };
                    await _context.ProductImages.AddAsync(productImage);
                    await _context.SaveChangesAsync();
                }
                return product.ToProductDto();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ProductDto> UpdateProductAsync(int productId, UpdateProductRequestDto requestDto)
        {
            var product = await _context.Products.Include(p=>p.Categories).FirstOrDefaultAsync(p=>p.ProductId==productId);
            if (product == null)
            {
                throw new NotFoundException("Không tìm thấy sản phẩm.");
            }
            product.UpdateProductFromDto(requestDto);
            if (requestDto.CategoryIds != null)
            {
                var newCategories = await _context.Categories.Where(c=>requestDto.CategoryIds.Contains(c.CategoryId)).ToListAsync();
                product.Categories.Clear();
                foreach (var category in newCategories)
                {
                    product.Categories.Add(category);
                }
            }
            await _context.SaveChangesAsync();
            return product.ToProductDto();
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                throw new NotFoundException("Không tìm thấy sản phẩm.");
            }
            product.Categories.Clear();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductCardDto>> GetAllProductsByUserAsync(ProductQuery productQuery)
        {
            int skipNumber = (productQuery.PageNumber - 1) * productQuery.PageSize;

            // Query sản phẩm, bao gồm các quan hệ cần thiết
            var products = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Ratings)
                .Include(p => p.ProductDiscounts)
                    .ThenInclude(pd => pd.Discount)
                .Skip(skipNumber)
                .Take(productQuery.PageSize)
                .ToListAsync();

            var productDtos = products.Select(product =>
            {
                var selectedAttributeDetailIds = new List<int>();
                var (discountedPrice, discountType, discountValue) = ProductCalculationValue.CalculateDiscount(product, selectedAttributeDetailIds);
                double avgRating = ProductCalculationValue.CalculateAverageRating(product);

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

        public async Task<List<ProductCardDto>> GetDiscountedProductsAsync(ProductQuery productQuery)
        {
            // Lấy thời điểm hiện tại
            var now = DateTime.Now;
            int skipNumber = (productQuery.PageNumber - 1) * productQuery.PageSize;

            // Query các sản phẩm có discount đang hiệu lực
            var products = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Ratings)
                .Include(p => p.ProductDiscounts)
                    .ThenInclude(pd => pd.Discount)
                .Where(p => p.ProductDiscounts.Any(pd =>
                          pd.Discount != null &&
                          pd.Discount.StartDate <= now &&
                          pd.Discount.EndDate >= now))
                .Skip(skipNumber)
                .Take(productQuery.PageSize)
                .ToListAsync();

            var productDtos = products.Select(product =>
            {
                var selectedAttributeDetailIds = new List<int>();
                var (discountedPrice, discountType, discountValue) = ProductCalculationValue.CalculateDiscount(product, selectedAttributeDetailIds);
                double avgRating = ProductCalculationValue.CalculateAverageRating(product);

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

        public async Task<ProductGroupedDto> GetProductsByDiscountIdAsync(int? discountId, string? search)
        {
            IQueryable<Product> query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            List<Product> selectedProducts = new List<Product>();
            List<Product> availableProducts = new List<Product>();

            if (discountId.HasValue)
            {
                selectedProducts = await query
                    .Where(p => p.ProductDiscounts.Any(pd => pd.DiscountId == discountId.Value))
                    .ToListAsync();

                availableProducts = await query
                    .Where(p => !p.ProductDiscounts.Any(pd => pd.DiscountId == discountId.Value))
                    .ToListAsync();
            }
            else
            {
                availableProducts = await query.ToListAsync();
            }

            return new ProductGroupedDto
            {
                Available = availableProducts.Select(p => p.ToProductDto()),
                Selected = selectedProducts.Select(p => p.ToProductDto())
            };
        }
    }
}

