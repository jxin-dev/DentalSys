using DentalSys.Api.Common.Models;
using DentalSys.Api.Contracts.Response;
using FluentValidation;
using MediatR;

namespace DentalSys.Api.Features.Patients.GetPatient
{
    public class Handler : IRequestHandler<Query, Result<PatientResponse>>
    {
        private readonly IPatientRepository _patientRepository;

        public Handler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Result<PatientResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            Guid value;
            if (!Guid.TryParse(request.PatientId, out value))
            {
                return Result.Failure<PatientResponse>(PatientErrors.Validation("Parameter 'PatientId' is required and must be a valid GUID."));
            }

            var patient = await _patientRepository.GetAsync(p => p.PatientId == value);
            if (patient is null)
            {
                return Result.Failure<PatientResponse>(PatientErrors.NotFound(value));
            }

            var response = new PatientResponse(
                patient.PatientId,
                patient.FirstName,
                patient.LastName,
                patient.MiddleName,
                patient.DateOfBirth,
                patient.Gender,
                patient.Address,
                patient.PhoneNumber,
                patient.Email);

            return Result.Success(response);
        }
    }

}
