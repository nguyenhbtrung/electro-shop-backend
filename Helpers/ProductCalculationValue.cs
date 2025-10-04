using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Helpers
{
    public static class ProductCalculationValue
    {
        public static (decimal originalPrice, decimal discountedPrice, string discountType, decimal discountValue) CalculateDiscount(Product product, List<int> selectedAttributeDetailIds)
        {
            decimal totalModifier = product.ProductAttributeDetails
                .Where(detail => selectedAttributeDetailIds.Contains(detail.AttributeDetailId))
                .Sum(detail => detail.PriceModifier);
            decimal basePrice = product.Price + totalModifier;

            var now = DateTime.Now;
            var effectiveDiscount = product.ProductDiscounts
                .Where(pd => pd.Discount != null &&
                             pd.Discount.StartDate <= now &&
                             pd.Discount.EndDate >= now)
                .Select(pd => pd.Discount)
                .FirstOrDefault();

            string discountType = string.Empty;
            decimal discountValue = 0;
            decimal discountedPrice = basePrice;
            Console.WriteLine(">>> Check discountType: " + effectiveDiscount?.DiscountType);
            Console.WriteLine(">>> Check DiscountValue: " + effectiveDiscount?.DiscountValue);
            if (effectiveDiscount != null)
            {
                discountType = effectiveDiscount.DiscountType;
                discountValue = effectiveDiscount.DiscountValue ?? 0;
                if (string.Equals(discountType, "Percentage", StringComparison.OrdinalIgnoreCase))
                {
                    discountedPrice = basePrice * (1 - discountValue / 100);
                }
                else if (string.Equals(discountType, "Flat Amount", StringComparison.OrdinalIgnoreCase))
                {
                    discountedPrice = basePrice - discountValue;
                }

                if (discountedPrice < 0)
                {
                    discountedPrice = 0;
                }
            }

            return (basePrice, discountedPrice, discountType, discountValue);
        }

        public static double CalculateAverageRating(Product product)
        {
            if (product.Ratings != null && product.Ratings.Any())
            {
                return product.Ratings.Average(r => r.RatingScore);
            }
            return 0;
        }
    }
}
