using FluentValidation;

namespace DentalSys.Api.Features.Patients.CreatePatient
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.FirstName).NotEmpty();
        }
    }
}
