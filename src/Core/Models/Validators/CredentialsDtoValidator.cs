using FluentValidation;
using Template.Core.Models.Dtos;

namespace Template.Core.Models.Validators
{
    public class CredentialsDtoValidator : AbstractValidator<CredentialsDto>
    {
        public CredentialsDtoValidator()
        {
            this.RuleFor(m => m.UserName)
                .NotEmpty()
                .EmailAddress();

            this.RuleFor(m => m.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}
