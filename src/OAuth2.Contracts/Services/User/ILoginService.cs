﻿using System.Threading.Tasks;
using OAuth2.Models;
using OAuth2.Transfer;

namespace OAuth2.Contracts.Services.User
{
    public interface ILoginService<TUser> where TUser : User<TUser>
    {
        Task<TUser> Login(Login dto);
    }
}