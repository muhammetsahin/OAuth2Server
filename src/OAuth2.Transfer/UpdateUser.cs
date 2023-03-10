using NodaTime;

namespace OAuth2.Transfer
{
    public class UpdateUser
    {
        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public Instant? EmailVerifier { get; set; }
    }
}