using DentalSys.Api.Common.Models;
using DentalSys.Api.Contracts.Response;
using MediatR;

namespace DentalSys.Api.Features.Patients.GetPatient
{
    public record Query(string PatientId) : IRequest<Result<PatientResponse>>;
}
