using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace electro_shop_backend.Migrations
{
    /// <inheritdoc />
    public partial class CascadeProductImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Product_I__produ__44FF419A",
                table: "Product_Image");

            migrationBuilder.AddForeignKey(
                name: "FK__Product_I__produ__44FF419A",
                table: "Product_Image",
                column: "product_id",
                principalTable: "Product",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Product_I__produ__44FF419A",
                table: "Product_Image");

            migrationBuilder.AddForeignKey(
                name: "FK__Product_I__produ__44FF419A",
                table: "Product_Image",
                column: "product_id",
                principalTable: "Product",
                principalColumn: "product_id");
        }
    }
}
