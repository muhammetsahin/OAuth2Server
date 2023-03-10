using System;

namespace OAuth2Server.Exceptions
{
    public class ClientSecretInvalidException : Exception
    {
        public ClientSecretInvalidException()
            : base("Client secret is invalid")
        {
        }

        public ClientSecretInvalidException(string message)
            : base(message)
        {
        }
    }
}