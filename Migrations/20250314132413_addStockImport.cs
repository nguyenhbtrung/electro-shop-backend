using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace electro_shop_backend.Migrations
{
    /// <inheritdoc />
    public partial class addStockImport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    supplier_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supplier_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supplier_contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__1A56936E", x => x.supplier_id);
                });

            migrationBuilder.CreateTable(
                name: "Stock_Imports",
                columns: table => new
                {
                    stock_import_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    total_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    stock_import_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    import_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StockImp__D3A3E3A3D3A3E3A3", x => x.stock_import_id);
                    table.ForeignKey(
                        name: "FK__StockImpo__suppl__5EBF139D",
                        column: x => x.supplier_id,
                        principalTable: "Supplier",
                        principalColumn: "supplier_id");
                });

            migrationBuilder.CreateTable(
                name: "Stock_Import_Details",
                columns: table => new
                {
                    stock_import_detail_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stock_import_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StockImpDetail__1A56936E", x => x.stock_import_detail_id);
                    table.ForeignKey(
                        name: "FK__StockImportDetail__ProductId",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id");
                    table.ForeignKey(
                        name: "FK__StockImportDetail__StockImportId",
                        column: x => x.stock_import_id,
                        principalTable: "Stock_Imports",
                        principalColumn: "stock_import_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stock_Import_Details_product_id",
                table: "Stock_Import_Details",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_Import_Details_stock_import_id",
                table: "Stock_Import_Details",
                column: "stock_import_id");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_Imports_supplier_id",
                table: "Stock_Imports",
                column: "supplier_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stock_Import_Details");

            migrationBuilder.DropTable(
                name: "Stock_Imports");

            migrationBuilder.DropTable(
                name: "Supplier");
        }
    }
}
