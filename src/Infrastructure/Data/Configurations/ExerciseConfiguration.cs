using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(e => e.ExerciseId).HasName("PK__Exercise__A074AD0F37A63A03");

            builder.ToTable("Exercise");

            builder.Property(e => e.ExerciseId).HasColumnName("ExerciseID");
            builder.Property(e => e.DemoUrl)
                .HasMaxLength(4096) // Increase size for Base64 strings
                .HasColumnName("DemoURL");
            builder.Property(e => e.EquipmentId).HasColumnName("EquipmentID");
            builder.Property(e => e.ExerciseName).HasMaxLength(100);
            builder.Property(e => e.Type).HasMaxLength(20);

            builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Exercises)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exercise__Create__35BCFE0A");

            builder.HasOne(d => d.Equipment).WithMany(p => p.Exercises)
                .HasForeignKey(d => d.EquipmentId)
                .HasConstraintName("FK__Exercise__Equipm__37A5467C");
        }
    }
}
