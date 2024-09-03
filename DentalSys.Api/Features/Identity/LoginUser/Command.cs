using DentalSys.Api.Common.Models;
using DentalSys.Api.Contracts.Response;
using MediatR;

namespace DentalSys.Api.Features.Identity.LoginUser
{
    public record Command(string Username, string Password) : IRequest<Result<TokenResponse>>;
}
