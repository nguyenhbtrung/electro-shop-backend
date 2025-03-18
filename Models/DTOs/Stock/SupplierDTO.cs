namespace electro_shop_backend.Models.DTOs.Stock
{
    public class SupplierDTO
    {
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierAddress { get; set; }
        public string? SupplierContact { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
