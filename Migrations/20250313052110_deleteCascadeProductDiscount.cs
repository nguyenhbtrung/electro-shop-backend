using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace electro_shop_backend.Migrations
{
    /// <inheritdoc />
    public partial class deleteCascadeProductDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Product_D__disco__72C60C4A",
                table: "Product_Discount");

            migrationBuilder.DropForeignKey(
                name: "FK__Product_D__produ__71D1E811",
                table: "Product_Discount");

            migrationBuilder.AddForeignKey(
                name: "FK__Product_D__disco__72C60C4A",
                table: "Product_Discount",
                column: "discount_id",
                principalTable: "Discount",
                principalColumn: "discount_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Product_D__produ__71D1E811",
                table: "Product_Discount",
                column: "product_id",
                principalTable: "Product",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Product_D__disco__72C60C4A",
                table: "Product_Discount");

            migrationBuilder.DropForeignKey(
                name: "FK__Product_D__produ__71D1E811",
                table: "Product_Discount");

            migrationBuilder.AddForeignKey(
                name: "FK__Product_D__disco__72C60C4A",
                table: "Product_Discount",
                column: "discount_id",
                principalTable: "Discount",
                principalColumn: "discount_id");

            migrationBuilder.AddForeignKey(
                name: "FK__Product_D__produ__71D1E811",
                table: "Product_Discount",
                column: "product_id",
                principalTable: "Product",
                principalColumn: "product_id");
        }
    }
}
