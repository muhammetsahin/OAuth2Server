using System;

namespace OAuth2Server.Exceptions
{
    public class EmailIsNotVerifiedException : Exception
    {
        public EmailIsNotVerifiedException()
            : base ("Email is not verified")
        {
        }
        public EmailIsNotVerifiedException(string message)
            : base (message)
        {
        }
    }
}