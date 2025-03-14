using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace electro_shop_backend.Models.Entities
{
    [Table("Stock_Imports")]
    public class StockImport
    {
        [Key]
        [Column("stock_import_id")]
        public int StockImportId { get; set; }
        [Column("supplier_id")]
        public int SupplierId { get; set; }
        [Column("total_price")]
        public decimal TotalPrice { get; set; }
        [Column("stock_import_status")]
        public string? StockImportStatus { get; set; }
        [Column("import_date")]
        public DateTime ImportDate { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("StockImports")]
        public virtual Supplier? Supplier { get; set; }
        [InverseProperty("StockImport")]
        public virtual ICollection<StockImportDetail> StockImportDetails { get; set; } = new List<StockImportDetail>();

    }
}
