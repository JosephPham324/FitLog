using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class AspNetUserLoginConfiguration : IEntityTypeConfiguration<AspNetUserLogin>
    {
        public void Configure(EntityTypeBuilder<AspNetUserLogin> builder)
        {
            builder.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            builder.Property(e => e.LoginProvider).HasMaxLength(128);
            builder.Property(e => e.ProviderKey).HasMaxLength(128);
        }
    }
}
