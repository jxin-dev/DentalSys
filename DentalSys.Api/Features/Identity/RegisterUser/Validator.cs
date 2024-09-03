using FluentValidation;

namespace DentalSys.Api.Features.Identity.RegisterUser
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(6)
                .Matches(@"^\S*$").WithMessage("Username cannot contain spaces.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.PhoneNumber).Matches("^09\\d{9}$").WithMessage("Phone number must start with 09 and be exactly 11 digits long.");
        }
    }
}
