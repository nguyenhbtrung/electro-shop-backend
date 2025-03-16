namespace electro_shop_backend.Models.DTOs.Stock
{
    public class StockImportDTO
    {
        public int StockImportId { get; set; }
        public string? StockImportName { get; set; }
        public string? SupplierName { get; set; }
        public decimal TotalPrice { get; set; }
        public string? StockImportStatus { get; set; }
        public DateTime ImportDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<StockImportItemsDTO>? StockImportItems { get; set; }
    }

    public class StockImportItemsDTO
    {
        public int StockImportDetailId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ProductName { get; set; }
    }
}
