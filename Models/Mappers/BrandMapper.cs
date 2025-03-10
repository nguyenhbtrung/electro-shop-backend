using electro_shop_backend.Models.DTOs.Brand;
using electro_shop_backend.Models.Entities;
using System.Runtime.CompilerServices;
namespace electro_shop_backend.Models.Mappers
{
    public static class BrandMapper
    {
        public static BrandDto ToBrandDto(this Brand brand) //get
        {
            return new BrandDto
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                Country = brand.Country,
                ImageUrl = brand.ImageUrl,
                Info = brand.Info,
            };
        }
        public static Brand ToBrandFromCreateDto(this CreateBrandRequestDto requestDto) //create
        {
            return new Brand
            {
                BrandName = requestDto.BrandName,
                Country = requestDto.Country,
                ImageUrl = requestDto.ImageUrl,
                Info = requestDto.Info,
            };
        }
        public static void  UpdateBrandDto(this Brand brand,UpdateBrandRequestDto requestDto) //update
        {
            brand.BrandName =requestDto.BrandName;
            brand.Country = requestDto.Country;
            brand.ImageUrl = requestDto.ImageUrl;
            brand.Info = requestDto.Info;
        
        }
    }
}
