using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace electro_shop_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductViewHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_View_History_user_id",
                table: "Product_View_History");

            migrationBuilder.CreateIndex(
                name: "IX_Product_View_History_user_id_product_id",
                table: "Product_View_History",
                columns: new[] { "user_id", "product_id" },
                unique: true,
                filter: "[user_id] IS NOT NULL AND [product_id] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_View_History_user_id_product_id",
                table: "Product_View_History");

            migrationBuilder.CreateIndex(
                name: "IX_Product_View_History_user_id",
                table: "Product_View_History",
                column: "user_id");
        }
    }
}
