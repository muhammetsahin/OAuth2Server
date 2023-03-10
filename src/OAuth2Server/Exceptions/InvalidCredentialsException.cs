using System;

namespace OAuth2Server.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
            : base("User credentials are incorrect")
        {
        }
        
        public InvalidCredentialsException(string message)
            : base(message)
        {
        }
    }
}