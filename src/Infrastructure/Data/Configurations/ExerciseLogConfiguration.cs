using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class ExerciseLogConfiguration : IEntityTypeConfiguration<ExerciseLog>
    {
        public void Configure(EntityTypeBuilder<ExerciseLog> builder)
        {
            builder.HasKey(e => e.ExerciseLogId).HasName("PK__Exercise__EE96A3631179C9B6");

            builder.ToTable("ExerciseLog");

            builder.Property(e => e.ExerciseLogId).HasColumnName("ExerciseLogID");
            builder.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            builder.Property(e => e.ExerciseId).HasColumnName("ExerciseID");
            builder.Property(e => e.FootageUrls)
                .HasColumnType("nvarchar(max)") // Increase size for Base64 strings
                .HasColumnName("FootageURLs");
            builder.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            builder.Property(e => e.NumberOfReps)
                .HasColumnType("nvarchar(max)") // Increase size for JSON string
                .HasColumnName("NumberOfReps");
            builder.Property(e => e.WeightsUsed)
                .HasColumnType("nvarchar(max)") // Increase size for JSON string
                .HasColumnName("WeightsUsed");
            builder.Property(e => e.WorkoutLogId).HasColumnName("WorkoutLogID");

            builder.HasOne(d => d.Exercise).WithMany(p => p.ExerciseLogs)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("FK__ExerciseL__Exerc__571DF1D5");

            builder.HasOne(d => d.WorkoutLog).WithMany(p => p.ExerciseLogs)
                .HasForeignKey(d => d.WorkoutLogId)
                .HasConstraintName("FK__ExerciseL__Worko__5629CD9C");
        }
    }
}
