using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class CoachingServiceConfiguration : IEntityTypeConfiguration<CoachingService>
    {
        public void Configure(EntityTypeBuilder<CoachingService> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Coaching__7CB5DCB74574309F");

            builder.ToTable("CoachingService");

            builder.Property(e => e.Id).HasColumnName("CoachingServiceID");
            builder.Property(e => e.Price).HasColumnType("money");
            builder.Property(e => e.ServiceName).HasMaxLength(100);

            builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CoachingServices)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__CoachingS__Creat__5BE2A6F2");
        }
    }
}
