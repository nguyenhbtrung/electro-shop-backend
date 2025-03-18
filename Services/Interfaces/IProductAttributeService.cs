using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IProductAttributeService
    {
        Task<IActionResult> AssignAttributeDetails(int productId, List<int> attributeDetailIds);
    }
}
