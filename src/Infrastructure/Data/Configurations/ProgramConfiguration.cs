using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class ProgramConfiguration : IEntityTypeConfiguration<Program>
    {
        public void Configure(EntityTypeBuilder<Program> builder)
        {
            builder.HasKey(e => e.ProgramId).HasName("PK__Program__752560385E73D9F3");

            builder.ToTable("Program");

            builder.Property(e => e.ProgramId).HasColumnName("ProgramID");
            builder.Property(e => e.AgeGroup).HasMaxLength(50);
            builder.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            builder.Property(e => e.ExperienceLevel).HasMaxLength(50);
            builder.Property(e => e.Goal).HasMaxLength(255);
            builder.Property(e => e.GymType).HasMaxLength(50);
            builder.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            builder.Property(e => e.MusclesPriority).HasMaxLength(255);
            builder.Property(e => e.ProgramName).HasMaxLength(100);
            builder.Property(e => e.UserId).HasColumnName("UserID");

            builder.HasOne(d => d.User).WithMany(p => p.Programs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Program__UserID__44FF419A");
        }
    }
}
