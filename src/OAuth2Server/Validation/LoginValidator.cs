using OAuth2.Transfer;
using FluentValidation;

namespace OAuth2Server.Validation
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .MaximumLength(200)
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(30);

            RuleFor(x => x.ClientId)
                .NotEmpty()
                .NotNull();            
            RuleFor(x => x.ClientSecret)
                .NotEmpty()
                .NotNull();
        }
    }
}