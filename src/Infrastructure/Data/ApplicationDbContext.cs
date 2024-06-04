using System.Reflection;
using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<AspNetUser,AspNetRole,string>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Certification> Certifications { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ChatLine> ChatLines { get; set; }

    public virtual DbSet<CoachingBooking> CoachingBookings { get; set; }

    public virtual DbSet<CoachingService> CoachingServices { get; set; }

    public virtual DbSet<Equipment> Equipment { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<ExerciseLog> ExerciseLogs { get; set; }

    public virtual DbSet<MuscleGroup> MuscleGroups { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Program> Programs { get; set; }

    public virtual DbSet<ProgramEnrollment> ProgramEnrollments { get; set; }

    public virtual DbSet<ProgramWorkout> ProgramWorkouts { get; set; }

    public virtual DbSet<SurveyAnswer> SurveyAnswers { get; set; }

    //public virtual DbSet<SystemRole> SystemRoles { get; set; }


    //public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WorkoutLog> WorkoutLogs { get; set; }

    public virtual DbSet<WorkoutTemplate> WorkoutTemplates { get; set; }

    public virtual DbSet<WorkoutTemplateExercise> WorkoutTemplateExercises { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });


        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            //entity.HasMany(d => d.Roles).WithMany(p => p.Users)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "AspNetUserRole",
            //        r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
            //        l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
            //        j =>
            //        {
            //            j.HasKey("UserId", "RoleId");
            //            j.ToTable("AspNetUserRoles");
            //            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
            //        });
        });

        modelBuilder.Entity<AspNetRole>()
        .HasDiscriminator<string>("UserType");
        
        modelBuilder.Entity<AspNetRole>()
        .HasKey(p => p.Id);

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });


        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            //entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            //entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Certification>(entity =>
        {
            entity.HasKey(e => e.CertificationId).HasName("PK__Certific__1237E5AAC39A9000");

            entity.ToTable("Certification");

            entity.Property(e => e.CertificationId).HasColumnName("CertificationID");
            entity.Property(e => e.CertificationName).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Certifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Certifica__UserI__29572725");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PK__Chat__A9FBE62670E630D1");

            entity.ToTable("Chat");

            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ChatLine>(entity =>
        {
            entity.HasKey(e => e.ChatLineId).HasName("PK__ChatLine__3EC271E3197D524A");

            entity.ToTable("ChatLine");

            entity.Property(e => e.ChatLineId).HasColumnName("ChatLineID");
            entity.Property(e => e.AttachmentPath).HasMaxLength(255);
            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LinkUrl).HasMaxLength(255);

            entity.HasOne(d => d.Chat).WithMany(p => p.ChatLines)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("FK__ChatLine__ChatID__6754599E");
        });

        modelBuilder.Entity<CoachingBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Coaching__73951ACDD5A42ACC");

            entity.ToTable("CoachingBooking");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.CoachingServiceId).HasColumnName("CoachingServiceID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CoachingService).WithMany(p => p.CoachingBookings)
                .HasForeignKey(d => d.CoachingServiceId)
                .HasConstraintName("FK__CoachingB__Coach__5FB337D6");

            entity.HasOne(d => d.User).WithMany(p => p.CoachingBookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CoachingB__UserI__5EBF139D");
        });

        modelBuilder.Entity<CoachingService>(entity =>
        {
            entity.HasKey(e => e.CoachingServiceId).HasName("PK__Coaching__7CB5DCB74574309F");

            entity.ToTable("CoachingService");

            entity.Property(e => e.CoachingServiceId).HasColumnName("CoachingServiceID");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ServiceName).HasMaxLength(100);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CoachingServices)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__CoachingS__Creat__5BE2A6F2");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("PK__Equipmen__34474599B3C7BF43");

            entity.Property(e => e.EquipmentId).HasColumnName("EquipmentID");
            entity.Property(e => e.EquipmentName).HasMaxLength(50);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(256)
                .HasColumnName("ImageURL");
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("PK__Exercise__A074AD0F37A63A03");

            entity.ToTable("Exercise");

            entity.HasIndex(e => e.MuscleGroupId, "IDX_Exercise_MuscleGroupID");

            entity.Property(e => e.ExerciseId).HasColumnName("ExerciseID");
            entity.Property(e => e.DemoUrl)
                .HasMaxLength(256)
                .HasColumnName("DemoURL");
            entity.Property(e => e.EquipmentId).HasColumnName("EquipmentID");
            entity.Property(e => e.ExerciseName).HasMaxLength(100);
            entity.Property(e => e.MuscleGroupId).HasColumnName("MuscleGroupID");
            entity.Property(e => e.Type).HasMaxLength(20);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Exercises)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exercise__Create__35BCFE0A");

            entity.HasOne(d => d.Equipment).WithMany(p => p.Exercises)
                .HasForeignKey(d => d.EquipmentId)
                .HasConstraintName("FK__Exercise__Equipm__37A5467C");

            entity.HasOne(d => d.MuscleGroup).WithMany(p => p.Exercises)
                .HasForeignKey(d => d.MuscleGroupId)
                .HasConstraintName("FK__Exercise__Muscle__36B12243");
        });

        modelBuilder.Entity<ExerciseLog>(entity =>
        {
            entity.HasKey(e => e.ExerciseLogId).HasName("PK__Exercise__EE96A3631179C9B6");

            entity.ToTable("ExerciseLog");

            entity.Property(e => e.ExerciseLogId).HasColumnName("ExerciseLogID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExerciseId).HasColumnName("ExerciseID");
            entity.Property(e => e.FootageUrls).HasColumnName("FootageURLs");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NumberOfReps).HasMaxLength(50);
            entity.Property(e => e.WeightsUsed).HasMaxLength(50);
            entity.Property(e => e.WorkoutLogId).HasColumnName("WorkoutLogID");

            entity.HasOne(d => d.Exercise).WithMany(p => p.ExerciseLogs)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("FK__ExerciseL__Exerc__571DF1D5");

            entity.HasOne(d => d.WorkoutLog).WithMany(p => p.ExerciseLogs)
                .HasForeignKey(d => d.WorkoutLogId)
                .HasConstraintName("FK__ExerciseL__Worko__5629CD9C");
        });

        modelBuilder.Entity<MuscleGroup>(entity =>
        {
            entity.HasKey(e => e.MuscleGroupId).HasName("PK__MuscleGr__097AE8062C42CCA3");

            entity.ToTable("MuscleGroup");

            entity.Property(e => e.MuscleGroupId).HasColumnName("MuscleGroupID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(256)
                .HasColumnName("ImageURL");
            entity.Property(e => e.MuscleGroupName).HasMaxLength(50);
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__Profile__290C88848C6B876C");

            entity.ToTable("Profile");

            entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
            entity.Property(e => e.ProfilePicture).HasMaxLength(256);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Profile__UserID__267ABA7A");
        });

        modelBuilder.Entity<Program>(entity =>
        {
            entity.HasKey(e => e.ProgramId).HasName("PK__Program__752560385E73D9F3");

            entity.ToTable("Program");

            entity.Property(e => e.ProgramId).HasColumnName("ProgramID");
            entity.Property(e => e.AgeGroup).HasMaxLength(50);
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExperienceLevel).HasMaxLength(50);
            entity.Property(e => e.Goal).HasMaxLength(255);
            entity.Property(e => e.GymType).HasMaxLength(50);
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MusclesPriority).HasMaxLength(255);
            entity.Property(e => e.ProgramName).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Programs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Program__UserID__44FF419A");
        });

        modelBuilder.Entity<ProgramEnrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__ProgramE__7F6877FBA65A74F3");

            entity.ToTable("ProgramEnrollment");

            entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.EnrolledDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProgramId).HasColumnName("ProgramID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Program).WithMany(p => p.ProgramEnrollments)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__ProgramEn__Progr__4AB81AF0");

            entity.HasOne(d => d.User).WithMany(p => p.ProgramEnrollments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ProgramEn__UserI__49C3F6B7");
        });

        modelBuilder.Entity<ProgramWorkout>(entity =>
        {
            entity.HasKey(e => e.ProgramWorkoutId).HasName("PK__ProgramW__6475206B460CD37C");

            entity.ToTable("ProgramWorkout");

            entity.Property(e => e.ProgramWorkoutId).HasColumnName("ProgramWorkoutID");
            entity.Property(e => e.ProgramId).HasColumnName("ProgramID");
            entity.Property(e => e.WorkoutTemplateId).HasColumnName("WorkoutTemplateID");

            entity.HasOne(d => d.Program).WithMany(p => p.ProgramWorkouts)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__ProgramWo__Progr__4F7CD00D");

            entity.HasOne(d => d.WorkoutTemplate).WithMany(p => p.ProgramWorkouts)
                .HasForeignKey(d => d.WorkoutTemplateId)
                .HasConstraintName("FK__ProgramWo__Worko__5070F446");
        });

        modelBuilder.Entity<SurveyAnswer>(entity =>
        {
            entity.HasKey(e => e.SurveyAnswerId).HasName("PK__SurveyAn__E5C3DB53339411C2");

            entity.Property(e => e.SurveyAnswerId).HasColumnName("SurveyAnswerID");
            entity.Property(e => e.ExperienceLevel).HasMaxLength(50);
            entity.Property(e => e.Goal).HasMaxLength(255);
            entity.Property(e => e.GymType).HasMaxLength(50);
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MusclesPriority).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.SurveyAnswers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SurveyAns__UserI__6B24EA82");
        });

        //modelBuilder.Entity<SystemRole>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__SystemRo__8AFACE3A9FD9B530");

        //    entity.ToTable("SystemRole");

        //    entity.Property(e => e.RoleId).HasColumnName("RoleID");
        //    entity.Property(e => e.RoleDesc).HasMaxLength(256);
        //    entity.Property(e => e.RoleName).HasMaxLength(50);
        //});

        //modelBuilder.Entity<TodoItem>(entity =>
        //{
        //    entity.HasIndex(e => e.ListId, "IX_TodoItems_ListId");

        //    entity.Property(e => e.Title).HasMaxLength(200);

        //    entity.HasOne(d => d.List).WithMany(p => p.Items).HasForeignKey(d => d.ListId);
        //});

        //modelBuilder.Entity<TodoList>(entity =>
        //{
        //    entity.Property(e => e.Colour).HasColumnName("Colour_Code");
        //    entity.Property(e => e.Title).HasMaxLength(200);
        //});

        //modelBuilder.Entity<User>(entity =>
        //{
        //    entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC958C4999");

        //    entity.ToTable("User");

        //    entity.HasIndex(e => e.Email, "IDX_User_Email");

        //    entity.Property(e => e.UserId).HasColumnName("UserID");
        //    entity.Property(e => e.Email).HasMaxLength(100);
        //    entity.Property(e => e.FirstName).HasMaxLength(50);
        //    entity.Property(e => e.Gender).HasMaxLength(10);
        //    entity.Property(e => e.LastName).HasMaxLength(50);
        //    entity.Property(e => e.PasswordHash).HasMaxLength(256);
        //    entity.Property(e => e.Username).HasMaxLength(50);

        //    entity.HasMany(d => d.Roles).WithMany(p => (ICollection<User>)p.Users)
        //        .UsingEntity<Dictionary<string, object>>(
        //            "UserRole",
        //             r => r.HasOne<AspNetRole>().WithMany()
        //                .HasForeignKey("RoleId")
        //                .OnDelete(DeleteBehavior.ClientSetNull)
        //                .HasConstraintName("FK__UserRole__RoleID__2F10007B"),
        //            l => l.HasOne<User>().WithMany()
        //                .HasForeignKey("UserId")
        //                .OnDelete(DeleteBehavior.ClientSetNull)
        //                .HasConstraintName("FK__UserRole__UserID__2E1BDC42"),
        //            j =>
        //            {
        //                j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF27604F148A4E68");
        //                j.ToTable("UserRole");
        //                j.IndexerProperty<int>("UserId").HasColumnName("UserID");
        //                j.IndexerProperty<int>("RoleId").HasColumnName("RoleID");
        //            });
        //});

        modelBuilder.Entity<WorkoutLog>(entity =>
        {
            entity.HasKey(e => e.WorkoutLogId).HasName("PK__WorkoutL__592592550AEBF56C");

            entity.ToTable("WorkoutLog");

            entity.Property(e => e.WorkoutLogId).HasColumnName("WorkoutLogID");
            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WorkoutLogs)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__WorkoutLo__Creat__534D60F1");
        });

        modelBuilder.Entity<WorkoutTemplate>(entity =>
        {
            entity.HasKey(e => e.WorkoutTemplateId).HasName("PK__WorkoutT__8959FF2F261B5AB7");

            entity.ToTable("WorkoutTemplate");

            entity.Property(e => e.WorkoutTemplateId).HasColumnName("WorkoutTemplateID");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.TemplateName).HasMaxLength(100);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WorkoutTemplateCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkoutTe__Creat__3B75D760");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.WorkoutTemplateLastModifiedByNavigations)
                .HasForeignKey(d => d.LastModifiedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkoutTe__LastM__3C69FB99");
        });

        modelBuilder.Entity<WorkoutTemplateExercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseTemlateId).HasName("PK__WorkoutT__2F7444A27D57BE2E");

            entity.ToTable("WorkoutTemplateExercise");

            entity.Property(e => e.ExerciseTemlateId).HasColumnName("ExerciseTemlateID");
            entity.Property(e => e.ExerciseId).HasColumnName("ExerciseID");
            entity.Property(e => e.NumbersOfReps).HasMaxLength(100);
            entity.Property(e => e.WeightsUsed).HasMaxLength(100);
            entity.Property(e => e.WorkoutTemplateId).HasColumnName("WorkoutTemplateID");

            entity.HasOne(d => d.Exercise).WithMany(p => p.WorkoutTemplateExercises)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("FK__WorkoutTe__Exerc__403A8C7D");

            entity.HasOne(d => d.WorkoutTemplate).WithMany(p => p.WorkoutTemplateExercises)
                .HasForeignKey(d => d.WorkoutTemplateId)
                .HasConstraintName("FK__WorkoutTe__Worko__3F466844");
        });
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
