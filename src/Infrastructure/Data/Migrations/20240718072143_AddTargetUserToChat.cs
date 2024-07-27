using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitLog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTargetUserToChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatName",
                table: "Chat");

            migrationBuilder.AddColumn<string>(
                name: "TargetUserId",
                table: "Chat",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_TargetUserId",
                table: "Chat",
                column: "TargetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvitedChat_AspNetUser",
                table: "Chat",
                column: "TargetUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvitedChat_AspNetUser",
                table: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Chat_TargetUserId",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "TargetUserId",
                table: "Chat");

            migrationBuilder.AddColumn<string>(
                name: "ChatName",
                table: "Chat",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
