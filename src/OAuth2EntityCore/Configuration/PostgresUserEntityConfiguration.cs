using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OAuth2.Models;

namespace OAuth2EntityCore.Configuration
{
    public class PostgresUserEntityConfiguration<TUser> : IEntityTypeConfiguration<TUser> where TUser : User<TUser>
    {
        public virtual void Configure(EntityTypeBuilder<TUser> builder)
        {
            builder.Property(u => u.Id)
                .UseHiLo("Users_HiLo");

            builder.Property(u => u.CreatedAt)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.UpdateAt)
                .IsRequired(false)
                .HasDefaultValue()
                .ValueGeneratedOnAddOrUpdate();

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .UseHiLo();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Name)
                .IsRequired()
                .IsUnicode();

            builder.Property(u => u.Surname)
                .IsRequired()
                .IsUnicode();

            builder.Property(u => u.Email)
                .IsRequired()
                .IsUnicode();

            builder.Property(u => u.Password)
                .IsRequired()
                .IsUnicode();

            builder.Property(u => u.EmailVerifier)
                .IsRequired(false)
                .HasDefaultValue();
        }
    }
}