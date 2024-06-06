using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitLog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FitLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUserTokens",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AspNetUserId",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUserLogins",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUserClaims",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleDesc",
                table: "AspNetRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoleClaims",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AspNetRoleAspNetUser",
                columns: table => new
                {
                    RolesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleAspNetUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AspNetRoleAspNetUser_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetRoleAspNetUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Certification",
                columns: table => new
                {
                    CertificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CertificationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CertificationDateIssued = table.Column<DateOnly>(type: "date", nullable: true),
                    CertificationExpirationData = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Certific__1237E5AAC39A9000", x => x.CertificationID);
                    table.ForeignKey(
                        name: "FK__Certifica__UserI__29572725",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    ChatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Chat__A9FBE62670E630D1", x => x.ChatID);
                });

            migrationBuilder.CreateTable(
                name: "CoachingService",
                columns: table => new
                {
                    CoachingServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ServiceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "money", nullable: true),
                    ServiceAvailability = table.Column<bool>(type: "bit", nullable: true),
                    AvailabilityAnnouncement = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Coaching__7CB5DCB74574309F", x => x.CoachingServiceID);
                    table.ForeignKey(
                        name: "FK__CoachingS__Creat__5BE2A6F2",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    EquipmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Equipmen__34474599B3C7BF43", x => x.EquipmentID);
                });

            migrationBuilder.CreateTable(
                name: "MuscleGroup",
                columns: table => new
                {
                    MuscleGroupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MuscleGroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MuscleGr__097AE8062C42CCA3", x => x.MuscleGroupID);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    ProfileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePicture = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Profile__290C88848C6B876C", x => x.ProfileID);
                    table.ForeignKey(
                        name: "FK__Profile__UserID__267ABA7A",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Program",
                columns: table => new
                {
                    ProgramID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProgramName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumberOfWeeks = table.Column<int>(type: "int", nullable: true),
                    DaysPerWeek = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Goal = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ExperienceLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GymType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MusclesPriority = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AgeGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PublicProgram = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Program__752560385E73D9F3", x => x.ProgramID);
                    table.ForeignKey(
                        name: "FK__Program__UserID__44FF419A",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SurveyAnswers",
                columns: table => new
                {
                    SurveyAnswerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Goal = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DaysPerWeek = table.Column<int>(type: "int", nullable: true),
                    ExperienceLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GymType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MusclesPriority = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SurveyAn__E5C3DB53339411C2", x => x.SurveyAnswerID);
                    table.ForeignKey(
                        name: "FK__SurveyAns__UserI__6B24EA82",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutLog",
                columns: table => new
                {
                    WorkoutLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<TimeOnly>(type: "time", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkoutL__592592550AEBF56C", x => x.WorkoutLogID);
                    table.ForeignKey(
                        name: "FK__WorkoutLo__Creat__534D60F1",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutTemplate",
                columns: table => new
                {
                    WorkoutTemplateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TemplateName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkoutT__8959FF2F261B5AB7", x => x.WorkoutTemplateID);
                    table.ForeignKey(
                        name: "FK__WorkoutTe__Creat__3B75D760",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__WorkoutTe__LastM__3C69FB99",
                        column: x => x.LastModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChatLine",
                columns: table => new
                {
                    ChatLineID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatID = table.Column<int>(type: "int", nullable: true),
                    ChatLineText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AttachmentPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChatLine__3EC271E3197D524A", x => x.ChatLineID);
                    table.ForeignKey(
                        name: "FK__ChatLine__ChatID__6754599E",
                        column: x => x.ChatID,
                        principalTable: "Chat",
                        principalColumn: "ChatID");
                });

            migrationBuilder.CreateTable(
                name: "CoachingBooking",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CoachingServiceID = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Coaching__73951ACDD5A42ACC", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK__CoachingB__Coach__5FB337D6",
                        column: x => x.CoachingServiceID,
                        principalTable: "CoachingService",
                        principalColumn: "CoachingServiceID");
                    table.ForeignKey(
                        name: "FK__CoachingB__UserI__5EBF139D",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    ExerciseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MuscleGroupID = table.Column<int>(type: "int", nullable: true),
                    EquipmentID = table.Column<int>(type: "int", nullable: true),
                    ExerciseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DemoURL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicVisibility = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exercise__A074AD0F37A63A03", x => x.ExerciseID);
                    table.ForeignKey(
                        name: "FK__Exercise__Create__35BCFE0A",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Exercise__Equipm__37A5467C",
                        column: x => x.EquipmentID,
                        principalTable: "Equipment",
                        principalColumn: "EquipmentID");
                    table.ForeignKey(
                        name: "FK__Exercise__Muscle__36B12243",
                        column: x => x.MuscleGroupID,
                        principalTable: "MuscleGroup",
                        principalColumn: "MuscleGroupID");
                });

            migrationBuilder.CreateTable(
                name: "ProgramEnrollment",
                columns: table => new
                {
                    EnrollmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProgramID = table.Column<int>(type: "int", nullable: true),
                    EnrolledDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CurrentWeekNo = table.Column<int>(type: "int", nullable: true),
                    CurrentWorkoutOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProgramE__7F6877FBA65A74F3", x => x.EnrollmentID);
                    table.ForeignKey(
                        name: "FK__ProgramEn__Progr__4AB81AF0",
                        column: x => x.ProgramID,
                        principalTable: "Program",
                        principalColumn: "ProgramID");
                    table.ForeignKey(
                        name: "FK__ProgramEn__UserI__49C3F6B7",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProgramWorkout",
                columns: table => new
                {
                    ProgramWorkoutID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramID = table.Column<int>(type: "int", nullable: true),
                    WorkoutTemplateID = table.Column<int>(type: "int", nullable: true),
                    WeekNumber = table.Column<int>(type: "int", nullable: true),
                    OrderInWeek = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProgramW__6475206B460CD37C", x => x.ProgramWorkoutID);
                    table.ForeignKey(
                        name: "FK__ProgramWo__Progr__4F7CD00D",
                        column: x => x.ProgramID,
                        principalTable: "Program",
                        principalColumn: "ProgramID");
                    table.ForeignKey(
                        name: "FK__ProgramWo__Worko__5070F446",
                        column: x => x.WorkoutTemplateID,
                        principalTable: "WorkoutTemplate",
                        principalColumn: "WorkoutTemplateID");
                });

            migrationBuilder.CreateTable(
                name: "ExerciseLog",
                columns: table => new
                {
                    ExerciseLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkoutLogID = table.Column<int>(type: "int", nullable: true),
                    ExerciseID = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    OrderInSession = table.Column<int>(type: "int", nullable: true),
                    OrderInSuperset = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfSets = table.Column<int>(type: "int", nullable: true),
                    WeightsUsed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumberOfReps = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FootageURLs = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exercise__EE96A3631179C9B6", x => x.ExerciseLogID);
                    table.ForeignKey(
                        name: "FK__ExerciseL__Exerc__571DF1D5",
                        column: x => x.ExerciseID,
                        principalTable: "Exercise",
                        principalColumn: "ExerciseID");
                    table.ForeignKey(
                        name: "FK__ExerciseL__Worko__5629CD9C",
                        column: x => x.WorkoutLogID,
                        principalTable: "WorkoutLog",
                        principalColumn: "WorkoutLogID");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutTemplateExercise",
                columns: table => new
                {
                    ExerciseTemlateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkoutTemplateID = table.Column<int>(type: "int", nullable: true),
                    ExerciseID = table.Column<int>(type: "int", nullable: true),
                    OrderInSession = table.Column<int>(type: "int", nullable: true),
                    OrderInSuperset = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetsRecommendation = table.Column<int>(type: "int", nullable: true),
                    IntensityPercentage = table.Column<int>(type: "int", nullable: true),
                    RpeRecommendation = table.Column<int>(type: "int", nullable: true),
                    WeightsUsed = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumbersOfReps = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkoutT__2F7444A27D57BE2E", x => x.ExerciseTemlateID);
                    table.ForeignKey(
                        name: "FK__WorkoutTe__Exerc__403A8C7D",
                        column: x => x.ExerciseID,
                        principalTable: "Exercise",
                        principalColumn: "ExerciseID");
                    table.ForeignKey(
                        name: "FK__WorkoutTe__Worko__3F466844",
                        column: x => x.WorkoutTemplateID,
                        principalTable: "WorkoutTemplate",
                        principalColumn: "WorkoutTemplateID");
                });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex1",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "([NormalizedUserName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_AspNetUserId",
                table: "AspNetUserLogins",
                column: "AspNetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleAspNetUser_UsersId",
                table: "AspNetRoleAspNetUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Certification_UserID",
                table: "Certification",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ChatLine_ChatID",
                table: "ChatLine",
                column: "ChatID");

            migrationBuilder.CreateIndex(
                name: "IX_CoachingBooking_CoachingServiceID",
                table: "CoachingBooking",
                column: "CoachingServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_CoachingBooking_UserID",
                table: "CoachingBooking",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CoachingService_CreatedBy",
                table: "CoachingService",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IDX_Exercise_MuscleGroupID",
                table: "Exercise",
                column: "MuscleGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_CreatedBy",
                table: "Exercise",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_EquipmentID",
                table: "Exercise",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLog_ExerciseID",
                table: "ExerciseLog",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLog_WorkoutLogID",
                table: "ExerciseLog",
                column: "WorkoutLogID");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_UserID",
                table: "Profile",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Program_UserID",
                table: "Program",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollment_ProgramID",
                table: "ProgramEnrollment",
                column: "ProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramEnrollment_UserID",
                table: "ProgramEnrollment",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramWorkout_ProgramID",
                table: "ProgramWorkout",
                column: "ProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramWorkout_WorkoutTemplateID",
                table: "ProgramWorkout",
                column: "WorkoutTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswers_UserID",
                table: "SurveyAnswers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLog_CreatedBy",
                table: "WorkoutLog",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplate_CreatedBy",
                table: "WorkoutTemplate",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplate_LastModifiedBy",
                table: "WorkoutTemplate",
                column: "LastModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateExercise_ExerciseID",
                table: "WorkoutTemplateExercise",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateExercise_WorkoutTemplateID",
                table: "WorkoutTemplateExercise",
                column: "WorkoutTemplateID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_AspNetUserId",
                table: "AspNetUserLogins",
                column: "AspNetUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_AspNetUserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetRoleAspNetUser");

            migrationBuilder.DropTable(
                name: "Certification");

            migrationBuilder.DropTable(
                name: "ChatLine");

            migrationBuilder.DropTable(
                name: "CoachingBooking");

            migrationBuilder.DropTable(
                name: "ExerciseLog");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "ProgramEnrollment");

            migrationBuilder.DropTable(
                name: "ProgramWorkout");

            migrationBuilder.DropTable(
                name: "SurveyAnswers");

            migrationBuilder.DropTable(
                name: "WorkoutTemplateExercise");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropTable(
                name: "CoachingService");

            migrationBuilder.DropTable(
                name: "WorkoutLog");

            migrationBuilder.DropTable(
                name: "Program");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "WorkoutTemplate");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "MuscleGroup");

            migrationBuilder.DropIndex(
                name: "UserNameIndex1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogins_AspNetUserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUserTokens");

            migrationBuilder.DropColumn(
                name: "AspNetUserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "RoleDesc",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoleClaims");
        }
    }
}
