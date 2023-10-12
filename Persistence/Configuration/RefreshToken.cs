using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration;

public class RefreshToken : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ToTable("RefreshTokens");
        
        builder.HasKey(t => t.IdRefresh);

        builder.Property(t => t.IdRefresh)
            .IsRequired()
            .HasColumnName("id_RefreshToken");

        builder.Property(t => t.Token)
            .IsRequired();

        builder.Property(t => t.Expires)
            .IsRequired();

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.ReplacedToken)
            .IsRequired(false);

        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(u => u.AppUserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("Fk_RefreshToken_AppUser");
    }
}
