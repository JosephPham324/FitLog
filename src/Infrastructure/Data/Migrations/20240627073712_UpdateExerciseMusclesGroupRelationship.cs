using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitLog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExerciseMusclesGroupRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Exercise__Muscle__36B12243",
                table: "Exercise");

            migrationBuilder.DropIndex(
                name: "IDX_Exercise_MuscleGroupID",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "MuscleGroupID",
                table: "Exercise");

            migrationBuilder.CreateTable(
                name: "ExerciseMuscleGroup",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseMuscleGroup", x => new { x.ExerciseId, x.MuscleGroupId });
                    table.ForeignKey(
                        name: "FK_ExerciseMuscleGroup_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "ExerciseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseMuscleGroup_MuscleGroup_MuscleGroupId",
                        column: x => x.MuscleGroupId,
                        principalTable: "MuscleGroup",
                        principalColumn: "MuscleGroupID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseMuscleGroup_MuscleGroupId",
                table: "ExerciseMuscleGroup",
                column: "MuscleGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseMuscleGroup");

            migrationBuilder.AddColumn<int>(
                name: "MuscleGroupID",
                table: "Exercise",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IDX_Exercise_MuscleGroupID",
                table: "Exercise",
                column: "MuscleGroupID");

            migrationBuilder.AddForeignKey(
                name: "FK__Exercise__Muscle__36B12243",
                table: "Exercise",
                column: "MuscleGroupID",
                principalTable: "MuscleGroup",
                principalColumn: "MuscleGroupID");
        }
    }
}
