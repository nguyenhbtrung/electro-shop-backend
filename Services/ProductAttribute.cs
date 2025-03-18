using electro_shop_backend.Data;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class ProductAttributeService: IProductAttributeService
    {
        private readonly ApplicationDbContext _context;

        public ProductAttributeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> AssignAttributeDetails(int productId, List<int> attributeDetailIds)
        {
            var product = await _context.Products
                .Include(p => p.ProductAttributeDetails)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return new NotFoundObjectResult("Product not found.");
            }
            var details = await _context.ProductAttributeDetails
                .Where(d => attributeDetailIds.Contains(d.AttributeDetailId))
                .ToListAsync();

            foreach (var detail in details)
            {
                if (!product.ProductAttributeDetails.Any(x => x.AttributeDetailId == detail.AttributeDetailId))
                {
                    product.ProductAttributeDetails.Add(detail);
                }
            }

            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                product.ProductId,
                AttributeDetails = product.ProductAttributeDetails.Select(d => new
                {
                    d.AttributeDetailId,
                    d.Value,
                    d.PriceModifier
                })
            });
        }
    }
}
