using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Infrastructure.Data.Configurations;
public class WorkoutLogConfiguration : IEntityTypeConfiguration<WorkoutLog>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<WorkoutLog> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__WorkoutL__592592550AEBF56C");

        builder.ToTable("WorkoutLog");

        builder.Property(e => e.Id).HasColumnName("WorkoutLogID");
        builder.Property(e => e.Note).HasMaxLength(1000);
        builder.Property(e => e.Duration).HasColumnType("time");
        builder.Property(e => e.Created)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()")
            .HasColumnType("datetimeoffset");
        builder.Property(e => e.LastModified)
            .HasColumnType("datetimeoffset")
            .HasDefaultValueSql("SYSDATETIMEOFFSET()"); // Ensure this default value is a valid datetimeoffset expression

        builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WorkoutLogs)
            .HasForeignKey(d => d.CreatedBy)
            .HasConstraintName("FK__WorkoutLo__Creat__534D60F1");
    }
}
