using System.Threading;
using System.Threading.Tasks;
using OAuth2.Models;
using MediatR;
using OAuth2.Events.Notifications;

namespace OAuth2.Events.Handlers
{
    public class SendVerificationEmailHandler<TUser> : INotificationHandler<UserRegistered<TUser>>
        where TUser : User<TUser>
    {
        public Task Handle(UserRegistered<TUser> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}