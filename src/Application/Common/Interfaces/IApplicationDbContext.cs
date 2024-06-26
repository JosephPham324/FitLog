using FitLog.Domain.Entities;

namespace FitLog.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<AspNetRole> AspNetRoles { get; set; }

    DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    DbSet<AspNetUser> AspNetUsers { get; set; }

    DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    DbSet<Certification> Certifications { get; set; }

    DbSet<Chat> Chats { get; set; }

     DbSet<ChatLine> ChatLines { get; set; }

    DbSet<CoachingBooking> CoachingBookings { get; set; }

    DbSet<CoachingService> CoachingServices { get; set; }

    DbSet<Equipment> Equipment { get; set; }

    DbSet<Exercise> Exercises { get; set; }

    DbSet<ExerciseLog> ExerciseLogs { get; set; }

    DbSet<MuscleGroup> MuscleGroups { get; set; }

    DbSet<FitLog.Domain.Entities.Profile> Profiles { get; set; }

    DbSet<Program> Programs { get; set; }

    DbSet<ProgramEnrollment> ProgramEnrollments { get; set; }

    DbSet<ProgramWorkout> ProgramWorkouts { get; set; }

    DbSet<SurveyAnswer> SurveyAnswers { get; set; }

    //DbSet<SystemRole> SystemRoles { get; set; }


    //DbSet<User> Users { get; set; }

    DbSet<WorkoutLog> WorkoutLogs { get; set; }

    DbSet<WorkoutTemplate> WorkoutTemplates { get; set; }

    DbSet<WorkoutTemplateExercise> WorkoutTemplateExercises { get; set; }
    DbSet<CoachApplication> CoachApplications { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
