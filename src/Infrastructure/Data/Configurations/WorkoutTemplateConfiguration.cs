using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class WorkoutTemplateConfiguration : IEntityTypeConfiguration<WorkoutTemplate>
    {
        public void Configure(EntityTypeBuilder<WorkoutTemplate> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__WorkoutT__8959FF2F261B5AB7");

            builder.ToTable("WorkoutTemplate");

            builder.Property(e => e.Id).HasColumnName("WorkoutTemplateID");

            builder.Property(e => e.LastModified)
                .HasColumnType("datetimeoffset") // Update column type to datetimeoffset
                .IsRequired(); // Make it required if needed

            builder.Property(e => e.Created)
                .HasColumnType("datetimeoffset") // Update column type to datetimeoffset
                .IsRequired(); // Make it required if needed

            builder.Property(e => e.TemplateName).HasMaxLength(100);

            builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WorkoutTemplateCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkoutTe__Creat__3B75D760");

            builder.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.WorkoutTemplateLastModifiedByNavigations)
                .HasForeignKey(d => d.LastModifiedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkoutTe__LastM__3C69FB99");
        }
    }
}
