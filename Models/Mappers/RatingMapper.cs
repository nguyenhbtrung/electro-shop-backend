﻿using electro_shop_backend.Models.DTOs.Rating;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class RatingMapper
    {
        public static Rating ToRatingFromCreate(this CreateRatingRequestDto requestDto, string userId)
        {
            return new Rating
            {
                ProductId = requestDto.ProductId,
                UserId = userId,
                RatingScore = requestDto.RatingScore,
                RatingContent = requestDto.RatingContent,
                Status = requestDto.Status,
                TimeStamp = requestDto.TimeStamp,
            };
        }
    }
}
