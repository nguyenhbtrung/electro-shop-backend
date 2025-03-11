using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Helpers
{
    public static class ProductCalculationValue
    {
        public static (decimal discountedPrice, string discountType, decimal discountValue) CalculateDiscount(Product product)
        {
            var now = DateTime.Now;
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

            return (discountedPrice, discountType, discountValue);
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

