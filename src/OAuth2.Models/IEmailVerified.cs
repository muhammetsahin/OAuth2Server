using NodaTime;

namespace OAuth2.Models
{
    public interface IEmailVerified
    {
        Instant? GetEmailVerification();
        bool IsEmailVerified();
        bool ShouldVerifyEmail();
    }
}