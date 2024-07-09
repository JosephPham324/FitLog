using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class CoachingBookingConfiguration : IEntityTypeConfiguration<CoachingBooking>
    {
        public void Configure(EntityTypeBuilder<CoachingBooking> builder)
        {
            builder.HasKey(e => e.BookingId).HasName("PK__Coaching__73951ACDD5A42ACC");

            builder.ToTable("CoachingBooking");

            builder.Property(e => e.BookingId).HasColumnName("BookingID");
            builder.Property(e => e.CoachingServiceId).HasColumnName("CoachingServiceID");
            builder.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            builder.Property(e => e.PaymentDate).HasColumnType("datetime");
            builder.Property(e => e.Status).HasMaxLength(20);
            builder.Property(e => e.UserId).HasColumnName("UserID");

            builder.HasOne(d => d.CoachingService).WithMany(p => p.CoachingBookings)
                .HasForeignKey(d => d.CoachingServiceId)
                .HasConstraintName("FK__CoachingB__Coach__5FB337D6");

            builder.HasOne(d => d.User).WithMany(p => p.CoachingBookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CoachingB__UserI__5EBF139D");
        }
    }
}
