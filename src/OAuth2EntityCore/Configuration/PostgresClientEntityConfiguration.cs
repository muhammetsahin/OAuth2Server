using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OAuth2.Models;

namespace OAuth2EntityCore.Configuration
{
    public class PostgresClientEntityConfiguration<TUser> : IEntityTypeConfiguration<Client<TUser>> 
        where TUser : User<TUser>
    {
        public void Configure(EntityTypeBuilder<Client<TUser>> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .UseHiLo("Client_HiLo");
            
            
            builder.Property(u => u.Id);

            builder.Property(u => u.CreatedAt)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.UpdateAt)
                .IsRequired(false)
                .HasDefaultValue()
                .ValueGeneratedOnAddOrUpdate();

            builder.HasIndex(c => c.Name).IsUnique();

            // Name of the application
            builder.Property(c => c.Name)
                .IsRequired()
                .IsUnicode();

            builder.Property(c => c.Redirects)
                .HasColumnType("text[]")
                .IsRequired();

            builder.Property(c => c.Scopes)
                .HasColumnType("text[]")
                .IsRequired();

            builder.Property(c => c.Secret)
                .IsRequired()
                .IsUnicode();

            builder.Property(c => c.IsFirstParty)
                .IsRequired()
                .HasDefaultValue(false);

            // Checks if client has permission to login on user's behalf using their password
            builder.Property(c => c.IsPasswordClient)
                .HasDefaultValue(false);

            // Public client do not require secret on frontend
            builder.Property(c => c.IsPublicClient)
                .HasDefaultValue(false);

            builder.Property(c => c.Revoked)
                .HasDefaultValue(false);

            // Auth Codes -> Codes that needs to be authenticated
            builder.HasMany(c => c.AuthCodes)
                .WithOne(a => a.Client)
                .IsRequired(false)
                .HasForeignKey(a => a.ClientId)
                .HasPrincipalKey(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);


            // Owner of the Client
            builder.HasOne(c => c.User)
                .WithMany(u => u.Clients)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(c => c.UserId)
                .HasPrincipalKey(u => u.Id);

            // Users who authorized the client
            builder.HasMany(c => c.AuthorizedUsers)
                .WithOne(a => a.Client)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(a => a.ClientId)
                .HasPrincipalKey(c => c.Id);
        }
    }
}