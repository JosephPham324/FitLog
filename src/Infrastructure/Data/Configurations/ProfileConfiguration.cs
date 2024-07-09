using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(e => e.ProfileId).HasName("PK__Profile__290C88848C6B876C");

            builder.ToTable("Profile");

            builder.Property(e => e.ProfileId).HasColumnName("ProfileID");
            builder.Property(e => e.ProfilePicture)
                .HasColumnType("nvarchar(max)") // Increase size for Base64 strings
                .HasColumnName("ProfilePicture");
            builder.Property(e => e.UserId).HasColumnName("UserID");
            builder.Property(e => e.GalleryImageLinksJson)
                .HasColumnType("nvarchar(max)") // Increase size for JSON string
                .HasColumnName("GalleryImageLinks");

            builder.HasOne(d => d.User).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Profile__UserID__267ABA7A");
        }
    }
}
