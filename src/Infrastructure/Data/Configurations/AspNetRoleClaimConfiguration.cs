using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class AspNetRoleClaimConfiguration : IEntityTypeConfiguration<AspNetRoleClaim>
    {
        public void Configure(EntityTypeBuilder<AspNetRoleClaim> builder)
        {
            builder.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            builder.HasOne(d => d.Role)
                   .WithMany(p => p.AspNetRoleClaims)
                   .HasForeignKey(d => d.RoleId);
        }
    }
}
