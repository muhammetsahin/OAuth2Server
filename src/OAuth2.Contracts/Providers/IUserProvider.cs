using OAuth2.Models;

namespace OAuth2.Contracts.Providers
{
    public interface IUserProvider<TUser> : IProvider<string, TUser> where TUser : User<TUser>
    {
    }
}