using System.Collections.Generic;
using System.Threading.Tasks;
using OAuth2.Models;

namespace OAuth2.Contracts.Services.Client
{
    public interface IClientService<TUser> where TUser : User<TUser>
    {
        Task<Client<TUser>> Get(long id);
        Task<List<Client<TUser>>> Get(int page, int perPage);
        Task<(Client<TUser> client, string clientId, string clientSecret)>  Create(Transfer.Client dto, TUser user, bool isFirstParty);
    }
}