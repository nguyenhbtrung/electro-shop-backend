namespace electro_shop_backend.Models.DTOs.Stock
{
    public class AddStockImportDTO
    {
        public string StockImportName { get; set; }
        public int SupplierId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime ImportDate { get; set; }
        public List<AddStockImportItemsDTO>? StockImportItems { get; set; }
    }

    public class AddStockImportItemsDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
