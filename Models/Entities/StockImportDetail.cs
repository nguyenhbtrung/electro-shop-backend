using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace electro_shop_backend.Models.Entities
{
    [Table("Stock_Import_Details")]
    public class StockImportDetail
    {
        [Key]
        [Column("stock_import_detail_id")]
        public int StockImportDetailId { get; set; }
        [Column("stock_import_id")]
        public int StockImportId { get; set; }
        [Column("product_id")]
        public int ProductId { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("unit_price")]
        public decimal UnitPrice { get; set; }
        [ForeignKey("StockImportId")]
        [InverseProperty("StockImportDetails")]
        public virtual StockImport? StockImport { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
