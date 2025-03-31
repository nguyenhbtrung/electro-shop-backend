using Azure;
using electro_shop_backend.Data;
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
        private readonly IReturnStatusHistoryService _returnStatusHistoryService;

        public ReturnService(ApplicationDbContext context, IImageService imageService, IReturnStatusHistoryService returnStatusHistoryService)
        {
            _context = context;
            _imageService = imageService;
            _returnStatusHistoryService = returnStatusHistoryService;
        }

        public async Task<List<AllReturnDto>> GetAllReturnAsync()
        {
            return await _context.Returns
                .AsNoTracking()
                .OrderByDescending(r => r.TimeStamp)
                .Select(p => new AllReturnDto
                {
                    ReturnId = p.ReturnId,
                    OrderId = p.OrderId,
                    Status = p.Status,
                    ReturnMethod = p.ReturnMethod,
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

            if (order.Status != "successed")
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
                await _returnStatusHistoryService.CreateReturnStatusHistoryAsync(new CreateReturnStatusHistoryRequestDto
                {
                    ReturnId = newReturn.ReturnId,
                    Status = ReturnStatus.Pending,
                    ChangedAt = newReturn.TimeStamp
                });
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

        public async Task<ReturnDetailResponseDto> GetReturnByIdAsync(int returnId)
        {
            var existingReturn = await _context.Returns
                .AsNoTracking()
                .Include(r => r.ReturnHistories)
                .Select(r => new ReturnDetailResponseDto
                {
                    ReturnId = r.ReturnId,
                    OrderId = r.OrderId,
                    Reason = r.Reason,
                    Detail = r.Detail,
                    Status = r.Status,
                    ReturnMethod = r.ReturnMethod,
                    AdminComment = r.AdminComment,
                    CreatedAt = r.TimeStamp,
                    ReturnHistories = r.ReturnHistories.Select(rh => new ReturnHistoryDto
                    {
                        Status = rh.Status,
                        ChangedAt = rh.ChangedAt
                    }).ToList(),
                })
                .FirstOrDefaultAsync(r => r.ReturnId == returnId);
            if (existingReturn == null)
            {
                throw new NotFoundException("Không tìm thấy yêu cầu hoàn trả");
            }

            await GetReturnProductsById(returnId, existingReturn.ReturnProducts);

            return existingReturn;


            //var returnItems = await _context.ReturnItems
            //    .AsNoTracking()
            //    .Include(ri => ri.OrderItem)
            //    .ThenInclude(oi => oi!.Product)
            //    .Where(ri => ri.ReturnId == returnId)
            //    .ToListAsync();
            //foreach (var item in returnItems)
            //{
            //    var returnProduct = new ReturnProductDto
            //    {
            //        ProductId = item?.OrderItem?.ProductId,
            //        Name = item?.OrderItem?.Product?.Name,
            //        ReturnQuantity = item?.ReturnQuantity
            //    };
            //    var img = await _context.ProductImages
            //        .AsNoTracking()
            //        .Select(i => new
            //        {
            //            i.ProductId,
            //            i.ImageUrl
            //        })
            //        .FirstOrDefaultAsync(i => i.ProductId == returnProduct.ProductId);
            //    returnProduct.Image = img?.ImageUrl;
            //    existingReturn.ReturnProducts.Add(returnProduct);
            //}
            //return existingReturn;
        }

        private async Task GetReturnProductsById(int returnId, List<ReturnProductDto> returnProducts)
        {
            var returnItems = await _context.ReturnItems
                            .AsNoTracking()
                            .Include(ri => ri.OrderItem)
                            .ThenInclude(oi => oi!.Product)
                            .Select(ri => new
                            {
                                ri.ReturnId,
                                ri!.OrderItem!.ProductId,
                                ri!.OrderItem!.Product!.Name,
                                ri.ReturnQuantity
                            })
                            .Where(ri => ri.ReturnId == returnId)
                            .ToListAsync();
            var productIds = returnItems
                .Where(ri => ri.ProductId != null)
                .Select(ri => ri.ProductId)
                .ToList();
            var images = await _context.ProductImages
                .AsNoTracking()
                .Where(i => productIds.Contains(i.ProductId))
                .GroupBy(i => i.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ImageUrl = g.OrderBy(i => i.ProductId)
                                .Select(i => i.ImageUrl)
                                .FirstOrDefault()
                })
                .ToListAsync();
            var productImageDict = images.ToDictionary(i => i.ProductId, i => i.ImageUrl);

            foreach (var item in returnItems)
            {
                var returnProduct = new ReturnProductDto
                {
                    ProductId = item?.ProductId,
                    Name = item?.Name,
                    ReturnQuantity = item?.ReturnQuantity,
                    Image = item?.ProductId != null && productImageDict.TryGetValue(item?.ProductId, out var imageUrl)
                                 ? imageUrl
                                 : null
                };

                returnProducts.Add(returnProduct);
            }
        }

        public async Task<List<ReturnUserHistoryDto>> GetUserReturnHistoryAsync(string userId)
        {
            var returns = await _context.Returns
                .AsNoTracking()
                .Include(r => r.Order)
                .Include(r => r.ReturnItems)
                .ThenInclude(ri => ri.OrderItem)
                .Where(r => r.Order!.UserId == userId)
                .OrderByDescending(r => r.TimeStamp)
                .Select(r => new ReturnUserHistoryDto
                {
                    ReturnId = r.ReturnId,
                    Status = r.Status,
                    ReturnMethod = r.ReturnMethod,
                    TimeStamp = r.TimeStamp,
                    ReturnProducts = r.ReturnItems.Select(ri => new ReturnProductDto
                    {
                        ProductId = ri.OrderItem!.ProductId,
                        Name = ri.OrderItem.ProductName,
                        ReturnQuantity = ri.ReturnQuantity
                    }).ToList()
                })
                .ToListAsync();

            return returns;
        }

        public async Task<ReturnDetailAdminResponseDto> GetReturnByAdminAsync(int returnId)
        {
            var existingReturn = await _context.Returns
                .AsNoTracking()
                .Include(r => r.ReturnImages)
                .Include(r => r.Order)
                .ThenInclude(o => o.User)
                .Select(r => new ReturnDetailAdminResponseDto
                {
                    ReturnId = r.ReturnId,
                    OrderId = r.OrderId,
                    OrderDate = r.Order!.TimeStamp,
                    CustomerName = r.Order.User.FullName ?? r.Order.User.UserName,
                    Address = r.Order.User.Address,
                    PhoneNumber = r.Order.User.PhoneNumber,
                    Reason = r.Reason,
                    Detail = r.Detail,
                    Status = r.Status,
                    ReturnMethod = r.ReturnMethod,
                    CreatedAt = r.TimeStamp,
                    ReturnImageUrls = r.ReturnImages.Select(ri => ri.ImageUrl).ToList(),
                })
                .FirstOrDefaultAsync(r => r.ReturnId == returnId);
            if (existingReturn == null)
            {
                throw new NotFoundException("Không tìm thấy yêu cầu hoàn trả");
            }

            await GetReturnProductsById(returnId, existingReturn.ReturnProducts);

            return existingReturn;
        }

        public async Task<UpdateReturnStatusResponseDto> UpdateReturnStatusAsync(int returnId, UpdateReturnStatusRequestDto requestDto)
        {
            var existingReturn = await _context.Returns.FirstOrDefaultAsync(r => r.ReturnId == returnId);
            if (existingReturn == null)
            {
                throw new NotFoundException("không tìm thấy yêu cầu hoàn trả");
            }
            existingReturn.Status = requestDto.ReturnStatus.ToString().ToLower();
            existingReturn.AdminComment = requestDto.AdminComment;
            var newReturnHistory = new ReturnHistory
            {
                ReturnId = returnId,
                Status = requestDto.ReturnStatus.ToString().ToLower()
            };
            await _context.ReturnHistories.AddAsync(newReturnHistory);
            await _context.SaveChangesAsync();
            return new UpdateReturnStatusResponseDto
            {
                ReturnId = existingReturn.ReturnId,
                ReturnStatus = existingReturn.Status,
                AdminComment = existingReturn.AdminComment
            };
        }

        public async Task<PaymentDTO> GetPaymentByOrderIdAsync(int orderId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
            if (payment == null)
            {
                throw new BadRequestException("Không tìm thấy payment có orderId này");
            }

            return new PaymentDTO
            {
                OrderId = orderId,
                Amount = payment.Amount,
                PayDate = payment.PaidAt?.ToString("yyyyMMddHHmmss"),
                PaymentMethod = payment.PaymentMethod,
                TxnRef = payment.TxnRef
            };
        }
    }

    public enum ReturnStatus
    {
        Pending,
        Approved,
        Processing,
        Completed,
        Rejected,
        Canceled
    }
}
