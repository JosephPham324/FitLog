using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FitLog.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;


    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();

    public virtual ICollection<CoachingBooking> CoachingBookings { get; set; } = new List<CoachingBooking>();

    public virtual ICollection<CoachingService> CoachingServices { get; set; } = new List<CoachingService>();

    public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();

    public virtual ICollection<ProgramEnrollment> ProgramEnrollments { get; set; } = new List<ProgramEnrollment>();

    public virtual ICollection<Program> Programs { get; set; } = new List<Program>();

    public virtual ICollection<SurveyAnswer> SurveyAnswers { get; set; } = new List<SurveyAnswer>();

    public virtual ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();

    public virtual ICollection<WorkoutTemplate> WorkoutTemplateCreatedByNavigations { get; set; } = new List<WorkoutTemplate>();

    public virtual ICollection<WorkoutTemplate> WorkoutTemplateLastModifiedByNavigations { get; set; } = new List<WorkoutTemplate>();

    public SystemRole? Role { get; set; }
}
