using electro_shop_backend.Models.DTOs.Stock;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class StockMapper
    {
        public static StockImportDTO ToStockImportDTOFromStock(this StockImport stockImport)
        {
            return new StockImportDTO()
            {
                StockImportId = stockImport.StockImportId,
                StockImportName = stockImport.StockImportName,
                SupplierId = stockImport.SupplierId,
                TotalPrice = stockImport.TotalPrice,
                StockImportStatus = stockImport.StockImportStatus,
                ImportDate = stockImport.ImportDate,
                CreatedAt = stockImport.CreatedAt,
                StockImportItems = stockImport.StockImportDetails.Select(x => x.ToStockImportItemsDTOFromStockImportItems()).ToList()
            };
        }
        public static StockImportItemsDTO ToStockImportItemsDTOFromStockImportItems(this StockImportDetail stockImportDetail)
        {
            return new StockImportItemsDTO()
            {
                StockImportDetailId = stockImportDetail.StockImportDetailId,
                ProductId = stockImportDetail.ProductId,
                Quantity = stockImportDetail.Quantity,
                UnitPrice = stockImportDetail.UnitPrice,
                ProductName = stockImportDetail.Product.Name
            };
        }

        public static StockImport ToStockImport(this AddStockImportDTO addStockImportDTO)
        {
            return new StockImport
            {
                StockImportName = addStockImportDTO.StockImportName,
                SupplierId = addStockImportDTO.SupplierId,
                TotalPrice = addStockImportDTO.TotalPrice,
                StockImportStatus = addStockImportDTO.StockImportStatus,
                ImportDate = addStockImportDTO.ImportDate,
                CreatedAt = DateTime.UtcNow,
                StockImportDetails = addStockImportDTO.StockImportItems.Select(item => item.FromAddStockImportItemsDTOToStockImportDetail()).ToList()
            };
        }

        public static StockImportDetail FromAddStockImportItemsDTOToStockImportDetail(this AddStockImportItemsDTO addStockImportItems)
        {
            return new StockImportDetail
            {
                ProductId = addStockImportItems.ProductId,
                Quantity = addStockImportItems.Quantity,
                UnitPrice = addStockImportItems.UnitPrice
            };
        }
    }
}
