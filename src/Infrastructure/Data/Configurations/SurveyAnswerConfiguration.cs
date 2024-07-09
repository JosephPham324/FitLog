using System.Reflection.Emit;
using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure;

public class SurveyAnswerConfiguration : IEntityTypeConfiguration<SurveyAnswer>
{
    public void Configure(EntityTypeBuilder<SurveyAnswer> builder)
    {
        builder.HasKey(e => e.SurveyAnswerId).HasName("PK__SurveyAn__E5C3DB53339411C2");

        builder.Property(e => e.SurveyAnswerId).HasColumnName("SurveyAnswerID");
        builder.Property(e => e.ExperienceLevel).HasMaxLength(50);
        builder.Property(e => e.Goal).HasMaxLength(255);
        builder.Property(e => e.GymType).HasMaxLength(50);
        builder.Property(e => e.LastModified)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");
        builder.Property(e => e.MusclesPriority).HasMaxLength(255);
        builder.Property(e => e.UserId).HasColumnName("UserID");

        builder.HasOne(d => d.User).WithMany(p => p.SurveyAnswers)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("FK__SurveyAns__UserI__6B24EA82");
    }
}
