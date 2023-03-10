using System.Threading;
using System.Threading.Tasks;
using OAuth2.Models;
using MediatR;
using OAuth2.Events.Notifications;

namespace OAuth2.Events.Handlers
{
    public class SuccessfulLoginHandler<TUser> : INotificationHandler<LoginSuccessfulNotification<TUser>>
        where TUser : User<TUser>
    {
        public Task Handle(LoginSuccessfulNotification<TUser> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}