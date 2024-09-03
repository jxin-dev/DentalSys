using Carter;
using DentalSys.Api.Common.Models;
using DentalSys.Api.Contracts.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DentalSys.Api.Features.Identity.RegisterUser
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("users/register", async ([FromBody] RegisterUserRequest request, ISender sender) =>
            {
                var command = new Command(
                    request.FirstName, request.LastName,
                    request.Username, request.Password,
                    request.Role, request.Email, request.PhoneNumber);

                var result = await sender.Send(command);
                return result.Match(
                    onSuccess: () => Results.Created("users/register", result.Value),
                    onFailure: error => new CustomProblemResult(error));
            })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status409Conflict)
                .Produces(StatusCodes.Status400BadRequest)
                .WithTags(Tags.Identity)
                .MapToApiVersion(1);
        }
    }
}
