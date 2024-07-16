using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class ExerciseMuscleGroupConfiguration : IEntityTypeConfiguration<ExerciseMuscleGroup>
    {
        public void Configure(EntityTypeBuilder<ExerciseMuscleGroup> builder)
        {
            builder.HasKey(e => new { e.ExerciseId, e.MuscleGroupId });

            builder.HasOne(d => d.Exercise)
                .WithMany(p => p.ExerciseMuscleGroups)
                .HasForeignKey(d => d.ExerciseId);

            builder.HasOne(d => d.MuscleGroup)
                .WithMany(p => p.ExerciseMuscleGroups)
                .HasForeignKey(d => d.MuscleGroupId);
        }
    }
}
