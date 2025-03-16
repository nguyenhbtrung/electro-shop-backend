using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace electro_shop_backend.Migrations
{
    /// <inheritdoc />
    public partial class deleteRatingAveandRatingCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "average_rating",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "rating_count",
                table: "Product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "average_rating",
                table: "Product",
                type: "float",
                nullable: true,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "rating_count",
                table: "Product",
                type: "int",
                nullable: true,
                defaultValue: 0);
        }
    }
}
