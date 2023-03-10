using System.Threading.Tasks;

namespace OAuth2.Contracts.Providers
{
    public interface IProvider<TType, TModel> where TModel : class
    {
        Task<TModel> Fetch(TType userNameOrEmail);
    }
}