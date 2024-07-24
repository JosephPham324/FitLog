using FitLog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitLog.Infrastructure.Data.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(e => e.ChatId).HasName("PK__Chat__A9FBE62670E630D1");

            builder.ToTable("Chat");

            builder.Property(e => e.ChatId)
                .HasColumnName("ChatID");

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            // Link user to CreatedBy
            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(450)
                .HasColumnName("CreatedBy");

            builder.HasOne(e => e.UserNavigation)
                .WithMany(u => u.CreatedChats)
                .HasForeignKey(e => e.CreatedBy)
                .HasConstraintName("FK_Chat_AspNetUser")
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(e => e.TargetUserNavigation)
                .WithMany(u => u.InvitedChats)
                .HasForeignKey(e => e.TargetUserId)
                .HasConstraintName("FK_InvitedChat_AspNetUser")
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(e => e.ChatLines)
                .WithOne(cl => cl.Chat)
                .HasForeignKey(cl => cl.ChatId)
                .HasConstraintName("FK_ChatLine_Chat");

        }

    }
}
