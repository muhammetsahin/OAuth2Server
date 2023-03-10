using Bogus;

namespace OAuth2Server.Tests.Factories
{
    public interface IFactory<TModel> where TModel : class
    
    {
        Faker<TModel> Create();    
    }
}