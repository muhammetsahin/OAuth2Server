using System.Collections.Generic;

namespace OAuth2.Transfer
{
    public class Client
    {
        public string Name { get; set; }
        public List<string> Redirects { get; set; }
        public List<string> Scopes { get; set; }
        public bool IsPasswordClient { get; set; }
        public bool IsPublicClient { get; set; }
        public bool IsFirstPartyClient { get; set; }
    }
}