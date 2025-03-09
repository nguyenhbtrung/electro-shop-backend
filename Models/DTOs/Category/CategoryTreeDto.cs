namespace electro_shop_backend.Models.DTOs.Category
{
    public class CategoryTreeDto
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public List<CategoryTreeDto> Childs { get; set; } = new List<CategoryTreeDto>();
    }

}
