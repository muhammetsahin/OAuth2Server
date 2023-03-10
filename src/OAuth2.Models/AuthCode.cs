using System;
using System.Collections.Generic;
using NodaTime;

namespace OAuth2.Models
{
    public class AuthCode<TUser> where TUser : User<TUser>
    {
        public Guid Id { get; set; }
        public Instant CreatedAt { get; set; }
        
        public Instant? UpdateAt { get; set; }

        public Instant? ExpiresAt { get; set; }

        public List<string> Scopes { get; set; }
        
        public string Code { get; set; }
        
        public long UserId { get; set; }
        
        public virtual TUser User { get; set; }
        
        public long ClientId { get; set; }
        
        public virtual Client<TUser> Client { get; set; }
    }
}