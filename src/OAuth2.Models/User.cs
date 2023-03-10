using System.Collections.Generic;
using NodaTime;

namespace OAuth2.Models
{
    public abstract class User<TUser> : IEmailVerified where TUser : User<TUser>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Instant? EmailVerifier { get; set; }
        public Instant CreatedAt { get; set; }
        public Instant? UpdateAt { get; set; }

        public virtual ICollection<Client<TUser>> Clients { get; set; }

        public virtual ICollection<Authorized<TUser>> AuthorizedClients { get; set; }

        public virtual ICollection<AuthCode<TUser>> AuthCodes { get; set; }

        public virtual Instant? GetEmailVerification()
        {
            return EmailVerifier;
        }

        public virtual bool IsEmailVerified()
        {
            return EmailVerifier != null;
        }

        public virtual bool ShouldVerifyEmail()
        {
            return false;
        }
    }
}