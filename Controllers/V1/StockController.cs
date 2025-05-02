using electro_shop_backend.Models.DTOs.Stock;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        public StockController(IStockService stockService) => _stockService = stockService;
        [HttpGet]
        public async Task<IActionResult> GetAllStock()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _stockService.GetAllStockAsync();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStock([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _stockService.GetStockAsync(id);
        }
        [HttpPost]
        public async Task<IActionResult> AddStock(AddStockImportDTO addStockImportDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _stockService.AddStockAsync(addStockImportDTO);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, AddStockImportDTO addStockImportDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _stockService.UpdateStockAsync(id, addStockImportDTO);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _stockService.DeleteStockAsync(id);
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateStockStatus([FromRoute] int id, string status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _stockService.UpdateStockStatusAsync(id, status);
        }
    }
}
