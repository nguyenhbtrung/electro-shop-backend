using electro_shop_backend.Data;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Price;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ProductPricingService : IProductPricingService
{
    private readonly ApplicationDbContext _context;

    public ProductPricingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PriceResultDto> CalculatePriceAsync(int productId, List<int> selectedAttributeDetailIds)
    {
        var product = await _context.Products
            .Include(p => p.ProductAttributeDetails)
            .Include(p => p.ProductDiscounts)
                .ThenInclude(pd => pd.Discount)
            .FirstOrDefaultAsync(p => p.ProductId == productId);

        if (product == null)
        {
            throw new Exception("Sản phẩm không tồn tại.");
        }

        var (originalPrice, discountedPrice, discountType, discountValue) = ProductCalculationValue.CalculateDiscount(product, selectedAttributeDetailIds);

        return new PriceResultDto
        {
            OriginalPrice = originalPrice,
            DiscountedPrice = discountedPrice,
            DiscountType = discountType,
            DiscountValue = discountValue
        };
    }
}
