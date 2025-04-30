using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Exceptions.CustomExceptions;
using electro_shop_backend.Models.DTOs.ProductImage;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace electro_shop_backend.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService; // Service upload file

        public ProductImageService(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<List<ProductImageDto>> CreateProductImagesAsync(int productId, CreateProductImageDto requestDto)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            List<string> uploadedImageUrls = new List<string>();
            List<ProductImage> newProductImages = new List<ProductImage>();

            try
            {
                // Lặp qua từng file ảnh trong DTO
                foreach (var image in requestDto.Images)
                {
                    // Upload file và lấy URL trả về
                    var imageUrl = await _imageService.UploadImageAsync(image);
                    uploadedImageUrls.Add(imageUrl);

                    // Tạo mới đối tượng ProductImage với URL đã lấy
                    var newProductImage = new ProductImage
                    {
                        ProductId = productId,
                        ImageUrl = imageUrl
                    };

                    newProductImages.Add(newProductImage);
                    _context.ProductImages.Add(newProductImage);
                }
                await _context.SaveChangesAsync();

                // Chuyển entity sang DTO và trả về danh sách
                return newProductImages.Select(pi => pi.ToProductImageDto()).ToList();
            }
            catch (Exception)
            {
                // Nếu có lỗi, xóa các ảnh đã upload
                foreach (var imageUrl in uploadedImageUrls)
                {
                    await _imageService.DeleteImageByUrlAsync(imageUrl);
                }
                throw;
            }
        }

        public async Task<ProductImageDto> UpdateProductImageAsync(int productId, CreateProductImageDto requestDto)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            var productImage = await _context.ProductImages
                .FirstOrDefaultAsync(pi => pi.ProductId == productId);
            if (productImage == null)
            {
                throw new KeyNotFoundException("Product image not found");
            }

            // Ở update, giả sử bạn muốn thay thế toàn bộ ảnh hiện có bằng ảnh mới (với 1 file ảnh)
            // Nếu muốn update nhiều ảnh, cần có logic riêng
            var imageUrl = await _imageService.UploadImageAsync(requestDto.Images.First());
            productImage.ImageUrl = imageUrl;

            await _context.SaveChangesAsync();
            return productImage.ToProductImageDto();
        }

        public async Task<bool> DeleteProductImageAsync(int id)
        {
            var productImage = await _context.ProductImages.FindAsync(id);
            if (productImage == null)
            {
                throw new NotFoundException("Không tìm thấy ảnh sản phẩm.");
            }
            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
