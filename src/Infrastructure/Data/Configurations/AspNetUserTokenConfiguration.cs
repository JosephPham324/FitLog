using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class AspNetUserTokenConfiguration : IEntityTypeConfiguration<AspNetUserToken>
    {
        public void Configure(EntityTypeBuilder<AspNetUserToken> builder)
        {
            builder.Property(e => e.LoginProvider).HasMaxLength(128);
            builder.Property(e => e.Name).HasMaxLength(128);

            builder.HasOne(d => d.User)
                   .WithMany(p => p.AspNetUserTokens)
                   .HasForeignKey(d => d.UserId);
        }
    }
}
