using OAuth2.Models;
using MediatR;

namespace OAuth2.Events.Notifications
{
    public class InvalidUserCredentialsNotification<TUser> : INotification
        where TUser : User<TUser>
    {
        public TUser User { get; set; }
    }
}