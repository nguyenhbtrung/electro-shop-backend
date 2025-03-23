using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs;
using electro_shop_backend.Models.DTOs.Discount;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.DTOs.ProductAttribute;
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
                .Include(p => p.Brand)
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

                var (originalPrice, discountedPrice, discountType, discountValue) = ProductCalculationValue.CalculateDiscount(p, selectedAttributeDetailIds);
                productDto.OriginalPrice = originalPrice;
                productDto.DiscountValue = discountValue;
                productDto.DiscountType = discountType;
                productDto.DiscountedPrice = discountedPrice;
                productDto.AverageRating = ProductCalculationValue.CalculateAverageRating(p);
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
                .Include(p => p.Ratings)
                .Include(p => p.ProductDiscounts)
                    .ThenInclude(pd => pd.Discount)
                .Include(p => p.ProductAttributeDetails)
                    .ThenInclude(d => d.ProductAttribute)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
                return null;

            var selectedAttributeDetailIds = new List<int>();
            var productDto = ProductMapper.ToProductDto(product);
            productDto.ProductImages = product.ProductImages
                .Select(ProductImageMapper.ToProductImageDto)
                .ToList();
            productDto.Categories = product.Categories
                .Select(CategoryMapper.ToCategoryIdDto)
                .ToList();
            productDto.ProductAttributeDetail = product.ProductAttributeDetails
                .Select(d => new ProductAttributeDetailDto
                {
                    AttributeDetailId = d.AttributeDetailId,
                    Value = d.Value,
                    PriceModifier = d.PriceModifier,
                    ProductAttributeId = d.ProductAttributeId,
                    ProductAttributeName = d.ProductAttribute.Name
                })
                .ToList();
            var (originalPrice, discountedPrice, discountType, discountValue) = ProductCalculationValue.CalculateDiscount(product, selectedAttributeDetailIds);
            productDto.OriginalPrice = originalPrice;
            productDto.DiscountValue = discountValue;
            productDto.DiscountType = discountType;
            productDto.DiscountedPrice = discountedPrice;
            productDto.AverageRating = ProductCalculationValue.CalculateAverageRating(product);
            productDto.Brand = product.Brand != null ? BrandMapper.ToBrandDto(product.Brand) : null;
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
            var product = await _context.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                throw new NotFoundException("Không tìm thấy sản phẩm.");
            }
            product.UpdateProductFromDto(requestDto);
            if (requestDto.CategoryIds != null)
            {
                var newCategories = await _context.Categories.Where(c => requestDto.CategoryIds.Contains(c.CategoryId)).ToListAsync();
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

        public async Task<List<ProductCardDto>> GetDiscountedProductsAsync(ProductQuery productQuery)
        {
            var now = DateTime.Now;
            int skipNumber = (productQuery.PageNumber - 1) * productQuery.PageSize;
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

        public async Task<List<ProductDto>> GetRecommendedProductsAsync(int productId)
        {
            var targetProduct = await _context.Products
                .AsNoTracking()
                .Include(p => p.Brand)
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (targetProduct == null)
                throw new NotFoundException("Không tìm thấy sản phẩm tham chiếu.");
            var allProducts = await _context.Products
                .AsNoTracking()
                .Include(p => p.Brand)
                .Include(p => p.Categories)
                .Where(p => p.ProductId != productId)
                .ToListAsync();
            double CalculateSimilarity(Product p1, Product p2)
            {
                int score = 0;
                if (!string.IsNullOrEmpty(p1.Name) && !string.IsNullOrEmpty(p2.Name))
                {
                    if (p1.Name.IndexOf(p2.Name, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        p2.Name.IndexOf(p1.Name, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        score += 5;
                    }
                }

                if (!string.IsNullOrEmpty(p1.Info) && !string.IsNullOrEmpty(p2.Info))
                {
                    if (p1.Info.IndexOf(p2.Info, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        p2.Info.IndexOf(p1.Info, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        score += 3;
                    }
                }

                if (p1.Brand != null && p2.Brand != null)
                {
                    if (p1.Brand.BrandId == p2.Brand.BrandId)
                    {
                        score += 4;
                    }
                }
                if (p1.Categories != null && p2.Categories != null)
                {
                    int commonCategories = p1.Categories.Count(c1 =>
                        p2.Categories.Any(c2 => c1.CategoryId == c2.CategoryId));
                    score += commonCategories * 3;
                }

                return score;
            }

            var recommendedProducts = allProducts
                .Select(p => new { Product = p, SimilarityScore = CalculateSimilarity(targetProduct, p) })
                .Where(x => x.SimilarityScore > 0)
                .OrderByDescending(x => x.SimilarityScore)
                .Take(5)
                .Select(x => x.Product)
                .ToList();
            var recommendedDtos = recommendedProducts.Select(p => p.ToProductDto()).ToList();

            return recommendedDtos;
        }
    }
}
