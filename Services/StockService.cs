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
                var allStock = await _context.StockImports
                    .Include(si => si.Supplier)
                    .ToListAsync();
                var allStockDTO = allStock.Select(stock => stock.ToListStockDTOFromStock()).ToList();
                return new OkObjectResult(allStockDTO);
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
                    .Include(si => si.Supplier)
                    .FirstOrDefaultAsync(si => si.StockImportId == id);
                if (stockImport == null)
                {
                    return new NotFoundResult();
                }
                var stockImportDTO = stockImport.ToStockImportDTOFromStock();
                return new OkObjectResult(stockImportDTO);
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
                stockImport.CreatedAt = System.DateTime.UtcNow;
                stockImport.StockImportStatus = "Pending";
                _context.StockImports.Add(stockImport);
                await _context.SaveChangesAsync();
                return new OkObjectResult("Stock added successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> UpdateStockAsync(int id, AddStockImportDTO addStockImportDTO)
        {
            try
            {
                var stockImport = await _context.StockImports
                    .Include(si => si.StockImportDetails)
                    .FirstOrDefaultAsync(si => si.StockImportId == id);
                if (stockImport == null)
                {
                    return new NotFoundResult();
                }
                if (stockImport.StockImportStatus != "Pending")
                {
                    return new BadRequestObjectResult("Stock is not pending, cannot update") { StatusCode = 400 };
                }
                stockImport.UpdateStockImportFromDTO(addStockImportDTO);
                await _context.SaveChangesAsync();
                return new OkObjectResult("Stock updated successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> DeleteStockAsync(int id)
        {
            try
            {
                var stockImport = await _context.StockImports.FindAsync(id);
                if (stockImport == null)
                {
                    return new NotFoundResult();
                }
                _context.StockImports.Remove(stockImport);
                await _context.SaveChangesAsync();
                return new OkObjectResult("Stock deleted successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> UpdateStockStatusAsync(int id, string status)
        {
            try
            {
                var validStatusTransitions = new Dictionary<string, List<string>>
                {
                    { "Pending", new List<string> { "Approved", "Canceled" } },
                    { "Approved", new List<string> { "Completed", "Canceled" } },
                    { "Completed", new List<string> { "Returned" } },
                    { "Canceled", new List<string>() },
                    { "Returned", new List<string>() }
                };

                if (!validStatusTransitions.ContainsKey(status))
                {
                    return new BadRequestObjectResult("Invalid status") { StatusCode = 400 };
                }

                var stockImport = await _context.StockImports
                    .Include(si => si.StockImportDetails)
                    .FirstOrDefaultAsync(si => si.StockImportId == id);
                if (stockImport == null)
                {
                    return new NotFoundResult();
                }

                if (!validStatusTransitions[stockImport.StockImportStatus].Contains(status))
                {
                    return new BadRequestObjectResult("Invalid status transition") { StatusCode = 400 };
                }

                if (status == "Returned")
                {
                    foreach (var stockImportDetail in stockImport.StockImportDetails)
                    {
                        var product = await _context.Products.FindAsync(stockImportDetail.ProductId);
                        if (product.Stock < stockImportDetail.Quantity)
                        {
                            return new BadRequestObjectResult("Not enough stock to return") { StatusCode = 400 };
                        }
                    }

                    foreach (var stockImportDetail in stockImport.StockImportDetails)
                    {
                        var product = await _context.Products.FindAsync(stockImportDetail.ProductId);
                        product.Stock -= stockImportDetail.Quantity;
                    }
                }

                stockImport.StockImportStatus = status;
                await _context.SaveChangesAsync();

                if (status == "Completed")
                {
                    foreach (var stockImportDetail in stockImport.StockImportDetails)
                    {
                        var product = await _context.Products.FindAsync(stockImportDetail.ProductId);
                        product.Stock += stockImportDetail.Quantity;
                    }
                    await _context.SaveChangesAsync();
                }

                return new OkObjectResult("Stock status updated successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex) { StatusCode = 500 };
            }
        }
    }
}
