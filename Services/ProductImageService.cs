﻿using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Brand;
using electro_shop_backend.Models.DTOs.ProductImage;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class ProductImageService:IProductImageService
    {
        private readonly ApplicationDbContext _context;
        public ProductImageService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ProductImageDto> CreateProductImageAsync(int productId,CreateProductImageDto requestDto)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            var newProductImage = requestDto.ToProductImageFromCreate();
            newProductImage.ProductId = productId;
            _context.ProductImages.Add(newProductImage);
            await _context.SaveChangesAsync();

            return ProductImageMapper.ToProductImageDto(newProductImage);
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
            requestDto.ToProductImageFromUpdate(productImage);
            await _context.SaveChangesAsync();
            return productImage.ToProductImageDto();
        }

        public async Task<bool> DeleteProductImageAsync(int id)
        {
            var productimage = await _context.ProductImages.FindAsync(id);
            if (productimage == null)
            {
                throw new NotFoundException("Không tìm thấy ảnh sản phẩm.");
            }
            _context.ProductImages.Remove(productimage);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
