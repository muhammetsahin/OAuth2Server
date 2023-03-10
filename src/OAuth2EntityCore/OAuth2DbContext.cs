using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OAuth2.Models;

namespace OAuth2EntityCore
{
    public interface IOAuth2DbContext<TUser> where TUser : User<TUser>
    {
        public DbSet<Client<TUser>> Clients { get; set; }
        public DbSet<AuthCode<TUser>> AuthCodes { get; set; }
        public DbSet<Authorized<TUser>> Authorized { get; set; }
        public DbSet<TUser> Users { get; set; }

        public int SaveChanges();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}