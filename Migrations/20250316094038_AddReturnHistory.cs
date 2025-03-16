using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace electro_shop_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Return_History",
                columns: table => new
                {
                    return_history_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    return_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    changed_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Return_history", x => x.return_history_id);
                    table.ForeignKey(
                        name: "FK__Return_History__return",
                        column: x => x.return_id,
                        principalTable: "Return",
                        principalColumn: "return_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Return_History_return_id",
                table: "Return_History",
                column: "return_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Return_History");
        }
    }
}
