namespace OAuth2.Transfer
{
    public interface IAuthorize
    {
        public string State { get; set; }
        public string ClientId { get; set; }
        public string CodeChallenge { get; set; }
        public string ResponseType { get; set; }
        public string Scopes { get; set; }
        public string RedirectUrl { get; set; }
    }
}