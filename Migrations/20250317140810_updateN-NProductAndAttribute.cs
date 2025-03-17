using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace electro_shop_backend.Migrations
{
    /// <inheritdoc />
    public partial class updateNNProductAndAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttribute_Product",
                table: "ProductAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeDetail_ProductAttribute",
                table: "ProductAttributeDetail");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttribute_ProductId",
                table: "ProductAttribute");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductAttribute");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeDetail_ProductAttribute_ProductAttributeId",
                table: "ProductAttributeDetail",
                column: "ProductAttributeId",
                principalTable: "ProductAttribute",
                principalColumn: "AttributeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeDetail_ProductAttribute_ProductAttributeId",
                table: "ProductAttributeDetail");

            migrationBuilder.DropTable(
                name: "ProductProductAttributeDetail");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductAttribute",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_ProductId",
                table: "ProductAttribute",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttribute_Product",
                table: "ProductAttribute",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeDetail_ProductAttribute",
                table: "ProductAttributeDetail",
                column: "ProductAttributeId",
                principalTable: "ProductAttribute",
                principalColumn: "AttributeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
