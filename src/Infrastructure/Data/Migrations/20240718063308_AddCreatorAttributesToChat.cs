using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitLog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorAttributesToChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatName",
                table: "Chat",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Chat",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_CreatedBy",
                table: "Chat",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Chat_AspNetUser",
                table: "Chat",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chat_AspNetUser",
                table: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Chat_CreatedBy",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "ChatName",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Chat");
        }
    }
}
