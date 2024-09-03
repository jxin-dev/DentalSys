using FluentValidation;

namespace DentalSys.Api.Features.Identity.LoginUser
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("You must provide a username.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("You must provide a password.");
        }
    }
}
