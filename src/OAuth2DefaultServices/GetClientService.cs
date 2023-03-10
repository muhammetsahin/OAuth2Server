using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OAuth2.Contracts.Services.Client;
using OAuth2.Models;
using OAuth2EntityCore;

namespace OAuth2DefaultServices
{
    public class GetClientService<TUser> : IGetClient<TUser> where TUser : User<TUser>
    {
        private readonly IOAuth2DbContext<TUser> _context;

        public GetClientService(IOAuth2DbContext<TUser> context)
        {
            _context = context;
        }

        public Task<List<Client<TUser>>> Get(int page, int perPage) =>
            _context.Clients 
                .Take(perPage)
                .Skip((page - 1) * perPage)
                .ToListAsync();

        public Task<Client<TUser>> Get(long id) => _context.Clients.FindAsync(id).AsTask();

        public Task<Client<TUser>> Get(string id) => _context.Clients.Where(c => c.ClientId == id).SingleAsync();
    }
}