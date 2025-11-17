using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyPersonalLibrary.Server.Models.Configurations
{
    public sealed class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.RefreshTokenId);
            builder.Property(rt => rt.RefreshTokenId)
                .ValueGeneratedOnAdd();

            builder.Property(rt => rt.TokenHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(rt => rt.ExpiresAtUtc)
                .IsRequired();

            builder.Property(rt => rt.CreatedAtUtc)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(rt => rt.IsRevoked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(rt => rt.RevokedAtUtc)
                .IsRequired(false);

            builder.Property(rt => rt.ReplacedByTokenHash)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.HasIndex(rt => rt.TokenHash)
                .IsUnique()
                .HasDatabaseName("idx_token_hash");

            builder.HasIndex(rt => rt.UserId)
                .HasDatabaseName("idx_user_id");

            builder.HasIndex(rt => rt.ExpiresAtUtc)
                .HasDatabaseName("idx_expires_at");
        }
    }
}