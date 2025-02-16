using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace electro_shop_backend.Migrations
{
    /// <inheritdoc />
    public partial class deleteUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
