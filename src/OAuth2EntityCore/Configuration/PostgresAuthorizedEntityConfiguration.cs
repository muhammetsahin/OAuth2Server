using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OAuth2.Models;

namespace OAuth2EntityCore.Configuration
{
    public class PostgresAuthorizedEntityConfiguration<TUser> : IEntityTypeConfiguration<Authorized<TUser>>
        where TUser : User<TUser>
    {
        public void Configure(EntityTypeBuilder<Authorized<TUser>> builder)
        {
            builder.ToTable("authorized");
            
            
            builder.Property(u => u.Id)
                .UseHiLo("Authorized_HiLo");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id);

            builder.Property(u => u.CreatedAt)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.UpdateAt)
                .IsRequired(false)
                .HasDefaultValue()
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(a => a.Scopes)
                .IsRequired()
                .HasColumnType("text[]");
            
            // User who triggered login
            builder.HasOne(a => a.User)
                .WithMany(u => u.AuthorizedClients)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(a => a.UserId)
                .HasPrincipalKey(u => u.Id);

            builder.HasOne(a => a.Client)
                .WithMany(u => u.AuthorizedUsers)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(a => a.UserId)
                .HasPrincipalKey(u => u.Id);
        }
    }
}