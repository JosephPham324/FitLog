using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace FitLog.Domain.Entities;

public partial class AspNetUser : IdentityUser<string>
{
    public string? GoogleID { get; set; }
    public string? FacebookID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public bool? IsRestricted { get; set; }
    public bool? IsDeleted { get; set; }
    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();

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

    //For coaching position application
    public virtual ICollection<CoachApplication> CoachApplications { get; set; } = new List<CoachApplication>();
    public virtual ICollection<CoachApplication> CoachApplicationsUpdated { get; set; } = new List<CoachApplication>();
}
