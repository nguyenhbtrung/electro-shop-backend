using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace electro_shop_backend.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Rating__product___68487DD7",
                table: "Rating");

            migrationBuilder.AddForeignKey(
                name: "FK__Rating__product___68487DD7",
                table: "Rating",
                column: "product_id",
                principalTable: "Product",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Rating__product___68487DD7",
                table: "Rating");

            migrationBuilder.AddForeignKey(
                name: "FK__Rating__product___68487DD7",
                table: "Rating",
                column: "product_id",
                principalTable: "Product",
                principalColumn: "product_id");
        }
    }
}
