using System.Text.RegularExpressions;
using FluentValidation;
using OAuth2.Transfer;

namespace OAuth2Server.Validation
{
    public class RegisterValidator : AbstractValidator<Register>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(30)
                .Matches(new Regex("^[A-Z][a-z]+$"))
                .WithMessage("{PropertyName} must start with uppercase letter");
            RuleFor(x => x.Surname)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(30)
                .Matches(new Regex("^[A-Z][a-z]+$"))
                .WithMessage("{PropertyName} must start with uppercase letter");
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
        }
    }
}