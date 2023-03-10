namespace OAuth2.Contracts.Providers
{
    public interface IPasswordProvider
    {
        string Hash(string password);
        bool Verify(string password, string hash);
        bool NeedsRehash(string hash);
    }
}