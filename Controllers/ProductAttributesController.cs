using electro_shop_backend.Data;
using electro_shop_backend.Models.DTOs;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductAttributesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductAttributesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAttributes()
        {
            var attributes = await _context.ProductAttributes
                .Include(a => a.Details)
                .ToListAsync();
            return Ok(attributes);
        }

        [HttpGet("{attributeId}")]
        public async Task<IActionResult> GetAttribute(int attributeId)
        {
            var attribute = await _context.ProductAttributes
                .Include(a => a.Details)
                .FirstOrDefaultAsync(a => a.AttributeId == attributeId);
            if (attribute == null)
                return NotFound();
            return Ok(attribute);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateAttribute([FromBody] ProductAttributeDto attributeDto)
        {
            var attribute = new ProductAttribute
            {
                Name = attributeDto.Name
            };

            _context.ProductAttributes.Add(attribute);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAttribute), new { attributeId = attribute.AttributeId }, attribute);
        }

        [HttpPut("{attributeId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateAttribute(int attributeId, [FromBody] ProductAttributeDto attributeDto)
        {
            var attribute = await _context.ProductAttributes.FindAsync(attributeId);
            if (attribute == null)
                return NotFound();

            attribute.Name = attributeDto.Name;
            _context.ProductAttributes.Update(attribute);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{attributeId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteAttribute(int attributeId)
        {
            var attribute = await _context.ProductAttributes.FindAsync(attributeId);
            if (attribute == null)
                return NotFound();

            _context.ProductAttributes.Remove(attribute);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ------ Các API Nested cho AttributeDetail ------

        [HttpGet("{attributeId}/details")]
        public async Task<IActionResult> GetAttributeDetails(int attributeId)
        {
            var attribute = await _context.ProductAttributes
                .Where(a => a.AttributeId == attributeId)
                .Select(a => new
                {
                    a.AttributeId,
                    a.Name,
                    Details = a.Details.Select(d => new
                    {
                        d.AttributeDetailId,
                        d.Value,
                        d.PriceModifier,
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (attribute == null)
                return NotFound("Product attribute not found.");

            return Ok(attribute.Details);
        }

        [HttpGet("{attributeId}/details/{detailId}")]
        public async Task<IActionResult> GetAttributeDetail(int attributeId, int detailId)
        {
            var detailDto = await _context.ProductAttributeDetails
                .Where(d => d.ProductAttributeId == attributeId && d.AttributeDetailId == detailId)
                .Select(d => new
                {
                    d.AttributeDetailId,
                    d.Value,
                    d.PriceModifier,
                }).FirstOrDefaultAsync();

            if (detailDto == null)
                return NotFound();

            return Ok(detailDto);
        }


        [HttpPost("{attributeId}/details")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateAttributeDetail(int attributeId, [FromBody] ProductAttributeDetailDto detailDto)
        {
            var attribute = await _context.ProductAttributes.FindAsync(attributeId);
            if (attribute == null)
                return NotFound("Product attribute not found.");

            var detail = new AttributeDetail
            {
                Value = detailDto.Value,
                PriceModifier = detailDto.PriceModifier,
                ProductAttributeId = attributeId
            };

            _context.ProductAttributeDetails.Add(detail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAttributeDetail), new { attributeId = attributeId, detailId = detail.AttributeDetailId }, detail);
        }

        [HttpPut("{attributeId}/details/{detailId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateAttributeDetail(int attributeId, int detailId, [FromBody] ProductAttributeDetailDto detailDto)
        {
            var detail = await _context.ProductAttributeDetails
                .FirstOrDefaultAsync(d => d.ProductAttributeId == attributeId && d.AttributeDetailId == detailId);
            if (detail == null)
                return NotFound();

            detail.Value = detailDto.Value;
            detail.PriceModifier = detailDto.PriceModifier;

            _context.ProductAttributeDetails.Update(detail);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{attributeId}/details/{detailId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteAttributeDetail(int attributeId, int detailId)
        {
            var detail = await _context.ProductAttributeDetails
                .FirstOrDefaultAsync(d => d.ProductAttributeId == attributeId && d.AttributeDetailId == detailId);
            if (detail == null)
                return NotFound();

            _context.ProductAttributeDetails.Remove(detail);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
