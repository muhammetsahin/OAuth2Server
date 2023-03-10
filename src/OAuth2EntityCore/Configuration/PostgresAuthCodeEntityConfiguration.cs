using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OAuth2.Models;

namespace OAuth2EntityCore.Configuration
{
    public class PostgresAuthCodeEntityConfiguration<TUser> : IEntityTypeConfiguration<AuthCode<TUser>>
        where TUser : User<TUser>
    {
        public void Configure(EntityTypeBuilder<AuthCode<TUser>> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.CreatedAt)
                .ValueGeneratedOnAdd();


            builder.Property(u => u.UpdateAt)
                .IsRequired(false)
                .HasDefaultValue()
                .ValueGeneratedOnAddOrUpdate();
            builder.Property(a => a.Code)
                .IsRequired()
                .IsUnicode();

            builder.Property(a => a.Scopes)
                .HasColumnType("text[]")
                .IsRequired();

            builder.Property(a => a.ExpiresAt)
                .IsRequired();

            builder.HasIndex(u => new {u.UserId, u.ClientId})
                .IsUnique();
            // User who triggered login
            builder.HasOne(a => a.User)
                .WithMany(u => u.AuthCodes)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(a => a.UserId)
                .HasPrincipalKey(u => u.Id);

            // Application
            builder.HasOne(a => a.Client)
                .WithMany(u => u.AuthCodes)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(a => a.ClientId)
                .HasPrincipalKey(u => u.Id);
        }
    }
}