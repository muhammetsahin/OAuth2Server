using System.Collections.Generic;
using System.Threading.Tasks;
using OAuth2.Models;

namespace OAuth2.Contracts.Services.Client
{
    public interface IGetClient<TUser> where TUser : User<TUser>
    {
        public Task<List<Client<TUser>>> Get(int page, int perPage);
        public Task<Client<TUser>> Get(long id);
        public Task<Client<TUser>> Get(string id);
    }
}