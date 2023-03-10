using System;
using System.Collections.Generic;
using NodaTime;

namespace OAuth2.Models
{
    public class Client<TUser> where TUser : User<TUser>
    {
        public long Id { get; set; }

        public string Name { get; set; }
        
        public string ClientId { get; set; }
        public string Secret { get; set; }
        
        public bool IsPublicClient { get; set; }
        
        public bool IsPasswordClient { get; set; }
        
        public bool IsFirstParty { get; set; }
        
        public bool Revoked { get; set; }
        
        public List<string> Redirects { get; set; }
        
        public List<string> Scopes { get; set; }
        
        public Instant CreatedAt { get; set; }
        
        public Instant? UpdateAt { get; set; }
        
        public long UserId { get; set; }
        
         public virtual TUser User { get; set; }
        
        public virtual ICollection<AuthCode<TUser>> AuthCodes { get; set; }
        
        public virtual ICollection<Authorized<TUser>> AuthorizedUsers { get; set; }
    }
    
}