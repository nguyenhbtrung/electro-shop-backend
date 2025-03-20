using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace electro_shop_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddSupportMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Support_Message",
                columns: table => new
                {
                    message_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sender_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    receiver_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    is_from_admin = table.Column<bool>(type: "bit", nullable: true),
                    sent_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Support_Message", x => x.message_id);
                    table.ForeignKey(
                        name: "FK__SupportMessage__receiverId",
                        column: x => x.receiver_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__SupportMessage__senderId",
                        column: x => x.sender_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Support_Message_receiver_id",
                table: "Support_Message",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "IX_Support_Message_sender_id",
                table: "Support_Message",
                column: "sender_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Support_Message");
        }
    }
}
