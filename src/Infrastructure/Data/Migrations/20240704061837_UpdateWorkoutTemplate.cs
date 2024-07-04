using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitLog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkoutTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastModified",
                table: "WorkoutTemplate",
                type: "datetimeoffset",
                nullable: false,
                //defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeSpan.Zero),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                table: "WorkoutTemplate",
                type: "datetimeoffset",
                nullable: false);
                //defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeSpan.Zero));
            
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "WorkoutTemplate",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "WorkoutTemplate");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "WorkoutTemplate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "WorkoutTemplate",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }
    }
}
