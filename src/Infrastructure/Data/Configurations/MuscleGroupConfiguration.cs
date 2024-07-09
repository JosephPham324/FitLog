using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class MuscleGroupConfiguration : IEntityTypeConfiguration<MuscleGroup>
    {
        public void Configure(EntityTypeBuilder<MuscleGroup> builder)
        {
            builder.HasKey(e => e.MuscleGroupId).HasName("PK__MuscleGr__097AE8062C42CCA3");

            builder.ToTable("MuscleGroup");

            builder.Property(e => e.MuscleGroupId).HasColumnName("MuscleGroupID");
            builder.Property(e => e.ImageUrl)
                .HasMaxLength(4096) // Increase size for Base64 strings
                .HasColumnName("ImageURL");
            builder.Property(e => e.MuscleGroupName).HasMaxLength(50);
        }
    }
}
