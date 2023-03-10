using OAuth2.Contracts.Providers;
using OAuth2.Contracts.Services.User;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OAuth2.Contracts.Services.AuthCode;
using OAuth2.Contracts.Services.Client;
using OAuth2.Models;
using OAuth2.Transfer;
using OAuth2Server.Providers;
using OAuth2Server.Services;
using OAuth2Server.Validation;

namespace OAuth2Server
{
    public static class ServiceProvider
    {
        public static IServiceCollection AddOAuth2<TUser, TAuthorize>(this IServiceCollection services)
            where TUser : User<TUser>, new()
            where TAuthorize : class, IAuthorize, new()
            => services
                .AddOAuth2DefaultLoginProvider<TUser>()
                .AddOAuth2DefaultPasswordProvider()
                .AddOAuth2Validations<TAuthorize>()
                .AddOAuth2DefaultUserProvider<TUser>();

        public static IServiceCollection AddOAuth2Validations<TAuthorize>(this IServiceCollection services)
            where TAuthorize : class, IAuthorize, new()
            => services
                .AddSingleton<IValidator<Register>, RegisterValidator>()
                .AddSingleton<IValidator<Login>, LoginValidator>()
                .AddSingleton<IValidator<TAuthorize>, AuthorizeValidator<TAuthorize>>();


        public static IServiceCollection AddOAuth2DefaultPasswordProvider(this IServiceCollection services) =>
            services
                .AddSingleton<IPasswordProvider>(provider => new DefaultPasswordProvider(HashStrength.Interactive));

        public static IServiceCollection AddOAuth2DefaultUserProvider<TUser>(this IServiceCollection services)
            where TUser : User<TUser> =>
            services
                .AddScoped<IClientProvider<TUser>, DefaultClientProvider<TUser>>();

        public static IServiceCollection AddOAuth2DefaultLoginProvider<TUser>(this IServiceCollection services)
            where TUser : User<TUser>, new() =>
            services
                .AddScoped<ILoginService<TUser>, LoginService<TUser>>()
                .AddScoped<IRegisterService<TUser>, RegisterService<TUser>>()
                .AddScoped<IClientService<TUser>, ClientService<TUser>>()
                .AddScoped<IAuthCodeService<TUser>, AuthCodeService<TUser>>();
    }
}