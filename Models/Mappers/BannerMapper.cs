using electro_shop_backend.Models.DTOs.Banner;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class BannerMapper
    {
        public static Banner FromBannerDTOToBanner(BannerDTO bannerDto)
        {
            return new Banner
            {
                ImageUrl = bannerDto.ImageUrl,
                Link = bannerDto.Link,
                Title = bannerDto.Title,
                Position = bannerDto.Position,
                StartDate = bannerDto.StartDate,
                EndDate = bannerDto.EndDate
            };
        }
    }
}
