using electro_shop_backend.Data;
using electro_shop_backend.Models.DTOs.Stock;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDbContext _context;
        public SupplierService(ApplicationDbContext context) => _context = context;
        public async Task<IActionResult> AddSupplierAsync(AddSupplierDTO addSupplierDTO)
        {
            try
            {
                var supplier = new Supplier
                {
                    SupplierName = addSupplierDTO.SupplierName,
                    SupplierAddress = addSupplierDTO.SupplierAddress,
                    SupplierContact = addSupplierDTO.SupplierContact,
                    CreatedAt = DateTime.Now
                };
                await _context.Suppliers.AddAsync(supplier);
                await _context.SaveChangesAsync();
                return new OkObjectResult(supplier);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new { message = e.Message });
            }
        }
        public async Task<IActionResult> DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return new NotFoundObjectResult(new { message = "Supplier not found" });
            }
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return new OkObjectResult(new { message = "Supplier deleted successfully" });
        }
        public async Task<IActionResult> GetAllSuppliersAsync()
        {
            var suppliers = await _context.Suppliers.ToListAsync();
            var suppliersDTO = new List<SupplierDTO>();
            foreach (var supplier in suppliers)
            {
                suppliersDTO.Add(supplier.ToSupplierDTOFromSupplier());
            }
            return new OkObjectResult(suppliersDTO);
        }
        public async Task<IActionResult> GetSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return new NotFoundObjectResult(new { message = "Supplier not found" });
            }
            return new OkObjectResult(supplier);
        }
        public async Task<IActionResult> UpdateSupplierAsync(int id, AddSupplierDTO updateSupplierDTO)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return new NotFoundObjectResult(new { message = "Supplier not found" });
            }
            supplier.SupplierName = updateSupplierDTO.SupplierName;
            supplier.SupplierAddress = updateSupplierDTO.SupplierAddress;
            supplier.SupplierContact = updateSupplierDTO.SupplierContact;
            await _context.SaveChangesAsync();
            return new OkObjectResult(supplier);
        }
    }
}
