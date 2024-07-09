using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class CertificationConfiguration : IEntityTypeConfiguration<Certification>
    {
        public void Configure(EntityTypeBuilder<Certification> builder)
        {
            builder.HasKey(e => e.CertificationId).HasName("PK__Certific__1237E5AAC39A9000");

            builder.ToTable("Certification");

            builder.Property(e => e.CertificationId).HasColumnName("CertificationID");
            builder.Property(e => e.CertificationName).HasMaxLength(100);
            builder.Property(e => e.UserId).HasColumnName("UserID");

            builder.HasOne(d => d.User).WithMany(p => p.Certifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Certifica__UserI__29572725");
        }
    }
}
