namespace electro_shop_backend.Models.DTOs.Stock
{
    public class ListStockDTO
    {
        public int StockImportId { get; set; }
        public string? StockImportName { get; set; }
        public string? SupplierName { get; set; }
        public decimal TotalPrice { get; set; }
        public string? StockImportStatus { get; set; }
        public DateTime ImportDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
