using Carter;
using DentalSys.Api.Common.Models;
using DentalSys.Api.Contracts.Request;
using MediatR;

namespace DentalSys.Api.Features.Patients.CreatePatient
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("patients", async (CreatePatientRequest request, ISender sender) =>
            {
                var command = new Command(
                    request.FirstName,
                    request.LastName,
                    request.MiddleName,
                    request.DateOfBirth,
                    request.Gender,
                    request.Address,
                    request.PhoneNumber,
                    request.Email);

                var result = await sender.Send(command);

                return result.Match(
                    onSuccess: () => Results.CreatedAtRoute("GetPatient", new { patientId = result.Value }),
                    onFailure: error => new CustomProblemResult(error));

            })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .RequireAuthorization("Patient.Create")
                .WithTags(Tags.Patient)
                .MapToApiVersion(1);
        }
    }
}
