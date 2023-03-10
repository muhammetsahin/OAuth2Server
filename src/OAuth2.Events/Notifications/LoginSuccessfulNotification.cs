using OAuth2.Models;
using MediatR;

namespace OAuth2.Events.Notifications
{
    public class LoginSuccessfulNotification<TUser> : INotification
        where TUser : User<TUser>
    {
        public Client<TUser> Client { get; set; }
        public TUser User { get; set; }
    }
}