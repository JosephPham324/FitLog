using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Infrastructure.Data.Configurations;

public class AspNetRoleConfiguration : IEntityTypeConfiguration<AspNetRole>
{
    public void Configure(EntityTypeBuilder<AspNetRole> builder)
    {
        builder.Property(e => e.Name).HasMaxLength(256);

        builder.Property(e => e.RoleDesc)
               .HasMaxLength(256)
               .IsRequired(false);

        builder.HasKey(p => p.Id);
    }
}
