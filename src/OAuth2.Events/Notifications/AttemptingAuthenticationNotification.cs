using OAuth2.Models;
using OAuth2.Transfer;
using MediatR;

namespace OAuth2.Events.Notifications
{
    public class AttemptingAuthenticationNotification<TUser> : INotification
        where TUser : User<TUser>
    {
        public Login Data { get; set; }
        public Client<TUser> Client { get; set; }
    }
}