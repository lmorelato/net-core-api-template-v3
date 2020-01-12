using FluentValidation;
using Template.Core.Models.Dtos;

namespace Template.Core.Models.Validators
{
    public sealed class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            this.RuleFor(m => m.Id)
                .GreaterThan(0);

            this.RuleFor(m => m.UserName)
                .NotEmpty()
                .EmailAddress();

            this.RuleFor(m => m.FullName)
                .NotEmpty()
                .MinimumLength(2);
        }
    }
}
