using OAuth2.Contracts.Providers;
using Sodium;

namespace OAuth2Server.Providers
{
    public enum HashStrength 
    {
        Interactive,
        Medium,
        Moderate,
        Sensitive,
    }
    
    
    public class DefaultPasswordProvider : IPasswordProvider
    {
        private readonly HashStrength _strength;

        public DefaultPasswordProvider(HashStrength strength)
        {
            _strength = strength;
        }
        
        public string Hash(string password)
        {
            return PasswordHash.ArgonHashString(password, (PasswordHash.StrengthArgon) _strength);
        }

        public bool Verify(string password, string hash)
        {
            return PasswordHash.ArgonHashStringVerify(hash, password);
        }

        public bool NeedsRehash(string hash)
        {
            return PasswordHash.ArgonPasswordNeedsRehash(hash, (PasswordHash.StrengthArgon) _strength);
        }
    }
}