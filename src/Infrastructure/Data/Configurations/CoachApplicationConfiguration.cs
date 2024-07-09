using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class CoachApplicationConfiguration : IEntityTypeConfiguration<CoachApplication>
    {
        public void Configure(EntityTypeBuilder<CoachApplication> builder)
        {
            // Configure primary key
            builder.HasKey(e => e.Id);

            // Configure relationships
            builder.HasOne(e => e.Applicant)
                .WithMany(u => u.CoachApplications)
                .HasForeignKey(e => e.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.StatusUpdatedBy)
                .WithMany(u => u.CoachApplicationsUpdated)
                .HasForeignKey(e => e.LastModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure other properties if needed
            builder.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValue("Pending");

            builder.Property(e => e.StatusReason)
                .HasMaxLength(500);
        }
    }
}
