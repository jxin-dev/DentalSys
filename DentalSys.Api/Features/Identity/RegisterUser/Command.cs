using DentalSys.Api.Common.Models;
using DentalSys.Api.Contracts.Response;
using MediatR;

namespace DentalSys.Api.Features.Identity.RegisterUser
{
    public record Command(
        string FirstName,
        string LastName,
        string Username,
        string Password,
        string Role,
        string Email,
        string PhoneNumber) : IRequest<Result<RegisterUserResponse>>;
}
