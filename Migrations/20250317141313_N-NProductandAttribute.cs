using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace electro_shop_backend.Migrations
{
    /// <inheritdoc />
    public partial class NNProductandAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProductAttributeDetail");

            migrationBuilder.CreateTable(
                name: "AttributeDetailProduct",
                columns: table => new
                {
                    ProductAttributeDetailsAttributeDetailId = table.Column<int>(type: "int", nullable: false),
                    ProductsProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeDetailProduct", x => new { x.ProductAttributeDetailsAttributeDetailId, x.ProductsProductId });
                    table.ForeignKey(
                        name: "FK_AttributeDetailProduct_ProductAttributeDetail_ProductAttributeDetailsAttributeDetailId",
                        column: x => x.ProductAttributeDetailsAttributeDetailId,
                        principalTable: "ProductAttributeDetail",
                        principalColumn: "AttributeDetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttributeDetailProduct_Product_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeDetailProduct_ProductsProductId",
                table: "AttributeDetailProduct",
                column: "ProductsProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttributeDetailProduct");

            migrationBuilder.CreateTable(
                name: "ProductProductAttributeDetail",
                columns: table => new
                {
                    ProductAttributeDetailsAttributeDetailId = table.Column<int>(type: "int", nullable: false),
                    ProductsProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductAttributeDetail", x => new { x.ProductAttributeDetailsAttributeDetailId, x.ProductsProductId });
                    table.ForeignKey(
                        name: "FK_ProductProductAttributeDetail_ProductAttributeDetail_ProductAttributeDetailsAttributeDetailId",
                        column: x => x.ProductAttributeDetailsAttributeDetailId,
                        principalTable: "ProductAttributeDetail",
                        principalColumn: "AttributeDetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductAttributeDetail_Product_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductAttributeDetail_ProductsProductId",
                table: "ProductProductAttributeDetail",
                column: "ProductsProductId");
        }
    }
}
