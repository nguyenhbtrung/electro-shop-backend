using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace electro_shop_backend.Models.Entities
{
    [Table("Supplier")]
    public class Supplier
    {
        [Key]
        [Column("supplier_id")]
        public int SupplierId { get; set; }
        [Column("supplier_name")]
        public string? SupplierName { get; set; }
        [Column("supplier_address")]
        public string? SupplierAddress { get; set; }
        [Column("supplier_contact")]
        public string? SupplierContact { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<StockImport> StockImports { get; set; } = new List<StockImport>();
    }
}
