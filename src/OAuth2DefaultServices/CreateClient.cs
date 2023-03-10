using System.Threading.Tasks;
using OAuth2.Contracts.Services.Client;
using OAuth2.Models;
using OAuth2EntityCore;

namespace OAuth2DefaultServices
{
    public class CreateClient<TUser> : ICreateClient<TUser> where TUser : User<TUser>
    {
        private readonly IOAuth2DbContext<TUser> _context;

        public CreateClient(IOAuth2DbContext<TUser> context)
        {
            _context = context;
        }


        public async Task<Client<TUser>> Create(Client<TUser> client)
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
            return client;
        }
    }
}