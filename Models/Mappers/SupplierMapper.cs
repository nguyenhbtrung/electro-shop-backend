using electro_shop_backend.Models.DTOs.Stock;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class SupplierMapper
    {
        public static SupplierDTO ToSupplierDTOFromSupplier(this Supplier supplier)
        {
            return new SupplierDTO()
            {
                SupplierId = supplier.SupplierId,
                SupplierName = supplier.SupplierName,
                SupplierAddress = supplier.SupplierAddress,
                SupplierContact = supplier.SupplierContact,
                CreatedAt = supplier.CreatedAt
            };
        }
    }
}
