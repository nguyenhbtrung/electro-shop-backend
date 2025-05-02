using electro_shop_backend.Models.DTOs.Stock;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService) => _supplierService = supplierService;

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllSuppliers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _supplierService.GetAllSuppliersAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetSupplier([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _supplierService.GetSupplierAsync(id);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddSupplier([FromBody] AddSupplierDTO addSupplierDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _supplierService.AddSupplierAsync(addSupplierDTO);
        }
        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] AddSupplierDTO updateSupplierDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _supplierService.UpdateSupplierAsync(id, updateSupplierDTO);
        }

        [HttpDelete]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _supplierService.DeleteSupplierAsync(id);
        }
    }
}
