using System.Threading.Tasks;
using OAuth2.Models;

namespace OAuth2.Contracts.Services.Client
{
    public interface ICreateClient<TUser> where TUser : User<TUser>
    {
        public Task<Client<TUser>> Create(Client<TUser> client);
    }
}