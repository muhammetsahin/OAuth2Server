using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OAuth2.Contracts.Providers;
using OAuth2.Contracts.Services.AuthCode;
using OAuth2.Contracts.Services.Client;
using OAuth2.Contracts.Services.User;
using OAuth2.Models;
using OAuth2EntityCore;

namespace OAuth2DefaultServices
{
    public static class ServiceProvider
    {
        public static IServiceCollection AddOAuth2DefaultServices<TUser, TDbContext>(this IServiceCollection services)
            where TUser : User<TUser>, new() where TDbContext : DbContext, IOAuth2DbContext<TUser> => services
            .AddScoped<IProvider<ClaimsPrincipal, TUser>, GetUser<TUser>>()
            .AddScoped<IGetUser<TUser>, GetUser<TUser>>()
            .AddScoped<IOAuth2DbContext<TUser>, TDbContext>()
            .AddScoped<ICreateUser<TUser>, CreateUser<TUser>>()
            .AddScoped<ICreateClient<TUser>, CreateClient<TUser>>()
            .AddScoped<IUserProvider<TUser>, UserProvider<TUser>>()
            .AddScoped<IGetClient<TUser>, GetClientService<TUser>>()
            .AddScoped<IUpdateUser<TUser>, UpdateUser<TUser>>()
            .AddScoped<ICreateAuthCode<TUser>, CreateAuthCode<TUser>>()
            .AddScoped<IDeleteAuthCode<TUser>, DeleteAuthCode<TUser>>();
    }
}