using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class ChatLineConfiguration : IEntityTypeConfiguration<ChatLine>
    {
        public void Configure(EntityTypeBuilder<ChatLine> builder)
        {
            builder.HasKey(e => e.ChatLineId).HasName("PK__ChatLine__3EC271E3197D524A");

            builder.ToTable("ChatLine");

            builder.Property(e => e.ChatLineId).HasColumnName("ChatLineID");
            builder.Property(e => e.AttachmentPath).HasMaxLength(255);
            builder.Property(e => e.ChatId).HasColumnName("ChatID");
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            builder.Property(e => e.LinkUrl).HasMaxLength(255);

            builder.HasOne(d => d.Chat).WithMany(p => p.ChatLines)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("FK__ChatLine__ChatID__6754599E");
        }
    }
}
