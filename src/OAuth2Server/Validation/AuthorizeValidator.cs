using System;
using System.Text.RegularExpressions;
using FluentValidation;
using OAuth2.Transfer;

namespace OAuth2Server.Validation
{
    public class AuthorizeValidator<TAuthorize> : AbstractValidator<TAuthorize>
        where TAuthorize : class, IAuthorize, new()
    {
        public AuthorizeValidator()
        {
            RuleFor(a => a.ClientId)
                .NotEmpty()
                .WithMessage("client_id is required")
                .MinimumLength(16)
                .WithMessage("client_id has to be longer than {MinimumLengthValidator} characters")
                .MaximumLength(64)
                .WithMessage("client_id cannot be longer than {MaximumLengthValidator} characters")
                .Matches(new Regex("^[a-zA-Z0-9_-]$"))
                .WithMessage("client_id has to be base64url encoded");

            RuleFor(a => a.RedirectUrl)
                .NotEmpty()
                .WithMessage("redirect_url is required")
                .MaximumLength(255)
                .WithMessage("redirect_url cannot be longer than {MaximumLengthValidator} characters")
                .Must(value =>
                {
                    if (value == null)
                    {
                        return false;
                    }

                    try
                    {
                        var _ = new Uri(value);
                        return true;
                    }
                    catch (UriFormatException)
                    {
                        return false;
                    }
                })
                .WithMessage("redirect_url must be a valid url");
        }
    }
}