using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class AspNetUserClaimConfiguration : IEntityTypeConfiguration<AspNetUserClaim>
    {
        public void Configure(EntityTypeBuilder<AspNetUserClaim> builder)
        {
            builder.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            builder.HasOne(d => d.User)
                   .WithMany(p => p.AspNetUserClaims)
                   .HasForeignKey(d => d.UserId);
        }
    }
}
