using DentalSys.Api.Common.Models;
using MediatR;

namespace DentalSys.Api.Features.Patients.CreatePatient
{
    public record Command(
        string FirstName,
        string LastName,
        string MiddleName,
        DateTime DateOfBirth,
        string Gender,
        string Address,
        string PhoneNumber,
        string Email) : IRequest<Result<Guid>>;
}
