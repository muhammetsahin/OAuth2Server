using System.Threading.Tasks;
using OAuth2.Models;
using OAuth2.Transfer;

namespace OAuth2.Contracts.Services.User
{
    public interface ICreateUser<TUser> where TUser : User<TUser>
    {
        public Task<TUser> Create<TRegister>(TRegister register) where TRegister : Register;
    }
}