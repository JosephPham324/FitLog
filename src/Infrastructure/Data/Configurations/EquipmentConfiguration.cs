using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.HasKey(e => e.EquipmentId).HasName("PK__Equipmen__34474599B3C7BF43");

            builder.Property(e => e.EquipmentId).HasColumnName("EquipmentID");
            builder.Property(e => e.EquipmentName).HasMaxLength(50);
            builder.Property(e => e.ImageUrl)
                .HasMaxLength(4096) // Increase size for Base64 strings
                .HasColumnName("ImageURL");
        }
    }
}
