using electro_shop_backend.Models.DTOs.Stock;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<IActionResult> GetAllSuppliersAsync();
        Task<IActionResult> GetSupplierAsync(int id);
        Task<IActionResult> AddSupplierAsync(AddSupplierDTO addSupplierDTO);
        Task<IActionResult> UpdateSupplierAsync(int id, AddSupplierDTO updateSupplierDTO);
        Task<IActionResult> DeleteSupplierAsync(int id);
    }
}
