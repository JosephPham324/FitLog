using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class ProgramWorkoutConfiguration : IEntityTypeConfiguration<ProgramWorkout>
    {
        public void Configure(EntityTypeBuilder<ProgramWorkout> builder)
        {
            builder.HasKey(e => e.ProgramWorkoutId).HasName("PK__ProgramW__6475206B460CD37C");

            builder.ToTable("ProgramWorkout");

            builder.Property(e => e.ProgramWorkoutId).HasColumnName("ProgramWorkoutID");
            builder.Property(e => e.ProgramId).HasColumnName("ProgramID");
            builder.Property(e => e.WorkoutTemplateId).HasColumnName("WorkoutTemplateID");

            builder.HasOne(d => d.Program).WithMany(p => p.ProgramWorkouts)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__ProgramWo__Progr__4F7CD00D");

            builder.HasOne(d => d.WorkoutTemplate).WithMany(p => p.ProgramWorkouts)
                .HasForeignKey(d => d.WorkoutTemplateId)
                .HasConstraintName("FK__ProgramWo__Worko__5070F446");
        }
    }
}
