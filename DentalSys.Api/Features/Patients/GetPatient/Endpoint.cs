using Carter;
using DentalSys.Api.Common.Models;
using MediatR;

namespace DentalSys.Api.Features.Patients.GetPatient
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("patients/{patientId}", async (string patientId, ISender sender) =>
            {
                var query = new Query(patientId);
                var result = await sender.Send(query);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => new CustomProblemResult(error));
            })
                .WithName("GetPatient")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status403Forbidden)
                .RequireAuthorization("Patient.View")
                .WithTags(Tags.Patient)
                .MapToApiVersion(1);
        }
    }
}
