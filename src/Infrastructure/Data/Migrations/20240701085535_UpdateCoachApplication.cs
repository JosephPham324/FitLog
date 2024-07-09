using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitLog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCoachApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoachApplications_AspNetUsers_StatusUpdatedById",
                table: "CoachApplications");

            migrationBuilder.DropColumn(
                name: "StatusUpdateTime",
                table: "CoachApplications");

            migrationBuilder.RenameColumn(
                name: "StatusUpdatedById",
                table: "CoachApplications",
                newName: "LastModifiedBy");

            migrationBuilder.RenameIndex(
                name: "IX_CoachApplications_StatusUpdatedById",
                table: "CoachApplications",
                newName: "IX_CoachApplications_LastModifiedBy");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CoachApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                table: "CoachApplications",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoachApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModified",
                table: "CoachApplications",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "StatusReason",
                table: "CoachApplications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CoachApplications_AspNetUsers_LastModifiedBy",
                table: "CoachApplications",
                column: "LastModifiedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoachApplications_AspNetUsers_LastModifiedBy",
                table: "CoachApplications");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "CoachApplications");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoachApplications");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "CoachApplications");

            migrationBuilder.DropColumn(
                name: "StatusReason",
                table: "CoachApplications");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "CoachApplications",
                newName: "StatusUpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_CoachApplications_LastModifiedBy",
                table: "CoachApplications",
                newName: "IX_CoachApplications_StatusUpdatedById");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CoachApplications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Pending");

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusUpdateTime",
                table: "CoachApplications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_CoachApplications_AspNetUsers_StatusUpdatedById",
                table: "CoachApplications",
                column: "StatusUpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
