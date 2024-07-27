using FitLog.Domain.Entities;
using FitLog.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static FitLog.Domain.Entities.ProgramEnrollment;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class ProgramEnrollmentConfiguration : IEntityTypeConfiguration<ProgramEnrollment>
    {
        public void Configure(EntityTypeBuilder<ProgramEnrollment> builder)
        {
            builder.HasKey(e => e.EnrollmentId).HasName("PK__ProgramE__7F6877FBA65A74F3");

            builder.ToTable("ProgramEnrollment");

            builder.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");
            builder.Property(e => e.EndDate).HasColumnType("datetime");
            builder.Property(e => e.EnrolledDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            builder.Property(e => e.ProgramId).HasColumnName("ProgramID");
            builder.Property(e => e.Status).HasMaxLength(20);
            builder.Property(e => e.UserId).HasColumnName("UserID");

            builder.HasOne(d => d.Program).WithMany(p => p.ProgramEnrollments)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__ProgramEn__Progr__4AB81AF0");

            builder.HasOne(d => d.User).WithMany(p => p.ProgramEnrollments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ProgramEn__UserI__49C3F6B7");

            // Apply the value converter and value comparer for WorkoutsProgress property
            var workoutsProgressConverter = new WorkoutsProgressConverter();
            var workoutsProgressComparer = new ValueComparer<Dictionary<int, WorkoutProgress>>(
               (c1, c2) => (c1 ?? new Dictionary<int, WorkoutProgress>()).SequenceEqual(c2 ?? new Dictionary<int, WorkoutProgress>()),
               c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
               c => c == null ? new Dictionary<int, WorkoutProgress>() : c.ToDictionary(entry => entry.Key, entry => entry.Value));

            builder.Property(e => e.WorkoutsProgress)
                .HasConversion(workoutsProgressConverter)
                .Metadata.SetValueComparer(workoutsProgressComparer);
        }
    }
}
