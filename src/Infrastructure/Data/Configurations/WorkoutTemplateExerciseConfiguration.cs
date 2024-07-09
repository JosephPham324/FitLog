using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class WorkoutTemplateExerciseConfiguration : IEntityTypeConfiguration<WorkoutTemplateExercise>
    {
        public void Configure(EntityTypeBuilder<WorkoutTemplateExercise> builder)
        {
            builder.HasKey(e => e.ExerciseTemlateId).HasName("PK__WorkoutT__2F7444A27D57BE2E");

            builder.ToTable("WorkoutTemplateExercise");

            builder.Property(e => e.ExerciseTemlateId).HasColumnName("ExerciseTemlateID");
            builder.Property(e => e.ExerciseId).HasColumnName("ExerciseID");
            builder.Property(e => e.NumbersOfReps).HasMaxLength(100);
            builder.Property(e => e.WeightsUsed).HasMaxLength(100);
            builder.Property(e => e.WorkoutTemplateId).HasColumnName("WorkoutTemplateID");

            builder.HasOne(d => d.Exercise).WithMany(p => p.WorkoutTemplateExercises)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("FK__WorkoutTe__Exerc__403A8C7D");

            builder.HasOne(d => d.WorkoutTemplate).WithMany(p => p.WorkoutTemplateExercises)
                .HasForeignKey(d => d.WorkoutTemplateId)
                .HasConstraintName("FK__WorkoutTe__Worko__3F466844");
        }
    }
}
