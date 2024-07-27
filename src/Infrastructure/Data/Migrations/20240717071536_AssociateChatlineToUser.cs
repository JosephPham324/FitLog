using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitLog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AssociateChatlineToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ChatLineText",
                table: "ChatLine",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ChatLine",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ChatLine_CreatedBy",
                table: "ChatLine",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK__ChatLine__CreatedBy__6754599F",
                table: "ChatLine",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ChatLine__CreatedBy__6754599F",
                table: "ChatLine");

            migrationBuilder.DropIndex(
                name: "IX_ChatLine_CreatedBy",
                table: "ChatLine");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ChatLine");

            migrationBuilder.AlterColumn<string>(
                name: "ChatLineText",
                table: "ChatLine",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
