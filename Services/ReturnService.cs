﻿using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Rating;
using electro_shop_backend.Models.DTOs.Return;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class ReturnService : IReturnService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public ReturnService(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<List<AllReturnDto>> GetAllReturnAsync()
        {
            return await _context.Returns
                .AsNoTracking()
                .Select(p => new AllReturnDto
                {
                    ReturnId = p.ReturnId,
                    OrderId = p.OrderId,
                    Reason = p.Reason,
                    Detail = p.Detail,
                    Status = p.Status,
                    ReturnMethod = p.ReturnMethod,
                    Address = p.Address,
                    AdminComment = p.AdminComment,
                    TimeStamp = p.TimeStamp,
                })
                .ToListAsync();
        }

        public async Task<CreateReturnResponseDto> CreateReturnAsync(string userId, CreateReturnRequestDto requestDto)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == requestDto.OrderId);

            if (order == null || order.UserId != userId)
            {
                throw new BadRequestException("Đơn hàng không tồn tại hoặc không thuộc quyền sở hữu");
            }

            if (order.Status != "Successed")
            {
                throw new BadRequestException("Chỉ có thể yêu cầu hoàn trả cho đơn hàng đã giao");
            }

            var existingReturn = await _context.Returns.FirstOrDefaultAsync(r => r.OrderId == requestDto.OrderId);
            if (existingReturn != null)
            {
                throw new BadRequestException("Bạn đã gửi yêu cầu hoàn trả cho đơn hàng này");
            }

            if (requestDto.ReturnItems.Count == 0)
            {
                throw new BadRequestException("Không có sản phẩm nào được chọn để trả hàng.");
            }

            var orderItemsDictionary = order.OrderItems.ToDictionary(oi => oi.OrderItemId);

            foreach (var returnItem in requestDto.ReturnItems)
            {
                if (!orderItemsDictionary.TryGetValue(returnItem.OrderItemId, out OrderItem? value))
                {
                    throw new BadRequestException($"OrderItem với mã {returnItem.OrderItemId} không tồn tại trong đơn hàng.");
                }

                var orderItem = value;

                if (returnItem.ReturnQuantity <= 0 || returnItem.ReturnQuantity > orderItem.Quantity)
                {
                    throw new BadRequestException($"Số lượng trả của OrderItem {returnItem.OrderItemId} không hợp lệ. Số lượng cho phép trả: từ 1 đến {orderItem.Quantity}.");
                }
            }


            var newReturn = new Return
            {
                OrderId = requestDto.OrderId,
                Reason = requestDto.Reason,
                Detail = requestDto.Detail,
                ReturnMethod = requestDto.ReturnMethod.ToString().ToLower(),
                Status = ReturnStatus.Pending.ToString().ToLower(),
                ReturnItems = requestDto.ReturnItems.Select(i => new ReturnItem
                {
                    OrderItemId = i.OrderItemId,
                    ReturnQuantity = i.ReturnQuantity
                }).ToList()
            };

            List<string> uploadedImageUrls = new List<string>();
            try
            {
                foreach (var image in requestDto.EvidenceImages)
                {
                    var imageUrl = await _imageService.UploadImageAsync(image);
                    uploadedImageUrls.Add(imageUrl);
                    newReturn.ReturnImages.Add(new ReturnImage
                    {
                        ImageUrl = imageUrl
                    });
                }
                await _context.Returns.AddAsync(newReturn);
                await _context.SaveChangesAsync();
                return new CreateReturnResponseDto
                {
                    ReturnId = newReturn.ReturnId,
                    Status = newReturn.Status,
                    TimeStamp = newReturn.TimeStamp
                };
            }
            catch (Exception)
            {
                foreach (var imageUrl in uploadedImageUrls)
                {
                    await _imageService.DeleteImageByUrlAsync(imageUrl);
                }
                throw;
            }
            
        }

        public async Task<ReturnDto> UpdateReturnAsync(int returnId, UpdateReturnDto requestDto)
        {
            var returnEntity = await _context.Returns
                .FindAsync(returnId);
            if (returnEntity == null)
            {
                throw new NotFoundException("Not found");
            }
            requestDto.UpdateReturnDto(returnEntity);
            await _context.SaveChangesAsync();
            return returnEntity.ToReturnDto();
        }

        public async Task<bool> DeleteReturnAsync(int returnId)
        {
            var returnEntity = await _context.Returns
                .FirstOrDefaultAsync(r => r.ReturnId == returnId);
            if (returnEntity == null)
            {
                throw new NotFoundException("Not found");
            }
            _context.Returns.Remove(returnEntity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public enum ReturnStatus
    {
        Pending
    }
}
