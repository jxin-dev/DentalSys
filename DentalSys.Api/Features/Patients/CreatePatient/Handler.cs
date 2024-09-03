using DentalSys.Api.Common.Models;
using FluentValidation;
using MediatR;

namespace DentalSys.Api.Features.Patients.CreatePatient
{
    public class Handler : IRequestHandler<Command, Result<Guid>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IValidator<Command> _validator;

        public Handler(IPatientRepository patientRepository, IValidator<Command> validator)
        {
            _patientRepository = patientRepository;
            _validator = validator;
        }

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result.Failure<Guid>(PatientErrors.Validation(validationResult.Errors.First().ErrorMessage));
            }

            var patient = new Patient()
            {
                PatientId = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Address = request.Address,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            await _patientRepository.CreateAsync(patient);
       
            return Result.Success(patient.PatientId);
        }
    }
}
