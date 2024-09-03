using Carter;
using DentalSys.Api.Common.Models;
using DentalSys.Api.Contracts.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DentalSys.Api.Features.Identity.LoginUser
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("users/login", async ([FromBody] LoginUserRequest request, ISender sender) =>
            {
                var command = new Command(request.Username, request.Password);
                var result = await sender.Send(command);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => new CustomProblemResult(error));
            })
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .WithTags(Tags.Identity)
                .MapToApiVersion(1);
        }
    }
}
