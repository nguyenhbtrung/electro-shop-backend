using electro_shop_backend.Data;
using electro_shop_backend.Models.DTOs.Stock;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class StockService : IStockService
    {
        private readonly ApplicationDbContext _context;
        public StockService(ApplicationDbContext context) => _context = context;
        public async Task<IActionResult> GetAllStockAsync()
        {
            try
            {
                var allStock = await _context.StockImports.ToListAsync();
                return new OkObjectResult(allStock);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> GetStockAsync(int id)
        {
            try
            {
                var stockImport = await _context.StockImports
                    .Include(si => si.StockImportDetails)
                    .ThenInclude(sid => sid.Product)
                    .FirstOrDefaultAsync(si => si.StockImportId == id);
                if (stockImport == null)
                {
                    return new NotFoundResult();
                }
                var stockImportDTO = stockImport.ToStockImportDTOFromStock();
                // đoạn này đang lấy full info của product, hơi thừa, sửa lại
                // sửa lại, thay vì lấy supplierId thì lấy supplierName
                return new OkObjectResult(stockImport);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> AddStockAsync(AddStockImportDTO addStockImportDTO)
        {
            try
            {
                var stockImport = addStockImportDTO.ToStockImport();
                _context.StockImports.Add(stockImport);
                await _context.SaveChangesAsync();
                return new OkObjectResult("Stock added successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> DeleteStockAsync(int id)
        {
            // Implementation for deleting a specific stock by id
            return new OkResult();
        }

        public async Task<IActionResult> UpdateStockAsync(AddStockImportDTO addStockImportDTO)
        {
            // Implementation for updating existing stock
            return new OkResult();
        }
    }
}
