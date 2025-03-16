using electro_shop_backend.Models.DTOs.Stock;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IStockService
    {
        Task<IActionResult> GetAllStockAsync();
        Task<IActionResult> GetStockAsync(int id);
        Task<IActionResult> AddStockAsync(AddStockImportDTO addStockImportDTO);
        Task<IActionResult> DeleteStockAsync(int id);
        Task<IActionResult> UpdateStockAsync(int id, AddStockImportDTO addStockImportDTO);
        Task<IActionResult> UpdateStockStatusAsync(int id, string status);
    }
}
